# Changelog

All notable changes to this package are documented here.

## [Unreleased]

## [9.1.73] - 2026-10-29

### Fixed

- Retire Editor initialization in the earliest BuildPlayerProcessor preparation
  callback, before Addressables preparation and Player script work. A later
  preprocess-report callback cannot cover a long Addressables preparation phase.
  Verify the actual early callback and its ordering in the focused lifecycle test.

## [9.1.72] - 2026-10-29

### Fixed

- Keep the build-boundary pending fixture initializer in its own source file,
  satisfying the one-top-level-type policy without changing production behavior.

## [9.1.71] - 2026-10-29

### Fixed

- Cancel and join asynchronous Editor initialization at the native Player-build
  boundary, then schedule the ordinary initializer after the build retires.
  Failed builds use the same scheduler. Keep the existing timeout and prevent
  concurrent authoring work during build or prior-initializer retirement.

## [9.1.70] - 2026-10-28

### Fixed

- Clear pooled panels with immediate native destruction in Edit Mode and deferred
  destruction in Play Mode. Verify physical Edit Mode retirement together with
  exactly-once external subscription release.

## [9.1.69] - 2026-10-26

### Fixed

- Adopt the compiled event dependency product before event registration and
  retain it until subscriptions retire. Panel token release can propagate after
  the global settings provider has been destroyed during native shutdown.
- Exercise panel close and deinitialization with both event-manager retirement
  orders after the settings provider retires.

## [9.1.68] - 2026-10-26

### Documentation

- Keep native panel-count budgets in the consuming project and account for
  distinct sorting groups from the default PanelSettings factory.

## [9.1.67] - 2026-10-26

### Fixed

- Explicitly execute native panel retirement in the standalone Edit Mode
  presented-rental fixture, where an unawakened MonoBehaviour receives no
  automatic OnDestroy callback. Retain the production lifecycle assertions.

## [9.1.66] - 2026-10-26

### Fixed

- Publish the complete presented panel set, including nonunique rentals and
  closing animations, for native window input consumers. Closed registrations
  do not remain in that query.
- Keep the UIDocument viewport root non-pickable; authored descendants continue
  to own interactive areas. Reconstructed roots adopt the same contract.

## [9.1.65] - 2026-10-25

### Fixed

- Exercise both tooltip-manager/panel native retirement orders in Edit Mode.
  Do not call the Play Mode-only deferred destruction path from an Editor fixture.

## [9.1.64] - 2026-10-25

### Fixed

- Preserve the original first-registered live tooltip selection when a retired
  panel is replaced within a tooltip group.

## [9.1.63] - 2026-10-25

### Fixed

- Tooltip registrations retain their actual panel identity and unregister after
  modifier deinitialization without consulting the now-empty modifier collection.
  Remove empty groups and retire subscriptions on their original panel manager.
- Cover native panel destruction and pooled retirement before later tooltip
  operations, preventing stale tooltip callbacks during Profile teardown.

## [9.1.62] - 2026-10-24

### Fixed

- The pending-manager cancellation fixture initializes the actual current-frame
  work credit before consuming it. Preserve its original pending/token/publication
  assertions and avoid accidentally creating an unauthored graph in the test scene.
- Runtime manager preparation is unchanged from9.1.61.

## [9.1.61] - 2026-10-24

### Fixed

- Runtime and Editor initialization join cancellable assembly-budgeted manager
  metadata discovery before any physical manager mutation. Preserve complete
  type order, abstract/interface products and atomic singleton adoption.
- Keep the explicit Editor authoring command on the same adoption owner. Cover
  complete discovery order and cancellation before/pending preparation.

## [9.1.60] - 2026-10-24

### Added

- Native manager-creation stages distinguish actual setup, discovery iterator
  advancement, provider selection, invalid-base retirement and component adoption.
  Preserve candidate order, iterator retirement and physical singleton behavior.
- Document the13.262ms/91.705ms first-entry witnesses without attributing their
  difference to JIT or another unmeasured owner.

## [9.1.59] - 2026-10-24

### Fixed

- Prepare setting-file loaders through the existing cancellable incremental
  type-discovery source before admitting file loads. Publish the complete
  loader catalog once and reuse existing actual instances.
- Retain the prior published catalog on cancellation or constructor failure.
  Cover successful reuse, pre-cancellation, cancellation before publication
  and exact constructor failure through focused native Editor fixtures.

## [9.1.58] - 2026-10-24

### Added

- Native synchronous global-setting load stages distinguish actual dispatch,
  file/loader discovery, loader selection and the selected loader call. Scopes
  end before awaits and preserve the existing tasks, order and failure policy.
- Document the native cold-loading witness and constant observation cost;
  these stages do not claim a responsiveness or Player performance pass.

## [9.1.57] - 2026-10-24

### Fixed

- UI Toolkit panel creation preserves the prefab-copied visual-tree identity.
  The native rebuilding setter now runs only when the configured tree actually
  differs, so creating an unchanged closed document does not clone its UXML.

## [9.1.56] - 2026-10-24

### Fixed

- A started source transition now restores its withdrawn original source after
  exit-loading, native exit or exit-publication failure. Previously only failures
  after target loading restored the source. Admission failures and owner
  cancellation/retirement retain their existing ownership semantics.

## [9.1.55] - 2026-10-24

### Fixed

- Panel-manager open membership sets now belong directly to their retaining
  manager instead of being borrowed indefinitely from a global pool.
- Procedure close snapshots return their exact borrowed list when panel close
  throws, while preserving the actual source failure.

## [9.1.54] - 2026-10-24

### Added

- Native procedure transition samples separate request admission, synchronous
  loading invocation, state entry/exit and entry/exit publication. Scopes end
  before asynchronous waits and preserve the original transition lifecycle.

## [9.1.53] - 2026-10-24

### Fixed

