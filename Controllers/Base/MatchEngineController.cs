using System.Security.Principal;
using ItZnak.Infrastruction.Services;
using ItZnak.Infrastruction.Web.Controllers;
using MatchEngineApi.Services;

namespace MatchEngineApi.Controllers.Base
{
    public class MatchEngineApiController : WebApiController, IMatchEngineController
    {
        public MatchEngineApiController(IConfigService conf, ILogService logger, IInboundDbService dbContext) : base(conf, logger)
        {
            DbContext = dbContext;
        }
        public IInboundDbService DbContext { get; }
    }
}