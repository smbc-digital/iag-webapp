using Moq;
using StockportWebapp.Controllers;
using StockportWebapp.Services;
using Xunit;

namespace StockportWebappTests.Unit.Controllers
{
    public class HealthcheckControllerTest
    {
        private readonly Mock<IHealthcheckService> _healthCheckRepository;

        public HealthcheckControllerTest()
        {
            _healthCheckRepository = new Mock<IHealthcheckService>();
        }

        [Fact]
        public void ShouldGetRepositoryToReturnHealth()
        {
            var healthcheckController = new HealthcheckController(_healthCheckRepository.Object);

            AsyncTestHelper.Resolve(healthcheckController.Index());

            _healthCheckRepository.Verify(x=>x.Get(),Times.Once);
        }
    }
}
