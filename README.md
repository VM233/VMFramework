# VMFramework

VMFramework is a reusable Unity 6.4 framework package for VM233 projects. It contains shared gameplay architecture, configuration tooling, UI panel infrastructure, localization helpers, resource management utilities, map/tile helpers, editor tooling, and optional FishNet integration.

## Installation

Add the package through Unity Package Manager using the Git URL:

```text
https://github.com/VM233/VMFramework.git
```

For a fixed revision, use:

```text
https://github.com/VM233/VMFramework.git#<commit>
```

## Package Name

```text
com.vm233.vmframework
```

The package keeps the existing assembly name:

```text
VMFramework
```

## Requirements

The package declares Unity registry dependencies in `package.json` for Addressables, Localization, Input System, TextMeshPro/UGUI, Tilemap Extras, Visual Effect Graph, and Newtonsoft JSON.

These external packages or plugins must also be available in the consuming Unity project because the current `VMFramework.asmdef` references their assemblies:

- VMCore (`com.vm233.vmcore` or an embedded `Assets/Plugins/VMCore` copy)
- VM Odin Extensions (`https://github.com/VM233/VMOdinExtensions.git`)
- UniTask (`com.cysharp.unitask` or an embedded UniTask copy)
- Odin Inspector
- FishNet, when using `FishnetExtension`

BattleIdle currently supplies these dependencies from its project manifest and embedded plugins.

## Layout

- `Main`: core runtime, editor tools, JSON helpers, UI panel infrastructure, configuration, game logic architecture, localization, resources, timers, and procedures.
- `MapExtension`: tilemap, grid map, map utilities, and tile config support.
- `FishnetExtension`: optional FishNet networking integration.
- `Experimental`: experimental framework code.
- `GameResources`: package-owned fonts and script templates. Runtime project global setting assets are expected under `Assets/GameResources/Configurations/GlobalSettings`.

## Manager Containers

`ManagerCreator` owns the root `^Core` object in the active scene. Manager category containers are
resolved only among the direct children of that root. Business and configuration hierarchies may
therefore reuse category names such as `Audio` without being moved or treated as manager owners.

## Logic Tick Simulation Phases

`LogicTickManager` publishes one ordered deterministic step:

1. `OnPreTick`
2. `OnTick`
3. the current `OnNextTick` snapshot
4. `OnPreSimulationTick`
5. `OnSimulationTick`
6. `OnPostSimulationTick`
7. `OnPostTick`

Simulation command producers should use `OnPreSimulationTick`, the single simulation owner should
use `OnSimulationTick`, and achieved-state or collision observers should use
`OnPostSimulationTick`. Actions registered from simulation callbacks remain queued until the next
logic tick.

Use `TickDeltaTime` for per-step simulation math. It remains the immutable admitted duration
throughout the current tick even if a callback changes `TickGap`; outside a tick it reports the
active gap for the next admission. Use `TickInterpolationAlpha` for presentation interpolation.
`AdvanceTime` is available to deterministic clock owners and tests; it uses the active `TickGap`
configured through `SetTickGap`.

## Notes

- This repository is now a Unity Package Manager package root, not a full Unity project.
- `.meta` files are kept so Unity asset GUID references survive the move from `Assets/VMFramework` to a Git package.
- `JSONConverters` was removed from VMFramework; framework code no longer depends on `JSONConverterExt`.
