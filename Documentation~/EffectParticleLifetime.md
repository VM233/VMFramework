# Effect particle lifetime

## Ownership and acceptance

`Effect` owns each native particle rental. Stopping emission is distinct from
returning the rental. Native ParticleSystem and VisualEffect children complete
together; only an empty aggregate can return. Direct GameItemManager.Return is
the explicit cancellation boundary, including pool clear and scene teardown.
Every observation is cancelled on that boundary, before another rental starts.

Native particle effects follow an attachment without becoming children of the
attachment's GameObject. Parent pool retirement, disable and destruction stop
emission and release the attachment while retaining the last world pose and
living particles. Sprite/audio effects retain physical parenting and their
authored playback completion. Their existing duration timers remain meaningful;
particle return uses native completion instead of a guessed duration.

## Static Cost Ledger (before executable writes)

The cache axis is the authored native components of one physical Effect:
P ParticleSystems, V VisualEffects and S total CPU VFX spawners. These are
composition counts, not elapsed frames or prior rentals. The current consumer
has three native effect prefabs, each P=0, V=1, S=1 (one particle system per
graph). The frozen native fixture permits P<=3, V<=1, S<=1, two rentals and one
attachment per case. Initialization performs one hierarchy read per component
kind and one VFX spawner-name read per VFX. Each completion observation visits
P+V+S entries, with at most P IsAlive, V HasAnySystemAwake/alive-count and S
GetSpawnSystemInfo calls. Stop and reset each visit P+V; no cross product,
asset scan, physics query, GPU readback request or history traversal is added.
One reusable native VFXSpawnerState per VFX is disposed on physical destruction.
One cancellation source and at most one attachment belong to each live rental;
observation allocates no per-frame collections. Attachment application is O(1)
and runs before native VFX simulation; completion observes LastPostLateUpdate.
Generic duration scheduling owns one timer per pending item and removes that
timer before retirement publication or cancellation. Its idle timer pool retains
only cleared timers, bounded by peak simultaneous scheduled items. Destruction
visits exactly the pending entries. Production admission remains authored input,
not an invented universal hard cap. For the frozen composition/fixture and
linear traversal, budget PASS; no wall-clock guarantee is claimed.

## Native configuration

Effects whose completion must continue offscreen require native simulation to
continue offscreen. Their VFX Graph culling policy must not suspend simulation;
ParticleSystem pause culling must likewise not freeze a live tail. Persistent
graph configuration belongs to the consumer's authored assets.

The old single-system particle callback and timer-based VFX recyclers, and the
VFX-only stop subscriber, are replaced by the aggregate owner. Remove their
components from prefabs when adopting this revision; retain the Effect root.
