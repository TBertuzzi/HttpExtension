using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public static class HttpExtensionHelper
{
    public static async Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, string address,
        object dto, string mediaType = "application/json")
    {
        var jsonRequest = JsonConvert.SerializeObject(dto);
        var content = new StringContent(jsonRequest, Encoding.UTF8, mediaType);

        return await httpClient.PostAsync(address, content);
    }


    public static async Task<HttpExtensionResponse<T>> PostAsync<T>(this HttpClient httpClient, string address,
        object dto, string mediaType = "application/json")
    {
        try
        {
            var jsonRequest = JsonConvert.SerializeObject(dto);
            var content = new StringContent(jsonRequest, Encoding.UTF8, mediaType);

            var response = await httpClient.PostAsync(
                address,
                content);

            return await GetResponse<T>(response);
        }
        catch (Exception ex)
        {
            return new HttpExtensionResponse<T>(
                HttpStatusCode.InternalServerError,
                ex);
        }
    }

    public static async Task<HttpResponseMessage> PutAsync(this HttpClient httpClient, string address, object dto,
        string mediaType = "application/json")
    {
        var jsonRequest = JsonConvert.SerializeObject(dto);
        var content = new StringContent(jsonRequest, Encoding.UTF8, mediaType);

        return await httpClient.PutAsync(address, content);
    }


    public static async Task<HttpExtensionResponse<T>> PutAsync<T>(this HttpClient httpClient, string address,
        object dto, string mediaType = "application/json")
    {
        try
        {
            var jsonRequest = JsonConvert.SerializeObject(dto);
            var content = new StringContent(jsonRequest, Encoding.UTF8, mediaType);

            var response = await httpClient.PutAsync(
                address,
                content);

            return await GetResponse<T>(response);
        }
        catch (Exception ex)
        {
            return new HttpExtensionResponse<T>(
                HttpStatusCode.InternalServerError,
                ex);
        }
    }

    public static async Task<HttpResponseMessage> DeleteAsync(this HttpClient httpClient,
        string address, object dto, string mediaType = "application/json")
    {
        var jsonRequest = JsonConvert.SerializeObject(dto);
        var content = new StringContent(jsonRequest, Encoding.UTF8, mediaType);

        return await httpClient.DeleteAsync(address, content);
    }


    public static async Task<HttpExtensionResponse<T>> DeleteAsync<T>(this HttpClient httpClient, string address,
        object dto, string mediaType = "application/json")
    {
        try
        {
            var jsonRequest = JsonConvert.SerializeObject(dto);
            var content = new StringContent(jsonRequest, Encoding.UTF8, mediaType);

            var response = await httpClient.DeleteAsync(
                address,
                content);

            return await GetResponse<T>(response);
        }
        catch (Exception ex)
        {
            return new HttpExtensionResponse<T>(
                HttpStatusCode.InternalServerError,
                ex);
        }
    }


    public static async Task<HttpExtensionResponse<T>> GetAsync<T>(this HttpClient httpClient, string address)
    {
        try
        {
            var response = await httpClient.GetAsync(address).ConfigureAwait(false);
            return await GetResponse<T>(response);
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
            var builder = new StringBuilder();

            foreach (var pair in values)
            {
                builder.Append($"&{pair.Key}={pair.Value}");
            }

            var url = $"{address}?{builder.ToString().Substring(1)}";
            var response = await httpClient.GetAsync(url).ConfigureAwait(false);
            return await GetResponse<T>(response);
        }
        catch (Exception ex)
        {
            return new HttpExtensionResponse<T>(
                HttpStatusCode.InternalServerError,
                ex);
        }
    }

    private static async Task<HttpExtensionResponse<T>> GetResponse<T>(HttpResponseMessage response)
    {
        var returnResponse = new HttpExtensionResponse<T>(response.StatusCode);

        try
        {
            returnResponse.Content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (returnResponse.Content != null)
            {
                returnResponse.Value = JsonConvert.DeserializeObject<T>(returnResponse.Content);
            }

        }
        catch (Exception ex)
        {
            returnResponse.Error = ex;
        }

        return returnResponse;
    }
}
