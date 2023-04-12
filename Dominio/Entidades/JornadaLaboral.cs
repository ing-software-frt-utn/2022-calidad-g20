using Dominio.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dominio.Entidades
{
    public class JornadaLaboral : EntidadBase
    {
        public Empleado SupervisorDeCalidad { get; set; }
        public DateTime FechaYHoraInicio { get; set; } = System.DateTime.Now;
        public DateTime FechaYHoraFin { get; set; }
        public Turno Turno { get; set; }
        public List<Alerta> Alertas { get; set; } = new List<Alerta>();
        public List<Incidencia> Incidencias { get; set; } = new List<Incidencia>();



        #region
        public void FinalizarJornada()
        {
            FechaYHoraFin = DateTime.Now;
        }
        public Alerta UltimaAlertaActiva(TipoDeDefecto tipoAlerta)
        {
            return Alertas
                .FirstOrDefault(a => a.FechaReinicio
                .Equals(DateTime.MinValue)
                && a.Tipo.Equals(tipoAlerta));
        }
        public List<Alerta> AlertasDeLaUltimaJornada(TipoDeDefecto tipo)
        {
            return Alertas
                .Where(a => a.Tipo.Equals(tipo))
                .ToList();
        }
        public Incidencia CrearIncidenciaNueva(int hora, Defecto defecto, int numeroPie)
        {
            return new Incidencia()
            {
                HoraDeRegistro = hora,
                Defecto = defecto,
                Pie = (Pie)numeroPie
            };
        }
        public Alerta CrearAlertaNueva(TipoDeDefecto tipo)
        {
            return new Alerta()
            {
                Tipo = tipo
            };
        }
        public Incidencia BuscarIncidencia(int hora, string descripcionDefecto, int numeroPie)
        {
            switch (descripcionDefecto)
            {
                case null:
                    return Incidencias
                        .FirstOrDefault(i => i.HoraDeRegistro == hora
                        && i.Defecto is null);
                default:
                    return Incidencias
                        .FirstOrDefault(i => i.HoraDeRegistro == hora
                        && i.Defecto.Descripcion.Equals(descripcionDefecto)
                        && i.Pie.Equals((Pie)numeroPie));
            }
        }
        public int TotalIncidencias(TipoDeDefecto tipo)
        {
            switch (UltimaAlertaActiva(tipo))
            {
                case null:
                    switch (AlertasDeLaUltimaJornada(tipo).Count())
                    {
                        case 0:
                            return Incidencias
                                .Where(i => i.Defecto != null)
                                .Count(i => i.Defecto.TipoDeDefecto.Equals(tipo));
                        default:
                            var fechaUltimoReinicio = AlertasDeLaUltimaJornada(tipo)
                                .OrderBy(a => a.FechaReinicio != DateTime.MinValue)
                                .Last()
                                .FechaReinicio;
                            return Incidencias
                                .Where(i => i.Defecto != null)
                                .Count(i => i.Defecto.TipoDeDefecto.Equals(tipo)
                                && i.FechaYHoraDeRegistro > fechaUltimoReinicio);
                    }
                default:
                    switch (AlertasDeLaUltimaJornada(tipo).Count())
                    {
                        case 1:
                            return Incidencias
                                .Where(i => i.Defecto != null)
                                .Count(i => i.Defecto.TipoDeDefecto.Equals(tipo));
                        default:
                            var fechaUltimoReinicio = AlertasDeLaUltimaJornada(tipo)
                                .OrderBy(a => a.FechaReinicio != DateTime.MinValue)
                                .Last()
                                .FechaReinicio;
                            return Incidencias
                                .Where(i => i.Defecto != null)
                                .Count(i => i.Defecto.TipoDeDefecto.Equals(tipo)
                                && i.FechaYHoraDeRegistro > fechaUltimoReinicio);
                    }
            }
        }
        public List<Tuple<int, int>> TotalIncidenciasPorHora()    //Por ahora sólo de pares de primera
        {
            List<Tuple<int, int>> listaTotalesHora = new List<Tuple<int, int>>();            
            var incidencias = Incidencias.Where(x => x.Defecto is null);
            var horas = incidencias.Select(i => i.HoraDeRegistro).Distinct();
            foreach (var hora in horas)
            {
                int total = incidencias.Count(i => i.HoraDeRegistro.Equals(hora));
                var tupla = Tuple.Create(hora, total);
                listaTotalesHora.Add(tupla);
            }
            return listaTotalesHora;
        }
        #endregion
    }
}
