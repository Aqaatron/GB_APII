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

namespace MetricsAgentTests
{
    public class RamMetricsControllerUnitTests
    {
        private RamMetricsController controller;

        private Mock<ILogger<RamMetricsController>> mockLogger;

        private Mock<IRamMetricsRepository> mockRepository;

        public RamMetricsControllerUnitTests()
        {
            this.mockLogger = new Mock<ILogger<RamMetricsController>>();

            this.mockRepository = new Mock<IRamMetricsRepository>();

            this.controller = new RamMetricsController(mockLogger.Object, mockRepository.Object);
        }

        [Fact]
        public void GetMetricsFromAgent_ReturnsOk()
        {
            //Arrange
            var fromTime = DateTimeOffset.Parse("2021 - 05 - 01 00:00:00");
            var toTime = DateTimeOffset.Parse("2021 - 05 - 01 02:00:00");
            //Act
            var result = controller.GetMetrics(fromTime, toTime);

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void Create_ShouldCall_Create_From_Repository()
        {
            // устанавливаем параметр заглушки
            // в заглушке прописываем что в репозиторий прилетит CpuMetric объект
            mockRepository.Setup(repository => repository.Create(It.IsAny<RamMetric>())).Verifiable();

            // выполняем действие на контроллере
            //var result = controller.Create(new MetricsAgent.Requests.CpuMetricCreateRequest { Time = TimeSpan.FromSeconds(1), Value = 50 });

            // проверяем заглушку на то, что пока работал контроллер
            // действительно вызвался метод Create репозитория с нужным типом объекта в параметре
            mockRepository.Verify(repository => repository.Create(It.IsAny<RamMetric>()), Times.AtMostOnce());
        }
    }
}
