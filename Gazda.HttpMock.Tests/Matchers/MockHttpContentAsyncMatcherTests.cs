using Gazda.HttpMock.Matchers;

namespace Gazda.HttpMock.Tests.Matchers;

public class MockHttpContentAsyncMatcherTests
{
    [Test]
    [TestCaseSource(nameof(ContentPredicateTestDataTrue))]
    public void Match_Should_Return_True_When_Predicate_Criteria_Met_For_Given_Content(HttpContent content,
        Func<HttpContent?, Task<bool>> predicate)
    {
        //GIVEN
        var httpRequest = Substitute.For<HttpRequestMessage>();
        httpRequest.Content = content;
        var matcher = new MockHttpContentAsyncMatcher(predicate);

        //THEN
        var result = matcher.Match(httpRequest);

        //THEN
        Assert.That(result, Is.True);
    }

    [Test]
    [TestCaseSource(nameof(ContentPredicateTestDataFalse))]
    public void Match_Should_Return_False_When_Predicate_Criteria_Not_Met_For_Given_Content(HttpContent content,
        Func<HttpContent?, Task<bool>> predicate)
    {
        //GIVEN
        var httpRequest = Substitute.For<HttpRequestMessage>();
        httpRequest.Content = content;
        var matcher = new MockHttpContentAsyncMatcher(predicate);

        //THEN
        var result = matcher.Match(httpRequest);

        //THEN
        Assert.That(result, Is.False);
    }

    private static readonly TestCaseData[] ContentPredicateTestDataTrue = new TestCaseData[]
    {
        new TestCaseData(new StringContent("Szatnia 13"),
            new Func<HttpContent?, Task<bool>>(_ => Task.FromResult(true))),
        new TestCaseData(new StreamContent(new MemoryStream()),
            new Func<HttpContent?, Task<bool>>(async c => (await c.ReadAsStreamAsync()).Length == 0)),
        new TestCaseData(new StringContent("hElLo"),
            new Func<HttpContent?, Task<bool>>(async c =>
                (await c.ReadAsStringAsync()).Equals("hello", StringComparison.InvariantCultureIgnoreCase)))
    };


    private static readonly TestCaseData[] ContentPredicateTestDataFalse = new TestCaseData[]
    {
        new TestCaseData(new StringContent("Szatnia 13"),
            new Func<HttpContent?, Task<bool>>(_ => Task.FromResult(false))),
        new TestCaseData(new StreamContent(new MemoryStream()),
            new Func<HttpContent?, Task<bool>>(async c => (await c.ReadAsStreamAsync()).Length == 2137)),
        new TestCaseData(new StringContent("hElLo"),
            new Func<HttpContent?, Task<bool>>(async c =>
                (await c.ReadAsStringAsync()).Equals("hello", StringComparison.Ordinal)))
    };
}