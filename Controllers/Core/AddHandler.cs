
using ItZnak.Infrastruction.Web.Controllers;
using MatchEngineApi.Controllers.Base;
using MatchEngineApi.Services;

namespace MatchEngineApi.Controllers
{
    /* Структура результата запроса  */
    public class AddRQ
    {
       public string? memberkey {get;set;}
       public string internalkey {get;set;}
       public string image {get;set;}=null!;
    }

    /*======================================================================================================================================================= 
    Class: GetWizardSettingsHandler
    ======================================================================================================================================================= */

    public class AddHandler : WebApiControllerHandler<AddRQ, ApiResponse<string>>
    {
        private readonly IInboundDbService _db;
        public AddHandler(IMatchEngineController context) : base(context)
        {
            _db = context.DbContext;
        }
        /*      
           
        */
        public override ApiResponse<string>  Handle(AddRQ payment)
        {
          return new ApiResponse<string>(){Status=nameof(ApiResponseStatus.OK), Method=nameof(ApiMethod.ADD)};
        }
    }
}