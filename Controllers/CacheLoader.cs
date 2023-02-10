using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItZnak.Infrastruction.Services;
using MatchEngineApi.Controllers.Tools;
using MatchEngineApi.Services;

namespace MatchEngineApi.Controllers
{
    public class CacheLoader
    {
        private readonly IDistributeCache _cache;
        private readonly IInboundDbService _db;
        private readonly ILogService _log;
        public CacheLoader(IDistributeCache cache, IInboundDbService db, ILogService log)
        {
            _cache = cache; _db = db; _log=log;
        }

        public void Run()
        {
            _log.Info("START CACHE LOADING");
            // foreach(var itm in _db.VECTORS){
            //     _cache.Set(ToolsExtentions.BuildCacheKey(itm.MemberKey, itm.InternalKey), itm.Vector);
            // }

            Parallel.ForEach(
                 _db.VECTORS,
                 parallelOptions:new ParallelOptions { MaxDegreeOfParallelism = 4 },
                 itm => _cache.Set(ToolsExtentions.BuildCacheKey(itm.MemberKey, itm.InternalKey), itm.Vector)
            );
            _log.Info("CACHE IS LOADED");
        }
    }
}