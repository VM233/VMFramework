using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Profiling;
using VMFramework.Procedure;

namespace VMFramework.Configuration
{
    public static class ConfigurationValidation
    {
        public static async UniTask CheckAsync(ICheckableConfig config, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var marker = new ProfilerMarker($"Validate config: {config.GetType().FullName}");
            if (config is IAsyncCheckableConfig asynchronous)
            {
                UniTask actual;
                using (marker.Auto()) actual = asynchronous.CheckSettingsAsync(cancellationToken);
                await actual;
            }
            else
            {
                using (marker.Auto()) config.CheckSettings();
            }
            cancellationToken.ThrowIfCancellationRequested();
        }

        public static async UniTask CheckAllAsync(IEnumerable<ICheckableConfig> configs, CancellationToken cancellationToken)
        {
            var slice = new InitializationWorkSlice();
            foreach (var config in configs)
            {
                await slice.YieldIfBudgetExpired(cancellationToken);
                await CheckAsync(config, cancellationToken);
            }
        }
    }
}
