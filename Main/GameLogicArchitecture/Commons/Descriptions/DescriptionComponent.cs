using System.Threading;
using System.Threading.Tasks;
using VMFramework.Localization;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.GameLogicArchitecture
{
    public class DescriptionComponent : MonoBehaviour
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [CommonPreset(DescriptionMeta.TYPE_PRESET_KEY)]
        [IsNotNullOrEmpty]
        public string type;

        [TitleGroup(ComponentNames.CONFIG)]
        public LocalizedString localizedDescription = new();

        protected DescriptionManager descriptionManager;

        protected virtual void Awake()
        {
            descriptionManager = GetComponentInParent<DescriptionManager>();

            if (localizedDescription.TableReference.ReferenceType == TableReference.Type.Empty ||
                localizedDescription.TableEntryReference.ReferenceType == TableEntryReference.Type.Empty)
            {
                return;
            }

            descriptionManager.Register(type, Generate, GenerateAsync);
            descriptionManager.Register(type, localizedDescription);
        }

        protected virtual async Task<DescriptionManager.DescriptionResult> GenerateAsync(CancellationToken cancellationToken)
        {
            string description = await localizedDescription.GetLocalizedTextAsync(cancellationToken);
            return new DescriptionManager.DescriptionResult(true, description);
        }

        protected virtual bool Generate(out string description)
        {
            description = localizedDescription.GetLocalizedString();
            return true;
        }
    }
}
