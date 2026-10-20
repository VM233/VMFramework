using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using VMFramework.Core;

namespace VMFramework.Procedure
{
    public static class InitializationTypeDiscovery
    {
        public static async UniTask<IReadOnlyList<Type>> FindAsync(Type contract, bool includingSelf,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var result = new List<Type>();
            var slice = new InitializationWorkSlice();
            foreach (var assembly in assemblies)
            {
                cancellationToken.ThrowIfCancellationRequested();
                foreach (var type in contract.GetDerivedClasses(assembly, includingSelf, false))
                    if (!type.IsAbstract && !type.IsInterface) result.Add(type);
                await slice.YieldIfBudgetExpired(cancellationToken);
            }
            return result;
        }
    }
}
