using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.Core.Pools;

namespace VMFramework.Properties
{
    public class DataTokenProperty<TToken, TData> : IDataTokenProperty<TToken, TData>
    {
        public string Name { get; set; }

        [ShowInInspector]
        public object Owner { get; protected set; }

        object IReadOnlyProperty.ObjectValue => null;

        public IReadOnlyCollection<TToken> Tokens => dataLookup.Keys;

        public IReadOnlyCollection<TData> AllData => tokensLookup.Keys;

        public event PropertyDirtyHandler OnDirty;

        public event IReadOnlyDataTokenProperty<TToken, TData>.DataChangedHandler OnDataChanged;

        public event IReadOnlyDataTokenProperty<TToken, TData>.TokenChangedHandler OnTokenChanged;

        [ShowInInspector]
        [DictionaryDrawerSettings(IsReadOnly = true)]
        protected readonly Dictionary<TToken, HashSet<TData>> dataLookup = new();

        [ShowInInspector]
        [DictionaryDrawerSettings(IsReadOnly = true)]
        protected readonly Dictionary<TData, HashSet<TToken>> tokensLookup = new();

        public void SetOwner(object owner)
        {
            Owner = owner;
        }

        public bool ContainsToken(TToken token)
        {
            return dataLookup.ContainsKey(token);
        }

        public bool ContainsData(TData data)
        {
            return tokensLookup.ContainsKey(data);
        }

        public bool TryGetTokens(TData data, out IReadOnlyCollection<TToken> tokens)
        {
            if (tokensLookup.TryGetValue(data, out var tokenCollection))
            {
                tokens = tokenCollection;
                return true;
            }

            tokens = null;
            return false;
        }

        public bool TryGetData(TToken token, out IReadOnlyCollection<TData> dataCollection)
        {
            if (dataLookup.TryGetValue(token, out var data))
            {
                dataCollection = data;
                return true;
            }

            dataCollection = null;
            return false;
        }

        public virtual void AddData(TToken token, TData data)
        {
            bool added;
            bool tokenAdded;

            if (dataLookup.TryGetValue(token, out var dataCollection) == false)
            {
                dataCollection = HashSetPool<TData>.Default.Get();
                dataCollection.Clear();
                dataCollection.Add(data);
                added = true;
                tokenAdded = true;

                dataLookup[token] = dataCollection;
            }
            else
            {
                added = dataCollection.Add(data);
                tokenAdded = false;
            }

            if (added == false)
            {
                return;
            }

            bool dataAdded = false;
            if (tokensLookup.TryGetValue(data, out var tokenCollection) == false)
            {
                tokenCollection = HashSetPool<TToken>.Default.Get();
                tokenCollection.Clear();
                tokensLookup[data] = tokenCollection;
                dataAdded = true;
            }

            tokenCollection.Add(token);

            OnDirty?.Invoke(this, initial: false);
            if (tokenAdded)
            {
                OnTokenChanged?.Invoke(this, token, added: true);
            }

            if (dataAdded)
            {
                OnDataChanged?.Invoke(this, data, added: true);
            }
        }

        public virtual void RemoveData(TData data)
        {
            if (tokensLookup.TryGetValue(data, out var tokenCollection) == false)
            {
                return;
            }

            var tokens = ListPool<TToken>.Default.Get();
            tokens.Clear();
            tokens.AddRange(tokenCollection);

            foreach (var token in tokens)
            {
                RemoveData(token, data);
            }

            tokens.ReturnToDefaultPool();
        }

        public virtual void RemoveData(TToken token, TData data)
        {
            if (dataLookup.TryGetValue(token, out var dataCollection) == false)
            {
                return;
            }

            if (dataCollection.Remove(data) == false)
            {
                return;
            }

            var tokenRemoved = dataCollection.Count <= 0;
            if (tokenRemoved)
            {
                dataLookup.Remove(token);
                dataCollection.ReturnToDefaultPool();
            }

            var dataRemoved = false;
            if (tokensLookup.TryGetValue(data, out var tokenCollection))
            {
                tokenCollection.Remove(token);

                if (tokenCollection.Count <= 0)
                {
                    tokensLookup.Remove(data);
                    tokenCollection.ReturnToDefaultPool();
                    dataRemoved = true;
                }
            }

            OnDirty?.Invoke(this, initial: false);
            if (dataRemoved)
            {
                OnDataChanged?.Invoke(this, data, added: false);
            }

            if (tokenRemoved)
            {
                OnTokenChanged?.Invoke(this, token, added: false);
            }
        }

        public virtual void RemoveToken(TToken token)
        {
            if (dataLookup.Remove(token, out var dataCollection) == false)
            {
                return;
            }

            var removedData = ListPool<TData>.Default.Get();
            removedData.Clear();

            foreach (var data in dataCollection)
            {
                if (tokensLookup.TryGetValue(data, out var tokenCollection))
                {
                    tokenCollection.Remove(token);

                    if (tokenCollection.Count <= 0)
                    {
                        tokensLookup.Remove(data);
                        tokenCollection.ReturnToDefaultPool();
                        removedData.Add(data);
                    }
                }
            }

            OnDirty?.Invoke(this, initial: false);

            foreach (var data in removedData)
            {
                OnDataChanged?.Invoke(this, data, added: false);
            }

            OnTokenChanged?.Invoke(this, token, added: false);

            dataCollection.ReturnToDefaultPool();
            removedData.ReturnToDefaultPool();
        }

        public virtual void Clear()
        {
            foreach (var (token, dataCollection) in dataLookup)
            {
                OnTokenChanged?.Invoke(this, token, added: false);

                dataCollection.ReturnToDefaultPool();
            }

            foreach (var (data, tokenCollection) in tokensLookup)
            {
                OnDataChanged?.Invoke(this, data, added: false);

                tokenCollection.ReturnToDefaultPool();
            }

            dataLookup.Clear();
            tokensLookup.Clear();

            OnDirty?.Invoke(this, initial: false);
        }

        public virtual void ClearAndReset()
        {
            Clear();
            OnDirty = null;
            OnDataChanged = null;
        }
    }
}