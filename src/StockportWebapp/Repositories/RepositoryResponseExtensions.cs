using System;
using Microsoft.AspNetCore.Mvc;

namespace StockportWebapp.Repositories
{
    public static class RepositoryResponseExtensions
    {
        public static TResult Map<TInput, TResult>(this IRepositoryResponse<TInput> response,
            Func<Success<TInput>, TResult> successFunction,
            Func<Error<TInput>, TResult> errorFunction)
        {
            return response.IsError() 
                ? errorFunction((Error<TInput>) response) 
                : successFunction((Success<TInput>) response);
        }

        public static IActionResult MapToActionResult<TInput>(this IRepositoryResponse<TInput> response, Func<TInput,IActionResult> retrievedObjectToActionResult )
        {
            if (response.IsError())
            {
                return response as StatusCodeResult;
            }
            var success = (Success<TInput>) response;
            return retrievedObjectToActionResult(success.Content);
        }
    }
}