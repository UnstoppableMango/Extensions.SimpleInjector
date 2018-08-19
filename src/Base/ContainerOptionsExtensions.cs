using System;
using JetBrains.Annotations;
using SimpleInjector;

namespace UnMango.Extensions.SimpleInjector
{
    public static class ContainerOptionsExtensions
    {
        public static bool TryChange(this ContainerOptions options, [NotNull] Action<ContainerOptions> change) {
            try {
                Check.NotNull(change, nameof(change)).Invoke(options);
                return true;
            } catch (InvalidOperationException) {
                return false;
            }
        }
    }
}
