using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Datos;
using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Negocio.Servicios;
using System.Collections.Generic;
using InfraestructuraTransversal;

namespace Pruebas.Servicios
{
    [TestClass]
    public class PruebaUnitariaServicios
    {
        Mock<DatosContexto> _contexto = new Mock<DatosContexto>();

        [TestMethod]//  todo: no puedo instanciar DbSet para simular colores, modelos --> rediseñar el contexto con interfaz para usar moq
        public void GenerarOPNuevaConNumeroSinRepetir()
        {
            Cache.Instance.LiberarCache();
            OrdenDeProduccionService servicio = new OrdenDeProduccionService(_contexto.Object);

            _contexto.Setup(x => x.OrdenesDeProduccionSinFinalizar()).Returns(new List<OrdenDeProduccion>());
            _contexto.Setup(x => x.LineasDisponiblesParaCrearOP()).Returns(new List<LineaDeProduccion>() 
            {
                new LineaDeProduccion {Numero = 5}
            });
            //_contexto.Setup(x => x.Modelos).Returns(new DbSet<Modelo>());
            //_contexto.Setup(x => x.Colores).Returns(new DbSet<Color>());          

            int numeroOP = 16;
            int numeroLinea = 5;
            int SKUModelo = 4;
            string descripcionColor = "Negro";
            Cache.Instance.GuardarEmpleadoID(1);            

            servicio.GenerarOP(numeroOP, numeroLinea, SKUModelo, descripcionColor);

            var orden = servicio.BuscarOPActiva();
            
            Assert.AreEqual(numeroOP,orden.Numero);
        }

        [TestMethod]
        public void FinalizarOrdenDeProduccionActiva()
        {
            Cache.Instance.GuardarEmpleadoID(1);
            OrdenDeProduccionService servicio = new OrdenDeProduccionService(_contexto.Object);

            var orden = servicio.BuscarOPActiva();
            servicio.ActualizarEstadoOP(2);

            Assert.AreEqual(orden.Estado, Dominio.Enumeraciones.EstadoOP.Finalizada);
            Assert.AreNotEqual(orden.FechaYHoraFin, System.DateTime.MinValue);
        }
    }
}
