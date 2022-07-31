using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoDemo.Api.Mappers;
using TodoDemo.Api.Models;
using TodoDemo.Core.Interfaces;
using TodoDemo.Core.SharedKernel;
using TodoDemo.Infrastructure.Data;
using TodoDemo.Infrastructure.DomainEvents;

namespace TodoDemo.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
            });
            // TODO: Add DbContext and IOC
            string dbName = Guid.NewGuid().ToString();
            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase(dbName));
            //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });


            services.AddControllers();

            services.AddSingleton<IMapper>(_ => AutoMapperConfig.GetMapper());
            //services.AddScoped<IDomainEventDispatcher, DomainDeventDeparcher>();
            var container = new Container();

            container.Configure(config =>
            {
                config.Scan(_ =>
                {
                    _.AssemblyContainingType(typeof(Startup)); // Web
                    _.AssemblyContainingType(typeof(BaseEntity)); // Core
                    _.Assembly("TodoDemo.Infrastructure"); // Infrastructure
                    _.WithDefaultConventions();
                    _.ConnectImplementationsToTypesClosing(typeof(IHandler<>));
                });

                // TODO: Add Registry Classes to eliminate reference to Infrastructure

                // TODO: Move to Infrastucture Registry
                config.For(typeof(IRepository<>)).Add(typeof(EfRepository<>));
                config.For(typeof(IDomainEventDispatcher)).Add(typeof(DomainEventDispatcher));

                //Populate the container using the service collection
                config.Populate(services);
            });

            return container.GetInstance<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                SeedData.EnsurePopulated(app.ApplicationServices.GetService<AppDbContext>());
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API v1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
