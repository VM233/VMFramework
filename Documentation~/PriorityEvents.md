# Priority events

PriorityEvents owns unique delegate registrations and their numeric priorities.
Add of an existing delegate and Remove of an absent delegate are idempotent.
Null delegates are invalid inputs and raise ArgumentNullException at admission.
Count and the collection enumerator describe individual registrations.
GetCombinedCallbacks returns the live, read-only priority-group view: one
multicast delegate per ascending priority, with registration order preserved
inside each group. A saved multicast delegate is an immutable invocation
snapshot. Mutation invalidates an active group-view enumerator, as before.

The registration list is the only membership authority. A group publishes its
combined delegate lazily when first enumerated, then reuses it until Add or
Remove changes that group. Clear retires all groups and delegates. Balanced
composition preserves invocation order, arguments, return values and exception
propagation while avoiding repeated prefix copies during every registration.
Remove retires the exact registered delegate, including an overlapping multicast
registration, rather than removing a matching subsequence inside another one.
Pool creation/get/return/clear, icon evaluation and parameterless game-event
propagation all consume the same existing GetCombinedCallbacks contract.

Entry/producer/sole owner: PriorityEvents.Add, Remove and Clear. Product: each
group's immutable combined delegate. Publication/adoption: the group's first
view enumeration after a membership change. Consumers: ControllerGameItem,
IconManager and ParameterlessGameEvent. Invalid input fails at admission;
callback failures propagate from their existing invocation boundary. No second
event dispatcher, changed pool lifetime, delayed gameplay or native warmup.

Static Cost Ledger before executable writes: for N registrations containing M
total multicast invocation entries, eager sequential composition copies prefixes
with quadratic cost for single-entry callbacks. Registration now uses dictionary
lookup, a sorted priority lookup and amortized list append; it does not compose
a delegate. One changed group's publication performs N-1 combinations across
ceil(log2(N)) balanced levels, copying at most M*ceil(log2(N)) invocation entries.
Its stack depth is ceil(log2(N)); unchanged publication reuses its delegate.
Remove performs one linear list search and invalidation. All storage is bounded
by current registration counts: O(N+M), with no static cache or history.

The frozen native Jelly Eye witness has 77 property providers per new actor.
For 77 single-entry callbacks, the previous prefix-copy bound is 3,002 entries
per group; the balanced bound is 539. The focused 256-registration test bounds
composition to 255 combinations, 2,048 copied entries, depth 8 and less than
64 KiB of added group/list/composition storage including Mono delegate headers.
No new native call, main-thread loop domain, timer, forced collection, scenario
or calibration gate. PASS for the frozen workload and the stated linear storage
and N-log-N publication bounds; native allocation and original performance
acceptance are verified independently in the consumer.

Tests cover priorities, insertion order, duplicate registration, exact multicast
removal, absent removal, live views, immutable old snapshots, group-local cache
invalidation, callback exceptions, mutation during enumeration, clear/reuse and
256 independent callbacks. Consumer validation must compile/reload the adopted
revision, review the actual sources, run this focused fixture and replay the
unchanged summon request with all Console, source and retirement gates intact.
