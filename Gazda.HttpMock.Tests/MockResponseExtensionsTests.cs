using System.Net.Http.Headers;
using Gazda.HttpMock.Matchers;
using Gazda.HttpMock.Tests.Helpers;

namespace Gazda.HttpMock.Tests;

public class MockResponseExtensionsTests
{
    [Test]
    public void PrepareMockResponse_Should_Return_New_Mock_Response()
    {
        //GIVEN 
        var mockHttpMessageHandler = Substitute.For<IMockHttpMessageHandler>();
        var response = Substitute.For<HttpResponseMessage>();

        //WHEN - THEN
        var res = mockHttpMessageHandler.PrepareMockResponse(response);

        Assert.That(res is IMockResponse);
    }

    [Test]
    public void For_Should_Add_MockHttpMatcher()
    {
        //GIVEN
        var mockResponse = Substitute.For<IMockResponse>();
        var predicate = Substitute.For<Predicate<HttpRequestMessage>>();

        //WHEN
        mockResponse.For(predicate);

        //THEN
        mockResponse.Received(1).AddMatcher(Arg.Is<IMockHttpMatcher>(x => x is MockHttpMatcher));
    }

    [Test]
    public void ForMethod_Should_Add_MockHttpUrlMatcher()
    {
        //GIVEN
        var mockResponse = Substitute.For<IMockResponse>();
        var predicate = Substitute.For<Predicate<string>>();

        //WHEN
        mockResponse.ForUrl(predicate);

        //THEN
        mockResponse.Received(1).AddMatcher(Arg.Is<IMockHttpMatcher>(x => x is MockHttpUrlMatcher));
    }

    [Test]
    [TestCaseSource(typeof(HttpMethodHelper), nameof(HttpMethodHelper.HttpMethodToTest))]
    public void ForUrl_Should_Add_MockHttpUrlMatcher(HttpMethod method)
    {
        //GIVEN
        var mockResponse = Substitute.For<IMockResponse>();

        //WHEN
        mockResponse.ForMethod(method);

        //THEN
        mockResponse.Received(1).AddMatcher(Arg.Is<IMockHttpMatcher>(x => x is MockHttpMethodMatcher));
    }

    [Test]
    public void ForHeaders_Should_Add_MockHttpHeaderMatcher()
    {
        //GIVEN
        var mockResponse = Substitute.For<IMockResponse>();
        var predicate = Substitute.For<Predicate<HttpHeaders>>();

        //WHEN
        mockResponse.ForHeaders(predicate);

        //THEN
        mockResponse.Received(1).AddMatcher(Arg.Is<IMockHttpMatcher>(x => x is MockHttpHeaderMatcher));
    }

    [Test]
    public void ForContent_Should_Add_MockHttpContentMatcher()
    {
        //GIVEN
        var mockResponse = Substitute.For<IMockResponse>();
        var predicate = Substitute.For<Predicate<HttpContent?>>();

        //WHEN
        mockResponse.ForContent(predicate);

        //THEN
        mockResponse.Received(1).AddMatcher(Arg.Is<IMockHttpMatcher>(x => x is MockHttpContentMatcher));
    }

    [Test]
    public void ForContent_Async_Version_Should_Add_MockHttpContentAsyncMatcher()
    {
        //GIVEN
        var mockResponse = Substitute.For<IMockResponse>();
        var predicate = Substitute.For<Func<HttpContent?, Task<bool>>>();

        //WHEN
        mockResponse.ForContent(predicate);

        //THEN
        mockResponse.Received(1).AddMatcher(Arg.Is<IMockHttpMatcher>(x => x is MockHttpContentAsyncMatcher));
    }

    [Test]
    public void ForCustomMatch_Should_Add_IMockHttpMatcher()
    {
        //GIVEN
        var mockResponse = Substitute.For<IMockResponse>();
        var customMatcher = Substitute.For<IMockHttpMatcher>();

        //WHEN
        mockResponse.ForCustomMatch(customMatcher);

        //THEN
        mockResponse.Received(1).AddMatcher(Arg.Is<IMockHttpMatcher>(x => x != null));
    }
}