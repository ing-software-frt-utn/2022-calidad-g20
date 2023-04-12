using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Negocio.Contratos;
using Negocio.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalzadosSPA
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("MiCookieDeAutenticacion").AddCookie("MiCookieDeAutenticacion", options => 
            {
                options.Cookie.Name = "MiCookieDeAutenticacion";
                options.LoginPath = "/Login/IniciarSesion";
                options.AccessDeniedPath = "/Login/AccesoDenegado";
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("DebeSerAdmininstrativo",
                    policy => policy.RequireClaim("Empleado", "Administrativo"));

                options.AddPolicy("DebeSerSDL",
                    policy => policy.RequireClaim("Empleado", "SupervisorDeLinea"));
                options.AddPolicy("DebeSerSDC",
                    policy => policy.RequireClaim("Empleado", "SupervisorDeCalidad"));                
            });

            services.AddRazorPages();
            services.AddScoped<IModeloService, ModeloService>();
            services.AddScoped<IOrdenDeProduccionService, OrdenDeProduccionService>();
            services.AddScoped<ISesionService,SesionService>();
            services.AddScoped<IInspeccionService,InspeccionService>();
            services.AddScoped<ISemaforoService,SemaforoService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();    //middleware autenticar

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
