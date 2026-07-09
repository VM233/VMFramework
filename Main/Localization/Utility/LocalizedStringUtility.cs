using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace VMFramework.Localization
{
    public static class LocalizedStringUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValid(this LocalizedString localizedString)
        {
            if (localizedString.TableReference.ReferenceType == TableReference.Type.Empty)
            {
                return false;
            }

            if (localizedString.TableEntryReference.ReferenceType == TableEntryReference.Type.Empty)
            {
                return false;
            }

            return true;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetGeneralString(this LocalizedString localizedString)
        {
            if (localizedString == null)
            {
                return null;
            }
                
#if UNITY_EDITOR
            if (Application.isPlaying == false)
            {
                return localizedString.GetLocalizedStringInEditor();
            }
#endif
            if (localizedString.TableReference.ReferenceType == TableReference.Type.Empty)
            {
                return null;
            }
            
            return localizedString.GetLocalizedString();
        }
    }
}