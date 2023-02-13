using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchEngineApi.Middleware
{
    public static class MiddlewareExtentions
    {
          public static IApplicationBuilder UseExceptionHandlerMdl(this IApplicationBuilder builder)
            {
                return builder.UseMiddleware<ExceptionHandlerMdl>();
            }
            public static IApplicationBuilder UseWriteBalancerMdl(this IApplicationBuilder builder)
            {
                return builder.UseMiddleware<WriteBalancerMdl>();
            }
    }
}