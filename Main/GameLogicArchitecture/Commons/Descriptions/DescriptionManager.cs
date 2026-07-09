using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Localization;
using VMFramework.Core;

namespace VMFramework.GameLogicArchitecture
{
    public class DescriptionManager : MonoBehaviour
    {
        public delegate bool GenerateHandler(out string description);

        public delegate void GetHandler(string type);

        public event GetHandler OnGetDescription;

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        protected readonly Dictionary<string, GenerateHandler> generators = new();

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        protected readonly Dictionary<string, HashSet<LocalizedString>> localizedStringsLookup = new();
        
        protected readonly HashSet<LocalizedString> allLocalizedStrings = new();

        public virtual bool TryGetDescription(string type, out string description)
        {
            if (generators.TryGetValue(type, out var generator))
            {
                OnGetDescription?.Invoke(type);
                return generator(out description);
            }

            description = null;
            return false;
        }

        public virtual bool TryGetLocalizedStrings(string key,
            out IReadOnlyCollection<LocalizedString> localizedStrings)
        {
            if (localizedStringsLookup.TryGetValue(key, out var strings))
            {
                localizedStrings = strings;
                return true;
            }

            localizedStrings = null;
            return false;
        }

        public virtual IReadOnlyCollection<LocalizedString> GetLocalizedStrings()
        {
            return allLocalizedStrings;
        }

        public virtual void Register(string type, GenerateHandler generator)
        {
            if (type.IsNullOrEmpty())
            {
                UnityEngine.Debug.LogError($"Type cannot be null or empty. Attempted to register {generator.Method.Name}.");
                return;
            }

            if (generators.TryAdd(type, generator) == false)
            {
                UnityEngine.Debug.LogWarning($"Type {type} already exists in the description manager. " +
                                    $"Attempted to register {generator.Method.Name}.");
            }
        }

        public virtual void Register(string type, LocalizedString localizedString)
        {
            if (type.IsNullOrEmpty())
            {
                UnityEngine.Debug.LogError(
                    $"Type cannot be null or empty. Attempted to register localized string with empty Type.");
                return;
            }

            var strings = localizedStringsLookup.GetOrCreate(type);
            strings.Add(localizedString);
            
            allLocalizedStrings.Add(localizedString);
        }
    }
}