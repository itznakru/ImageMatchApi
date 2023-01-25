using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchEngineApi.Controllers.Exceptions
{
    public class MatchEngineApiException : Exception
    {
        public MatchEngineApiException() : base()
        {
        }

        public MatchEngineApiException(string method, string message ) : base(message)
        {
                Method=method;
                ErrorList = new List<string> { message };

        }

        public MatchEngineApiException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public List<string> ErrorList { get; }
        public string Method { get; }


    }
}