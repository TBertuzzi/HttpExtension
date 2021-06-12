using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Polly.Retry;

public static partial class HttpExtensions
{
    public static async Task<HttpResponseMessage> PutAsync(this HttpClient httpClient, string address, object dto,
        string mediaType = "application/json")
    {
        return await InternalPutAsync(httpClient, address, dto, mediaType);
    }

    public static async Task<HttpResponseMessage> PutAsync(this HttpClient httpClient, string address,
        object dto, AsyncRetryPolicy<HttpResponseMessage> policy, string mediaType = "application/json")
    {
        return await policy.ExecuteAsync(async () =>
        {
            return await InternalPutAsync(httpClient, address, dto, mediaType);
        });
    }

    public static async Task<HttpResponseMessage> PutAsync(this HttpClient httpClient, string address,
        object dto, AsyncRetryPolicy policy, string mediaType = "application/json")
    {
        return await policy.ExecuteAsync(async () =>
        {
            return await InternalPutAsync(httpClient, address, dto, mediaType);
        });
    }

    public static async Task<HttpExtensionResponse<T>> PutAsync<T>(this HttpClient httpClient, string address,
        object dto, string mediaType = "application/json")
    {
        try
        {
            return await InternalPutAsync<T>(httpClient, address, dto, mediaType);
        }
        catch (Exception ex)
        {
            return new HttpExtensionResponse<T>(
                HttpStatusCode.InternalServerError,
                ex);
        }
    }
    
    public static async Task<HttpExtensionResponse<T>> PutAsync<T>(this HttpClient httpClient, string address,
        object dto, AsyncRetryPolicy<HttpExtensionResponse<T>> policy, string mediaType = "application/json")
    {
        return await policy.ExecuteAsync(async () =>
        {
            return await InternalPutAsync<T>(httpClient, address, dto, mediaType);
        });
    }

    public static async Task<HttpExtensionResponse<T>> PutAsync<T>(this HttpClient httpClient, string address,
        object dto, AsyncRetryPolicy policy, string mediaType = "application/json")
    {
        return await policy.ExecuteAsync(async () =>
        {
            return await InternalPutAsync<T>(httpClient, address, dto, mediaType);
        });
    }

    private static async Task<HttpResponseMessage> InternalPutAsync(HttpClient httpClient, string address, object dto, string mediaType)
    {
        var jsonRequest = JsonConvert.SerializeObject(dto);
        var content = new StringContent(jsonRequest, Encoding.UTF8, mediaType);

        return await httpClient.PutAsync(address, content);
    }

    private static async Task<HttpExtensionResponse<T>> InternalPutAsync<T>(HttpClient httpClient, string address, object dto, string mediaType)
    {
        var jsonRequest = JsonConvert.SerializeObject(dto);
        var content = new StringContent(jsonRequest, Encoding.UTF8, mediaType);

        var response = await httpClient.PutAsync(
            address,
            content);

        return await GetResponse<T>(response);
    }
}