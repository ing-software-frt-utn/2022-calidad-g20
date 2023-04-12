using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Datos.Migrations
{
    public partial class inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Colores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Defectos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoDeDefecto = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Defectos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Empleados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Usuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contrasena = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoDeEmpleado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LineasDeTrabajo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineasDeTrabajo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Modelos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SKU = table.Column<int>(type: "int", nullable: false),
                    Denominacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LimiteInferiorReproceso = table.Column<int>(type: "int", nullable: false),
                    LimiteSuperiorReproceso = table.Column<int>(type: "int", nullable: false),
                    LimiteInferiorObservado = table.Column<int>(type: "int", nullable: false),
                    LimiteSuperiorObservado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modelos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Turnos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HoraInicio = table.Column<double>(type: "float", nullable: false),
                    HoraFin = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turnos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrdenesDeProduccion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    FechaYHoraInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaYHoraFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    LineaId = table.Column<int>(type: "int", nullable: true),
                    SupervisorDeLineaId = table.Column<int>(type: "int", nullable: true),
                    ModeloId = table.Column<int>(type: "int", nullable: true),
                    ColorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdenesDeProduccion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrdenesDeProduccion_Colores_ColorId",
                        column: x => x.ColorId,
                        principalTable: "Colores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrdenesDeProduccion_Empleados_SupervisorDeLineaId",
                        column: x => x.SupervisorDeLineaId,
                        principalTable: "Empleados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrdenesDeProduccion_LineasDeTrabajo_LineaId",
                        column: x => x.LineaId,
                        principalTable: "LineasDeTrabajo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrdenesDeProduccion_Modelos_ModeloId",
                        column: x => x.ModeloId,
                        principalTable: "Modelos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Jornadas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupervisorDeCalidadId = table.Column<int>(type: "int", nullable: true),
                    FechaYHoraInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaYHoraFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TurnoId = table.Column<int>(type: "int", nullable: true),
                    OrdenDeProduccionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jornadas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Jornadas_Empleados_SupervisorDeCalidadId",
                        column: x => x.SupervisorDeCalidadId,
                        principalTable: "Empleados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Jornadas_OrdenesDeProduccion_OrdenDeProduccionId",
                        column: x => x.OrdenDeProduccionId,
                        principalTable: "OrdenesDeProduccion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Jornadas_Turnos_TurnoId",
                        column: x => x.TurnoId,
                        principalTable: "Turnos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Alertas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    FechaDisparo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaReinicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    JornadaLaboralId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alertas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alertas_Jornadas_JornadaLaboralId",
                        column: x => x.JornadaLaboralId,
                        principalTable: "Jornadas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Incidencias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaYHoraDeRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoraDeRegistro = table.Column<int>(type: "int", nullable: false),
                    DefectoId = table.Column<int>(type: "int", nullable: true),
                    Pie = table.Column<int>(type: "int", nullable: false),
                    JornadaLaboralId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incidencias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Incidencias_Defectos_DefectoId",
                        column: x => x.DefectoId,
                        principalTable: "Defectos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Incidencias_Jornadas_JornadaLaboralId",
                        column: x => x.JornadaLaboralId,
                        principalTable: "Jornadas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alertas_JornadaLaboralId",
                table: "Alertas",
                column: "JornadaLaboralId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidencias_DefectoId",
                table: "Incidencias",
                column: "DefectoId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidencias_JornadaLaboralId",
                table: "Incidencias",
                column: "JornadaLaboralId");

            migrationBuilder.CreateIndex(
                name: "IX_Jornadas_OrdenDeProduccionId",
                table: "Jornadas",
                column: "OrdenDeProduccionId");

            migrationBuilder.CreateIndex(
                name: "IX_Jornadas_SupervisorDeCalidadId",
                table: "Jornadas",
                column: "SupervisorDeCalidadId");

            migrationBuilder.CreateIndex(
                name: "IX_Jornadas_TurnoId",
                table: "Jornadas",
                column: "TurnoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenesDeProduccion_ColorId",
                table: "OrdenesDeProduccion",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenesDeProduccion_LineaId",
                table: "OrdenesDeProduccion",
                column: "LineaId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenesDeProduccion_ModeloId",
                table: "OrdenesDeProduccion",
                column: "ModeloId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenesDeProduccion_SupervisorDeLineaId",
                table: "OrdenesDeProduccion",
                column: "SupervisorDeLineaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alertas");

            migrationBuilder.DropTable(
                name: "Incidencias");

            migrationBuilder.DropTable(
                name: "Defectos");

            migrationBuilder.DropTable(
                name: "Jornadas");

            migrationBuilder.DropTable(
                name: "OrdenesDeProduccion");

            migrationBuilder.DropTable(
                name: "Turnos");

            migrationBuilder.DropTable(
                name: "Colores");

            migrationBuilder.DropTable(
                name: "Empleados");

            migrationBuilder.DropTable(
                name: "LineasDeTrabajo");

            migrationBuilder.DropTable(
                name: "Modelos");
        }
    }
}
