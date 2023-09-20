﻿using System.Net;

namespace Gazda.HttpMock;

public class MockHttpMessageHandler : HttpMessageHandler, IMockHttpMessageHandler
{
    private readonly Dictionary<IMockResponse, int> _mockResponsesWithReturnCount = new();

    private readonly HttpResponseMessage _defaultResponse = new HttpResponseMessage(HttpStatusCode.NotFound)
    {
        Content = new StringContent("No mocked response was found. Returning default.")
    };

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        try
        {
            var mockedResponse = _mockResponsesWithReturnCount.First(x => x.Key.Match(request)).Key;
            var response = mockedResponse.GetResponse();
            _mockResponsesWithReturnCount[mockedResponse] += 1;
            return Task.FromResult(response);
        }
        catch (InvalidOperationException ex)
        {
            return Task.FromResult(_defaultResponse);
        }
    }

    public IMockHttpMessageHandler RespondWith(IMockResponse mockResponse)
    {
        _mockResponsesWithReturnCount.TryAdd(mockResponse, 0);
        return this;
    }

    public IMockHttpMessageHandler RespondWith(IEnumerable<IMockResponse> mockResponses)
    {
        foreach (var mockResponse in mockResponses)
        {
            _mockResponsesWithReturnCount.TryAdd(mockResponse, 0);
        }

        return this;
    }

    public void ClearResponses()
    {
        _mockResponsesWithReturnCount.Clear();
    }

    public HttpClient ToHttpClient()
    {
        return new HttpClient(this);
    }

    public bool AssertResponseReturned(IMockResponse response, int times = 1)
    {
        var isFound = _mockResponsesWithReturnCount.TryGetValue(response, out var found);
        if (!isFound)
            return times == 0;

        return times == found;
    }

    public bool AssertResponseNotReturned(IMockResponse response)
    {
        var isFound = _mockResponsesWithReturnCount.TryGetValue(response, out var found);
        if (!isFound)
            return true;

        return found == 0;
    }

    public int CountResponseReturns(IMockResponse response)
    {
        var isFound = _mockResponsesWithReturnCount.TryGetValue(response, out var found);
        return isFound ? found : 0;
    }
}