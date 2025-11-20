namespace StockportWebapp.Services;

public interface ITPOService
{
	Task<TPOItem> GetTPODataByID(string heRef);
}

public class TPOService(ITPOApiClient tPOApiClient, MarkdownWrapper markdownWrapper) : ITPOService
{
	private readonly ITPOApiClient _tPOApiClient = tPOApiClient;
	private readonly MarkdownWrapper _markdownWrapper = markdownWrapper;

	public async Task<TPOItem> GetTPODataByID(string heRef)
	{
		string json = await _tPOApiClient.GetTPODataByID(heRef);

		if (string.IsNullOrEmpty(json))
			return null;

		TPOItem tPOItem = System.Text.Json.JsonSerializer.Deserialize<TPOItem>(json);

		//if (tPOItem is not null)
			//tPOItem.Tpo_name = _markdownWrapper.ConvertToHtml(tPOItem.Tpo_name ?? string.Empty);

		return tPOItem ?? null;
	}
}
