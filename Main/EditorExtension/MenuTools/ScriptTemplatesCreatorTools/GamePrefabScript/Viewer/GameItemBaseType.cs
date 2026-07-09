#if UNITY_EDITOR
namespace VMFramework.Editor
{
    public enum GameItemBaseType
    {
        GameItem,
        ControllerGameItem,
        VisualGameItem,
#if FISHNET
        UUIDGameItem,
        UUIDVisualGameItem
#endif
    }
}
#endif