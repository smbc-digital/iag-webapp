using System.Threading.Tasks;
using StockportWebapp.Repositories;
using StockportWebapp.Services.Profile;
using StockportWebapp.Services.Profile.Entities;

namespace StockportWebapp.Services.Profile
{
    public class ProfileService : IProfileService
    {
        private readonly IRepository _repository;

        public ProfileService(IRepository repository)
        {
            _repository = repository;
        }
        public async Task<ProfileEntity> GetProfile(string slug)
        {
            var response = await _repository.Get<ProfileEntity>(slug);

            if (response.StatusCode == 200)
            {
                return response.Content as ProfileEntity;
            }

            return null;
        }
    }
}