using System.Security.Principal;
using ItZnak.Infrastruction.Services;
using ItZnak.Infrastruction.Web.Controllers;
using MatchEngineApi.Services;

namespace MatchEngineApi.Controllers.Base
{
    public class MatchEngineApiController : WebApiController, IMatchEngineController
    {
        protected readonly string _rootNodeIp;
        protected readonly List<string> _vectorNodes;
        protected readonly List<string> _searchNodes;
        public MatchEngineApiController(IConfigService conf, ILogService logger, IInboundDbService dbContext) : base(conf, logger)
        {

            DbContext = dbContext;
            _rootNodeIp = _conf.GetString("rootNode");
            _vectorNodes = conf.GetObject<List<string>>("vectorNodes");
            _searchNodes = conf.GetObject<List<string>>("searchNodes");
        }
        public IInboundDbService DbContext { get; }

        protected bool IsRootNode { get { return _rootNodeIp.IndexOf(this.HttpContext.Connection.RemoteIpAddress.ToString()) > -1; } }

        public TOUT CallGetRemoteNode<TOUT>(string param)
        {
            throw new NotImplementedException();
        }

        public TOUT CallPostRemoteNode<TOUT, TIN>(TIN param)
        {
            throw new NotImplementedException();
        }
    }
}