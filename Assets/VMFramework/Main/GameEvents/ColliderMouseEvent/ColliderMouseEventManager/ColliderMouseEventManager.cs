using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using EnumsNET;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.Procedure;

namespace VMFramework.GameEvents
{
    [ManagerCreationProvider(ManagerType.EventCore)]
    public sealed partial class ColliderMouseEventManager : ManagerBehaviour<ColliderMouseEventManager>
    {
        private const string DEBUGGING_CATEGORY = "Only For Debugging";

        private static ColliderMouseEventGeneralSetting Setting => CoreSetting.ColliderMouseEventGeneralSetting;

        [SerializeField]
        private Camera fixedBindCamera;

        [ShowInInspector]
        [HideInEditorMode]
        public static Camera BindCamera { get; set; }

        [ShowInInspector]
        public static float DetectDistance3D { get; set; }

        [ShowInInspector]
        public static float DetectDistance2D { get; set; }

        [ShowInInspector]
        public static ObjectDimensions DimensionsDetectPriority { get; set; }

        [ShowInInspector]
        public static LayerMask LayerMask { get; set; }

        [ShowInInspector]
        public static readonly List<PhysicsScene> physicsScene3Ds = new();

        [ShowInInspector]
        public static readonly List<PhysicsScene2D> physicsScene2Ds = new();

        #region Triggers

        [FoldoutGroup(DEBUGGING_CATEGORY), ReadOnly, ShowInInspector]
        private static ColliderMouseEventTrigger currentHoverTrigger;

        [FoldoutGroup(DEBUGGING_CATEGORY), ReadOnly, ShowInInspector]
        private static ColliderMouseEventTrigger lastHoverTrigger;

        [FoldoutGroup(DEBUGGING_CATEGORY), ReadOnly, ShowInInspector]
        private static ColliderMouseEventTrigger leftMouseUpDownTrigger;

        [FoldoutGroup(DEBUGGING_CATEGORY), ReadOnly, ShowInInspector]
        private static ColliderMouseEventTrigger rightMouseUpDownTrigger;

        [FoldoutGroup(DEBUGGING_CATEGORY), ReadOnly, ShowInInspector]
        private static ColliderMouseEventTrigger middleMouseUpDownTrigger;

        [FoldoutGroup(DEBUGGING_CATEGORY), ReadOnly, ShowInInspector]
        private static ColliderMouseEventTrigger dragTrigger;

        #endregion

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

        private void Update()
        {
            if (BindCamera == null)
            {
                return;
            }

            currentHoverTrigger = DetectTrigger();

            var currentHoverTriggerIsNull = currentHoverTrigger == null;
            var lastHoverTriggerIsNull = lastHoverTrigger == null;

            #region Pointer Enter & Leave & Hover

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
                if (currentHoverTrigger != lastHoverTrigger)
                {
                    if (lastHoverTriggerIsNull == false)
                    {
                        Invoke(MouseEventType.PointerExit, lastHoverTrigger);
                    }

                    Invoke(MouseEventType.PointerEnter, currentHoverTrigger);
                }

                // Pointer Hover
                Invoke(MouseEventType.PointerHover, currentHoverTrigger);
            }

            #endregion

            #region Left Mouse Button Up & Down

            if (leftMouseUpDownTrigger == null)
            {
                if (currentHoverTriggerIsNull == false)
                {
                    //Down
                    if (Input.GetMouseButtonDown(0))
                    {
                        leftMouseUpDownTrigger = currentHoverTrigger;

                        Invoke(MouseEventType.LeftMouseButtonDown, leftMouseUpDownTrigger);
                        Invoke(MouseEventType.LeftMouseButtonStay, leftMouseUpDownTrigger);
                    }
                }
            }
            else
            {
                if (currentHoverTrigger == leftMouseUpDownTrigger)
                {
                    //Up & Click
                    if (Input.GetMouseButtonUp(0))
                    {
                        Invoke(MouseEventType.LeftMouseButtonUp, leftMouseUpDownTrigger);
                        Invoke(MouseEventType.LeftMouseButtonClick, leftMouseUpDownTrigger);

                        leftMouseUpDownTrigger = null;
                    }
                    //Stay
                    else if (Input.GetMouseButton(0))
                    {
                        Invoke(MouseEventType.LeftMouseButtonStay, leftMouseUpDownTrigger);
                    }
                }
                else
                {
                    //Up
                    if (Input.GetMouseButtonUp(0))
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
                    if (Input.GetMouseButtonDown(1))
                    {
                        rightMouseUpDownTrigger = currentHoverTrigger;

                        Invoke(MouseEventType.RightMouseButtonDown, rightMouseUpDownTrigger);
                        Invoke(MouseEventType.RightMouseButtonStay, rightMouseUpDownTrigger);
                    }
                }
            }
            else
            {
                if (currentHoverTrigger == rightMouseUpDownTrigger)
                {
                    //Up & Click
                    if (Input.GetMouseButtonUp(1))
                    {
                        Invoke(MouseEventType.RightMouseButtonUp, rightMouseUpDownTrigger);
                        Invoke(MouseEventType.RightMouseButtonClick, rightMouseUpDownTrigger);

                        rightMouseUpDownTrigger = null;
                    }
                    //Stay
                    else if (Input.GetMouseButton(1))
                    {
                        Invoke(MouseEventType.RightMouseButtonStay, rightMouseUpDownTrigger);
                    }
                }
                else
                {
                    //Up
                    if (Input.GetMouseButtonUp(1))
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
                    if (Input.GetMouseButtonDown(2))
                    {
                        middleMouseUpDownTrigger = currentHoverTrigger;

                        Invoke(MouseEventType.MiddleMouseButtonDown, middleMouseUpDownTrigger);
                        Invoke(MouseEventType.MiddleMouseButtonStay, middleMouseUpDownTrigger);
                    }
                }
            }
            else
            {
                if (currentHoverTrigger == middleMouseUpDownTrigger)
                {
                    //Up & Click
                    if (Input.GetMouseButtonUp(2))
                    {
                        Invoke(MouseEventType.MiddleMouseButtonUp, middleMouseUpDownTrigger);
                        Invoke(MouseEventType.MiddleMouseButtonClick, middleMouseUpDownTrigger);

                        middleMouseUpDownTrigger = null;
                    }
                    //Stay
                    else if (Input.GetMouseButton(2))
                    {
                        Invoke(MouseEventType.MiddleMouseButtonStay, middleMouseUpDownTrigger);
                    }
                }
                else
                {
                    //Up
                    if (Input.GetMouseButtonUp(2))
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
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
                {
                    Invoke(MouseEventType.AnyMouseButtonDown, currentHoverTrigger);
                }

                //Up
                if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2))
                {
                    Invoke(MouseEventType.AnyMouseButtonUp, currentHoverTrigger);
                }

