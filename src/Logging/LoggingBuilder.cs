using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleInjector;

namespace UnMango.Extensions.SimpleInjector.Logging
{
    // TODO: Work around Microsoft's 'anti-pattern'
    internal class LoggingBuilder : ILoggingBuilder
    {
        public LoggingBuilder(Container container) {
            Container = container;
        }

        public Container Container { get; }

        public IServiceCollection Services { get; set; }
    }
}
