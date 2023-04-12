using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public class Turno : EntidadBase
    {
        public string Descripcion { get; set; }
        public double HoraInicio { get; set; }
        public double HoraFin { get; set; }
    }
}
