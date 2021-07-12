using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Polly.Retry;

public static partial class HttpExtensions
{
    public static async Task<HttpResponseMessage> DeleteAsync(this HttpClient httpClient,
        string address, object dto, string mediaType = "application/json")
    {
        return await InternalDeleteAsync(httpClient, address, dto, mediaType);
    }

    public static async Task<HttpResponseMessage> DeleteAsync(this HttpClient httpClient, string address,
        object dto, AsyncRetryPolicy<HttpResponseMessage> policy, string mediaType = "application/json")
    {
        return await policy.ExecuteAsync(async () =>
        {
            return await InternalDeleteAsync(httpClient, address, dto, mediaType);
        });
    }

    public static async Task<HttpResponseMessage> DeleteAsync(this HttpClient httpClient, string address,
        object dto, AsyncRetryPolicy policy, string mediaType = "application/json")
    {
        return await policy.ExecuteAsync(async () =>
        {
            return await InternalDeleteAsync(httpClient, address, dto, mediaType);
        });
    }

    public static async Task<HttpExtensionResponse<T>> DeleteAsync<T>(this HttpClient httpClient, string address,
        object dto, string mediaType = "application/json")
    {
        try
        {
            return await InternalDeleteAsync<T>(httpClient, address, dto, mediaType);
        }
        catch (Exception ex)
        {
            return new HttpExtensionResponse<T>(
                HttpStatusCode.InternalServerError,
                ex);
        }
    }

    public static async Task<HttpExtensionResponse<T>> DeleteAsync<T>(this HttpClient httpClient, string address,
        object dto, AsyncRetryPolicy<HttpExtensionResponse<T>> policy, string mediaType = "application/json")
    {
        return await policy.ExecuteAsync(async () =>
        {
            return await InternalDeleteAsync<T>(httpClient, address, dto, mediaType);
        });
    }

    public static async Task<HttpExtensionResponse<T>> DeleteAsync<T>(this HttpClient httpClient, string address,
        object dto, AsyncRetryPolicy policy, string mediaType = "application/json")
    {
        return await policy.ExecuteAsync(async () =>
        {
            return await InternalDeleteAsync<T>(httpClient, address, dto, mediaType);
        });
    }

    public static async Task<HttpExtensionResponse<T>> DeleteAsync<T>(this HttpClient httpClient, string address)
    {
        try
        {
            return await InternalDeleteAsync<T>(httpClient, address);
        }
        catch (Exception ex)
        {
            return new HttpExtensionResponse<T>(
                HttpStatusCode.InternalServerError,
                ex);
        }
    }
    public static async Task<HttpExtensionResponse<T>> DeleteAsync<T>(this HttpClient httpClient, string address,
        AsyncRetryPolicy<HttpExtensionResponse<T>> policy)
    {
        return await policy.ExecuteAsync(async () =>
        {
            return await InternalDeleteAsync<T>(httpClient, address);
        });
    }

    private static async Task<HttpResponseMessage> InternalDeleteAsync(HttpClient httpClient, string address, object dto, string mediaType)
    {
        var jsonRequest = JsonConvert.SerializeObject(dto);
        var content = new StringContent(jsonRequest, Encoding.UTF8, mediaType);

        return await httpClient.DeleteAsync(address, content);
    }

    private static async Task<HttpExtensionResponse<T>> InternalDeleteAsync<T>(HttpClient httpClient, string address)
    {

        var response = await httpClient.DeleteAsync(address);
        return await GetResponse<T>(response);
    }

    private static async Task<HttpExtensionResponse<T>> InternalDeleteAsync<T>(HttpClient httpClient, string address, object dto, string mediaType)
    {
        var jsonRequest = JsonConvert.SerializeObject(dto);
        var content = new StringContent(jsonRequest, Encoding.UTF8, mediaType);

        var response = await httpClient.DeleteAsync(
            address,
            content);

        return await GetResponse<T>(response);
    }
}