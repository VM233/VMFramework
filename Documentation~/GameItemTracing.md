# Game item tracing lifetime

`GameItemTransformTracingManager.Add` publishes one binding per target Transform:
the position source supplies its rendered position and the owner supplies its
pool lifetime. The two forward indexes and the two reverse group indexes have
the same membership. Rebinding first retires the old binding. Returning either
role, removing the target, or destroying the manager retires both indexes and
the last group's subscription and collection lease. A single item may fill both
roles, and either return order is valid.

`GameItemTracingManager` owns the equivalent whole-item relationship and its
offset, defined as target position minus source position at binding. The first
render update preserves the published target position; subsequent source
displacement produces the same target displacement. Removing its last target
also detaches the source subscription. Manager
destruction releases remaining subscriptions and collection leases in both
implementations; no binding survives into a subsequent manager lifetime.

These are render-frame consumers. They retain their existing Update clock,
public entry points and pooling authority. No gameplay
timer, warmup, simulation cadence or performance threshold is changed.

## Static Cost Ledger

Before executable writes: each manager has one input axis, N currently admitted
bindings. Source and owner group counts are each <= N; they are indexes of that
same axis, not independent Cartesian axes. Add/remove uses expected constant
dictionary/set work; removing a role visits exactly its k <= N bindings; Update
visits N bindings. Destruction visits at most 2N group entries and clears at most
2N binding references. There is no loop over prior rentals, ticks, assets or
frames. Correct retirement removes the old unbounded-history membership defect.
No new collection, cache, thread or native synchronization call is added.
Collection leases retain the existing Default pool's 500-idle-collection bound
per closed type; references are cleared before lease return.

The focused fixtures freeze at 13 Edit Mode and two Play Mode cases, at most four bindings, three
items, four target Transforms and two managers per case. Its repeated-rental
case runs eight add/return cycles (16 group acquisitions/returns), never more
than one simultaneous binding. Other cases perform at most four admissions and
four removals, and six render updates. Total admissions <= 64, removals <= 64,
render updates <= 96, native GameObjects <= 144 across the fixtures; no physics
or asset scan runs. Added fixture live storage budget is 64 KiB excluding Unity
object/test-runner storage; added production scratch allocation is zero. PASS
for this frozen fixture and the unchanged linear production traversal. Dynamic
gameplay admission remains the caller's input domain, not a claimed hard cap or
wall-clock guarantee. Each Play Mode case creates two consecutive manager
lifetimes with the same three participants, observes four rendered frames,
and destroys each manager through Unity. Its additional bound is six native
GameObjects and two binding admissions per case, with no asset or physics scan.

## Validation

`GameItemTracingLifetimeTests` and `GameItemTracingPlayModeTests` use the production managers, ControllerGameItem
pool-return callbacks and native Transforms. It checks visible motion, retiring
subscriptions, role order, selective shared-group removal, replacing bindings,
the same item in both roles, eight rental cycles and actual Play Mode manager teardown/recreation. Package
adoption, compilation and exact changed-source policy review remain separate.
The calibration stall's original native trace remains a counterexample; this
lifetime repair alone does not establish its complete CPU cause.
