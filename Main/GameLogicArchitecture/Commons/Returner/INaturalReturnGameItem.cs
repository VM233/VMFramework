namespace VMFramework.GameLogicArchitecture
{
    public interface INaturalReturnGameItem
    {
        // True transfers completion ownership to the item's native playback.
        bool TryBeginNaturalReturn();
    }
}
