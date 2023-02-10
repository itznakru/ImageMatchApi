using System.Security.Principal;
using ItZnak.Infrastruction.Services;
using MatchEngineApi.Controllers.Base;
using MatchEngineApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace MatchEngineApi.Controllers.Core
{
    [Route("{memberKey}/[controller]")]
    [ApiController]
    public class CoreController : MatchEngineApiController
    {
        private readonly IDistributeCache _cache;

        public CoreController(IConfigService conf, ILogService logger, IInboundDbService dbContext, IDistributeCache cache) : base(conf, logger, dbContext)
        {
            _cache = cache;
        }
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add(string memberKey, [FromBody] AddRQ payload)
        {
            AddHandler h = new(this);
            payload.MemberKey = memberKey;
            return Ok(await h.HandleAsync(payload));
        }

        [HttpPost]
        [Route("addtemplate")]
        public IActionResult AddTemplate(string memberKey, [FromBody] AddTemplateHandlerRQ payload)
        {
            AddTemplateHandler h = new(this, _cache);
            payload.MemberKey = memberKey;
            return Ok(h.Handle(payload));
        }

        [HttpGet]
        [Route("status")]
        public ActionResult GetStatus(string memberKey)
        {
            StatusHandler h = new(this);
            return Ok(h.Handle(memberKey));
        }
        [HttpGet]
        [Route("clear")]
        public async Task<ActionResult> Clear(string memberKey)
        {
            ClearHandler h = new(this, _cache);
            return Ok(await h.HandleAsync(memberKey));
        }
    }
}