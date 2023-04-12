using Dominio.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public class Alerta : EntidadBase
    {
        public TipoDeDefecto Tipo { get; set; }
        public DateTime FechaDisparo { get; set; } = System.DateTime.Now;
        public DateTime FechaReinicio { get; set; }
    }
}
