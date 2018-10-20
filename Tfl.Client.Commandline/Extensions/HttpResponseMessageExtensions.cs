using Newtonsoft.Json;
using System.Threading.Tasks;
using Tfl.Client.Commandline.Dtos;

namespace System.Net.Http
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<ApiResponse<T>> GetResponseObject<T>(this HttpResponseMessage httpResponseMessage) where T : class
        {
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var responseString = await httpResponseMessage.Content.ReadAsStringAsync();

                if (responseString.IndexOf("Your authentication credentials were missing or incorrect") >= 0)
                {
                    return new ApiResponse<T>(null, new ApiError() { Message = "Your authentication credentials were missing or incorrect." });
                }
                else
                {
                    var response = JsonConvert.DeserializeObject<T>(responseString);
                    return new ApiResponse<T>(response, null);
                }
            }
            else
            {
                var responseString = await httpResponseMessage.Content.ReadAsStringAsync();

                if ((int)httpResponseMessage.StatusCode == 429)
                {
                    return new ApiResponse<T>(null, new ApiError() { Message = responseString });
                }
                else
                {
                    var apiError = JsonConvert.DeserializeObject<ApiError>(responseString);
                    return new ApiResponse<T>(null, apiError);
                }
            }
        }
    }
}