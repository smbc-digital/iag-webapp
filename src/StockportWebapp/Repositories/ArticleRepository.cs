namespace StockportWebapp.Repositories;

public interface IArticleRepository
{
    Task<HttpResponse> Get(string slug = "", string SearchTerm = "", string SearchFolder = "", string currentUrl = "");
}

public class ArticleRepository : IArticleRepository
{
    private readonly ArticleFactory _articleFactory;
    private readonly IHttpClient _httpClient;
    private readonly IStubToUrlConverter _urlGenerator;

    private static string BucketName = "live-iag-static-assets";
    private static string ServiceUrl = "s3-eu-west-1.amazonaws.com";
    public static int Count = 0;
    private readonly Dictionary<string, string> authenticationHeaders;
    private readonly IApplicationConfiguration _config;

    public ArticleRepository(IStubToUrlConverter urlGenerator, IHttpClient httpClient, ArticleFactory articleFactory, IApplicationConfiguration config)
    {
        _urlGenerator = urlGenerator;
        _httpClient = httpClient;
        _articleFactory = articleFactory;
        _config = config;
        authenticationHeaders = new Dictionary<string, string> { { "Authorization", _config.GetContentApiAuthenticationKey() }, { "X-ClientId", _config.GetWebAppClientId() } };
    }

    public async Task<HttpResponse> Get(string slug = "", string searchTerm = "", string searchFolder = "", string currentUrl = "")
    {
        var url = _urlGenerator.UrlFor<Article>(slug);
        var httpResponse = await _httpClient.Get(url, authenticationHeaders);

        if (!httpResponse.IsSuccessful())
        {
            return httpResponse;
        }

        var model = HttpResponse.Build<Article>(httpResponse);
        var article = (Article)model.Content;
        var bucket = new S3BucketSearch();
        bucket.Files = new List<string>();
        bucket.Folders = new List<string>();
        bucket.Slug = slug;
        bucket.SearchTerm = searchTerm;
        bucket.SearchFolder = searchFolder;
        bucket.AWSLink = ServiceUrl;
        bucket.S3Bucket = BucketName;
        bucket.CurrentUrl = currentUrl;
        if (!string.IsNullOrEmpty(searchTerm) && !string.IsNullOrEmpty(searchFolder))
        {
            bucket.Files = await ListFilesIn(searchFolder, searchTerm);
        }
        article.S3Bucket = bucket;

        var processedModel = _articleFactory.Build(article);

        return HttpResponse.Successful(200, processedModel);
    }

    public static async Task<List<string>> ListFilesIn(string folder, string searchTerm)
    {
        var settings = new { S3ServiceUrl = ServiceUrl, S3SecretKey = "", S3KeyId = "", S3BucketName = BucketName };

        var amazonS3Config = new AmazonS3Config
        {
            ServiceURL = string.Format("https://{0}", settings.S3ServiceUrl)
        };

        var fullpathfolder = "pdf/";

        if (!string.IsNullOrEmpty(folder))
        {
            fullpathfolder = folder;
        }

        using (var amazonS3Client = new AmazonS3Client(settings.S3KeyId, settings.S3SecretKey, amazonS3Config))
        {
            var response = await amazonS3Client.ListObjectsAsync(new ListObjectsRequest
            {
                BucketName = settings.S3BucketName,
                Prefix = fullpathfolder
            });

            if (response.S3Objects.Count > 0)  // if (response.S3Objects.Count() > 0)
            {
                var files = response.S3Objects.Select(s => s.Key).Where(w => w != fullpathfolder).ToList();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    List<string> temp = new List<string>();
                    foreach (var item in files)
                    {
                        var tempSplit = item.Split('/');
                        if (tempSplit.Last().ToLower().Contains(searchTerm.ToLower()))
                        {
                            temp.Add(item);
                        }
                    }
                    return temp;
                }
            }

            return new List<string>();
        }
    }
}
