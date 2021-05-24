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
using FluentMigrator.Runner;
using Quartz;
using Quartz.Spi;
using MetricsAgent.Jobs;
using Quartz.Impl;

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

            services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // добавляем поддержку SQLite 
                    .AddSQLite()
                    // устанавливаем строку подключения
                    .WithGlobalConnectionString(ConnectionString)
                    // подсказываем где искать классы с миграциями
                    .ScanIn(typeof(Startup).Assembly).For.Migrations()
                ).AddLogging(lb => lb
                    .AddFluentMigratorConsole());

            // ДОбавляем сервисы
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            // добавляем нашу задачу
            services.AddSingleton<CpuMetricJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(CpuMetricJob),
                cronExpression: "0/5 * * * * ?")); // запускать каждые 5 секунд
            services.AddSingleton(new JobSchedule(
                jobType: typeof(RamMetricJob),
                cronExpression: "0/5 * * * * ?"));

            services.AddHostedService<QuartzHostedService>();
        }
        public void getConnectionString()
        {
            ConnectionString = ConnectionStringClass.ConnectionString;
        }
        private void ConfigureSqlLiteConnection(IServiceCollection services)
        {
            var connection = new SQLiteConnection(ConnectionString);
        }
        //private void PrepareSchema(SQLiteConnection connection)
        //{
        //    using (var command = new SQLiteCommand(connection))
        //    {

        //        command.CommandText = "DROP TABLE IF EXISTS cpumetrics";

        //        command.ExecuteNonQuery();

        //        command.CommandText = @"CREATE TABLE cpumetrics(id INTEGER PRIMARY KEY,
        //            value INT, time INT)";
        //        command.ExecuteNonQuery();

        //        command.CommandText = "DROP TABLE IF EXISTS dotnetmetrics";

        //        command.ExecuteNonQuery();

        //        command.CommandText = @"CREATE TABLE dotnetmetrics(id INTEGER PRIMARY KEY,
        //            value INT, time INT)";

        //        command.ExecuteNonQuery();


        //        command.CommandText = "DROP TABLE IF EXISTS hddmetrics";

        //        command.ExecuteNonQuery();

        //        command.CommandText = @"CREATE TABLE hddmetrics(id INTEGER PRIMARY KEY,
        //            value INT, time INT)";

        //        command.ExecuteNonQuery();

        //        command.CommandText = "DROP TABLE IF EXISTS networkmetrics";

        //        command.ExecuteNonQuery();

        //        command.CommandText = @"CREATE TABLE networkmetrics(id INTEGER PRIMARY KEY,
        //            value INT, time INT)";

        //        command.ExecuteNonQuery();


        //        command.CommandText = "DROP TABLE IF EXISTS rammetrics";

        //        command.ExecuteNonQuery();

        //        command.CommandText = @"CREATE TABLE rammetrics(id INTEGER PRIMARY KEY,
        //            value INT, time INT)";

        //        command.ExecuteNonQuery();
        //    }
        //}
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMigrationRunner migrationRunner)
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

            migrationRunner.MigrateUp();
        }
    }
}
