namespace StockportWebappTests_Unit.Unit.Controllers;

public class TPOControllerTests
{
	private readonly TPOController _controller;
	private readonly Mock<ITPOService> _mockTPOService = new();
	private readonly Mock<IFeatureManager> _featureManager = new();

	public TPOControllerTests()
	{
		_featureManager
			.Setup(manager => manager.IsEnabledAsync("TPOPage"))
			.Returns(Task.FromResult(true));

		_controller = new TPOController(_mockTPOService.Object,
										_featureManager.Object);
	}

	[Fact]
	public async Task Detail_ReturnsNotFound_WhenTPODoesNotExist()
	{
		// Arrange
		_mockTPOService
			.Setup(service => service.GetTPODataByID(It.IsAny<string>()))
			.ReturnsAsync((TPOItem)null);

		// Act
		IActionResult result = await _controller.Detail("non-existent-shed");

		// Assert
		Assert.IsType<NotFoundResult>(result);
	}

	[Fact]
	public async Task Detail_ReturnsViewResult_WithTPOItem()
	{
		// Arrange
		TPOItem TPOItem = new() { Tpo_name = "Existing TPO" };
		_mockTPOService
			.Setup(service => service.GetTPODataByID(It.IsAny<string>()))
			.ReturnsAsync(TPOItem);

		// Act
		IActionResult result = await _controller.Detail("existing-tpo");

		// Assert
		ViewResult viewResult = Assert.IsType<ViewResult>(result);
		Assert.Equal(TPOItem, viewResult.Model);
	}
}