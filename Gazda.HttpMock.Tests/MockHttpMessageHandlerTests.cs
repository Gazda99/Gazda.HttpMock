using System.Net;
using Bogus;

namespace Gazda.HttpMock.Tests;

public class MockHttpMessageHandlerTests
{
    [Test]
    public async Task AssertResponseReturned_Should_Return_True_When_Response_Was_Returned_X_Times()
    {
        //GIVEN
        var request = Substitute.For<HttpRequestMessage>();
        var request2 = Substitute.For<HttpRequestMessage>();
        var response = Substitute.For<HttpResponseMessage>();
        var response2 = Substitute.For<HttpResponseMessage>();
        var mockResponse = Substitute.For<IMockResponse>();
        mockResponse.Match(request).Returns(true);
        mockResponse.GetResponse().Returns(response);

        var mockResponse2 = Substitute.For<IMockResponse>();
        mockResponse2.Match(request2).Returns(false);
        mockResponse2.GetResponse().Returns(response2);

        var mockHttpMessageHandler = new MockHttpMessageHandler();
        mockHttpMessageHandler.RespondWith(new[] { mockResponse, mockResponse2 });

        var client = mockHttpMessageHandler.ToHttpClient();
        client.BaseAddress = new Uri(new Faker().Internet.Url());

        //WHEN
        await client.SendAsync(request);
        await client.SendAsync(request2);

        var mockResponseCheck = mockHttpMessageHandler.AssertResponseReturned(mockResponse);
        var mockResponseCheck2 = mockHttpMessageHandler.AssertResponseReturned(mockResponse2, 0);

        //THEN
        Assert.That(mockResponseCheck, Is.True);
        Assert.That(mockResponseCheck2, Is.True);
    }

    [Test]
    public void AssertResponseReturned_Should_Return_False_When_No_Responses()
    {
        //GIVEN
        var mockResponse = Substitute.For<IMockResponse>();
        var mockHttpMessageHandler = new MockHttpMessageHandler();

        var client = mockHttpMessageHandler.ToHttpClient();
        client.BaseAddress = new Uri(new Faker().Internet.Url());

        //WHEN - THEN
        var mockResponseCheck = mockHttpMessageHandler.AssertResponseNotReturned(mockResponse);
        var mockResponseCheck2 = mockHttpMessageHandler.AssertResponseReturned(mockResponse, 0);
        var mockResponseCheck3 = mockHttpMessageHandler.AssertResponseReturned(mockResponse, 1);

        //THEN
        Assert.That(mockResponseCheck, Is.True);
        Assert.That(mockResponseCheck2, Is.True);
        Assert.That(mockResponseCheck3, Is.False);
    }

    [Test]
    public async Task CountResponseReturns_Should_Return_Count_Of_Response_Returns()
    {
        //GIVEN
        var request = Substitute.For<HttpRequestMessage>();
        var request2 = Substitute.For<HttpRequestMessage>();
        var response = Substitute.For<HttpResponseMessage>();
        var response2 = Substitute.For<HttpResponseMessage>();
        var mockResponse = Substitute.For<IMockResponse>();
        mockResponse.Match(request).Returns(true);
        mockResponse.GetResponse().Returns(response);

        var mockResponse2 = Substitute.For<IMockResponse>();
        mockResponse2.Match(request2).Returns(false);
        mockResponse2.GetResponse().Returns(response2);

        var mockHttpMessageHandler = new MockHttpMessageHandler();
        mockHttpMessageHandler.RespondWith(mockResponse);
        mockHttpMessageHandler.RespondWith(mockResponse2);

        var client = mockHttpMessageHandler.ToHttpClient();
        client.BaseAddress = new Uri(new Faker().Internet.Url());

        //WHEN
        await client.SendAsync(request);
        await client.SendAsync(request2);

        var mockResponseCount = mockHttpMessageHandler.CountResponseReturns(mockResponse);
        var mockResponseCount2 = mockHttpMessageHandler.CountResponseReturns(mockResponse2);

        //THEN
        Assert.That(mockResponseCount, Is.EqualTo(1));
        Assert.That(mockResponseCount2, Is.Zero);
    }

    [Test]
    public async Task SendAsync_Should_Return_Default_Response_When_No_MockResponse_Match()
    {
        //GIVEN
        var request = Substitute.For<HttpRequestMessage>();
        var response = Substitute.For<HttpResponseMessage>();
        var mockResponse = Substitute.For<IMockResponse>();
        mockResponse.Match(request).Returns(false);
        mockResponse.GetResponse().Returns(response);

        var mockHttpMessageHandler = new MockHttpMessageHandler();
        mockHttpMessageHandler.RespondWith(mockResponse);

        var client = mockHttpMessageHandler.ToHttpClient();
        client.BaseAddress = new Uri(new Faker().Internet.Url());

        //WHEN
        var res = await client.SendAsync(request);

        //THEN
        Assert.That(res, Is.Not.Null);
        Assert.That(res.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        var content = await res.Content.ReadAsStringAsync();
        Assert.That(content, Is.EqualTo("No mocked response was found. Returning default."));
    }

    [Test]
    public async Task SendAsync_Should_Return_Default_Response_Set_By_User_When_No_MockResponse_Match()
    {
        //GIVEN
        var request = Substitute.For<HttpRequestMessage>();
        var response = Substitute.For<HttpResponseMessage>();
        var mockResponse = Substitute.For<IMockResponse>();
        mockResponse.Match(request).Returns(false);
        mockResponse.GetResponse().Returns(response);
        var defaultResponse = Substitute.For<HttpResponseMessage>();

        var mockHttpMessageHandler = new MockHttpMessageHandler();
        mockHttpMessageHandler.RespondWith(mockResponse);
        mockHttpMessageHandler.SetDefaultResponse(defaultResponse);

        var client = mockHttpMessageHandler.ToHttpClient();
        client.BaseAddress = new Uri(new Faker().Internet.Url());

        //WHEN
        var res = await client.SendAsync(request);

        //THEN
        Assert.That(res, Is.Not.Null);
        Assert.That(res, Is.EqualTo(defaultResponse));
    }
}