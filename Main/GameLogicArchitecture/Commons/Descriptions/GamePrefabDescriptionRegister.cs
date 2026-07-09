using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Localization;
using VMFramework.Core;
using VMFramework.Core.Pools;
using VMFramework.OdinExtensions;

namespace VMFramework.GameLogicArchitecture
{
    public class GamePrefabDescriptionRegister : MonoBehaviour
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [CommonPreset(DescriptionMeta.TYPE_PRESET_KEY)]
        [IsNotNullOrEmpty]
        public string type;

        [TitleGroup(ComponentNames.CONFIG)]
        public bool copyDescription = false;

        protected IGameItem gameItem;
        protected IPoolEventProvider poolEventProvider;
        protected DescriptionManager descriptionManager;

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowIf(nameof(copyDescription))]
        [ShowInInspector]
        protected LocalizedString localizedDescription;

        protected virtual void Awake()
        {
            gameItem = GetComponentInParent<IGameItem>();
            poolEventProvider = GetComponentInParent<IPoolEventProvider>();
            poolEventProvider.OnGetEvent += OnGet;

            descriptionManager = GetComponentInParent<DescriptionManager>();
            descriptionManager.Register(type, Generate);
            
            localizedDescription = null;
        }

        protected virtual void OnGet(IPoolEventProvider provider)
        {
            if (copyDescription)
            {
                if (localizedDescription == null)
                {
                    if (GamePrefabManager.TryGetGamePrefab(gameItem.id, out var gamePrefab))
                    {
                        if (gamePrefab is ILocalizedDescriptionOwner { DescriptionReference: { } descriptionReference })
                        {
                            localizedDescription = new LocalizedString();
                            localizedDescription.TableReference = descriptionReference.TableReference;
                            localizedDescription.TableEntryReference = descriptionReference.TableEntryReference;
                        }
                    }
                }
            }
            else
            {
                if (GamePrefabManager.TryGetGamePrefab(gameItem.id, out var gamePrefab))
                {
                    if (gamePrefab is ILocalizedDescriptionOwner descriptionOwner)
                    {
                        localizedDescription = descriptionOwner.DescriptionReference;
                    }
                }
            }

            if (localizedDescription != null)
            {
                descriptionManager.Register(type, localizedDescription);
            }
        }

        protected virtual bool Generate(out string description)
        {
            if (localizedDescription != null)
            {
                description = localizedDescription.GetLocalizedString();
                return description.IsNullOrEmpty() == false;
            }

            description = null;
            return false;
        }
    }
}