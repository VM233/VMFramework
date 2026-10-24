using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Profiling;
using VMFramework.Core;

namespace VMFramework.Procedure
{
    public static class InitializationTypeDiscovery
    {
        public static UniTask<IReadOnlyList<Type>> FindAsync(Type contract, bool includingSelf,
            CancellationToken cancellationToken)
            => DiscoverAsync(contract, includingSelf, true, cancellationToken);

        public static UniTask<IReadOnlyList<Type>> FindAllAsync(Type contract, bool includingSelf,
            CancellationToken cancellationToken)
            => DiscoverAsync(contract, includingSelf, false, cancellationToken);

        private static async UniTask<IReadOnlyList<Type>> DiscoverAsync(Type contract, bool includingSelf,
            bool concreteOnly, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var result = new List<Type>();
            var slice = new InitializationWorkSlice();
            foreach (var assembly in assemblies)
            {
                await slice.YieldIfBudgetExpired(cancellationToken);
                using (new ProfilerMarker($"Discover initialization: {contract.FullName} / {assembly.GetName().Name}").Auto())
                    foreach (var type in contract.GetDerivedClasses(assembly, includingSelf, false))
                        if (!concreteOnly || !type.IsAbstract && !type.IsInterface) result.Add(type);
            }
            return result;
        }
    }
}
