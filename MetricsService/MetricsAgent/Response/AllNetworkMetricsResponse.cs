using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetricsAgent.MetricsClasses;

namespace MetricsAgent.Response
{
    public class AllNetworkMetricsResponse
    {
        public List<NetworkMetric> Metrics { get; set; }
    }
}
