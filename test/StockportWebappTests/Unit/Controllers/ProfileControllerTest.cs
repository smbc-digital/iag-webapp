namespace StockportWebappTests_Unit.Unit.Controllers;

public class ProfileControllerTest
{
    private readonly Mock<IProcessedContentRepository> _fakeRepository = new Mock<IProcessedContentRepository>();
    private readonly Mock<IProfileService> _profileService = new Mock<IProfileService>();
    private readonly ProfileController _profileController;


    public ProfileControllerTest()
    {
        _profileController = new ProfileController(_fakeRepository.Object, _profileService.Object);
    }

    [Fact]
    public async Task ItReturnsAProfileWithProcessedBody()
    {
        var profileEntity = new ProfileEntity
        {
            Title = "test"
        };

        _profileService
            .Setup(_ => _.GetProfile(It.IsAny<string>()))
            .ReturnsAsync(profileEntity);

        var view = await _profileController.Index("slug") as ViewResult;
        var model = view.ViewData.Model as Profile;

        model.Title.Should().Be(profileEntity.Title);
    }

    //[Fact]
    //public async Task GetsA404NotFoundProfile()
    //{
    //    _profileService
    //        .Setup(_ => _.GetProfile(It.IsAny<string>()))
    //        .ReturnsAsync(default(ProfileEntity));

    //    var response = await _profileController.Index("not-found-slug") as ViewResult;

    //    response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    //}
}