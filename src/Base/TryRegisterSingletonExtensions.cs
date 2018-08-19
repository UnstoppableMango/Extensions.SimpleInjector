using System;
using System.Collections.Generic;
using System.Reflection;
using SimpleInjector;

namespace UnMango.Extensions.SimpleInjector
{
    public static class TryRegisterSingletonExtensions
    {
        /// <summary>
        ///     Registers the specified delegate <paramref name="instanceCreator"/> that will produce instances
        ///     of type <see cref="Lifestyle"/> and will be returned when an instance of type <see cref="Lifestyle"/>
        ///     is requested. The delegate is expected to produce new instances on each call.
        ///     The instances are cached according to the supplied <see cref="Lifestyle"/>.
        /// </summary>
        /// <param name="container">The <see cref="Container"/> to register the service to.</param>
        /// <param name="serviceType">The interface or base type that can be used to retrieve instances.</param>
        /// <param name="instanceCreator">The delegate that allows building or creating new instances.</param>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> that specifies how the returned instance will be cached.</param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one of the supplied arguments is a null reference (Nothing in VB).
        /// </exception>
        /// <returns>Whether the service was registered or not.</returns>
        public static bool TryRegisterSingleton(this Container container, Type serviceType, Func<object> instanceCreator)
            => container.TryRegister(serviceType, instanceCreator, Lifestyle.Singleton);

        /// <summary>
        ///     Registers that a new instance of <paramref name="implementationType"/> will be returned every time
        ///     a <paramref name="serviceType"/> is requested. If <paramref name="serviceType"/> and <paramref name="implementationType"/> represent the
        ///     same type, the type is registered by itself. Open and closed generic types are supported.
        /// </summary>
        /// <param name="container">The <see cref="Container"/> to register the service to.</param>
        /// <param name="serviceType">The base type or interface to register. This can be an open-generic type.</param>
        /// <param name="implementationType">
        ///     The actual type that will be returned when requested. This can be an open-generic
        ///     type.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="serviceType"/> or <paramref name="implementationType"/> are null references (Nothing in
        ///     VB).
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="implementationType"/> is no sub type from <paramref name="serviceType"/> (or the same type).
        /// </exception>
        /// <remarks>
        ///     This method uses the <see cref="Container"/>'s <see cref="ContainerOptions.LifestyleSelectionBehavior"/>
        ///     to select the exact <see cref="Lifestyle"/> for the specified type. By default this will be
        ///     <see cref="Lifestyle.Transient"/>.
        /// </remarks>
        /// <returns>Whether the service was registered or not.</returns>
        public static bool TryRegisterSingleton(this Container container, Type serviceType, Type implementationType)
            => container.TryRegister(serviceType, implementationType, Lifestyle.Singleton);

        /// <summary>
        ///     Registers that an instance of <typeparamref name="TConcrete"/> will be returned when it is requested.
        ///     The instance is cached according to the supplied <see cref="Lifestyle"/>.
        /// </summary>
        /// <typeparam name="TConcrete">The concrete type that will be registered.</typeparam>
        /// <param name="container">The <see cref="Container"/> to register the service to.</param>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> that specifies how the returned instance will be cached.</param>
        /// <exception cref="ArgumentException">
        ///     Thrown when the <typeparamref name="TConcrete"/> is a type that can not be created by the container.
        /// </exception>
        /// <returns>Whether the service was registered or not.</returns>
        public static bool TryRegisterSingleton<TConcrete>(this Container container)
            where TConcrete : class
            => container.TryRegister<TConcrete>(Lifestyle.Singleton);

        /// <summary>
        ///     Registers the specified delegate <paramref name="instanceCreator"/> that will produce instances
        ///     of type <typeparamref name="TService"/> and will be returned when an instance of type <typeparamref name="TService"/> is requested.
        ///     The delegate is expected to produce new instances on each call. The instances
        ///     are cached according to the supplied <see cref="Lifestyle"/>.
        /// </summary>
        /// <typeparam name="TService">The interface or base type that can be used to retrieve instances.</typeparam>
        /// <param name="container">The <see cref="Container"/> to register the service to.</param>
        /// <param name="instanceCreator">The delegate that allows building or creating new instances.</param>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> that specifies how the returned instance will be cached.</param>
        /// <exception cref="ArgumentException">
        ///     Thrown when one of the supplied arguments is a null reference (Nothing in VB).
        /// </exception>
        /// <returns>Whether the service was registered or not.</returns>
        public static bool TryRegisterSingleton<TService>(this Container container, Func<TService> instanceCreator)
            where TService : class
            => container.TryRegister(instanceCreator, Lifestyle.Singleton);

