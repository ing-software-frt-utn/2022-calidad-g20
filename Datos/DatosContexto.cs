using Dominio.Entidades;
using Dominio.Enumeraciones;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datos
{
    public class DatosContexto : DbContext
    {
        public DbSet<Modelo> Modelos { get; set; }
        public DbSet<OrdenDeProduccion> OrdenesDeProduccion { get; set; }
        public DbSet<Color> Colores { get; set; }
        public DbSet<LineaDeProduccion> LineasDeTrabajo { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Turno> Turnos { get; set; }
        public DbSet<Defecto> Defectos { get; set; }
        public DbSet<JornadaLaboral> Jornadas { get; set; }
        public DbSet<Incidencia> Incidencias { get; set; }
        public DbSet<Alerta> Alertas { get; set; }

        public DatosContexto()
        {
        }
        public DatosContexto(DbContextOptions<DatosContexto> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlServer(@"Server=.\SQLExpress;Database=IS_TFI;Trusted_Connection = True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder
            //    .Entity<OrdenDeProduccion>()
            //    .HasMany(op => op.Jornadas)
            //    .WithOne(op => op.OrdenDeProduccion);
            //.UsingEntity<Dictionary<string, object>>("CursosEstudiantes",
            //j => j.HasOne<Curso>().WithMany().HasForeignKey("CursoId"),
            //j => j.HasOne<Estudiante>().WithMany().HasForeignKey("EstudianteId"),
            //j => j.ToTable("CursosEstudiantes"));
            //.IsRequired();
            //modelBuilder.Entity<Incidencia>()
            //    .HasOne(x => x.Jornada)
            //    .WithMany(x => x.Incidencias)
            //    .OnDelete(DeleteBehavior.ClientCascade);
        }


        #region
        public List<OrdenDeProduccion> OrdenesDeProduccionSinFinalizar()
        {
            return OrdenesDeProduccion
                .Where(op => op.Estado!=EstadoOP.Finalizada)
                .Include(op => op.Modelo)
                .Include(op => op.Color)
                .Include(op => op.Linea)
                .Include(op => op.SupervisorDeLinea)
                .Include(op => op.Jornadas).ThenInclude(j => j.Turno)
                .Include(op => op.Jornadas).ThenInclude(j => j.Incidencias).ThenInclude(i => i.Defecto)
                .Include(op => op.Jornadas).ThenInclude(j => j.Alertas)
                .ToList();
        }
        public List<OrdenDeProduccion> OrdenesDeProduccionActivas()
        {
            return OrdenesDeProduccion
                .Where(op => op.Estado.Equals(EstadoOP.Inciada))
                .Include(op => op.Modelo)
                .Include(op => op.Color)
                .Include(op => op.Linea)
                .Include(op => op.SupervisorDeLinea)
                .Include(op => op.Jornadas).ThenInclude(j => j.Turno)
                .Include(op => op.Jornadas).ThenInclude(j => j.Incidencias).ThenInclude(i => i.Defecto)
                .Include(op => op.Jornadas).ThenInclude(j => j.Alertas)
                .ToList();
        }
        public List<LineaDeProduccion> LineasDisponiblesParaCrearOP()
        {
            var lineas = LineasDeTrabajo.ToList();
            var ordenes = OrdenesDeProduccion.ToList();
            foreach (var orden in ordenes)
            {
                if (orden.Estado != EstadoOP.Finalizada)
                    lineas.RemoveAll(linea => linea.Numero.Equals(orden.Linea.Numero));
            }
            return lineas;
        }
        public List<LineaDeProduccion> LineasDisponiblesParaInspeccion()
        {
            var ordenes = OrdenesDeProduccionActivas();
            var lineasActivas = ordenes.Select(op => op.Linea).ToList();
            foreach (var orden in ordenes)
            {
                if (orden.JornadaActual() != null)
                {
                    var borrar = lineasActivas
                        .FirstOrDefault(l => l.Numero
                        .Equals(orden.Linea.Numero));
                    lineasActivas.Remove(borrar);
                }
            }
            return lineasActivas;
        }
        public JornadaLaboral UltimaJornadaActivaDelSupervisor(Empleado empleado)
        {

            return Jornadas.Include(j => j.SupervisorDeCalidad)
                .FirstOrDefault(j => j.SupervisorDeCalidad
                .Equals(empleado)
                && j.FechaYHoraFin.Equals(DateTime.MinValue));
        }
        #endregion
    }
}
