using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LikeButton.Core.Interfaces;
using LikeButton.Infrastructure.Data;
using LikeButton.Infrastructure.Logger;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LikeButton.Solution
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<AppDbContext>();
                    context.Database.Migrate();
                    //Seed.SeedUsers(context);
                }
                catch (Exception exception)
                {
                    var logger = services.GetRequiredService<IFileLogger>();
                    logger.LogError(exception + " An error occured during migrations", "ApplicationStart");

                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
