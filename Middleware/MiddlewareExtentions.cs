using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItZnak.Infrastruction.Services;

namespace MatchEngineApi.Middleware
{
    public static class MiddlewareExtentions
    {
          public static IApplicationBuilder UseExceptionHandlerMdl(this IApplicationBuilder builder)
            {
                return builder.UseMiddleware<ExceptionHandlerMdl>();
            }
            public static IApplicationBuilder UseWriteBalancerMdl(this IApplicationBuilder builder
            , IConfigService configService,
            ILogService logService )
            {
                return builder.UseMiddleware<BalancerMdl>(configService,logService);
            }
    }
}