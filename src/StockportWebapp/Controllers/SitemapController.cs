using Profile = StockportWebapp.Models.Profile;

namespace StockportWebapp.Controllers;

[ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Short)]
public class SitemapController : Controller
{
    private readonly IRepository _repository;
    private readonly ILogger<SitemapController> _logger;

    public SitemapController(IRepository repository, ILogger<SitemapController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [Route("/sitemap.xml")]
    public async Task<IActionResult> Sitemap(string type)
    {
        string baseURL = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
        DateTime now = DateTime.Now;
        _logger.LogInformation(string.Concat("Hitting site map for: ", type));

        string xml = string.Empty;
        switch (type)
        {
            case "news":
                List<Query> queries = new();
                queries.Add(new Query("DateFrom", DateTime.MinValue.ToString("yyyy-MM-dd")));
                queries.Add(new Query("DateTo", now.ToString("yyyy-MM-dd")));

                HttpResponse response = await _repository.Get<Newsroom>(queries: queries);
                Newsroom news = response.Content as Newsroom;
                List<SitemapGoogle> listOfSitemaps =
                    news.News.Select(
                        n =>
                            new SitemapGoogle
                            {
                                changefreq = "daily",
                                lastmod = now.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                loc = $"{baseURL}/news/{n.Slug}",
                                priority = "1.0"
                            }).ToList();

                listOfSitemaps.Insert(0, new SitemapGoogle
                {
                    changefreq = "weekly",
                    lastmod = now.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                    loc = $"{baseURL}/news",
                    priority = "1.0"
                });

                xml = SerializeObject(listOfSitemaps);
                break;

            case "events":
                List<Query> queriesEvent = new List<Query>();
                queriesEvent.Add(new Query("DateFrom", DateTime.MinValue.ToString("yyyy-MM-dd")));
                queriesEvent.Add(new Query("DateTo", DateTime.MaxValue.ToString("yyyy-MM-dd")));

                HttpResponse responseEvent = await _repository.Get<EventCalendarViewModel>(queries: queriesEvent);
                EventCalendarViewModel events = responseEvent.Content as EventCalendarViewModel;
                List<SitemapGoogle> listOfSitemapsEvents =
                    events.Events.Select(e => e.Slug).Distinct().Select(
                        slug =>
                            new SitemapGoogle()
                            {
                                changefreq = "daily",
                                lastmod = now.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                loc = $"{baseURL}/events/{slug}",
                                priority = "1.0"
                            }).ToList();

                listOfSitemapsEvents.Insert(0, new SitemapGoogle()
                {
                    changefreq = "weekly",
                    lastmod = now.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                    loc = $"{baseURL}/events",
                    priority = "1.0"
                });

                xml = SerializeObject(listOfSitemapsEvents);
                break;

            case "article":
                HttpResponse responseArticle = await _repository.Get<List<ArticleSiteMap>>();
                List<ArticleSiteMap> articles = responseArticle.Content as List<ArticleSiteMap>;
                List<SitemapGoogle> listOfSitemapsArticles =
                    articles.Select(e => e.Slug).Distinct().Select(
                        slug =>
                            new SitemapGoogle()
                            {
                                changefreq = "daily",
                                lastmod = now.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                loc = $"{baseURL}/{slug}",
                                priority = "1.0"
                            }).ToList();

                xml = SerializeObject(listOfSitemapsArticles);
                break;

            case "homepage":
                SitemapGoogle sitemapHomepage = new SitemapGoogle()
                {
                    changefreq = "weekly",
                    lastmod = now.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                    loc = baseURL,
                    priority = "0.5"
                };

                List<SitemapGoogle> list = new List<SitemapGoogle>();
                list.Add(sitemapHomepage);

                xml = SerializeObject(list);
                break;

            case "groups":
                HttpResponse responseGroups = await _repository.Get<List<Group>>();
                List<Group> groups = responseGroups.Content as List<Group>;
                List<SitemapGoogle> listOfGroupSitemaps =
                    groups.Select(
                        n =>
                            new SitemapGoogle()
                            {
                                changefreq = "weekly",
                                lastmod = now.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                loc = $"{baseURL}/groups/{n.Slug}",
                                priority = "0.5"
                            }).ToList();

                listOfGroupSitemaps.Insert(0, new SitemapGoogle()
                {
                    changefreq = "weekly",
                    lastmod = now.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                    loc = $"{baseURL}/groups",
                    priority = "0.5"
                });

                xml = SerializeObject(listOfGroupSitemaps);
                break;

            case "showcase":
                HttpResponse responseShowcases = await _repository.Get<List<Showcase>>();
                List<Showcase> showcases = responseShowcases.Content as List<Showcase>;
                List<SitemapGoogle> listOfShowcaseSitemaps =
                    showcases.Select(
                        n =>
                            new SitemapGoogle()
                            {
                                changefreq = "weekly",
                                lastmod = now.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                loc = $"{baseURL}/showcase/{n.Slug}",
                                priority = "0.5"
                            }).ToList();

                xml = SerializeObject(listOfShowcaseSitemaps);
                break;

            case "section":
                HttpResponse responseSections = await _repository.Get<List<SectionSiteMap>>();
                List<SectionSiteMap> sections = responseSections.Content as List<SectionSiteMap>;
                List<SitemapGoogle> listOfSectionSitemaps =
                    sections.Select(
                        n =>
                            new SitemapGoogle()
                            {
                                changefreq = "daily",
                                lastmod = now.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                loc = $"{baseURL}/{n.Slug}",
                                priority = "1.0"
                            }).ToList();

                xml = SerializeObject(listOfSectionSitemaps);
                break;

            case "topic":
                HttpResponse responseTopic = await _repository.Get<List<TopicSitemap>>();
                List<TopicSitemap> topics = responseTopic.Content as List<TopicSitemap>;
                List<SitemapGoogle> listOfTopicSitemaps =
                    topics.Select(
                        n =>
                            new SitemapGoogle()
                            {
                                changefreq = "weekly",
                                lastmod = now.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                loc = $"{baseURL}/topic/{n.Slug}",
                                priority = "0.5"
                            }).ToList();

                xml = SerializeObject(listOfTopicSitemaps);
                break;

            case "profile":
                HttpResponse responseProfiles = await _repository.Get<List<Models.Profile>>();
                List<Profile> profiles = responseProfiles.Content as List<Models.Profile>;
                List<SitemapGoogle> listOfProfileSitemaps =
                    profiles.Select(
                        n =>
                            new SitemapGoogle()
                            {
                                changefreq = "monthly",
                                lastmod = now.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                loc = $"{baseURL}/profile/{n.Slug}",
                                priority = "0.5"
                            }).ToList();

                xml = SerializeObject(listOfProfileSitemaps);
                break;

            case "payment":
                HttpResponse responsePayments = await _repository.Get<List<Payment>>();
                List<Payment> payments = responsePayments.Content as List<Payment>;
                List<SitemapGoogle> listOfPaymentSitemaps =
                    payments.Select(
                        n =>
                            new SitemapGoogle()
                            {
                                changefreq = "monthly",
                                lastmod = now.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                loc = $"{baseURL}/payment/{n.Slug}",
                                priority = "0.5"
                            }).ToList();

                xml = SerializeObject(listOfPaymentSitemaps);
                break;

            case "start":
                HttpResponse responseStarts = await _repository.Get<List<StartPage>>();
                List<StartPage> starts = responseStarts.Content as List<StartPage>;
                List<SitemapGoogle> listOfStartSitemaps =
                    starts.Select(
                        n =>
                            new SitemapGoogle()
                            {
                                changefreq = "monthly",
                                lastmod = now.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                                loc = $"{baseURL}/start/{n.Slug}",
                                priority = "0.5"
                            }).ToList();

                xml = SerializeObject(listOfStartSitemaps);
                break;

            default:

                List<SitemapGoogleIndex> result = new List<SitemapGoogleIndex>();
                result.Add(new SitemapGoogleIndex { lastmod = now.ToString("yyyy-MM-ddTHH:mm:sszzz"), loc = $"{baseURL}/google-sitemap.xml?type=news" });
                result.Add(new SitemapGoogleIndex { lastmod = now.ToString("yyyy-MM-ddTHH:mm:sszzz"), loc = $"{baseURL}/google-sitemap.xml?type=events" });
                result.Add(new SitemapGoogleIndex { lastmod = now.ToString("yyyy-MM-ddTHH:mm:sszzz"), loc = $"{baseURL}/google-sitemap.xml?type=article" });
                result.Add(new SitemapGoogleIndex { lastmod = now.ToString("yyyy-MM-ddTHH:mm:sszzz"), loc = $"{baseURL}/google-sitemap.xml?type=homepage" });
                result.Add(new SitemapGoogleIndex { lastmod = now.ToString("yyyy-MM-ddTHH:mm:sszzz"), loc = $"{baseURL}/google-sitemap.xml?type=groups" });
                result.Add(new SitemapGoogleIndex { lastmod = now.ToString("yyyy-MM-ddTHH:mm:sszzz"), loc = $"{baseURL}/google-sitemap.xml?type=showcase" });
                result.Add(new SitemapGoogleIndex { lastmod = now.ToString("yyyy-MM-ddTHH:mm:sszzz"), loc = $"{baseURL}/google-sitemap.xml?type=section" });
                result.Add(new SitemapGoogleIndex { lastmod = now.ToString("yyyy-MM-ddTHH:mm:sszzz"), loc = $"{baseURL}/google-sitemap.xml?type=topic" });
                result.Add(new SitemapGoogleIndex { lastmod = now.ToString("yyyy-MM-ddTHH:mm:sszzz"), loc = $"{baseURL}/google-sitemap.xml?type=profile" });
                result.Add(new SitemapGoogleIndex { lastmod = now.ToString("yyyy-MM-ddTHH:mm:sszzz"), loc = $"{baseURL}/google-sitemap.xml?type=payment" });
                result.Add(new SitemapGoogleIndex { lastmod = now.ToString("yyyy-MM-ddTHH:mm:sszzz"), loc = $"{baseURL}/google-sitemap.xml?type=start" });

                xml = SerializeObject(result, true);
                break;
        }

        return this.Content(xml, "text/xml");
    }


    private string SerializeObject<T>(T dataToSerialize, bool indexPage = false)
    {
        string xml = string.Empty;
        string attribute = indexPage ? "sitemapindex" : "urlset";
        XmlSerializerNamespaces xmlSerializerNamespace = new XmlSerializerNamespaces();
        string ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
        xmlSerializerNamespace.Add(string.Empty, ns);
        XmlSerializer xsSubmit = new XmlSerializer(typeof(T), null, null, new XmlRootAttribute(attribute), ns);

        using (Utf8StringWriter sww = new Utf8StringWriter())
        {
            using (XmlWriter writer = XmlWriter.Create(sww))
            {
                xsSubmit.Serialize(writer, dataToSerialize, xmlSerializerNamespace);
                xml = sww.ToString(); // Your XML
            }
        }

        return xml;
    }
}

public class Utf8StringWriter : StringWriter
{
    public override Encoding Encoding
    {
        get { return new UTF8Encoding(false); }
    }
}

[XmlType("sitemap")]
public class SitemapGoogleIndex
{
    public string loc { get; set; }
    public string lastmod { get; set; }

    public SitemapGoogleIndex()
    {
    }
}

[XmlType("url")]
public class SitemapGoogle : SitemapGoogleIndex
{
    public string priority { get; set; }
    public string changefreq { get; set; }

    public SitemapGoogle()
    {
    }
}
