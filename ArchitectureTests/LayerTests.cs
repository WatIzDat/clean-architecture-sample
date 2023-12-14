using ArchitectureTests.Domain;
using FluentAssertions;
using NetArchTest.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchitectureTests
{
    public class LayerTests : BaseDomainTest
    {
        [Fact]
        public void Domain_Should_NotHaveDependencyOnApplication()
        {
            TestResult result = Types.InAssembly(DomainAssembly)
                .Should()
                .NotHaveDependencyOn("Application")
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }
    }
}
