using System.Threading;
using Cysharp.Threading.Tasks;

namespace VMFramework.Procedure
{
    public delegate UniTask InitActionHandler(CancellationToken cancellationToken);
}
