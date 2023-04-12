using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidades
{
    public class Modelo : EntidadBase
    {
        public int SKU { get; set; }
        public string Denominacion { get; set; }
        public int LimiteInferiorReproceso { get; set; }
        public int LimiteSuperiorReproceso { get; set; }
        public int LimiteInferiorObservado { get; set; }
        public int LimiteSuperiorObservado { get; set; }
    }
}
