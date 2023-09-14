using Gazda.HttpMock.Matchers;

namespace Gazda.HttpMock;

public class MockResponse : IMockResponse
{
    private readonly HttpResponseMessage _response;
    private readonly List<IMockHttpMatcher> _matchers = new List<IMockHttpMatcher>();

    public MockResponse(HttpResponseMessage response)
    {
        _response = response;
    }

    public HttpResponseMessage GetResponse()
    {
        return _response;
    }

    public void AddMatcher(IMockHttpMatcher matcher)
    {
        _matchers.Add(matcher);
    }

    public void AddMatchers(IEnumerable<IMockHttpMatcher> matchers)
    {
        _matchers.AddRange(matchers);
    }

    public bool Match(HttpRequestMessage request)
    {
        return _matchers.TrueForAll(matcher => matcher.Match(request));
    }
}