using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SimpleInjector;
using UnMango.Extensions.SimpleInjector.Options;

namespace UnMango.Extensions.SimpleInjector.Logging
{
    public static class ContainerExtensions
    {
        /// <summary>
        /// Adds logging services to the specified <see cref="Container" />.
        /// </summary>
        /// <param name="container">The <see cref="Container" /> to add services to.</param>
        /// <returns>The <see cref="Container"/> so that additional calls can be chained.</returns>
        public static Container AddLogging(this Container container)
        {
            return AddLogging(container, builder => { });
        }

        /// <summary>
        /// Adds logging services to the specified <see cref="Container" />.
        /// </summary>
        /// <param name="container">The <see cref="Container" /> to add services to.</param>
        /// <param name="configure">The <see cref="ILoggingBuilder"/> configuration delegate.</param>
        /// <returns>The <see cref="Container"/> so that additional calls can be chained.</returns>
        public static Container AddLogging(this Container container, Action<ILoggingBuilder> configure)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.UseLargestResolvableConstructor();

            container.AddOptions();

            container.TryRegisterSingleton<ILoggerFactory, LoggerFactory>();
            container.TryRegisterSingleton(typeof(ILogger<>), typeof(Logger<>));

            container.Collection.TryRegister<IConfigureOptions<LoggerFilterOptions>>(
                new DefaultLoggerLevelConfigureOptions(LogLevel.Information));

            configure(new LoggingBuilder(container));
            return container;
        }
    }
}
