

namespace ApiWithNetCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ApiWithNetCore.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
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
            //aqui haremos la inyecccion de despendecia para utilizar el servicio de entity framework con la dbContext
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseInMemoryDatabase("ArticulosDb"));
            services.AddMvc().AddJsonOptions(ConfigureJson);
        }

        private void ConfigureJson(MvcJsonOptions obj)
        {
            obj.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,ApplicationDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            //aqui vamos a crear un metodo para que si no tienes datos que agregue datos de pruevas

            if (!context.Countries.Any())
            {
                context.Countries.AddRange(new List<Country>() {
                    new Country(){ Name="Republica Dominicana", Cities=new List<City>(){
                    new City(){name="Santo domingo" }, 
                    new City (){name ="Bani"}
                    } },
                    new Country(){Name="Estado Unidos", Cities =new List<City>(){
                        new City(){name="Boston"},
                        new City(){name="New York"}
                    } },
                    new Country (){Name="España" , Cities=new List<City>(){
                        new City(){ name="Barcelona"},
                        new City(){ name="Madrid"}
                    } }

                });
                context.SaveChanges();
            }
        }
    }
}
