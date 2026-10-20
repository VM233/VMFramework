# Runtime configuration initialization

GlobalSettingsFileLoadingInitializer loads and validates actual global-setting files during VMFrameworkInitializationDoneProcedure. Loading finishes in InitStart. GamePrefabRegisterInitializer then collects, registers and validates the GamePrefab reference graph in PostInit. Global settings and GamePrefabs check their configuration in InitComplete, after that registry is available. GameInitializationDoneProcedure adopts those completed configuration products for gameplay managers.

Configuration checks may resolve strict GamePrefab references. They must use the runtime provider graph, rather than the Editor authoring registry or a second registration table. A missing reference remains the original configuration error. Cold Player validation must exercise startup without an Editor registry; successful Editor startup alone is not that proof.

InitializerManager invokes and awaits each accepted action source exactly once in its captured order. The action token already links sibling, caller and timeout cancellation; cancellation does not detach a pending source. After the source finishes, the manager checks those actual tokens before adopting success. A late source failure remains the first failure; an independently requested forced cancellation is recorded after that join through its actual token boundary. Original source exceptions and sibling cancellation callback failures remain observable. A foreign cancellation is identified by the token and exception actually received from the source, rather than an exception injected into a producer that only retains cancellation tokens. Procedure entry/exit awaits the actual initialization product and retires it under the same transition lifetime. See ProcedureTransitionLifetime.md for cancellation identity, phase failure and request ownership.

The validation placement does not add a registry, polling loop or another loading pass. Each captured global-setting file is loaded and checked once, with one original task per file. Exact asset counts belong to the consuming project's current registered graph and cannot be copied from another project's historical checkpoint. The focused VMFramework.Editor.Tests.InitializerManagerTests fixture covers ordered sources and failure/cancellation ownership; cold configuration registration and native consumer startup remain separate integration witnesses.
## Initialization CPU observation

Native CPU Profiler markers named `Collect initialization: <type>` and
`Initialize: <type>.<method>` identify action construction and synchronous action
invocation. The marker ends when the handler returns its UniTask; it does not
claim the cost of an asynchronous continuation or waiting frames. Existing order,
sibling completion, error and cancellation ownership are unchanged. Each scope
adds constant work and retains no timing history or native resource.

## Cooperative runtime validation

Runtime registration, validation and unique panel creation consume an
InitializationWorkSlice with a4ms continuous-work budget. Budget expiry awaits an
actual next PlayerLoop frame on the existing action token; cancellation and source
retirement remain owned by InitializerManager. This is a scheduling boundary,
with no delay/minimum presentation duration and no worker-thread Unity API calls.
An indivisible native operation cannot be preempted by this budget.

IAsyncCheckableConfig declares that a configuration owns a cancellable incremental
runtime check. The runtime initializer awaits that source instead of calling the
synchronous authoring check. Both must consume the same validation semantics;
the interface does not authorize omitting checks. A registry may be populated
across frames in PostInit; dependent initialization remains after its completed
order. The actual action sources and all sibling tasks join before procedure
adoption. No completed/partial registry is substituted for a failed source.

Static Cost Ledger: registration and validation remain single linear traversals
of the frozen registered graph; unique-panel creation remains a single traversal
and creates every unique config once. Each active action adds one constant-size
slice and at most one pending frame wait; no history, worker queue or cache is
introduced. Content-specific incremental checks own their bounded atomic steps.

Cold runtime type discovery freezes the current AppDomain assembly array and
checks each assembly once using ReflectionUtility's existing type cache. It keeps
the original type order, inclusion and instantiability rules. There is one
constant-size work slice and one result list bounded by the captured type domain.
No second reflection cache is added. GameItemEvents receiver discovery is an
explicit preceding initialization order, so subscriptions cannot observe a
partially discovered receiver set. Global-setting files may explicitly implement
IAsyncCheckableConfig to distribute their actual general-setting checks across
frames; file-specific checks remain that implementer's responsibility.
