using System.Diagnostics.CodeAnalysis;
using VMFramework.Core;

namespace VMFramework.Properties
{
    public interface ITokenProperty<in TToken> : IReadOnlyTokenProperty<TToken>
    {
        public void AddToken([DisallowNull] TToken token);

        public void RemoveToken([DisallowNull] TToken token);
    }
}