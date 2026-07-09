using System.Diagnostics.CodeAnalysis;
using VMFramework.Core;

namespace VMFramework.Properties
{
    public interface IReadOnlyTokenProperty<in TToken> : IReadOnlyProperty<bool>
    {
        public bool ContainsToken([DisallowNull] TToken token);
    }
}