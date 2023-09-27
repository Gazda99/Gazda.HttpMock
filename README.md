[![Static Badge](https://img.shields.io/badge/Nuget-1.0.2-blue)](https://www.nuget.org/packages/Gazda.HttpMock)

### Download & Install

Nuget Package [Gazda.HttpMock](https://www.nuget.org/packages/Gazda.HttpMock)

```
Install-Package Gazda.HttpMock
```

## Gazda.HttpMock

Gazda.HttpMock is a library that expose tools for mocking the C# HttpClient class.
It is especially useful for Unit Testing.


---

### How to use

```csharp
var mockHttpMessageHandler = new MockHttpMessageHandler();
var response = new HttpResponseMessage();
IMockResponse mockedResponse = mockHttpMessageHandler.PrepareMockResponse(response)
    .ForUrl(x => x.Contains("some_url"))
    .ForContent(x => x.ReadAsStream().Length > 0)
    .ForContent(async x => (await x.ReadAsStringAsync()) == "content");
    .ForMethod(HttpMethod.Post)
    .ForHeaders(x=>x.Contains("someHeaderName"))
    .For(x=> ...your custom logic...)
    .ForCustomMatch((IMockHttpMatcher) yourCustomMatcher);

HttpClient client = mockHttpMessageHandler.ToHttpClient();
client.SendAsync(new HttpRequestMessage());

int count = mockHttpMessageHandler.CountResponseReturns(mockedResponse);
bool check = mockHttpMessageHandler.AssertResponseReturned(mockedResponse, 2);

```

---

#### Available methods to prepare mock HttpResponseMessage to match some request.

<br/>

All methods are extensions methods on IMockResponse interface, and they might be chained in fluent way.

```csharp
IMockResponse mockedResponse;

mockedResponse.ForXYZ(...).ForABC(...);
```

<br/>

<table>
<tr>
<td> Method </td> <td> Description </td>
</tr>

<tr>
<td>

Default MockHttpMatcher

```csharp
.For(Predicate<HttpRequestMessage> p)
```

</td>
<td>
Predicate run against request (HttpRequestMessage)

Example:

```csharp
.For(x => x.Method == HttpMethod.Get)
```

</td>
</tr>

<tr>
<td>

MockHttpUrlMatcher

```csharp
.ForUrl(Predicate<string> p)
```

</td>
<td>
Predicate run against request.RequestUri.ToString() (string)

Example:

```csharp
.For(x => x.Contains("url_part"))
```

</td>
</tr>

<tr>
<td>

MockHttpMethodMatcher

```csharp
.ForMethod(HttpMethod method)
```

</td>
<td>
Predicate run against request.Method (HttpMethod)

Example:

```csharp
.ForMethod(HttpMethod.Post)
```

</td>
</tr>

<tr>
<td>

MockHttpHeaderMatcher

```csharp
.ForHeaders(Predicate<HttpRequestHeaders> p)
```

</td>
<td>
Predicate run against request.Headers (HttpRequestHeaders)

Example:

```csharp
.ForHeaders(x => x.Contains("some_header"))
```

</td>
</tr>

<tr>
<td>

MockHttpContentMatcher

```csharp
.ForContent(Predicate<HttpContent?> p)
```

</td>
<td>
Predicate run against request.Content (HttpContent?)

Example:

```csharp
.ForContent(x => x != null)
```

</td>
</tr>

<tr>
<td>

MockHttpContentAsyncMatcher

```csharp
.ForContent(Func<HttpContent?, Task<bool>> p)
```

</td>
<td>
Predicate run against request.Content (HttpContent?)

Example:

```csharp
.ForContent(async x => (await x.ReadAsStringAsync()).Equals("hello"))
```

</td>
</tr>

<tr>
<td>

CustomMatcher

```csharp
.ForCustomMatch(IMockHttpMatcher customMatcher)
```

</td>
<td>

Pass your own implementation of IMockHttpMatcher

Example:

```csharp
IMockHttpMatcher customMatcher = new YourOwnCustomMatcher();
.ForCustomMatch(customMatcher)
```

</td>
</tr>







</table>

---

#### What happens when no mocked response matches the request made by HttpClient?

If no mocked responses matches the request, default response is produced.  
Response which is:

404 Not Found with content
_"No mocked response was found. Returning default."_

<br/>

You can also set your own default response using:

```csharp
var mockHttpMessageHandler = new MockHttpMessageHandler();
var newDefaultResponse = new HttpResponseMessage();
mockHttpMessageHandler.SetDefaultResponse(newDefaultResponse);
```

---

#### Available methods to assert response was successfully returned from HttpClient.

<br/>

```csharp
var mockHttpMessageHandler = new MockHttpMessageHandler();
var response = new HttpResponseMessage();
IMockResponse mockedResponse = mockHttpMessageHandler.PrepareMockResponse(response)
var check = mockHttpMessageHandler.CheckOrAssertXYZ(mockedResponse);
```

<br/>

<table>

<tr>
<td> Method </td> <td> Description </td>
</tr>

<tr>
<td>

```csharp
bool AssertResponseReturned(IMockResponse response, int n = 1)
```

</td>
<td>
Asserts that given response was returned 'n' times.
</td>
</tr>

<tr>
<td>

```csharp
bool AssertResponseNotReturned(IMockResponse response)
```

</td>
<td>
Asserts that given response was not returned at all.
</td>
</tr>

<tr>
<td>

```csharp
int CountResponseReturns(IMockResponse response)
```

</td>
<td>
Returns count of returns of given response.
</td>
</tr>




</table>

---
