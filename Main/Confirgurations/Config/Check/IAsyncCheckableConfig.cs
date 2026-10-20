using System.Threading;
using Cysharp.Threading.Tasks;

namespace VMFramework.Configuration
{
    public interface IAsyncCheckableConfig : ICheckableConfig
    {
        UniTask CheckSettingsAsync(CancellationToken cancellationToken);
    }
}
