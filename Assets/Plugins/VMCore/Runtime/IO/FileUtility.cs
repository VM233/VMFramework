using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace VMFramework.Core
{
    public static class FileUtility
    {
        #region File

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ExistsFile(this string filePath)
        {
            return File.Exists(filePath);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OpenFile(this string filePath)
        {
            if (filePath.ExistsFile())
            {
                Process.Start(filePath);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OverWriteFile(this string filePath, string content)
        {
            FileStream fs = new(filePath, FileMode.OpenOrCreate, FileAccess.Write);

            fs.Seek(0, SeekOrigin.Begin);
            fs.SetLength(0);

            StreamWriter sw = new(fs, Encoding.UTF8);

            sw.Write(content);
            sw.Flush();

            sw.Close();
            fs.Close();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AppendFile(this string filePath, string content)
        {
            StreamWriter sw = new(filePath, true, Encoding.UTF8);

            sw.Write(content);
            sw.Flush();

            sw.Close();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ReadText(this string filePath)
        {
            return File.ReadAllText(filePath);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetFileNameFromPath(this string path)
        {
            return Path.GetFileName(path);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetFileNameWithoutExtensionFromPath(this string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetExtensionFromPath(this string path)
        {
            return Path.GetExtension(path);
        }

        #endregion
    }
}
