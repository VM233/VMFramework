# Authoring asset changes

The Game Editor records native assets from its published menu graph, including
the graph's visual objects. Imports, moves and deletions invalidate a window
only when they affect those sources or an enclosing native asset folder. The
existing windows are reused. Common Preset membership is a separate project
settings input; its key/reference snapshot is compared on project changes.
Configured root imports additionally resolve their declared path through the
root-settings owner. A root can therefore return after removal even when the
preceding menu's native source set is empty for that root.

The wrapper registration owner records actual wrapper, setting and provider
paths. The native importer classifies live assets by their actual Unity type
and provider interfaces, and removed paths through that preceding publication.
Unrelated Mesh assets do not rebuild the registry or menu. Real changes retain
one paired deferred batch; entering Play or reloading cancels queued authoring
work. Returning to Edit uses the normal complete Editor initialization.

A refresh retires the preceding configurations' ID subscriptions before
rebuilding the registry. Source associations use configuration identity, so
renaming an ID cannot leave a stale source key. Removed configurations retain
no callback that could re-register them. Missing provider references are pruned
by this registration owner even when no Game Editor is open.

`GamePrefabAuthoringLifecycleTests` exercises native import, repeated refresh
and rename, replaced configuration retirement, unrelated Mesh import/deletion,
native move/folder deletion and actual source invalidation in an already-open
Game Editor. It owns unique temporary assets and waits for actual native import
publications. The host tests never create or focus an Editor window.
The root-return control temporarily moves the clean, Assets-owned single-module
framework root and restores its exact original path, GUID, bytes, meta and
registered ID set. Failed restoration preserves its directory instead of deleting it.
Its cancellation control invokes the real owner hook after a native import and
before deferred publication; it does not claim an actual Play transition.
The observer callback declares RunAfterAssembly("VMFramework");
GetPostprocessOrder does not order OnPostprocessAllAssets.
