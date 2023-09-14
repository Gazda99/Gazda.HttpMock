using System.Net.Http.Headers;
using Gazda.HttpMock.Matchers;

namespace Gazda.HttpMock;

public static class MockResponseExtensions
{
    public static IMockResponse RespondWith(this IMockHttpMessageHandler _, HttpResponseMessage response)
    {
        return new MockResponse(response);
    }

    public static IMockResponse For(this IMockResponse mockResponse, Predicate<HttpRequestMessage> requestPredicate)
    {
        var mockHttpMethodMatcher = new MockHttpMatcher(requestPredicate);
        mockResponse.AddMatcher(mockHttpMethodMatcher);
        return mockResponse;
    }

    public static IMockResponse ForUrl(this IMockResponse mockResponse,
        Predicate<string> urlPredicate)
    {
        var mockHttpUrlMatcher = new MockHttpUrlMatcher(urlPredicate);
        mockResponse.AddMatcher(mockHttpUrlMatcher);
        return mockResponse;
    }

    public static IMockResponse ForMethod(this IMockResponse mockResponse,
        HttpMethod method)
    {
        var mockHttpMethodMatcher = new MockHttpMethodMatcher(method);
        mockResponse.AddMatcher(mockHttpMethodMatcher);
        return mockResponse;
    }

    public static IMockResponse ForHeaders(this IMockResponse mockResponse,
        Predicate<HttpRequestHeaders> headersPredicate)
    {
        var mockHttpMethodMatcher = new MockHttpHeaderMatcher(headersPredicate);
        mockResponse.AddMatcher(mockHttpMethodMatcher);
        return mockResponse;
    }

    public static IMockResponse ForContent(this IMockResponse mockResponse,
        Predicate<HttpContent?> headersPredicate)
    {
        var mockHttpMethodMatcher = new MockHttpContentMatcher(headersPredicate);
        mockResponse.AddMatcher(mockHttpMethodMatcher);
        return mockResponse;
    }

    public static IMockResponse ForCustomMatch(this IMockResponse mockResponse, IMockHttpMatcher customMatcher)
    {
        mockResponse.AddMatcher(customMatcher);
        return mockResponse;
    }
}