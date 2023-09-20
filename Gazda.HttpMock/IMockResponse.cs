using Gazda.HttpMock.Matchers;

namespace Gazda.HttpMock;

/// <summary>
/// Contains real HttpResponseMessage response and list of matchers which should be fulfilled in order to match some HttpRequestMessage.
/// </summary>
public interface IMockResponse
{
    HttpResponseMessage GetResponse();
    void AddMatcher(IMockHttpMatcher matcher);
    void AddMatchers(IEnumerable<IMockHttpMatcher> matchers);
    bool Match(HttpRequestMessage request);
}