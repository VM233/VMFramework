﻿// using System;
// using Sirenix.OdinInspector;
// using System.Collections.Generic;
// using System.Runtime.CompilerServices;
// using System.Linq;
// using UnityEngine;
// using VMFramework.Core;
// using VMFramework.Core.Linq;
// using VMFramework.GameLogicArchitecture;
//
// namespace VMFramework.Containers
// {
//     public class GridContainer : Container, IGridContainer
//     {
//         protected IGridContainerConfig GridContainerConfig => (IGridContainerConfig)GamePrefab;
//
//         [ShowInInspector]
//         [ListDrawerSettings(NumberOfItemsPerPage = 5)]
//         private IContainerItem[] items;
//
//         public override int Count
//         {
//             [MethodImpl(MethodImplOptions.AggressiveInlining)]
//             get => items?.Length ?? 0;
//         }
//         
//         public override bool IsFull
//         {
//             [MethodImpl(MethodImplOptions.AggressiveInlining)]
//             get => ValidCount == Count;
//         }
//
//         public override bool IsEmpty => ValidCount == 0;
//
//         #region Pool Events
//
//         protected override void OnCreate()
//         {
//             base.OnCreate();
//
//             GridContainerConfig.Size.AssertIsAbove(0, $"{nameof(GridContainerConfig)}'s size");
//
//             items = new IContainerItem[GridContainerConfig.Size];
//         }
//
//         #endregion
//
//         #region Basic Operation
//
//         #region Get
//
//         [MethodImpl(MethodImplOptions.AggressiveInlining)]
//         public override bool TryGetItem(int index, out IContainerItem item)
//         {
//             if (index < 0 || index >= items.Length)
//             {
//                 item = null;
//                 return false;
//             }
//
//             item = items[index];
//
//             return true;
//         }
//
//         [MethodImpl(MethodImplOptions.AggressiveInlining)]
//         public override IContainerItem GetItem(int index)
//         {
//             if (index < 0 || index >= items.Length)
//             {
//                 return null;
//             }
//
//             return items[index];
//         }
//
//         [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
//             if (index < 0 || index >= items.Length)
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
//             if (oldItem != null)
//             {
//                 OnItemRemoved(index, oldItem);
//             }
//
//             OnBeforeItemChanged(index, oldItem);
//
//             items[index] = targetItem;
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
//         protected virtual bool PreTryAddItem(IContainerItem item, out IEnumerable<int> slotIndices,
//             out bool shouldEndAddingItem)
//         {
//             slotIndices = Enumerable.Empty<int>();
//             shouldEndAddingItem = false;
//             return false;
//         }
//
//         public override bool TryAddItem(IContainerItem item, int startIndex, int endIndex)
//         {
//             if (IsDebugging)
//             {
//                 Debug.LogWarning($"尝试添加物品 {item}");
//             }
//
//             if (item == null)
//             {
//                 return true;
//             }
//
//             if (item.Count <= 0)
//             {
//                 return true;
//             }
//
//             if (startIndex > endIndex)
//             {
//                 return false;
//             }
//
//             if (PreTryAddItem(item, out var preSlotIndices, out var shouldEndAddingItem))
//             {
//                 return true;
//             }
//
//             if (shouldEndAddingItem)
//             {
//                 return false;
//             }
//
//             startIndex = startIndex.ClampMin(0);
//             endIndex = endIndex.ClampMax(items.Length - 1);
//
//             for (var i = startIndex; i <= endIndex; i++)
//             {
//                 var itemInContainer = items[i];
//                 if (itemInContainer == null)
//                 {
//                     continue;
//                 }
//
//                 if (TryMergeItem(i, item))
//                 {
//                     if (item.Count <= 0)
//                     {
//                         return true;
//                     }
//                 }
//             }
//
//             for (var i = startIndex; i <= endIndex; i++)
//             {
//                 var itemInContainer = items[i];
//                 if (itemInContainer != null)
//                 {
//                     continue;
//                 }
//
//                 bool shouldReturn;
//
//                 if (item.Count <= item.MaxStackCount)
//                 {
//                     SetItem(i, item);
//
//                     shouldReturn = true;
//                 }
//                 else
//                 {
//                     var cloneItem = item.GetClone();
//                     cloneItem.Count = item.MaxStackCount;
//                     item.Count -= item.MaxStackCount;
//
//                     SetItem(i, cloneItem);
//
//                     shouldReturn = false;
//                 }
//
//                 if (shouldReturn)
//                 {
//                     return true;
//                 }
//             }
//
//             return false;
//         }
//
//         #endregion
//
//         #region Pop
//
//         #endregion
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
//             if (itemList.Count != 0)
//             {
//                 return;
//             }
//
//             itemList.Sort(comparison);
//
//             for (int i = 0; i < itemList.Count; i++)
//             {
//                 SetItem(startIndex + i, itemList[i]);
//             }
//
//             for (int i = startIndex + itemList.Count; i <= endIndex; i++)
//             {
//                 SetItem(i, null);
//             }
//         }
//
//         #endregion
//
//         #region Compress Items
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
//             if (itemList.Count == 0)
//             {
//                 return;
//             }
//
//             for (int i = 0; i < itemList.Count; i++)
//             {
//                 SetItem(startIndex + i, itemList[i]);
//             }
//
//             for (int i = startIndex + itemList.Count; i <= endIndex; i++)
//             {
//                 SetItem(i, null);
//             }
//         }
//
//         #endregion
//
//         #region Get Items
//
//         public override IEnumerable<IContainerItem> GetAllItems()
//         {
//             return items;
//         }
//
//         public Dictionary<int, IContainerItem> GetSomeItemsDictionary(IEnumerable<int> slotIndices)
//         {
//             var items = new Dictionary<int, IContainerItem>();
//
//             foreach (var slotIndex in slotIndices)
//             {
//                 var item = GetItem(slotIndex);
//
//                 if (item != null)
//                 {
//                     items[slotIndex] = item;
//                 }
//             }
//
//             return items;
//         }
//
//         #endregion
//
//         #region Array Operations
//
//         public override void LoadFromItemsArray<TItem>(TItem[] itemsArray)
//         {
//             for (int i = 0; i < itemsArray.Length; i++)
//             {
//                 SetItem(i, itemsArray[i]);
//             }
//
//             for (int i = itemsArray.Length; i < items.Length; i++)
//             {
//                 SetItem(i, null);
//             }
//         }
//
//         public override void CopyAllItemsToArray<TItem>(TItem[] itemsArray)
//         {
//             for (int i = 0; i < items.Length; i++)
//             {
//                 itemsArray[i] = (TItem)items[i];
//             }
//         }
//
//         #endregion
//
//         #region Resize
//
//         public void Resize(int newSize)
//         {
//             if (newSize == items.Length)
//             {
//                 return;
//             }
//
//             var newItems = new IContainerItem[newSize];
//
//             for (int i = 0; i < newSize && i < items.Length; i++)
//             {
//                 newItems[i] = items[i];
//             }
//
//             items = newItems;
//         }
//
//         #endregion
//
//         #region Debug
//
//         #region Local
//
//         [Button]
//         private void Shuffle()
//         {
//             var itemList = items.ToList();
//
//             itemList.Shuffle();
//
//             foreach (var (slotIndex, item) in itemList.Enumerate())
//             {
//                 SetItem(slotIndex, item);
//             }
//         }
//
//         #endregion
//
//         #endregion
//     }
// }