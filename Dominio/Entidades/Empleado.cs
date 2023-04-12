using Dominio.Enumeraciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dominio.Entidades
{
    public class Empleado : EntidadBase
    {
        [Required]
        public string Usuario { get; set; }
        [Required]
        public string Contrasena { get; set; }
        public TipoDeEmpleado TipoDeEmpleado { get; set; }
    }
}
