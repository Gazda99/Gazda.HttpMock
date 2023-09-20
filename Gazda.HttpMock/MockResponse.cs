using Gazda.HttpMock.Matchers;

namespace Gazda.HttpMock;

/// <summary>
/// Contains real HttpResponseMessage response and list of matchers which should be fulfilled in order to match some HttpRequestMessage.
/// </summary>
public class MockResponse : IMockResponse
{
    private readonly HttpResponseMessage _response;
    private readonly List<IMockHttpMatcher> _matchers = new List<IMockHttpMatcher>();

    public MockResponse(HttpResponseMessage response)
    {
        _response = response;
    }

    /// <returns>Real HttpResponseMessage</returns>
    public HttpResponseMessage GetResponse()
    {
        return _response;
    }

    /// <summary>
    /// Adds single IMockHttpMatcher.
    /// </summary>
    public void AddMatcher(IMockHttpMatcher matcher)
    {
        _matchers.Add(matcher);
    }

    /// <summary>
    /// Adds collection of IMockHttpMatcher(s).
    /// </summary>
    public void AddMatchers(IEnumerable<IMockHttpMatcher> matchers)
    {
        _matchers.AddRange(matchers);
    }

    /// <summary>
    /// Clear all existing matchers.
    /// </summary>
    public void ClearMatchers()
    {
        _matchers.Clear();
    }

    /// <summary>
    /// Run all matchers against <paramref name="request"/>.
    /// </summary>
    /// <returns>True, when all matchers were fulfilled.</returns>
    public bool Match(HttpRequestMessage request)
    {
        return _matchers.TrueForAll(matcher => matcher.Match(request));
    }
}