using System;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SimpleInjector;

namespace UnMango.Extensions.SimpleInjector.Options
{
    /// <summary>
    ///     Extension methods for adding confuration related options services to the DI container.
    /// </summary>
    public static class ConfigurationContainerExtensions
    {
        /// <summary>
        ///     Registers a configuration instance which <typeparamref name="TOptions"/> will bind against.
        /// </summary>
        /// <typeparam name="TOptions">The type of options being configured.</typeparam>
        /// <param name="container">The <see cref="Container"/> to add the services to.</param>
        /// <param name="config">The configuration being bound.</param>
        /// <returns>The <see cref="Container"/> so that additional calls can be chained.</returns>
        public static Container Configure<TOptions>(this Container container, IConfiguration config)
            where TOptions : class
            => container.Configure<TOptions>(Options.DefaultName, config);

        /// <summary>
        ///     Registers a configuration instance which <typeparamref name="TOptions"/> will bind against.
        /// </summary>
        /// <typeparam name="TOptions">The type of options being configured.</typeparam>
        /// <param name="container">The <see cref="Container"/> to add the services to.</param>
        /// <param name="name">The name fo the options instance.</param>
        /// <param name="config">The configuration being bound.</param>
        /// <returns>The <see cref="Container"/> so that additional calls can be chained.</returns>
        [UsedImplicitly]
        public static Container Configure<TOptions>(this Container container, string name, IConfiguration config)
            where TOptions : class
            => container.Configure<TOptions>(name, config, _ => { });

        /// <summary>
        ///     Registers a configuration instance which <typeparamref name="TOptions"/> will bind against.
        /// </summary>
        /// <typeparam name="TOptions">The type of the options being configured.</typeparam>
        /// <param name="container">The <see cref="Container"/> to add ther services to.</param>
        /// <param name="config">The configuration being bound.</param>
        /// <param name="configureBinder">Used to configure the <see cref="BinderOptions"/>.</param>
        /// <returns>The <see cref="Container"/> so that additonal calls can be chained.</returns>
        [UsedImplicitly]
        public static Container Configure<TOptions>(this Container container, IConfiguration config, Action<BinderOptions> configureBinder)
            where TOptions : class
            => container.Configure<TOptions>(Options.DefaultName, config, configureBinder);

        /// <summary>
        ///     Registers a configuration instance which TOptions will bind against.
        /// </summary>
        /// <typeparam name="TOptions">The type of options being configured.</typeparam>
        /// <param name="container">The <see cref="Container"/> to add the services to.</param>
        /// <param name="name">The name of the options instance.</param>
        /// <param name="config">The configuration being bound.</param>
        /// <param name="configureBinder">Used to cinfugre the <see cref="BinderOptions"/>.</param>
        /// <returns>The <see cref="Container"/> so that additonal calls can be chained.</returns>
        [UsedImplicitly]
        public static Container Configure<TOptions>(this Container container, string name, IConfiguration config, Action<BinderOptions> configureBinder)
            where TOptions : class {
            Check.NotNull(container, nameof(container));
            Check.NotNull(config, nameof(config));

            container.AddOptions();
            container.Collection.Register<IConfigureOptions<TOptions>>(Enumerable.Empty<Type>());
            container.Collection.Register<IPostConfigureOptions<TOptions>>(Enumerable.Empty<Type>());
            container.RegisterInstance<IOptionsChangeTokenSource<TOptions>>(new ConfigurationChangeTokenSource<TOptions>(name, config));
            container.RegisterInstance<IConfigureOptions<TOptions>>(new NamedConfigureFromConfigurationOptions<TOptions>(name, config, configureBinder));
            return container;
        }
    }
}
