using Datos;
using Dominio.Entidades;
using Dominio.Enumeraciones;
using InfraestructuraTransversal;
using Microsoft.EntityFrameworkCore;
using Negocio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Negocio.Servicios
{
    public class OrdenDeProduccionService : IOrdenDeProduccionService
    {
        private DatosContexto _contexto = new DatosContexto();

        public OrdenDeProduccionService(){}
        public OrdenDeProduccionService(DatosContexto contexto)
        {
            _contexto = contexto;
        }

        //1 -   Check
        public List<Color> ObtenerListadoColores()
        {
            return _contexto
                .Colores
                .OrderBy(col => col.Descripcion).ToList();
        }
        //2 -   Check
        public List<LineaDeProduccion> ObtenerListadoLineasDisponibles()
        {
            return _contexto
                .LineasDisponiblesParaCrearOP();
        }
        //3 -   Check
        public List<Modelo> ObtenerListadoModelos()
        {
            return _contexto
                .Modelos
                .OrderBy(mod => mod.SKU)
                .ToList();
        }
        //4 -   Check
        public OrdenDeProduccion BuscarOPActiva()
        {
            return _contexto
                .OrdenesDeProduccionSinFinalizar()
                .FirstOrDefault(op => op.SupervisorDeLinea.Id
                .Equals(Cache.Instance.ObtenerEmpleadoID()));
        }
        //5 -   TODO: patrón constructor para crear la orden nueva?
        public void GenerarOP(int numeroOP, int numeroLinea, int SKUModelo, string descripcionColor)    //MOQ library para simular BD en los tests
        {
            if(NumeroDeOPUnico(numeroOP) && LineaSigueDisponible(numeroLinea))
            {
                OrdenDeProduccion orden = new OrdenDeProduccion()
                {
                    Numero = numeroOP,
                    Linea = _contexto.LineasDeTrabajo.FirstOrDefault(linea => linea.Numero.Equals(numeroLinea)),
                    Modelo = _contexto.Modelos.FirstOrDefault(modelo => modelo.SKU.Equals(SKUModelo)),
                    Color = _contexto.Colores.FirstOrDefault(color => color.Descripcion.Equals(descripcionColor)),
                    SupervisorDeLinea = _contexto.Empleados.FirstOrDefault(emp => emp.Id.Equals(Cache.Instance.ObtenerEmpleadoID()))
                };
                _contexto.OrdenesDeProduccion.Add(orden);
                _contexto.SaveChanges();
            }
        }
        //6 -   Check
        public void ActualizarEstadoOP(int estado)
        {
            EstadoOP estadoOP = (EstadoOP)estado;
            var orden = _contexto.OrdenesDeProduccionSinFinalizar()
                .FirstOrDefault(op => op.SupervisorDeLinea.Id
                .Equals(Cache.Instance.ObtenerEmpleadoID()));
            orden.ActualizarEstadoDeLaOP(estadoOP);
            _contexto.OrdenesDeProduccion.Update(orden);
            _contexto.SaveChanges();
        }
        //7 -   Check
        public void ReiniciarSemaforo(int tipoDeSemaforo, string codigoReinicio)
        {
            if (codigoReinicio.Equals("contraseña"))
            {
                var orden = BuscarOPActiva();
                TipoDeDefecto tipo = (TipoDeDefecto)tipoDeSemaforo;
                var alerta = orden.JornadaActual().UltimaAlertaActiva(tipo);
                if (alerta != null)
                {
                    alerta.FechaReinicio = DateTime.Now;
                    _contexto.Update(orden);
                    _contexto.SaveChanges();
                }
            }
        }


        private bool NumeroDeOPUnico(int numero)
        {
            return _contexto
                .OrdenesDeProduccionSinFinalizar()
                .FirstOrDefault(op => op.Numero == numero)
                is null;
        }
        private bool LineaSigueDisponible(int numero)
        {
            var lineasDisponibles = _contexto.LineasDisponiblesParaCrearOP();
            return lineasDisponibles
                .FirstOrDefault(l => l.Numero == numero)
                != null;
        }
    }
}
