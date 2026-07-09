using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.GameLogicArchitecture
{
    public class StateCloner : MonoBehaviour, IStateCloner
    {
        [ListDrawerSettings(ShowFoldout = false)]
        [ShowInInspector]
        protected readonly SortedDictionary<int, List<IStateCloneable>> stateCloneables = new();

        protected virtual void Awake()
        {
            foreach (var stateCloneable in GetComponentsInChildren<IStateCloneable>())
            {
                var priority = stateCloneable.ClonePriority;
                var list = stateCloneables.GetOrCreate(priority);
                list.Add(stateCloneable);
            }
        }

        public virtual void CloneFrom(IStateCloner cloner, StateCloneHint hint)
        {
            var stateCloner = (StateCloner)cloner;
            foreach (var (priority, list) in stateCloneables)
            {
                for (int i = 0;  i < list.Count; i++)
                {
                    list[i].CloneFrom(stateCloner.stateCloneables[priority][i], hint);
                }
            }
        }
    }
}