using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using TikiSoft.UniversalPaymentGateway.API.ApplicationServices;
using TikiSoft.UniversalPaymentGateway.API.Extensions;
using TikiSoft.UniversalPaymentGateway.Domain.DataInterfaces;
using TikiSoft.UniversalPaymentGateway.Infrastructure;

namespace TikiSoft.UniversalPaymentGateway
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
            //services.AddControllers().AddNewtonsoftJson(opt => opt.SerializerSettings.Converters.Add(new StringEnumConverter()));            
            services.AddControllers().AddNewtonsoftJson();
            services.AddMvc().SetCompatibilityVersion
            (CompatibilityVersion.Version_3_0)
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreNullValues = true;
            });

            //Esto de abajo es opcional, si no se setea usa un InvalidModelStateResponseFactory por default del Framework.
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var errors = context.ModelState.Values.SelectMany(x => x.Errors.Select(p => p.ErrorMessage)).ToList();
                    var result = new
                    {
                        success = "false",
                        error_code = "validation_error",
                        error_message = "errores de validación en el json entrante",
                        error_source="framework",
                        error_list= errors
                    };
                    return new BadRequestObjectResult(result);
                };
            });

            services.ConfigureDatabase(Configuration);            
            services.AddScoped(typeof(IAuthorizerConfigReader<>), typeof(AuthorizerConfigReader<>));
            services.AddScoped<IProcessorConfigService, ProcessorConfigService>();
            services.AddScoped<IMerchantConfigService, MerchantConfigService>();
            services.ConfigureAuthorizers(Configuration);            
            services.AddAutoMapper();
            services.ConfigureSwagger();
            services.ConfigureAuthentication(Configuration);
            services.Configure<HostOptions>(option =>
            {
                option.ShutdownTimeout = System.TimeSpan.FromSeconds(20);                
            });
                        
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
               app.AddProductionExceptionHandler();
            }

            app.UseStaticFiles();
            
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api/swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "UTG API V1");
                c.RoutePrefix = "api/swagger";
            }); 

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Worker Process Name : "
            //        + System.Diagnostics.Process.GetCurrentProcess().ProcessName);
            //});

        }
    }
}