        /// <summary>
        ///     Registers that an instance of <typeparamref name="TImplementation"/> will be returned when an instance
        ///     of type <typeparamref name="TService"/> is requested. The instance is cached according to the supplied
        ///     <see cref="Lifestyle"/>.
        /// </summary>
        /// <typeparam name="TService">The interface or base type that can be used to retrieve the instances.</typeparam>
        /// <typeparam name="TImplementation">The concrete type that will be registered.</typeparam>
        /// <param name="container">The <see cref="Container"/> to register the service to.</param>
        /// <param name="lifestyle">The lifestyle that specifies how the returned instance will be cached.</param>
        /// <exception cref="ArgumentException">
        ///     Thrown when the given <typeparamref name="TImplementation"/> type is not a type that can be created
        ///     by the container.
        /// </exception>
        /// <returns>Whether the service was registered or not.</returns>
        public static bool TryRegisterSingleton<TService, TImplementation>(this Container container)
            where TService : class
            where TImplementation : class, TService
            => container.TryRegister<TService, TImplementation>(Lifestyle.Singleton);

        /// <summary>
        ///     Registers all concrete, non-generic, public and internal types in the given assembly
        ///     that implement the given <paramref name="openGenericServiceType"/> with the supplied <see cref="Lifestyle"/>.
        ///     <see cref="TypesToRegisterOptions.IncludeDecorators"/> and L<see cref="TypesToRegisterOptions.IncludeGenericTypeDefinitions"/>
        ///     will be excluded from registration, while <see cref="TypesToRegisterOptions.IncludeComposites"/>
        ///     are included.
        /// </summary>
        /// <param name="container">The <see cref="Container"/> to register the service to.</param>
        /// <param name="openGenericServiceType">The definition of the open generic type.</param>
        /// <param name="assembly">An assembly that will be searched.</param>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> to register instances with.</param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one of the arguments contain a null reference (Nothing in VB).
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="openGenericServiceType"/> is not an open generic type.
        /// </exception>
        /// <returns>Whether the service was registered or not.</returns>
        public static bool TryRegisterSingleton(this Container container, Type openGenericServiceType, Assembly assembly)
            => container.TryRegister(openGenericServiceType, assembly, Lifestyle.Singleton);

        /// <summary>
        ///     Registers all concrete, non-generic, public and internal types in the given set
        ///     of assemblies that implement the given <paramref name="openGenericServiceType"/> with the supplied
        ///     <see cref="Lifestyle"/>. <see cref="TypesToRegisterOptions.IncludeDecorators"/> and <see cref="TypesToRegisterOptions.IncludeGenericTypeDefinitions"/>
        ///     will be excluded from registration, while <see cref="TypesToRegisterOptions.IncludeComposites"/>
        ///     are included.
        /// </summary>
        /// <param name="container">The <see cref="Container"/> to register the service to.</param>
        /// <param name="openGenericServiceType">The definition of the open generic type.</param>
        /// <param name="assemblies">A list of assemblies that will be searched.</param>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> to register instances with.</param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one of the arguments contain a null reference (Nothing in VB).
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="openGenericServiceType"/> is not an open generic type.
        /// </exception>
        /// <returns>Whether the service was registered or not.</returns>
        public static bool TryRegisterSingleton(this Container container, Type openGenericServiceType, IEnumerable<Assembly> assemblies)
            => container.TryRegister(openGenericServiceType, assemblies, Lifestyle.Singleton);

        /// <summary>
        ///     Registers all supplied <paramref name="implementationTypes"/> based on the closed-generic version
        ///     of the given <paramref name="openGenericServiceType"/> with the given <see cref="Lifestyle"/>.
        /// </summary>
        /// <param name="container">The <see cref="Container"/> to register the service to.</param>
        /// <param name="openGenericServiceType">The definition of the open generic type.</param>
        /// <param name="implementationTypes">A list types to be registered.</param>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> to register instances with.</param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one of the arguments contain a null reference (Nothing in VB).
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="openGenericServiceType"/> is not an open generic type or when one of
        ///     the supplied types from the <paramref name="implementationTypes"/> collection does not derive from
        ///     <paramref name="openGenericServiceType"/>.
        /// </exception>
        /// <returns>Whether the service was registered or not.</returns>
        public static bool TryRegisterSingleton(this Container container, Type openGenericServiceType, IEnumerable<Type> implementationTypes)
            => container.TryRegister(openGenericServiceType, implementationTypes, Lifestyle.Singleton);
    }
}
