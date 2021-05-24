using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using MetricsAgent.DAL.Repositories;

namespace MetricsAgent.Jobs
{
    public class HddMetricJob : IJob
    {
        private ICpuMetricsRepository _repository;

        public HddMetricJob(ICpuMetricsRepository repository)
        {
            _repository = repository;
        }

        public Task Execute(IJobExecutionContext context)
        {
            // теперь можно записать что-то при помощи репозитория

            return Task.CompletedTask;
        }
    }
}
