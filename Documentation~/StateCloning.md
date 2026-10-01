# State cloning and native initialization

`GetClone(source, context)` delegates to GameItemManager's clone producer. The
producer rents through the existing native pool, copies source state with the
caller-provided StateCloneContext, then publishes OnGameItemCreated. If copying
fails, it returns the unpublished rental and propagates the original exception.

GameItemManager.CurrentInitializationKind is available only inside native
OnCreate/OnGet initialization. Ordinary Get, typed Get and PrewarmUntil declare
AuthoredDefaults; Clone declares ClonedState. Nested native rentals restore the
parent context even on exceptions. No context is active after native initialization.
Initialization remains synchronous and uses the pool's existing thread boundary.

Owned child providers that will copy their state can use ClonedState to leave
the child empty until their state-clone receiver installs the source clone.
Providers excluded from state copying must still initialize authored defaults.
Reference providers keep their independent ownership semantics. Do not query
the initialization kind outside native initialization or use it as a gameplay mode.

Cloning retains the original StateCloneContext tags and native owner adoption.
It does not prewarm, postpone copying, reuse the source object or create a second pool.

GameItemReferenceManager owns one initialized reference per ID for its lifetime.
Initialization callbacks run before cache publication. Controller references are
then made inactive as complete roots, which removes all their native physics and
gameplay updates while preserving live data and identity. Managed references
retain their normal data lifetime. Callback failure returns the unpublished
rental and propagates the original error without publishing a cache entry.
Ordinary Get and Clone rentals remain separate objects with their own activation;
reference inactivity is not copied into their pool activation state.
