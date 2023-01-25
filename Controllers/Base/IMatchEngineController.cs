using ItZnak.Infrastruction.Web.Controllers;
using MatchEngineApi.Services;

namespace MatchEngineApi.Controllers.Base
{
    public interface IMatchEngineController : IMWebApiController
    {
        IInboundDbService DbContext { get; }
    }
}