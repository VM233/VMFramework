# Changelog

All notable changes to this package are documented here.

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
