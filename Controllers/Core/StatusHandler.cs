using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItZnak.Infrastruction.Services;
using ItZnak.Infrastruction.Web.Controllers;
using MatchEngineApi.Controllers.Base;
using MatchEngineApi.Services;

namespace MatchEngineApi.Controllers.Core
{
    public class StatusRS
    {
        public int DbCount { get; set; }
        public bool IsVectorBuilderOK { get; set; }
    }

    public class StatusHandler : WebApiControllerHandler<string, StatusRS>
    {
        private readonly IInboundDbService _db;
        public StatusHandler(IMatchEngineController context) : base(context)
        {
            _db = context.DbContext;
        }

        public override StatusRS Handle(string memberKey)
        {
            int dbCount = _db.VECTORS.Count(itm => itm.MemberKey == memberKey);
            return new StatusRS()
            {
                DbCount = dbCount,
                IsVectorBuilderOK = true
            };
        }
    }
}