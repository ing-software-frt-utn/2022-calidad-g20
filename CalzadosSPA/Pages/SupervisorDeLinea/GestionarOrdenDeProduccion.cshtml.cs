using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Enumeraciones;
using Dominio.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Negocio.Contratos;
using CalzadosSPA.Pages.Administrativo;

namespace CalzadosSPA.Pages.SupervisorDeLinea
{
    [BindProperties]
    [Authorize(Policy = "DebeSerSDL")]
    public class GestionarOrdenDeProduccionModel : PageModel    //TODO: sólo permitir reiniciar el semáforo en algunos casos, retornos a la vista como error, datos no nulos OP
    {
        private IOrdenDeProduccionService _servicio;

        //Datos de la vista
        public List<LineaDeProduccion> Lineas;
        public List<Modelo> Modelos;
        public List<Color> Colores;
        public List<Empleado> SupervisoresDeLinea;
        public OrdenDeProduccion OrdenDeProduccion { get; set; }
        public int EstadoOP { get; set; }

        //Datos de la orden como primitivos
        public int NumeroOP { get; set; }
        public int NumeroLinea { get; set; }
        public int SKUModelo { get; set; }
        public string DescripcionColor { get; set; }

        //Semáforo
        public int TipoDeSemaforo { get; set; } = 0;
        public string CodigoReinicio { get; set; }


        public GestionarOrdenDeProduccionModel(IOrdenDeProduccionService servicio)
        {
            _servicio = servicio;
        }

        public void OnGet()
        {         
            OrdenDeProduccion = _servicio.BuscarOPActiva();
            Lineas = _servicio.ObtenerListadoLineasDisponibles();
            Modelos = _servicio.ObtenerListadoModelos();    
            Colores = _servicio.ObtenerListadoColores();
        }

        public void OnPost() 
        {
            _servicio.GenerarOP(NumeroOP, NumeroLinea,SKUModelo, DescripcionColor);
            OnGet();
        }

        public void OnPostUpdateEstadoOP()
        {
            _servicio.ActualizarEstadoOP(EstadoOP);
            OnGet();
        }

        public void OnPostReiniciarSemaforo()
        {
            _servicio.ReiniciarSemaforo(TipoDeSemaforo,CodigoReinicio);
            OnGet();
        }
    }
}