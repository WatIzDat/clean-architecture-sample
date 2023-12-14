using Domain;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArchitectureTests.Domain
{
    public abstract class BaseDomainTest
    {
        protected static readonly Assembly DomainAssembly = AssemblyReference.Assembly;
    }
}
