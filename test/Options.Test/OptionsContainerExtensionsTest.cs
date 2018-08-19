using System.Collections.Generic;
using Microsoft.Extensions.Options;
using SimpleInjector;
using Xunit;

namespace UnMango.Extensions.SimpleInjector.Options.Test
{
    public class OptionsContainerExtensionsTest
    {
        private readonly Container _container = new Container();

        [Fact]
        [Trait("Category", "Unit")]
        public void Adds_Options() {
            // ReSharper disable once InvokeAsExtensionMethod
            ContainerExtensions.AddOptions(_container);

            _container.Verify(VerificationOption.VerifyAndDiagnose);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void Configures_Options() {
            _container.Configure<FakeOptions>(x => { });

            _container.Verify(VerificationOption.VerifyAndDiagnose);

            var options = _container.GetInstance<IOptions<FakeOptions>>();
            Assert.NotNull(options);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void Append_To_Collection() {
            _container.Collection.Append(typeof(FakeOptions), typeof(FakeOptions));

            _container.Verify(VerificationOption.VerifyAndDiagnose);

            var options = _container.GetInstance<IEnumerable<FakeOptions>>();
            Assert.NotEmpty(options);
        }

        private class FakeOptions { }
    }
}
