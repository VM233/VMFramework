# Manager singleton lifetime

`ManagerBehaviour<TInstance>` owns the sole `Instance` writer. `Awake` publishes the physical component after confirming its closed generic interface and rejecting any different existing owner. `OnDestroy` releases the reference only when `ReferenceEquals(Instance, this)`. Destroying a failed duplicate cannot clear another component's publication. The contract also covers interface-valued singletons, whose references do not use Unity's overloaded null equality.

The public setter is removed. Consumers create and retire the actual component through its lifecycle. Every derived retirement hook must be `protected override void OnDestroy()` and call `base.OnDestroy()` in `finally`, including when its own cleanup fails. Do not write `Instance = null`, replace a live owner for a test, retain a prior owner for later restoration, or call all-manager creation from a consumer to repair publication.

`ManagerCreator` retains its actual discovery and replacement flow: it removes an invalid base component before adopting the selected derived component. Native scene integration must prove that flow publishes DD2's `SurvivalGameItemManager`; the private tests do not invoke a full manager scan. The former report's short-circuit expression did not observe `ManagerTypes`, and the authored free-pool policy did not measure the base pool's capacity. The updated lifecycle report independently samples actual type, Unity-object destruction, the complete collection and configured capacity before rentals.

Subscriptions retain the source that accepted them. Localized and pointer managers capture the actual panel manager, update manager and LocalizationSettings instance. Their per-panel receipts survive removal of panel modifiers and retire the matching panel and locale callbacks. The localized manager's protected collection becomes a per-panel subscription dictionary; derived consumers that accessed its former HashSet must adopt this owner contract. `GameItemManagerEventsReceiver` captures its actual rental manager when `GameItemEvents` builds receivers at `BeforeInitStart`, after manager creation has completed all `Awake` publications. Later event removal uses that captured source.

`LogicTickTimerManager` captures its actual `ILogicTickManager` at `BeforeInitStart`.
The same clock supplies deadlines, stop notifications and the acquired tick
subscription. Retirement detaches that subscription before releasing timer-manager
publication. A manager that never initialized acquired no clock subscription.
Pending queue membership stays with that physical timer manager until its consumer
retires the registration. Consumers must retain the `ILogicTickTimerManager` that
accepted their timers, and use it for registration, rearming and cleanup; a later
`Instance` lookup cannot identify an old queue. Destroying or replacing the clock
publication does not replace the original clock's final tick.

The four `LogicTickTimerLifetime` cases in `VMFramework.Editor.Tests` exercise
pending stop after clock replacement, stop through a retained retired timer owner,
tick-subscription retirement and an owner that never initialized. They invoke the
actual manager hooks explicitly in Edit Mode. They establish queue and callback
contracts; an authorized consumer's native Play retirement and another admission
remain required to prove Unity's real lifetime.

FishNet UUIDCoreManager retains separate actual server and client event sources. Stopping either role removes only its acquired subscription; a host that did not acquire a client subscription has none to remove. Its Check, Query and Editor declarations merge into the main non-partial owner, with only the Editor method under `UNITY_EDITOR`. The main MonoScript GUID and all existing query/network behavior remain. DD2 has no `FISHNET` symbol, so this candidate establishes controlled static closure; native/network behavior remains unverified. No extra FishNet installation or another project's Editor is part of adoption.

`GameEventManager.Register(string)` owns the rental acquired from its captured `IGameItemManager` until unregister or retirement. Failed dictionary adoption returns that rental once. `Register(IGameEvent)` borrows an existing managed or pooled product: the caller retains its lifetime, and unregister or manager retirement does not return it. The framework initializer now uses the owning ID overload.

Unregister takes the original registration out before invoking consumers and returns only that registration's rental, including when notification fails. A callback may publish a new same-ID event without old cleanup deleting it. Manager retirement takes an immutable registration snapshot and empties membership before detaching dirty subscriptions and returning owned rentals; actual failures are preserved independently. Registration during this existing owned-retirement phase is rejected. Owned pool reset occurs after removal from the registry; a borrowed caller may reset its product after unregister. Game event callbacks and propagation ordering are unchanged.

