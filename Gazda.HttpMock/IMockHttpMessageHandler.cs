namespace Gazda.HttpMock;

public interface IMockHttpMessageHandler
{
    IMockHttpMessageHandler RespondWith(IMockResponse mockResponse);
    IMockHttpMessageHandler RespondWith(IEnumerable<IMockResponse> mockResponses);
    void ClearResponses();
    HttpClient ToHttpClient();
    bool AssertResponseReturned(IMockResponse response, int n);
    bool AssertResponseNotReturned(IMockResponse response);
    int CountResponseReturns(IMockResponse response);
}