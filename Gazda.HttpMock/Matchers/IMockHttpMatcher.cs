namespace Gazda.HttpMock.Matchers;

/// <summary>
/// Defines matching behaviour for HttpRequestMessage.
/// </summary>
public interface IMockHttpMatcher
{
    bool Match(HttpRequestMessage request);
}