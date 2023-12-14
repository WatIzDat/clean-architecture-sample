using Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArchitectureTests.Application
{
    public abstract class BaseApplicationTest
    {
        protected static readonly Assembly ApplicationAssembly = AssemblyReference.Assembly;
    }
}
