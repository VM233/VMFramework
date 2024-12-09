using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public static class DirectoryUtility
    {
        /// <summary>
        /// 目录是否存在
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExistsDirectory(this string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }

        /// <summary>
        /// 获取目录路径
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetDirectoryPath(this string filePath)
        {
            return Path.GetDirectoryName(filePath);
        }

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns>
        /// true:目录不存在，创建成功
        /// false:目录已存在
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CreateDirectory(this string directoryPath)
        {
            var existed = Directory.Exists(directoryPath);
            if (existed == false)
            {
                Directory.CreateDirectory(directoryPath);
            }

            return existed == false;
        }

        /// <summary>
        /// 获取目录下的所有文件路径
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        /// <exception cref="DirectoryNotFoundException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<string> GetAllFilesPath(this string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException($"Directory not found: {directoryPath}");
            }

            return Directory.EnumerateFiles(directoryPath, "*.*", SearchOption.TopDirectoryOnly);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<string> EnumerateAllDirectories(this string directoryPath)
        {
            return Directory.EnumerateDirectories(directoryPath, "*", SearchOption.AllDirectories);
        }
    }
}