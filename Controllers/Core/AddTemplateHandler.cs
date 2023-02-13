using ItZnak.Infrastruction.Web.Controllers;
using MatchEngineApi.Controllers.Base;
using MatchEngineApi.DTO;
using MatchEngineApi.Services;
using Newtonsoft.Json;
using MatchEngineApi.Controllers.Tools;
using MatchEngineApi.Controllers.Exceptions;
using ItZnak.Infrastruction.Services;
using Infrastraction.Services.MemoryCache;

namespace MatchEngineApi.Controllers
{
    public class AddTemplateHandlerRQ
    {
        [JsonProperty("memberkey")]
        public string MemberKey { get; set; }
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
        private readonly IInboundDbService _db;
        private readonly IMemoryCache<byte[]>  _cache;
        private readonly ILogService _log;

        public AddTemplateHandler(IMatchEngineController context, IMemoryCache<byte[]>  cache) : base(context)
        {
            _db = context.DbContext;
            _cache = cache;
            _log = _context.Log;
            _semaphoregate = new SemaphoreSlim(1);
        }

        private bool IsExistInDb(string memberKey, string intervalKey)
        {
            if (string.IsNullOrEmpty(intervalKey))
                return true;

            return _db.VECTORS.Any(_ => _.MemberKey == memberKey && _.InternalKey == intervalKey);
        }
        private void TryAddToCache(AddTemplateHandlerRQ p)
        {
            var key = ToolsExtentions.BuildCacheKey(p.MemberKey, p.InternalKey);
            if (!_cache.IsExists(key) )
            {
                _cache.Set(key, Convert.FromBase64String(p.Template));
                _log.Info($"add vector {key} to cache ");
            }
        }

        public override ApiResponse<string> Handle(AddTemplateHandlerRQ p){
            var key = ToolsExtentions.BuildCacheKey(p.MemberKey, p.InternalKey);
            if (!p.Image.IsBase64StringAnImage()) throw new MatchEngineApiException(ApiMethod.ADDTEMPLATE, "Parametr 'Image' wrong. Is not an image.");
            if (!p.Template.IsBase64StringAnDoubleArray()) throw new MatchEngineApiException(ApiMethod.ADDTEMPLATE, "Parametr 'Template' wrong. Is not an vector.");
            if (IsExistInDb(p.MemberKey, p.InternalKey)) throw new MatchEngineApiException(ApiMethod.ADDTEMPLATE, "Record with key " + p.InternalKey + " exists in DB");

             _semaphoregate.Wait();
            /* ADD RECORD TO CACHE */
            TryAddToCache(p);

             _db.VECTORS.Add(new DTO.VectorDto()
            {
                MemberKey = p.MemberKey ?? "",
                Image = p.Image,
                InternalKey = p.InternalKey,
                ImageType = p.ImageType,
                Vector = Convert.FromBase64String(p.Template)
            });

             _db.CTX.SaveChanges();
            _semaphoregate.Release();

            _log.Info($"add vector {key}, MemberKey:{p.MemberKey}, InternalKey:{p.InternalKey} ");
            return new ApiResponse<string>(ApiMethod.ADDTEMPLATE) { Status = nameof(ApiResponseStatus.OK) };
        }
    }
}