- Initializer regression setup owns a compiler-generated iterator while joining
  its actual initial-credit task. This supports Unity Test Framework's setup
  program-counter lifecycle and cancels the owned wait on setup retirement.

## [9.1.52] - 2026-10-24

### Fixed

- Initialization action admission shares the existing frame work credit across
  orders without serializing asynchronous siblings. Cancellation during admission
  joins every accepted source before publishing a terminal result.
- Event registration and sprite preload check that same credit between actual
  units. Edit Mode uses actual Editor update epochs and retires its clock hooks
  before assembly reload or quitting.

### Added

- Actual Editor-frame admission, cancellation-while-admitting and source-failure
  lifecycle regressions alongside the existing ordering and timeout controls.

## [9.1.51] - 2026-10-20

### Fixed

- Configured root paths publish Unity asset separators at their source, so
  native imports match the same path on Windows and returning roots rebuild
  their existing hosts. Generic filesystem path combination is unchanged.

### Added

- Native path identity coverage across all current configured root assets.

## [9.1.50] - 2026-10-20

### Fixed

- A configured settings root that returns after native removal invalidates
  existing Game Editor hosts through its actual configured path, including
  when the preceding menu graph no longer contains that root.
- Native lifecycle observations run after the framework's post-import callbacks
  using Unity's callback dependency contract. Cancellation asserts that actual
  registration has not yet published at its interception boundary.

### Added

- Native configured-root move/return coverage with original path, GUID, bytes
  and meta preservation, clean single-module source admission and paired
  restoration on failure, reload and quit.

## [9.1.49] - 2026-10-20

### Fixed

- The authoring lifecycle tests reuse an existing Game Editor without the object
  ID API removed in Unity6000.6. Test ownership is established before waiting for
  initialization, so an admission failure still has a valid cleanup identity.

## [9.1.48] - 2026-10-20

### Fixed

- Native authoring imports invalidate the actual wrapper/settings/provider and
  menu source graph. Unrelated Mesh assets no longer rebuild all registrations
  and open Game Editor trees. Native moves/deletions and Common Preset membership
  still update their real consumers.
- Wrapper refresh pairs ID subscriptions with retirement and associates sources
  with configuration identity. Repeated refresh, rename and removal cannot leave
  duplicate callbacks or stale ID keys. Provider cleanup belongs to registration,
  independent of whether a Game Editor window is open.
- Queued native authoring work retires on Play admission and assembly reload.

### Added

- Native authoring lifecycle regression tests and focused ownership guidance.

## [9.1.47] - 2026-10-20

### Changed

- InitializationActionStatus and InitializationActionExecution have their own
  source files, preserving their existing names, state transitions and API
  while satisfying the one-top-level-type source contract.

## [9.1.46] - 2026-10-20

### Changed

- Counted work belongs to an explicit InitializationAction.progressSource.
  Validation alone publishes the GamePrefab work batch; other actions of the
  same initializer cannot expose a previous batch during a later invocation.

## [9.1.45] - 2026-10-20

### Changed

- GamePrefab progress measures the validation batch only, starting before
  check-processor discovery. Registration remains an uncounted prerequisite;
  consumers never see its completed count reused as unfinished validation.

## [9.1.44] - 2026-10-20

### Added

- Procedure initialization exposes its existing read-only manager. Initializers
  may publish immutable completed/total work through IInitializationProgressSource.
  GamePrefab registration and validation publish counts only after each real item
  succeeds; failure and cancellation preserve the actual unfinished counts.

## [9.1.43] - 2026-10-20

### Fixed

- InitializationWorkSlice owns one shared credit for the actual PlayerLoop frame.
  New sibling handles and resumed awaiters cannot reset consumed time. Subsystem
  registration retires the clock for a fresh Play lifetime. Runtime consumers
  acquire frame credit before each indivisible unit, preventing concurrent
  initialization actions from multiplying the continuous-work allowance.
- Added a native sibling test whose late-created handle must await the next real
  frame after another handle consumed the frame credit.

## [9.1.42] - 2026-10-20

### Changed

- Cooperative startup work uses a10ms continuous-work allowance. The original
  4ms prototype kept the loading book responsive but stretched the measured
  fixed-content loading interval to26–43 seconds in Editor. The revised allowance
  reserves part of a60Hz frame for UI while reducing needless next-frame waits;
  every complete-interval100ms frame/paint acceptance gate remains unchanged.

## [9.1.41] - 2026-10-20

### Added

- Native CPU scopes identify each cold discovery assembly and configuration
  validation prefix without spanning asynchronous waits or frame boundaries.

## [9.1.40] - 2026-10-20

### Fixed

- Initialization PlayMode fixtures explicitly bind NUnit Assert alongside the
  framework reflection extension namespace, removing an ambiguous assertion type.

## [9.1.39] - 2026-10-20

### Fixed

- Runtime collector, check-processor and game-item receiver discovery capture
  the loaded assembly domain once and consume the existing reflection owner one
  assembly per work unit. Receiver discovery completes before the BeforeInitStart
  subscriber phase. No new cache, thread or source detachment is introduced.
- Global-setting validation adopts explicit asynchronous configuration checks;
  shared ConfigurationValidation owns cancellation checks and capability dispatch.
- Accepted global file loads and collector sources join before cancellation is
  adopted; frame slicing does not detach those sources. Native discovery tests
  compare the exact reflected type domain.

## [9.1.38] - 2026-10-20

### Fixed

- The PlayMode test assembly explicitly references UniTask for the new
  initialization frame-budget and cancellation fixtures.

## [9.1.37] - 2026-10-20

### Fixed

- Runtime GamePrefab registration, configuration validation and unique UI panel
  creation return to the actual PlayerLoop when their4ms work budget expires.
  Validation supports the explicit IAsyncCheckableConfig capability for content
  whose complete check requires multiple main-thread steps. Procedure adoption
  still joins every actual initialization source before publishing readiness.
