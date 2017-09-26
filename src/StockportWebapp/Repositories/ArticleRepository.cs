using System.Threading.Tasks;
using StockportWebapp.Models;
using StockportWebapp.Http;
using System.Linq;
using System.Collections.Generic;
using Amazon.S3;
using Amazon.S3.Model;
using StockportWebapp.ContentFactory;
using StockportWebapp.Utils;
using StockportWebapp.Config;

namespace StockportWebapp.Repositories
{
    public interface IArticleRepository
    {
        Task<HttpResponse> Get(string slug = "", string SearchTerm = "", string SearchFolder = "");
    }

    public class ArticleRepository : IArticleRepository
    {
        private readonly ArticleFactory _articleFactory;
        private readonly IHttpClient _httpClient;
        private readonly IStubToUrlConverter _urlGenerator;

        private static string BucketName = "live-iag-static-assets";
        private static string ServiceUrl = "s3-eu-west-1.amazonaws.com";
        private static IAmazonS3 client;
        static string keyName = "";
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

        public async Task<HttpResponse> Get(string slug = "", string SearchTerm = "", string SearchFolder = "")
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
            //SearchTerm = "pdf";
            //SearchFolder = "areaA";
            if (!string.IsNullOrEmpty(SearchTerm) && !string.IsNullOrEmpty(SearchFolder))
            {
                bucket.Files = await ListFilesIn(SearchFolder, SearchTerm);
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
                            if (tempSplit.Last().Contains(searchTerm))
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
}
