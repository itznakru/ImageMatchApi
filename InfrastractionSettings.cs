using ItZnak.Infrastruction.Services;

namespace MatchEngineApi
{
    /* Prupouse: check enviroment state of app*/
    public class InfrastractionSettings
    {
        private readonly ILogService _log;
        public IConfigService _config ;
        public InfrastractionSettings( IConfigService config, ILogService log )
        {
            _config = config;
            _log = log;
        }

        public void ApplaySettings(){
            ThreadPool.SetMinThreads(50, 200);
            _log.Info("CHANGED THRED POOL SETTINGS: minTheard=50, minPortThreads=200");
        }
    }
}