- Added native PlayerLoop and cancellation tests for initialization work slices.

## [9.1.36] - 2026-10-20

### Added

- Native CPU Profiler scopes identify initialization action collection and each
  action's synchronous invocation prefix. Async waits remain outside these scopes,
  so markers never span unrelated PlayerLoop work or a frame boundary.

## [9.1.35] - 2026-10-11

### Fixed

- Automatic panel data refresh owns the actual timer node and manager acquired
  for each open. Close and deinitialization release that registration after the
  manager publication retires; old callbacks cannot rearm a subsequent open.
- Removed the modifier's priority-queue-node surface. The existing Timer product
  owns queue identity, while refreshInterval and the protected refresh callback
  remain on the modifier. Focused fixtures cover native expiry, retirement,
  deinitialization and reentrant close/reopen in three cycles each.

## [9.1.34] - 2026-10-11

### Fixed

- The native slot-filter retirement fixture selects its input from the actual
  published slot owner. Detached authoring generation cannot overwrite the input
  reference. Each cycle creates an explicit preview before native pointer input
  to cover this lifetime boundary deterministically.

## [9.1.33] - 2026-10-11

### Fixed

- Slot-filter regression fixtures report live-target, container-binding and
  native pointer-delivery preconditions before claiming filter acquisition or
  retirement. The input acknowledgement callback is retired within its stimulus.

## [9.1.32] - 2026-10-11

### Fixed

- Initialize the slot-filter lifetime fixture's real container with its owned
  one-slot configuration before admitting a visual slot. The original fixture
  attempted to expand an unconfigured controller and failed before exercising
  filter retirement.

## [9.1.31] - 2026-10-11

### Fixed

- Slot filter producers and displays release filters, pointer callbacks and
  event subscriptions through the actual manager source. Native manager
  retirement can precede document disable or panel close without querying an
  absent singleton. Modifier deinitialization also retires remaining live slots.
- Add focused regressions for native disable, logical close and deinitialization,
  with a real slot pointer callback and three consecutive owner admissions.

## [9.1.30] - 2026-10-11

### Fixed

- Toolkit tracing owns the actual published root and container until root
  retirement. Position reset and tracing removal finish against those resources
  before releasing them; later logical close cannot query a retired root.
- Tracing subscriptions deinitialize in pairs and release their actual manager
  source. Focused regressions exercise automatic and manual tracing across
  close/reopen, native disable, Live Reload and modifier deinitialization.

## [9.1.29] - 2026-10-11

### Fixed

- Container slot visuals subscribe to binding and container changes only while
  their UI Toolkit root is published. Root retirement detaches these callbacks
  before clearing slots, so later logical binding cleanup cannot query a released
  root. Reopening and Live Reload adopt the current single-mode container once;
  modifier deinitialization releases both rental and visual subscriptions.
- Add focused close/reopen, native-disable, Live Reload and deinitialization
  regressions, including container replacement and exact callback retirement.

## [9.1.28] - 2026-10-11

### Fixed

- Enable native Unity callbacks in the Edit Mode cleanup fixture. Runtime
  placement permits attachment; ExecuteAlways permits the engine to deliver
  OnDestroy during the actual Edit Mode destruction boundary. Cleanup assertions
  and production ControllerGameItem behavior remain unchanged.

## [9.1.27] - 2026-10-11

### Fixed

- Keep the native cleanup test component in the runtime test assembly so Unity
  can attach it to a GameObject. Preserve its script GUID and reference the
  fixture assembly from Edit Mode tests.
- Reference-cache tests use the existing runtime ControllerGameItem directly
  instead of an empty component declared inside an Editor-only fixture.
- Cleanup fixture teardown owns manager retirement even when a witness stops
  before its expected terminal state.

## [9.1.26] - 2026-10-11

### Fixed

- Qualify NUnit assertions in native cleanup tests and migrate the localized
  description fixture to register the actual synchronous and asynchronous
  producers together. Package test assemblies compile with tests exposed.

## [9.1.25] - 2026-10-11

### Breaking

- Publish the original UI Toolkit layout Task and typed root retirement product.
  Consumers retain their root resources until that Task terminates; release,
  native destruction and pool retirement preserve the original failure while
  independently completing each cleanup operation.
- Publish panel rentals only after acquisition completes. Logical item retirement
  precedes physical pool retention, and failed acquisition returns only its owned
  prefix. Pool clearing withdraws its entry snapshot before invoking callbacks.
- Consolidate controller, inspector and optional FishNet lifecycle members into
  their canonical component source, preserving its Unity asset identity.
- Description generators publish their synchronous and asynchronous producers
  together. Description notifications receive an invocation-owned BindingRequest;
  migrate bindings and await real localized names before root formatting.
- Procedure UI authoring declares one typed entering page separately from auxiliary
  panels. Page transitions consume that exact primary panel identity.

### Fixed

- Preserve post-close subscriber failures in the original typed root retirement.
- Join actual Localization initialization and accepted string requests before
  cancellation propagation; retire their exact references and subscriptions.

### Dependencies

- Require VMCore 1.1.8 for the matching acquisition and retirement contracts.

## [9.1.24] - 2026-10-11

### Fixed

- Allow projects that use explicit tooltip IDs or their own inspection presentation to leave the default tooltip binding unset. Default Open/Close operations require that binding at their actual consumption boundary and report a direct configuration exception instead of silently returning or producing an unrelated startup warning.

## [9.1.23] - 2026-10-11

### Fixed

- Await each original initializer action once, keeping it owned through caller,
  timeout and sibling cancellation. Check actual cancellation after source
  completion instead of attaching a second pending continuation to its UniTask.
- Retain late source failures with independently requested cancellation. Correct
  the foreign-cancellation fixture to throw its original exception from the
  pending async source; keep all eight focused cases and their assertions.

