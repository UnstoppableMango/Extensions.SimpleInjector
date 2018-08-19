using System;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using SimpleInjector;

namespace UnMango.Extensions.SimpleInjector.Caching.Memory
{
    using Options;

    public static class ContainerExtensions
    {
        /// <summary>
        /// Adds a non distributed in memory implementation of <see cref="IMemoryCache"/> to the
        /// <see cref="Container" />.
        /// </summary>
        /// <param name="container">The <see cref="Container" /> to add container to.</param>
        /// <returns>The <see cref="Container"/> so that additional calls can be chained.</returns>
        public static Container AddMemoryCache(this Container container) {
            if (container == null) {
                throw new ArgumentNullException(nameof(container));
            }

            container.AddOptions();
            container.TryRegisterSingleton<IMemoryCache, MemoryCache>();

            return container;
        }

        /// <summary>
        /// Adds a non distributed in memory implementation of <see cref="IMemoryCache"/> to the
        /// <see cref="Container" />.
        /// </summary>
        /// <param name="container">The <see cref="Container" /> to add container to.</param>
        /// <param name="setupAction">
        /// The <see cref="Action{MemoryCacheOptions}"/> to configure the provided <see cref="MemoryCacheOptions"/>.
        /// </param>
        /// <returns>The <see cref="Container"/> so that additional calls can be chained.</returns>
        public static Container AddMemoryCache(this Container container, Action<MemoryCacheOptions> setupAction) {
            if (container == null) {
                throw new ArgumentNullException(nameof(container));
            }

            if (setupAction == null) {
                throw new ArgumentNullException(nameof(setupAction));
            }

            container.AddMemoryCache();
            container.Configure(setupAction);

            return container;
        }

        /// <summary>
        /// Adds a default implementation of <see cref="IDistributedCache"/> that stores items in memory
        /// to the <see cref="Container" />. Frameworks that require a distributed cache to work
        /// can safely add this dependency as part of their dependency list to ensure that there is at least
        /// one implementation available.
        /// </summary>
        /// <remarks>
        /// <see cref="AddDistributedMemoryCache(Container)"/> should only be used in single
        /// server scenarios as this cache stores items in memory and doesn't expand across multiple machines.
        /// For those scenarios it is recommended to use a proper distributed cache that can expand across
        /// multiple machines.
        /// </remarks>
        /// <param name="container">The <see cref="Container" /> to add container to.</param>
        /// <returns>The <see cref="Container"/> so that additional calls can be chained.</returns>
        public static Container AddDistributedMemoryCache(this Container container) {
            if (container == null) {
                throw new ArgumentNullException(nameof(container));
            }

            container.AddOptions();
            container.TryRegisterSingleton<IDistributedCache, MemoryDistributedCache>();

            return container;
        }

        /// <summary>
        /// Adds a default implementation of <see cref="IDistributedCache"/> that stores items in memory
        /// to the <see cref="Container" />. Frameworks that require a distributed cache to work
        /// can safely add this dependency as part of their dependency list to ensure that there is at least
        /// one implementation available.
        /// </summary>
        /// <remarks>
        /// <see cref="AddDistributedMemoryCache(Container)"/> should only be used in single
        /// server scenarios as this cache stores items in memory and doesn't expand across multiple machines.
        /// For those scenarios it is recommended to use a proper distributed cache that can expand across
        /// multiple machines.
        /// </remarks>
        /// <param name="container">The <see cref="Container" /> to add container to.</param>
        /// <param name="setupAction">
        /// The <see cref="Action{MemoryDistributedCacheOptions}"/> to configure the provided <see cref="MemoryDistributedCacheOptions"/>.
        /// </param>
        /// <returns>The <see cref="Container"/> so that additional calls can be chained.</returns>
        public static Container AddDistributedMemoryCache(this Container container, Action<MemoryDistributedCacheOptions> setupAction) {
            if (container == null) {
                throw new ArgumentNullException(nameof(container));
            }

            if (setupAction == null) {
                throw new ArgumentNullException(nameof(setupAction));
            }

            container.AddDistributedMemoryCache();
            container.Configure(setupAction);

            return container;
        }
    }
}
