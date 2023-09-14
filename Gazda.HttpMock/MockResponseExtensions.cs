using Gazda.HttpMock.Matchers;

namespace Gazda.HttpMock;

public static class MockResponseExtensions
{
    public static IMockResponse RespondWith(this IMockHttpMessageHandler _, HttpResponseMessage response)
    {
        return new MockResponse(response);
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
}