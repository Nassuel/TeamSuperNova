using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ContosoCrafts.WebSite
{

    /// <summary>
    /// Entry point class for the application
    /// </summary>
    public class Program
    {

        /// <summary>
        /// Main entry point method that builds and runs the web host
        /// </summary>
        /// <param name="args">Command line arguments</param>
        public static void Main(string[] args)
        {

            // Create and run the web host
            CreateHostBuilder(args).Build().Run();

        }

        /// <summary>
        /// Creates and configures the host builder for the web application
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>Configured host builder instance</returns>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {

            // Create default host builder
            var hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {

                    // Configure web host to use Startup class
                    webBuilder.UseStartup<Startup>();

                });

            return hostBuilder;

        }

    }

}