using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Polly.Retry;

public static partial class HttpExtensions
{
    public static async Task<HttpExtensionResponse<T>> SendAsync<T>(this HttpClient httpClient, HttpRequestMessage request)
    {
        try
        {
            return await InternalSendAsync<T>(httpClient, request);
        }
        catch (Exception ex)
        {
            return new HttpExtensionResponse<T>(
                HttpStatusCode.InternalServerError,
                ex);
        }
    }

    public static async Task<HttpExtensionResponse<T>> SendAsync<T>(this HttpClient httpClient, HttpRequestMessage request,
        AsyncRetryPolicy<HttpExtensionResponse<T>> policy)
    {
        return await policy.ExecuteAsync(async () =>
        {
            return await InternalSendAsync<T>(httpClient, request);
        });
    }

    public static async Task<HttpExtensionResponse<T>> SendAsync<T>(this HttpClient httpClient, HttpRequestMessage request,
        AsyncRetryPolicy policy)
    {
        return await policy.ExecuteAsync(async () =>
        {
            return await InternalSendAsync<T>(httpClient, request);
        });
    }

    private static async Task<HttpExtensionResponse<T>> InternalSendAsync<T>(HttpClient httpClient, HttpRequestMessage request)
    {
        var response = await httpClient.SendAsync(request).ConfigureAwait(false);
        return await GetResponse<T>(response);
    }
}