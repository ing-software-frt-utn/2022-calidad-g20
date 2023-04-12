using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Negocio.Contratos;

namespace CalzadosSPA.Pages.SupervisorDeLinea
{
    [BindProperties]
    [Authorize(Policy = "DebeSerAdmininstrativo")]
    public class GestionarCalzadosModel : PageModel
    {
        private IModeloService _servicio;

        public List<Modelo> Modelos { get; set; }
        public int SKUBorrar { get; set; }
        public int SKUActualizar { get; set; }
        //Datos como primitivos para la ALTA
        public int SKU { get; set; }
        public string Denominacion { get; set; }
        public int LimiteInfRepro { get; set; }
        public int LimiteSupRepro { get; set; }
        public int LimiteInfObser { get; set; }
        public int LimiteSupObser { get; set; }



        public GestionarCalzadosModel(IModeloService servicio)
        {
            _servicio = servicio;
        }

        public void OnGet()
        {
            Modelos = _servicio.ObtenerListadoModelos();
        }
        //Check
        public void OnPost()
        {
            _servicio.AgregarModelo(SKU, Denominacion,LimiteInfObser, LimiteSupObser, LimiteInfRepro, LimiteSupRepro);
            OnGet();            
        }
        public void OnPostUpdate()  //TODO: verificar Limites sup > Limites inf
        {
            _servicio.ActualizarModelo(SKUActualizar, SKU, Denominacion, LimiteInfObser, LimiteSupObser, LimiteInfRepro, LimiteSupRepro);
            OnGet();
        }
        //Check
        public void OnPostDelete()  //TODO: PK exception cuando una OP usó un modelo 
        {
           _servicio.EliminarModelo(SKUBorrar);
            OnGet();
           //RedirectToPage("SupervisorDeLinea\\GestionarCalzados");
        }
    }
}
