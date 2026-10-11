# State cloning and native initialization

`GetClone(source, context)` delegates to GameItemManager's clone producer. The
producer rents through the existing native pool, copies source state with the
caller-provided StateCloneContext, then publishes OnGameItemCreated. Until its
method returns, it owns that rental. Copy, type, cloner or creation-notification
failure returns the unpublished rental once. Ordinary Get and typed Get own the
same notification/type prefix. A successful retirement rethrows the original
exception; a failed retirement preserves both original errors in PoolItemAcquisitionException,
including the exact pool, IPoolItem and retirement phase.

VMCore's native pool owns earlier prefixes that have not returned a rental to
GameItemManager: interrupted fresh OnCreate is cleared without retention;
failed pooled OnGet returns the exact dequeued object through existing capacity.
GameItemMaterialization still restores its initialization context in finally.
No caller compensates for a rental it never received, and no global rental journal
or alternative materialization pipeline participates.

PrewarmUntil keeps all acquired rentals in its existing local list before notifying
creation. Every list member is independently returned even if a later rental,
notification or return fails. Its list is cleared and returned after the attempts.
Acquisition or notification failure with successful cleanup retains original exception identity.
A failed member return carries that member's exact pool/object/phase in PoolItemAcquisitionException;
the batch combines all failures in order without presenting one rental as the whole batch. These errors do not claim that failed child cleanup completed.
The normal creation/return notification order and existing pool capacities remain.
An unpublished individual Get/Clone rollback uses the native pool directly, as the
existing Clone rollback did; it does not fabricate a normal manager return notification.

GameItemManager.CurrentInitializationKind is available only inside native
OnCreate/OnGet initialization. Ordinary Get, typed Get and PrewarmUntil declare
AuthoredDefaults; Clone declares ClonedState. Nested native rentals restore the
parent context even on exceptions. No context is active after native initialization.
Initialization remains synchronous and uses the pool's existing thread boundary.

Owned child providers that will copy their state can use ClonedState to leave
the child empty until their state-clone receiver installs the source clone.
Providers excluded from state copying must still initialize authored defaults.
Reference providers keep their independent ownership semantics. Do not query
the initialization kind outside native initialization or use it as a gameplay mode.

Cloning retains the original StateCloneContext tags and native owner adoption.
It does not prewarm, postpone copying, reuse the source object or create a second pool.

GameItemReferenceManager owns one initialized reference per ID for its lifetime.
Initialization callbacks run before cache publication. Controller references are
then made inactive as complete roots, which removes all their native physics and
gameplay updates while preserving live data and identity. Managed references
retain their normal data lifetime. Callback failure returns the unpublished
rental and propagates the original error without publishing a cache entry.
Ordinary Get and Clone rentals remain separate objects with their own activation;
reference inactivity is not copied into their pool activation state.

## Cleanup and native lifetime

Native fixture components belong to the runtime test assembly, even when their
witness runs in Edit Mode. Unity does not attach an Editor-only script to a
GameObject. The Editor test assembly references that runtime fixture assembly;
empty native test subclasses should reuse the existing runtime component.
Fixtures that observe native Unity callbacks during Edit Mode also declare
ExecuteAlways. Destruction assertions consume the engine-delivered OnDestroy;
they never call that callback manually. Production components retain their
runtime execution contract.
Standalone test fixtures own manager publication and retire it during teardown,
including when a witness throws before its expected terminal state.

Controller lease retirement publishes IsDestroyed before its callbacks. It
snapshots individual ReturnEvents invocations in priority and registration order,
independently attempts all acquired component cleanup and root OnReturn, and
returns the sole callback buffer in finally. Components release only resources
they actually hold; there is no successful-Get shadow registry.

Controller and plain GameItem explicit Clear distinguish the actual existing
IsDestroyed fact. An active/failed fresh acquisition first retires its lease;
already-retired storage only invokes root OnClear. Clear independently attempts
storage cleanup even if the active lease cleanup failed. A queue Clear or capacity
overflow does not run ReturnEvents or OnDestructed twice. The plain explicit
Return entry owns the same liveness publication even when a derived root throws
before base.OnReturn.

OnDestructed and manager Destroyed observers are attempted independently. Return
deactivation is still attempted after a notification failure. Manager Get,
Get<T>, Clone rollback, ordinary Return and Prewarm use the same Core Return
producer: actual lease cleanup, qualified IsDestroyed notification, then storage
retention. A Created observer/cast failure can have already reached another
observer; the real retirement event withdraws that actual adopted prefix. A
root Return error does not suppress the liveness notification. Notification
failure prevents enqueue and retires storage; it is not a successful notification.

Created observers borrow a product still owned by the rental publisher. They do
not gain authority to Return that same borrowed rental before its Get completes.
Other independent Get/Return work is allowed. Destroyed reentry Get of the same ID
cannot dequeue the object still being published; later listeners see the old
destroyed lease. Current queue capacity is read after this callback, so a newly
returned distinct rental cannot overflow retention capacity. No pairing journal,
pending mode, callback retry, or fake metric-success event is introduced.

Single root cleanup failure is rethrown as the original exception. Independent
failures preserve ordered original objects in GameItemCleanupException or the
Core actual-pool/item/phase product. Prewarm preserves each failed member's
identity separately; an entire batch is not attributed to one item. None of
these products asserts native resources released.

Controller OnDestroy is the sole native IsDestroyed writer. Derived native hooks
invoke it before cancellation can synchronously resume Return continuations.
UI owns its held registration/root and actual Layout Task; only a fully returned
GameItemManager.Get product can be adopted by the UI registration producer.
Return/Clone reject a destroyed source before another pool operation. Deactivation,
root Clear completion and deferred Unity Destroy are separate facts. External
destruction of a free retained pool object is not certified by the focused active-
rental native-destruction test.

Existing conditional FISHNET members remain in the principal Controller/GameItem
source files, preserving public signatures and principal script GUIDs. The actual
UUID manager Destroyed consumer receives this same liveness publication; no second
network or trace cleanup authority is created.
