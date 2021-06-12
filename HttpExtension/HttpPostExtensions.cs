using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Polly.Retry;

public static partial class HttpExtensions
{
    public static async Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, string address,
        object dto, string mediaType = "application/json")
    {
        return await InternalPostAsync(httpClient, address, dto, mediaType);
    }

    public static async Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, string address,
        object dto, AsyncRetryPolicy<HttpResponseMessage> policy, string mediaType = "application/json")
    {
        return await policy.ExecuteAsync(async () =>
        {
            return await InternalPostAsync(httpClient, address, dto, mediaType);
        });
    }

    public static async Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, string address,
        object dto, AsyncRetryPolicy policy, string mediaType = "application/json")
    {
        return await policy.ExecuteAsync(async () =>
        {
            return await InternalPostAsync(httpClient, address, dto, mediaType);
        });
    }

    public static async Task<HttpExtensionResponse<T>> PostAsync<T>(this HttpClient httpClient, string address)
    {
        try
        {
            return await InternalPostAsync<T>(httpClient, address);
        }
        catch (Exception ex)
        {
            return new HttpExtensionResponse<T>(
                HttpStatusCode.InternalServerError,
                ex);
        }
    }

    public static async Task<HttpExtensionResponse<T>> PostAsync<T>(this HttpClient httpClient, string address,
        AsyncRetryPolicy<HttpExtensionResponse<T>> policy)
    {
        return await policy.ExecuteAsync(async () =>
        {
            return await InternalPostAsync<T>(httpClient, address);
        });
    }

    public static async Task<HttpExtensionResponse<T>> PostAsync<T>(this HttpClient httpClient, string address,
        AsyncRetryPolicy policy)
    {
        return await policy.ExecuteAsync(async () =>
        {
            return await InternalPostAsync<T>(httpClient, address);
        });
    }

    public static async Task<HttpExtensionResponse<T>> PostAsync<T>(this HttpClient httpClient, string address,
        object dto, string mediaType = "application/json")
    {
        try
        {
            return await InternalPostAsync<T>(httpClient, address, dto, mediaType);
        }
        catch (Exception ex)
        {
            return new HttpExtensionResponse<T>(
                HttpStatusCode.InternalServerError,
                ex);
        }
    }

    public static async Task<HttpExtensionResponse<T>> PostAsync<T>(this HttpClient httpClient, string address,
        object dto, AsyncRetryPolicy<HttpExtensionResponse<T>> policy, string mediaType = "application/json")
    {
        return await policy.ExecuteAsync(async () =>
        {
            return await InternalPostAsync<T>(httpClient, address, dto, mediaType);
        });
    }

    public static async Task<HttpExtensionResponse<T>> PostAsync<T>(this HttpClient httpClient, string address,
        object dto, AsyncRetryPolicy policy, string mediaType = "application/json")
    {
        return await policy.ExecuteAsync(async () =>
        {
            return await InternalPostAsync<T>(httpClient, address, dto, mediaType);
        });
    }

    private static async Task<HttpResponseMessage> InternalPostAsync(HttpClient httpClient, string address, object dto, string mediaType)
    {
        var jsonRequest = JsonConvert.SerializeObject(dto);
        var content = new StringContent(jsonRequest, Encoding.UTF8, mediaType);

        return await httpClient.PostAsync(address, content);
    }

    private static async Task<HttpExtensionResponse<T>> InternalPostAsync<T>(HttpClient httpClient, string address)
    {
        var response = await httpClient.PostAsync(address, null);
        return await GetResponse<T>(response);
    }

    private static async Task<HttpExtensionResponse<T>> InternalPostAsync<T>(HttpClient httpClient, string address, object dto, string mediaType)
    {
        var jsonRequest = JsonConvert.SerializeObject(dto);
        var content = new StringContent(jsonRequest, Encoding.UTF8, mediaType);

        var response = await httpClient.PostAsync(
            address,
            content);

        return await GetResponse<T>(response);
    }
}