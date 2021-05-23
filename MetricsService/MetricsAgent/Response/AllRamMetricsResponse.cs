using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetricsAgent.MetricsClasses;

namespace MetricsAgent.Response
{
    public class AllRamMetricsResponse
    {
        public List<RamMetric> Metrics { get; set; }
    }
}
