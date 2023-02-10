using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ItZnak.Infrastruction.Services;
using MatchEngineApi.Controllers.Base;
using MatchEngineApi.Controllers.Exceptions;

namespace MatchEngineApi.Middleware
{
    public class ExceptionHandlerMdl
    {
        readonly RequestDelegate _next;
        readonly ILogService _log;

        public ExceptionHandlerMdl(RequestDelegate next, ILogService log)
        {
            _log = log;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (MatchEngineApiException ex)
            {
                _log.Info(ex.Method+":"+ String.Join(",",ex.ErrorList));
                await context.Response.WriteAsync(ExcepFormatToJson(ex));
            }
            catch (Exception ex)
            {
                var s=ex.ToString();
                _log.Exception(s);
                context.Response.StatusCode = 500;
                await context.Response
                            .WriteAsync(
                                ExcepFormatToJson(new MatchEngineApiException(ApiMethod.UNKNOWN,s))
                            );
            }
        }

        static string ExcepFormatToJson(MatchEngineApiException ex)
        {
            ApiResponse<string> rslt = new()
            {
                Status=nameof(ApiResponseStatus.ERROR),
                Method = ex.Method ?? "CRITICAL EXCEPTION",
                Error = ex.ErrorList.ToArray()
            };
            return JsonSerializer.Serialize(rslt);
        }
    }
}