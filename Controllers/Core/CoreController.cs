using System.Security.Principal;
using Infrastraction.Services.MemoryCache;
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
        private readonly IMemoryCache<byte[]> _cache;

        public CoreController(IConfigService conf, ILogService logger, IInboundDbService dbContext, IMemoryCache<byte[]> cache) : base(conf, logger, dbContext)
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
            AddTemplateHandler h = new(this, _cache, _conf);
            payload.MemberKey = memberKey;
            return Ok(h.Handle(payload));
        }
    }
}