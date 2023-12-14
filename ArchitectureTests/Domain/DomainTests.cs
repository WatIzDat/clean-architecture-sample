using SharedKernel;
using FluentAssertions;
using NetArchTest.Rules;
using System.Reflection;

namespace ArchitectureTests.Domain
{
    public class DomainTests : BaseDomainTest
    {
        [Fact]
        public void DomainEvents_Should_BeSealed()
        {
            TestResult result = Types.InAssembly(DomainAssembly)
                .That()
                .ImplementInterface(typeof(IDomainEvent))
                .Should()
                .BeSealed()
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void DomainEvents_Should_HaveDomainEventPostfix()
        {
            TestResult result = Types.InAssembly(DomainAssembly)
                .That()
                .ImplementInterface(typeof(IDomainEvent))
                .Should()
                .HaveNameEndingWith("DomainEvent")
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Entities_Should_HavePrivateParameterlessConstructor()
        {
            IEnumerable<Type> entityTypes = Types.InAssembly(DomainAssembly)
                .That()
                .Inherit(typeof(Entity))
                .GetTypes();

            List<Type> failingTypes = [];

            foreach (Type type in entityTypes)
            {
                ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);

                if (!constructors.Any(c => c.IsPrivate && c.GetParameters().Length == 0))
                {
                    failingTypes.Add(type);
                }
            }

            failingTypes.Should().BeEmpty();
        }
    }
}