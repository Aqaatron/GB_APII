using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetricsManager.Enums;
using Microsoft.Extensions.Logging;
using System.Data.SQLite;
using MetricsAgent.DAL.Repositories;
using MetricsAgent.DAL.MetricsClasses;
using MetricsAgent.DAL.MetricsClassesDto;
using MetricsAgent.DAL.Response;
using MetricsAgent.DAL.Requests;
using AutoMapper;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/ram")]
    [ApiController]
    public class RamMetricsController : ControllerBase
    {
        private readonly ILogger<RamMetricsController> _logger;

        private readonly IRamMetricsRepository _repository;

        private readonly IMapper _mapper;
        public RamMetricsController(ILogger<RamMetricsController> logger, IRamMetricsRepository repository, IMapper mapper)
        {
            this._logger = logger;

            this._repository = repository;

            this._mapper = mapper;

            _logger.LogDebug(1, "NLog встроен в RamMetricsController");
        }

        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetrics([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            var metrics = _repository.GetByTimePeriod(fromTime, toTime);

            var response = new AllRamMetricsResponse()
            {
                Metrics = new List<RamMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<RamMetricDto>(metric));
            }

            return Ok(response);
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] CpuMetricCreateRequest request)
        {
            _repository.Create(new RamMetric
            {
                Time = request.Time,
                Value = request.Value
            });

            return Ok();
        }
    }
}
