using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using MetricsAgent.DAL.Repositories;
using MetricsAgent.DAL.MetricsClasses;
using System.Diagnostics;

namespace MetricsAgent.Jobs
{
    public class HddMetricJob : IJob
    {
        private IHddMetricsRepository _repository;

        private PerformanceCounter _hddCounter;

        public HddMetricJob(IHddMetricsRepository repository)
        {
            _repository = repository;
            _hddCounter = new PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");
        }

        public Task Execute(IJobExecutionContext context)
        {
            // получаем значение занятости CPU
            var metricVal = Convert.ToInt32(_hddCounter.NextValue());

            // узнаем когда мы сняли значение метрики.
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            // теперь можно записать что-то при помощи репозитория

            _repository.Create(new HddMetric { Time = time, Value = metricVal });


            // теперь можно записать что-то при помощи репозитория

            return Task.CompletedTask;
        }


    }
}
