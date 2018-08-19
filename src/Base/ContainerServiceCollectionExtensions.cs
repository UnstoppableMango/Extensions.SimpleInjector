using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

namespace UnMango.Extensions.SimpleInjector
{
    /// <summary>
    /// Extension methods for <see cref="Container"/> working with <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ContainerServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the services from <paramref name="services"/> to <paramref name="container"/>/
        /// </summary>
        /// <param name="container">The <see cref="Container"/> to add services to.</param>
        /// <param name="services">The <see cref="IServiceCollection"/> to get services from.</param>
        /// <returns>The <see cref="Container"/> so calls can be chained.</returns>
        [UsedImplicitly]
        public static Container AddServices([NotNull] this Container container, [NotNull] IServiceCollection services) {
            foreach (var service in services) {
                Register(container, service);
            }

            return container;
        }

        /// <summary>
        /// Registers a <see cref="ServiceDescriptor"/> with the specified <see cref="Container"/>.
        /// </summary>
        /// <param name="container">The <see cref="Container"/> to add services to.</param>
        /// <param name="service">The <see cref="ServiceDescriptor"/> to register with the container.</param>
        /// <returns>The <see cref="Container"/> so calls can be chained.</returns>
        [UsedImplicitly]
        public static Container Register([NotNull] this Container container, [NotNull] ServiceDescriptor service) {
            if (service.ImplementationInstance != null) {
                RegisterInstance(container, service);
            } else if (service.ImplementationFactory != null) {
                RegisterFactory(container, service);
            } else {
                RegisterDefault(container, service);
            }

            return container;
        }

        /// <summary>
        /// Gets the lifestyle from the <see cref="ServiceDescriptor"/>.
        /// </summary>
        /// <param name="service">The descriptor to get the lifestyle of.</param>
        /// <returns>The <see cref="Lifestyle"/> of the <paramref name="service"/>.</returns>
        [UsedImplicitly]
        public static Lifestyle GetLifestyle([NotNull] this ServiceDescriptor service)
            => GetLifestyle(service.Lifetime);

        /// <summary>
        /// Gets the lifestyle from the <see cref="ServiceLifetime"/>.
        /// </summary>
        /// <param name="lifetime">The <see cref="ServiceLifetime"/> to map to a <see cref="Lifestyle"/>.</param>
        /// <returns>The <see cref="Lifestyle"/> of the <paramref name="lifetime"/>.</returns>
        [UsedImplicitly]
        public static Lifestyle GetLifestyle(this ServiceLifetime lifetime) {
            switch (lifetime) {
                case ServiceLifetime.Singleton:
                    return Lifestyle.Singleton;
                case ServiceLifetime.Scoped:
                    return Lifestyle.Scoped;
                // ReSharper disable once RedundantCaseLabel
                case ServiceLifetime.Transient:
                default:
                    return Lifestyle.Transient;
            }
        }

        internal static void RegisterInstance([NotNull] this Container container, [NotNull] ServiceDescriptor service)
            => container.RegisterInstance(
                service.ServiceType,
                service.ImplementationInstance);

        internal static void RegisterFactory([NotNull] this Container container, [NotNull] ServiceDescriptor service)
            => container.Register(
                service.ServiceType,
                () => service.ImplementationFactory(container),
                GetLifestyle(service));

        internal static void RegisterDefault([NotNull] this Container container, [NotNull] ServiceDescriptor service)
            => container.Register(
                service.ServiceType,
                service.ImplementationType,
                GetLifestyle(service));
    }
}
