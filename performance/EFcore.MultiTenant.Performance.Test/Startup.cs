using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFcore.MultiTenant.Performance.Test.DbContext;
using EFcore.MultiTenant.Performance.Test.Generator;
using kiwiho.EFcore.MultiTenant.Core;
using kiwiho.EFcore.MultiTenant.Core.Interface;
using kiwiho.EFcore.MultiTenant.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EFcore.MultiTenant.Performance.Test
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
            services.AddMemoryCache(option=>{
                option.SizeLimit = 1_000_000;
            });
            services.AddScoped<IConnectionGenerator, CombindedConnectionGenerator>();
            services.AddMySqlPerTable<DemoContext>(settings =>
            {
                settings.DbContextSetup = SetUpMySql;
            });
            services.AddControllers();
        }

        void SetUpMySql(IServiceProvider serviceProvider, string connectionString,
            DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString, builder =>
            {
            });
            optionsBuilder.UseMemoryCache(serviceProvider.GetService<IMemoryCache>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<TenantInfoMiddleware>();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
