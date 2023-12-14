using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateOpenConnection();
    }
}
