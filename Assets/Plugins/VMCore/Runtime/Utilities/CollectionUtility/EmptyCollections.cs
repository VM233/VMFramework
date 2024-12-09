using System.Collections.Generic;

namespace VMFramework.Core
{
    public static class EmptyCollections<T>
    {
        public static readonly IReadOnlyList<T> emptyList = new List<T>();
    }

    public static class EmptyCollections<T1, T2>
    {
        public static readonly IReadOnlyDictionary<T1, T2> emptyDictionary = new Dictionary<T1, T2>();
    }
}