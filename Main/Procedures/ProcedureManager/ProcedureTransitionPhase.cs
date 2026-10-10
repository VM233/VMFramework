namespace VMFramework.Procedure
{
    public enum ProcedureTransitionPhase
    {
        Admitted,
        ExitingSourceLoading,
        ExitingSourceState,
        PublishingSourceExit,
        EnteringTargetLoading,
        EnteringTargetState,
        PublishingTargetEntry,
        RetiringLoading,
        RetiringState,
        PublishingRetirement,
        RestoringSourceLoading,
        RestoringSourceState,
        PublishingSourceRestoration
    }
}
