using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastraction.Services.MemoryCache;
using ItZnak.Infrastruction.Services;
using MatchEngineApi.Controllers.Tools;
using MatchEngineApi.Services;

namespace MatchEngineApi.Controllers
{
    public class CacheLoader
    {
        private readonly IMemoryCache<byte[]> _cache;
        private readonly IInboundDbService _db;
        private readonly ILogService _log;
        public CacheLoader(IMemoryCache<byte[]> cache, IInboundDbService db, ILogService log)
        {
            _cache = cache; _db = db; _log=log;
        }

        public void Run()
        {
            _log.Info("START CACHE LOADING");

            Parallel.ForEach(
                 _db.VECTORS,
                 parallelOptions:new ParallelOptions { MaxDegreeOfParallelism = 4 },
                 itm => _cache.Set(ToolsExtentions.BuildCacheKey(itm.MemberKey, itm.InternalKey), itm.Vector)
            );
            _log.Info("CACHE IS LOADED");
        }
    }
}