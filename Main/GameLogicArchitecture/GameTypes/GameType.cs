// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Runtime.CompilerServices;
// using VMFramework.Core;
// using Sirenix.OdinInspector;
// using UnityEngine;
// using VMFramework.Localization;
// using VMFramework.OdinExtensions;
//
// namespace VMFramework.GameLogicArchitecture
// {
//     [HideReferenceObjectPicker]
//     [HideDuplicateReferenceBox]
//     [PreviewComposite]
//     public partial class GameType : ITreeNode<GameType>, ICloneable, INameOwner
//     {
//         public const string ALL_ID = "all";
//
//         public static GameType Root
//         {
//             [MethodImpl(MethodImplOptions.AggressiveInlining)]
//             get
//             {
//                 _root ??= new(ALL_ID);
//                 return _root;
//             }
//         }
//         
//         [HideInInspector]
//         private static GameType _root = new(ALL_ID);
//
//         private static readonly Dictionary<string, GameType> allTypesDict = new();
//
//         [LabelText("ID")]
//         public readonly string id;
//
//         [LabelText("名称")]
//         private IReadOnlyLocalizedStringReference _name;
//
//         public string Name
//         {
//             get
//             {
//                 if (_name == null)
//                 {
//                     return id;
//                 }
//
//                 return _name.ToString();
//             }
//         }
//         
//         public IReadOnlyDictionary<string, GameType> Subtypes => subtypes;
//
//         [LabelText("子种类")]
//         [ShowInInspector]
//         private readonly Dictionary<string, GameType> subtypes = new();
//
//         [LabelText("父种类")]
//         public GameType Parent { get; init; }
//
//         public bool IsLeaf => Subtypes.Count == 0;
//         
//         public bool IsRoot => Parent == null || id == ALL_ID;
//
//         #region Constructor
//
//         public GameType(string id)
//         {
//             if (CheckID(id) == false)
//             {
//                 throw new ArgumentException($"无效ID:{id}");
//             }
//
//             this.id = id;
//         }
//
//         #endregion
//
//         #region IUniversalTree
//
//         public IEnumerable<GameType> GetChildren() => Subtypes.Values;
//
//         public GameType GetParent() => Parent;
//
//         public bool DirectEquals(GameType rhs) => id == rhs.id;
//
//         #endregion
//
//         #region Clear
//
//         public static void Clear()
//         {
//             allTypesDict.Clear();
//             _root.subtypes.Clear();
//         }
//
//         #endregion
//
//         public GameType AddSubtype(string subtypeID, IReadOnlyLocalizedStringReference name)
//         {
//             if (allTypesDict.ContainsKey(subtypeID))
//             {
//                 Debug.LogWarning($"种类ID:{subtypeID}重复添加");
//             }
//
//             var subtype = new GameType(subtypeID)
//             {
//                 Parent = this,
//                 _name = name
//             };
//
//             subtypes[subtypeID] = subtype;
//
//             allTypesDict[subtypeID] = subtype;
//
//             return subtype;
//         }
//
//         public GameType GetChildGameType(string typeID)
//         {
//             return this.PreorderTraverse(true)
//                 .FirstOrDefault(gameType => DirectEquals(gameType, typeID));
//         }
//
//         public bool BelongTo(GameType rhs)
//         {
//             return this.HasParent(rhs, true);
//         }
//
//         public bool BelongTo(string rhsID)
//         {
//             return this.HasParent(rhsID, DirectEquals, true);
//         }
//
//         #region Clone
//
//         public object Clone()
//         {
//             return this;
//         }
//
//         #endregion
//
//         #region To String
//
//         public override string ToString()
//         {
//             if (_name == null)
//             {
//                 return id;
//             }
//             
//             return _name.ToString();
//         }
//
//         #endregion
//
//         #region Utils
//
//         private static bool CheckID(string id)
//         {
//             if (string.IsNullOrEmpty(id))
//             {
//                 return false;
//             }
//
//             if (_root == null)
//             {
//                 return true;
//             }
//             
//             return id != ALL_ID;
//         }
//
//         public static bool DirectEquals(GameType type, string rhsID)
//         {
//             return type.id == rhsID;
//         }
//
//         #endregion
//
//         #region Create
//
//         public static GameType CreateSubroot(string subrootID, IReadOnlyLocalizedStringReference name)
//         {
//             return Root.AddSubtype(subrootID, name);
//         }
//
//         public static void Create(string newID, IReadOnlyLocalizedStringReference name, string parentID)
//         {
//             if (parentID == ALL_ID)
//             {
//                 Debug.LogWarning($"请使用CreateSubroot来创建次根种类");
//             }
//
//             GameType parentType = Root.GetChildGameType(parentID);
//
//             if (parentType == null)
//             {
//                 throw new ArgumentException($"创建id为{newID}时，" +
//                                             $"发现规定parentID为{parentID}的GameType不存在");
//             }
//
//             parentType.AddSubtype(newID, name);
//         }
//
//         #endregion
//
//         #region Operators
//
//         public static implicit operator GameType(string id)
//         {
//             return Root.GetChildGameType(id);
//         }
//
//         public static implicit operator string(GameType gameType)
//         {
//             return gameType.id;
//         }
//
//         #endregion
//     }
// }
