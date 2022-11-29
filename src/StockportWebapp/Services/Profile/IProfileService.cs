using StockportWebapp.Services.Profile.Entities;

namespace StockportWebapp.Services.Profile
{
    public interface IProfileService
    {
        Task<ProfileEntity> GetProfile(string slug);
    }
}