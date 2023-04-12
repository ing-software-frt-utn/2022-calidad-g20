using Dominio.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public class Defecto : EntidadBase
    {
        public string Descripcion { get; set; }
        public TipoDeDefecto TipoDeDefecto { get; set; }
    }
}
