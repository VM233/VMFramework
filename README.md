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

The package Editor test assembly is `VMFramework.Editor.Tests`. When running package tests from a
consumer project, expose the package through that project's `testables` manifest entry.

Keep package `.meta` files intact so Unity asset GUID references remain stable.

## Changes and License

See [CHANGELOG.md](CHANGELOG.md) for version-specific behavior, migrations, and breaking changes.
The package is licensed under [GPL-3.0-or-later](LICENSE).