## [9.1.22] - 2026-10-11

### Fixed

- Publish matched procedure-request cancellation with the token-preserving
  TaskCompletionSource API available in Unity's .NET Standard 2.1 profile. Keep
  the original accepted Task and reject duplicate terminal publication directly.

## [9.1.21] - 2026-10-11

### Breaking

- Return each procedure entry/exit request's actual Task from the existing
  IProcedureManager APIs. Callers must await that accepted product instead of
  creating an OnEnter callback completion or waiting synchronously for native
  loading. Merge ProcedureManager's loading/query/Inspector partial declarations
  into its single lifecycle owner; preserve the main script identity.

### Fixed

- Publish transition Tasks before work, retire loading and active request ownership
  before completion, and preserve phase-specific entry failures with independent
  inverse cleanup. Retire queued/active requests and all acquired initializer
  sources on actual Play exit or owner destruction.
- Preserve and join every original same-order initializer source after cancellation,
  timeout or a sibling failure. Retain cancellation callback and late source errors
  independently, and match actual cancellation tokens before classifying a source
  as canceled or timed out. Extend the focused InitializerManagerTests fixture.
- Keep the 9.1.20 physical LogicTickTimer clock/subscription retirement contract
  unchanged. See Documentation~/ProcedureTransitionLifetime.md for the request,
  rollback, source-lifetime and caller migration contract.

## [9.1.20] - 2026-10-11

### Fixed

- Retain LogicTickTimerManager's actual clock for queue deadlines and stop
  notifications after clock publication withdraws or changes. End its acquired
  tick subscription before manager publication releases. Preserve pending queue
  membership so each consumer can retire its original registration.
- Merge the timer's Editor-only Inspector declaration into its actual owner and
  cover clock replacement, pending timer retirement, event detachment and an
  uninitialized owner. See `Documentation~/ManagerSingletonLifetime.md`.

## [9.1.19] - 2026-10-11

### Breaking

- Make `ManagerBehaviour<TInstance>.Instance` a lifecycle-owned read-only publication.
  Derived retirement hooks must override `OnDestroy` and release the base owner in
  `finally`; external singleton assignment is removed. Retain actual subscription
  and rental sources for their matching cleanup, including panel/localization,
  GameItem events and optional FishNet UUID subscriptions. Merge the modified
  GameEventManager and UUIDCoreManager partial declarations into their main owners.
- Distinguish owned `GameEventManager.Register(id)` rentals from borrowed
  `Register(existingEvent)` products. Unregister and manager retirement return
  only owned rentals, after removing the original registration. See
  `Documentation~/ManagerSingletonLifetime.md` for migration and focused validation.

### Fixed

- Release interface-valued manager singletons when their native owners retire,
  allowing ManagerCreator's destroyed-base replacement to publish its selected
  derived owner. Reject a second live physical owner at the publication boundary.
- Keep standalone test owners isolated from production manager discovery and
  remove test setter overrides. Cover native replacement, deferred destruction,
  duplicate owners, failing cleanup and invalid generic contracts.

## [9.1.18] - 2026-10-11

### Fixed

- Apply UI Toolkit close policy at panel creation. DisplayNone documents remain
  enabled and hidden before the first open and across subsequent closes, so
  reopening retains their document and runtime panel instead of constructing
  them inside gameplay callbacks. Consumer root bindings and pending layout
  work still retire on close. Cover creation, native panel identity, repeated
  opens, callback retirement and canceled layouts through the real producer.
  See `Documentation~/UIToolkitRootLifetime.md` for the lifetime contract.

## [9.1.17] - 2026-10-10

### Fixed

- Give the native Play Mode tracing fixture a valid 32-digit script GUID so
  Unity imports it and compiles its declared test assembly.
- Preserve whole-item tracing's initial published target position instead of
  reflecting its offset around the source. Cover both offset signs and source
  displacement through the default Add entry point.

## [9.1.16] - 2026-10-10

### Fixed

- Exercise tracing manager destruction through actual Unity Play Mode lifetimes,
  followed by recreation and another pool rental. Edit Mode binding tests no
  longer assume Unity calls OnDestroy for objects that never ran Awake.

## [9.1.15] - 2026-10-10

### Fixed

- Preserve the distinct owner and position-source roles of Transform tracing.
  Returning either role, removing or replacing a target now retires both indexes,
  subscriptions and pooled group collections without retaining old rentals.
- Detach the source when whole-item tracing loses its last target. Both tracing
  managers release all remaining relationships when destroyed.
- Add native Transform and ControllerGameItem lifetime regressions. See
  `Documentation~/GameItemTracing.md` for the ownership and validation scope.

## [9.1.14] - 2026-10-09

### Fixed

- Give contour snapshots a local-path value input and an explicit Sprite capture
  producer. Native comparison tests now consume local contours directly instead
  of relying on a rejected runtime Sprite physics override.

## [9.1.13] - 2026-10-09

### Fixed

- Construct the contour regression's runtime Sprite with Full Rect geometry and
  physics data. Convert its local control outlines to Sprite.rect coordinates
  before applying the native physics-shape override.

## [9.1.12] - 2026-10-09

### Added

- Refine native 2D mouse candidates with an owned point filter before priority
  and stay-event publication. Sprite contours can use one coarse box without
  decomposing moving UI selection geometry into many physics shapes.
- Capture immutable Sprite physics contours for allocation-free local point
  queries. See `Documentation~/ColliderMousePointFilters.md` for the lifecycle.

## [9.1.11] - 2026-10-08

### Fixed

- Keep the collider trigger's runtime callbacks and Editor reset hook in one
  owned declaration. Move its unchanged public callback delegate to its own
  source file and remove unreachable null checks on the readonly callback table.

## [9.1.10] - 2026-10-08

### Fixed

