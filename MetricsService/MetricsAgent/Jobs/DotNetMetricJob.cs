using MetricsAgent.DAL;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MetricsAgent.DAL.Repositories;
using MetricsAgent.DAL.MetricsClasses;

namespace MetricsAgent.Jobs
{
    public class DotNetMetricJob : IJob
    {
        private IDotNetMetricsRepository _repository;

        private PerformanceCounter _dotnetCounter;

        public DotNetMetricJob(IDotNetMetricsRepository repository)
        {
            _repository = repository;
            _dotnetCounter = new PerformanceCounter(".NET CLR Memory", "# Bytes in all heaps", "_Global_");
        }

        public Task Execute(IJobExecutionContext context)
        {
            // получаем значение занятости CPU
            var metricVal = Convert.ToInt32(_dotnetCounter.NextValue());

            // узнаем когда мы сняли значение метрики.
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            // теперь можно записать что-то при помощи репозитория

            _repository.Create(new DotNetMetric { Time = time, Value = metricVal });


            // теперь можно записать что-то при помощи репозитория

            return Task.CompletedTask;
        }


    }
}
