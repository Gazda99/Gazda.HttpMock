using System.Net;

namespace Gazda.HttpMock;

/// <summary>
/// Mock implementation of HttpMessageHandler.
/// </summary>
public class MockHttpMessageHandler : HttpMessageHandler, IMockHttpMessageHandler
{
    private readonly object _lock = new object();
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
            lock (_lock)
            {
                var mockedResponse = _mockResponsesWithReturnCount.First(x => x.Key.Match(request)).Key;
                var response = mockedResponse.GetResponse();
                _mockResponsesWithReturnCount[mockedResponse] += 1;
                return Task.FromResult(response);
            }
        }
        catch (InvalidOperationException ex)
        {
            return Task.FromResult(_defaultResponse);
        }
    }

    /// <summary>
    /// Adds IMockResponse <paramref name="mockResponse"/> to list of possible returns from this message handler.
    /// </summary>
    public IMockHttpMessageHandler RespondWith(IMockResponse mockResponse)
    {
        lock (_lock)
            _mockResponsesWithReturnCount.TryAdd(mockResponse, 0);
        return this;
    }

    /// <summary>
    /// Adds collection of IMockResponse <paramref name="mockResponses"/> to list of possible returns from this message handler.
    /// </summary>
    public IMockHttpMessageHandler RespondWith(IEnumerable<IMockResponse> mockResponses)
    {
        lock (_lock)
        {
            foreach (var mockResponse in mockResponses)
            {
                _mockResponsesWithReturnCount.TryAdd(mockResponse, 0);
            }
        }

        return this;
    }

    /// <summary>
    /// Clear all possible IMockResponse returns.
    /// </summary>
    public void ClearResponses()
    {
        lock (_lock)
            _mockResponsesWithReturnCount.Clear();
    }

    /// <returns>New HttpClient using this MockHttpMessageHandler.</returns>
    public HttpClient ToHttpClient()
    {
        return new HttpClient(this);
    }

    /// <summary>
    /// Check if <paramref name="response"/> was returned <paramref name="n"/> times.
    /// </summary>
    /// <param name="response">IMockResponse to check.</param>
    /// <param name="n">How many times should <paramref name="response"/> be returned.</param>
    /// <returns>True if assertion was correct.</returns>
    public bool AssertResponseReturned(IMockResponse response, int n = 1)
    {
        lock (_lock)
        {
            var isFound = _mockResponsesWithReturnCount.TryGetValue(response, out var found);
            if (!isFound)
                return n == 0;

            return n == found;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="response">IMockResponse to check.</param>
    /// <returns>True if assertion was correct.</returns>
    public bool AssertResponseNotReturned(IMockResponse response)
    {
        lock (_lock)
        {
            var isFound = _mockResponsesWithReturnCount.TryGetValue(response, out var found);
            if (!isFound)
                return true;

            return found == 0;
        }
    }

    /// <param name="response">IMockResponse to check.</param>
    /// <returns>How many times, given response was returned.</returns>
    public int CountResponseReturns(IMockResponse response)
    {
        lock (_lock)
        {
            var isFound = _mockResponsesWithReturnCount.TryGetValue(response, out var found);
            return isFound ? found : 0;
        }
    }
}