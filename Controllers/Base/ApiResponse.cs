using Newtonsoft.Json;

namespace MatchEngineApi.Controllers.Base
{
    public enum ApiResponseStatus { OK, ERROR }
    public enum ApiMethod { ADD, ADDTEMPLATE }
    public class ApiResponse<T>
    {

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