namespace StockportWebappTests_Unit.Unit.Controllers;

public class ProfileControllerTest
{
    private readonly Mock<IProfileService> _profileService = new();
    private readonly ProfileController _profileController;

    public ProfileControllerTest() =>
        _profileController = new(_profileService.Object);

    [Fact]
    public async Task GetProfile_ReturnsAProfileWithProcessedBody()
    {
        // Arrange
        Profile profile = new()
        {
            Title = "test"
        };

        _profileService
            .Setup(service => service.GetProfile(It.IsAny<string>()))
            .ReturnsAsync(profile);

        // Act
        ViewResult view = await _profileController.Index("slug") as ViewResult;
        ProfileViewModel model = view.ViewData.Model as ProfileViewModel;

        // Assert
        Assert.Equal(profile.Title, model.Profile.Title);
    }

    [Fact]
    public async Task GetProfile_ReturnsNotFound()
    {
        // Arrange
        _profileService
            .Setup(service => service.GetProfile(It.IsAny<string>()))
            .ReturnsAsync((Profile)null);

        // Act
        StatusCodeResult result = await _profileController.Index("slug") as StatusCodeResult;

        // Assert
        Assert.Equal(404, result.StatusCode);
    }
}