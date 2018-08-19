using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SimpleInjector;

namespace UnMango.Extensions.SimpleInjector.Options.Internal
{
    /// <summary>
    ///     Used to configure the <typeparamref name="TOptions"/> instances.
    /// </summary>
    /// <typeparam name="TOptions">The options type to be configured.</typeparam>
    internal class InternalOptionsBuilder<TOptions> : OptionsBuilder<TOptions>
        where TOptions : class
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="container">The <see cref="Container"/> for the options being configured.</param>
        /// <param name="name">The default name of the <typeparamref name="TOptions"/> instance, if null Options.DefaultName is used.</param>
        public InternalOptionsBuilder(Container container, string name)
            : base(new ServiceCollectionProxy(), name) {
            Container = Check.NotNull(container, nameof(container));
        }

        /// <summary>
        ///     The <see cref="Container"/> for the options being configured.
        /// </summary>
        [UsedImplicitly]
        public Container Container { get; }

        /// <summary>
        ///     Registers an action used to configure a particular type of options.
        ///     Note: These are run before all <seealso cref="PostConfigure(Action{TOptions})"/>.
        /// </summary>
        /// <param name="configureOptions">The action used to configure the options.</param>
        public override OptionsBuilder<TOptions> Configure(Action<TOptions> configureOptions) {
            Check.NotNull(configureOptions, nameof(configureOptions));
            Container.RegisterInstance<IConfigureOptions<TOptions>>(
                new ConfigureNamedOptions<TOptions>(Name, configureOptions));
            return this;
        }

        public override OptionsBuilder<TOptions> Configure<TDep>(Action<TOptions, TDep> configureOptions) {
            Check.NotNull(configureOptions, nameof(configureOptions));
            Container.Register<IConfigureOptions<TOptions>>(() =>
                new ConfigureNamedOptions<TOptions, TDep>(Name, Container.GetInstance<TDep>(), configureOptions));
            return this;
        }

        public override OptionsBuilder<TOptions> Configure<TDep1, TDep2>(
            Action<TOptions, TDep1, TDep2> configureOptions) {
            Check.NotNull(configureOptions, nameof(configureOptions));
            Container.Register<IConfigureOptions<TOptions>>(() =>
                new ConfigureNamedOptions<TOptions, TDep1, TDep2>(Name, Container.GetInstance<TDep1>(),
                    Container.GetInstance<TDep2>(), configureOptions));
            return this;
        }

        public override OptionsBuilder<TOptions> Configure<TDep1, TDep2, TDep3>(
            Action<TOptions, TDep1, TDep2, TDep3> configureOptions) {
            Check.NotNull(configureOptions, nameof(configureOptions));
            Container.Register<IConfigureOptions<TOptions>>(() =>
                new ConfigureNamedOptions<TOptions, TDep1, TDep2, TDep3>(
                    Name,
                    Container.GetInstance<TDep1>(),
                    Container.GetInstance<TDep2>(),
                    Container.GetInstance<TDep3>(),
                    configureOptions));
            return this;
        }

        public override OptionsBuilder<TOptions> Configure<TDep1, TDep2, TDep3, TDep4>(
            Action<TOptions, TDep1, TDep2, TDep3, TDep4> configureOptions) {
            Check.NotNull(configureOptions, nameof(configureOptions));
            Container.Register<IConfigureOptions<TOptions>>(() =>
                new ConfigureNamedOptions<TOptions, TDep1, TDep2, TDep3, TDep4>(
                    Name,
                    Container.GetInstance<TDep1>(),
                    Container.GetInstance<TDep2>(),
                    Container.GetInstance<TDep3>(),
                    Container.GetInstance<TDep4>(),
                    configureOptions));
            return this;
        }

        public override OptionsBuilder<TOptions> Configure<TDep1, TDep2, TDep3, TDep4, TDep5>(
            Action<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5> configureOptions) {
            Check.NotNull(configureOptions, nameof(configureOptions));
            Container.Register<IConfigureOptions<TOptions>>(() =>
                new ConfigureNamedOptions<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5>(
                    Name,
                    Container.GetInstance<TDep1>(),
                    Container.GetInstance<TDep2>(),
                    Container.GetInstance<TDep3>(),
                    Container.GetInstance<TDep4>(),
                    Container.GetInstance<TDep5>(),
                    configureOptions));
            return this;
        }

