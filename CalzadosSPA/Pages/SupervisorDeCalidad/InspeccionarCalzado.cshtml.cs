using System    ;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalzadosSPA.Pages.Administrativo;
using Dominio.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Negocio.Contratos;

namespace CalzadosSPA.Pages.SupervisorDeCalidad
{
    [Authorize(Policy = "DebeSerSDC")]
    [BindProperties]
    public class InspeccionarCalzadoModel : PageModel
    {
        private IInspeccionService _servicio;
        public bool NecesitaDesasociarse { get; set; }

        //Datos de la vista 
        public int NumeroDeLinea { get; set; }
        public OrdenDeProduccion OrdenDeProduccion { get; set; }
        public List<Defecto> Defectos { get; set; } //= new List<Defecto>();
        public List<LineaDeProduccion> LineasDisponibles { get; set; }// = new List<LineaDeProduccion>();
        public List<Tuple<int, int>> TotalesPorHora { get; set; } = new List<Tuple<int, int>>();

        //Incidencia como primitivos
        public int? HoraSeleccionada { get; set; }
        public string DefectoSeleccionado { get; set; }
        public int PieSeleccionado { get; set; } = 0;


        public InspeccionarCalzadoModel(IInspeccionService servicio)
        {
            _servicio = servicio;

        }

        public void OnGet()
        {
            OrdenDeProduccion = _servicio.BuscarOPActiva(); 
            LineasDisponibles = _servicio.ObtenerLineasDisponibles();
            Defectos = _servicio.ObtenerListadoDefectos();
            NecesitaDesasociarse = _servicio.NecesitaDesasociarse();
            TotalesPorHora = _servicio.ObtenerTotalesDeIncidenciasPorHora();
        }

        public void OnPostAsociar() //Verificado
        {
            _servicio.AsociarseAOrdenDeProduccion(NumeroDeLinea);
            OnGet();
        }       

        public void OnPostDesasociar()  //Verificado
        {
            _servicio.DesasociarseDeOrdenDeProduccion();
            OnGet();
        }

        public void OnPostRegistrarIncidencia()     //Verificado
        {
            _servicio.AgregarIncidencia(HoraSeleccionada, DefectoSeleccionado, PieSeleccionado);
            OnGet();
        }  

        public void OnPostEliminarIncidencia()  //Verificado 
        {
            _servicio.EliminarIncidencia(HoraSeleccionada, DefectoSeleccionado, PieSeleccionado);
            OnGet();
        }
    }
}
