# Runtime configuration initialization

GlobalSettingsFileLoadingInitializer loads and validates actual global-setting files during VMFrameworkInitializationDoneProcedure. Loading finishes in InitStart. GamePrefabRegisterInitializer then collects, registers and validates the GamePrefab reference graph in PostInit. Global settings and GamePrefabs check their configuration in InitComplete, after that registry is available. GameInitializationDoneProcedure adopts those completed configuration products for gameplay managers.

Configuration checks may resolve strict GamePrefab references. They must use the runtime provider graph, rather than the Editor authoring registry or a second registration table. A missing reference remains the original configuration error. Cold Player validation must exercise startup without an Editor registry; successful Editor startup alone is not that proof.

InitializerManager invokes and awaits each accepted action source exactly once in its captured order. The action token already links sibling, caller and timeout cancellation; cancellation does not detach a pending source. After the source finishes, the manager checks those actual tokens before adopting success. A late source failure remains the first failure; an independently requested forced cancellation is recorded after that join through its actual token boundary. Original source exceptions and sibling cancellation callback failures remain observable. A foreign cancellation is identified by the token and exception actually received from the source, rather than an exception injected into a producer that only retains cancellation tokens. Procedure entry/exit awaits the actual initialization product and retires it under the same transition lifetime. See ProcedureTransitionLifetime.md for cancellation identity, phase failure and request ownership.

The validation placement does not add a registry, polling loop or another loading pass. Each captured global-setting file is loaded and checked once, with one original task per file. Exact asset counts belong to the consuming project's current registered graph and cannot be copied from another project's historical checkpoint. The focused VMFramework.Editor.Tests.InitializerManagerTests fixture covers ordered sources and failure/cancellation ownership; cold configuration registration and native consumer startup remain separate integration witnesses.
## Initialization CPU observation

IProcedureManager.Initialization exposes the active read-only initialization
manager. A running action may declare an IInitializationProgressSource through
InitializationAction.progressSource. Its immutable Progress reports actual completed/total units for that source's
current batch. GamePrefab validation publishes its frozen registry count before
processor discovery and advances only after each item's complete check succeeds.
Registration is a prerequisite and does not publish a second counted batch.
The pair measures batch work, not whole-application time or a predicted duration.
Consumers must use the action's actual Running status; cancellation and failure
do not manufacture completion. No extra registry traversal, timer or global
progress singleton is introduced. Static cost is O(1) per existing item and one
pair per source; observation is bounded by the current order's existing actions.

Native CPU Profiler markers named `Collect initialization: <type>` and
`Initialize: <type>.<method>` identify action construction and synchronous action
invocation. The marker ends when the handler returns its UniTask; it does not
claim the cost of an asynchronous continuation or waiting frames. Existing order,
sibling completion, error and cancellation ownership are unchanged. Each scope
adds constant work and retains no timing history or native resource.

## Cooperative runtime validation

Runtime registration, validation and unique panel creation consume an
InitializationWorkSlice with a10ms continuous-work budget. Budget expiry awaits an
actual next PlayerLoop frame on the existing action token; cancellation and source
retirement remain owned by InitializerManager. This is a scheduling boundary,
with no delay/minimum presentation duration and no worker-thread Unity API calls.
An indivisible native operation cannot be preempted by this budget. All work-slice
handles share one clock/credit for the current Time.frameCount; a late-created
handle cannot reset that credit. Subsystem registration retires the clock before
a new Play lifetime, including native no-domain-reload entry. Each runtime caller
checks credit before its next indivisible unit. Awaiters that resume after another
source consumed the new frame continue waiting on their actual cancellation token.

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

Incremental runtime discovery used by registration and event-receiver owners
freezes the current AppDomain assembly array and
checks each assembly once using ReflectionUtility's existing type cache. It keeps
the original type order, inclusion and instantiability rules. There is one
constant-size work slice and one result list bounded by the captured type domain.
No second reflection cache is added. GameItemEvents receiver discovery is an
explicit preceding initialization order, so subscriptions cannot observe a
partially discovered receiver set. Global-setting files may explicitly implement
IAsyncCheckableConfig to distribute their actual general-setting checks across
frames; file-specific checks remain that implementer's responsibility.

