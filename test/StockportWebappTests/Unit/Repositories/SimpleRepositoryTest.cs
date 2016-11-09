using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.Utils;
using Xunit;

namespace StockportWebappTests.Unit.Repositories
{
    public class SimpleRepositoryTest
    {
        //Because we don't want to be testing the serialisation library.
        public class TestType
        {
            public string Text { get; set; }
            public int Number { get; set; }
        }

        private readonly IRepository<TestType> _repository;
        private readonly Mock<IHttpClient> _httpClientMock = new Mock<IHttpClient>();
        private readonly Mock<IStubToUrlConverter> _urlGeneratorMock = new Mock<IStubToUrlConverter>();

        public SimpleRepositoryTest()
        {
            _repository = new SimpleRepository<TestType>(_urlGeneratorMock.Object, _httpClientMock.Object);
        }

        [Fact]
        public void ShouldGiveMeANonErrorResponseWhenValidJsonAnd200()
        {
            var testTypeJson = "{\"text\":\"different text\",\"number\":1}";
            SetupDependenciesToReturnJsonFromContentApi(testTypeJson);
            var response = AsyncTestHelper.Resolve(_repository.Get());
            response.IsError().Should().Be(false);
        }

        [Theory]
        [InlineData("{\"text\":\"different text\",\"number\":1}","different text",1)]
        [InlineData("{\"text\":\"words\",\"number\":5}","words",5)]
        public void ShouldGiveMeAResponseContainingCorrectTestTypeWhenValidResponse(string testTypeJson, string expectedText, int expectedNumber)
        {
            SetupDependenciesToReturnJsonFromContentApi(testTypeJson);
            var testType = AsyncTestHelper.Resolve(_repository.Get())
                                          .Map(s=>s.Content, e=>null);
            testType.Text.Should().Be(expectedText);
            testType.Number.Should().Be(expectedNumber);
        }

        private void SetupDependenciesToReturnJsonFromContentApi(string testTypeJson)
        {
            _urlGeneratorMock.Setup(x => x.UrlFor<TestType>(string.Empty, It.IsAny<List<Query>>())).Returns(string.Empty);
            _httpClientMock.Setup(x => x.Get(string.Empty))
                .Returns(Task.FromResult(new HttpResponse(200, testTypeJson, string.Empty)));
        }

        [Theory]
        [InlineData(500, "{\"text\":\"different text\",\"number\":1}")]
        [InlineData(404, "")]
        [InlineData(400, "{")]
        [InlineData(502, "{}")]
        public void ShouldGiveMeAnErrorResponseWhenNon200(int statusCode, string returnedObject)
        {
            _urlGeneratorMock.Setup(x => x.UrlFor<TestType>(string.Empty, It.IsAny<List<Query>>())).Returns(string.Empty);
            _httpClientMock.Setup(x => x.Get(string.Empty))
                           .Returns(Task.FromResult(new HttpResponse(statusCode, returnedObject, string.Empty)));
            var response = AsyncTestHelper.Resolve(_repository.Get());
            response.IsError().Should().Be(true);
        }

        [Fact]
        public void ShouldUseTheUrlTheUrlGeneratorProvidesToRetrieveTheObject()
        {
            const string urlReturned = "totally real url";
            _urlGeneratorMock.Setup(x => x.UrlFor<TestType>(string.Empty, It.IsAny<List<Query>>())).Returns(urlReturned);
            _httpClientMock.Setup(x => x.Get(urlReturned))
                           .Returns(Task.FromResult(new HttpResponse(200, "{\"text\":\"words\",\"number\":5}", string.Empty)));
            AsyncTestHelper.Resolve(_repository.Get());
            _httpClientMock.Verify(x=>x.Get(urlReturned));
        }

        [Fact]
        public void ShouldUseTheStubToGenerateTheUrl()
        {
            var slug = "slug for this type";
            _urlGeneratorMock.Setup(x => x.UrlFor<TestType>(slug, It.IsAny<List<Query>>())).Returns(string.Empty);
            _httpClientMock.Setup(x => x.Get(string.Empty))
                           .Returns(Task.FromResult(new HttpResponse(400, "something", string.Empty)));
            AsyncTestHelper.Resolve(_repository.Get(slug));
            _urlGeneratorMock.Verify(x=> x.UrlFor<TestType>(slug, It.IsAny<List<Query>>()));
        }

        //Todo: Using the response from the content api without converting it seems like a bad thing to do.
        //But I wanted to preserve existing behaviour.
        [Fact]
        public void ShouldBeAbleToUseTheErrorResponseAsAStatusCodeResponse()
        {
            _urlGeneratorMock.Setup(x => x.UrlFor<TestType>(string.Empty, It.IsAny<List<Query>>())).Returns(string.Empty);
            _httpClientMock.Setup(x => x.Get(string.Empty))
                           .Returns(Task.FromResult(new HttpResponse(502, string.Empty , "something")));
            var testTypeResponse = AsyncTestHelper.Resolve(_repository.Get());
            var statusCodeResponse = testTypeResponse as StatusCodeResult;
            statusCodeResponse.StatusCode.Should().Be(502);
        }
    }
}