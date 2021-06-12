using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

public static partial class HttpExtensions
{
    internal static async Task<HttpExtensionResponse<T>> GetResponse<T>(HttpResponseMessage response)
    {
        var returnResponse = new HttpExtensionResponse<T>(response.StatusCode);
        try
        {
            returnResponse.Content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (returnResponse.Content != null)
                returnResponse.Value = JsonConvert.DeserializeObject<T>(returnResponse.Content);

        }
        catch (Exception ex)
        {
            returnResponse.Error = ex;
        }
        return returnResponse;
    }
}