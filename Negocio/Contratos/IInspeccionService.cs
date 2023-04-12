using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Negocio.Contratos
{
    public interface IInspeccionService
    {
        OrdenDeProduccion BuscarOPActiva();
        List<Defecto> ObtenerListadoDefectos();
        void AsociarseAOrdenDeProduccion(int numeroLinea);
        void DesasociarseDeOrdenDeProduccion();
        void AgregarIncidencia(int? horaElegida, string defecto, int pie);
        void EliminarIncidencia(int? HoraSeleccionada, string DefectoSeleccionado, int PieSeleccionado);
        List<LineaDeProduccion> ObtenerLineasDisponibles();
        bool NecesitaDesasociarse();
        List<Tuple<int, int>> ObtenerTotalesDeIncidenciasPorHora();
    }
}
