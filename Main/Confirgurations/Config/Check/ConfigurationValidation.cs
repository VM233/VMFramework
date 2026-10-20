using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using VMFramework.Procedure;

namespace VMFramework.Configuration
{
    public static class ConfigurationValidation
    {
        public static async UniTask CheckAsync(ICheckableConfig config, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (config is IAsyncCheckableConfig asynchronous)
                await asynchronous.CheckSettingsAsync(cancellationToken);
            else config.CheckSettings();
            cancellationToken.ThrowIfCancellationRequested();
        }

        public static async UniTask CheckAllAsync(IEnumerable<ICheckableConfig> configs, CancellationToken cancellationToken)
        {
            var slice = new InitializationWorkSlice();
            foreach (var config in configs)
            {
                await CheckAsync(config, cancellationToken);
                await slice.YieldIfBudgetExpired(cancellationToken);
            }
        }
    }
}
