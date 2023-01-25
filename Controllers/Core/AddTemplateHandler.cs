using ItZnak.Infrastruction.Web.Controllers;
using MatchEngineApi.Controllers.Base;
using MatchEngineApi.DTO;
using MatchEngineApi.Services;
using Newtonsoft.Json;
using MatchEngineApi.Controllers.Tools;
using MatchEngineApi.Controllers.Exceptions;
using ItZnak.Infrastruction.Services;

namespace MatchEngineApi.Controllers
{
    public class AddTemplateHandlerRQ
    {
        [JsonProperty("memberkey")]
        public string? MemberKey { get; set; }
        [JsonProperty("internalkey")]
        public string InternalKey { get; set; }
        [JsonProperty("image")]
        public string Image { get; set; } = null!;
        [JsonProperty("template")]
        public string Template { get; set; } = null!;
        [JsonProperty("imagetype")]
        public ImageType ImageType { get; set; }
    }

    public class AddTemplateHandler : WebApiControllerHandler<AddTemplateHandlerRQ, ApiResponse<string>>
    {
        const string METHOD_NAME = "addtemplate";
        private readonly IInboundDbService _db;
        private readonly IDistributeCache _cache;
        public AddTemplateHandler(IMatchEngineController context, IDistributeCache cache) : base(context)
        {
            _db = context.DbContext;
            _cache = cache;
        }

        private bool IsExistInDbAsync(string memberKey, string intervalKey)
        {
            if (string.IsNullOrEmpty(intervalKey))
                return true;

            return _db.VECTORS.Any(_ => _.MemberKey == memberKey && _.InternalKey == intervalKey);
        }
        private void AddToCache(AddTemplateHandlerRQ p){
              _cache.Set(ToolsExtentions.BuildCacheKey(p.MemberKey, p.InternalKey), Convert.FromBase64String(p.Template));
        }

        public override async Task<ApiResponse<string>> HandleAsync(AddTemplateHandlerRQ p)
        {
            if (!p.Image.IsBase64StringAnImage()) throw new MatchEngineApiException(METHOD_NAME, "Parametr image wrong. Is not an image.");
            if (!p.Template.IsBase64StringAnDoubleArray()) throw new MatchEngineApiException(METHOD_NAME, "Parametr template wrong. Is not an vector.");
            if (IsExistInDbAsync(p.MemberKey, p.InternalKey)) throw new MatchEngineApiException(METHOD_NAME, "Record with key " + p.InternalKey + " exists in DB");
            AddToCache(p);

            await _db.VECTORS.AddAsync(new DTO.VectorDto()
            {
                MemberKey = p.MemberKey ?? "",
                Image = p.Image,
                InternalKey = p.InternalKey,
                ImageType = p.ImageType,
                Vector = Convert.FromBase64String(p.Template)
            });

            await _db.CTX.SaveChangesAsync();
            return new ApiResponse<string>() { Method = METHOD_NAME, Status = nameof(ApiResponseStatus.OK) };
        }
    }
}