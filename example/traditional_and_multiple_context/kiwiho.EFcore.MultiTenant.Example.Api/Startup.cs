using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kiwiho.EFcore.MultiTenant.Core;
using kiwiho.EFcore.MultiTenant.Example.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace kiwiho.EFcore.MultiTenant.Example.Api
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
            // MySql
            // services.AddMySqlPerConnection<StoreDbContext>(settings=>
            // {
            //     settings.ConnectionPrefix = "mysql_";
            // });
            // services.AddMySqlPerTable<StoreDbContext>(connectionName:"mysql_default");
            // services.AddMySqlPerTable<StoreDbContext>(settings=>
            // {
            //     settings.ConnectionName = "mysql_default";
            //     settings.TableNameFunc = (tenantInfo, tableName)=>$"{tenantInfo.Name}44{tableName}";
            // });

            // SqlServer
            // services.AddSqlServerPerConnection<StoreDbContext>(connectionPrefix: "sqlserver_");
            // services.AddSqlServerPerTable<StoreDbContext>(connectionName: "sqlserver_default");
            // services.AddSqlServerPerSchema<StoreDbContext>(connectionName: "sqlserver_default");

            //Postgre
            // services.AddPostgrePerConnection<StoreDbContext>(connectionPrefix:"postgre_");
            services.AddPostgrePerTable<StoreDbContext>(connectionName: "postgre_default");
            // services.AddPostgrePerSchema<StoreDbContext>(connectionName: "postgre_default");


            services.AddControllers();
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
