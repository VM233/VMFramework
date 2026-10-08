# Runtime configuration initialization

`GlobalSettingsFileLoadingInitializer` owns loading and validating global-setting
files during `VMFrameworkInitializationDoneProcedure`. Loading finishes in
`InitStart`. `GamePrefabRegisterInitializer` then collects, registers and validates
the GamePrefab reference graph in `PostInit`. Global settings and GamePrefabs check
their configuration in `InitComplete`, after that registry is available.

Configuration checks may resolve strict GamePrefab references. They must not rely
on the Editor's authoring registry, or construct a second registry. A missing
reference remains an error in the published runtime graph. The subsequent
`GameInitializationDoneProcedure` initializes general settings and gameplay
managers using the completed framework configuration.

## Cold Player validation

Validate a Player starting without any Editor registration state. A setting that
resolves a configured item during `CheckSettings` must find the item registered by
the runtime provider graph. Run the focused `InitializerManagerTests` fixture to
check ordered phases, concurrent actions within a phase, cancellation and failure
propagation. A successful Editor start alone does not cover cold registration.

## Static Cost Ledger

The validation-phase change adds no loop, registry, polling, allocation or task.
It relocates the existing single validation invocation after the existing
registration phase. Each global-setting file is still loaded and checked once.
The consuming DoomsdayDiary witness contains five global-setting files, thirty-four
general-setting assets and 310 registered GamePrefabs. There are five unchanged
root validation calls and no additional configuration checks; nested checks keep
their existing configured domains. Loading still allocates the same list of five
tasks and awaits its completion once. Registration and validation remain startup
work on the existing initializer scheduler. Incremental work and storage: zero.
Pass. Changing domains or adding validation work requires a new ledger.
