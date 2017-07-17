using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockportWebapp.Exceptions;
using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.Repositories;
using StockportWebapp.ViewModels;
using StockportWebapp.Http;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Http;
using StockportWebapp.ProcessedModels;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Short)]
    public class SitemapController : Controller
    {
        // private const string _baseURL = "https://www.stockport.gov.uk";
        private const string _baseURL = "http://stockportgov.local:5555";
        private readonly IRepository _repository;

        public SitemapController(IRepository repository)
        {
            _repository = repository;
        }

        [Route("/google-sitemap.xml")]
        public async Task<IActionResult> Sitemap(string type)
        {
            var now = DateTime.Now;
            var xml = "";
            switch (type)
            {
                case "news":
                    var queries = new List<Query>();
                    queries.Add(new Query("DateFrom", DateTime.MinValue.ToString("yyyy-MM-dd")));
                    queries.Add(new Query("DateTo", DateTime.Now.ToString("yyyy-MM-dd")));

                    var response = await _repository.Get<Newsroom>(queries: queries);
                    var news = response.Content as Newsroom;
                    var listOfSitemaps =
                        news.News.Select(
                            n =>
                                new SitemapGoogle()
                                {
                                    changefreq = "daily",
                                    lastmod = now,
                                    loc = $"{_baseURL}/news/{n.Slug}",
                                    priority = "1.0"
                                }).ToList();

                    listOfSitemaps.Insert(0, new SitemapGoogle()
                                {
                                    changefreq = "weekly",
                                    lastmod = now,
                                    loc = $"{_baseURL}/news",
                                    priority = "1.0"
                                });

                    xml = SerializeObject(listOfSitemaps);
                    break;

                case "events":
                    var queriesEvent = new List<Query>();
                    queriesEvent.Add(new Query("DateFrom", DateTime.MinValue.ToString("yyyy-MM-dd")));
                    queriesEvent.Add(new Query("DateTo", DateTime.MaxValue.ToString("yyyy-MM-dd")));

                    var responseEvent = await _repository.Get<EventCalendar>(queries: queriesEvent);
                    var events = responseEvent.Content as EventCalendar;
                    var listOfSitemapsEvents =
                        events.Events.Select(e => e.Slug).Distinct().Select(
                            slug =>
                                new SitemapGoogle()
                                {
                                    changefreq = "daily",
                                    lastmod = now,
                                    loc = $"{_baseURL}/events/{slug}",
                                    priority = "1.0"
                                }).ToList();

                    listOfSitemapsEvents.Insert(0, new SitemapGoogle()
                    {
                        changefreq = "weekly",
                        lastmod = now,
                        loc = $"{_baseURL}/events",
                        priority = "1.0"
                    });

                    xml = SerializeObject(listOfSitemapsEvents);
                    break;

                case "article":
                    var responseArticle = await _repository.Get<List<ArticleSiteMap>>();
                    var articles = responseArticle.Content as List<ArticleSiteMap>;
                    var listOfSitemapsArticles =
                        articles.Select(e => e.Slug).Distinct().Select(
                            slug =>
                                new SitemapGoogle()
                                {
                                    changefreq = "daily",
                                    lastmod = now,
                                    loc = $"{_baseURL}/{slug}",
                                    priority = "1.0"
                                }).ToList();

                    xml = SerializeObject(listOfSitemapsArticles);
                    break;

                case "homepage":
                    var sitemapHomepage = new SitemapGoogle()
                    {
                        changefreq = "weekly",
                        lastmod = now,
                        loc = _baseURL,
                        priority = "0.5"
                    };

                    xml = SerializeObject(sitemapHomepage);
                    break;

                case "groups":
                    var responseGroups = await _repository.Get<List<Group>>();
                    var groups = responseGroups.Content as List<Group>;
                    var listOfGroupSitemaps =
                        groups.Select(
                            n =>
                                new SitemapGoogle()
                                {
                                    changefreq = "weekly",
                                    lastmod = now,
                                    loc = $"{_baseURL}/groups/{n.Slug}",
                                    priority = "0.5"
                                }).ToList();

                    listOfGroupSitemaps.Insert(0, new SitemapGoogle()
                                {
                                    changefreq = "weekly",
                                    lastmod = now,
                                    loc = $"{_baseURL}/groups",
                                    priority = "1.0"
                                });

                    xml = SerializeObject(listOfGroupSitemaps);
                    break;

                case "showcase":
                    var responseShowcases = await _repository.Get<List<Showcase>>();
                    var showcases = responseShowcases.Content as List<Showcase>;
                    var listOfShowcaseSitemaps =
                        showcases.Select(
                            n =>
                                new SitemapGoogle()
                                {
                                    changefreq = "weekly",
                                    lastmod = now,
                                    loc = $"{_baseURL}/showcase/{n.Slug}",
                                    priority = "0.5"
                                }).ToList();

                    xml = SerializeObject(listOfShowcaseSitemaps);
                    break;

                case "section":
                    var responseSections = await _repository.Get<List<SectionSiteMap>>();
                    var sections = responseSections.Content as List<SectionSiteMap>;
                    var listOfSectionSitemaps =
                        sections.Select(
                            n =>
                                new SitemapGoogle()
                                {
                                    changefreq = "daily",
                                    lastmod = now,
                                    loc = $"{_baseURL}/{n.Slug}",
                                    priority = "1"
                                }).ToList();

                    xml = SerializeObject(listOfSectionSitemaps);
                    break;

                case "topic":
                    var responseTopic = await _repository.Get<List<Topic>>();
                    var topics = responseTopic.Content as List<Topic>;
                    var listOfTopicSitemaps =
                        topics.Select(
                            n =>
                                new SitemapGoogle()
                                {
                                    changefreq = "daily",
                                    lastmod = now,
                                    loc = $"{_baseURL}/topic/{n.Slug}",
                                    priority = "1"
                                }).ToList();

                    xml = SerializeObject(listOfTopicSitemaps);
                    break;

                case "profile":
                    var responseProfiles = await _repository.Get<List<Profile>>();
                    var profiles = responseProfiles.Content as List<Profile>;
                    var listOfProfileSitemaps =
                        profiles.Select(
                            n =>
                                new SitemapGoogle()
                                {
                                    changefreq = "daily",
                                    lastmod = now,
                                    loc = $"{_baseURL}/profile/{n.Slug}",
                                    priority = "1"
                                }).ToList();

                    xml = SerializeObject(listOfProfileSitemaps);
                    break;

                case "payment":
                    var responsePayments = await _repository.Get<List<Payment>>();
                    var payments = responsePayments.Content as List<Payment>;
                    var listOfPaymentSitemaps =
                        payments.Select(
                            n =>
                                new SitemapGoogle()
                                {
                                    changefreq = "daily",
                                    lastmod = now,
                                    loc = $"{_baseURL}/payment/{n.Slug}",
                                    priority = "1"
                                }).ToList();

                    xml = SerializeObject(listOfPaymentSitemaps);
                    break;

                case "start":
                    var responseStarts = await _repository.Get<List<StartPage>>();
                    var starts = responseStarts.Content as List<StartPage>;
                    var listOfStartSitemaps =
                        starts.Select(
                            n =>
                                new SitemapGoogle()
                                {
                                    changefreq = "daily",
                                    lastmod = now,
                                    loc = $"{_baseURL}/start/{n.Slug}",
                                    priority = "1"
                                }).ToList();

                    xml = SerializeObject(listOfStartSitemaps);
                    break;

                default:

                    var result = new List<SitemapGoogleIndex>();
                    result.Add(new SitemapGoogleIndex { changefreq = "daily", lastmod = now, loc = $"{_baseURL}/google-sitemap.xml?type=news", priority = "1" });
                    result.Add(new SitemapGoogleIndex { changefreq = "daily", lastmod = now, loc = $"{_baseURL}/google-sitemap.xml?type=events", priority = "1" });
                    result.Add(new SitemapGoogleIndex { changefreq = "daily", lastmod = now, loc = $"{_baseURL}/google-sitemap.xml?type=article", priority = "1" });
                    result.Add(new SitemapGoogleIndex { changefreq = "daily", lastmod = now, loc = $"{_baseURL}/google-sitemap.xml?type=homepage", priority = "1" });
                    result.Add(new SitemapGoogleIndex { changefreq = "daily", lastmod = now, loc = $"{_baseURL}/google-sitemap.xml?type=groups", priority = "1" });
                    result.Add(new SitemapGoogleIndex { changefreq = "daily", lastmod = now, loc = $"{_baseURL}/google-sitemap.xml?type=showcase", priority = "1" });
                    result.Add(new SitemapGoogleIndex { changefreq = "daily", lastmod = now, loc = $"{_baseURL}/google-sitemap.xml?type=section", priority = "1" });
                    result.Add(new SitemapGoogleIndex { changefreq = "daily", lastmod = now, loc = $"{_baseURL}/google-sitemap.xml?type=topic", priority = "1" });
                    result.Add(new SitemapGoogleIndex { changefreq = "daily", lastmod = now, loc = $"{_baseURL}/google-sitemap.xml?type=profile", priority = "1" });
                    result.Add(new SitemapGoogleIndex { changefreq = "daily", lastmod = now, loc = $"{_baseURL}/google-sitemap.xml?type=payment", priority = "1" });
                    result.Add(new SitemapGoogleIndex { changefreq = "daily", lastmod = now, loc = $"{_baseURL}/google-sitemap.xml?type=start", priority = "1" });

                    xml = SerializeObject(result, true);
                    break;
            }

            return this.Content(xml, "text/xml");
        }


        private string SerializeObject<T>(T dataToSerialize, bool indexPage = false)
        {
            var xml = string.Empty;
            var attribute = indexPage ? "sitemapindex" : "urlset";
            var xsSubmit = new XmlSerializer(typeof(T), null, null, new XmlRootAttribute(attribute), "http://www.sitemaps.org/schemas/sitemap/0.9");

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, dataToSerialize);
                    xml = sww.ToString(); // Your XML
                }
            }

            return xml;
        }
    }

    [XmlType("sitemap")]
    public class SitemapGoogleIndex
    {
        public string loc { get; set; }
        public DateTime lastmod { get; set; }
        public string changefreq { get; set; }
        public string priority { get; set; }

        public SitemapGoogleIndex()
        {

        }
    }

    [XmlType("url")]
    public class SitemapGoogle : SitemapGoogleIndex
    {
        public SitemapGoogle()
        {

        }
    }
}