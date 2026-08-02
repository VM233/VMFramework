# Changelog

All notable changes to this package are documented here.

## [Unreleased]

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
