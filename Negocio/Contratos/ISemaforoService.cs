using Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Negocio.Contratos
{
    public interface ISemaforoService
    {
        OrdenDeProduccion BuscarOPActiva();
        Tuple<string, string> ObtenerColores();
    }
}
