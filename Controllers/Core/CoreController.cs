using System.Security.Principal;
using Infrastraction.Services.MemoryCache;
using ItZnak.Infrastruction.Services;
using MatchEngineApi.Controllers.Base;
using MatchEngineApi.Controllers.Exceptions;
using MatchEngineApi.Controllers.Tools;
using MatchEngineApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace MatchEngineApi.Controllers.Core
{
    [Route("{memberKey}/[controller]")]
    [ApiController]
    public class CoreController : MatchEngineApiController
    {
        private readonly IMemoryCache<double[]> _cache;

        public CoreController(IConfigService conf, ILogService logger, IInboundDbService dbContext, IMemoryCache<double[]> cache) : base(conf, logger, dbContext)
        {
            _cache = cache;
        }
        [HttpPost]
        [Route("createvector")]
        public async Task<IActionResult> CreateVectorAsync(string memberKey, [FromBody] CreateVectorHandlerRQ payload)
        {
            if (!_conf.IsRootNode())
                throw new MatchEngineApiException(ApiMethod.CREATEVECTOR, "I CAN'T SERVE THIS REQUEST. I AM NOT ROOT NODE");
            payload.MemberKey = memberKey;
            CreateVectorHandler h = new(this, GetBalanceNodeIp());
            return Ok(await h.HandleAsync(payload));
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddAsync(string memberKey, [FromBody] CreateVectorHandlerRQ payload)
        {
            if (!_conf.IsRootNode())
                throw new MatchEngineApiException(ApiMethod.CREATEVECTOR, "I CAN'T SERVE THIS REQUEST. I AM NOT ROOT NODE");

            CreateVectorHandler h = new(this, GetBalanceNodeIp());
            var vector = await h.HandleAsync(payload);

            if (vector.Status != nameof(ApiResponseStatus.OK))
                throw new MatchEngineApiException(ApiMethod.CREATEVECTOR, "Attempt to form a vector failed");

            AddTemplateHandler addHandler = new(this, _cache, _conf);
            AddTemplateHandlerRQ p = new();
            p.Image = payload.Image;
            p.ImageType = DTO.ImageType.None;
            p.InternalKey = payload.InternalKey;
            p.MemberKey = memberKey;
            p.Template = vector.Result[0];
            return this.Ok(addHandler.Handle(p));
        }

        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> SearchAsync(string memberKey, [FromBody] SearchRQ payload)
        {
            payload.MemberKey=memberKey;
            SearchHandler h = new(this,_cache, GetBalanceNodeIp());
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