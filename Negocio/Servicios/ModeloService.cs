using Datos;
using Dominio.Entidades;
using Negocio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Negocio.Servicios
{
    public class ModeloService : IModeloService
    {
        private DatosContexto _contexto = new DatosContexto();

        //1 -   Check
        public List<Modelo> ObtenerListadoModelos()
        {
            return _contexto.Modelos.OrderBy(mod => mod.SKU).ToList();
        }
        //2 -   Check
        public void AgregarModelo(int SKU, string denominacion, int limiteInfObs, int limiteSupObs, int limiteInfRepro, int limiteSupRepro)
        {
            if (SKUUnico(null,SKU))
            {
                Modelo modelo = new Modelo()
                {
                    SKU = SKU,
                    Denominacion = denominacion,
                    LimiteInferiorObservado = limiteInfObs,
                    LimiteSuperiorObservado = limiteSupObs,
                    LimiteInferiorReproceso = limiteInfRepro,
                    LimiteSuperiorReproceso = limiteSupRepro
                };
                _contexto.Modelos.Add(modelo);
                _contexto.SaveChanges();
            }
        }
        //3 -   Check
        public void EliminarModelo(int SKU)
        {
            var modelo = _contexto.Modelos.FirstOrDefault(mod => mod.SKU.Equals(SKU));
            if(modelo != null)
            {
                _contexto.Modelos.Remove(modelo);
                _contexto.SaveChanges();
            }
        }
        //4 -   Check
        public void ActualizarModelo(int SKUAntes, int SKU, string denominacion, int limiteInfObs, int limiteSupObs, int limiteInfRepro, int limiteSupRepro)
        {
            var modelo = _contexto.Modelos.FirstOrDefault(mod => mod.SKU.Equals(SKUAntes));
            if (modelo != null && SKUUnico(SKUAntes, SKU))
            {
                modelo.SKU = SKU;
                modelo.Denominacion = denominacion;
                modelo.LimiteInferiorReproceso = limiteInfObs;
                modelo.LimiteSuperiorReproceso = limiteSupObs;
                modelo.LimiteInferiorObservado = limiteInfRepro;
                modelo.LimiteSuperiorObservado = limiteSupRepro;
                _contexto.Modelos.Update(modelo);
                _contexto.SaveChanges();
            }
        }

        private bool SKUUnico(int? SKUPrevio, int SKUNuevo)
        {
            if (SKUPrevio == SKUNuevo)
                return true;
            return _contexto
                .Modelos
                .FirstOrDefault(mod => mod.SKU.Equals(SKUNuevo)) 
                is null;
        }
    }
}
