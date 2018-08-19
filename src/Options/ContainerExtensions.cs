using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using SimpleInjector;

namespace UnMango.Extensions.SimpleInjector.Options
{
    using Internal;
    using Properties;

    /// <summary>
    ///     Extension methods for adding options services to the DI container.
    /// </summary>
    public static class ContainerExtensions
    {
        /// <summary>
        ///     Adds services required for using options.
        /// </summary>
        /// <param name="container">The <see cref="Container"/> to add the services to.</param>
        /// <returns>The <see cref="Container"/> so that additional calls can be changed.</returns>
        [UsedImplicitly]
        public static Container AddOptions(this Container container)
        {
            Check.NotNull(container, nameof(container));

            container.TryRegister(typeof(IOptions<>), typeof(OptionsManager<>), Lifestyle.Singleton);
            container.TryRegister(typeof(IOptionsSnapshot<>), typeof(OptionsManager<>), Lifestyle.Scoped);
            container.TryRegister(typeof(IOptionsMonitor<>), typeof(OptionsMonitor<>), Lifestyle.Singleton);
            // The original extension has this as transiant, but SI says thats a bad DI graph.
            container.TryRegister(typeof(IOptionsFactory<>), typeof(OptionsFactory<>), Lifestyle.Singleton);
            container.TryRegister(typeof(IOptionsMonitorCache<>), typeof(OptionsCache<>), Lifestyle.Singleton);

            return container;
        }

        /// <summary>
        ///     Reigsters an actino used to configure a particular type of options.
        ///     Note: These are run before all <seealso cref="PostConfigure{TOptions}(Container, Action{TOptions})"/>
        /// </summary>
        /// <typeparam name="TOptions">The options type to be configured.</typeparam>
        /// <param name="container">The <see cref="Container"/> to add the services to.</param>
        /// <param name="configureOptions">The action used to configure the options.</param>
        /// <returns>The <see cref="Container"/> so that additional calls can be chained.</returns>
        [UsedImplicitly]
        public static Container Configure<TOptions>(this Container container, Action<TOptions> configureOptions)
            where TOptions : class
            => container.Configure(Options.DefaultName, configureOptions);

        /// <summary>
        ///     Registers an action used to configure a particular type of options.
        ///     Note: These are run before all <seealso cref="PostConfigure{TOptions}(Container, Action{TOptions})"/>
        /// </summary>
        /// <typeparam name="TOptions">The options type to be configured.</typeparam>
        /// <param name="container">The <see cref="Container"/> to add the services to.</param>
        /// <param name="name">The name of the options instance.</param>
        /// <param name="configureOptions">The action used to configure the options.</param>
        /// <returns>The <see cref="Container"/> so that additonal calls can be chained.</returns>
        [UsedImplicitly]
        public static Container Configure<TOptions>(this Container container, string name, Action<TOptions> configureOptions)
            where TOptions : class
        {
            Check.NotNull(container, nameof(container));
            Check.NotNull(configureOptions, nameof(configureOptions));
            container.AddOptions();
            var registration = Lifestyle.Singleton.CreateRegistration<IConfigureOptions<TOptions>>(
                () => new ConfigureNamedOptions<TOptions>(name, configureOptions), container);
            container.Collection.Append(typeof(IConfigureOptions<TOptions>), registration);
            container.Collection.Register<IPostConfigureOptions<TOptions>>(Enumerable.Empty<Type>());
            return container;
        }

        /// <summary>
        ///     Registers an action used to configure all instances of a particular type of options.
        /// </summary>
        /// <typeparam name="TOptions">The options type to be configured.</typeparam>
        /// <param name="container">The <see cref="Container"/> to add the services to.</param>
        /// <param name="configureOptions">The action used to configure the options.</param>
        /// <returns>The <see cref="Container"/> so that additional calls can be chained.</returns>
        [UsedImplicitly]
        public static Container ConfigureAll<TOptions>(this Container container, Action<TOptions> configureOptions)
            where TOptions : class
            => container.Configure(name: null, configureOptions: configureOptions);

        /// <summary>
        ///     Registers an action used to initialize a particular type of options.
        ///     Note: These are run after all <seealso cref="Configure{TOptions}(Container, Action{TOptions})"/>.
        /// </summary>
        /// <typeparam name="TOptions">The options type to be configured.</typeparam>
        /// <param name="container">The <see cref="Container"/> to add the services to.</param>
        /// <param name="configureOptions">The action used to configure the options.</param>
        /// <returns>The <see cref="Container"/> so that additional calls can be chained.</returns>
        [UsedImplicitly]
        public static Container PostConfigure<TOptions>(this Container container, Action<TOptions> configureOptions)
            where TOptions : class
            => container.PostConfigure(Options.DefaultName, configureOptions);

        /// <summary>
        ///     Registers an action used to configure a particular type of options.
        ///     Note: These are run after all <seealso cref="Configure{TOptions}(Container, Action{TOptions})"/>.
        /// </summary>
        /// <typeparam name="TOptions">The options type to be configure.</typeparam>
        /// <param name="container">The <see cref="Container"/> to add the services to.</param>
        /// <param name="name">The name of the options instance.</param>
        /// <param name="configureOptions">The action used to configure the options.</param>
        /// <returns>The <see cref="Container"/> so that additional calls can be chained.</returns>
        [UsedImplicitly]
        public static Container PostConfigure<TOptions>(this Container container, string name, Action<TOptions> configureOptions)
            where TOptions : class
        {
            Check.NotNull(container, nameof(container));
            Check.NotNull(configureOptions, nameof(configureOptions));
            container.AddOptions();
            container.Collection.Register<IConfigureOptions<TOptions>>(Enumerable.Empty<Type>());
            var registration = Lifestyle.Singleton.CreateRegistration<IPostConfigureOptions<TOptions>>(
                () => new PostConfigureOptions<TOptions>(name, configureOptions), container);
            container.Collection.Append(typeof(IPostConfigureOptions<TOptions>), registration);
            return container;
        }

