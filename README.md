# HttpExtension

Extensions for HttpClient

###### This is the component, works on .NET Core and.NET Framework

**NuGet**

|Name|Info|Contributors|
| ------------------- | ------------------- | ------------------- |
|HttpExtension|[![NuGet](https://buildstats.info/nuget/HttpExtension)](https://www.nuget.org/packages/HttpExtension/)|[![GitHub contributors](https://img.shields.io/github/contributors/TBertuzzi/HttpExtension.svg)](https://github.com/TBertuzzi/HttpExtension/graphs/contributors)|

**Platform Support**

HttpExtension is a netstandard 2.1 library.

Extensions to make using HttpClient easy.

* GetAsync<T> : Gets the return of a Get Rest and converts to the object or collection of pre-defined objects.
You can use only the path of the rest method, or pass a parameter dictionary. In case the url has parameters.

```csharp
 public static async Task<HttpExtensionResponse<T>> GetAsync<T>(this HttpClient httpClient, string address);
 public static async Task<HttpExtensionResponse<T>> GetAsync<T>(this HttpClient httpClient, string address, Dictionary<string, string> values);
```


* PostAsync<T>,PutAsync<T> and DeleteAsync<T> : Use post, put and delete service methods rest asynchronously and return objects if necessary. 

```csharp
 public static async Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, string address, object dto);
 public static async Task<HttpExtensionResponse<T>> PostAsync<T>(this HttpClient httpClient, string address, object dto);
 
 public static async Task<HttpResponseMessage> PutAsync(this HttpClient httpClient,string address, object dto);
 public static async Task<HttpExtensionResponse<T>> PutAsync<T>(this HttpClient httpClient, string address, object dto);
 
 public static async Task<HttpResponseMessage> DeleteAsync(this HttpClient httpClient,string address, object dto);
 public static async Task<HttpExtensionResponse<T>> DeleteAsync<T>(this HttpClient httpClient, string address, object dto);
```

* SendAsync<T> : Use SendAsync for your custom HTTP request message and return predefined objects or collection.

```csharp
 public static async Task<HttpExtensionResponse<T>> SendAsync<T>(this HttpClient httpClient, HttpRequestMessage request);
```

* HttpExtensionResponse<T> : Object that facilitates the return of requests Rest. It returns the Http code of the request, already converted object and the contents in case of errors.

```csharp
public class HttpExtensionResponse<T>
{
  public HttpStatusCode StatusCode { get; private set; }

  public T Value { get; set; }

  public string Content { get; set; }

  public Exception Error { get; set; }
}
```

Example of use :

```csharp
public async Task<List<Model.Todo>> GetTodos()
{
    try
    {
        //GetAsync Return with Object
        var response = await _httpClient.GetAsync<List<Model.Todo>>("todos");
           
        if (response.StatusCode == HttpStatusCode.OK)
        {
              return response.Value;
        }
        else
        {
            throw new Exception(
                   $"HttpStatusCode: {response.StatusCode.ToString()} Message: {response.Content}");
        }
    }
    catch (Exception ex)
    {
        throw new Exception(ex.Message);
    }
}
```

**Retry Pattern support using Polly**

You can use the retry pattern with HttpExtension using [Polly](https://github.com/App-vNext/Polly).

Example of use :

```csharp
public async Task<List<Model.Todo>> GetTodos()
{
    var policy = CreatePolicy();
    try
    {
        //GetAsync using retry pattern
        var response = await _httpClient.GetAsync<List<Model.Todo>>("todos", policy);
           
        if (response.StatusCode == HttpStatusCode.OK)
            return response.Value;
        else
        {
            throw new Exception(
                   $"HttpStatusCode: {response.StatusCode.ToString()} Message: {response.Content}");
        }
    }
    catch (Exception ex)
    {
        throw new Exception(ex.Message);
    }
}

private AsyncRetryPolicy CreatePolicy()
{
    return Policy
    .Handle<HttpRequestException>()
    .WaitAndRetryAsync(
        retryCount: 3,
        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(2)
    );
}
```
