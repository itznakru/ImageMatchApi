using ItZnak.Infrastruction.Web.Controllers;
using MatchEngineApi.Services;

namespace MatchEngineApi.Controllers.Base
{
    public interface IMatchEngineController : IMWebApiController
    {
        IInboundDbService DbContext { get; }
        TOUT CallPostRemoteNode<TOUT,TIN> (TIN param);
        TOUT CallGetRemoteNode<TOUT> (string param);
    }
}