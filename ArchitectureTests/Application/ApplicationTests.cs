using Application.Abstractions.Messaging;
using FluentAssertions;
using NetArchTest.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchitectureTests.Application
{
    public class ApplicationTests : BaseApplicationTest
    {
        [Fact]
        public void Commands_Should_HaveCommandPostfix()
        {
            TestResult result = Types.InAssembly(ApplicationAssembly)
                .That()
                .ImplementInterface(typeof(ICommand))
                .Should()
                .HaveNameEndingWith("Command")
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void CommandHandlers_Should_HaveCommandHandlerPostfix()
        {
            TestResult result = Types.InAssembly(ApplicationAssembly)
                .That()
                .ImplementInterface(typeof(ICommandHandler<>))
                .Should()
                .HaveNameEndingWith("CommandHandler")
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Queries_Should_HaveQueryPostfix()
        {
            TestResult result = Types.InAssembly(ApplicationAssembly)
                .That()
                .ImplementInterface(typeof(IQuery<>))
                .Should()
                .HaveNameEndingWith("Query")
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void QueryHandlers_Should_HaveQueryHandlerPostfix()
        {
            TestResult result = Types.InAssembly(ApplicationAssembly)
                .That()
                .ImplementInterface(typeof(IQueryHandler<,>))
                .Should()
                .HaveNameEndingWith("QueryHandler")
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }
    }
}
