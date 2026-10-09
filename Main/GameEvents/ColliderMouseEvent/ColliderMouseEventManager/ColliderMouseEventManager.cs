using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Sirenix.OdinInspector;
using EnumsNET;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.Procedure;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace VMFramework.GameEvents
{
    [ManagerCreationProvider(ManagerType.EventCore)]
    public class ColliderMouseEventManager : ManagerBehaviour<ColliderMouseEventManager>
    {
        private const string DEBUGGING_CATEGORY = "Only For Debugging";

        private static ColliderMouseEventGeneralSetting Setting => CoreSetting.ColliderMouseEventGeneralSetting;

        [SerializeField]
        private Camera fixedBindCamera;

        [ShowInInspector]
        [HideInEditorMode]
        public Camera BindCamera { get; set; }

        [ShowInInspector]
        public float DetectDistance3D { get; set; }

        [ShowInInspector]
        public float DetectDistance2D { get; set; }

        [ShowInInspector]
        public ObjectDimensions DimensionsDetectPriority { get; set; }

        [ShowInInspector]
        public LayerMask LayerMask { get; set; }

        [ShowInInspector]
        public readonly List<PhysicsScene> physicsScene3Ds = new();

        [ShowInInspector]
        public readonly List<PhysicsScene2D> physicsScene2Ds = new();

        #region Triggers

        [FoldoutGroup(DEBUGGING_CATEGORY), ReadOnly, ShowInInspector]
        public ColliderMouseEventTrigger CurrentHoverTrigger { get; protected set; }

        [FoldoutGroup(DEBUGGING_CATEGORY), ReadOnly, ShowInInspector]
        private ColliderMouseEventTrigger lastHoverTrigger;

        [FoldoutGroup(DEBUGGING_CATEGORY), ReadOnly, ShowInInspector]
        private ColliderMouseEventTrigger leftMouseUpDownTrigger;

        [FoldoutGroup(DEBUGGING_CATEGORY), ReadOnly, ShowInInspector]
        private ColliderMouseEventTrigger rightMouseUpDownTrigger;

        [FoldoutGroup(DEBUGGING_CATEGORY), ReadOnly, ShowInInspector]
        private ColliderMouseEventTrigger middleMouseUpDownTrigger;

        [FoldoutGroup(DEBUGGING_CATEGORY), ReadOnly, ShowInInspector]
        private ColliderMouseEventTrigger dragTrigger;

        private readonly HashSet<ColliderMouseEventTrigger> lastStayTriggers = new();

        private readonly List<ColliderMouseEventTrigger> currentStayTriggers = new();

        private readonly HashSet<ColliderMouseEventTrigger> triggersToLeave = new();

        #endregion

        #region Init

        protected override void Awake()
        {
            base.Awake();

            physicsScene2Ds.Clear();
            physicsScene3Ds.Clear();

            CurrentHoverTrigger = null;
            lastHoverTrigger = null;
            leftMouseUpDownTrigger = null;
            rightMouseUpDownTrigger = null;
            middleMouseUpDownTrigger = null;
            dragTrigger = null;

            lastStayTriggers.Clear();
            currentStayTriggers.Clear();
            triggersToLeave.Clear();

            mouseEvents.Clear();
        }

        protected override void OnBeforeInitStart()
        {
            base.OnBeforeInitStart();

            if (fixedBindCamera != null)
            {
                BindCamera = fixedBindCamera;
            }
            else
            {
                BindCamera = Camera.main;
            }

            DetectDistance3D = Setting.detectDistance3D;

            DetectDistance2D = Setting.detectDistance2D;

            DimensionsDetectPriority = Setting.dimensionsDetectPriority;

            LayerMask = Setting.detectLayerMask;

            if (Setting.includingDefaultPhysicsScene3D)
            {
                physicsScene3Ds.Add(Physics.defaultPhysicsScene);
            }

            if (Setting.includingDefaultPhysicsScene2D)
            {
                physicsScene2Ds.Add(Physics2D.defaultPhysicsScene);
            }
        }

        #endregion

        private void Update()
        {
            if (BindCamera == null)
            {
                return;
            }

#if ENABLE_INPUT_SYSTEM
            bool leftDown = Mouse.current.leftButton.wasPressedThisFrame;
            bool leftUp = Mouse.current.leftButton.wasReleasedThisFrame;
            bool leftHeld = Mouse.current.leftButton.isPressed;
            bool rightDown = Mouse.current.rightButton.wasPressedThisFrame;
            bool rightUp = Mouse.current.rightButton.wasReleasedThisFrame;
            bool rightHeld = Mouse.current.rightButton.isPressed;
            bool middleDown = Mouse.current.middleButton.wasPressedThisFrame;
            bool middleUp = Mouse.current.middleButton.wasReleasedThisFrame;
            bool middleHeld = Mouse.current.middleButton.isPressed;
#else
            bool leftDown = Input.GetMouseButtonDown(0);
            bool leftUp = Input.GetMouseButtonUp(0);
            bool leftHeld = Input.GetMouseButton(0);
            bool rightDown = Input.GetMouseButtonDown(1);
            bool rightUp = Input.GetMouseButtonUp(1);
            bool rightHeld = Input.GetMouseButton(1);
            bool middleDown = Input.GetMouseButtonDown(2);
            bool middleUp = Input.GetMouseButtonUp(2);
            bool middleHeld = Input.GetMouseButton(2);
#endif

            currentStayTriggers.Clear();

            CurrentHoverTrigger = DetectTrigger(currentStayTriggers);

            var currentHoverTriggerIsNull = CurrentHoverTrigger == null;
            var lastHoverTriggerIsNull = lastHoverTrigger == null;

            #region Multiple Pointer Enter & Leave & Stay

            triggersToLeave.Clear();

            triggersToLeave.UnionWith(lastStayTriggers);
            triggersToLeave.ExceptWith(currentStayTriggers);

            foreach (var trigger in triggersToLeave)
            {
                Invoke(MouseEventType.PointerExitMultiple, trigger);
            }

            foreach (var trigger in currentStayTriggers)
            {
                if (lastStayTriggers.Contains(trigger) == false)
                {
                    Invoke(MouseEventType.PointerEnterMultiple, trigger);
                }
                else
                {
                    Invoke(MouseEventType.PointerStayMultiple, trigger);
                }
            }

            lastStayTriggers.Clear();
            lastStayTriggers.UnionWith(currentStayTriggers);

            #endregion

            #region Pointer Enter & Leave & Stay

            if (currentHoverTriggerIsNull)
            {
                // Pointer Leave
                if (lastHoverTriggerIsNull == false)
                {
                    Invoke(MouseEventType.PointerExit, lastHoverTrigger);
                }
            }
            else
            {
                // Pointer Leave & Enter
                if (CurrentHoverTrigger != lastHoverTrigger)
                {
                    if (lastHoverTriggerIsNull == false)
                    {
                        Invoke(MouseEventType.PointerExit, lastHoverTrigger);
                    }

                    Invoke(MouseEventType.PointerEnter, CurrentHoverTrigger);
                }

                // Pointer Hover
                Invoke(MouseEventType.PointerStay, CurrentHoverTrigger);
            }

            #endregion

            #region Left Mouse Button Up & Down

            if (leftMouseUpDownTrigger == null)
            {
                if (currentHoverTriggerIsNull == false)
                {
                    //Down
                    if (leftDown)
                    {
                        leftMouseUpDownTrigger = CurrentHoverTrigger;

                        Invoke(MouseEventType.LeftMouseButtonDown, leftMouseUpDownTrigger);
                        Invoke(MouseEventType.LeftMouseButtonStay, leftMouseUpDownTrigger);
                    }
                }
            }
            else
            {
                if (CurrentHoverTrigger == leftMouseUpDownTrigger)
                {
                    //Up & Click
                    if (leftUp)
                    {
                        Invoke(MouseEventType.LeftMouseButtonUp, leftMouseUpDownTrigger);
                        Invoke(MouseEventType.LeftMouseButtonClick, leftMouseUpDownTrigger);

                        leftMouseUpDownTrigger = null;
                    }
                    //Stay
                    else if (leftHeld)
                    {
                        Invoke(MouseEventType.LeftMouseButtonStay, leftMouseUpDownTrigger);
                    }
                }
                else
                {
                    //Up
                    if (leftUp)
                    {
                        Invoke(MouseEventType.LeftMouseButtonUp, leftMouseUpDownTrigger);

                        leftMouseUpDownTrigger = null;
                    }
                }
            }

            #endregion

            #region Right Mouse Button Up & Down

            if (rightMouseUpDownTrigger == null)
            {
                if (currentHoverTriggerIsNull == false)
                {
                    //Down
                    if (rightDown)
                    {
                        rightMouseUpDownTrigger = CurrentHoverTrigger;

                        Invoke(MouseEventType.RightMouseButtonDown, rightMouseUpDownTrigger);
                        Invoke(MouseEventType.RightMouseButtonStay, rightMouseUpDownTrigger);
                    }
                }
            }
            else
            {
                if (CurrentHoverTrigger == rightMouseUpDownTrigger)
                {
                    //Up & Click
                    if (rightUp)
                    {
                        Invoke(MouseEventType.RightMouseButtonUp, rightMouseUpDownTrigger);
                        Invoke(MouseEventType.RightMouseButtonClick, rightMouseUpDownTrigger);

                        rightMouseUpDownTrigger = null;
                    }
                    //Stay
                    else if (rightHeld)
                    {
                        Invoke(MouseEventType.RightMouseButtonStay, rightMouseUpDownTrigger);
                    }
                }
                else
                {
                    //Up
                    if (rightUp)
                    {
                        Invoke(MouseEventType.RightMouseButtonUp, rightMouseUpDownTrigger);

                        rightMouseUpDownTrigger = null;
                    }
                }
            }

            #endregion

            #region Middle Mouse Button Up & Down

            if (middleMouseUpDownTrigger == null)
            {
                if (currentHoverTriggerIsNull == false)
                {
                    //Down
                    if (middleDown)
                    {
                        middleMouseUpDownTrigger = CurrentHoverTrigger;

                        Invoke(MouseEventType.MiddleMouseButtonDown, middleMouseUpDownTrigger);
                        Invoke(MouseEventType.MiddleMouseButtonStay, middleMouseUpDownTrigger);
                    }
                }
            }
            else
            {
                if (CurrentHoverTrigger == middleMouseUpDownTrigger)
                {
                    //Up & Click
                    if (middleUp)
                    {
                        Invoke(MouseEventType.MiddleMouseButtonUp, middleMouseUpDownTrigger);
                        Invoke(MouseEventType.MiddleMouseButtonClick, middleMouseUpDownTrigger);

                        middleMouseUpDownTrigger = null;
                    }
                    //Stay
                    else if (middleHeld)
                    {
                        Invoke(MouseEventType.MiddleMouseButtonStay, middleMouseUpDownTrigger);
                    }
                }
                else
                {
                    //Up
                    if (middleUp)
                    {
                        Invoke(MouseEventType.MiddleMouseButtonUp, middleMouseUpDownTrigger);

                        middleMouseUpDownTrigger = null;
                    }
                }
            }

            #endregion

            #region Any Mouse Button Up & Down

            if (currentHoverTriggerIsNull == false)
            {
                //Down
                if (leftDown || rightDown || middleDown)
                {
                    Invoke(MouseEventType.AnyMouseButtonDown, CurrentHoverTrigger);
                }

                //Up
                if (leftUp || rightUp || middleUp)
                {
                    Invoke(MouseEventType.AnyMouseButtonUp, CurrentHoverTrigger);
                }

                //Stay
                if (leftHeld || rightHeld || middleHeld)
                {
                    Invoke(MouseEventType.AnyMouseButtonStay, CurrentHoverTrigger);
                }
            }

            #endregion

            #region Drag Begin & Stay & End

            if (dragTrigger == null)
            {
                // Drag Begin
                if (currentHoverTriggerIsNull == false && CurrentHoverTrigger.draggable)
                {
                    var triggerDrag = false;

                    foreach (var mouseType in CurrentHoverTrigger.dragButton.GetFlags())
                    {
                        if (mouseType == MouseButtonType.LeftButton && leftHeld)
                        {
                            triggerDrag = true;
                            break;
                        }

                        if (mouseType == MouseButtonType.RightButton && rightHeld)
                        {
                            triggerDrag = true;
                            break;
                        }

                        if (mouseType == MouseButtonType.MiddleButton && middleHeld)
                        {
                            triggerDrag = true;
                            break;
                        }
                    }

                    if (triggerDrag)
                    {
                        dragTrigger = CurrentHoverTrigger;

                        Invoke(MouseEventType.DragBegin, dragTrigger);
                    }
                }
            }
            else
            {
                var keepDragging = false;

                foreach (var mouseType in dragTrigger.dragButton.GetFlags())
                {
                    if (mouseType == MouseButtonType.LeftButton && leftHeld)
                    {
                        keepDragging = true;
                        break;
                    }

                    if (mouseType == MouseButtonType.RightButton && rightHeld)
                    {
                        keepDragging = true;
                        break;
                    }

                    if (mouseType == MouseButtonType.MiddleButton && middleHeld)
                    {
                        keepDragging = true;
                        break;
                    }
                }

                if (keepDragging)
                {
                    Invoke(MouseEventType.DragStay, dragTrigger);
                }
                else
                {
                    Invoke(MouseEventType.DragEnd, dragTrigger);

                    dragTrigger = null;
                }
            }

            #endregion

            lastHoverTrigger = CurrentHoverTrigger;
        }

        private ColliderMouseEventTrigger DetectTrigger(List<ColliderMouseEventTrigger> triggers)
        {
            if (DimensionsDetectPriority == ObjectDimensions.TWO_D)
            {
                var detected2D = Detect2DTrigger(triggers);
                if (detected2D != null)
                {
                    return detected2D;
                }

                return Detect3DTrigger();
            }

            var detected3D = Detect3DTrigger();
            if (detected3D != null)
            {
                return detected3D;
            }

            return Detect2DTrigger(triggers);
        }

        private ColliderMouseEventTrigger Detect3DTrigger()
        {
#if ENABLE_INPUT_SYSTEM
            Vector3 mousePos = Mouse.current.position.ReadValue();
#else
            Vector3 mousePos = Input.mousePosition;
#endif

            if (mousePos.IsInfinity() || mousePos.IsNaN())
            {
                return null;
            }

            var ray = BindCamera.ScreenPointToRay(mousePos);

            Debug.DrawRay(ray.origin, ray.direction, Color.green);

            foreach (var physicsScene in physicsScene3Ds)
            {
                if (physicsScene.Raycast(ray.origin, ray.direction, out var hit3D, DetectDistance3D, LayerMask))
                {
                    var detectResult =
                        hit3D.collider.gameObject.GetComponent<ColliderMouseEventTrigger>();

                    return detectResult;
                }
            }

            return null;
        }

        private readonly List<Collider2D> overlapResults = new();
        private readonly SortedList<int, ColliderMouseEventTrigger> triggerSorted = new();

        private ColliderMouseEventTrigger Detect2DTrigger(List<ColliderMouseEventTrigger> triggers)
        {
#if ENABLE_INPUT_SYSTEM
            Vector3 mousePos = Mouse.current.position.ReadValue();
#else
            Vector3 mousePos = Input.mousePosition;
#endif

            if (mousePos.IsInfinity())
            {
                return null;
            }

            triggerSorted.Clear();

            var contactFilter = new ContactFilter2D()
            {
                useTriggers = true,
                useLayerMask = true,
                layerMask = LayerMask
            };

            var point = BindCamera.ScreenToWorldPoint(mousePos);

            foreach (var physicsScene in physicsScene2Ds)
            {
                int count = physicsScene.OverlapPoint(point.XY(), contactFilter, overlapResults);

                for (int i = 0; i < count; i++)
                {
                    var collider = overlapResults[i];

                    if (collider.TryGetComponent(out ColliderMouseEventTrigger trigger) == false)
                    {
                        continue;
                    }

                    if (trigger.enabled == false)
                    {
                        continue;
                    }

                    if (trigger.MatchesPoint2D(point.XY()) == false)
                    {
                        continue;
                    }

                    triggerSorted.TryAdd(-trigger.priority, trigger);
                    triggers.Add(trigger);
                }
            }

            if (triggerSorted.Count > 0)
            {
                return triggerSorted.Values[0];
            }

            return null;
        }

        [ShowInInspector]
        private static readonly Dictionary<MouseEventType, ColliderMouseEvent> mouseEvents = new();

        [Button]
        private static void Invoke(MouseEventType eventType, ColliderMouseEventTrigger trigger)
        {
            trigger.TriggerEvent(eventType);

            if (mouseEvents.TryGetValue(eventType, out var mouseEvent) == false)
            {
                return;
            }

            mouseEvent.Propagate(trigger);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddCallback(MouseEventType eventType, Action<ColliderMouseEventTrigger> callback,
            int priority = PriorityDefines.TINY)
        {
            if (mouseEvents.TryGetValue(eventType, out var gameEvent) == false)
            {
                gameEvent = GameItemManager.Instance.Get<ColliderMouseEvent>(ColliderMouseEventConfig.ID);
                mouseEvents.Add(eventType, gameEvent);
            }

            gameEvent.AddCallback(callback, priority);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RemoveCallback(MouseEventType eventType, Action<ColliderMouseEventTrigger> callback)
        {
            if (mouseEvents.TryGetValue(eventType, out var gameEvent) == false)
            {
                UnityEngine.Debug.LogWarning($"{nameof(ColliderMouseEvent)} for {eventType} not found.");
                return;
            }

            gameEvent.RemoveCallback(callback);
        }
    }
}
