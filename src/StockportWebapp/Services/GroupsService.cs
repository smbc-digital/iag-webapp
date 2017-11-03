using StockportWebapp.Models;
using StockportWebapp.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockportWebapp.Services
{
    public interface IGroupsService
    {
        Task<GroupHomepage> GetGroupHomepage();
        Task<List<GroupCategory>> GetGroupCategories();
    }

    public class GroupsService : IGroupsService
    {
        private IContentApiRepository _contentApiRepository;

        public GroupsService(IContentApiRepository contentApiRepository)
        {
            _contentApiRepository = contentApiRepository;
        }

        public async Task<GroupHomepage> GetGroupHomepage()
        {
            return await _contentApiRepository.GetResponse<GroupHomepage>();
        }

        public async Task<List<GroupCategory>> GetGroupCategories()
        {
            return await _contentApiRepository.GetResponse<List<GroupCategory>>();
        }
    }
}
