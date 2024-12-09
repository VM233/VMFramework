﻿using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.GameEvents
{
    public sealed partial class ColliderMouseEventGeneralSetting : GeneralSetting
    {
        private const string COLLIDER_MOUSE_EVENT_CATEGORY = "Collider Mouse Event";
        
        [TabGroup(TAB_GROUP_NAME, COLLIDER_MOUSE_EVENT_CATEGORY)]
        public ObjectDimensions dimensionsDetectPriority = ObjectDimensions.TWO_D;
        
        [TabGroup(TAB_GROUP_NAME, COLLIDER_MOUSE_EVENT_CATEGORY)]
        [Range(0, 100)]
        public float detectDistance2D = 30;
        
        [TabGroup(TAB_GROUP_NAME, COLLIDER_MOUSE_EVENT_CATEGORY)]
        [Range(0, 5000)]
        public float detectDistance3D = 1000;
        
        [TabGroup(TAB_GROUP_NAME, COLLIDER_MOUSE_EVENT_CATEGORY)]
        public LayerMask detectLayerMask = -1;
        
        [TabGroup(TAB_GROUP_NAME, COLLIDER_MOUSE_EVENT_CATEGORY)]
        public bool includingDefaultPhysicsScene3D = true;
        
        [TabGroup(TAB_GROUP_NAME, COLLIDER_MOUSE_EVENT_CATEGORY)]
        public bool includingDefaultPhysicsScene2D = true;
    }
}
