# Logic tick profiling

`LogicTickManager` owns tick advancement and its seven callback phases. Native CPU
Profiler markers follow the actual event dispatch order: `LogicTick.PreTick`,
`LogicTick.Tick`, `LogicTick.NextTickActions`, `LogicTick.PreSimulation`,
`LogicTick.Simulation`, `LogicTick.PostSimulation`, and `LogicTick.PostTick`.
The markers cover each phase's complete synchronous callbacks, including deferred
actions. Exceptions close the corresponding marker scope and retain the original
tick failure. Profiling does not change cadence, callback ordering, deferred-action
publication, recursion checks, or clock ownership.

Use the native recorded CPU frame to distinguish cold initialization, gameplay,
physics preparation, simulation and observation costs. Keep the original failing
input and build frozen while comparing independent cases. A warm replay alone
does not resolve a cold-start performance failure.

## Static Cost Ledger

There are exactly seven fixed phase scopes per admitted tick. Each scope uses a
static native `ProfilerMarker` handle and a value-type `AutoScope`, adding no
per-tick managed allocation, subscriber enumeration, reflection, file access,
cache or new lifecycle state. The existing deferred-action collection is visited
once with unchanged membership and scheduling. New work is O(7) per tick and
O(7) native handles per manager type, independent of callback count. All scopes
run on the existing tick caller's thread and close on normal or exceptional
return. PASS for the added instrumentation; subscriber and physics costs remain
owned and measured by their actual producers.

Validation uses the existing `VMFramework.Editor.Tests` logic-tick fixtures for
phase ordering, deferral across two ticks, active cadence, cadence changes and
recursive failure, plus a consuming project's native CPU capture at the original
performance witness. No performance threshold is changed.
