
using ItZnak.Infrastruction.Web.Controllers;
using MatchEngineApi.Controllers.Base;
using MatchEngineApi.Services;
using Newtonsoft.Json;
namespace MatchEngineApi.Controllers
{
    /* Структура результата запроса  */
    public class AddRQ
    {
        [JsonProperty("memberkey")]
        public string MemberKey { get; set; }

        [JsonProperty("internalkey")]
        public string InternalKey { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; } = null!;
    }

    /*======================================================================================================================================================= 
    Class: GetWizardSettingsHandler
    ======================================================================================================================================================= */

    public class AddHandler : WebApiControllerHandler<AddRQ, ApiResponse<string>>
    {
        private readonly IInboundDbService _db;
        private readonly HttpClient _httpClient;
        public AddHandler(IMatchEngineController context) : base(context)
        {
            _db = context.DbContext;
            _httpClient= new HttpClient();
            _semaphoregate = new SemaphoreSlim(2);
        }

        public override async Task<ApiResponse<string>> HandleAsync(AddRQ p)
        {
                HttpClient _httpClient = new();
                 await _semaphoregate.WaitAsync();
                 await Task.Delay(20000);
                 var response = await _httpClient.GetAsync("https://test.k6.io/contacts.php");
                 Console.WriteLine(DateTime.Now.TimeOfDay.TotalSeconds.ToString()+":" +response.StatusCode);
                _semaphoregate.Release();
                return new ApiResponse<string>() { Status = nameof(ApiResponseStatus.OK), Method = nameof(ApiMethod.ADD) };
        }

        public override ApiResponse<string> Handle(AddRQ payment)
        {
            return new ApiResponse<string>() { Status = nameof(ApiResponseStatus.OK), Method = nameof(ApiMethod.ADD) };
        }
    }
}