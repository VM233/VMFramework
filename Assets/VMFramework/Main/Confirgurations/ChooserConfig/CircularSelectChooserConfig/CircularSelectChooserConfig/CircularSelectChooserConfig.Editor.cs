#if UNITY_EDITOR
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.Core.Linq;

namespace VMFramework.Configuration
{
    public partial class CircularSelectChooserConfig<TWrapper, TItem>
    {
        #region Category

        protected const string CIRCULAR_ACTIONS_CATEGORY = "CircularActions";

        #endregion
        
        private bool ShowPingPongOption => items is { Count: > 2 };

        #region Add Item GUI

        private CircularSelectItemConfig<TItem> AddItemGUI()
        {
            CircularSelectItemConfig<TItem> item = new()
            {
                index = items.Count,
                times = 1,
                value = default,
            };

            return item;
        }

        #endregion

        #region Circular Actions

        [Button]
        [ButtonGroup(CIRCULAR_ACTIONS_CATEGORY)]
        private void ShiftUp()
        {
            items.Rotate(-1);
            
            OnItemsChangedGUI();
        }

        [Button]
        [ButtonGroup(CIRCULAR_ACTIONS_CATEGORY)]
        private void ShiftDown()
        {
            items.Rotate(1);
            
            OnItemsChangedGUI();
        }

        [Button]
        [ButtonGroup(CIRCULAR_ACTIONS_CATEGORY)]
        private void Shuffle()
        {
            items.Shuffle();
            
            OnItemsChangedGUI();
        }

        [Button]
        [ButtonGroup(CIRCULAR_ACTIONS_CATEGORY)]
        private void ResetCircularTimes()
        {
            foreach (var item in items)
            {
                item.times = 1;
            }
        }

        #endregion

        #region On Items Changed GUI

        protected void OnItemsChangedGUI()
        {
            foreach (var (index, item) in items.Enumerate())
            {
                if (item == null)
                {
                    continue;
                }
                
                item.index = index;
            }

            if (chooser != null)
            {
                chooser = GenerateThisChooser();
            }
        }

        #endregion
    }
}
#endif