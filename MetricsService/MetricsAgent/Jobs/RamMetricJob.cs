﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using MetricsAgent.DAL.Repositories;

namespace MetricsAgent.Jobs
{
    public class RamMetricJob : IJob
    {
        private ICpuMetricsRepository _repository;

        public RamMetricJob(ICpuMetricsRepository repository)
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
