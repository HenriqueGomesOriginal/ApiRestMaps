/**
* Author: Henrique Gomes
* Date: 29, April, 2022
*/

using Microsoft.AspNetCore.Rewrite;
using Microsoft.OpenApi.Models;
using Microsoft.Net.Http.Headers;
using Serilog;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ApiRestMaps.Services;
using ApiRestMaps.Services.Implementation;
using ApiRestMaps.Configuration;

namespace ApiRestMaps
{
    public class Startup
    {

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            // Log config
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Media Type
            services.AddMvc(options =>
            {
                options.RespectBrowserAcceptHeader = true;

                options.FormatterMappings.SetMediaTypeMappingForFormat("xml", MediaTypeHeaderValue.Parse("application/xml"));
                options.FormatterMappings.SetMediaTypeMappingForFormat("json", MediaTypeHeaderValue.Parse("application/json"));
            })
                .AddXmlSerializerFormatters();

            // Google Cloud
            services.Configure<GoogleCloud>(Configuration.GetSection("GoogleApiConfig"));

            // Version control
            services.AddApiVersioning();

            services.AddControllers();

            // Dependency injection
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IMapsService, MapsService>();

            services.AddOptions();

            services.AddCors(options => options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin();
            }));

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Title = "ApiRestMaps",
                    Version = "v1",
                    Description = "Developed in ASP.NET 6",
                    Contact = new OpenApiContact
                    {
                        Name = "Henrique Gomes",
                        Url = new Uri("https://github.com/HenriqueGomesOriginal"),
                        Email = "henriquegomesoriginal@gmail.com"
                    }
                });
            });
        }

        public void Configure(WebApplication app, IWebHostEnvironment environment)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();

            app.UseRouting();

            // Adding CORS, need to be in this location
            app.UseCors();

            app.UseAuthorization();

            var options = new RewriteOptions();
            options.AddRedirect("ˆ$", "swagger");
            app.UseRewriter(options);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute("DefaultApi", "{controller=values}/{id?}");
            });
        }
    }
}
