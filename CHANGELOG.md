# Changelog

All notable changes to this package are documented here.

## [Unreleased]

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
