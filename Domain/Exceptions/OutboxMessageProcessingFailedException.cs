using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    /// <summary>
    /// An exception that is thrown when an outbox message failed to be processed.
    /// </summary>
    public class OutboxMessageProcessingFailedException : Exception
    {
        public OutboxMessageProcessingFailedException()
        {
        }

        public OutboxMessageProcessingFailedException(string message)
            : base(message)
        {
        }

        public OutboxMessageProcessingFailedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
