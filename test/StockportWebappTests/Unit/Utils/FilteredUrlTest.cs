using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using StockportWebapp.Utils;
using Xunit;

namespace StockportWebappTests.Unit.Utils
{
    public class FilteredUrlTest
    {
        [Fact]
        public void WillRemoveCategoryQueryParamFromUrl()
        {
            // Arrange
            var queryUrl = new QueryUrl(
                new RouteValueDictionary(), 
                new QueryCollection(
                    new Dictionary<string, StringValues>
                    {
                        {
                            "category",
                            new StringValues(new[] {"business"})
                        },
                        {
                            "tag",
                            new StringValues(new[] {"healthy"})
                        }
                    }
                )
                );
            var filteredUrl = new FilteredUrl(queryUrl);

            // Act
            var newQueryUrl = filteredUrl.WithoutCategory();

            // Assert
            Assert.Equal(false, newQueryUrl.ContainsKey("category"));
        }

        [Fact]
        public void WillAddCategoryFilterToUrl()
        {
            // Arrange
            var queryUrl = new QueryUrl(
                new RouteValueDictionary(),
                new QueryCollection(
                    new Dictionary<string, StringValues>
                    {                      
                        {
                            "tag",
                            new StringValues(new[] {"healthy"})
                        }
                    }
                )
                );
            var filteredUrl = new FilteredUrl(queryUrl);

            // Act
            var newQueryUrl = filteredUrl.AddCategoryFilter("business");

            // Assert
            Assert.Equal(true, newQueryUrl.ContainsKey("category"));
        }

        [Fact]
        public void WillIdentifyWhenCategoryFilterIsPresent()
        {
            // Arrange
            var queryUrl = new QueryUrl(
                new RouteValueDictionary(),
                new QueryCollection(
                    new Dictionary<string, StringValues>
                    {
                        {
                            "category",
                            new StringValues(new[] {"business"})
                        }
                    }
                )
                );
            var filteredUrl = new FilteredUrl(queryUrl);

            // Act
            bool hasNoCategoryfilter = filteredUrl.HasNoCategoryFilter();

            // Assert
            Assert.Equal(false, hasNoCategoryfilter);
        }

        [Fact]
        public void WillIdentifyWhenCategoryFilterIsNotPresent()
        {
            // Arrange
            var queryUrl = new QueryUrl(
                new RouteValueDictionary(),
                new QueryCollection(new Dictionary<string, StringValues>()
                ));
            var filteredUrl = new FilteredUrl(queryUrl);

            // Act
            bool hasNoCategoryfilter = filteredUrl.HasNoCategoryFilter();

            // Assert
            Assert.Equal(true, hasNoCategoryfilter);
        }

        [Fact]
        public void WillAddDateFromFilterToUrl()
        {
            // Arrange
            var queryUrl = new QueryUrl(
                new RouteValueDictionary(),
                new QueryCollection(
                    new Dictionary<string, StringValues>
                    {
                        {
                            "tag",
                            new StringValues(new[] {"healthy"})
                        }
                    }
                )
                );
            var filteredUrl = new FilteredUrl(queryUrl);
            DateTime startDate = DateTime.Today;

            // Act
            var newQueryUrl = filteredUrl.AddMonthFilter(startDate);

            // Assert
            Assert.Equal(true, newQueryUrl.ContainsKey("dateFrom"));
        }

        [Fact]
        public void WillAddDateToFilterToUrl()
        {
            // Arrange
            var queryUrl = new QueryUrl(
                new RouteValueDictionary(),
                new QueryCollection(
                    new Dictionary<string, StringValues>
                    {
                        {
                            "tag",
                            new StringValues(new[] {"healthy"})
                        }
                    }
                )
                );
            var filteredUrl = new FilteredUrl(queryUrl);
            DateTime startDate = DateTime.Today;

            // Act
            var newQueryUrl = filteredUrl.AddMonthFilter(startDate);

            // Assert
            Assert.Equal(true, newQueryUrl.ContainsKey("dateTo"));
        }

