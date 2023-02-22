using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchEngineApi.Controllers.Exceptions
{
    public class RemoteServerCallException : Exception
    {
        public RemoteServerCallException() : base()
        {
        }

        public RemoteServerCallException(string message) : base(message)
        {
        }

        public RemoteServerCallException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}