        public override OptionsBuilder<TOptions> PostConfigure(Action<TOptions> configureOptions) {
            Check.NotNull(configureOptions, nameof(configureOptions));
            Container.RegisterInstance<IPostConfigureOptions<TOptions>>(
                new PostConfigureOptions<TOptions>(Name, configureOptions));
            return this;
        }

        public override OptionsBuilder<TOptions> PostConfigure<TDep>(Action<TOptions, TDep> configureOptions) {
            Check.NotNull(configureOptions, nameof(configureOptions));
            Container.Register<IPostConfigureOptions<TOptions>>(() =>
                new PostConfigureOptions<TOptions, TDep>(Name, Container.GetInstance<TDep>(), configureOptions));
            return this;
        }

        public override OptionsBuilder<TOptions> PostConfigure<TDep1, TDep2>(
            Action<TOptions, TDep1, TDep2> configureOptions) {
            Check.NotNull(configureOptions, nameof(configureOptions));
            Container.Register<IPostConfigureOptions<TOptions>>(() =>
                new PostConfigureOptions<TOptions, TDep1, TDep2>(Name, Container.GetInstance<TDep1>(),
                    Container.GetInstance<TDep2>(), configureOptions));
            return this;
        }

        public override OptionsBuilder<TOptions> PostConfigure<TDep1, TDep2, TDep3>(
            Action<TOptions, TDep1, TDep2, TDep3> configureOptions) {
            Check.NotNull(configureOptions, nameof(configureOptions));
            Container.Register<IPostConfigureOptions<TOptions>>(() =>
                new PostConfigureOptions<TOptions, TDep1, TDep2, TDep3>(
                    Name,
                    Container.GetInstance<TDep1>(),
                    Container.GetInstance<TDep2>(),
                    Container.GetInstance<TDep3>(),
                    configureOptions));
            return this;
        }

        public override OptionsBuilder<TOptions> PostConfigure<TDep1, TDep2, TDep3, TDep4>(
            Action<TOptions, TDep1, TDep2, TDep3, TDep4> configureOptions) {
            Check.NotNull(configureOptions, nameof(configureOptions));
            Container.Register<IPostConfigureOptions<TOptions>>(() =>
                new PostConfigureOptions<TOptions, TDep1, TDep2, TDep3, TDep4>(
                    Name,
                    Container.GetInstance<TDep1>(),
                    Container.GetInstance<TDep2>(),
                    Container.GetInstance<TDep3>(),
                    Container.GetInstance<TDep4>(),
                    configureOptions));
            return this;
        }

        public override OptionsBuilder<TOptions> PostConfigure<TDep1, TDep2, TDep3, TDep4, TDep5>(
            Action<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5> configureOptions) {
            Check.NotNull(configureOptions, nameof(configureOptions));
            Container.Register<IPostConfigureOptions<TOptions>>(() =>
                new PostConfigureOptions<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5>(
                    Name,
                    Container.GetInstance<TDep1>(),
                    Container.GetInstance<TDep2>(),
                    Container.GetInstance<TDep3>(),
                    Container.GetInstance<TDep4>(),
                    Container.GetInstance<TDep5>(),
                    configureOptions));
            return this;
        }

        private class ServiceCollectionProxy : IServiceCollection
        {
            public ServiceDescriptor this[int index] {
                get => throw new InvalidOperationException();
                set => throw new InvalidOperationException();
            }

            public int Count => throw new InvalidOperationException();
            public bool IsReadOnly => throw new InvalidOperationException();

            public void Add(ServiceDescriptor item) => throw new InvalidOperationException();

            public void Clear() => throw new InvalidOperationException();

            public bool Contains(ServiceDescriptor item) => throw new InvalidOperationException();

            public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
                => throw new InvalidOperationException();

            public IEnumerator<ServiceDescriptor> GetEnumerator()
                => throw new InvalidOperationException();

            public int IndexOf(ServiceDescriptor item) => throw new InvalidOperationException();

            public void Insert(int index, ServiceDescriptor item) => throw new InvalidOperationException();

            public bool Remove(ServiceDescriptor item) => throw new InvalidOperationException();

            public void RemoveAt(int index) => throw new InvalidOperationException();

            IEnumerator IEnumerable.GetEnumerator() => throw new InvalidOperationException();
        }
    }
}
