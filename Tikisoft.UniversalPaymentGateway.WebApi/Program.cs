using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TikiSoft.UniversalPaymentGateway.Authorizers.LaPos.Data;
using TikiSoft.UniversalPaymentGateway.Persistence.Contexts;



namespace TikiSoft.UniversalPaymentGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
           
            using (var scope = host.Services.CreateScope())
            {                
                using (var context = scope.ServiceProvider.GetService<ConfigDbContext>())
                {
                    context.Database.EnsureCreated();
                }
                using (var context = scope.ServiceProvider.GetService<LaPosDbContext>())
                {
                    context.Database.EnsureCreated();
                }
            }

            host.Run();
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.CaptureStartupErrors(true);
                    webBuilder.UseSetting(WebHostDefaults.DetailedErrorsKey, "true");                    
                    
                    //webBuilder.UseUrls("http://*:5000;http://localhost:5001;https://hostname:5002");
                });
    }
}
