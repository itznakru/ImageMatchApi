using System.Security.Principal;
using ItZnak.Infrastruction.Services;
using MatchEngineApi.Controllers.Base;
using MatchEngineApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace MatchEngineApi.Controllers.Core{
    [Route("{memberKey}/[controller]")]
    [ApiController]
    public class CoreController : MatchEngineApiController
    {
        private readonly IDistributeCache _cache;
        public CoreController(IConfigService conf, ILogService logger, IInboundDbService dbContext,IDistributeCache cache) : base(conf, logger, dbContext)
        {
            _cache=cache;
        }
        [HttpPost]
        [Route("add")]
        public IActionResult Add(string memberKey, [FromBody]AddRQ payload)
        {
            AddHandler h = new(this);
            payload.memberkey= memberKey;
            return  this.Ok(h.Handle(payload));
        }

        [HttpPost]
        [Route("addtemplate")]
        public async Task<IActionResult> AddTemplateAsync(string memberKey, [FromBody]AddTemplateHandlerRQ payload)
        {
            AddTemplateHandler h = new(this,_cache);
            payload.MemberKey=memberKey;
            return  this.Ok(await h.HandleAsync(payload));
        }
    }
}