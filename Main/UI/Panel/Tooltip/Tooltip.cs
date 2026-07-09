using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class Tooltip : PanelModifier, ITooltip
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [BindObjectsName]
        [IsNotNullOrEmpty]
        public string bindObjectsName;

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        protected TooltipOpenInfo CurrentOpenInfo { get; private set; }

        public bool Open(object target, IUIPanel source, TooltipOpenInfo info)
        {
            if (Panel.BindObjectsManager.ContainsObject(bindObjectsName, target))
            {
                return false;
            }

            if (Panel.BindObjectsManager.GetObjects(bindObjectsName).Count > 0)
            {
                if (info.priority < CurrentOpenInfo.priority)
                {
                    return false;
                }
            }

            CurrentOpenInfo = info;

            Panel.Open(source);

            Panel.BindObjectsManager.AddObject(bindObjectsName, target);

            return true;
        }

        public bool Close(object target)
        {
            if (Panel.BindObjectsManager.ContainsObject(bindObjectsName, target))
            {
                Panel.Close();
                return true;
            }

            return false;
        }
    }
}