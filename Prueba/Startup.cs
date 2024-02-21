using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Prueba
{
    /// <summary>
    /// Clase de inicio que configura los servicios de la aplicación y el middleware.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Método para configurar los servicios de la aplicación.
        /// </summary>
        /// <param name="services">Colección de servicios de la aplicación.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Configurar servicios aquí
            services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp",
                    builder => builder
                        .WithOrigins("http://localhost:3000") // Permitir solo el origen de la aplicación de React
                        .AllowAnyMethod() // Permitir cualquier método HTTP
                        .AllowAnyHeader()); // Permitir cualquier encabezado
            });

            services.AddControllers();
        }

        /// <summary>
        /// Método para configurar el middleware de la aplicación.
        /// </summary>
        /// <param name="app">Constructor de aplicaciones de ASP.NET Core.</param>
        public void Configure(IApplicationBuilder app)
        {
            // Configurar middleware aquí
            app.UseDeveloperExceptionPage(); // Agregar este middleware para capturar y mostrar excepciones
            app.UseHttpLogging(); // Agregar este middleware para registrar solicitudes y respuestas en la consola
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("AllowReactApp"); // Aplicar la política CORS

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
