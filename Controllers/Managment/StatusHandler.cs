using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastraction.Services.MemoryCache;
using ItZnak.Infrastruction.Services;
using ItZnak.Infrastruction.Web.Controllers;
using MatchEngineApi.Controllers.Base;
using MatchEngineApi.Services;

namespace MatchEngineApi.Controllers.Managment
{
    public class StatusRS
    {
        public string NodeIP {get;set;}
        public string  MemberKey { get; set; }
        public int  Count {get;set;}
     }

    public class StatusHandler : WebApiControllerHandler<string, StatusRS[]>
    {
        private readonly IInboundDbService _db;
        public StatusHandler(IMatchEngineController context) : base(context)
        {
            _db = context.DbContext;
        }

        public override StatusRS[] Handle(string currentIp)
        {
           
            return _db.VECTORS.GroupBy(_=>_.MemberKey)
                              .Select(p=>new StatusRS(){NodeIP=currentIp, Count=p.Count(), MemberKey=p.Key})
                              .ToArray();
        }
    }
}