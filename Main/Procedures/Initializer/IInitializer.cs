using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace VMFramework.Procedure
{
    public delegate UniTask InitActionHandler(CancellationToken cancellationToken);
    
    public interface IInitializer
    {
        public bool EnableInitializationDebugLog => true;

        public void GetInitializationActions(ICollection<InitializationAction> actions);
    }
}