## Action admission and atomic batches

ProcedureManager publishes six fixed native profiler markers for request run,
synchronous loading start, state entry/exit and entry/exit publication. These
measure the real transition producer rather than inferring it from menu drawing.
Every scope is synchronous; none crosses an await or changes state/event atomicity.
The marker metadata is constant (six handles, below4KiB); scopes add constant
work at the existing transition boundaries, with no domain traversal, allocation
per sample, frame delay, retained history or asynchronous lifetime. This diagnostic
admission is pass; cold first-entry responsiveness remains a separate acceptance.

Cold startup witness DD-CONT-30 records434.921ms in native frame6090 while the initializer manager admits multiple synchronous sources in one frame. The shared10ms work credit must also gate each accepted action across every captured order. Same-order asynchronous sources remain concurrent; accepted sources must join even when caller/timeout cancellation interrupts admission. Pending sources that were never invoked stay Pending.

Static Cost Ledger for this repair: freeze this consumer at at most2048 registered definitions (actual1045),256 managers,256 general settings and64 contributed action hooks per owner plus64 standalone hooks. This conservative source-audited envelope gives at most163904 already-captured actions, with the original ordered grouping, execution products and task references (under64MiB total metadata allowance). No new domain cap or content truncation is introduced. New admission performs one constant work-credit check per existing invocation; no extra candidate traversal or expensive operation is added. Only one pending admission frame await and one constant slice are added per active manager, below4KiB incremental live state. The original accepted-source list and linked cancellation lifetime remain authoritative.

GameEvent registration remains one pass over at most2048 registered event definitions. Sprite preload remains one pass over at most2048 sprite presets and32 integer-enum flag positions: at most65536 original sprite requests and the same number of added constant credit checks. Each source keeps one slice and one frame await, below4KiB incremental live state. No second registry, native object, pool or output cache is introduced. The shared slice observes actual PlayerLoop frame identity in Play and actual EditorApplication.update identity in Edit; the Editor epoch callback has one static owner and paired reload/quit retirement. No timer, target frame rate, initialization deadline or accepted semantics changes.

Pass for unchanged finite iteration products and constant incremental state on this frozen consumer. The10ms credit bounds admission between indivisible units; it does not claim that a native call or first managed invocation can be preempted. Cold first-Play native frame acceptance, Editor progress and exact failure/cancellation lifecycle cases remain required before claiming responsiveness or integration passes.

The UnitySetUp initial-credit fixture must be a compiler-generated iterator that joins its actual budget task while yielding native Editor frames. Unity Test Framework resets that setup iterator program counter between cases; returning UniTask custom IEnumerator directly is outside that lifecycle contract. Setup owns and cancels its pending task lifetime on retirement. This repair adds one iterator/task/cancellation source per sequential case (eleven cases, below8KiB total metadata), with the same credit acquisition and original assertions. No production scheduling, input, timeout or acceptance changes.

## Synchronous global-setting load stages

Native C25 first-entry evidence attributes512.707ms to GlobalSettingsFileLoadingInitializer.OnInitStart, with495.584ms opaque self time. That identifies the real synchronous loading owner; it does not establish JIT, reflection, Addressables or a particular loader as its cause. File metadata discovery remains synchronous for the TypeConfigs authoring product. Runtime loader discovery now uses the incremental preparation source described below.

The9.1.58 observation marks each synchronous LoadGlobalSettingFile dispatch. GlobalSettingFileManager marks file-type discovery, synchronous loader-preparation fragments, loader selection and the call that begins the actual selected loader. No scope crosses an await. Their public marker-name constants define the observation contract; nested inclusive times overlap and must not be summed. A first managed invocation before a scope begins remains visible in its parent, so missing child cost is evidence rather than a JIT claim. The original file tasks, inclusion/order, chosen loader, errors and final adoption remain authoritative; loader preparation now joins before file-task admission.

