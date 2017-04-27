using AutoMapper;
using BC_ContosoRecordsModule.Application.MappingProfile;
using BC_ContosoRecordsModule.DependencyInjection;
using ContosoRetail.WebAPI.MappingProfile;
using ContosoRetail.WebAPI.ModelBinderProviders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ContosoRetail.WebAPI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add module services.
            services.AddContosoRecordsModule();

            // Add framework services.
            services.AddSingleton<IMapper>(
                new Mapper(
                    new MapperConfiguration(cfg =>
                    {
                        cfg.AddProfile<BCContosoRecordsModuleApplicationMappingProfile>();
                        cfg.AddProfile<ContosoRetailWebAPIMappingProfile>();
                    })
                ));
            services.AddCors();
            services.AddMvc(
                config =>
                {
                    config.ModelBinderProviders.Insert(0, new DataSourceLoadOptionsBinderProvider());
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors(cfg => { cfg.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin(); });
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseMvc();
        }
    }
}
