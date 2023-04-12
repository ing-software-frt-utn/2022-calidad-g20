using Dominio.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public class Incidencia : EntidadBase
    {
        public DateTime FechaYHoraDeRegistro { get; set; } = System.DateTime.Now;
        public int HoraDeRegistro { get; set; } = System.DateTime.Now.Hour;
        public Defecto Defecto { get; set; }
        public Pie Pie { get; set; }

        //https://learn.microsoft.com/en-us/ef/core/saving/cascade-delete
        //public JornadaLaboral Jornada { get; set; }
    }
}
