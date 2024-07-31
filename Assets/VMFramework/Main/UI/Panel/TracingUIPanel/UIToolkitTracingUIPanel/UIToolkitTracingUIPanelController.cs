﻿using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;
using VMFramework.Core;

namespace VMFramework.UI
{
    public partial class UIToolkitTracingUIPanelController : UIToolkitPanelController, IUIToolkitTracingUIPanel
    {
        protected UIToolkitTracingUIPanelPreset tracingUIPanelPreset { get; private set; }

        protected Vector2 defaultPivot;

        protected bool enableOverflow, autoPivotCorrection;

        [ShowInInspector]
        protected VisualElement tooltipContainer;

        protected override void OnPreInit(UIPanelPreset preset)
        {
            base.OnPreInit(preset);

            tracingUIPanelPreset = preset as UIToolkitTracingUIPanelPreset;

            tracingUIPanelPreset.AssertIsNotNull(nameof(tracingUIPanelPreset));
        }

        protected override void OnInit(UIPanelPreset preset)
        {
            base.OnInit(preset);

            defaultPivot = tracingUIPanelPreset.defaultPivot;
            enableOverflow = tracingUIPanelPreset.enableScreenOverflow;
            autoPivotCorrection = tracingUIPanelPreset.autoPivotCorrection;
        }

        protected override void OnOpenInstantly(IUIPanelController source)
        {
            base.OnOpenInstantly(source);

            tooltipContainer =
                UIDocument.rootVisualElement.Q(tracingUIPanelPreset.containerVisualElementName);

            if (tracingUIPanelPreset.enableAutoMouseTracing)
            {
                TracingUIManager.StartTracing(this, tracingUIPanelPreset.persistentTracing);
            }
        }

        protected override void OnCloseInstantly(IUIPanelController source)
        {
            base.OnCloseInstantly(source);

            tooltipContainer = null;

            TracingUIManager.StopTracing(this);
        }

        #region IUIToolkitTracingUIPanel

        Vector2 IUIToolkitTracingUIPanel.referenceResolution =>
            UIDocument.panelSettings.referenceResolution;

        VisualElement IUIToolkitTracingUIPanel.tracingContainer => tooltipContainer;

        bool IUIToolkitTracingUIPanel.enableOverflow => enableOverflow;

        bool IUIToolkitTracingUIPanel.autoPivotCorrection => autoPivotCorrection;

        Vector2 IUIToolkitTracingUIPanel.defaultPivot => defaultPivot;

        bool IUIToolkitTracingUIPanel.useRightPosition =>
            tracingUIPanelPreset.useRightPosition;

        bool IUIToolkitTracingUIPanel.useTopPosition =>
            tracingUIPanelPreset.useTopPosition;

        #endregion
    }
}