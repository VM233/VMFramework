using Sirenix.OdinInspector;

namespace VMFramework.Configuration
{
    public abstract class ObjectFilter : SerializedScriptableObject, IFilter
    {
        public abstract bool IsMatch(object obj);
    }
}