- Give each game-event propagation its own callback snapshot lease. Repeated
  dispatch no longer returns an instance-owned list to the pool multiple times;
  nested propagation preserves outer membership, arguments and priority order.
- Release callback references and return the snapshot on callback exceptions,
  while preserving the original exception and successful completion hooks.
- Apply the same invocation lifetime to collider mouse callback snapshots so
  nested event types do not invalidate an outer trigger's enumeration.

See `Documentation~/GameEventPropagation.md` for ownership and focused validation.

## [9.1.9] - 2026-10-08

### Fixed

- Validate loaded global settings after runtime GamePrefab registration. Strict
  references in general-setting checks now resolve in a cold Player as well as in
  the Editor, without depending on the Editor's existing authoring registry.

See `Documentation~/ConfigurationInitialization.md` for lifecycle ownership and
cold Player validation.

## [9.1.8] - 2026-10-05

### Fixed

- Supply the required target for the focused UI-root mouse-event fixture and
  attach retired elements to the live panel during its negative control, so the
  test proves callback retirement independently of element detachment.

## [9.1.7] - 2026-10-05

### Fixed

- Publish paired live-root availability and retirement events when a UI Toolkit
  document opens, is reconstructed by Unity Live Reload, closes or is destroyed.
  Child panel modifiers can rebind to the displayed tree without treating a
  detached Inspector preview as a live view.
- Transfer pointer registrations and language styling to the replacement root,
  cancel retired layout continuations, and preserve closed-document visibility.
- Publish layout change notifications after the root's final visibility and
  picking setup, so view consumers can restore their own overlay visibility.

See `Documentation~/UIToolkitRootLifetime.md` for ownership and validation.

## [9.1.6] - 2026-10-04

### Fixed

- Create the framework EventSystem with InputSystemUIInputModule when Unity enables
  the Input System backend. Legacy input projects retain StandaloneInputModule.
  This removes disabled legacy Input reads during normal scene startup.
- Keep Input Action ID authoring metadata available independently of the selected
  runtime input backend, so legacy-only configurations compile with the declared
  Input System package dependency.
- Select the same backend for collider pointer buttons and 3D position reads,
  closing the remaining legacy-only compilation errors in that owner.

See `Documentation~/InputBackend.md` for selection and validation.

## [9.1.5] - 2026-10-02

### Added

- Record the seven logic-tick callback phases in the native CPU Profiler, retaining
  phase order and exception behavior without per-tick managed allocations.
  This exposes cold-start and gameplay work previously combined in one tick sample.

See `Documentation~/LogicTickProfiling.md` for timing ownership and cost bounds.

## [9.1.4] - 2026-10-01

### Fixed

- Exercise reference ownership with the real manager types in focused tests.
  Remove fixture manager subclasses whose inherited creation metadata made them
  eligible to replace production managers while the test assembly was loaded.

## [9.1.3] - 2026-10-01

### Fixed

- Publish inactive controller references after initialization so cached reference
  objects cannot enter native physics or gameplay updates.
- Return unpublished reference rentals when initialization fails, preserving the
  original exception and leaving the cache empty.

See `Documentation~/StateCloning.md` for reference and clone ownership.

## [9.1.2] - 2026-10-01

### Fixed

- Avoid quadratic eager multicast-prefix allocation while registering priority
  callbacks. Publish and cache balanced combined delegates after membership changes.
- Remove the exact delegate registration when multicast invocation lists overlap,
  preserving the remaining callbacks' priority and insertion order.
- Reject invalid null registrations at their admission boundary.

See `Documentation~/PriorityEvents.md` for ownership, view and snapshot semantics.

## [9.1.1] - 2026-09-30

### Fixed

- Normalize the new materialization sources and meta records to the source
  policy's single final newline and whitespace contract.

## [9.1.0] - 2026-09-30

### Changed

- Make GameItemManager the completed-clone producer and publish creation only
  after source-state copying; return unpublished clones when copying fails.
- Declare AuthoredDefaults or ClonedState during native rental initialization,
  allowing owned-child providers to avoid constructing discarded defaults.
- Restore nested initialization contexts on normal and exceptional exits.

See `Documentation~/StateCloning.md` for the initialization contract.

## [9.0.12] - 2026-09-29

### Fixed

- Mark 36 existing folder meta records as directory assets, preserving all GUIDs.
  This closes the resolved-package folder ownership findings without changing
  runtime code or asset references.

## [9.0.11] - 2026-09-29

### Fixed

- Retire UI callbacks and disable tokens through the exact event instances acquired
  for that panel lifetime, including shutdown after the game-event registry is cleared.
- Release container, focus and pointer event bindings on panel retirement, including
  open panels that do not receive a normal close event.
- Replace the incomplete native-entry fixture with event recycling, registry removal
  and same-ID replacement cases. Native message delivery remains a production Play Mode check.

## [9.0.10] - 2026-09-29

### Fixed

- Test pool-clear and native-destruction retirement entries directly in Edit Mode,
  where runtime Awake/OnDestroy messages are not dispatched, and supply the fixture's
  panel manager explicitly. Real Play Mode shutdown remains an integration check.

## [9.0.9] - 2026-09-29

### Fixed

- Exercise native destruction with the production UIPanel and an interface-level
  subscription fixture; Editor-only test components cannot be attached as runtime scripts.

## [9.0.8] - 2026-09-29

### Fixed

- Keep UI lifetime regression support components in individual scripts so the
  focused fixture satisfies the package's one-top-level-type source policy.

## [9.0.7] - 2026-09-29

### Fixed

- Retire both directions of visual-element bindings on removal and replacement,
  and release binding maps when their panel modifiers deinitialize.
- Deinitialize panel modifiers on native GameObject destruction as well as pool
  clear, releasing external subscriptions when Play Mode exits without domain reload.
