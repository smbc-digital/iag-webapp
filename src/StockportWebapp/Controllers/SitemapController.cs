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
        private readonly IRepository _repository;

        public SitemapController(IRepository repository)
        {
            _repository = repository;
        }

        [Route("/google-sitemap.xml")]
        public async Task<IActionResult> Sitemap(string type)
        {
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
                                    lastmod = DateTime.Now,
                                    loc = "www.stockport.gov.uk/news/" + n.Slug,
                                    priority = "1.0"
                                }).ToList();

                    xml = SerializeObject(listOfSitemaps);
                    break;

                case "events":
                    var queriesEvent = new List<Query>();
                    queriesEvent.Add(new Query("DateFrom", DateTime.MinValue.ToString("yyyy-MM-dd")));
                    queriesEvent.Add(new Query("DateTo", DateTime.Now.ToString("yyyy-MM-dd")));

                    var responseEvent = await _repository.Get<EventCalendar>(queries: queriesEvent);
                    var events = responseEvent.Content as EventCalendar;
                    var listOfSitemapsEvents =
                        events.Events.Select(e => e.Slug).Distinct().Select(
                            slug =>
                                new SitemapGoogle()
                                {
                                    changefreq = "daily",
                                    lastmod = DateTime.Now,
                                    loc = "www.stockport.gov.uk/events/" + slug,
                                    priority = "1.0"
                                }).ToList();

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
                                    lastmod = DateTime.Now,
                                    loc = "www.stockport.gov.uk/" + slug,
                                    priority = "1.0"
                                }).ToList();

                    xml = SerializeObject(listOfSitemapsArticles);
                    break;
            }

            return this.Content(xml, "text/xml");
        }


        private string SerializeObject<T>(T dataToSerialize)
        {
            string xml;
            XmlSerializer xsSubmit = new XmlSerializer(typeof(List<SitemapGoogle>), new XmlRootAttribute("urlset"));

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

    [XmlType("url")]
    public class SitemapGoogle
    {
        public string loc { get; set; }
        public DateTime lastmod { get; set; }
        public string changefreq { get; set; }
        public string priority { get; set; }

        public SitemapGoogle()
        {
                
        }
    }
}