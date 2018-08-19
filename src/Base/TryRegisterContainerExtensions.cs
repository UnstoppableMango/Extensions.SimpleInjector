using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using SimpleInjector;

namespace UnMango.Extensions.SimpleInjector
{
    /// <summary>
    ///     Extensions catching <see cref="InvalidOperationException"/>s during registration.
    /// </summary>
    [UsedImplicitly]
    public static class TryRegisterContainerExtensions
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
        public static bool TryRegister(this Container container, Type serviceType, Func<object> instanceCreator, Lifestyle lifestyle) {
            try {
                container.Register(serviceType, instanceCreator, lifestyle);
                return true;
            } catch (InvalidOperationException) {
                return false;
            }
        }

        /// <summary>
        ///     Registers the specified delegate that allows returning instances of <paramref name="serviceType"/>.
        /// </summary>
        /// <param name="container">The <see cref="Container"/> to register the service to.</param>
        /// <param name="serviceType">The base type or interface to register.</param>
        /// <param name="instanceCreator">The delegate that will be used for creating new instances.</param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when either <paramref name="serviceType"/> or <paramref name="instanceCreator"/> are null references (Nothing
        ///     in VB).
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="serviceType"/> represents an open generic type.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when this <paramref name="container"/> instance is locked and can not be altered, or when
        ///     a <paramref name="serviceType"/> has already been registered.
        /// </exception>
        /// <remarks>
        ///     This method uses the container's <see cref="ContainerOptions.LifestyleSelectionBehavior"/>
        ///     to select the exact <see cref="Lifestyle"/> for the specified type. By default this will be
        ///     <see cref="Lifestyle.Transient"/>.
        /// </remarks>
        /// <returns>Whether the service was registered or not.</returns>
        public static bool TryRegister(this Container container, Type serviceType, Func<object> instanceCreator) {
            try {
                container.Register(serviceType, instanceCreator);
                return true;
            } catch (InvalidOperationException) {
                return false;
            }
        }

        /// <summary>
        ///     Registers that an instance of type <paramref name="implementationType"/> will be returned when an
        ///     instance of type <paramref name="serviceType"/> is requested. The instance is cached according to
        ///     the supplied <see cref="Lifestyle"/>. Open and closed generic types are supported.
        /// </summary>
        /// <param name="container">The <see cref="Container"/> to register the service to.</param>
        /// <param name="serviceType">
        ///     The interface or base type that can be used to retrieve the instances. This can
        ///     be an open-generic type.
        /// </param>
        /// <param name="implementationType">The concrete type that will be registered. This can be an open-generic type.</param>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> that specifies how the returned instance will be cached.</param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one of the supplied arguments is a null reference (Nothing in VB).
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Thrown when the given <paramref name="implementationType"/> type is not a type that can be created
        ///     by the container, when either <paramref name="serviceType"/> or <paramref name="implementationType"/> are open generic
        ///     types, or when <paramref name="serviceType"/> is not assignable from the <paramref name="implementationType"/>.
        /// </exception>
        /// <returns>Whether the service was registered or not.</returns>
        public static bool TryRegister(this Container container, Type serviceType, Type implementationType, Lifestyle lifestyle) {
            try {
                container.Register(serviceType, implementationType, lifestyle);
                return true;
            } catch (InvalidOperationException) {
                return false;
            }
        }

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
        public static bool TryRegister(this Container container, Type serviceType, Type implementationType) {
            try {
                container.Register(serviceType, implementationType);
                return true;
            } catch (InvalidOperationException) {
                return false;
            }
        }

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
        public static bool TryRegister<TConcrete>(this Container container, Lifestyle lifestyle)
            where TConcrete : class {
            try {
                container.Register<TConcrete>(lifestyle);
                return true;
            } catch (InvalidOperationException) {
                return false;
            }
        }

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
        public static bool TryRegister<TService>(this Container container, Func<TService> instanceCreator, Lifestyle lifestyle)
            where TService : class {
            try {
                container.Register(instanceCreator, lifestyle);
                return true;
            } catch (InvalidOperationException) {
                return false;
            }
        }

