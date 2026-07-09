using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class PreDefinedBindObjectsNames : PanelModifier, IBindObjectsNamesProvider
    {
        [TitleGroup(ComponentNames.CONFIG)]
        public List<string> names = new();

        [TitleGroup(ComponentNames.CONFIG)]
        [BindObjectsName]
        public List<string> singleModeNames = new();

        [TitleGroup(ComponentNames.CONFIG)]
        public bool useGameObjectNames = true;

        public virtual void GetBindObjectsNames(ICollection<string> names, ICollection<string> singleModeNames)
        {
            names.AddRange(this.names);
            singleModeNames.AddRange(this.singleModeNames);

            if (useGameObjectNames)
            {
                foreach (var child in transform.GetAllChildren(includingSelf: false))
                {
                    var resultName = child.gameObject.name.ToPascalCase(" ");
                    names.Add(resultName);

                    if (child.gameObject.CompareTag("SingleMode"))
                    {
                        singleModeNames.Add(resultName);
                    }
                }
            }
        }
    }
}