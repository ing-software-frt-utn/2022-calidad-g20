using Dominio.Entidades;
using Dominio.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Text;

namespace Negocio.Contratos
{
    public interface IOrdenDeProduccionService
    {
        List<LineaDeProduccion> ObtenerListadoLineasDisponibles();
        List<Modelo> ObtenerListadoModelos();
        List<Color> ObtenerListadoColores();
        OrdenDeProduccion BuscarOPActiva();
        void GenerarOP(int numeroOP, int numeroLinea, int SKUModelo, string descripcionColor);
        void ActualizarEstadoOP(int estado);
        void ReiniciarSemaforo(int tipoDeSemaforo, string codigoReinicio);
    }
}
