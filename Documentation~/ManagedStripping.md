# Managed stripping and reflection discovery

Framework discovery interfaces carry Unity's `RequireImplementors` annotation.
The IGameItem factory contract carries it too: GenerateGameItem uses the
configured GameItemType through Activator rather than a statically visible new.
The linker retains their concrete implementation identities and instance
constructors while continuing to strip unused members. Registration attributes
carry `RequireAttributeUsages`, so High stripping retains the start procedure,
initializer registration, default transitions, global-setting loader selection
and prefab auto-registration metadata consumed by their runtime owners.

ProcedureManager publishes one initialization owner at construction. Collection
resets this owner through its normal Set contract before publishing new groups;
it does not replace the progress interface or discard active initialization.
An original discovery exception remains the primary failure.

This is a dependency declaration on the existing producer contracts. There is no
parallel runtime registry, extra assembly scan, generated project link file or
whole-assembly preservation. Custom processor interfaces supplied as a new
reflection discovery contract must declare the same native dependency. Explicit
processor instances keep their ordinary static references.

Static cost ledger: annotations add no runtime loops, allocations, callbacks,
caches or publication states. The existing discovery/lifecycle bounds remain
unchanged. The linker marks only implementation families of contracts actually
used by the Player and the usage sites of their registration attributes.

Qualification requires an actual High-stripping IL2CPP Player. An Editor test
cannot exercise removal by UnityLinker. Inspect the original and stripped
assemblies to establish that a reflection-only start procedure is retained, then
start the real Player through its menu and verify initialization, content
registration, session entry and save/load. Keep compiler/metadata evidence
separate from real runtime acceptance.