        [Fact]
        public void WillPopulateDateFromFilterInUrl()
        {
            // Arrange
            var queryUrl = new QueryUrl(
                new RouteValueDictionary(),
                new QueryCollection(
                    new Dictionary<string, StringValues>
                    {
                        {
                            "tag",
                            new StringValues(new[] {"healthy"})
                        }
                    }
                )
                );
            var filteredUrl = new FilteredUrl(queryUrl);
            DateTime startDate = DateTime.Today;

            // Act
            var newQueryUrl = filteredUrl.AddMonthFilter(startDate);

            // Assert
            Assert.Equal(startDate.ToString("yyyy-MM-dd"), newQueryUrl["dateFrom"]);
        }

        [Fact]
        public void WillPopulateDateToFilterInUrl()
        {
            // Arrange
            var queryUrl = new QueryUrl(
                new RouteValueDictionary(),
                new QueryCollection(
                    new Dictionary<string, StringValues>
                    {
                        {
                            "tag",
                            new StringValues(new[] {"healthy"})
                        }
                    }
                )
                );
            var filteredUrl = new FilteredUrl(queryUrl);
            DateTime startDate = DateTime.Today;

            // Act
            var newQueryUrl = filteredUrl.AddMonthFilter(startDate);

            // Assert
            Assert.Equal(startDate.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd"), newQueryUrl["dateTo"]);
        }

        [Fact]
        public void WillRemoveDateFilterQueryParamFromUrl()
        {
            // Arrange
            var queryUrl = new QueryUrl(
                new RouteValueDictionary(),
                new QueryCollection(
                    new Dictionary<string, StringValues>
                    {
                        {
                            "datefrom",
                            new StringValues(new[] {"irrelevant"})
                        },
                        {
                            "dateto",
                            new StringValues(new[] {"irrelevant"})
                        }
                    }
                )
                );
            var filteredUrl = new FilteredUrl(queryUrl);

            // Act
            var newQueryUrl = filteredUrl.WithoutDateFilter();

            // Assert
            Assert.Equal(false, newQueryUrl.ContainsKey("datefrom"));
            Assert.Equal(false, newQueryUrl.ContainsKey("dateto"));
        }

        [Fact]
        public void WillIdentifyWhenDateFilterIsPresent()
        {
            // Arrange
            var queryUrl = new QueryUrl(
                new RouteValueDictionary(),
                new QueryCollection(
                    new Dictionary<string, StringValues>
                    {
                        {
                            "datefrom",
                            new StringValues(new[] {"irrelevant"})
                        }
                    }
                )
                );
            var filteredUrl = new FilteredUrl(queryUrl);

            // Act
            bool hasNoDatefilter = filteredUrl.HasNoDateFilter();

            // Assert
            Assert.Equal(false, hasNoDatefilter);
        }

        [Fact]
        public void WillIdentifyWhenDateFilterIsNotPresent()
        {
            // Arrange
            var queryUrl = new QueryUrl(
                new RouteValueDictionary(),
                new QueryCollection(new Dictionary<string, StringValues>()
                ));
            var filteredUrl = new FilteredUrl(queryUrl);

            // Act
            bool hasNoDatefilter = filteredUrl.HasNoDateFilter();

            // Assert
            Assert.Equal(true, hasNoDatefilter);
        }

        [Fact]
        public void WillRemoveTagQueryParamFromUrl()
        {
            // Arrange
            var queryUrl = new QueryUrl(
                new RouteValueDictionary(),
                new QueryCollection(
                    new Dictionary<string, StringValues>
                    {
                        {
                            "category",
                            new StringValues(new[] {"business"})
                        },
                        {
                            "tag",
                            new StringValues(new[] {"healthy"})
                        }
                    }
                )
                );
            var filteredUrl = new FilteredUrl(queryUrl);

            // Act
            var newQueryUrl = filteredUrl.WithoutTagFilter();

            // Assert
            Assert.Equal(false, newQueryUrl.ContainsKey("tag"));
        }
    }
}
