using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Pools;
using VMFramework.Procedure;

namespace VMFramework.GameLogicArchitecture
{
    [ManagerCreationProvider(ManagerType.GameItemCore)]
    public class GameItemTransformTracingManager : ManagerBehaviour<GameItemTransformTracingManager>
    {
        /// <summary>
        /// [Owner, Transforms]
        /// </summary>
        [TitleGroup(ComponentNames.CONFIG)]
        [ShowInInspector]
        protected readonly Dictionary<IControllerGameItem, HashSet<Transform>> transformsByOwner = new();

        /// <summary>
        /// [Position Source, Transforms]
        /// </summary>
        [TitleGroup(ComponentNames.CONFIG)]
        [ShowInInspector]
        protected readonly Dictionary<IControllerGameItem, HashSet<Transform>> transformsByPositionSource = new();

        /// <summary>
        /// [Target Transform, Position Source]
        /// </summary>
        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        protected readonly Dictionary<Transform, IControllerGameItem> positionSourceLookup = new();

        /// <summary>
        /// [Target Transform, Owner]
        /// </summary>
        [TitleGroup(ComponentNames.CONFIG)]
        [ShowInInspector]
        protected readonly Dictionary<Transform, IControllerGameItem> ownerLookup = new();

        protected IReturnEventProvider.ReturnHandler positionSourceReturnFunc;
        protected IReturnEventProvider.ReturnHandler ownerReturnFunc;

        protected override void Awake()
        {
            base.Awake();

            positionSourceReturnFunc = OnPositionSourceReturn;
            ownerReturnFunc = OnOwnerReturn;

            transformsByOwner.Clear();
            transformsByPositionSource.Clear();
            positionSourceLookup.Clear();
            ownerLookup.Clear();
        }

        protected virtual void OnPositionSourceReturn(IReturnEventProvider provider)
        {
            var positionSource = (IControllerGameItem)provider;
            RemovePositionSource(positionSource);
        }

        protected virtual void OnOwnerReturn(IReturnEventProvider provider)
        {
            var owner = (IControllerGameItem)provider;
            RemoveOwner(owner);
        }

        public virtual void Add(IControllerGameItem positionSource, IControllerGameItem owner,
            Transform targetTransform)
        {
            RemoveTransform(targetTransform);

            ownerLookup.Add(targetTransform, owner);
            positionSourceLookup.Add(targetTransform, positionSource);

            var targetTransformsByOwner =
                transformsByOwner.GetOrCreateFromFactory(owner, HashSetPoolFactory<Transform>.CreateFromDefaultPool);

            if (targetTransformsByOwner.Count == 0)
            {
                owner.OnReturnEvent += ownerReturnFunc;
            }

            targetTransformsByOwner.Add(targetTransform);

            var targetTransformsByPositionSource =
                transformsByPositionSource.GetOrCreateFromFactory(positionSource,
                    HashSetPoolFactory<Transform>.CreateFromDefaultPool);

            if (targetTransformsByPositionSource.Count == 0)
            {
                positionSource.OnReturnEvent += positionSourceReturnFunc;
            }

            targetTransformsByPositionSource.Add(targetTransform);
        }

        public virtual void RemoveTransform(Transform targetTransform)
        {
            if (ownerLookup.Remove(targetTransform, out var owner) == false)
            {
                return;
            }

            var positionSource = positionSourceLookup[targetTransform];
            positionSourceLookup.Remove(targetTransform);
            RemoveGroupBinding(transformsByOwner, owner, targetTransform, ownerReturnFunc);
            RemoveGroupBinding(transformsByPositionSource, positionSource, targetTransform,
                positionSourceReturnFunc);
        }

        public virtual void RemoveOwner(IControllerGameItem owner)
        {
            if (transformsByOwner.Remove(owner, out var targetTransforms) == false)
            {
                return;
            }

            owner.OnReturnEvent -= ownerReturnFunc;

            foreach (var targetTransform in targetTransforms)
            {
                var positionSource = positionSourceLookup[targetTransform];
                positionSourceLookup.Remove(targetTransform);
                RemoveGroupBinding(transformsByPositionSource, positionSource, targetTransform,
                    positionSourceReturnFunc);
                ownerLookup.Remove(targetTransform);
            }

            targetTransforms.Clear();
            targetTransforms.ReturnToDefaultPool();
        }

        public virtual void RemovePositionSource(IControllerGameItem positionSource)
        {
            if (transformsByPositionSource.Remove(positionSource, out var targetTransforms) == false)
            {
                return;
            }

            positionSource.OnReturnEvent -= positionSourceReturnFunc;

            foreach (var targetTransform in targetTransforms)
            {
                positionSourceLookup.Remove(targetTransform);

                var owner = ownerLookup[targetTransform];
                ownerLookup.Remove(targetTransform);
                RemoveGroupBinding(transformsByOwner, owner, targetTransform, ownerReturnFunc);
            }

            targetTransforms.Clear();
            targetTransforms.ReturnToDefaultPool();
        }

        private static void RemoveGroupBinding(
            Dictionary<IControllerGameItem, HashSet<Transform>> groups, IControllerGameItem item,
            Transform targetTransform, IReturnEventProvider.ReturnHandler returnHandler)
        {
            var transforms = groups[item];
            transforms.Remove(targetTransform);
            if (transforms.Count == 0)
            {
                groups.Remove(item);
                item.OnReturnEvent -= returnHandler;
                transforms.ReturnToDefaultPool();
            }
        }

        protected override void OnDestroy()
        {
            try
            {
                foreach (var (owner, transforms) in transformsByOwner)
                {
                    owner.OnReturnEvent -= ownerReturnFunc;
                    transforms.Clear();
                    transforms.ReturnToDefaultPool();
                }
                foreach (var (source, transforms) in transformsByPositionSource)
                {
                    source.OnReturnEvent -= positionSourceReturnFunc;
                    transforms.Clear();
                    transforms.ReturnToDefaultPool();
                }
                transformsByOwner.Clear();
                transformsByPositionSource.Clear();
                ownerLookup.Clear();
                positionSourceLookup.Clear();
            }
            finally
            {
                base.OnDestroy();
            }
        }

        protected virtual void Update()
        {
            foreach (var (positionSource, targetTransforms) in transformsByPositionSource)
            {
                foreach (var targetTransform in targetTransforms)
                {
                    targetTransform.position = positionSource.transform.position;
                }
            }
        }
    }
}
