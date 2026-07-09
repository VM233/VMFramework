using System.Diagnostics.CodeAnalysis;

namespace VMFramework.Properties
{
    public interface IDataTokenProperty<TToken, TData> : IReadOnlyDataTokenProperty<TToken, TData>
    {
        public void AddData([DisallowNull] TToken token, [DisallowNull] TData data);

        public void RemoveData([DisallowNull] TToken token, [DisallowNull] TData data);

        public void RemoveToken([DisallowNull] TToken token);
    }
}