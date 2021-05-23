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
    [Route("api/metrics/ram")]
    [ApiController]
    public class RamMetricsController : ControllerBase
    {
        private readonly ILogger<RamMetricsController> _logger;
        private readonly IRamMetricsRepository _repository;
        public RamMetricsController(ILogger<RamMetricsController> logger, IRamMetricsRepository repository)
        {
            this._logger = logger;

            this._repository = repository;

            _logger.LogDebug(1, "NLog встроен в RamMetricsController");
        }

        [HttpGet("available")]
        public IActionResult GetMetrics([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            _logger.LogInformation("Hello! This is my first message in logs!");
            return Ok();
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] RamMetricCreateRequest request)
        {
            _repository.Create(new RamMetric
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

            var response = new AllRamMetricsResponse()
            {
                Metrics = new List<RamMetric>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(new RamMetric { Time = metric.Time, Value = metric.Value, Id = metric.Id });
            }

            return Ok(response);
        }
    }
}
