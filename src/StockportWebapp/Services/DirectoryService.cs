namespace StockportWebapp.Services;

public interface IDirectoryService
{
}

public class DirectoryService : IDirectoryService
{
    private readonly IProcessedContentRepository _processedContentRepository;
    private readonly IStockportApiRepository _stockportApiRepository;
    private readonly IApplicationConfiguration _configuration;

    public DirectoryService
    (
        IProcessedContentRepository processedContentRepository,
        IApplicationConfiguration configuration,
        IStockportApiRepository stockportApiRepository
    )
    {
        _configuration = configuration;
        _stockportApiRepository = stockportApiRepository;
        _processedContentRepository = processedContentRepository;
    }

    public void DoPagination(GroupResults groupResults, int currentPageNumber, int pageSize)
    {
        if ((groupResults != null) && groupResults.Groups.Any())
        {
            var paginatedGroups = PaginationHelper.GetPaginatedItemsForSpecifiedPage(
                groupResults.Groups,
                currentPageNumber,
                "groups",
                pageSize,
                _configuration.GetGroupsDefaultPageSize("stockportgov"));

            groupResults.Groups = paginatedGroups.Items;
            groupResults.Pagination = paginatedGroups.Pagination;
            groupResults.Pagination.CurrentUrl = groupResults.CurrentUrl;
        }
        else
        {
            groupResults.Pagination = new Pagination();
        }
    }
}