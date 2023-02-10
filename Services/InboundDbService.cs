using ItZnak.Infrastruction.Services;
using MatchEngineApi.DTO;
using Microsoft.EntityFrameworkCore;

namespace MatchEngineApi.Services
{
    public class InboundDbService : DbContext, IInboundDbService
    {
        private readonly ILogService _log;
        private readonly string _dbPatch;
        public InboundDbService(ILogService log, IConfigService conf)
        {
            _log = log;
            _dbPatch = conf.GetString("dbPath");
        }

        public DbSet<VectorDto> VECTORS { get; set; }

        public DbContext CTX {get{return this;}}
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={_dbPatch}");
            _log.Info($"The DbContext has been configured. File path:{_dbPatch}");
        }
    }
}