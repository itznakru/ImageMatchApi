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
    }
}