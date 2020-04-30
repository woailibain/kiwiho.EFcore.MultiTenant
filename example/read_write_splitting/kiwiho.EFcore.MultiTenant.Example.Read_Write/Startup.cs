using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kiwiho.EFcore.MultiTenant.Core;
using kiwiho.EFcore.MultiTenant.Core.Extension;
using kiwiho.EFcore.MultiTenant.Core.Interface;
using kiwiho.EFcore.MultiTenant.Example.Read_Write.DbContext;
using kiwiho.EFcore.MultiTenant.Example.Read_Write.Generator;
using kiwiho.EFcore.MultiTenant.Example.Read_Write.Provider;
using kiwiho.EFcore.MultiTenant.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace kiwiho.EFcore.MultiTenant.Example.Read_Write
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
            services.AddSingleton<IActionInvokerProvider, ReadWriteActionInvokerProvider>();
            services.AddScoped<IConnectionGenerator, ReadWriteConnectionGenerator>();
            services.AddTenantedDatabase<StoreDbContext>(null, setupDb);

            services.AddControllers();
        }

        void setupDb(TenantSettings<StoreDbContext> settings)
        {
            settings.ConnectionPrefix = "mysql_";
            settings.DbContextSetup = (serviceProvider, connectionString, optionsBuilder) =>
            {
                var tenant = serviceProvider.GetService<TenantInfo>();
                optionsBuilder.UseMySql(connectionString, builder =>
                {
                    // not necessary, if you are not using the table or schema 
                    builder.TenantBuilderSetup(serviceProvider, settings, tenant);
                });
            };
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            // app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
