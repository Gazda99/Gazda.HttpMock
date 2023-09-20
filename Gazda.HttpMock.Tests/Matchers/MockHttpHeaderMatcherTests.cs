using System.Net.Http.Headers;
using Gazda.HttpMock.Matchers;

namespace Gazda.HttpMock.Tests.Matchers;

public class MockHttpHeaderMatcherTests
{
    [Test]
    [TestCaseSource(nameof(HeadersPredicateTestDataTrue))]
    public void Match_Should_Return_True_When_Predicate_Criteria_Met_For_Given_Headers(
        IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers,
        Predicate<HttpRequestHeaders> predicate)
    {
        //GIVEN
        var httpRequest = Substitute.For<HttpRequestMessage>();
        foreach (var header in headers)
        {
            httpRequest.Headers.Add(header.Key, header.Value);
        }

        var matcher = new MockHttpHeaderMatcher(predicate);

        //THEN
        var result = matcher.Match(httpRequest);

        //THEN
        Assert.That(result, Is.True);
    }

    [Test]
    [TestCaseSource(nameof(HeadersPredicateTestDataFalse))]
    public void Match_Should_Return_False_When_Predicate_Criteria_Not_Met_For_Given_Headers(
        IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers,
        Predicate<HttpRequestHeaders> predicate)
    {
        //GIVEN
        var httpRequest = Substitute.For<HttpRequestMessage>();
        foreach (var header in headers)
        {
            httpRequest.Headers.Add(header.Key, header.Value);
        }

        var matcher = new MockHttpHeaderMatcher(predicate);

        //THEN
        var result = matcher.Match(httpRequest);

        //THEN
        Assert.That(result, Is.False);
    }

    private static readonly TestCaseData[] HeadersPredicateTestDataTrue = new TestCaseData[]
    {
        new TestCaseData(new List<KeyValuePair<string, IEnumerable<string>>>(),
            new Predicate<HttpRequestHeaders>(_ => true)),
        new TestCaseData(new List<KeyValuePair<string, IEnumerable<string>>>()
            {
                new KeyValuePair<string, IEnumerable<string>>("something_yes_yes", new[] { "some_value" })
            },
            new Predicate<HttpRequestHeaders>(h => h.Contains("something_yes_yes"))),
        new TestCaseData(new List<KeyValuePair<string, IEnumerable<string>>>()
            {
                new KeyValuePair<string, IEnumerable<string>>("some_VALUE", new[] { "some_value222" })
            },
            new Predicate<HttpRequestHeaders>(h => h.GetValues("some_VALUE").Contains("some_value222"))),
    };

    private static readonly TestCaseData[] HeadersPredicateTestDataFalse = new TestCaseData[]
    {
        new TestCaseData(new List<KeyValuePair<string, IEnumerable<string>>>(),
            new Predicate<HttpRequestHeaders>(_ => false)),
        new TestCaseData(new List<KeyValuePair<string, IEnumerable<string>>>()
            {
                new KeyValuePair<string, IEnumerable<string>>("something_yes_yes", new[] { "asfasdfa" })
            },
            new Predicate<HttpRequestHeaders>(h => h.Contains("something_no_no"))),
        new TestCaseData(new List<KeyValuePair<string, IEnumerable<string>>>()
            {
                new KeyValuePair<string, IEnumerable<string>>("some_VALUE", new[] { "!!!!!391240" })
            },
            new Predicate<HttpRequestHeaders>(h => h.GetValues("some_VALUE").Contains("some_value222"))),
    };
}