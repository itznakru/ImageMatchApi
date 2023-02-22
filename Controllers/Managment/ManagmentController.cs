using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastraction.Services.MemoryCache;
using ItZnak.Infrastruction.Services;
using MatchEngineApi.Controllers.Base;
using MatchEngineApi.Controllers.Tools;
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

        /* For root node asked all child nodes and  results should union in one. For child node only execute handler */
        [HttpGet]
        [Route("status")]
        public async Task<ActionResult> GetStatus()
        {
            string myIp = HttpContext.Connection.RemoteIpAddress.ToString();

            Func<List<Task<StatusNodeRS[]>>> buildTaskList = () =>
            {
                List<Task<StatusNodeRS[]>> taskList = new List<Task<StatusNodeRS[]>>();
                foreach (var url in _searchNodes)
                {
                    if (url.IndexOf(myIp) == -1)
                        taskList.Add(this.CallGetRemoteNodeAsync<StatusNodeRS[]>($"http://{url}/managment/status"));
                }

                return taskList;
            };

            StatusHandler h = new(this);
            
            /* IF ROOT NODE MODE RUN ALL OTHER NODES */
            if (_conf.IsRootNode())
            {
                /* forming list of task */
                var childNodesGetStatusTasks = buildTaskList();
                var currentNodeGetStatusTask = Task.Run(() => h.Handle(myIp));
                childNodesGetStatusTasks.Add(currentNodeGetStatusTask);
                /* wait */
                Task.WaitAll(childNodesGetStatusTasks.ToArray());

                /* union results */
                List<StatusNodeRS> rslt = new();
                childNodesGetStatusTasks.ForEach(t => rslt.AddRange(t.Result));
                return Ok(rslt.ToArray());
            }
            else
            {
                return Ok(h.Handle(myIp));
            }
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