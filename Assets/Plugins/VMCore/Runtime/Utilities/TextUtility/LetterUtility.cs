using System.Linq;
using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public static class LetterUtility
    {
        public static bool HasLetter(this string str)
        {
            return str.Any(char.IsLetter);
        }

        public static bool HasUppercaseLetter(this string str)
        {
            return str.Any(char.IsUpper);
        }
        
        public static bool HasLowercaseLetter(this string str)
        {
            return str.Any(char.IsLower);
        }
        
        public static string CapitalizeFirstLetter(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            
            return char.ToUpper(str[0]) + str[1..];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsLowercaseAndHyphenOnly(this string str)
        {
            foreach (char c in str)
            {
                if (c != '-' && c.IsLower() == false)
                {
                    return false;
                }
            }
            
            return true;
        }
    }
}