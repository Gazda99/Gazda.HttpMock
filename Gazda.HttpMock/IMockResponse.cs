using Gazda.HttpMock.Matchers;

namespace Gazda.HttpMock;

public interface IMockResponse
{
    HttpResponseMessage GetResponse();
    void AddMatcher(IMockHttpMatcher matcher);
    void AddMatchers(IEnumerable<IMockHttpMatcher> matchers);
    bool Match(HttpRequestMessage request);
}