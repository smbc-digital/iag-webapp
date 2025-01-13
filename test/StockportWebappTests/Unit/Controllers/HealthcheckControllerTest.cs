namespace StockportWebappTests_Unit.Unit.Controllers;

public class HealthcheckControllerTest
{
    private readonly Mock<IHealthcheckService> _healthCheckRepository;

    public HealthcheckControllerTest() =>
        _healthCheckRepository = new Mock<IHealthcheckService>();

    [Fact]
    public async Task ShouldGetRepositoryToReturnHealth()
    {
        // Arrange
        HealthcheckController healthcheckController = new(_healthCheckRepository.Object);

        // Act
        await healthcheckController.Index();
        
        // Assert
        _healthCheckRepository.Verify(repo => repo.Get(), Times.Once);
    }
}