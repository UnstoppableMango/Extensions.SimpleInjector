using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using SimpleInjector;
using UnMango.Extensions.SimpleInjector.Options.Internal;
using Xunit;

namespace UnMango.Extensions.SimpleInjector.Options.Test.Internal
{
    public class InternalOptionsBuilderTest
    {
        private readonly Container _container = new Container();

        private readonly InternalOptionsBuilder<FakeOptions> _optionsBuilder;

        public InternalOptionsBuilderTest() {
            _optionsBuilder = new InternalOptionsBuilder<FakeOptions>(_container, string.Empty);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void Registers_Custom_Dependency() {
            //_container.Register<FakeDep>();
            _optionsBuilder.Configure<FakeDep>((options, dep) => { });

            _container.Verify();

            var concreteOptions = _container.GetInstance<FakeOptions>();
            var concreteDep = _container.GetInstance<FakeDep>();
            var configureOptions = _container.GetInstance<IConfigureOptions<FakeOptions>>();
            Assert.NotNull(concreteOptions);
            Assert.NotNull(concreteDep);
            Assert.NotNull(configureOptions);
        }

        [UsedImplicitly]
        private class FakeOptions { }

        [UsedImplicitly]
        private class FakeDep { }
    }
}
