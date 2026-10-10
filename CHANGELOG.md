# Changelog

All notable changes to this package are documented here.

## [Unreleased]

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
