using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace UnMango.Extensions.SimpleInjector
{
    using Behaviours;

    public static partial class ContainerExtensions
    {
        public static Container UseLargestResolvableConstructor(this Container container)
        {
            var behaviour = new LargestResolvableConstructorBehaviour(container);

            container.Options.TryChange(x => x.ConstructorResolutionBehavior = behaviour);

            return container;
        }

        public static Container UseAsyncScopedLifestyle(this Container container)
        {
            var lifestyle = new AsyncScopedLifestyle();

            container.Options.TryChange(x => x.DefaultScopedLifestyle = lifestyle);

            return container;
        }
    }
}
