using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Moq;
using StockportWebapp.Utils;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Utils
{
    public class FilteredUrlTest
    {
        private readonly Mock<ITimeProvider> _timeProvider;
        private readonly FilteredUrl _filteredUrl;

        public FilteredUrlTest()
        {
            _timeProvider = new Mock<ITimeProvider>();
            _timeProvider.Setup(o => o.Now()).Returns(new DateTime(2017, 02, 21));

           _filteredUrl = new FilteredUrl(_timeProvider.Object);
        }

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
                            "Category",
                            new StringValues(new[] {"business"})
                        },
                        {
                            "tag",
                            new StringValues(new[] {"healthy"})
                        }
                    }
                )
                );

            _filteredUrl.SetQueryUrl(queryUrl);

            // Act
            var newQueryUrl = _filteredUrl.WithoutCategory();

            newQueryUrl.ContainsKey("Category").Should().BeFalse();
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
            
            _filteredUrl.SetQueryUrl(queryUrl);

            // Act
            var newQueryUrl = _filteredUrl.AddCategoryFilter("business");

            newQueryUrl.ContainsKey("Category").Should().BeTrue();
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
                            "Category",
                            new StringValues(new[] {"business"})
                        }
                    }
                )
                );

            _filteredUrl.SetQueryUrl(queryUrl);

            // Act
            var hasNoCategoryfilter = _filteredUrl.HasNoCategoryFilter();

            hasNoCategoryfilter.Should().BeFalse();
        }

        [Fact]
        public void WillIdentifyWhenCategoryFilterIsNotPresent()
        {
            // Arrange
            var queryUrl = new QueryUrl(
                new RouteValueDictionary(),
                new QueryCollection(new Dictionary<string, StringValues>()
                ));

            _filteredUrl.SetQueryUrl(queryUrl);

            // Act
            var hasNoCategoryfilter = _filteredUrl.HasNoCategoryFilter();

            hasNoCategoryfilter.Should().BeTrue();
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

            _filteredUrl.SetQueryUrl(queryUrl);
            var startDate = new DateTime(2017, 01, 01);

            // Act
            var newQueryUrl = _filteredUrl.AddMonthFilter(startDate);

            newQueryUrl.ContainsKey("dateFrom").Should().BeTrue();
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

            _filteredUrl.SetQueryUrl(queryUrl);

            var startDate = new DateTime(2017, 01, 01);

            // Act
            var newQueryUrl = _filteredUrl.AddMonthFilter(startDate);

            newQueryUrl.ContainsKey("dateTo").Should().BeTrue();
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

            _filteredUrl.SetQueryUrl(queryUrl);
            var startDate = new DateTime(2017, 01, 01);

            // Act
            var newQueryUrl = _filteredUrl.AddMonthFilter(startDate);

            newQueryUrl["dateFrom"].Should().Be(startDate.ToString("yyyy-MM-dd"));
        }

        [Fact]
        public void WillPopulateDateToFilterInUrl()
        {
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

            _filteredUrl.SetQueryUrl(queryUrl);

            var startDate = new DateTime(2017, 01, 01);
            var newQueryUrl = _filteredUrl.AddMonthFilter(startDate);

            newQueryUrl["dateTo"].Should().Be(new DateTime(2017, 01, 31).ToString("yyyy-MM-dd"));
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
                            "DateFrom",
                            new StringValues(new[] {"irrelevant"})
                        },
                        {
                            "DateTo",
                            new StringValues(new[] {"irrelevant"})
                        }
                    }
                )
                );
            _filteredUrl.SetQueryUrl(queryUrl);

            // Act
            var newQueryUrl = _filteredUrl.WithoutDateFilter();

            // Assert
            newQueryUrl.ContainsKey("DateFrom").Should().BeFalse();
            newQueryUrl.ContainsKey("DateTo").Should().BeFalse();
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
                            "DateFrom",
                            new StringValues(new[] {"irrelevant"})
                        }
                    }
                )
                );
            _filteredUrl.SetQueryUrl(queryUrl);

            // Act
            var hasNoDatefilter = _filteredUrl.HasNoDateFilter();

            hasNoDatefilter.Should().BeFalse();
        }

        [Fact]
        public void WillIdentifyWhenDateFilterIsNotPresent()
        {
            // Arrange
            var queryUrl = new QueryUrl(
                new RouteValueDictionary(),
                new QueryCollection(new Dictionary<string, StringValues>()
                ));
            _filteredUrl.SetQueryUrl(queryUrl);

            // Act
            var hasNoDatefilter = _filteredUrl.HasNoDateFilter();

            hasNoDatefilter.Should().BeTrue();
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
                            "Category",
                            new StringValues(new[] {"business"})
                        },
                        {
                            "tag",
                            new StringValues(new[] {"healthy"})
                        }
                    }
                )
                );
            _filteredUrl.SetQueryUrl(queryUrl);

            // Act
            var newQueryUrl = _filteredUrl.WithoutTagFilter();

            newQueryUrl.ContainsKey("tag").Should().BeFalse();
        }

        [Fact]
        public void ShouldReturnTodaysDateIfDateToIsWithinTheCurrentMonth()
        {
            var queryUrl = new QueryUrl(new RouteValueDictionary(), new QueryCollection());
            _filteredUrl.SetQueryUrl(queryUrl);

            var url = _filteredUrl.AddMonthFilter(new DateTime(2017, 02, 01));

            url["DateTo"].Should().Be(new DateTime(2017, 02, 21).ToString("yyyy-MM-dd"));
        }

        [Fact]
        public void ShouldReturnEmptyIfHasNullQueryUrlForAddMonthFilter()
        {
            var filteredUrl = new FilteredUrl(_timeProvider.Object);

            var result = filteredUrl.AddMonthFilter(new DateTime(2017, 01, 01));

            result.Should().BeEmpty();
        }

        [Fact]
        public void ShouldReturnEmptyIfHasNullQueryUrlForAddCategoryFilter()
        {
            var filteredUrl = new FilteredUrl(_timeProvider.Object);

            var result = filteredUrl.AddCategoryFilter("test");

            result.Should().BeEmpty();
        }

        [Fact]
        public void ShouldReturnFalseIfHasNullQueryUrlForHasNoCategoryFilter()
        {
            var filteredUrl = new FilteredUrl(_timeProvider.Object);

            var result = filteredUrl.HasNoCategoryFilter();

            result.Should().BeFalse();
        }

        [Fact]
        public void ShouldReturnEmptyIfHasNullQueryUrlForHasNoDateFilter()
        {
            var filteredUrl = new FilteredUrl(_timeProvider.Object);

            var result = filteredUrl.HasNoDateFilter();

            result.Should().BeFalse();
        }

        [Fact]
        public void ShouldReturnEmptyIfHasNullQueryUrlForWithoutCategory()
        {
            var filteredUrl = new FilteredUrl(_timeProvider.Object);

            var result = filteredUrl.WithoutCategory();

            result.Should().BeEmpty();
        }

        [Fact]
        public void ShouldReturnEmptyIfHasNullQueryUrlForWithoutDateFilter()
        {
            var filteredUrl = new FilteredUrl(_timeProvider.Object);

            var result = filteredUrl.WithoutDateFilter();

            result.Should().BeEmpty();
        }

        [Fact]
        public void ShouldReturnEmptyIfHasNullQueryUrlForWithoutTagFilter()
        {
            var filteredUrl = new FilteredUrl(_timeProvider.Object);

            var result = filteredUrl.WithoutTagFilter();

            result.Should().BeEmpty();
        }
    }
}
