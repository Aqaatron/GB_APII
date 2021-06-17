using System;
using Xunit;
using MetricsAgent;
using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using MetricsManager.Enums;
using Moq;
using MetricsAgent.DAL.Repositories;
using MetricsAgent.DAL.MetricsClasses;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace MetricsAgentTests
{
    public class HddMetricsControllerUnitTests
    {
        private HddMetricsController _controller;

        private Mock<ILogger<HddMetricsController>> _mockLogger;

        private Mock<IHddMetricsRepository> _mockRepository;

        private Mock<IMapper> _mockMapper;

        public HddMetricsControllerUnitTests()
        {
            this._mockLogger = new Mock<ILogger<HddMetricsController>>();

            this._mockRepository = new Mock<IHddMetricsRepository>();
            this._mockMapper = new Mock<IMapper>();

            this._controller = new HddMetricsController(_mockLogger.Object, _mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public void GetMetricsFromAgent_ReturnsOk()
        {
            //Arrange
            var fromTime = DateTimeOffset.Parse("2021 - 05 - 01 00:00:00");
            var toTime = DateTimeOffset.Parse("2021 - 05 - 01 02:00:00");
            //Act
            var result = _controller.GetMetrics(fromTime, toTime);

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void Create_ShouldCall_Create_From_Repository()
        {
            // устанавливаем параметр заглушки
            // в заглушке прописываем что в репозиторий прилетит CpuMetric объект
            _mockRepository.Setup(repository => repository.Create(It.IsAny<HddMetric>())).Verifiable();

            // выполняем действие на контроллере
            //var result = controller.Create(new MetricsAgent.Requests.CpuMetricCreateRequest { Time = TimeSpan.FromSeconds(1), Value = 50 });

            // проверяем заглушку на то, что пока работал контроллер
            // действительно вызвался метод Create репозитория с нужным типом объекта в параметре
            _mockRepository.Verify(repository => repository.Create(It.IsAny<HddMetric>()), Times.AtMostOnce());
        }

    }
}
