using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalzadosSPA.Pages.Administrativo;
using Dominio.Entidades;
using Dominio.Enumeraciones;
using InfraestructuraTransversal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Negocio.Contratos;

namespace CalzadosSPA.Pages.Público
{
    [Authorize(Policy = "DebeSerSDC")]
    [BindProperties]
    public class SemaforoModel : PageModel  //LISTO
    {
        private ISemaforoService _servicio;

        public OrdenDeProduccion OrdenDeProduccion { get; set; }
        public Tuple <string, string> Colores { get; set; }


        public SemaforoModel(ISemaforoService servicio)
        {
            _servicio = servicio;
        }

        public void OnGet()
        {
            OrdenDeProduccion = _servicio.BuscarOPActiva();
            Colores = _servicio.ObtenerColores();
        }
    }
}
