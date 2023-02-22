using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItZnak.Infrastruction.Web.Controllers;
using MatchEngineApi.Controllers.Base;
using Newtonsoft.Json;
using MatchEngineApi.Controllers.Tools;

namespace MatchEngineApi.Controllers.Core
{
    public class CreateVectorHandlerRQ
    {
        [JsonProperty("memberkey")]
        public string MemberKey { get; set; } = null!;
        [JsonProperty("internalkey")]
        public string InternalKey { get; set; } = null!;
        [JsonProperty("image")]
        public string Image { get; set; } = null!;
    }
    public class VectorNodeResponse
    {
        public double[] ImgVector { get; set; }
    }
    public class CreateVectorHandler : WebApiControllerHandler<CreateVectorHandlerRQ, ApiResponse<string>>
    {
        readonly string _vectorServerIp;
        readonly IMatchEngineController _controller;
        public CreateVectorHandler(IMWebApiController context, string vectorServerIp) : base(context)
        {
            _vectorServerIp = vectorServerIp;
            _controller = context as IMatchEngineController;
        }


        public override async Task<ApiResponse<string>> HandleAsync(CreateVectorHandlerRQ p)
        {
            string vectorNodeUrl = "http://" + _vectorServerIp;
            _context.Log.Info("Try call create vector by:" + _vectorServerIp);
            var vector = await _controller.CallPostRemoteNodeAsync<string, VectorNodeResponse>(vectorNodeUrl, p.Image);
            return new ApiResponse<string>(ApiMethod.CREATEVECTOR)
            {
                Status = nameof(ApiResponseStatus.OK),
                Result = new string[] { vector.ImgVector.ToJson() }
            };
        }
    }
}