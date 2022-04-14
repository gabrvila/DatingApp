using System;
using System.Threading.Tasks;
using API.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API
{
    // Program.cs is where the application starts. Program.cs file will work the same as Program.cs file in 
    // traditional console application of .NET Framework. Program.cs fille is the entry point of application
    // and will be responsible to register Startup.cs fill, IISIntegration and Create a host using an 
    // instance of IWebHostBuilder, the Main method.s
   public class Program
     {


        public static async Task Main(string[] args)
        {
            // Build() method returns an instance of IHost and Run() starts web application until it stops.
            var host = CreateHostBuilder(args).Build();
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<DataContext>();
                await context.Database.MigrateAsync();
                await Seed.SeedUsers(context);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error ocurred during migration");
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

    }
}
