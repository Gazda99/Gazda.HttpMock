namespace Gazda.HttpMock.Tests.Helpers;

public static class HttpMethodHelper
{
    public static readonly HttpMethod[] HttpMethodToTest = new HttpMethod[]
    {
        HttpMethod.Get,
        HttpMethod.Post,
        HttpMethod.Put,
        HttpMethod.Delete,
        HttpMethod.Connect,
        HttpMethod.Options,
        HttpMethod.Trace,
        HttpMethod.Patch
    };
}