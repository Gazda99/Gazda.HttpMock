## Gazda.HttpMock

Gazda.HttpMock is a library that expose tools for mocking the C# HttpClient class.
It is especially useful for Unit Testing.


---

### How to use

```csharp
var mockHttMessageHandler = new MockHttpMessageHandler();
var response = new HttpResponseMessage();
IMockResponse mockedResponse = mockHttMessageHandler.PrepareMockResponse(response)
    .ForUrl(x => x.Contains("some_url"))
    .ForContent(x => x.ReadAsStream().Length > 0)
    .ForContent(async x => (await x.ReadAsStringAsync()) == "content");
    .ForMethod(HttpMethod.Post)
    .ForHeaders(x=>x.Contains("someHeaderName"))
    .For(x=> ...your custom logic...)
    .ForCustomMatch((IMockHttpMatcher) yourCustomMatcher);

HttpClient client = mockHttMessageHandler.ToHttpClient();
client.SendAsync(new HttpRequestMessage());

int count = mockHttMessageHandler.CountResponseReturns(mockedResponse);
bool check = mockHttMessageHandler.AssertResponseReturned(mockedResponse, 2);

```

---

#### Available methods to prepare mock HttpResponseMessage to match some request.

\
All methods are extensions methods on IMockResponse interface, and they might be chained in fluent way.

```csharp
IMockResponse mockedResponse;

mockedResponse.ForXYZ(...).ForABC(...);
```

---

Default MockHttpMatcher

```csharp
.For(Predicate<HttpRequestMessage> p)
```

Predicate run against request (HttpRequestMessage). Example:

```csharp
.For(x => x.Method == HttpMethod.Get)
```

---
MockHttpUrlMatcher

```csharp
.ForUrl(Predicate<string> p)
```

Predicate run against request.RequestUri.ToString() (string). Example:

```csharp
.For(x => x.Contains("url_part"))
```

---
MockHttpMethodMatcher

```csharp
.ForMethod(HttpMethod method)
```

Predicate run against request.Method (HttpMethod). Example:

```csharp
.ForMethod(HttpMethod.Post)
```

---
MockHttpHeaderMatcher

```csharp
.ForHeaders(Predicate<HttpRequestHeaders> p)
```

Predicate run against request.Headers (HttpRequestHeaders). Example:

```csharp
.ForHeaders(x => x.Contains("some_header"))
```

---
MockHttpContentMatcher

```csharp
.ForContent(Predicate<HttpContent?> p)
```

Predicate run against request.Content (HttpContent?). Example:

```csharp
.ForContent(x => x != null)
```

---
MockHttpContentAsyncMatcher

```csharp
.ForContent(Func<HttpContent?, Task<bool>> p)
```

Predicate run against request.Content (HttpContent?). Example:

```csharp
.ForContent(async x => (await x.ReadAsStringAsync()).Equals("hello"))
```

---
CustomMatcher

```csharp
.ForCustomMatch(IMockHttpMatcher customMatcher)
```

Pass your own implementation of IMockHttpMatcher . Example:

```csharp
IMockHttpMatcher customMatcher = new YourOwnCustomMatcher();
.ForCustomMatch(customMatcher)
```

---

#### What happens when no mocked response matches the request made by HttpClient?

If no mocked responses matches the request, default response is produced.  
Response which is:

404 Not Found with content
_"No mocked response was found. Returning default."_

---

#### Available methods to assert response was successfully returned from HttpClient.

```csharp
var mockHttMessageHandler = new MockHttpMessageHandler();
var response = new HttpResponseMessage();
IMockResponse mockedResponse = mockHttMessageHandler.PrepareMockResponse(response)
var check = mockHttMessageHandler.CheckOrAssertXYZ(mockedResponse);
```

---

Asserts that given response was returned 'n' times:

```csharp
bool AssertResponseReturned(IMockResponse response, int n)
```

Asserts that given response was not returned at all:

```csharp
bool AssertResponseNotReturned(IMockResponse response)
```

Returns count of returns of given response:

```csharp
int CountResponseReturns(IMockResponse response)
```

---
