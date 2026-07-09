#if UNITY_EDITOR && ODIN_INSPECTOR
using System.Linq;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.Core.Editor;
using VMFramework.Core.Pools;

namespace VMFramework.OdinExtensions
{
    internal sealed class ColorChooserConfigContextMenuDrawer<T> : OdinValueDrawer<T>, IDefinesGenericMenuItems
        where T : IWrapperChooserConfig<Color, Color>
    {
        private const float COLOR_ALPHA_THRESHOLD = 0.3f;
        
        protected override void DrawPropertyLayout(GUIContent label)
        {
            CallNextDrawer(label);
        }

        void IDefinesGenericMenuItems.PopulateGenericMenu(InspectorProperty property, GenericMenu genericMenu)
        {
            var chooser = ValueEntry.SmartValue;

            if (chooser == null)
            {
                return;
            }

            var colors = ListPool<Color>.Default.Get();
            colors.Clear();
            chooser.GetAvailableItems(colors);

            if (colors.Any(color => color.a < COLOR_ALPHA_THRESHOLD))
            {
                genericMenu.AddItem("Set Alpha to 1", SetAlphaTo1);
            }

            if (colors.Any(color => color.a < 1))
            {
                genericMenu.AddItem("Set All Alpha to 1", SetAllAlphaTo1);
            }
            
            colors.ReturnToDefaultPool();

            return;

            void SetAlphaTo1()
            {
                chooser.SetAvailableValues(color =>
                {
                    if (color.a < COLOR_ALPHA_THRESHOLD)
                    {
                        return color.ReplaceAlpha(1);
                    }
                    
                    return color;
                });
            }

            void SetAllAlphaTo1()
            {
                chooser.SetAvailableValues(color => color.ReplaceAlpha(1));
            }
        }
    }
}
#endif