using Gazda.HttpMock.Matchers;
using Gazda.HttpMock.Tests.Helpers;

namespace Gazda.HttpMock.Tests.Matchers;

public class MockHttpMethodMatcherTests
{
    [Test]
    [TestCaseSource(typeof(HttpMethodHelper), nameof(HttpMethodHelper.HttpMethodToTest))]
    public void Match_Should_Return_True_For_Same_Methods(HttpMethod method)
    {
        //GIVEN
        var httpRequest = Substitute.For<HttpRequestMessage>();
        httpRequest.Method = method;
        var matcher = new MockHttpMethodMatcher(method);

        //THEN
        var result = matcher.Match(httpRequest);

        //THEN
        Assert.That(result, Is.True);
    }

    [Test]
    public void Match_Should_Return_False_For_Different_Methods()
    {
        //GIVEN
        var httpRequest = Substitute.For<HttpRequestMessage>();
        httpRequest.Method = HttpMethod.Post;
        var matcher = new MockHttpMethodMatcher(HttpMethod.Get);

        //THEN
        var result = matcher.Match(httpRequest);

        //THEN
        Assert.That(result, Is.False);
    }
}