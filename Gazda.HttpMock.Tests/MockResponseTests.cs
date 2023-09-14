﻿using Gazda.HttpMock.Matchers;

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
}