        /// <summary>
        ///     Registers an action used to post configure all instances of a particular type of options.
        ///     Note: These are run after all <seealso cref="Configure{TOptions}(Container, Action{TOptions})"/>.
        /// </summary>
        /// <typeparam name="TOptions">The options type to be configured.</typeparam>
        /// <param name="container">The <see cref="Container"/> to add the services to.</param>
        /// <param name="configureOptions">The action used to configure the options.</param>
        /// <returns>The <see cref="Container"/> so that additional calls can be chained.</returns>
        [UsedImplicitly]
        public static Container PostConfigureAll<TOptions>(this Container container, Action<TOptions> configureOptions)
            where TOptions : class
            => container.PostConfigure(name: null, configureOptions: configureOptions);

        /// <summary>
        ///     Registers a type that will have all of its I[Post]ConfigureOptions registered.
        /// </summary>
        /// <typeparam name="TConfigureOptions">The type that will configure options.</typeparam>
        /// <param name="container">The <see cref="Container"/> to add the services to.</param>
        /// <returns>The <see cref="Container"/> so that additional calls can be chained.</returns>
        [UsedImplicitly]
        public static Container ConfigureOptions<TConfigureOptions>(this Container container)
            where TConfigureOptions : class
            => container.ConfigureOptions(typeof(TConfigureOptions));

        /// <summary>
        ///     Registers a type that will have all of its I[Post]ConfigureOptions registered.
        /// </summary>
        /// <param name="container">The <see cref="Container"/> to add the services to.</param>
        /// <param name="configureType">The type that will configure options.</param>
        /// <returns>The <see cref="Container"/> so that additional calls can be chained.</returns>
        [UsedImplicitly]
        public static Container ConfigureOptions(this Container container, Type configureType)
        {
            container.AddOptions();
            var serviceTypes = FindIConfigureOptions(configureType);
            foreach (var serviceType in serviceTypes)
            {
                container.Register(serviceType, configureType);
            }
            return container;
        }

        /// <summary>
        ///     Registers an object that will have all of its I[Post]ConfigureOptions registered.
        /// </summary>
        /// <param name="container">The <see cref="Container"/> to add the services to.</param>
        /// <param name="configureInstance">The instance that will configure options.</param>
        /// <returns>The <see cref="Container"/> so that additional calls can be chained.</returns>
        [UsedImplicitly]
        public static Container ConfigureOptions(this Container container, object configureInstance)
        {
            container.AddOptions();
            var serviceTypes = FindIConfigureOptions(configureInstance.GetType());
            foreach (var serviceType in serviceTypes)
            {
                container.RegisterInstance(serviceType, configureInstance);
            }
            return container;
        }

        /// <summary>
        ///     Gets an options builder that forwards Configure calls for the same <typeparamref name="TOptions"/> to the underlying service collection.
        /// </summary>
        /// <typeparam name="TOptions">The options type to be configured.</typeparam>
        /// <param name="container">The <see cref="Container"/> to add the services to.</param>
        /// <returns>The <see cref="Microsoft.Extensions.Options.OptionsBuilder{TOptions}"/> so that configure calls can be chained in it.</returns>
        [UsedImplicitly]
        public static OptionsBuilder<TOptions> AddOptions<TOptions>(this Container container)
            where TOptions : class
            => container.AddOptions<TOptions>(Options.DefaultName);

        /// <summary>
        ///     Gets an options builder that forwards Configure calls for the same named <typeparamref name="TOptions"/> to the underlying service collection.
        /// </summary>
        /// <typeparam name="TOptions">The options type to be configured.</typeparam>
        /// <param name="container">The <see cref="Container"/> to add the services to.</param>
        /// <param name="name">The name of the options instance.</param>
        /// <returns>The <see cref="Microsoft.Extensions.Options.OptionsBuilder{TOptions}"/> so that configure calls can be chained in it.</returns>
        [UsedImplicitly]
        public static OptionsBuilder<TOptions> AddOptions<TOptions>(this Container container, string name)
            where TOptions : class
        {
            Check.NotNull(container, nameof(container));
            container.AddOptions();
            return new InternalOptionsBuilder<TOptions>(container, name);
        }

        private static bool IsAction(Type type)
            => (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Action<>));

        private static IEnumerable<Type> FindIConfigureOptions(Type type)
        {
            var serviceTypes = type.GetTypeInfo().ImplementedInterfaces
                .Where(t => t.GetTypeInfo().IsGenericType &&
                (t.GetGenericTypeDefinition() == typeof(IConfigureOptions<>)
                || t.GetGenericTypeDefinition() == typeof(IPostConfigureOptions<>)));

            var findIConfigureOptions = serviceTypes as Type[] ?? serviceTypes.ToArray();

            if (!findIConfigureOptions.Any())
            {
                throw new InvalidOperationException(
                    IsAction(type)
                    ? Resources.Error_NoIConfigureOptionsAndAction
                    : Resources.Error_NoIConfigureOptions);
            }

            return findIConfigureOptions;
        }
    }
}
