using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using MetricsAgent.DAL.Repositories;
using MetricsAgent.DAL.MetricsClasses;
using MetricsAgent.DAL;
using AutoMapper;
using Dapper;

namespace MetricsAgent
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        private string ConnectionString;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
            getConnectionString();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            ConfigureSqlLiteConnection(services);
            services.AddScoped<ICpuMetricsRepository, CpuMetricsRepository>();
            services.AddScoped<IDotNetMetricsRepository, DotNetMetricsRepository>();
            services.AddScoped<IHddMetricsRepository, HddMetricsRepository>();
            services.AddScoped<INetworkMetricsRepository, NetworkMetricsRepository>();
            services.AddScoped<IRamMetricsRepository, RamMetricsRepository>();
            var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfiles()));
            var mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);
        }
        public void getConnectionString()
        {
            ConnectionString = ConnectionStringClass.ConnectionString;
        }
        private void ConfigureSqlLiteConnection(IServiceCollection services)
        {
            var connection = new SQLiteConnection(ConnectionString);
            connection.Open();
            PrepareSchema(connection);
        }
        private void PrepareSchema(SQLiteConnection connection)
        {
            using (var command = new SQLiteCommand(connection))
            {

                command.CommandText = "DROP TABLE IF EXISTS cpumetrics";

                command.ExecuteNonQuery();

                command.CommandText = @"CREATE TABLE cpumetrics(id INTEGER PRIMARY KEY,
                    value INT, time INT)";
                command.ExecuteNonQuery();

                command.CommandText = "DROP TABLE IF EXISTS dotnetmetrics";

                command.ExecuteNonQuery();

                command.CommandText = @"CREATE TABLE dotnetmetrics(id INTEGER PRIMARY KEY,
                    value INT, time INT)";

                command.ExecuteNonQuery();


                command.CommandText = "DROP TABLE IF EXISTS hddmetrics";

                command.ExecuteNonQuery();

                command.CommandText = @"CREATE TABLE hddmetrics(id INTEGER PRIMARY KEY,
                    value INT, time INT)";

                command.ExecuteNonQuery();

                command.CommandText = "DROP TABLE IF EXISTS networkmetrics";

                command.ExecuteNonQuery();

                command.CommandText = @"CREATE TABLE networkmetrics(id INTEGER PRIMARY KEY,
                    value INT, time INT)";

                command.ExecuteNonQuery();


                command.CommandText = "DROP TABLE IF EXISTS rammetrics";

                command.ExecuteNonQuery();

                command.CommandText = @"CREATE TABLE rammetrics(id INTEGER PRIMARY KEY,
                    value INT, time INT)";

                command.ExecuteNonQuery();
            }
        }
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
        }
    }
}
