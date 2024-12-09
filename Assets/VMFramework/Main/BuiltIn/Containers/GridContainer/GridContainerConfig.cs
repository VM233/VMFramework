// using System;
// using Newtonsoft.Json;
// using Sirenix.OdinInspector;
//
// namespace VMFramework.Containers
// {
//     public class GridContainerConfig : ContainerConfig, IGridContainerConfig
//     {
//         public override Type GameItemType => typeof(GridContainer);
//
//         [TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY)]
//         [MinValue(1)]
//         [JsonProperty]
//         public int size = 9;
//
//         protected int maxSlotIndex => size - 1;
//
//         int IGridContainerConfig.Size => size;
//     }
// }