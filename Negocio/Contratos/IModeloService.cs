using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Negocio.Contratos
{
    public interface IModeloService
    {
        List<Modelo> ObtenerListadoModelos();
        void AgregarModelo(int SKU, string denominacion, int limiteInfObs, int limiteSupObs, int limiteInfRepro, int limiteSupRepro);
        void EliminarModelo(int SKU);
        void ActualizarModelo(int SKUPrevio, int SKU, string denominacion, int limiteInfObs, int limiteSupObs, int limiteInfRepro, int limiteSupRepro);
    }
}
