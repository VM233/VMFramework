# Game event propagation

The event's registered callbacks are the membership authority. Each call to
ParameterlessGameEvent.Propagate, either ParameterizedGameEvent.Propagate
overload, and ColliderMouseEventTrigger.TriggerEvent owns its own snapshot
lease. GameEventManager forwards to the same event entry points. Input system
and collider game events inherit the parameterized implementation.

The producer copies membership completely before invoking user callbacks.
Numeric priority stays ascending; parameterized callbacks precede parameterless
callbacks at each priority. propagateAction=false excludes parameterless
callbacks. The existing order within a parameterized HashSet is unchanged.
Parameterless priority-group delegates remain immutable products of
PriorityEvents, with its existing composition and invalidation rules.

The invocation acquires a list from the single-thread Default pool, clears it
before adoption, and releases its references and returns the lease in finally.
A nested invocation has a distinct active lease. Callback membership changes
affect subsequent invocations, including nested ones, while the current call
finishes its saved snapshot. No instance retains a returned list. The collider
snapshot follows the same lifetime even though its old instance list was never
returned to a pool. Neither class nor any audited derived consumer needs that
old protected scratch field.

The collider component's Editor Reset hook remains under UNITY_EDITOR in the
same owned declaration. MouseEventHandler retains its existing public name and
signature in its own source file. The readonly membership table is created by
the component and cannot be absent; event-type lookup still owns empty-type
admission and idempotent removal.

Exceptions propagate unchanged and stop later callbacks. A failed game-event
invocation does not call OnPropagationStopped; each normally completed call
calls it once, after releasing its snapshot. Admission and event-enable
semantics remain at their existing owners. No additional retry, cadence,
deferred publication, gameplay ordering or pool implementation is introduced.

## Static Cost Ledger

Before executable writes: N callback registrations, P nonempty priority groups
(P <= N), and D simultaneous nested invocations are the input axes. The existing
membership copy and invocation cost is O(N) per call, with O(D*N) live snapshot
references. Parameterless copies P combined delegates, parameterized copies N
callback pairs, and collider copies the matching event-type registrations.
List growth is amortized and clear/release is linear in the saved snapshot.
PriorityEvents' existing lazy composition cost remains documented separately.
Callbacks themselves own their work and recursion; this dispatcher adds no
recursion or loops over ticks, actors, history, frames or assets.

The focused fixture has 14 cases, at most four registrations, two active event
invocations, two repeated dispatches before a pool lease check, and at most six
recorded callbacks per assertion. Pool isolation acquires at most the existing
Default pool capacity of 500 idle lists per relevant closed type, then restores
every acquired lease. At most three cases use isolation, for at most 1,500
baseline get/return pairs plus 15 dispatch and external lease pairs, or 1,515
get and 1,515 return operations; the other cases do not enumerate the pool.
Additional live snapshot arrays hold at most eight callback pairs (128 bytes
on a 64-bit runtime), plus one 500-reference holding array (4 KiB), assertions,
small record lists and fixture objects. Budget: 64 KiB additional live fixture
and dispatch storage, excluding preexisting pool contents and test-runner
infrastructure; pass. Tests run synchronously on the owning Editor thread.
The snapshot cache owner is the established Default pool, bounded at 500 idle
lists per closed type; no instance cache, changed membership cache or native
call is added. Dynamic production membership/depth is not falsely advertised
as a hard gameplay cap or a wall-time guarantee.

## Validation

GameEventPropagationTests exercises independent simultaneous pool leases,
nested propagation, outer snapshot membership, nested arguments, both action
admission choices, priority order, original callback exceptions, completion
hooks, and collider reentry across event types. Adopt the immutable published
revision in the authorized consumer, compile/reload, review the changed C# and
package ownership, then run only this fixture through its declared test assembly.
Calibration performance witnesses still require independent native regression;
this ownership defect alone does not establish a particular stall's CPU cause.
