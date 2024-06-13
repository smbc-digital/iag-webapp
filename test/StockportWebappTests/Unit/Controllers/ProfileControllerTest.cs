namespace StockportWebappTests_Unit.Unit.Controllers;

public class ProfileControllerTest
{
    private readonly Mock<IProcessedContentRepository> _fakeRepository = new();
    private readonly Mock<IProfileService> _profileService = new();
    private readonly ProfileController _profileController;


    public ProfileControllerTest()
    {
        _profileController = new ProfileController(_fakeRepository.Object, _profileService.Object);
    }

    [Fact]
    public async Task GetProfile_ReturnsAProfileWithProcessedBody()
    {
        var profile = new Profile
        {
            Title = "test"
        };

        _profileService
            .Setup(_ => _.GetProfile(It.IsAny<string>()))
            .ReturnsAsync(profile);

        var view = await _profileController.Index("slug") as ViewResult;
        var model = view.ViewData.Model as ProfileViewModel;
        Assert.Equal(profile.Title, model.Profile.Title);
    }
}