using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MatchEngineApi.Controllers.Base;

namespace MatchEngineApi.Controllers.Exceptions
{
    public class MatchEngineApiException : Exception
    {
        public MatchEngineApiException() : base()
        {
        }

        public MatchEngineApiException(ApiMethod method, string message ) : base(message)
        {
                Method=method.ToString();
                ErrorList = new List<string> { message };
        }

        public MatchEngineApiException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public MatchEngineApiException(string message) : base(message)
        {
        }

        public List<string> ErrorList { get; }
        public string Method { get; }
    }
}