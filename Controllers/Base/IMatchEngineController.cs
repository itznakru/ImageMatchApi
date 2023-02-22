using ItZnak.Infrastruction.Web.Controllers;
using MatchEngineApi.Services;

namespace MatchEngineApi.Controllers.Base
{
    public interface IMatchEngineController : IMWebApiController
    {
        IInboundDbService DbContext { get; }
        Task<T> CallGetRemoteNodeAsync<T>(string url);
        Task<TOutput> CallPostRemoteNodeAsync<TInput, TOutput>(string url, TInput param );
    }
}