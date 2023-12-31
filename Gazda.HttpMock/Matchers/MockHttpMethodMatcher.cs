﻿namespace Gazda.HttpMock.Matchers;

internal class MockHttpMethodMatcher : IMockHttpMatcher
{
    private readonly HttpMethod _method;

    public MockHttpMethodMatcher(HttpMethod method)
    {
        _method = method;
    }

    public bool Match(HttpRequestMessage request)
    {
        return request.Method == _method;
    }
}