        /// <summary>
        ///     Registers the specified delegate that allows returning transient instances of
        ///     <typeparamref name="TService"/>. The delegate is expected to always return a new instance on each call.
        /// </summary>
        /// <typeparam name="TService">The interface or base type that can be used to retrieve instances.</typeparam>
        /// <param name="container">The <see cref="Container"/> to register the service to.</param>
        /// <param name="instanceCreator">The delegate that allows building or creating new instances.</param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when instanceCreator is a null reference.
        /// </exception>
        /// <returns>Whether the service was registered or not.</returns>
        public static bool TryRegister<TService>(this Container container, Func<TService> instanceCreator)
            where TService : class {
            try {
                container.Register(instanceCreator);
                return true;
            } catch (InvalidOperationException) {
                return false;
            }
        }

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
        public static bool TryRegister<TService, TImplementation>(this Container container, Lifestyle lifestyle)
            where TService : class
            where TImplementation : class, TService {
            try {
                container.Register<TService, TImplementation>(lifestyle);
                return true;
            } catch (InvalidOperationException) {
                return false;
            }
        }

        /// <summary>
        ///     Registers that a new instance of <typeparamref name="TConcrete"/> will be returned every time it is
        ///     requested (transient).
        /// </summary>
        /// <typeparam name="TConcrete">The concrete type that will be registered.</typeparam>
        /// <param name="container">The <see cref="Container"/> to register the service to.</param>
        /// <exception cref="ArgumentException">
        ///     Thrown when the <typeparamref name="TConcrete"/> is a type that can not be created by the container.
        /// </exception>
        /// <remarks>
        ///     This method uses the <see cref="Container"/>'s <see cref="ContainerOptions.LifestyleSelectionBehavior"/>
        ///     to select the exact <see cref="Lifestyle"/> for the specified type. By default this will be
        ///     <see cref="Lifestyle.Transient"/>.
        /// </remarks>
        /// <returns>Whether the service was registered or not.</returns>
        public static bool TryRegister<TConcrete>(this Container container)
            where TConcrete : class {
            try {
                container.Register<TConcrete>();
                return true;
            } catch (InvalidOperationException) {
                return false;
            }
        }

        /// <summary>
        ///     Registers that a new instance of <paramref name="concreteType"/> will be returned every time it
        ///     is requested (transient).
        /// </summary>
        /// <param name="container">The <see cref="Container"/> to register the service to.</param>
        /// <param name="concreteType">The concrete type that will be registered.</param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="concreteType"/> is a null references (Nothing in VB).
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="concreteType"/> represents an open generic type or is a type that can
        ///     not be created by the <see cref="Container"/>.
        /// </exception>
        /// <remarks>
        ///     This method uses the <see cref="Container"/>'s <see cref="ContainerOptions.LifestyleSelectionBehavior"/>
        ///     to select the exact <see cref="Lifestyle"/> for the specified type. By default this will be
        ///     <see cref="Lifestyle.Transient"/>.
        /// </remarks>
        /// <returns>Whether the service was registered or not.</returns>
        public static bool TryRegister(this Container container, Type concreteType) {
            try {
                container.Register(concreteType);
                return true;
            } catch (InvalidOperationException) {
                return false;
            }
        }

        /// <summary>
        ///     Registers that a new instance of <typeparamref name="TImplementation"/> will be returned every time
        ///     a <typeparamref name="TService"/> is requested (transient).
        /// </summary>
        /// <typeparam name="TService">The interface or base type that can be used to retrieve the instances.</typeparam>
        /// <typeparam name="TImplementation">The concrete type that will be registered.</typeparam>
        /// <param name="container">The <see cref="Container"/> to register the service to.</param>
        /// <exception cref="ArgumentException">
        ///     Thrown when the given <typeparamref name="TImplementation"/> type is not a type that can be created
        ///     by the container.
        /// </exception>
        /// <remarks>
        ///     This method uses the <see cref="Container"/>'s <see cref="ContainerOptions.LifestyleSelectionBehavior"/>
        ///     to select the exact <see cref="Lifestyle"/> for the specified type. By default this will be
        ///     <see cref="Lifestyle.Transient"/>.
        /// </remarks>
        /// <returns>Whether the service was registered or not.</returns>
        public static bool TryRegister<TService, TImplementation>(this Container container)
            where TService : class
            where TImplementation : class, TService {
            try {
                container.Register<TService, TImplementation>();
                return true;
            } catch (InvalidOperationException) {
                return false;
            }
        }

