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
        public CacheLoader(IDistributeCache cache, IInboundDbService db)
        {
            _cache = cache; _db = db;
        }

        public void Run()
        {
            foreach (var record in _db.VECTORS)
            {
                _cache.Set(ToolsExtentions.BuildCacheKey(record.MemberKey, record.InternalKey), record.Vector);
            }
            Console.WriteLine("CACHE IS LOADED");
        }
    }
}