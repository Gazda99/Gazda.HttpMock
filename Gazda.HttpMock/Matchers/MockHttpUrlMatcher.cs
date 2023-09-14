namespace Gazda.HttpMock.Matchers;

internal class MockHttpUrlMatcher : IMockHttpMatcher
{
    private readonly Predicate<string> _urlPredicate;

    public MockHttpUrlMatcher(Predicate<string> urlPredicate)
    {
        _urlPredicate = urlPredicate;
    }

    public bool Match(HttpRequestMessage request)
    {
        if (request.RequestUri == null)
            return false;

        return _urlPredicate.Invoke(request.RequestUri.ToString());
    }
}