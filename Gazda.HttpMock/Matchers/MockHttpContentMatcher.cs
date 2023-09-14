namespace Gazda.HttpMock.Matchers;

internal class MockHttpContentMatcher : IMockHttpMatcher
{
    private readonly Predicate<HttpContent?> _contentPredicate;

    public MockHttpContentMatcher(Predicate<HttpContent?> contentPredicate)
    {
        _contentPredicate = contentPredicate;
    }

    public bool Match(HttpRequestMessage request)
    {
        return _contentPredicate.Invoke(request.Content);
    }
}