
using Infrastraction.Services.MemoryCache;
using ItZnak.Infrastruction.Web.Controllers;
using MatchEngineApi.Controllers.Base;
using MatchEngineApi.Controllers.Core;
using MatchEngineApi.Services;
using MathNet.Numerics;
using Newtonsoft.Json;
namespace MatchEngineApi.Controllers
{
    /* Структура результата запроса  */
    public class SearchRQ
    {
        [JsonProperty("memberkey")]
        public string MemberKey { get; set; } = null!;

        [JsonProperty("image")]
        public string Image { get; set; }


    }

    /*======================================================================================================================================================= 
    Class: GetWizardSettingsHandler
    ======================================================================================================================================================= */

    public class SearchHandler : WebApiControllerHandler<SearchRQ, ApiResponse<string>>
    {
        readonly string _vectorServerIp;
        private readonly HttpClient _httpClient;
        readonly IMatchEngineController _controller;
        readonly  IMemoryCache<double[]> _cache;
        public SearchHandler(IMatchEngineController context, IMemoryCache<double[]>  cache, string vectorServerIp) : base(context)
        {
            _vectorServerIp = vectorServerIp;
            _httpClient = new HttpClient();
            _controller = context as IMatchEngineController;
            _cache=cache;
        }

        public override async Task<ApiResponse<string>> HandleAsync(SearchRQ p)
        {

            string vectorNodeUrl = "http://" + _vectorServerIp;
            _context.Log.Info("Try call create vector by:" + _vectorServerIp);
            var vector = await _controller.CallPostRemoteNodeAsync<string, VectorNodeResponse>(vectorNodeUrl, p.Image); 
            foreach (var key in _cache.PartionKeys(p.MemberKey))
            {
                   double d=Distance.Cosine(_cache.Get(p.MemberKey,key),vector.ImgVector);
                   Console.WriteLine(d);
            }
            // HttpClient _httpClient = new();
            //  await _semaphoregate.WaitAsync();
            //  await Task.Delay(20000);
            //  var response = await _httpClient.GetAsync("https://test.k6.io/contacts.php");
            //  Console.WriteLine(DateTime.Now.TimeOfDay.TotalSeconds.ToString()+":" +response.StatusCode);
            // _semaphoregate.Release();
            return new ApiResponse<string>()
            {
                Status = nameof(ApiResponseStatus.OK),
                Method = nameof(ApiMethod.SEARCH)
            };
        }

    }
}