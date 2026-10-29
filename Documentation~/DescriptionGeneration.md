# Description generation and localized request ownership

DescriptionManager owns one registry. `Register(type, generate, generateAsync)` publishes both producers in the same generator product; both use the same registered LocalizedString. Synchronous callers retain their explicit synchronous contract. Asynchronous callers await `TryGetDescriptionAsync(type, token)` and consume its DescriptionResult. Missing types return Success=false; registered producer and binding failures propagate their original cause.

`OnGetDescription(type, BindingRequest)` is the single binding event. Components call `request.Bind(bind, bindAsync)` using their actual variable producer. Synchronous requests invoke the synchronous binder immediately. An asynchronous invocation owns accepted binding Tasks in registration order and joins them before formatting. If notification throws after accepting work, that work is still joined and notification and binding failures are both retained. Migrate subscribers and two-argument delegate registrations to these contracts; there is no compatibility event or second registry.

Nested localized names must finish with the requested LocaleOverride and FallbackState before root formatting. A locale-specific caller snapshots exact previous LocaleOverride references, applies its locale, awaits the manager result and restores those references in finally before releasing its objects. Do not write the global selected Locale or call a synchronous manager after preloading.

`LocalizedStringUtility.GetLocalizedTextAsync` acquires and joins actual Localization initialization before starting the string request. The native InitializeSynchronously authoring option can make initialization synchronous; an asynchronous initialization contract requires that option to be false in the consumer's Localization Settings asset. An accepted string request owns an extra acquired reference and joins actual completion even after caller cancellation. Native failure remains observable before exact-token cancellation propagates.

Unity auto-releases its original string request only for operations completed while playing. The helper observes that decision at the actual completion or failure boundary, including failure notification preceding deferred Completed. It releases its acquired reference and the original request reference only when native auto-release does not own it. It removes its exact Completed and diagnostic subscriptions in finally and retains Unity's original error handler. Localization initialization is joined before diagnostic subscription because initialization can configure that handler.

Native LocalizationBehaviour owns deferred releases during Play. Actual text completion or one NextFrame does not prove its queue drained. Native cancellation, Play exit and reload therefore need focused consumer qualification. No cache, synthetic completion, blocking WaitForCompletion or secondary request owner provides that evidence.

This migration changes the API, without changing serialized configuration fields, tables or resource GUIDs. Game-specific review plans, trace flushing and row matrices remain with their project tools; their schemas come from the current typed catalog.

## Editor formatting

`GetGeneralString` keeps its existing Editor/runtime phase boundary and accepts
the caller's positional arguments. In Edit Mode, `GetLocalizedStringInEditor`
reads the native Editor table for the explicit LocaleOverride or project Locale;
it never loads a table or selects a Locale through runtime Addressables. The
entry overload formats an already-loaded StringTableEntry with the installed
SmartFormatter, its table context, typed local variables and Locale formatter.
It owns one temporary parse/cache and releases its parsed children in finally;
it neither changes the entry's shared cache nor subscribes to variable changes.
Nested localized variables remain caller-owned and must already contain the
intended text for static authoring probes. Runtime asynchronous binding retains
the request ownership contract above.

The native failure witness is Edit string validation followed by Play with domain
reload disabled and scene reload enabled. Direct repeated Play succeeds, while
runtime string lookup during Edit can leave the next initialization waiting on
a completed Addressables dependency until the initializer times out. Static
authoring consumers must use this Editor formatter; changing Enter Play Mode
options, resetting cached handles or manually ticking ResourceManager does not
repair their phase ownership.

Static Cost Ledger before executable writes: one call resolves one collection,
one of its authored Locale tables and one entry, then performs one native format
operation. It adds no collection/content scan, runtime operation, callback or
persistent cache. The current consumer freezes 13 skill nodes, 9 typed sources,
2 Locales and at most 4 rank probes: 66 actual skill descriptions, plus 146
Console formats, 212 calls total. The largest source table is 102873 bytes, so
using the entire file as a conservative template bound admits at most 21809076
source-character visits across those calls; native parsing/formatting remains
owned by Unity. One call retains its source, parse/cache and result only; a
conservative 32 bytes per source character is below 4 MiB, within a 16 MiB
temporary authoring budget. Focused tests use two detached in-memory Locale
tables, strings below 512 characters and no Scene or asset writes. PASS. Native
runtime formatting and repeated Play after the full consumer review are separate
integration requirements.
