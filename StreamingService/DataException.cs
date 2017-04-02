using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamingServiceCompare.StreamingService
{
    class InvalidResultException : ApplicationException
    {
        public InvalidResultException() : base() { }

        public InvalidResultException(string message) : base(message) { }

        public InvalidResultException(string message, Exception innerException) : base(message, innerException) { }
    }
}
