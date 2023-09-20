using Gazda.HttpMock.Matchers;

namespace Gazda.HttpMock.Tests.Matchers;

public class MockHttpContentMatcherTests
{
    [Test]
    [TestCaseSource(nameof(ContentPredicateTestDataTrue))]
    public void Match_Should_Return_True_When_Predicate_Criteria_Met_For_Given_Content(HttpContent content,
        Predicate<HttpContent?> predicate)
    {
        //GIVEN
        var httpRequest = Substitute.For<HttpRequestMessage>();
        httpRequest.Content = content;
        var matcher = new MockHttpContentMatcher(predicate);

        //THEN
        var result = matcher.Match(httpRequest);

        //THEN
        Assert.That(result, Is.True);
    }

    [Test]
    [TestCaseSource(nameof(ContentPredicateTestDataFalse))]
    public void Match_Should_Return_False_When_Predicate_Criteria_Not_Met_For_Given_Content(HttpContent content,
        Predicate<HttpContent?> predicate)
    {
        //GIVEN
        var httpRequest = Substitute.For<HttpRequestMessage>();
        httpRequest.Content = content;
        var matcher = new MockHttpContentMatcher(predicate);

        //THEN
        var result = matcher.Match(httpRequest);

        //THEN
        Assert.That(result, Is.False);
    }

    private static readonly TestCaseData[] ContentPredicateTestDataTrue = new TestCaseData[]
    {
        new TestCaseData(new StringContent("Szatnia 13"), new Predicate<HttpContent?>(_ => true)),
        new TestCaseData(new StreamContent(new MemoryStream()),
            new Predicate<HttpContent?>(c => c.ReadAsStream().Length == 0)),
        new TestCaseData((HttpContent) null, new Predicate<HttpContent?>(c => c == null))
    };


    private static readonly TestCaseData[] ContentPredicateTestDataFalse = new TestCaseData[]
    {
        new TestCaseData(new StringContent("Szatnia 13"), new Predicate<HttpContent?>(_ => false)),
        new TestCaseData(new StreamContent(new MemoryStream()),
            new Predicate<HttpContent?>(c => c.ReadAsStream().Length == 2137)),
        new TestCaseData((HttpContent) null, new Predicate<HttpContent?>(c => c != null))
    };
}