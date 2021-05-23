using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetricsAgent.MetricsClasses;

namespace MetricsAgent.Response
{
    public class AllCpuMetricsResponse
    {
        public List<CpuMetric> Metrics { get; set; }

    }

}
