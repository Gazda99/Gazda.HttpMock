using Bogus;
using Gazda.HttpMock.Matchers;

namespace Gazda.HttpMock.Tests.Matchers;

public class MockHttpMatcherTests
{
    [Test]
    [TestCaseSource(nameof(RequestPredicateTestDataTrue))]
    public void Match_Should_Return_True_When_Predicate_Criteria_Met_For_Given_Request(HttpRequestMessage httpRequest,
        Predicate<HttpRequestMessage> predicate)
    {
        //GIVEN
        var matcher = new MockHttpMatcher(predicate);

        //THEN
        var result = matcher.Match(httpRequest);

        //THEN
        Assert.That(result, Is.True);
    }

    [Test]
    [TestCaseSource(nameof(RequestPredicateTestDataFalse))]
    public void Match_Should_Return_False_When_Predicate_Criteria_Not_Met_For_Given_Request(
        HttpRequestMessage httpRequest, Predicate<HttpRequestMessage> predicate)
    {
        //GIVEN
        var matcher = new MockHttpMatcher(predicate);

        //THEN
        var result = matcher.Match(httpRequest);

        //THEN
        Assert.That(result, Is.False);
    }

    private static readonly TestCaseData[] RequestPredicateTestDataTrue = new TestCaseData[]
    {
        new TestCaseData(new HttpRequestMessage(), new Predicate<HttpRequestMessage>(_ => true)),
        new TestCaseData(new HttpRequestMessage(HttpMethod.Get, new Uri(new Faker().Internet.Url())),
            new Predicate<HttpRequestMessage>(r => r.Method == HttpMethod.Get)),
        new TestCaseData(new HttpRequestMessage(HttpMethod.Get, "https://github.com/Gazda99/Gazda.HttpMock"),
            new Predicate<HttpRequestMessage>(r => r.RequestUri.ToString().Contains("HttpMock"))),
    };

    private static readonly TestCaseData[] RequestPredicateTestDataFalse = new TestCaseData[]
    {
        new TestCaseData(new HttpRequestMessage(), new Predicate<HttpRequestMessage>(_ => false)),
        new TestCaseData(new HttpRequestMessage(HttpMethod.Get, new Uri(new Faker().Internet.Url())),
            new Predicate<HttpRequestMessage>(r => r.Method == HttpMethod.Post)),
        new TestCaseData(new HttpRequestMessage(HttpMethod.Get, "https://github.com/Gazda99/Gazda.HttpMock"),
            new Predicate<HttpRequestMessage>(r => r.RequestUri.ToString().Contains("Azure"))),
    };
}