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
    public class CpuMetricJob : IJob
    {
        private ICpuMetricsRepository _repository;

        private PerformanceCounter _cpuCounter;

        public CpuMetricJob(ICpuMetricsRepository repository)
        {
            _repository = repository;
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        }

        public Task Execute(IJobExecutionContext context)
        {
            // получаем значение занятости CPU
            var metricVal = Convert.ToInt32(_cpuCounter.NextValue());

            // узнаем когда мы сняли значение метрики.
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            // теперь можно записать что-то при помощи репозитория

            _repository.Create(new CpuMetric { Time = time, Value = metricVal });


            // теперь можно записать что-то при помощи репозитория

            return Task.CompletedTask;
        }


    }
}
