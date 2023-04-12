using Dominio.Enumeraciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Dominio.Entidades
{
    public class OrdenDeProduccion : EntidadBase
    {
        public int Numero { get; set; }
        public DateTime FechaYHoraInicio { get; set; } = DateTime.Now;
        public DateTime FechaYHoraFin { get; set; }
        public EstadoOP Estado { get; set; } = EstadoOP.Inciada;
        public LineaDeProduccion Linea { get; set; }
        public Empleado SupervisorDeLinea { get; set; }
        public Modelo Modelo { get; set; }
        public Color Color { get; set; }
        public List<JornadaLaboral> Jornadas { get; set; } = new List<JornadaLaboral>();
        


        #region
        public JornadaLaboral JornadaActual()
        {
            return Jornadas
                .FirstOrDefault(j =>
                j.FechaYHoraFin.Equals(DateTime.MinValue));
        }
        public JornadaLaboral CrearJornadaNueva(Empleado empleado, Turno turno)
        {
            return new JornadaLaboral()
            {
                SupervisorDeCalidad = empleado,
                Turno = turno
            };
        }
        public void ActualizarEstadoDeLaOP(EstadoOP estado)
        {
            Estado = estado;
            if (JornadaActual() != null)
                JornadaActual().FechaYHoraFin = DateTime.Now;
            if(estado.Equals(EstadoOP.Finalizada))
                FechaYHoraFin = DateTime.Now;
        }
        public bool InspeccionDetenida()
        {
            return JornadaActual().UltimaAlertaActiva(TipoDeDefecto.Reproceso) != null
                || JornadaActual().UltimaAlertaActiva(TipoDeDefecto.Observado) != null;
        }
        #endregion
    }
}
