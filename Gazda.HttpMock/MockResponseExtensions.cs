using System.Net.Http.Headers;
using Gazda.HttpMock.Matchers;

namespace Gazda.HttpMock;

public static class MockResponseExtensions
{
    /// <summary>
    /// Creates new MockResponse instance and adds it to <paramref name="mockHttpMessageHandler"/>.
    /// </summary>
    /// <param name="response">HttpResponseMessage which will be returned inside of mock response.</param>
    /// <returns>Created MockResponse instance</returns>
    public static IMockResponse PrepareMockResponse(this IMockHttpMessageHandler mockHttpMessageHandler,
        HttpResponseMessage response)
    {
        var mockResponse = new MockResponse(response);
        mockHttpMessageHandler.RespondWith(mockResponse);
        return mockResponse;
    }

    /// <summary>
    /// Adds general predicate matcher to <paramref name="mockResponse"/>.
    /// </summary>
    /// <param name="requestPredicate">General predicate that should be run against HttpRequestMessage.</param>
    public static IMockResponse For(this IMockResponse mockResponse, Predicate<HttpRequestMessage> requestPredicate)
    {
        var mockHttpMethodMatcher = new MockHttpMatcher(requestPredicate);
        mockResponse.AddMatcher(mockHttpMethodMatcher);
        return mockResponse;
    }

    /// <summary>
    /// Adds url predicate matcher to <paramref name="mockResponse"/>.
    /// </summary>
    /// <param name="urlPredicate">Url string predicate that should be run against HttpRequestMessage.RequestUri.ToString().</param>
    public static IMockResponse ForUrl(this IMockResponse mockResponse,
        Predicate<string> urlPredicate)
    {
        var mockHttpUrlMatcher = new MockHttpUrlMatcher(urlPredicate);
        mockResponse.AddMatcher(mockHttpUrlMatcher);
        return mockResponse;
    }

    /// <summary>
    /// Adds method predicate matcher to <paramref name="mockResponse"/>.
    /// </summary>
    /// <param name="method">Method that should be expected from HttpRequestMessage.</param>
    public static IMockResponse ForMethod(this IMockResponse mockResponse,
        HttpMethod method)
    {
        var mockHttpMethodMatcher = new MockHttpMethodMatcher(method);
        mockResponse.AddMatcher(mockHttpMethodMatcher);
        return mockResponse;
    }

    /// <summary>
    /// Adds headers predicate matcher to <paramref name="mockResponse"/>.
    /// </summary>
    /// <param name="headersPredicate">HttpRequestHeaders predicate that should be run against HttpRequestMessage.Headers.</param>
    public static IMockResponse ForHeaders(this IMockResponse mockResponse,
        Predicate<HttpRequestHeaders> headersPredicate)
    {
        var mockHttpMethodMatcher = new MockHttpHeaderMatcher(headersPredicate);
        mockResponse.AddMatcher(mockHttpMethodMatcher);
        return mockResponse;
    }

    /// <summary>
    /// Adds content predicate matcher to <paramref name="mockResponse"/>.
    /// </summary>
    /// <param name="contentPredicate">HttpContent predicate that should be run against HttpRequestMessage.Content.</param>
    public static IMockResponse ForContent(this IMockResponse mockResponse,
        Predicate<HttpContent?> contentPredicate)
    {
        var mockHttpMethodMatcher = new MockHttpContentMatcher(contentPredicate);
        mockResponse.AddMatcher(mockHttpMethodMatcher);
        return mockResponse;
    }

    /// <summary>
    /// Adds content predicate matcher to <paramref name="mockResponse"/>.
    /// </summary>
    /// <param name="contentPredicate">HttpContent async predicate that should be run against HttpRequestMessage.Content.</param>
    public static IMockResponse ForContent(this IMockResponse mockResponse,
        Func<HttpContent?, Task<bool>> contentPredicate)
    {
        var mockHttpMethodMatcher = new MockHttpContentAsyncMatcher(contentPredicate);
        mockResponse.AddMatcher(mockHttpMethodMatcher);
        return mockResponse;
    }

    /// <summary>
    /// Adds custom matcher to <paramref name="mockResponse"/>.
    /// </summary>
    /// <param name="customMatcher">Custom IMockHttpMatcher matcher.</param>
    public static IMockResponse ForCustomMatch(this IMockResponse mockResponse, IMockHttpMatcher customMatcher)
    {
        mockResponse.AddMatcher(customMatcher);
        return mockResponse;
    }
}