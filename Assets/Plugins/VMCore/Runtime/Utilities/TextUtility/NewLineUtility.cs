using System;
using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public static class NewLineUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string RemoveNewLine(this string input, int position)
        {
            if (position < 0 || position >= input.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(position), "Position is out of the string bounds.");
            }
           
            if (input[position] == '\r')
            {
                // Handle CRLF (\r\n) case
                if (position + 1 < input.Length && input[position + 1] == '\n')
                {
                    return input.Remove(position, 2);
                }
                
                // Handle CR case
                return input.Remove(position, 1);
            }

            if (input[position] == '\n')
            {
                // Handle CRLF (\r\n) case
                if (position > 0 && input[position - 1] == '\r')
                {
                    return input.Remove(position - 1, 2);
                }
                
                // Handle LF case
                return input.Remove(position, 1);
            }

            // If it's not a newline character, return the original string
            return input;
        }
    }
}