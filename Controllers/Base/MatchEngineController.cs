using System.Security.Principal;
using ItZnak.Infrastruction.Services;
using ItZnak.Infrastruction.Web.Controllers;
using MatchEngineApi.Controllers.Exceptions;
using MatchEngineApi.Services;
using Newtonsoft.Json;

namespace MatchEngineApi.Controllers.Base
{
    public class MatchEngineApiController : WebApiController, IMatchEngineController
    {
        private static readonly HttpClient client = new();

        protected readonly List<string> _vectorNodes;
        protected readonly List<string> _searchNodes;
        public MatchEngineApiController(IConfigService conf, ILogService logger, IInboundDbService dbContext) : base(conf, logger)
        {

            DbContext = dbContext;
            _vectorNodes = conf.GetObject<List<string>>("vectorNodes");
            _searchNodes = conf.GetObject<List<string>>("searchNodes");
        }
        public IInboundDbService DbContext { get; }

      

        public async Task<T> CallGetRemoteNodeAsync<T>(string url)
        {
            try
            {
                string responseBody = await client.GetStringAsync(url);
                return JsonConvert.DeserializeObject<T>(responseBody);
            }
            catch (Exception ex)
            {
                _logger.Exception(ex);
                throw new RemoteServerCallException(ex.ToString());
            }
        }

        public string GetBalanceNodeIp(){
            this.Request.Headers.TryGetValue("balancenode", out Microsoft.Extensions.Primitives.StringValues rslt);
            return rslt.FirstOrDefault();
        }
        public async Task<TOutput> CallPostRemoteNodeAsync<TInput, TOutput>(string url, TInput payload)
        {
            try
            {

                var json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);
                var responseContent = await response.Content.ReadAsStringAsync();
                var output = JsonConvert.DeserializeObject<TOutput>(responseContent);
                return output;
            }
            catch (Exception ex)
            {
                _logger.Exception(ex);
                throw new RemoteServerCallException(ex.ToString());
            }
        }
    }
}