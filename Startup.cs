using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Api_dotnet.Data;
using Microsoft.EntityFrameworkCore;

namespace Api_dotnet
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
            services.AddDbContext<AplicationDBContext>(options => options.UseMySql(Configuration.GetConnectionString("Conection")));
            services.AddSwaggerGen(config =>{
                config.SwaggerDoc("v1",new Microsoft.OpenApi.Models.OpenApiInfo{Title="Primeira Api",Version="v1"});
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            /*https://localhost:5001/swagger/index.html
            link para ver a documentação do projeto*/
            app.UseSwagger();// gera um arquivo json, que vai ficar no diretorio abaixo
            app.UseSwaggerUI(config =>{//views html swagger
                config.SwaggerEndpoint("/swagger/v1/swagger.json","v1 docs");
            });
        }
    }
}
