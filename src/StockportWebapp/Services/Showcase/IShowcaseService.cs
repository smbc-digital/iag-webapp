using System.Threading.Tasks;
using StockportWebapp.Services.Showcase.Entities;

namespace StockportWebapp.Services.Showcase
{
    public interface IShowcaseService
    {
        Task<ShowcaseEntity> GetShowcase(string slug);
    }
}
