
using System.IO;
using Flexify.Repositories;
using Flexify.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NLog;

namespace Flexify
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            string nLogPath = Directory.GetCurrentDirectory() + "/nlog.config";
            LogManager.LoadConfiguration(nLogPath);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string dbString = Configuration.GetConnectionString("FilmDBString");
            services.AddDbContextPool<DatabaseContext>(options => 
                options.UseMySql(dbString, ServerVersion.AutoDetect(dbString))
            );
            
            services.AddAuthentication("APIAuthenticationService")
                .AddScheme<AuthenticationSchemeOptions, APIAuthenticationService>("APIAuthenticationService", null);

            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddScoped<IFilmRepository, FilmRepository>();
            services.AddTransient<IAuthenticationRepository, APIRepository>();
            services.AddScoped<UploadImageService>();
            services.AddControllers();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "Flexify", Version = "v1"}); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {                        
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Flexify v1"));
            }
            
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}