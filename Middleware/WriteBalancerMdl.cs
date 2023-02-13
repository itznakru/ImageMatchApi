using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchEngineApi.Middleware
{
    public class WriteBalancerMdl
    {
        private static object _lock = new Object();
        private readonly RequestDelegate _next;
        private readonly List<string> _servers;
        private int _currentServerIndex;

        public WriteBalancerMdl(RequestDelegate next)
        {
            _next = next;
            _servers = new List<string> { "Server1", "Server2", "Server3" };
            _currentServerIndex = 0;
        }


        private static void EnrichAddTemplateHandlerByHandlers(HttpContext context)
        {
            Monitor.Enter(_lock);
            // Add the current server to the request header
            context.Request.Headers["Server"] = "sfdsdf";
            Console.WriteLine(context.Request.Path);
            Monitor.Exit(_lock);
        }


        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.ToString().IndexOf("addtemplate") > -1)
                EnrichAddTemplateHandlerByHandlers(context);

            // Call the next middleware in the pipeline
            await _next(context);

        }
    }
}