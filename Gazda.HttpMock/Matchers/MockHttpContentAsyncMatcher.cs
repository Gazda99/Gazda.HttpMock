namespace Gazda.HttpMock.Matchers;

internal class MockHttpContentAsyncMatcher : IMockHttpMatcher
{
    private readonly Func<HttpContent?, Task<bool>> _contentPredicate;

    public MockHttpContentAsyncMatcher(Func<HttpContent?, Task<bool>> contentPredicate)
    {
        _contentPredicate = contentPredicate;
    }

    public bool Match(HttpRequestMessage request)
    {
        return _contentPredicate.Invoke(request.Content).Result;
    }
}