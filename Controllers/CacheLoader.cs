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
        private readonly IMemoryCache<double[]> _cache;
        private readonly IInboundDbService _db;
        private readonly ILogService _log;
        public CacheLoader(IMemoryCache<double[]> cache, IInboundDbService db, ILogService log)
        {
            _cache = cache; _db = db; _log=log;
        }

        public void Run()
        {
            _log.Info("START CACHE LOADING");

            Parallel.ForEach(
                 _db.VECTORS,
                 parallelOptions:new ParallelOptions { MaxDegreeOfParallelism = 4 },
                 itm => _cache.Set(itm.MemberKey, itm.InternalKey, itm.Vector.JsonToDouble())
            );
            _log.Info("CACHE IS LOADED");
        }
    }
}