#if FISHNET
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using FishNet.CodeGenerating;
using FishNet.Serializing;
using UnityEngine;

namespace VMFramework.Network
{
    public static class SerializationUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [NotSerializer]
        public static void FastWriteDictionary<TKey, TValue>(this Writer writer,
            [DisallowNull] IReadOnlyDictionary<TKey, TValue> dictionary)
        {
            writer.WriteInt32(dictionary.Count);
            foreach (var pair in dictionary)
            {
                writer.Write(pair.Key);
                writer.Write(pair.Value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [NotSerializer]
        public static void FastReadDictionary<TKey, TValue>(this Reader reader,
            [DisallowNull] Dictionary<TKey, TValue> dictionary)
        {
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var key = reader.Read<TKey>();
                var value = reader.Read<TValue>();
                dictionary[key] = value;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [NotSerializer]
        public static IEnumerable<(TKey key, TValue value)> FastReadDictionaryAsEnumerable<TKey, TValue>(
            this Reader reader)
        {
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var key = reader.Read<TKey>();
                var value = reader.Read<TValue>();
                yield return (key, value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [NotSerializer]
        public static void WriteNullableVector3Int(this Writer writer, Vector3Int? vector)
        {
            if (vector.HasValue)
            {
                writer.WriteBoolean(true);
                writer.WriteVector3Int(vector.Value);
            }
            else
            {
                writer.WriteBoolean(false);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [NotSerializer]
        public static Vector3Int? ReadNullableVector3Int(this Reader reader)
        {
            if (reader.ReadBoolean())
            {
                return reader.ReadVector3Int();
            }

            return null;
        }
    }
}
#endif