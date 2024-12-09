using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public static class CharUtility
    {
        /// <summary>
        /// 判断字符是否为单词分隔符，包括空格、标点符号（逗号句号之类的）、符号(@、$、^、+、=之类的)
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsWordDelimiter(this char c)
        {
            return char.IsWhiteSpace(c) || char.IsSymbol(c) || char.IsPunctuation(c);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsLetter(this char c)
        {
            return char.IsLetter(c);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsLetterOrDigit(this char c)
        {
            return char.IsLetterOrDigit(c);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsLower(this char c) => char.IsLower(c);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsUpper(this char c) => char.IsUpper(c);
    }
}