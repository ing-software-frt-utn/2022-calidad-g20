using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dominio.Entidades;
using Dominio.Enumeraciones;
using InfraestructuraTransversal;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Negocio.Contratos;

namespace CalzadosSPA.Pages.Administrativo
{   
    [BindProperties]
    public class IniciarSesionModel : PageModel
    {
        private ISesionService _servicio;

        public Empleado Empleado { get; set; }


        public IniciarSesionModel(ISesionService servicio)
        {
            _servicio = servicio;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            Empleado = _servicio.AutenticarDatos(Empleado);
            if(Empleado != null)
            {
                List<Claim> claims = new List<Claim>() { new Claim(ClaimTypes.Name,Empleado.Usuario) };
                switch (Empleado.TipoDeEmpleado)
                {
                    case TipoDeEmpleado.Administrativo:
                        claims.Add(new Claim("Empleado", "Administrativo"));
                        break;
                    case TipoDeEmpleado.SupervisorDeLinea:
                        claims.Add(new Claim("Empleado", "SupervisorDeLinea"));
                        break;
                    case TipoDeEmpleado.SupervisorDeCalidad:
                        claims.Add(new Claim("Empleado", "SupervisorDeCalidad"));
                        break;
                }
                var identity = new ClaimsIdentity(claims, "MiCookieDeAutenticacion");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync("MiCookieDeAutenticacion", claimsPrincipal);

                Cache.Instance.GuardarEmpleadoID(Empleado.Id);
                return RedirectToPage("/Index");
            }
            return Page();
        }
    }
}