- Cover reciprocal lookups, replacement collisions, repeated entry generation,
  independent bind names and two native panel lifetimes in focused Editor tests.

## [9.0.6] - 2026-09-29

### Fixed

- Reference Unity Resource Manager from the Editor test assembly so locale lifecycle
  regression tests can complete Localization initialization before changing languages.

## [9.0.5] - 2026-09-29

### Fixed

- Notify localized panel controllers as well as their child modifiers when a panel
  opens or its locale changes, so configured language styles apply to UI Toolkit panels.
- Unsubscribe panel controllers when panels close, are destroyed, or the localization
  manager shuts down; cover these lifetimes and reopening in Editor regression tests.

## [9.0.4] - 2026-09-23

### Fixed

- Clear game-event callbacks, validity checks, and enable tokens whenever an event
  returns to its pool so later registrations cannot retain stale subscribers.
- Release registered game events without false callback-leak warnings when the
  game-event manager shuts down before UI subscribers.

## [9.0.3] - 2026-09-20

### Documentation

- Rewrite the README around the stable package installation, dependency,
  layout, project setup, and validation contracts.
- Remove version-specific behavior, implementation details, API examples,
  and migration instructions from the README; those remain in the changelog
  and authoritative code.

## [9.0.2] - 2026-09-19

### Fixed

- Restore the `VMFramework.Core` namespace import required by the weighted-item
  interfaces after removing the Inspector initialization callback.

## [9.0.1] - 2026-09-19

### Changed

- Stop creating missing weighted-item managed-reference values from hidden
  Inspector initialization; authors now assign those values explicitly.
- Remove redundant list-creation and empty Inspector initialization overrides
  when the serialized fields already initialize themselves at declaration.
- Use the imported `[Serializable]` attribute consistently instead of its
  fully qualified spelling.

## [9.0.0] - 2026-09-19

### Changed

- Replace the `BaseConfig` and `IConfig` inheritance contract with the narrow
  checking, initialization and Inspector contracts each configuration actually uses.
- Let configuration owners initialize their runtime indexes directly instead of
  initializing passive list elements.
- Keep weighted-item Inspector value creation without its former explicit JSON
  member annotations.

### Removed

- Remove the unused priority configuration types and their directory.

### Migration

- Plain serialized configuration data no longer inherits initialization state.
  Owners must call explicit runtime-index builders for data that produces a cache.
- Code that used `IConfig` should depend on `ICheckableConfig`,
  `IInitializableConfig` or `IInspectorConfig` only when it consumes that behavior.

## [8.0.2] - 2026-09-12

### Fixed

- Let container addability checks evaluate expandable empty slots even when the
  requested range contains no allocated slots. Empty unbounded containers now
  admit the same items as `AddItem`, including the first Buff grant.

### Changed

- Consolidate the container's enumeration, JSON/clone methods and Editor actions
  into its owning type without changing their public or serialized contracts.

## [8.0.1] - 2026-09-11

### Fixed

- Read and write item-count dictionary entries separately so IL2CPP can compile
  `BuildCountDictionary` for WebGL with full generic sharing. Keep its generic API and
  accumulation behavior unchanged.

## [8.0.0] - 2026-09-10

### Changed

- Replace UI language and procedure configuration containers with directly serialized `List<T>`
  fields. Their settings own lookup and ID validation, and initialize the list elements directly.
- Remove the dictionary and structure configuration abstractions, their interfaces and helpers,
  and unused tag/list containers and priority-preset adapter. Configuration queries now read the
  same authoring list before initialization, after initialization, and after script reload.
- Existing assets require migration of each container's nested `configs` list into the setting
  field before saving with this version. Preserve managed references and Unity asset references.

### Fixed

- Remove the previous locale's stylesheet when a UI Toolkit panel changes language.

### Tests

- Cover native list round trips, edits after initialization, duplicate IDs, unsaved edits across
  script reload, and locale-style replacement without removing shared panel styles.

## [7.0.9] - 2026-09-08

### Fixed

- Clean package-test wrapper registrations by direct provider identity so intentionally unmapped test
  GamePrefab types do not emit editor errors during teardown.

## [7.0.8] - 2026-09-08

### Fixed

- Unregister temporary native-serialization test wrappers before deleting them so package tests
  cannot leave missing provider references in a consumer project's production settings.
- Persist removal of deleted GamePrefab providers during editor refresh, including recovery after an
  interrupted test run or an external asset deletion.

## [7.0.7] - 2026-09-06

### Changed

- Disable initialization progress logs by default. Both action-start messages and the batch-start
  summary now require the initializer to opt in through `EnableInitializationDebugLog`.
  Initialization failures, cancellation, timeout diagnostics, and execution state remain available.

## [7.0.6] - 2026-09-05

### Fixed

- Use Unity's persistent setup lifecycle for the script-reload regression, preserving its fixture
  across the intentional domain reload.

## [7.0.5] - 2026-09-05

### Fixed

- Keep configuration initialization flags out of Unity script-reload serialization so authoring
  queries cannot select a runtime dictionary discarded by the reload. Verify hot-reload state,
  unsaved authoring values, and subsequent repeated initialization in an Editor regression.

## [7.0.4] - 2026-09-05

### Fixed

- Supply an explicit locale in the invalid-description regression so Edit Mode verifies the same
  table-reference failure boundary as runtime without changing the Editor's selected locale.

## [7.0.3] - 2026-09-05

### Fixed

- Respect `LocalizedGamePrefab.hasDescription` when publishing the localized description reference,
  preventing disabled descriptions materialized by native Unity serialization from entering Tooltip
  localization lookups with an empty table reference.

## [7.0.2] - 2026-09-05

### Fixed

- Preserve the four polymorphic base/boost limit sources on `BaseBoostFloatParameterConverter`
  through native Unity Prefab serialization.

## [7.0.1] - 2026-09-05

### Fixed

