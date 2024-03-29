﻿namespace StockportWebappTests_Unit.Unit.Controllers;

public class HealthcheckControllerTest
{
    private readonly Mock<IHealthcheckService> _healthCheckRepository;

    public HealthcheckControllerTest()
    {
        _healthCheckRepository = new Mock<IHealthcheckService>();
    }

    [Fact]
    public async Task ShouldGetRepositoryToReturnHealth()
    {
        var healthcheckController = new HealthcheckController(_healthCheckRepository.Object);

        await healthcheckController.Index();

        _healthCheckRepository.Verify(x => x.Get(), Times.Once);
    }
}
