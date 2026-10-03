# EventSystem input backend

ManagerCreator owns the framework EventSystem and its initial input module.
Unity's Active Input Handling setting supplies the compile-time backend:
ENABLE_INPUT_SYSTEM selects InputSystemUIInputModule, including projects enabling
both backends. Legacy-only projects create StandaloneInputModule. There is one
module creation path for each compiled product configuration.

Existing author-provided modules on the EventCore container retain their identity.
The framework does not change the project's input setting or create a second
EventSystem to compensate for a mismatched module.

Input Action ID authoring metadata and its Editor drawer remain available under
both runtime input configurations. The Input System package is a declared
dependency, so selecting legacy runtime input does not remove its authoring types.

ColliderMouseEventManager also reads pointer position and button state from the
compiled input backend. Button down, up and held values are sampled once for each
update and consumed by the existing click and drag transitions.

## Static Cost Ledger

The change adds one compile-time module selection at the existing creation site.
There is at most one native AddComponent call per EventCore initialization, with
the module's own action lifecycle. It adds no scan, loop, cache or per-frame work.
Collider pointer input adds no data-dependent iteration: nine button reads per
update and at most two position reads across the existing 2D/3D detection calls.
Local scalar values allocate no managed storage and retain no cross-frame cache.
Existing physics-scene and trigger iteration bounds, allocations, registration
costs and event transitions are unchanged. Budget: pass.

## Validation

Use the consumer's actual input setting and run its normal scene bootstrap.
Verify that the EventCore container owns the selected module, pointer interaction
works, and the Console contains no disabled legacy-input reads. Compile both
backend branches against Unity's declared support range. A test-created gameplay
Prefab alone does not prove normal framework startup.
