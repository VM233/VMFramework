using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.Procedure;

namespace VMFramework.UI
{
    [ManagerCreationProvider(ManagerType.UICore)]
    public sealed class SlotGlobalFiltersManager : ManagerBehaviour<SlotGlobalFiltersManager>
    {
        [ShowInInspector]
        private readonly Dictionary<string, Dictionary<IToken, ISlotFilter>> filtersByID = new();

        public event Action OnFilterChanged;

        protected override void Awake()
        {
            base.Awake();

            filtersByID.Clear();
        }

        protected override void OnBeforeInitStart()
        {
            base.OnBeforeInitStart();

            filtersByID.Clear();
            foreach (var id in GamePrefabManager.GetAllIDs<SlotFilterConfig>())
            {
                filtersByID[id] = new();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void IsMatch(SlotVisualElement slot, IDictionary<string, bool?> matchResults)
        {
            foreach (var (id, filters) in filtersByID)
            {
                if (filters.Count <= 0)
                {
                    matchResults[id] = null;
                    continue;
                }

                bool anyMatched = false;
                foreach (var filter in filters.Values)
                {
                    var matched = filter.IsMatch(slot);

                    if (matched.HasValue == false)
                    {
                        continue;
                    }

                    if (matched.Value)
                    {
                        anyMatched = true;
                    }
                    else
                    {
                        matchResults[id] = false;
                        goto CONTINUE;
                    }
                }

                if (anyMatched)
                {
                    matchResults[id] = true;
                }
                else
                {
                    matchResults[id] = null;
                }

                CONTINUE:
                continue;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AddFilter(string filterID, IToken token, ISlotFilter filter)
        {
            var filters = filtersByID.GetOrCreate(filterID);

            if (filters.TryAdd(token, filter))
            {
                OnFilterChanged?.Invoke();
                return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool RemoveFilter(string filterID, IToken token)
        {
            if (filtersByID.TryGetValue(filterID, out var filters) == false)
            {
                return false;
            }

            if (filters.Remove(token))
            {
                OnFilterChanged?.Invoke();
                return true;
            }

            return false;
        }
    }
}