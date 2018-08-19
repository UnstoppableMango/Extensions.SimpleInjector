using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace UnMango.Extensions.SimpleInjector.Logging
{
    internal class DefaultLoggerLevelConfigureOptions : ConfigureOptions<LoggerFilterOptions>
    {
        public DefaultLoggerLevelConfigureOptions(LogLevel level)
            : base(options => options.MinLevel = level) { }
    }
}
