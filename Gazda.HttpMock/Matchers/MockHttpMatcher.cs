namespace Gazda.HttpMock.Matchers;

internal class MockHttpMatcher : IMockHttpMatcher
{
    private readonly Predicate<HttpRequestMessage> _requestPredicate;

    public MockHttpMatcher(Predicate<HttpRequestMessage> requestPredicate)
    {
        _requestPredicate = requestPredicate;
    }

    public bool Match(HttpRequestMessage request)
    {
        return _requestPredicate.Invoke(request);
    }
}