Static Cost Ledger before executable writes: five constant domain-lifetime native marker handles (<4KiB metadata), one value-type scope per existing synchronous boundary. There is no added traversal, reflection, serialization, file/native asset operation, cache, thread, timer, wait, load request or retained sample history. For the frozen conservative envelope of256 global settings, dispatch/loader-discovery/selection/load-begin contribute at most1024 scopes plus one file-type discovery scope, at their existing invocations. Additional allocation per scope is zero after marker initialization. PASS for unchanged producer workloads and constant observation state. Native source/cancel retirement and current first-Play timing are separate required integration checks; these markers alone are not a responsiveness repair.

## Loader preparation and complete catalog publication

C26 first-entry native frame121 isolates344.005ms/4.2MB in synchronous DiscoverLoaders across five setting-file loads, versus4.689ms in their actual loader calls. The runtime initializer must prepare loader implementations through the existing InitializationTypeDiscovery.FindAsync source before dispatching setting-file loads. That source captures the same assembly order, uses the existing ReflectionUtility cache and preserves inclusion/instantiability semantics while paying the existing work-slice boundary between assemblies.

PrepareLoadersAsync receives the actual InitStart cancellation token. It joins type discovery, builds a local complete loader dictionary while reusing existing actual loader instances, checks that token before publication and replaces the catalog once. Cancellation and constructor failure preserve the prior published dictionary. File loads consume that published product; the sole normal runtime initializer awaits preparation before admitting any of them. File metadata and TypeConfigs authoring keep their existing owner. Loader discovery markers cover synchronous dispatch/instance construction; actual per-assembly discovery keeps its own existing markers. No profiler scope crosses an await.

Static Cost Ledger before executable writes: C26 freezes five global settings and one production loader implementation; the focused fixture adds one plain test implementation. Existing type-domain work is one captured AppDomain assembly array and its same assembly-local cached queries through the already-admitted InitializationTypeDiscovery owner, replacing five synchronous discovery requests; no new type/assembly search axis or reflection cache. The existing source performs one work-credit check per original assembly and owns at most one pending frame wait. Instance adoption traverses the returned implementation list once and copies the actual retained loader dictionary once (L=1 production/L=2 fixture, <=4 references across prior/new dictionaries plus the one new test instance). One additional immutable-list reference, one local dictionary and the original action token are retained until adoption, below4KiB in this frozen input. Existing order, selected loader, original file tasks and native asset-call counts are preserved; preparation precedes all five file requests. Focused four-case fixtures run sequentially and restore their exact dictionary/constructor callback in finally/teardown; no asset, file, view, gameplay source or native loader call is created by the fixture. PASS for the unchanged discovery domain and bounded added publication work. Assembly-local reflection remains an indivisible operation, so the100ms native first-Play gate remains separate and cannot be inferred from this scheduling change.


## Native Editor build boundary

The Editor initializer and the native Player build must not own authoring work
concurrently. One earliest-order BuildPlayerProcessor callback, before Addressables preparation,
cancels the current
Editor initialization token and queues the existing initialization scheduler.
The scheduler admits only stable Edit Mode outside BuildPipeline.isBuildingPlayer
and after the prior task has retired. Success and failed builds use the same
next Editor update; no postprocess-only cleanup or extended timeout is required.
The ordinary two-minute initialization deadline remains unchanged.

Static Cost Ledger: one existing cancellation source and one deduplicated update
delegate; one O(1) build-boundary invocation and one ordinary initializer run after
native build retirement. No new scan, cache, native object or window is introduced.
The focused test adds one test-only initializer with one token-owned pending task,
then cancels through the real prebuild callback and joins both old/new native
initialization lifetimes. All fixture state retires in finally. PASS: constant
incremental ownership, existing discovery budget and unchanged native build count.
