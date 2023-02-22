using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItZnak.Infrastruction.Services;

/* КАЖДЫЕ N СЕКУНД ДЕРГАЕТ managment/status КОРНЕВОЙ НОДЫ  */
namespace MatchEngineApi.Middleware
{
    public class HertBitStatus{
        public string Ip {get;set;}
        public bool IsRoot {get;set;}
        public bool IsLive {get;set;}
        public int Count {get;set;}
        public int MaxCount {get;set;} =5000000;
    }


    public class HertBitMdl
    {
        public HertBitMdl(IConfigService conf)
        {
            var hostName = System.Net.Dns.GetHostName();
            System.Net.Dns.GetHostAddressesAsync(hostName);
        }
    }
}