using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using StockportWebapp.Models;
using StockportWebapp.Utils;
using Xunit;

namespace StockportWebappTests.Unit.Utils
{
    public class PaginationGoldenMasterTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        public void NewViewLogicShouldGiveSameResultsAsOldViewLogicForIndexOfFirstItemOnPage(int currentPageNumber)
        {
            // Arrange
            Pagination paginationModel = new Pagination
            {
                TotalItemsOnPage = 15,
                Page = currentPageNumber,
                TotalItems = 150,
                TotalPages = 10,
                PageSize = 15
            };
            var paginationHelper = new PaginationHelper();
            int oldStart = ((paginationModel.Page - 1) * paginationModel.PageSize) + 1;

            // Act
            int newStart = paginationHelper.CalculateIndexOfFirstItemOnPage(paginationModel.Page, paginationModel.PageSize);

            // Assert
            newStart.Should().Be(oldStart);
        }

        [Theory]
        [InlineData(1, 10)]
        [InlineData(2, 15)]
        [InlineData(3, 2)]
        [InlineData(11, 9)]
        public void NewViewLogicShouldGiveSameResultsAsOldViewLogicForIndexOfLastItemOnPage(
            int currentPageNumber,
            int totalItemsOnPage)
        {
            // Arrange
            Pagination paginationModel = new Pagination
            {
                TotalItemsOnPage = totalItemsOnPage,
                Page = currentPageNumber,
                TotalItems = 150,
                TotalPages = 10,
                PageSize = 15
            };
            var paginationHelper = new PaginationHelper();
            int oldStart = ((paginationModel.Page - 1) * paginationModel.PageSize) + 1;
            int oldEnd = oldStart + paginationModel.TotalItemsOnPage - 1;

            // Act
            int newEnd = paginationHelper.CalculateIndexOfLastItemOnPage(paginationModel.Page, paginationModel.TotalItemsOnPage, paginationModel.PageSize);

            // Assert
            newEnd.Should().Be(oldEnd);
        }

        [Theory]
        [InlineData(1, 4)]
        [InlineData(2, 4)]
        [InlineData(3, 4)]
        [InlineData(4, 4)]
        [InlineData(1, 5)]
        [InlineData(2, 5)]
        [InlineData(3, 5)]
        [InlineData(4, 5)]
        [InlineData(5, 5)]
        [InlineData(1, 10)]
        [InlineData(2, 10)]
        [InlineData(3, 10)]
        [InlineData(4, 10)]
        [InlineData(5, 10)]
        [InlineData(6, 10)]
        [InlineData(7, 10)]
        [InlineData(8, 10)]
        [InlineData(9, 10)]
        [InlineData(10, 10)]
        [InlineData(10, 20)]
        public void NewViewLogicShouldGiveSamePageNumberResultsForVisiblePageNumbers(
            int currentPageNumber,
            int totalPages)
        {
            // Arrange
            Pagination paginationModel = new Pagination
            {
                TotalItemsOnPage = 15,
                Page = currentPageNumber,
                TotalItems = 150,
                TotalPages = totalPages,
                PageSize = 15
            };
            var paginationHelper = new PaginationHelper();
            var oldVisiblePageNumbers = OldLogicForFirstVisiblePageNumber(paginationModel);

            // Act
            var newVisiblePageNumbers = paginationHelper.GenerateVisiblePageNumbers(paginationModel.Page, paginationModel.TotalPages);

            // Assert
            newVisiblePageNumbers[0].PageNumber.Should().Be(oldVisiblePageNumbers[0].PageNumber);
            newVisiblePageNumbers[1].PageNumber.Should().Be(oldVisiblePageNumbers[1].PageNumber);
            newVisiblePageNumbers[2].PageNumber.Should().Be(oldVisiblePageNumbers[2].PageNumber);
            newVisiblePageNumbers[3].PageNumber.Should().Be(oldVisiblePageNumbers[3].PageNumber);
            if (oldVisiblePageNumbers.Count > 4)
            {
                newVisiblePageNumbers[4].PageNumber.Should().Be(oldVisiblePageNumbers[4].PageNumber);
            }
        }

        private List<VisiblePageNumber> OldLogicForFirstVisiblePageNumber(Pagination paginationModel)
        {
            List<VisiblePageNumber> results = new List<VisiblePageNumber>();

            var firstVisiblePageNumber = 0;
            var lastVisiblePageNumber = 0;

            // current page is 1+, current page num is less than or equal to last page, there's more than one page
            if (paginationModel.Page >= 1 && paginationModel.Page <= paginationModel.TotalPages && paginationModel.TotalPages > 1)
            {
                firstVisiblePageNumber = paginationModel.Page - 2;
                lastVisiblePageNumber = paginationModel.Page + 2;

                // current page is 1 or 2
                if (paginationModel.Page - 2 <= 0)
                {
                    firstVisiblePageNumber = 1;
                    lastVisiblePageNumber = 5;
                }

                // current page is penultimate or last page
                if (paginationModel.Page + 2 > paginationModel.TotalPages)
                {
                    firstVisiblePageNumber = paginationModel.Page - 5;
                    lastVisiblePageNumber = paginationModel.TotalPages;
                }

                for (int i = firstVisiblePageNumber; i <= lastVisiblePageNumber; i++)
                {
                    if (i > 0 && i <= paginationModel.TotalPages)
                    {
                        if (i == paginationModel.Page)
                        {
                            results.Add(
                                new VisiblePageNumber
                                {
                                    PageNumber = i,
                                    HtmlFragment = $"<span>{i}</span>"
                                });
                        }
                        else
                        {
                            IUrlHelper urlHelper = new UrlHelper(new ActionContext());
                            var url =
                                urlHelper.RouteUrl(
                                    paginationModel.CurrentUrl.AddQueriesToUrl(new Dictionary<string, string>
                                    {
                                        {"Page", i.ToString()}
                                    }));

                            results.Add(
                                new VisiblePageNumber
                                {
                                    PageNumber = i,
                                    HtmlFragment = $"<a href=\"{url}\">@i.ToString()</a>"
                                });
                        }
                    }
                }
            }

            return results;
        }
    }
}