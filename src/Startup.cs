using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ContosoCrafts.WebSite
{

    /// <summary>
    /// Startup class for configuring services and the application pipeline
    /// </summary>
    public class Startup
    {

        // Configuration settings for the application
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Constructor to initialize Startup with configuration
        /// </summary>
        /// <param name="configuration">Application configuration instance</param>
        public Startup(IConfiguration configuration)
        {

            Configuration = configuration;

        }

        /// <summary>
        /// Configures services to be added to the dependency injection container
        /// Called by the runtime to add services
        /// </summary>
        /// <param name="services">Service collection to configure</param>
        public void ConfigureServices(IServiceCollection services)
        {

            // Add Razor Pages support
            services.AddRazorPages();

            // Add server-side Blazor support
            services.AddServerSideBlazor();

            // Add HTTP client services
            services.AddHttpClient();

            // Add controller support
            services.AddControllers();

            // Register JSON file product service
            services.AddTransient<JsonFileProductService>();

        }

        /// <summary>
        /// Configures the HTTP request pipeline
        /// Called by the runtime to configure middleware
        /// </summary>
        /// <param name="app">Application builder instance</param>
        /// <param name="env">Web host environment instance</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            // Check if running in development environment
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Check if not running in development environment
            if (env.IsDevelopment() == false)
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            // Enable HTTPS redirection
            app.UseHttpsRedirection();

            // Enable static file serving
            app.UseStaticFiles();

            // Enable routing
            app.UseRouting();

            // Enable authorization
            app.UseAuthorization();

            // Configure endpoints
            app.UseEndpoints(endpoints =>
            {

                // Map Razor Pages endpoints
                endpoints.MapRazorPages();

                // Map controller endpoints
                endpoints.MapControllers();

                // Map Blazor Hub endpoint
                endpoints.MapBlazorHub();

            });

        }

    }

}