using Gazda.HttpMock.Matchers;

namespace Gazda.HttpMock.Tests;

public class MockResponseTests
{
    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public void Match_Should_Return_Correct_Value(bool val)
    {
        //GIVEN
        var response = Substitute.For<HttpResponseMessage>();
        var request = Substitute.For<HttpRequestMessage>();
        var mockResponse = new MockResponse(response);
        var matcher = Substitute.For<IMockHttpMatcher>();
        matcher.Match(Arg.Any<HttpRequestMessage>()).Returns(val);
        mockResponse.AddMatcher(matcher);

        //WHEN
        var result = mockResponse.Match(request);

        //THEN
        Assert.That(result, Is.EqualTo(val));
    }

    [Test]
    public void ClearMatchers_Should_Clear_Matchers()
    {
        //GIVEN
        var response = Substitute.For<HttpResponseMessage>();
        var request = Substitute.For<HttpRequestMessage>();
        var mockResponse = new MockResponse(response);
        var matcher = Substitute.For<IMockHttpMatcher>();
        matcher.Match(Arg.Any<HttpRequestMessage>()).Returns(false);
        var matcher2 = Substitute.For<IMockHttpMatcher>();
        matcher2.Match(Arg.Any<HttpRequestMessage>()).Returns(false);
        mockResponse.AddMatchers(new[] { matcher, matcher2 });
        mockResponse.ClearMatchers();

        //WHEN
        var result = mockResponse.Match(request);

        //THEN
        Assert.That(result, Is.EqualTo(true));
    }

    [Test]
    public void GetResponse_Should_Return_Real_HttpResponseMessage()
    {
        //GIVEN
        var response = Substitute.For<HttpResponseMessage>();
        var mockResponse = new MockResponse(response);

        //WHEN
        var responseFromMockResponse = mockResponse.GetResponse();

        //THEN
        Assert.That(response, Is.EqualTo(responseFromMockResponse));
    }
}