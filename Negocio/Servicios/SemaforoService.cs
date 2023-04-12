using Datos;
using Dominio.Entidades;
using Dominio.Enumeraciones;
using InfraestructuraTransversal;
using Microsoft.EntityFrameworkCore;
using Negocio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Negocio.Servicios
{
    public class SemaforoService : ISemaforoService
    {
        private InspeccionService _inspeccionService = new InspeccionService();

        public OrdenDeProduccion BuscarOPActiva()
        {
            return _inspeccionService.BuscarOPActiva();
        }       
        public Tuple<string, string> ObtenerColores()
        {
            string color1 = "green";
            string color2 = "green";
            if(Cache.Instance.ObtenerOrdenID() != 0)
            {
                var orden = BuscarOPActiva();
                int totalRepro = orden.JornadaActual().TotalIncidencias(TipoDeDefecto.Reproceso);
                int totalObser = orden.JornadaActual().TotalIncidencias(TipoDeDefecto.Observado);  
                //Semáforo de reproceso
                if (totalRepro > orden.Modelo.LimiteInferiorReproceso 
                    && totalRepro < orden.Modelo.LimiteSuperiorReproceso)
                    color1 = "yellow";
                else if (totalRepro >= orden.Modelo.LimiteSuperiorReproceso)
                    color1 = "red";
                //Semáforo de observados
                if (totalObser > orden.Modelo.LimiteInferiorObservado
                    && totalObser < orden.Modelo.LimiteSuperiorObservado)
                    color2 = "yellow";
                else if (totalObser >= orden.Modelo.LimiteSuperiorObservado)
                    color2 = "red";
            }
            return Tuple.Create(color1, color2);
        }
    }
}
