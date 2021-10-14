using MapWebApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MapWebApi.Services.Interfaces;
using MapWebApi.Services.Realization;

namespace MapWebApi
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        //Конфигурация приложения
        public IConfiguration Configuration { get; }

        // Конфигурация сервисов
        public void ConfigureServices(IServiceCollection services)
        {
            //для докера
            //"DefaultConnection": "Server=database,1433;Database=MapDB;User ID=sa;Password=Thesecretpassword_1;Trusted_Connection=False"

            //для локалки
            //"DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"

            services.AddScoped<IActualDataService, ActualDataService>();

            
            string connectionString = GetConnectionString("DefaultConnection");
            // устанавливаем контекст данных
            services.AddDbContext<SubdivisionsContext>(options => options.UseSqlServer(connectionString));

           // services.AddDbContext<SubdivisionsContext>(opt => opt.UseInMemoryDatabase("Alex"));
            //добавление контроллеров
            services.AddControllers();
            //добавление политики CORS
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader();
                                  });
            });
            //добавление Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MapWebApi", Version = "v1" });
            });
        }

        // Конфигурация pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ILogger<Startup> logger)
        {

            app.Use(async (context, next) =>
            {
                logger.LogInformation($"Processing request {context.Request.Path.Value}");
                await next();
            });
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MapWebApi v1"));
            app.UseHttpsRedirection();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseRouting();

            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

           
        }
        //Получение строки подключение к БД
        public string GetConnectionString(string connection)
        {
            string ConnectionString = string.Empty;
            var builder = new ConfigurationBuilder();
            // установка пути к текущему каталогу
            builder.SetBasePath(Directory.GetCurrentDirectory());
            // получаем конфигурацию из файла appsettings.json
            builder.AddJsonFile("appsettings.json");
            // создаем конфигурацию
            var config = builder.Build();
            ConnectionString = config.GetConnectionString(connection);
            return ConnectionString;
        }
    }
}
