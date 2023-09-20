namespace Gazda.HttpMock.Matchers;

public interface IMockHttpMatcher
{
    bool Match(HttpRequestMessage request);
}