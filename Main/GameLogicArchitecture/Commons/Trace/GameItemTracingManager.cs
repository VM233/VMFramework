using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Pools;
using VMFramework.Procedure;

namespace VMFramework.GameLogicArchitecture
{
    [ManagerCreationProvider(ManagerType.GameItemCore)]
    public class GameItemTracingManager : ManagerBehaviour<GameItemTracingManager>
    {
        /// <summary>
        /// [Position Source, [Target, Offset]]
        /// </summary>
        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        protected readonly Dictionary<IControllerGameItem, Dictionary<IControllerGameItem, Vector3>>
            offsetTargetLookup = new();

        /// <summary>
        /// [Target, Position Source]
        /// </summary>
        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        protected readonly Dictionary<IControllerGameItem, IControllerGameItem> positionSourceLookup = new();

        protected IReturnEventProvider.ReturnHandler sourceReturnFunc;
        protected IReturnEventProvider.ReturnHandler targetReturnFunc;

        protected override void Awake()
        {
            base.Awake();

            sourceReturnFunc = OnSourceReturn;
            targetReturnFunc = OnTargetReturn;

            offsetTargetLookup.Clear();
            positionSourceLookup.Clear();
        }

        public virtual void Add(IControllerGameItem positionSource, IControllerGameItem target)
        {
            var offset = positionSource.transform.position - target.transform.position;
            Add(positionSource, target, offset);
        }

        public virtual void Add(IControllerGameItem positionSource, IControllerGameItem target, Vector3 offset)
        {
            RemoveTarget(target);

            var sourcesDict = offsetTargetLookup.GetOrCreateFromFactory(positionSource,
                DictionaryPoolFactory<IControllerGameItem, Vector3>.CreateFromDefaultPool);

            if (sourcesDict.Count == 0)
            {
                positionSource.OnReturnEvent += sourceReturnFunc;
            }

            sourcesDict[target] = offset;

            positionSourceLookup[target] = positionSource;

            target.OnReturnEvent += targetReturnFunc;
        }

        public virtual void RemoveTarget(IControllerGameItem target)
        {
            if (positionSourceLookup.Remove(target, out var source) == false)
            {
                return;
            }

            if (offsetTargetLookup.TryGetValue(source, out var offsetTargets))
            {
                if (offsetTargets.Remove(target))
                {
                    if (offsetTargets.Count == 0)
                    {
                        offsetTargetLookup.Remove(source);
                        offsetTargets.ReturnToDefaultPool();
                    }
                }
            }

            target.OnReturnEvent -= targetReturnFunc;
        }

        public virtual void RemovePositionSource(IControllerGameItem source)
        {
            source.OnReturnEvent -= sourceReturnFunc;

            if (offsetTargetLookup.Remove(source, out var offsetTargets))
            {
                foreach (var target in offsetTargets.Keys)
                {
                    positionSourceLookup.Remove(target);
                    target.OnReturnEvent -= targetReturnFunc;
                }

                offsetTargets.ReturnToDefaultPool();
            }
        }

        protected virtual void OnTargetReturn(IReturnEventProvider provider)
        {
            var target = (IControllerGameItem)provider;
            RemoveTarget(target);
        }

        protected virtual void OnSourceReturn(IReturnEventProvider provider)
        {
            var source = (IControllerGameItem)provider;
            RemovePositionSource(source);
        }

        protected virtual void Update()
        {
            foreach (var (source, offsetTargets) in offsetTargetLookup)
            {
                foreach (var (target, offset) in offsetTargets)
                {
                    target.transform.position = source.transform.position + offset;
                }
            }
        }
    }
}