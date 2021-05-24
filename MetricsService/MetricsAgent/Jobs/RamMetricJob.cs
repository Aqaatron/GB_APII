using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using MetricsAgent.DAL.Repositories;
using System.Diagnostics;
using MetricsAgent.DAL.MetricsClasses;

namespace MetricsAgent.Jobs
{
    public class RamMetricJob : IJob
    {
        private IRamMetricsRepository _repository;

        private PerformanceCounter _ramCounter;

        public RamMetricJob(IRamMetricsRepository repository)
        {
            _repository = repository;

            _ramCounter = new PerformanceCounter("Memory", "Available MBytes");

        }

        public Task Execute(IJobExecutionContext context)
        {
            var metricVal = Convert.ToInt32(_ramCounter.NextValue());

            // узнаем когда мы сняли значение метрики.
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            // теперь можно записать что-то при помощи репозитория

            _repository.Create(new RamMetric { Time = time, Value = metricVal });
            // теперь можно записать что-то при помощи репозитория

            return Task.CompletedTask;
        }
    }
}
