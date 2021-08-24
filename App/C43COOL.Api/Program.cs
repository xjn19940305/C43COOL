using AutoMapper;
using C43COOL.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace C43COOL.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var logger = scope.ServiceProvider.GetService<ILogger<Program>>();
                var context = scope.ServiceProvider.GetRequiredService<C43DbContext>();
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
                try
                {
                    var Init = new Init(context, mapper, configuration);
                    //await Init.ExportData();
                    var shouldSeed = !await context.Database.CanConnectAsync();
                    logger.LogInformation("Migrating Database");
                    await context.Database.MigrateAsync();
                    if (shouldSeed)
                    {
                        logger.LogInformation($"{DateTime.Now} 初始化数据库初始化数据");
                        //await Init.Seeds();
                    }
                }
                catch (Exception err)
                {
                    logger.LogInformation(err, "Migration Failed");
                }
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
