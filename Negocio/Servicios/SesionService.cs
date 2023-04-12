using Datos;
using Dominio.Entidades;
using Negocio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Negocio.Servicios
{
    public class SesionService : ISesionService
    {
        private DatosContexto _contexto = new DatosContexto();

        public Empleado AutenticarDatos(Empleado empleado)
        {
            foreach (var emp in _contexto.Empleados.ToList())
            {
                if (emp.Usuario.Equals(empleado.Usuario) &&
                    emp.Contrasena.Equals(empleado.Contrasena))
                {
                    return emp;
                }
            }
            return null;
        }
    }
}
