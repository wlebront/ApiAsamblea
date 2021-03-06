using ApiAsamblea.Data;
using ApiAsamblea.AsambleistasMapper;
using ApiAsamblea.Repository;
using ApiAsamblea.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.IO;
using Microsoft.OpenApi.Models;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using ApiAsamblea.helpers;

namespace ApiAsamblea
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
            services.AddDbContext<ApplicationDbContext>(Options => Options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IAsambleistaRepository, AsambleistaRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();



            /*Agregar dependencia del token*/
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddAutoMapper(typeof(AsambleistasMappers));

            //De aqui en adelante configuracion de documentacion de nuestra api
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("ApiAsamblea", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "Api Asamblea",
                    Version = "1",
                    Description = "Backend Asamblea PRSC",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "Williamlebron23@hotmail.com",
                        Name = "William Lebron",
                        Url = new Uri("https://prsc.org.do")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
                    }

                });

                options.SwaggerDoc("ApiAsambleaUsuarios", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "Api Asamblea Usuarios",
                    Version = "1",
                    Description = "Backend Asamblea PRSC",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "Williamlebron23@hotmail.com",
                        Name = "William Lebron",
                        Url = new Uri("https://prsc.org.do")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
                    }

                });

                var archivoXmlComentarios = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var rutaApiComentario = Path.Combine(AppContext.BaseDirectory, archivoXmlComentarios);
                options.IncludeXmlComments(rutaApiComentario);

                //Primero definir el esquema de seguridad
                options.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description = "Auttenticacion JWT (Bearer)",
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer"
                    });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement{
                 {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    }, new List<string>()
                 }
                 });

            });



            /*Damos soporte para core*/
            services.AddCors(options => {
                options.AddPolicy("EnableCORS", builder => {
                    builder.WithOrigins("http://localhost:4200", "http://wlebront-001-site3.etempurl.com")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            services.AddControllers();
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
                app.UseExceptionHandler(ApplicationBuilder => {
                    ApplicationBuilder.Run(async context =>{
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        var error = context.Features.Get<IExceptionHandlerFeature>();

                        if (error != null)
                        {
                            context.Response.AddAplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                     });
                   });

            }

            app.UseHttpsRedirection();
            //Linea para documentacion api
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("swagger/ApiAsamblea/swagger.json", "API Asamblea");
                options.SwaggerEndpoint("swagger/ApiAsambleaUsuarios/swagger.json", "API Asamblea Usuarios");
                options.RoutePrefix = "";
            });



            app.UseRouting();

            /*Damos soporte para core*/
            app.UseCors("EnableCORS");

            /*Esto dos son para la autenticacion y autorizacion*/
            app.UseAuthentication();
            app.UseAuthentication();


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

           
        
        }
    }
}
