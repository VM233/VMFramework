// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Runtime.CompilerServices;
// using Sirenix.OdinInspector;
// using UnityEngine;
// using VMFramework.Core;
// using VMFramework.GameLogicArchitecture;
//
// namespace VMFramework.Containers
// {
//     public class ListContainer : Container, IListContainer
//     {
//         public override int Count
//         {
//             [MethodImpl(MethodImplOptions.AggressiveInlining)]
//             get => items?.Count ?? 0;
//         }
//
//         public override bool IsFull => false;
//
//         public override bool IsEmpty => ValidCount == 0;
//
//         [ShowInInspector]
//         private List<IContainerItem> items = new();
//
//         #region Get
//
//         public override bool TryGetItem(int index, out IContainerItem item)
//         {
//             if (index < 0 || index >= items.Count)
//             {
//                 item = null;
//                 return false;
//             }
//             
//
//             item = items[index];
//
//             return true;
//         }
//
//         public override IContainerItem GetItem(int index)
//         {
//             if (index < 0 || index >= items.Count)
//             {
//                 return null;
//             }
//
//             return items[index];
//         }
//
//         public override IContainerItem GetItemWithoutCheck(int index)
//         {
//             return items[index];
//         }
//
//         #endregion
//
//         #region Set
//
//         public override void SetItem(int index, IContainerItem item)
//         {
//             if (index < 0 || index >= items.Count)
//             {
//                 Debug.LogWarning($"设置物品 {item} 时index: {index} 越界");
//                 return;
//             }
//
//             var targetItem = item;
//
//             if (targetItem is { Count: <= 0 })
//             {
//                 targetItem = null;
//             }
//
//             var oldItem = items[index];
//
//             items[index] = targetItem;
//             
//             if (oldItem != null)
//             {
//                 OnItemRemoved(index, oldItem);
//             }
//
//             OnBeforeItemChanged(index, oldItem);
//
//             if (targetItem != null)
//             {
//                 OnItemAdded(index, targetItem);
//             }
//
//             OnAfterItemChanged(index, targetItem);
//
//             if (IsDebugging)
//             {
//                 if (targetItem == null)
//                 {
//                     Debug.LogWarning($"{this}的{index}被设置成了Null");
//                 }
//                 else
//                 {
//                     Debug.LogWarning($"{this}的{index}被设置成了{targetItem}");
//                 }
//             }
//         }
//
//         #endregion
//
//         #region Add
//
//         public override bool TryAddItem(IContainerItem item, int startIndex, int endIndex)
//         {
//             
//         }
//
//         #endregion
//
//         #region Pop
//
//         public override bool TryPopItemByPreferredCount(int preferredCount, out IContainerItem item,
//             out int slotIndex)
//         {
//             for (var i = items.Count - 1; i >= 0; i--)
//             {
//                 if (this.TryPopItemByPreferredCount(i, preferredCount, out item))
//                 {
//                     slotIndex = i;
//                     return true;
//                 }
//             }
//
//             item = null;
//             slotIndex = default;
//             return false;
//         }
//
//         #endregion
//
//         #region Sort
//
//         public override void Sort(Comparison<IContainerItem> comparison, int startIndex, int endIndex)
//         {
//             startIndex = startIndex.ClampMin(0);
//             endIndex = endIndex.ClampMax(Count - 1);
//             
//             this.StackItems(startIndex, endIndex);
//
//             var itemList = this.GetRangeItems(startIndex, endIndex).ToList();
//
//             itemList.RemoveAllNull();
//
//             itemList.Sort(comparison);
//
//             for (var i = 0; i < itemList.Count; i++)
//             {
//                 SetItem(startIndex + i, itemList[i]);
//             }
//
//             for (var i = endIndex; i >= startIndex + itemList.Count; i--)
//             {
//                 items.RemoveAt(i);
//             }
//
//             OnCountChanged();
//         }
//
//         #endregion
//
//         #region Compress
//
//         public override void Compress(int startIndex, int endIndex)
//         {
//             startIndex = startIndex.ClampMin(0);
//             endIndex = endIndex.ClampMax(Count - 1);
//             
//             var itemList = this.GetRangeItems(startIndex, endIndex).ToList();
//
//             itemList.RemoveAllNull();
//
//             for (var i = 0; i < itemList.Count; i++)
//             {
//                 SetItem(startIndex + i, itemList[i]);
//             }
//
//             for (var i = endIndex; i >= startIndex + itemList.Count; i--)
//             {
//                 items.RemoveAt(i);
//             }
//
//             OnCountChanged();
//         }
//
//         #endregion
//
//         #region Get All
//
//         public override IEnumerable<IContainerItem> GetAllItems()
//         {
//             return items;
//         }
//
//         #endregion
//
//         #region Expand
//
//         
//
//         #endregion
//
//         #region Array Operations
//
//         public override void LoadFromItemsArray<TItem>(TItem[] itemsArray)
//         {
//             for (var i = 0; i < items.Count; i++)
//             {
//                 SetItem(i, null);
//             }
//
//             items.Clear();
//             OnCountChanged();
//
//             for (var i = 0; i < itemsArray.Length; i++)
//             {
//                 items.Add(null);
//             }
//
//             OnCountChanged();
//
//             for (var i = 0; i < itemsArray.Length; i++)
//             {
//                 SetItem(i, itemsArray[i]);
//             }
//         }
//
//         public override void CopyAllItemsToArray<TItem>(TItem[] itemsArray)
//         {
//             for (var i = 0; i < items.Count; i++)
//             {
//                 itemsArray[i] = (TItem)items[i];
//             }
//         }
//
//         #endregion
//     }
// }