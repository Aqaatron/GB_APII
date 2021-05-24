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
    public class NetworkMetricJob : IJob
    {
        private INetworkMetricsRepository _repository;

        private PerformanceCounter _networkCounter;

        public NetworkMetricJob(INetworkMetricsRepository repository)
        {
            _repository = repository;
            PerformanceCounterCategory performanceCounterCategory = new PerformanceCounterCategory("Network Interface");
            string instance = performanceCounterCategory.GetInstanceNames()[0];
            _networkCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", instance);
        }

        public Task Execute(IJobExecutionContext context)
        {
            // получаем значение занятости CPU
            var metricVal = Convert.ToInt32(_networkCounter.NextValue());

            // узнаем когда мы сняли значение метрики.
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            // теперь можно записать что-то при помощи репозитория

            _repository.Create(new NetworkMetric { Time = time, Value = metricVal });


            // теперь можно записать что-то при помощи репозитория

            return Task.CompletedTask;
        }


    }
}