- Reference VMCore directly from the Editor test assembly so native grid serialization tests compile
  when enabled through a consumer project's package test configuration.

## [7.0.0] - 2026-09-05

### Changed

- Migrate all framework settings, filters, MonoBehaviour roots, and Editor containers to Unity
  serialization. Odin Inspector remains an authoring UI dependency.
- Store General Settings provider membership as Unity object references, preserve polymorphic
  configuration lists and dependency graphs, and serialize type-filter and grid configuration values.
- Rebuild runtime configuration indexes during initialization, including repeated initialization.
- Keep transient Editor selections in session memory and read registry viewers from their owner.
- Consolidate the modified configuration and component declarations and remove the serializer
  dependency from the framework test assembly.

### Migration

- Existing assets must be captured before upgrading and verified after native save/reload.
- Provider enumeration is read-only; authoring membership changes use the setting's add/remove API.
- VMCore 1.0.2 supplies native serialization for CubeInteger values in grid settings.

## [6.4.0] - 2026-09-05

### Added

- Added `SerializableType`, a Unity-native serializable Type reference backed by an
  assembly-qualified name. It supports fields and collection elements inside GamePrefab managed
  reference graphs, preserves null values, and fails directly when a persisted type cannot be
  resolved.

## [6.3.5] - 2026-09-05

### Fixed

- Game Prefab Game Tag selectors now enforce unique list entries while authoring, preventing the
  same registered tag from being selected more than once.

## [6.3.4] - 2026-09-05

### Fixed

- Qualified the serialization attributes on legacy GamePrefab test doubles without importing
  `System`, preserving the existing unambiguous `UnityEngine.Object` test calls.

## [6.3.3] - 2026-09-05

### Fixed

- Marked the package's legacy GamePrefab test doubles as serializable so the full package
  test suite satisfies the same native managed-reference contract as production config types.

## [6.3.2] - 2026-09-05

### Fixed

- Added the Input System assembly reference required by the expanded native-serialization Editor
  tests when package tests are enabled in a consumer project.

## [6.3.1] - 2026-09-05

### Added

- Expanded GamePrefab native-serialization tests to cover the wrapper field contract, every loaded
  GamePrefab config type, the production wrapper creator, nested managed references, Unity object
  references, edit-save-reload mutations, and every discoverable consumer-project wrapper asset.

## [6.3.0] - 2026-09-05

### Changed

- GamePrefab Wrapper assets now persist their polymorphic GamePrefab graphs with Unity
  managed-reference serialization. Game tags use a native serialized list, and Input System
  event action IDs use a native string representation.

### Removed

- GamePrefab Wrapper persistence no longer uses Odin `SerializedScriptableObject` or manually
  writes Odin `serializationData` payloads.

## [6.2.3] - 2026-09-05

### Fixed

- Runtime initialization now validates every registered `IPrefabProvider` after
  Game Prefab registration and throws one aggregated exception listing all missing
  or destroyed Prefab references before any gameplay system can instantiate them.

## [6.2.2] - 2026-09-03

### Fixed

- Tag navigation builds the Game Editor menu before accessing its search configuration,
  so opening a new window works before its first repaint. Tests cover both new windows
  and already-built windows with prior filters.

## [6.2.1] - 2026-09-03

### Fixed

- The Editor test assembly now references VM Odin Extensions, required by the Game Prefab
  type-validation interface used by the tag-filter tests.

## [6.2.0] - 2026-09-03

### Added

- Every Game Tag field has a funnel button that opens Game Editor with that tag as its only
  filter. It clears any previous text search and expands the matching navigation branches.
  The existing tag-settings shortcut remains available; empty and mixed-value fields cannot
  start a tag query.
- `GameEditor.FilterByGameTag` exposes the same navigation action to editor integrations.
- Focused Editor tests cover replacing prior selections, clearing stale searches, repeated
  navigation, filter persistence across tree rebuilds, and rejection of empty tag IDs.

### Fixed

- Removed an orphaned test-fixture folder meta from the Git package.

## [6.1.0] - 2026-09-03

### Added

- Game Editor has a searchable, multi-select Game Tag filter above its existing text search.
  All/Any matching reads the configs inside single and multiple wrappers, keeps navigation ancestors,
  and hides unrelated branches. Clear restores the complete tree without clearing the text search.
- Focused Editor tests cover tag matching, inversion, empty selections, wrapper ownership and
  filtered-tree ancestry.

### Fixed

- Multi-tag `GameTagFilter` now compares the requested tags with the owner's tags instead of
  comparing each owned tag with itself.

## [6.0.2] - 2026-08-13

### Fixed

- Made `StackableMergeItem` report the same removable count that its removal operation can
  actually remove, allowing callers to preflight multi-item removals without rejecting valid
  stackable items.

## [6.0.1] - 2026-08-12

### Changed

- Documented that `ILocalizedPanelModifier.OnCurrentLanguageChanged` runs once before modifier
  `IUIPanel.OnOpen` handlers during panel opening, so open-time UI bindings are not available to
  that initial localization callback.

## [6.0.0] - 2026-08-12

### Removed

- Removed the unused `BasicToggle`, `CarouselGroupVisualElement`, and `ToggleVisualElement` UI
  Toolkit controls.

## [5.0.0] - 2026-08-12

### Removed

- Removed `BoolStateVisualElement` from VMFramework. The general-purpose control now belongs to
  `com.vm233.ui-toolkit-extensions` as `VM233.UIElements.BoolStateVisualElement`.

## [4.1.0] - 2026-08-12

### Added

- Added `BoolStateVisualElement`, a non-interactive native boolean field for decorative two-state
  visuals. Scripts and UXML set its `value`, while USS consumes its `:checked` pseudo-state without
  exposing pointer or keyboard interaction.

## [4.0.0] - 2026-08-12

### Changed

