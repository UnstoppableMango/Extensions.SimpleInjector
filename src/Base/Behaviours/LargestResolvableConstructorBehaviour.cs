using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using SimpleInjector;
using SimpleInjector.Advanced;

namespace UnMango.Extensions.SimpleInjector.Behaviours
{
    // Taken from SI docs
    public class LargestResolvableConstructorBehaviour : IConstructorResolutionBehavior
    {
        private readonly Container _container;

        public LargestResolvableConstructorBehaviour(Container container)
        {
            _container = container;
        }

        private bool IsCalledDuringRegistrationPhase => !_container.IsLocked();

        [DebuggerStepThrough]
        public ConstructorInfo GetConstructor(Type implementationType)
        {
            var constructor = GetConstructors(implementationType).FirstOrDefault();
            if (constructor != null) return constructor;
            throw new ActivationException(BuildExceptionMessage(implementationType));
        }

        private IEnumerable<ConstructorInfo> GetConstructors(Type implementation) =>
            from ctor in implementation.GetConstructors()
            let parameters = ctor.GetParameters()
            where IsCalledDuringRegistrationPhase
                || implementation.GetConstructors().Length == 1
                || ctor.GetParameters().All(CanBeResolved)
            orderby parameters.Length descending
            select ctor;

        private bool CanBeResolved(ParameterInfo parameter) =>
            GetInstanceProducerFor(new InjectionConsumerInfo(parameter)) != null;

        private InstanceProducer GetInstanceProducerFor(InjectionConsumerInfo i) =>
            _container.Options.DependencyInjectionBehavior.GetInstanceProducer(i, false);

        private static string BuildExceptionMessage(Type type) =>
            !type.GetConstructors().Any()
                ? TypeShouldHaveAtLeastOnePublicConstructor(type)
                : TypeShouldHaveConstructorWithResolvableTypes(type);

        private static string TypeShouldHaveAtLeastOnePublicConstructor(Type type) =>
            string.Format(CultureInfo.InvariantCulture,
                "For the container to be able to create {0}, it should contain at least " +
                "one public constructor.", type.ToFriendlyName());

        private static string TypeShouldHaveConstructorWithResolvableTypes(Type type) =>
            string.Format(CultureInfo.InvariantCulture,
                "For the container to be able to create {0}, it should contain a public " +
                "constructor that only contains parameters that can be resolved.",
                type.ToFriendlyName());
    }
}
