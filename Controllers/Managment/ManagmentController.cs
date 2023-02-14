using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastraction.Services.MemoryCache;
using ItZnak.Infrastruction.Services;
using MatchEngineApi.Controllers.Base;
using MatchEngineApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace MatchEngineApi.Controllers.Managment
{
    [Route("[controller]")]
    [ApiController]
    public class ManagmentController : MatchEngineApiController
    {
        private readonly IMemoryCache<byte[]> _cache;

        public ManagmentController(IConfigService conf, ILogService logger, IInboundDbService dbContext, IMemoryCache<byte[]> cache) : base(conf, logger, dbContext)
        {

            _cache = cache;
        }


        [HttpGet]
        [Route("status")]
        public ActionResult GetStatus()
        {
            Console.WriteLine(this.IsRootNode);
            StatusHandler h = new(this);
            return Ok(h.Handle(HttpContext.Connection.RemoteIpAddress.ToString()));
        }

        [HttpGet]
        [Route("clear")]
        public async Task<ActionResult> Clear([FromQuery] string memberKey)
        {
            ClearHandler h = new(this, _cache);
            return Ok(await h.HandleAsync(memberKey));

        }
    }
}