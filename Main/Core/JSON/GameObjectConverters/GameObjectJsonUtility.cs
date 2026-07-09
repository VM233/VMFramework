using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VMFramework.Core.JSON
{
    public static class GameObjectJsonUtility
    {
        private static readonly JObject wrapperObjectToken = new();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PopulateToGameObject<TController>(this JsonSerializer serializer, JToken token,
            TController gameObject)
        {
            wrapperObjectToken[nameof(ControllerWrapper<TController>.gameObject)] = token;

            var wrapper = new ControllerWrapper<TController>(gameObject);
            serializer.Populate(wrapperObjectToken.CreateReader(), wrapper);
        }
    }
}