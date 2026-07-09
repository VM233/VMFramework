using System.Diagnostics.CodeAnalysis;
using VMFramework.Core;

namespace VMFramework.Properties
{
    public interface IReadOnlyDataTokenProperty<TToken, TData> : IReadOnlyProperty
    {
        public delegate void DataChangedHandler(IReadOnlyProperty property, TData data, bool added);
        
        public delegate void TokenChangedHandler(IReadOnlyProperty property, TToken token, bool added);
        
        public event DataChangedHandler OnDataChanged;
        
        public event TokenChangedHandler OnTokenChanged;
        
        public bool ContainsToken([DisallowNull] TToken token);
    }
}