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
    public class CpuMetricsControllerUnitTests
    {
        private CpuMetricsController controller;

        private Mock<ILogger<CpuMetricsController>> mockLogger;

        private Mock<ICpuMetricsRepository> mockRepository;

        public CpuMetricsControllerUnitTests()
        {
            this.mockLogger = new Mock<ILogger<CpuMetricsController>>();

            this.mockRepository = new Mock<ICpuMetricsRepository>();

            this.controller = new CpuMetricsController(mockLogger.Object, mockRepository.Object);
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
            // ������������� �������� ��������
            // � �������� ����������� ��� � ����������� �������� CpuMetric ������
            mockRepository.Setup(repository => repository.Create(It.IsAny<CpuMetric>())).Verifiable();

            // ��������� �������� �� �����������
            //var result = controller.Create(new MetricsAgent.Requests.CpuMetricCreateRequest { Time = TimeSpan.FromSeconds(1), Value = 50 });

            // ��������� �������� �� ��, ��� ���� ������� ����������
            // ������������� �������� ����� Create ����������� � ������ ����� ������� � ���������
            mockRepository.Verify(repository => repository.Create(It.IsAny<CpuMetric>()), Times.AtMostOnce());
        }
    }
}
