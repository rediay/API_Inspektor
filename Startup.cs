using Inspektor_API_REST.Models;
using Inspektor_API_REST.Models.Auth;
using Inspektor_API_REST.Servicios;
using Inspektor_API_REST.Servicios.Files;
using Inspektor_API_REST.Servicios.GenerateReport;
using Inspektor_API_REST.Servicios.MainQuery;
using Inspektor_API_REST.Servicios.ServicesAdditional;
using Inspektor_API_REST.Setup;
using Inspektor_API_REST.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Inspektor_API_REST
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration _configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<JwtManage>();
            services.AddScoped<IAuthService, AuthService<Usuarios>>();
            services.AddScoped<IMainQuery, MainQuery>();
            services.AddScoped<IServicesAdditional, ServicesAdditional>();
            services.AddScoped<IGenerateReport, GenerateReport>();
            services.AddScoped<IFileShare, Servicios.Files.FileShare>();


            services.AddHttpClient();
            services.AddSingleton<HttpClientFactory>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "API Inspektor",
                    Version = "v1",
                    Description = "API desarrollada para la consulta y verificación de información relacionada con listas restrictivas, vinculantes, inhibitorias o condicionantes, informativas y de PEPs para la simplificación de procesos de conocimiento de terceros y debida diligencia. El API está soportado por Inspektor®",
                    TermsOfService = new Uri("https://riskglobalconsulting.com/co/wp-content/uploads/2020/09/PL-GG-06-POLITICA-DE-PROTECCION-DE-DATOS.pdf"),
                    Contact = new OpenApiContact
                    {
                        Name = "Contacto",
                        Url = new Uri("https://www.riskglobalconsulting.com/co/contactanos-risk-consulting-global-group/")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Propietario",
                        Url = new Uri("https://example.com/license")
                    }

                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"El API cuenta con un esquema de autorización basado en tokens JWT (JSON Web Tokens) el cual es proporcionado a través de los encabezados de la solicitud hecha al servicio. El token usa el esquema 'Bearer', debe ingresar 'Bearer' [espacio] y el token que le fue proporcionado para hacer uso del servicio.
                      Por ejemplo: 'Bearer eslmvnLKFLK389...'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "http",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });

                var xmlPath = Path.Combine(AppContext.BaseDirectory, "InspektorAPI.xml");
                c.IncludeXmlComments(xmlPath);
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<DatabaseHelper>();
            services.AddDbContext<listasrestrictivas_riskconsultingcolombia_comContext>(options => options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));

            //Procesamiento del token JWT            
            services.ConfigureAuth(_configuration);

            //Habilitar CORS
            services.AddCors(options => options.AddPolicy("EnableCORS", builder =>
            {
                builder.SetIsOriginAllowedToAllowWildcardSubdomains()
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            }
            ));          

            services.AddAuthorization(options =>
            {
                var rolePolicyConfig = _configuration.GetSection("PolicesConfig").Get<RolePolicyConfiguration[]>();

                foreach (var configurationPolicy in rolePolicyConfig)
                {
                    options.AddPolicy(configurationPolicy.policy, policy => policy.RequireRole(configurationPolicy.roles));
                }
            });

        }
        public bool ValidadorVigencia(DateTime? despuesDe, DateTime? expiracion, SecurityToken tokenSeguridad, TokenValidationParameters parametrosValidacion)
        {
            if (expiracion != null)
            {
                if (DateTime.UtcNow < expiracion) { return true; }
            }
            return false;
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseSwagger();

            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Inspektor - v1"));

            app.UseCors("EnableCORS");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