- `SlotVisualElement` now uses Unity UI Toolkit's native boolean-field contract. Its `value`
  drives the `:checked` pseudo-state, and its inherited click manipulator supplies the `:active`
  pseudo-state without changing the slot's authored visual hierarchy or inheriting base-field
  theme styling.

### Removed

- Removed the configurable slot content-container type and its UXML attribute. Authored children
  now always use the slot itself as the content container.

## [3.1.0] - 2026-08-03

### Added

- Added a virtual `Common Presets` branch under `Core Runtime` in Game Editor. It displays the
  ordered Project Settings mappings and opens each concrete preset asset without adding serialized
  state back to `CoreSettingFile`.

## [3.0.0] - 2026-08-02

### Changed

- Common Preset is now consumed from the independent `com.vm233.common-preset` package instead of
  being implemented inside VMFramework.
- Asset-backed preset mappings now use the VM Common Preset package's Unity-serialized Project
  Settings list as their sole authority.
- `PriorityDefinesPreset` now uses the package's explicit fixed-preset definition and value
  attributes.

### Removed

- Removed VMFramework's Common Preset runtime types, editor initializers, Odin drawer, fixed-preset
  registry, legacy `GeneralSetting`, and embedded Common Preset tests.
- Removed Common Preset ownership from `CoreSetting`, `CoreSettingFile`, and VMFramework's global
  configuration paths. Asset-backed mappings now belong directly to VM Common Preset Project Settings.

## [2.0.0] - 2026-08-02

### Fixed

- The Manager container Edit Mode test now uses an isolated preview scene through an internal
  explicit-scene initialization path, without mutating or skipping tests around the Test Runner scene.
- Manager creation now resolves `^Core` only from active-scene roots and category containers only
  from its direct children, preventing unrelated nested objects with reserved names from being
  reparented and used as unconfigured manager owners.
- Logic tick accumulation now advances against the active `TickGap` instead of the serialized
  override field, so disabling the override and calling `SetTickGap` affect the real cadence.
- Tick-gap and elapsed-time APIs now reject non-finite or invalid values instead of allowing an
  infinite scheduler loop.
- The Editor test assembly now declares its public Odin serialization dependency, allowing
  package tests that instantiate serialized settings to compile.

### Changed

- Replaced the mutable, boolean-based `StateCloneHint` API with the immutable
  `StateCloneContext` tag set. Clone producers now add explicit tags and consumers query only the
  semantics they own.
- Editor-only General Setting and Game Prefab folder paths now live in
  `ProjectSettings/VMFrameworkEditorSettings.asset` and are configured through
  `Edit > Project Settings > VMFramework`, so editor tools no longer depend on framework manager or
  Global Setting initialization.
- Maintenance actions previously exposed as `EditorSettingFile` inspector buttons are now Unity
  menu commands under `VMFramework > Global Settings` and `VMFramework > Game Prefabs Tools`.
- Replaced callback-based initialization actions with cancellation-aware `UniTask` actions.
- Initialization orders remain sequential while actions in the same order run concurrently.
- Initialization failures, caller cancellation, and timeouts now propagate to callers and retain per-action status.
- Procedure and editor initialization no longer use `async void` or completion callbacks.
- Game Editor windows now rebuild after editor initialization has actually completed, preventing a completed initialization from leaving an open window on its loading preview.
- Auto-registered common presets now restore missing initial entries and save repaired preset assets immediately.

### Added

- Added process-local `StateCloneTag` registration, allocation-free span construction, immutable
  `WithTag` derivation, and the framework-owned `OwnerStateIncluded` tag used when cloning owned
  items together with their owner state.
- Added ordered pre-simulation, simulation-owner, and post-simulation Logic Tick phases.
- Added `TickDeltaTime`, `TickInterpolationAlpha`, and deterministic `AdvanceTime` APIs.
- Added Edit Mode coverage for Logic Tick phase ordering, deferred next-tick callbacks, active
  cadence, immutable admitted step duration, recursive-advance rejection, interpolation progress,
  pause gating, and invalid input.
- Added Edit Mode coverage for ordering, duplicate delegates, exception propagation, cancellation, timeout behavior,
  and common-preset seed reconciliation.
- Added an enabled-by-default `disableExistingSlotsWhenContainerUnbound` option to
  `UIToolkitContainerModifierBase`. Authored slots are disabled while no container is bound and restored when a
  container is bound again.

### Removed

- Removed `StateCloneHint` and its ambiguous `isNested` field. Root clones should use
  `StateCloneContext.Empty`; owned-child producers should derive an explicit tagged context.
- Removed the legacy `EditorSettingFile` Global Setting and its Game Editor node. Projects should
  remove the corresponding asset, Addressables entry, and `EditorSetting` scene component after
  migrating any custom folder paths to Project Settings.
- Removed the unused `GameTagBasedConfigBase`, `KeyCodeTranslation`,
  `SingleArgumentLocalizedString`, and `InitialTilemapConfig` legacy configuration types.
- Removed the unused GameTag extra-info API: `GameTagExtraInfo`,
  `IGameTagExtraInfosOwner`, and `GameTagExtraInfoUtility`.

## [1.0.0] - 2026-07-09

### Added

- Converted VMFramework into a Unity Package Manager Git package rooted at this repository.
- Added package metadata through `package.json`.
- Added package README with installation, dependency, and layout information.

### Changed

- Package content now mirrors BattleIdle's current `Assets/VMFramework` implementation.
- Internal global settings path now resolves to the project global settings folder: `Assets/GameResources/Configurations/GlobalSettings`.
- Existing package `.meta` files are preserved to keep Unity asset GUIDs stable.

### Removed

- Removed the obsolete `JSONConverters` class from VMFramework.
- Removed VMFramework's direct assembly reference to `JSONConverterExt`.
- Removed full Unity project folders from this repository so it can be consumed as a Git package.
