using System.Net.Http.Headers;

namespace Gazda.HttpMock.Matchers;

public class MockHttpHeaderMatcher : IMockHttpMatcher
{
    private readonly Predicate<HttpRequestHeaders> _headerPredicate;

    public MockHttpHeaderMatcher(Predicate<HttpRequestHeaders> headerPredicate)
    {
        _headerPredicate = headerPredicate;
    }

    public bool Match(HttpRequestMessage request)
    {
        return _headerPredicate.Invoke(request.Headers);
    }
}