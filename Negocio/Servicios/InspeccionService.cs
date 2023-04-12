using Datos;
using Dominio.Entidades;
using Dominio.Enumeraciones;
using InfraestructuraTransversal;
using Negocio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Negocio.Servicios
{
    public class InspeccionService : IInspeccionService
    {
        private DatosContexto _contexto = new DatosContexto();


        //1 -   Check
        public OrdenDeProduccion BuscarOPActiva()  
        {
            try
            {
                var supervisor = _contexto.Empleados.FirstOrDefault(e => e.Id.Equals(Cache.Instance.ObtenerEmpleadoID()));
                var ordenesActivas = _contexto.OrdenesDeProduccionActivas();
                var ultimaJornadaDelSupervisor = _contexto.UltimaJornadaActivaDelSupervisor(supervisor);  
                if (ultimaJornadaDelSupervisor != null)
                {
                    foreach (var orden in ordenesActivas)
                    {
                        if (orden.JornadaActual() == ultimaJornadaDelSupervisor)
                        {
                            Cache.Instance.GuardarOrdenID(orden.Id);                            
                            return orden;
                        }
                    }
                }
            }
            catch {  }
            return null;
        }
        //2 -   Check
        public List<LineaDeProduccion> ObtenerLineasDisponibles()
        {
            if (Cache.Instance.ObtenerOrdenID() != 0)   //Si hay una orden ni busque las líneas
                return new List<LineaDeProduccion>();
            return _contexto
                .LineasDisponiblesParaInspeccion();
        }
        //3-   Check
        public bool NecesitaDesasociarse()
        {
            if (Cache.Instance.ObtenerOrdenID() != 0)
            {
                Turno turnoActual = ObtenerTurnoActual();
                var ultimaJornada = _contexto.OrdenesDeProduccionActivas()
                    .FirstOrDefault(op => op.Id.Equals(Cache.Instance.ObtenerOrdenID()))
                    .JornadaActual();
                if (turnoActual != null && ultimaJornada != null)
                    return turnoActual != ultimaJornada.Turno;
            }                
            return false;
        }
        //4 -   Check
        public List<Tuple<int, int>> ObtenerTotalesDeIncidenciasPorHora()
        {
            if (Cache.Instance.ObtenerOrdenID() != 0)
            {
                var orden = _contexto
                    .OrdenesDeProduccionActivas()
                    .FirstOrDefault(op => op.Id
                    .Equals(Cache.Instance.ObtenerOrdenID()));
                return orden.JornadaActual().TotalIncidenciasPorHora();
            }
            return new List<Tuple<int, int>>();
        }
        //5 -   Check
        public List<Defecto> ObtenerListadoDefectos()
        {
            if (Cache.Instance.ObtenerOrdenID() == 0)
                return new List<Defecto>();
            return _contexto
                .Defectos
                .ToList();
        }
        //6 -   Check
        public void AsociarseAOrdenDeProduccion(int numeroLinea)
        {
            var turno = ObtenerTurnoActual();
            if (turno!=null && SigueDisponibleLaLinea(numeroLinea))
            {
                var op = _contexto.OrdenesDeProduccionActivas()
                    .FirstOrDefault(op => op.Linea.Numero
                    .Equals(numeroLinea));
                var supervisor = _contexto.Empleados
                    .FirstOrDefault(e => e.Id
                    .Equals(Cache.Instance.ObtenerEmpleadoID()));
                var jornadaNueva = op.CrearJornadaNueva(supervisor, turno);  //Patrón creador
                op.Jornadas.Add(jornadaNueva);
                _contexto.Update(op);
                _contexto.SaveChanges();
            }            
        }
        //7 -   Check
        public void DesasociarseDeOrdenDeProduccion()
        {
            if(SigueIniciadaLaOP() is true) //Sino ya fue desasociado xq al pausar o finalizar una OP las jornadas acaban
            {
                var orden = _contexto.OrdenesDeProduccionActivas()
                    .FirstOrDefault(op => op.Id
                    .Equals(Cache.Instance.ObtenerOrdenID()));
                orden.JornadaActual().FinalizarJornada();   //Patrón experto
                Cache.Instance.BorrarOPId();
                _contexto.Update(orden);
                _contexto.SaveChanges();
            }
        }
        //8 -   Check
        public void AgregarIncidencia(int? horaElegida, string descripcionDefecto, int numeroPie)
        {
            if (NecesitaDesasociarse() is false && SigueIniciadaLaOP() is true
                && horaElegida!=null)
            {
                try
                {
                    var orden = _contexto.OrdenesDeProduccionActivas()
                        .FirstOrDefault(op => op.Id
                        .Equals(Cache.Instance.ObtenerOrdenID()));
                    var defecto = _contexto.Defectos
                        .FirstOrDefault(d => d.Descripcion
                        .Equals(descripcionDefecto));
                    var incidencia = orden  //Patrón creador
                        .JornadaActual()
                        .CrearIncidenciaNueva((int)horaElegida, defecto, numeroPie);
                    orden.JornadaActual().Incidencias.Add(incidencia); //Patrón experto
                    if(defecto!=null)
                        GenerarAlertas(defecto);
                    _contexto.Update(orden);
                    _contexto.SaveChanges();
                }
                catch { }
            }
        }
        //9 -   Check
        public void EliminarIncidencia(int? horaElegida, string descripcionDefecto, int numeroPie)
        {
            if (NecesitaDesasociarse() is false && SigueIniciadaLaOP() is true
                && horaElegida != null)
            {
                try
                {
                    var orden = _contexto.OrdenesDeProduccionActivas()
                        .FirstOrDefault(op => op.Id
                        .Equals(Cache.Instance.ObtenerOrdenID()));
                    Incidencia borrar = orden.JornadaActual()
                        .BuscarIncidencia((int)horaElegida, descripcionDefecto, numeroPie);
                    if(borrar != null)
                    {
                        orden.JornadaActual().Incidencias.Remove(borrar);
                        _contexto.Update(borrar);
                        _contexto.SaveChanges();
                    }
                }
                catch { }
            }
        }



        private Turno ObtenerTurnoActual()
        {
            var horaActual = System.DateTime.Now.Hour + (System.DateTime.Now.Minute * 0.01);
            return _contexto.Turnos.FirstOrDefault(turno =>
            horaActual >= turno.HoraInicio && horaActual <= turno.HoraFin);
        }
        private void GenerarAlertas(Defecto defecto)
        {
            try
            {
                var orden = _contexto.OrdenesDeProduccionActivas()
                    .FirstOrDefault(op => op.Id
                    .Equals(Cache.Instance.ObtenerOrdenID()));
                var alertas = orden
                    .JornadaActual()
                    .AlertasDeLaUltimaJornada(defecto.TipoDeDefecto);
                int totalDeDefectos = 0;
                int limiteSuperior = 0;
                switch (defecto.TipoDeDefecto)
                {
                    case TipoDeDefecto.Observado:
                        limiteSuperior = orden.Modelo.LimiteSuperiorObservado;
                        break;
                    case TipoDeDefecto.Reproceso:
                        limiteSuperior = orden.Modelo.LimiteSuperiorReproceso;
                        break;
                }
                totalDeDefectos = orden
                    .JornadaActual()
                    .TotalIncidencias(defecto.TipoDeDefecto);
                if (totalDeDefectos == limiteSuperior)
                {
                    var alerta = orden.JornadaActual().CrearAlertaNueva(defecto.TipoDeDefecto);  //Patrón creador
                    orden.JornadaActual().Alertas.Add(alerta);
                }
                _contexto.Update(orden);
                _contexto.SaveChanges();
            }
            catch { }            
        }
        private bool SigueDisponibleLaLinea(int numeroLinea)
        {
            return _contexto    
                .LineasDisponiblesParaInspeccion()
                .FirstOrDefault(l => l.Numero == numeroLinea)
                != null;
        }
        private bool SigueIniciadaLaOP()
        {
            return _contexto
                .OrdenesDeProduccionActivas()
                .FirstOrDefault(op => op.Id
                .Equals(Cache.Instance.ObtenerOrdenID()))
                .Estado == EstadoOP.Inciada;

        }
    }
}
