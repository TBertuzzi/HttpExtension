using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Polly.Retry;

public static partial class HttpExtensions
{
    public static async Task<HttpExtensionResponse<T>> GetAsync<T>(this HttpClient httpClient, string address)
    {
        try
        {
            return await InternalGetAsync<T>(httpClient, address);
        }
        catch (Exception ex)
        {
            return new HttpExtensionResponse<T>(
                HttpStatusCode.InternalServerError,
                ex);
        }
    }

    public static async Task<HttpExtensionResponse<T>> GetAsync<T>(this HttpClient httpClient, string address,
        AsyncRetryPolicy<HttpExtensionResponse<T>> policy)
    {
        try
        {
            return await policy.ExecuteAsync(async () =>
            {
                return await InternalGetAsync<T>(httpClient, address);
            });
        }
        catch (Exception ex)
        {
            return new HttpExtensionResponse<T>(
                HttpStatusCode.InternalServerError,
                ex);
        }
    }

    public static async Task<HttpExtensionResponse<T>> GetAsync<T>(this HttpClient httpClient, string address,
        AsyncRetryPolicy policy)
    {
        try
        {
            return await policy.ExecuteAsync(async () =>
            {
                return await InternalGetAsync<T>(httpClient, address);
            });
        }
        catch (Exception ex)
        {
            return new HttpExtensionResponse<T>(
                HttpStatusCode.InternalServerError,
                ex);
        }
    }

    public static async Task<HttpExtensionResponse<T>> GetAsync<T>(this HttpClient httpClient, string address,
        Dictionary<string, string> values)
    {
        try
        {
            return await InternalGetAsync<T>(httpClient, address, values);
        }
        catch (Exception ex)
        {
            return new HttpExtensionResponse<T>(
                HttpStatusCode.InternalServerError,
                ex);
        }
    }

    public static async Task<HttpExtensionResponse<T>> GetAsync<T>(this HttpClient httpClient, string address,
        Dictionary<string, string> values, AsyncRetryPolicy<HttpExtensionResponse<T>> policy)
    {
        try
        {
            return await policy.ExecuteAsync(async () =>
            {
                return await InternalGetAsync<T>(httpClient, address, values);
            });
        }
        catch (Exception ex)
        {
            return new HttpExtensionResponse<T>(
                HttpStatusCode.InternalServerError,
                ex);
        }
    }

    public static async Task<HttpExtensionResponse<T>> GetAsync<T>(this HttpClient httpClient, string address,
        Dictionary<string, string> values, AsyncRetryPolicy policy)
    {
        try
        {
            return await policy.ExecuteAsync(async () =>
            {
                return await InternalGetAsync<T>(httpClient, address, values);
            });
        }
        catch (Exception ex)
        {
            return new HttpExtensionResponse<T>(
                HttpStatusCode.InternalServerError,
                ex);
        }
    }

    private static async Task<HttpExtensionResponse<T>> InternalGetAsync<T>(HttpClient httpClient, string address)
    {
        var response = await httpClient.GetAsync(address).ConfigureAwait(false);
        return await GetResponse<T>(response);
    }

    private static async Task<HttpExtensionResponse<T>> InternalGetAsync<T>(HttpClient httpClient, string address,
        Dictionary<string, string> values)
    {
        var builder = new StringBuilder();

        foreach (var pair in values)
            builder.Append($"&{pair.Key}={pair.Value}");

        var url = $"{address}?{builder.ToString().Substring(1)}";
        var response = await httpClient.GetAsync(url).ConfigureAwait(false);
        return await GetResponse<T>(response);
    }
}