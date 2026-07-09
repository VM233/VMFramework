using System;
using Sirenix.OdinInspector;
using VMFramework.OdinExtensions;

namespace VMFramework.Configuration
{
    [HideDuplicateReferenceBox]
    [HideReferenceObjectPicker]
    [Serializable]
    public class GamePrefabIDConfig : IChooserWrapper<string>
    {
        [HideLabel]
        [GamePrefabID]
        [IsNotNullOrEmpty]
        public string id;

        public override string ToString() => id;

        string IChooserWrapper<string>.UnboxWrapper() => id;
    }
}