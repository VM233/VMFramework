using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace VMFramework.Core
{
    public static class PathUtility
    {
        #region Path Combine

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string PathCombine(this string pathA, string pathB)
        {
            return Path.Combine(pathA, pathB).Replace('/', '\\');
        }

        #endregion

        #region Make Relative Path

        /// <summary>
        /// 是否可以将绝对路径转换为相对路径
        /// </summary>
        /// <param name="absoluteParentPath"></param>
        /// <param name="absolutePath"></param>
        /// <returns></returns>
        public static bool CanMakeRelative(string absoluteParentPath, string absolutePath)
        {
            absoluteParentPath.AssertIsNotNull(nameof(absoluteParentPath));
            absolutePath.AssertIsNotNull(nameof(absolutePath));

            absoluteParentPath = absoluteParentPath.Replace('\\', '/').Trim('/');
            absolutePath = absolutePath.Replace('\\', '/').Trim('/');
            return Path.GetPathRoot(absoluteParentPath).Equals(
                Path.GetPathRoot(absolutePath), StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// 将绝对路径转换为相对路径
        /// </summary>
        /// <param name="absoluteParentPath"></param>
        /// <param name="absolutePath"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static string MakeRelative(string absoluteParentPath, string absolutePath)
        {
            absoluteParentPath = absoluteParentPath.TrimEnd('\\', '/');
            absolutePath = absolutePath.TrimEnd('\\', '/');
            string[] array = absoluteParentPath.Split('/', '\\');
            string[] array2 = absolutePath.Split('/', '\\');
            int num = -1;
            for (int i = 0;
                 i < array.Length && i < array2.Length && array[i].Equals(array2[i],
                     StringComparison.CurrentCultureIgnoreCase);
                 i++)
            {
                num = i;
            }

            if (num == -1)
            {
                throw new InvalidOperationException("No common directory found.");
            }

            StringBuilder stringBuilder = new();
            if (num + 1 < array.Length)
            {
                for (int j = num + 1; j < array.Length; j++)
                {
                    if (stringBuilder.Length > 0)
                    {
                        stringBuilder.Append('/');
                    }

                    stringBuilder.Append("..");
                }
            }

            for (int k = num + 1; k < array2.Length; k++)
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append('/');
                }

                stringBuilder.Append(array2[k]);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 尝试将绝对路径转换为相对路径
        /// </summary>
        /// <param name="absoluteParentPath">父绝对路径</param>
        /// <param name="absolutePath">要转换的绝对路径</param>
        /// <param name="relativePath">相对路径结果</param>
        /// <returns></returns>
        public static bool TryMakeRelative(this string absoluteParentPath, string absolutePath, out string relativePath)
        {
            if (CanMakeRelative(absoluteParentPath, absolutePath))
            {
                relativePath = MakeRelative(absoluteParentPath, absolutePath);
                return true;
            }

            relativePath = null;
            return false;
        }

        #endregion

        #region Try Make Path Start With

        /// <summary>
        /// 尝试将路径以startWith开头
        /// 例如：将"E:/myGame/Assets/StreamingAssets/config.json"转换为"StreamingAssets/config.json"
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="startWith"></param>
        /// <param name="relativePath"></param>
        /// <param name="resultSeparator"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryMakeStartWith(this string fullPath, string startWith, out string relativePath,
            char resultSeparator)
        {
            var parts = fullPath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            int startIndex = -1;
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i] == startWith)
                {
                    startIndex = i;
                }
            }

            if (startIndex == -1)
            {
                relativePath = null;
                return false;
            }

            relativePath = string.Join(resultSeparator, parts[startIndex..]);
            return true;
        }

        #endregion

        #region Get Relative Path

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetRelativePath(this string path, string relativeTo)
        {
            return Path.GetRelativePath(relativeTo, path);
        }

        #endregion

        #region Replace To

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ReplaceToDirectorySeparator(this string path)
        {
            return path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ReplaceToAltDirectorySeparator(this string path)
        {
            return path.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        #endregion

        #region Split Path

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string[] SplitPath(this string path)
        {
            path = path.TrimEndDirectorySeparator();
            
            if (Path.DirectorySeparatorChar == Path.AltDirectorySeparatorChar)
            {
                return path.Split(Path.DirectorySeparatorChar);
            }

            return path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        #endregion

        #region Trim End Directory Separator

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string TrimEndDirectorySeparator(this string path)
        {
            if (Path.DirectorySeparatorChar == Path.AltDirectorySeparatorChar)
            {
                return path.TrimEnd(Path.DirectorySeparatorChar);
            }

            return path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string TrimDirectorySeparator(this string path)
        {
            if (Path.DirectorySeparatorChar == Path.AltDirectorySeparatorChar)
            {
                return path.Trim(Path.DirectorySeparatorChar);
            }
            
            return path.Trim(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        #endregion

        #region Path Contains

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool PathContains(this string path, string subPath)
        {
            subPath = subPath.ReplaceToDirectorySeparator().TrimEndDirectorySeparator();
            
            return path.ReplaceToDirectorySeparator().IndexOf(subPath, StringComparison.Ordinal) >= 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool PathEndsWith(this string path, string endWith)
        {
            path = path.ReplaceToDirectorySeparator().TrimEndDirectorySeparator();
            endWith = endWith.ReplaceToDirectorySeparator().TrimEndDirectorySeparator();

            return path.EndsWith(endWith, StringComparison.Ordinal);
        }

        /// <summary>
        /// Example 1:
        /// Path : "A/B/C/D"
        /// EndWith : "C/D"
        /// MatchingCount : 2
        ///
        /// Example 2:
        /// Path : "A/B/C"
        /// EndWith : "C/D"
        /// MatchingCount : 1
        /// </summary>
        /// <param name="path"></param>
        /// <param name="endWith"></param>
        /// <param name="matchingCount"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool PathEndsWith(this string path, string endWith, out int matchingCount)
        {
            path = path.TrimEndDirectorySeparator();
            endWith = endWith.TrimDirectorySeparator();

            var endWithParts = endWith.SplitPath();
            var pathParts = path.SplitPath();
            
            var firstEndWithPart = endWithParts[0];

            if (pathParts.TryGetIndex(firstEndWithPart, out var matchingIndex) == false)
            {
                matchingCount = 0;
                return false;
            }

            matchingCount = 1;

            for (int i = 1; i < endWithParts.Length; i++)
            {
                if (matchingIndex + i >= pathParts.Length)
                {
                    break;
                }

                if (pathParts[matchingIndex + i] == endWithParts[i])
                {
                    matchingCount++;
                }
                else
                {
                    return true;
                }
            }
            
            return true;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<(string directoryAbsolutePath, int matchCount)> PathEndsWith(
            this IEnumerable<string> directoryPaths, string endsWith)
        {
            foreach (var directoryPath in directoryPaths)
            {
                if (directoryPath.PathEndsWith(endsWith, out var matchCount))
                {
                    yield return (directoryPath, matchCount);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<(string directoryAbsolutePath, int matchCount)> PathEndsWith(
            this IEnumerable<string> directoryPaths, string endsWith, bool sortByMatchCountDescending)
        {
            if (sortByMatchCountDescending)
            {
                return PathEndsWith(directoryPaths, endsWith).OrderByDescending(x => x.matchCount);
            }
            
            return PathEndsWith(directoryPaths, endsWith);
        }

        #endregion

        #region Path Trim End

        /// <summary>
        /// Example:
        /// Path : "A/B/C/D"
        /// TrimEndCount : 2
        /// Result : "A/B"
        /// </summary>
        /// <param name="path"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string PathTrimEnd(this string path, int count)
        {
            var parts = path.SplitPath();
            return string.Join(Path.DirectorySeparatorChar, parts[..^count]);
        }

        #endregion

        #region Sub Path

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetSubPath(this string path, int startIndex, int endIndex)
        {
            var parts = path.SplitPath();
            return string.Join(Path.DirectorySeparatorChar, parts[startIndex..(endIndex + 1)]);
        }

        #endregion
    }
}