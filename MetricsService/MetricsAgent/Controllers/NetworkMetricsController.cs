using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetricsManager.Enums;
using Microsoft.Extensions.Logging;
using System.Data.SQLite;
using MetricsAgent.Repositories;
using MetricsAgent.MetricsClasses;
using MetricsAgent.Response;
using MetricsAgent.Requests;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/network")]
    [ApiController]
    public class NetworkMetricsController : ControllerBase
    {
        private readonly ILogger<NetworkMetricsController> _logger;

        private readonly INetworkMetricsRepository _repository;
        public NetworkMetricsController(ILogger<NetworkMetricsController> logger, INetworkMetricsRepository repository)
        {
            this._logger = logger;

            this._repository = repository;

            _logger.LogDebug(1, "NLog встроен в NetworkMetricsController");
        }

        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            _logger.LogInformation("Hello! This is my first message in logs!");
            return Ok();

        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] NetworkMetricCreateRequest request)
        {
            _repository.Create(new NetworkMetric
            {
                Time = request.Time,
                Value = request.Value
            });

            return Ok();
        }


        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var metrics = _repository.GetAll();

            var response = new AllNetworkMetricsResponse()
            {
                Metrics = new List<NetworkMetric>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new NetworkMetric { Time = metric.Time, Value = metric.Value, Id = metric.Id });
            }

            return Ok(response);
        }
    }
}
