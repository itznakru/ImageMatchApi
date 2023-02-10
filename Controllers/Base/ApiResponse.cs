using Newtonsoft.Json;

namespace MatchEngineApi.Controllers.Base
{
    public enum ApiResponseStatus { OK, ERROR }
    public enum ApiMethod { UNKNOWN, ADD, ADDTEMPLATE, CLEAR }
    public class ApiResponse<T>
    {
        public ApiResponse()
        {
        }

        public ApiResponse(ApiMethod method)
        {
            Method = method.ToString();
        }

        public ApiResponse(ApiMethod method, ApiResponseStatus status)
        {
            Method = method.ToString();
            Status = status.ToString();
        }

        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("error")]
        public string[] Error { get; set; } = Array.Empty<string>();
        [JsonProperty("method")]
        public string Method { get; set; }
        [JsonProperty("result")]
        public T[] Result { get; set; } = Array.Empty<T>();
    }
}