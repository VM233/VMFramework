# Runtime UI Toolkit roots

UIToolkitPanel owns the live UIDocument root. Its root lifetime is shorter than
the logical panel lifetime: Unity Live Reload disables companion components on
the document GameObject, recreates the root, and enables the companions again.
The child PanelModifiers are not reinitialized by this native transition.

OnRootVisualElementReady publishes the actual document root before the next
visible frame. OnRootVisualElementReleased retires that root before replacement,
closure or destruction. Runtime consumers subscribe to both events, bind once
per published root, and release callbacks and external subscriptions for that
same root. RootVisualElement is null outside an active root lifetime.

IVisualElementGenerator remains the generation/authoring contract. An Inspector
preview generated through GenerateVisualElement is detached and never changes
RootVisualElement or publishes a runtime root event.

The panel also moves its existing pointer registrations and language stylesheet
to the new root. Closing cancels the root's layout continuation. Closed panels
retain their configured DisableDocument or DisplayNone behavior after reload.

## Static Cost Ledger

Root publication/retirement runs on the Unity main thread and adds no polling,
scene scan or per-frame work. Each transition owns one cancellation source and
one existing layout continuation. Old continuations are canceled and disposed;
there is at most one pending continuation per panel. Pointer rebinding traverses
the same root children already used by AddPointerEvent/RemovePointerEvent, once
per lifetime, with no retained copy or extra iteration axis.

The current authorized consumer has seven authored documents, one top-level
content child per document, at most two runtime modifiers per panel, and the
existing finite pools of eight markers, three operations, four status bars,
sixteen body regions and sixty-four journal rows. Reload therefore adds at most
four pointer callback operations, two release and two bind notifications per
document. Each consumer performs its existing bounded bind/render once, keeping
the established UI budget of 5 ms per event and 512 KiB reusable data. Peak new
storage is one cancellation source per active document, below 8 KiB across all
seven documents; old subscriptions are removed before new ones are admitted.
There is no Cartesian product or background thread. Verdict: pass.

## Validation

Use the package Editor test assembly for focused root-lifetime cases: native
companion disable/recreate/enable sequencing, detached Inspector previews,
close/reopen, closed DisplayNone reload, pointer callback transfer and canceled
layout publication. Verify the real consumer in Simulator with its production
session, locale and operation paths; compilation alone does not prove binding.
