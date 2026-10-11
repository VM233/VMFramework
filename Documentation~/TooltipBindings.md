# Tooltip bindings

`TooltipGeneralSetting.defaultTooltipID` binds the default tooltip operations. Projects using only explicit tooltip IDs or a separate inspection presentation leave it empty. An unused default binding is valid configuration and does not produce an initialization warning.

`TooltipManager.Open(target, source, info)` and `CloseDefault(target)` consume `DefaultTooltipID` and throw a configuration exception when it is unset. Configure the actual tooltip panel before using those operations. Explicit `Open(tooltipID, ...)` and `Close(tooltipID, ...)` retain their own target and panel contracts.