        /// <summary>
        ///     Registers all concrete, non-generic, public and internal types in the given set
        ///     of assemblies that implement the given <paramref name="openGenericServiceType"/> with <paramref name="container"/>'s
        ///     default <see cref="Lifestyle"/> (which is transient by default). <see cref="TypesToRegisterOptions.IncludeDecorators"/>
        ///     and <see cref="TypesToRegisterOptions.IncludeGenericTypeDefinitions"/> will
        ///     be excluded from registration, while <see cref="TypesToRegisterOptions.IncludeComposites"/>
        ///     are included.
        /// </summary>
        /// <param name="container">The <see cref="Container"/> to register the service to.</param>
        /// <param name="openGenericServiceType">The definition of the open generic type.</param>
        /// <param name="assemblies">A list of assemblies that will be searched.</param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one of the arguments contain a null reference (Nothing in VB).
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="openGenericServiceType"/> is not an open generic type.
        /// </exception>
        /// <returns>Whether the service was registered or not.</returns>
        public static bool TryRegister(this Container container, Type openGenericServiceType, params Assembly[] assemblies) {
            try {
                container.Register(openGenericServiceType, assemblies);
                return true;
            } catch (InvalidOperationException) {
                return false;
            }
        }

        /// <summary>
        ///     Registers all concrete, non-generic, public and internal types in the given set
        ///     of assemblies that implement the given <paramref name="openGenericServiceType"/> with <paramref name="container"/>'s
        ///     default <see cref="Lifestyle"/> (which is transient by default). <see cref="TypesToRegisterOptions.IncludeDecorators"/>
        ///     and <see cref="TypesToRegisterOptions.IncludeGenericTypeDefinitions"/> will
        ///     be excluded from registration, while <see cref="TypesToRegisterOptions.IncludeComposites"/>
        ///     are included.
        /// </summary>
        /// <param name="container">The <see cref="Container"/> to register the service to.</param>
        /// <param name="openGenericServiceType">The definition of the open generic type.</param>
        /// <param name="assemblies">A list of assemblies that will be searched.</param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one of the arguments contain a null reference (Nothing in VB).
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="openGenericServiceType"/> is not an open generic type.
        /// </exception>
        /// <returns>Whether the service was registered or not.</returns>
        public static bool TryRegister(this Container container, Type openGenericServiceType, IEnumerable<Assembly> assemblies) {
            try {
                container.Register(openGenericServiceType, assemblies);
                return true;
            } catch (InvalidOperationException) {
                return false;
            }
        }

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
        public static bool TryRegister(this Container container, Type openGenericServiceType, Assembly assembly, Lifestyle lifestyle) {
            try {
                container.Register(openGenericServiceType, assembly, lifestyle);
                return true;
            } catch (InvalidOperationException) {
                return false;
            }
        }

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
        public static bool TryRegister(this Container container, Type openGenericServiceType, IEnumerable<Assembly> assemblies, Lifestyle lifestyle) {
            try {
                container.Register(openGenericServiceType, assemblies, lifestyle);
                return true;
            } catch (InvalidOperationException) {
                return false;
            }
        }

        /// <summary>
        ///     Registers all supplied <paramref name="implementationTypes"/> based on the closed-generic version
        ///     of the given <paramref name="openGenericServiceType"/> with the transient <see cref="Lifestyle"/>.
        /// </summary>
        /// <param name="container">The <see cref="Container"/> to register the service to.</param>
        /// <param name="openGenericServiceType">The definition of the open generic type.</param>
        /// <param name="implementationTypes">A list types to be registered.</param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one of the arguments contain a null reference (Nothing in VB).
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="openGenericServiceType"/> is not an open generic type or when one of
        ///     the supplied types from the <paramref name="implementationTypes"/> collection does not derive from
        ///     <paramref name="openGenericServiceType"/>.
        /// </exception>
        /// <returns>Whether the service was registered or not.</returns>
        public static bool TryRegister(this Container container, Type openGenericServiceType, IEnumerable<Type> implementationTypes) {
            try {
                container.Register(openGenericServiceType, implementationTypes);
                return true;
            } catch (InvalidOperationException) {
                return false;
            }
        }

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
        public static bool TryRegister(this Container container, Type openGenericServiceType, IEnumerable<Type> implementationTypes, Lifestyle lifestyle) {
            try {
                container.Register(openGenericServiceType, implementationTypes, lifestyle);
                return true;
            } catch (InvalidOperationException) {
                return false;
            }
        }
    }
}
