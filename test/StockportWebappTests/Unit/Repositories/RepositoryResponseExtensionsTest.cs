using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Repositories;
using Xunit;

namespace StockportWebappTests.Unit.Repositories
{
    public class RepositoryResponseExtensionsTest
    {
        #region MapToActionResult
        [Fact]
        public void MapToActionResultShouldSkipMappingForErrorT()
        {
            var error = new Error<int>(502);
            var result = error.MapToActionResult(x => { throw new Exception();});
            result.Should().Be(error);
        }


        [Theory]
        [InlineData(1337)]
        [InlineData(0xACECA11)]
        [InlineData(0xF00B411)]
        public void MapToActionResultShouldUseFunctionForSuccess(int valueToPassAround)
        {
            var resultFromFunction = new NoContentResult();
            var success = new Success<int>(valueToPassAround);
            var result = success.MapToActionResult(x => {
                x.Should().Be(valueToPassAround);
                return resultFromFunction;
            });
            result.Should().Be(resultFromFunction);
        }
        #endregion

        #region Map
        [Fact]
        public void MapCallsSuccessFunctionOnItselfAndNotErrorFunctionWhenSuccess()
        {
            var success = new Success<bool>(false);
            success.Map(x =>
            {
                x.Should().Be(success);
                return x;
            }, y => { throw new Exception(); });
        }

        [Fact]
        public void MapCallsErrorFunctionOnItselfAndNotSuccessFunctionWhenError()
        {
            var error = new Error<bool>(403);
            error.Map(y => { throw new Exception(); }, x =>
            {
                x.Should().Be(error);
                return x;
            });
        }
        #endregion

    }
}