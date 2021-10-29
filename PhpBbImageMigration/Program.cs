using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PhpBbImageMigration.Domain.App;
using PhpBbImageMigration.Domain.ImagesHandling;
using PhpBbImageMigration.Domain.Posts;
using PhpBbImageMigration.Infrastructure.ImageUpload.Ftp;
using PhpBbImageMigration.Infrastructure.ImageUpload.Ftp.Config;
using PhpBbImageMigration.Infrastructure.MySql;
using PhpBbImageMigration.Infrastructure.MySql.Context;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PhpBbImageMigration
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                //.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("app.secrets.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var serviceProvider = new ServiceCollection()
                .AddHttpClient()
                .AddTransient<IPostsRepository, MysqlPostsRepository>()
                .AddScoped<ImageMigrationWorker>()
                .AddSingleton<IConfiguration>(configuration)
                .AddSingleton<IImageUploader, FtpImageUploader>((sp) =>
                {
                    var client = sp.GetRequiredService<HttpClient>();
                    var config = sp.GetRequiredService<IConfiguration>();
                    var ftpConfig = config.GetSection("ImageUpload:Ftp").Get<FtpConfiguration>();

                    return new FtpImageUploader(ftpConfig, client);
                })
                .AddSingleton<AppConfiguration>((sp) =>
                {
                    return sp.GetRequiredService<IConfiguration>().Get<AppConfiguration>();
                })
                .AddDbContext<PhpbbContext>((sp, o) =>
                {
                    var cfg = sp.GetRequiredService<IConfiguration>();
                    o.UseMySQL(cfg.GetConnectionString("PhpBb"));
                })
                .AddLogging()
                .BuildServiceProvider();

            var worker = serviceProvider.GetRequiredService<ImageMigrationWorker>();

            worker.Start(new string[] { "shrani.si" }).Wait();

            //using IHost host = CreateHostBuilder(args).Build();

            //return host.RunAsync();
        }

        //static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureServices((_, services) =>
        //            {
        //                services.AddScoped<IPostsRepository, MysqlPostsRepository>();
        //                services.AddDbContext<PhpbbContext>(o =>
        //                {
        //                    o.UseMySQL("name=ConnectionStrings:PhpBb");
        //                });
        //            });
    }
}
