﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.DAL.Requests
{
    public class NetworkMetricCreateRequest
    {
        public long Time { get; set; }
        public int Value { get; set; }
    }
}