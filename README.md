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

- VM Common Preset (`com.vm233.common-preset` 1.1.0 or newer)
- VMCore (`com.vm233.vmcore`)
- VM Odin Extensions (`https://github.com/VM233/VMOdinExtensions.git`)
- UniTask (`com.cysharp.unitask`)
- Odin Inspector
- FishNet, when using `FishnetExtension`

Pin package dependencies to registry versions or remote Git URLs with full immutable commit SHAs.

Because VM Common Preset is distributed as a Git package, consuming projects must pin its Git URL
directly in `Packages/manifest.json`; the semantic dependency in this package only expresses the
required version to Unity's resolver.

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

## Game Prefab Startup Validation

After all Game Prefabs are registered, VMFramework validates every registered
`IPrefabProvider` before gameplay managers and procedures consume the registry. Missing or
destroyed Prefab references stop initialization with one `MissingGamePrefabReferencesException`
that lists every invalid Game Prefab ID and concrete config type, so projects can repair the
complete invalid set instead of discovering failures one instantiation at a time.

## Editor Project Settings

Open `Edit > Project Settings > VMFramework` to configure the project-relative folders used for
`GeneralSetting` assets and Game Prefab wrapper assets. The values are stored in
`ProjectSettings/VMFrameworkEditorSettings.asset` and are available directly to editor tooling;
they do not depend on VMFramework manager creation, global-setting loading, Addressables, or scene
initialization.

## Game Editor Tag Filtering

Use **Tags: All** above the menu tree's text search to select registered Game Tag IDs, then confirm
the selection. **All** requires every selected tag on the same Game Prefab; **Any** requires at least
one. The selector uses the current Game Tag registry, not a separate list of item categories.

Filtering reads the real configs in single/multiple Game Prefab wrappers. It preserves the ancestors
of matching entries but does not include unrelated siblings. **Clear** restores all entries; the
existing text search continues to apply. Filter selections belong to each Game Editor window and
do not modify Game Prefab assets or runtime tags.

Every Game Tag field, including entries in tag collections, also has a **funnel** button on its
right. Click it to open/focus Game Editor, replace the selected tags with that exact tag, clear
the old text search, and expand the matching branches. The adjacent magnifier still opens the
Game Tag settings. The funnel is disabled for empty or mixed-value fields. Editor integrations
can apply the same query with `GameEditor.FilterByGameTag(tagId)` on their target window.

## Editor Maintenance

Framework maintenance commands are available from the Unity menu:

- `VMFramework > Global Settings`: check, locate, create, move, and address settings.
- `VMFramework > Game Prefabs Tools`: collect providers, remove empty wrappers, and move wrappers to
  the configured folder.

Projects upgrading from the legacy `EditorSettingFile` should copy any non-default folder paths into
Project Settings, then remove the old `EditorSettingFile.asset`, its Addressables entry, and the
`EditorSetting` scene component. Those legacy objects are no longer configuration authorities.

## Common Presets

Common Preset runtime types, Odin drawers, registration, and Project Settings ownership now live in
the independent `com.vm233.common-preset` package. VMFramework retains only its
`PriorityDefinesPreset` declaration and consumes the package API.

Projects upgrading from VMFramework 2.x should install VM Common Preset and VMFramework 3.x in the
same Package Manager resolve. Configure existing preset asset references directly in
`ProjectSettings/VMCommonPresetSettings.asset` through `Edit > Project Settings > VM Common Preset`;
the legacy VMFramework `CommonPresetGeneralSetting` asset is no longer an authority.

Game Editor displays a virtual `Common Presets` branch under `Core Runtime`. Its children mirror the
ordered Project Settings list and open the concrete preset assets; neither the branch nor its items
are serialized into `CoreSettingFile`.

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

## SlotVisualElement

`SlotVisualElement` is a native boolean field. Use its `value`, `SetValueWithoutNotify`, and
`ChangeEvent<bool>` contract to own selection state. USS can style the slot directly with
`.slot:checked` and `.slot:active`; consumer projects decide what the boolean state means.

## State Clone Contexts

`StateCloneContext` is an immutable, allocation-free tag set passed through `IStateCloner` and
`IStateCloneable`. Each module owns its clone semantics by registering tags once in static fields:

```csharp
public static readonly StateCloneTag CustomBehavior = StateCloneTag.Create();
```

Root callers can build a context from stack memory:

```csharp
Span<StateCloneTag> tags = stackalloc[] { CustomBehavior };
var context = new StateCloneContext(tags);
var clone = source.GetClone(context);
```

Nested producers use `context.WithTag(tag)`; consumers use `context.HasTag(tag)`. VMFramework
defines only `StateCloneTags.OwnerStateIncluded`, which its Container clone path adds when cloning
items together with their owner state. Projects may define their own tags without changing
VMFramework. Tags are process-local, must not be serialized, and are limited to 64 registrations.
Use `StateCloneContext.Empty` when a root clone has no tags.

Projects migrating from 1.x must replace `StateCloneHint` parameters with `StateCloneContext`,
replace `isNested = false` roots with `StateCloneContext.Empty`, and replace nested boolean
mutation with explicit `WithTag` production and `HasTag` consumption.

## Notes

- This repository is now a Unity Package Manager package root, not a full Unity project.
- `.meta` files are kept so Unity asset GUID references survive the move from `Assets/VMFramework` to a Git package.
- `JSONConverters` was removed from VMFramework; framework code no longer depends on `JSONConverterExt`.
