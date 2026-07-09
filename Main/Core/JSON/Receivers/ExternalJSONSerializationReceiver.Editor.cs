#if UNITY_EDITOR
namespace VMFramework.Core.JSON
{
    public sealed partial class ExternalJSONSerializationReceiver
    {
        private void Reset()
        {
            foreach (var receiver in transform.GetComponentsInChildren<IJSONSerializationReceiver>())
            {
                if (ReferenceEquals(receiver.gameObject, gameObject) || serializedObjects.Contains(receiver.gameObject))
                {
                    continue;
                }
                
                serializedObjects.Add(receiver.gameObject);
            }
        }
    }
}
#endif