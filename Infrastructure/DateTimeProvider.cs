using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow { get; } = DateTime.UtcNow;
    }
}
