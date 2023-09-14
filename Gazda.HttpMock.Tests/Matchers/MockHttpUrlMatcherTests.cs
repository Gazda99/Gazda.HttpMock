using Bogus;
using Gazda.HttpMock.Matchers;

namespace Gazda.HttpMock.Tests.Matchers;

public class MockHttpUrlMatcherTests
{
    [Test]
    [TestCaseSource(nameof(UrlPredicateTestDataTrue))]
    public void Match_Should_Return_True_When_Predicate_Criteria_Met_For_Given_Url(string requestUri,
        Predicate<string> predicate)
    {
        //GIVEN
        var httpRequest = Substitute.For<HttpRequestMessage>();
        httpRequest.RequestUri = new Uri(requestUri);
        var matcher = new MockHttpUrlMatcher(predicate);

        //THEN
        var result = matcher.Match(httpRequest);

        //THEN
        Assert.That(result, Is.True);
    }

    [Test]
    [TestCaseSource(nameof(UrlPredicateTestDataFalse))]
    public void Match_Should_Return_False_When_Predicate_Criteria_Not_Met_For_Given_Url(string requestUri,
        Predicate<string> predicate)
    {
        //GIVEN
        var httpRequest = Substitute.For<HttpRequestMessage>();
        httpRequest.RequestUri = new Uri(requestUri);
        var matcher = new MockHttpUrlMatcher(predicate);

        //THEN
        var result = matcher.Match(httpRequest);

        //THEN
        Assert.That(result, Is.False);
    }

    [Test]
    public void Match_Should_Return_False_When_Provided_Url_Is_Null()
    {
        //GIVEN
        var httpRequest = Substitute.For<HttpRequestMessage>();
        var matcher = new MockHttpUrlMatcher(new Predicate<string>(_ => true));

        //THEN
        var result = matcher.Match(httpRequest);

        //THEN
        Assert.That(result, Is.False);
    }

    private static readonly TestCaseData[] UrlPredicateTestDataTrue = new TestCaseData[]
    {
        new TestCaseData(new Faker().Internet.Url(), new Predicate<string>(_ => true)),
        new TestCaseData("http://www.SoMe.com",
            new Predicate<string>(x => x.Contains("SoMe", StringComparison.InvariantCultureIgnoreCase))),
        new TestCaseData("http://www.SoMe.com", new Predicate<string>(x => x.EndsWith("com/"))),
    };

    private static readonly TestCaseData[] UrlPredicateTestDataFalse = new TestCaseData[]
    {
        new TestCaseData(new Faker().Internet.Url(), new Predicate<string>(_ => false)),
        new TestCaseData("http://www.SoMe.com", new Predicate<string>(x => x.Contains("not_here"))),
        new TestCaseData("http://www.SoMe.com", new Predicate<string>(x => x.EndsWith("http"))),
    };
}