using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItZnak.Infrastruction.Services;
using ItZnak.Infrastruction.Web.Controllers;
using MatchEngineApi.Controllers.Base;
using MatchEngineApi.Controllers.Tools;
using MatchEngineApi.Services;
using Microsoft.EntityFrameworkCore;

namespace MatchEngineApi.Controllers.Core
{
    public class ClearHandler : WebApiControllerHandler<string, ApiResponse<Boolean>>
    {
        private readonly IInboundDbService _db;
        private readonly IDistributeCache _cache;
        private readonly ILogService _log;

        public ClearHandler(IMatchEngineController context, IDistributeCache cache) : base(context)
        {
            _db = context.DbContext;
            _cache = cache;
            _log = _context.Log;
        }

        private void ClearCache(List<DTO.VectorDto> vectors)
        {
            if(vectors.Count==0)
                return;

            vectors.ForEach(v =>
            {
                var key = ToolsExtentions.BuildCacheKey(v.MemberKey, v.InternalKey);
                if (_cache.Get(key) != null)
                    _cache.Remove(key);
            });
              _log.Info($"removed from cache  {vectors.Count} by memberKey:{vectors[0].MemberKey}");
        }

        public async override Task<ApiResponse<bool>> HandleAsync(string memberKey)
        {
            /* get record keys */
            _log.Info($"start clear process for {memberKey}");
             List<DTO.VectorDto> vectorsForDestroy = await _db.VECTORS.Where(_ => _.MemberKey == memberKey)
                                      .Select(p => new DTO.VectorDto() { Id = p.Id, InternalKey = p.InternalKey })
                                      .ToListAsync();

            /* clear cache */
            ClearCache(vectorsForDestroy);

            /* clear db */
            _db.VECTORS.RemoveRange(vectorsForDestroy);

             /* save cahnges */
             await _db.CTX.SaveChangesAsync();
            _log.Info($"removed from db by memberkey {memberKey}:{vectorsForDestroy.Count}");

            return new ApiResponse<bool>(ApiMethod.CLEAR, ApiResponseStatus.OK);
        }
    }
}