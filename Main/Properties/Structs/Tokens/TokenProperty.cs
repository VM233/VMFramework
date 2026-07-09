using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.Core;

namespace VMFramework.Properties
{
    public class TokenProperty<TToken> : ITokenProperty<TToken> where TToken : class
    {
        public string Name { get; set; }

        [ShowInInspector]
        public object Owner { get; protected set; }

        public bool TrueIfEmptyTokens { get; set; } = true;

        public event PropertyDirtyHandler OnDirty;

        [ShowInInspector]
        protected readonly HashSet<TToken> tokens = new();

        public void SetOwner(object owner)
        {
            Owner = owner;
        }

        public bool GetValue()
        {
            return tokens.Count > 0 ^ TrueIfEmptyTokens;
        }

        public void AddToken(TToken token)
        {
            token.AssertIsNotNull(nameof(token));

            if (tokens.Add(token) == false)
            {
                return;
            }

            if (tokens.Count == 1)
            {
                OnDirty?.Invoke(this, initial: false);
            }
        }

        public void RemoveToken(TToken token)
        {
            token.AssertIsNotNull(nameof(token));

            if (tokens.Remove(token) == false)
            {
                return;
            }

            if (tokens.Count == 0)
            {
                OnDirty?.Invoke(this, initial: false);
            }
        }

        public bool ContainsToken(TToken token)
        {
            return tokens.Contains(token);
        }

        public void Clear()
        {
            tokens.Clear();
        }
    }
}