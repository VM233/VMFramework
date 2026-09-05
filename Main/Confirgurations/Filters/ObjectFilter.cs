using Sirenix.OdinInspector;

namespace VMFramework.Configuration
{
    public abstract class ObjectFilter : UnityEngine.ScriptableObject, IFilter
    {
        public abstract bool IsMatch(object obj);
    }
}
