# Runtime UI Toolkit roots

`UIToolkitPanel` owns the live `UIDocument` root. A logical panel can outlive a
root: native Live Reload, closing and destruction retire the published root and
its consumers. `GenerateVisualElement` creates a detached authoring preview and
does not publish a runtime root.

`OnRootVisualElementReady` publishes the actual root before layout starts. At
that event the new `LayoutCompletion` has not yet been assigned. Consumers bind
the published root and wait for `OnLayoutChangeEvent` before reading its actual
layout Task. The layout event runs within that Task, so the event itself does
not prove terminal completion. Await the original `LayoutCompletion` before
adopting a completed layout or releasing resources held by its continuation.

`OnRootVisualElementReleased` publishes a `UIToolkitRootRetirement` containing
the exact released root, its original failure and its actual layout Task.
Cancellation belongs to that root's `LayoutCancellationToken`. It does not
replace a foreign cancellation or a source failure. Root retirement joins the
accepted layout operation before releasing callbacks, pointer endpoints or
consumer resources; independent release failures retain their stage and owner.

Closing uses the configured close policy. `DisableDocument` disables the
document; `VisualElementDisplayNone` keeps it enabled and hides its existing
root. Reopening publishes the retained root without creating a second document.
Live Reload can replace a closed retained root, with the same close policy.
Native destruction publishes item liveness before its physical pool retirement.

`UIPanelManager` publishes a rental after acquisition succeeds. A failed
creation retires its actually acquired prefix, and never publishes an unfinished
panel. Panel modifiers and locale/pointer subscriptions belong to that rental
and root. A callback failure does not suppress other independent releases.

## Cost and verification

Each published root owns one cancellation source and one actual layout Task;
retirement joins that Task rather than adding a polling loop or a scene scan.
Pointer registration traverses the root's existing children once per lifetime.
Release fanout visits each acquired subscriber once. Its work is linear in the
actual root children and subscribed consumers, with no additional content axis.
Consumer-specific counts and native rendering budgets belong to the consuming
project's frozen authoring graph and cost ledger.

Use the package Editor test assembly for focused root creation, close/reopen,
Live Reload, retained document identity, pointer transfer and cancellation cases.
Exercise actual production consumers for layout adoption, input ownership,
locale changes and release failures. Compilation alone does not establish those
runtime behaviors.
