using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItZnak.Infrastruction.Services;
using MatchEngineApi.Controllers.Tools;

namespace MatchEngineApi.Middleware
{
    public class BalancerMdl
    {
        private static object _lock = new Object();
        private readonly RequestDelegate _next;
        private readonly List<string> _servers;

        private readonly List<string> _vectorNodes;
        private readonly string _rootNodeName;
        private readonly List<string> _searchNodes;
        private readonly ILogService _log;
        private IConfigService _conf;
        public BalancerMdl(RequestDelegate next, IConfigService conf, ILogService log)
        {
            _next = next;
            _vectorNodes = conf.GetObject<List<string>>("vectorNodes");
            _searchNodes = conf.GetObject<List<string>>("searchNodes");
            _conf=conf;
            _log = log;
        }

        private void AddToHeaderExecuteNodeIpForAddTemplate(HttpContext context)
        {
            Monitor.Enter(_lock);
            // Add the current server to the request header
            context.Request.Headers["balancenode"] = _vectorNodes[0];
            Console.WriteLine(context.Request.Path);
            Monitor.Exit(_lock);
        }
        private void AddToHeaderExecuteNodeIpForCreateVector(HttpContext context)
        {
            Monitor.Enter(_lock);
            // Add the current server to the request header
            context.Request.Headers["balancenode"] = _vectorNodes[0];
            Console.WriteLine(context.Request.Path);
            Monitor.Exit(_lock);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (_conf.IsRootNode())
            {
                // Call the next middleware in the pipeline
                var hostName = System.Net.Dns.GetHostName();
                _log.Info(hostName);
                var url=context.Request.Path.ToString();

                if (url.IndexOf("addtemplate") > -1)
                    AddToHeaderExecuteNodeIpForAddTemplate(context);

                if (url.IndexOf("createvector") > -1 || url.IndexOf("add") > -1 || url.IndexOf("search") > -1 )
                    AddToHeaderExecuteNodeIpForCreateVector(context);
            }
            // Call the next middleware in the pipeline
            await _next(context);
        }
    }
}