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
to the new root. Closing cancels the root's layout continuation.

The configured close mode applies from creation, including before the first
open. DisableDocument retires the document when closed. VisualElementDisplayNone
keeps the configured document enabled and its root hidden for the panel's
lifetime. Opening publishes that retained root; closing releases consumer
bindings and hides it without recreating the document or its runtime panel.
Live Reload can still replace the hidden root, and the same close policy applies
to its replacement. Destroying the panel retires the document with its owner.

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

Creation and close policy add no iteration axis, polling, field or duplicate
tree. Each transition applies one document state. Retained documents already
belong to the existing DisplayNone policy; establishing that lifetime at
creation does not increase its existing all-open peak. The MarbleBattlers
consumer has two such unique documents: Battle Defeat (12 authored visual
nodes) and Debug Screen (31). Their native trees have the same UXML and built-in
control descendants as before. Reopening constructs no document or runtime
panel. The focused retention fixture has one document, one pointer target,
three open/close cycles, six pointer stimuli and two yielded frames. There are
three root publications and three retirements, with at most one canceled layout
continuation per cycle and no new retained storage. Verdict: pass.

## Validation

Use the package Editor test assembly for focused root-lifetime cases: native
companion disable/recreate/enable sequencing, detached Inspector previews,
close/reopen, closed DisplayNone creation/reload, retained document and native
panel identity, pointer callback transfer and canceled layout publication.
Fixtures initialize through the actual panel creation producer. Verify the
real consumer through its production session, locale and operation paths;
compilation alone does not prove binding.
