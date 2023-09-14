﻿namespace Gazda.HttpMock;

public interface IMockHttpMessageHandler
{
    IMockHttpMessageHandler RespondWith(IMockResponse mockResponse);
    IMockHttpMessageHandler RespondWith(IEnumerable<IMockResponse> mockResponses);
    HttpClient ToHttpClient();
    bool AssertResponseReturned(IMockResponse response, int times);
    int CountResponseReturns(IMockResponse response);
}