                //Stay
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButtonUp(2))
                {
                    Invoke(MouseEventType.AnyMouseButtonStay, currentHoverTrigger);
                }
            }

            #endregion

            #region Drag Begin & Stay & End

            if (dragTrigger == null)
            {
                // Drag Begin
                if (currentHoverTriggerIsNull == false && currentHoverTrigger.draggable)
                {
                    var triggerDrag = false;

                    foreach (var mouseType in currentHoverTrigger.dragButton.GetFlags())
                    {
                        if (mouseType == MouseButtonType.LeftButton && Input.GetMouseButton(0))
                        {
                            triggerDrag = true;
                            break;
                        }

                        if (mouseType == MouseButtonType.RightButton && Input.GetMouseButton(1))
                        {
                            triggerDrag = true;
                            break;
                        }

                        if (mouseType == MouseButtonType.MiddleButton && Input.GetMouseButton(2))
                        {
                            triggerDrag = true;
                            break;
                        }
                    }

                    if (triggerDrag)
                    {
                        dragTrigger = currentHoverTrigger;

                        Invoke(MouseEventType.DragBegin, dragTrigger);
                    }
                }
            }
            else
            {
                var keepDragging = false;

                foreach (var mouseType in dragTrigger.dragButton.GetFlags())
                {
                    if (mouseType == MouseButtonType.LeftButton && Input.GetMouseButton(0))
                    {
                        keepDragging = true;
                        break;
                    }

                    if (mouseType == MouseButtonType.RightButton && Input.GetMouseButton(1))
                    {
                        keepDragging = true;
                        break;
                    }

                    if (mouseType == MouseButtonType.MiddleButton && Input.GetMouseButton(2))
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

            lastHoverTrigger = currentHoverTrigger;
        }

        private ColliderMouseEventTrigger DetectTrigger()
        {
            if (DimensionsDetectPriority == ObjectDimensions.TWO_D)
            {
                ColliderMouseEventTrigger detected2D = Detect2DTrigger();
                if (detected2D != null)
                {
                    return detected2D;
                }

                return Detect3DTrigger();
            }

            ColliderMouseEventTrigger detected3D = Detect3DTrigger();
            if (detected3D != null)
            {
                return detected3D;
            }

            return Detect2DTrigger();
        }

        private static ColliderMouseEventTrigger Detect3DTrigger()
        {
            Vector3 mousePos = Input.mousePosition;

            if (mousePos.IsInfinity() || mousePos.IsNaN())
            {
                return default;
            }

            var ray = BindCamera.ScreenPointToRay(mousePos);

            Debug.DrawRay(ray.origin, ray.direction, Color.green);

            foreach (var physicsScene in physicsScene3Ds)
            {
                if (physicsScene.Raycast(ray.origin, ray.direction, out var hit3D, DetectDistance3D, LayerMask))
                {
                    ColliderMouseEventTrigger detectResult =
                        hit3D.collider.gameObject.GetComponent<ColliderMouseEventTrigger>();

                    return detectResult;
                }
            }

            return default;
        }

        private static readonly Vector2[] hitDirections =
        {
            Vector2.left,
            Vector2.right,
            Vector2.down,
            Vector2.up
        };
        
        private static ColliderMouseEventTrigger Detect2DTrigger()
        {
            Vector3 mousePos = Input.mousePosition;

            if (mousePos.IsInfinity())
            {
                return default;
            }

            ColliderMouseEventTrigger resultTrigger = null;

            Ray ray = BindCamera.ScreenPointToRay(mousePos);

            float distance = -1;

            foreach (var physicsScene in physicsScene2Ds)
            {
                foreach (Vector2 direction in hitDirections)
                {
                    var origin = new Vector2(ray.origin.x, ray.origin.y);
                    RaycastHit2D newHit = physicsScene.Raycast(origin, direction, DetectDistance2D, LayerMask);

                    if (newHit == false)
                    {
                        continue;
                    }
                    
                    var pivotPosition = newHit.collider.transform.position;

                    if (Vector2.Dot(newHit.normal, (pivotPosition.XY() - newHit.point)) < 0)
                    {
                        continue;
                    }

                    if (newHit.collider.TryGetComponent(out ColliderMouseEventTrigger trigger) == false)
                    {
                        continue;
                    }

                    Vector2 colliderPos = BindCamera.WorldToScreenPoint(pivotPosition).XY();

                    float newDistance = Vector2.Distance(colliderPos, Input.mousePosition.XY());

                    if (newDistance < distance || distance < 0)
                    {
                        distance = newDistance;
                        resultTrigger = trigger;
                    }
                }
            }

            if (distance >= 0)
            {
                return resultTrigger;
            }

            return default;
        }
    }
}
