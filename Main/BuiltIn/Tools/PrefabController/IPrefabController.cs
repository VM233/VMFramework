namespace VMFramework.Tools
{
    public interface IPrefabController<TPrefab>
    {
        public TPrefab Prefab { get; }

        public void SetPrefab(TPrefab prefabObject);
    }
}