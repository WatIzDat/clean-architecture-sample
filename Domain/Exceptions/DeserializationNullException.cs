using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    /// <summary>
    /// An exception that is thrown when a deserialization (for example a JSON object) returns a null value. 
    /// </summary>
    public class DeserializationNullException : Exception
    {
        public DeserializationNullException()
        {
        }

        public DeserializationNullException(string message) 
            : base(message)
        {
        }

        public DeserializationNullException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
