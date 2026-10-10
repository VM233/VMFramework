# VMFramework

VMFramework is the shared Unity package used by VM233 projects. It provides gameplay and
configuration infrastructure, UI and localization helpers, resource utilities, editor tooling,
map support, and optional FishNet integration.

This repository is a Unity Package Manager package root, not a standalone Unity project.

## Install

Pin an immutable full commit SHA in the consuming project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.vm233.vmframework": "https://github.com/VM233/VMFramework.git#<full-commit-sha>"
  }
}
```

- Package ID: `com.vm233.vmframework`
- Main assembly: `VMFramework`
- Minimum Unity version: `6000.4`

## Dependencies

`package.json` is the authority for UPM dependencies. The consuming project must also provide
assemblies referenced by `VMFramework.asmdef`, including VM Odin Extensions and Odin Inspector.
Install FishNet when compiling `FishnetExtension`.

Use registry versions or remote Git URLs pinned to full immutable commit SHAs. Local filesystem
dependencies are not supported.

## Layout

- `Main`: runtime and Editor framework code.
- `MapExtension`: tilemap and grid-map support.
- `FishnetExtension`: FishNet integration.
- `Experimental`: experimental framework code.
- `GameResources`: font-authoring character lists and script templates.
- `Tests`: package Editor tests.

## Project Setup

Use `Edit > Project Settings > VMFramework` to configure the project-relative folders for
`GeneralSetting` assets and Game Prefab wrappers. The settings are stored in
`ProjectSettings/VMFrameworkEditorSettings.asset`.

Runtime project global-setting assets are expected under
`Assets/GameResources/Configurations/GlobalSettings`.

Framework maintenance commands are available under the `VMFramework` Unity menu.

## Validation

The package test assemblies are `VMFramework.Editor.Tests` and `VMFramework.PlayMode.Tests`. When running package tests from a
consumer project, expose the package through that project's `testables` manifest entry.

Keep package `.meta` files intact so Unity asset GUID references remain stable.

## Changes and License

See [CHANGELOG.md](CHANGELOG.md) for version-specific behavior, migrations, and breaking changes.
See [State cloning](Documentation~/StateCloning.md) for native initialization and clone publication.
See [Priority events](Documentation~/PriorityEvents.md) for callback registration and publication.
See [Game event propagation](Documentation~/GameEventPropagation.md) for callback snapshot lifetimes.
See [Game item tracing](Documentation~/GameItemTracing.md) for binding and pool-return lifetimes.
See [Collider mouse point filters](Documentation~/ColliderMousePointFilters.md) for precise 2D picking.
See [Logic tick profiling](Documentation~/LogicTickProfiling.md) for native phase timing.
See [EventSystem input](Documentation~/InputBackend.md) for input backend setup.
See [UI Toolkit roots](Documentation~/UIToolkitRootLifetime.md) for live-view binding.
See [Configuration initialization](Documentation~/ConfigurationInitialization.md) for runtime loading and validation.
The package is licensed under [GPL-3.0-or-later](LICENSE).
