using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using StockportWebapp.QuestionBuilder.Entities;
using Xunit;

namespace StockportWebappTests_Unit.Unit.SmartAnswers
{
    public class PageTests
    {
        [Fact]
        public void NewPage_ShouldReturnEmptyListOfQuestionsAndPreviousAnswers()
        {
            var page = new Page();

            page.Questions.Count.Should().Be(0);
            page.PreviousAnswers.Count.Should().Be(0);
        }

        [Fact]
        public void Action_ShouldBeSetAccordingTo_ValueOfIsLastPage()
        {
            var page = new Page
            {
                IsLastPage = true
            };

            page.Action.Should().Be("submitanswers");
        }

        [Fact]
        public void Action_ShouldBeSetNullAccordingTo_ValueOfIsNotLastPage()
        {
            var page = new Page
            {
                IsLastPage = false
            };

            page.Action.Should().BeNull();
        }
    }
}