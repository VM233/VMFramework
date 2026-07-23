# Changelog

All notable changes to this package are documented here.

## [Unreleased]

### Changed

- Replaced callback-based initialization actions with cancellation-aware `UniTask` actions.
- Initialization orders remain sequential while actions in the same order run concurrently.
- Initialization failures, caller cancellation, and timeouts now propagate to callers and retain per-action status.
- Procedure and editor initialization no longer use `async void` or completion callbacks.
- Game Editor windows now rebuild after editor initialization has actually completed, preventing a completed initialization from leaving an open window on its loading preview.

### Added

- Added Edit Mode coverage for ordering, duplicate delegates, exception propagation, cancellation, and timeout behavior.

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
