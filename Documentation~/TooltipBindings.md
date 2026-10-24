# Tooltip bindings

`TooltipGeneralSetting.defaultTooltipID` binds the default tooltip operations. Projects using only explicit tooltip IDs or a separate inspection presentation leave it empty. An unused default binding is valid configuration and does not produce an initialization warning.

`TooltipManager.Open(target, source, info)` and `CloseDefault(target)` consume `DefaultTooltipID` and throw a configuration exception when it is unset. Configure the actual tooltip panel before using those operations. Explicit `Open(tooltipID, ...)` and `Close(tooltipID, ...)` retain their own target and panel contracts.

Tooltip registration belongs to a concrete panel instance. Retirement removes that
identity even after the panel has deinitialized and cleared its modifiers. An empty
tooltip group is absent, so later explicit opens return false and closes visit only
live registrations. The tooltip manager retains its originating panel manager for
paired event subscription retirement; it does not resolve a replacement singleton.

The registry stores one entry per live tooltip panel and one group per distinct
tooltip ID, retaining registration order for the default open selection. Registration
is amortized O(1); retirement visits only that ID's entries, and CloseAll visits all
live entries once. No destroyed panel or empty pooled list is retained.