Existing Reference, UI binding, LogicTick and tracing fixtures retain their cases and assertions. Their unit harness calls the actual `Awake` and `OnDestroy` methods explicitly and once; destroying an Edit Mode object that never ran native Awake is not proof of native OnDestroy. Existing tracing Play Mode tests and six new `ManagerSingletonLifetime` cases independently exercise actual Unity callbacks. The new test owners use an independent unmarked generic interface and inherit no production `ManagerCreationProvider` attribute, so exposing package testables cannot shadow DD2's real rental manager.

The six focused cases run in `VMFramework.PlayMode.Tests`: destroyed base-to-derived replacement, deferred destroy/recreate, distinct live same-class/derived-class rejection, failing derived cleanup followed by recreation, and an invalid closed generic candidate. Their finite envelope is 10 GameObjects, 11 MonoBehaviours, peak two of each, one actual deferred-destruction frame, no GameItem rental, no trace and no manager scan. The private cost ledger separately charges the 45 existing fixture cases and new resource-receipt loops. Official compilation, exact code-policy and dependency reviews, focused native tests and DD2's original 30 lifecycle checks remain required after adoption.

Other consuming projects retain their pins. MarbleBattlers and MarbleBattlers-Balance have a controlled `ElementRenderTextureManager.OnDestroy` setter use that needs migration when their own upgrade is authorized. This task changes only the authorized DD2 consumer. `NetworkManagerBehaviour<TInstance>` and `UniqueMonoBehaviour<T>` have independent singleton contracts; their writers are not rewritten by this family.

## Manager metadata preparation and CPU boundaries

DoomsdayDiary C30 first Play attributes81.162ms (80.964ms self,50 iterator advances) to the old synchronous AppDomain manager scan, versus1.167ms provider selection and0.766ms physical adoption. Its actual consecutive first6->7 interval118.63390000144136ms fails the unchanged100ms gate. Earlier13.262ms/91.705ms complete-creation witnesses remain; none proves JIT or another unmeasured cause.

Runtime and Editor initializers await ManagerCreator.CreateManagersAsync with their actual action token. InitializationTypeDiscovery.FindAllAsync returns the same ordered non-generic derived domain, including abstract classes and interfaces, through its existing assembly-local reflection cache and shared10ms work-credit owner. FindAsync retains its concrete-only contract; both entries use one traversal with an explicit private domain selector. There is no second reflection cache or ready state.

Preparation owns only metadata and one pending cancellable frame wait. Cancellation before or during preparation preserves all actual managers, type collections and containers and retains the exact source token. After the complete source joins, the caller checks cancellation and performs the existing setup, provider selection, invalid-base retirement and selected-component adoption synchronously once. No mutable Unity graph crosses an await. The user-invoked Editor viewer retains an explicitly Editor-only synchronous authoring command feeding the same private adoption method. It is not a runtime fallback.

The five permanent native stages identify actual setup, discovery dispatch, selection, retirement and adoption. Async assembly work has the existing per-contract/per-assembly discovery markers; the dispatch scope never crosses an await and must not be interpreted as total awaited discovery time. Candidate order, physical singleton callbacks and exception identity remain. Existing abstract/interface collection semantics are unchanged.

Static Cost Ledger before executable writes: the captured assembly/type domain and assembly-local queries reuse the existing discovery owner, with one work-credit check per original assembly. Frozen C30 returns49 manager candidates; the conservative256 concrete plus256 abstract/interface envelope gives N<=512, <=130816 original pairwise subtype comparisons, <=256KiB original transient selection storage and <16KiB added preparation list/task/token/slice. No added native creation, retirement, thread, cache, queue or timer. Each source owns at most one frame wait and publishes no physical resources until joined. Atomic assembly/native operations remain indivisible and require native first-Play100ms acceptance independently of work-credit scheduling.

InitializationWorkSliceTests adds ordered complete-domain equality and already-canceled/pending-canceled manager preparation to its original four cases. Cancellation controls capture <=4*512 borrowed manager/type references and <=16 container references (<32KiB), create no manager/native fixture and assert exact token plus unchanged publication. One pending control uses the existing20ms credit stimulus. Focused seven-case Play Mode tests and actual first-Play source/cancel integration qualify this change; unit metadata equality does not substitute for physical startup adoption.
