using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public static class ExplorerUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OpenDirectoryInExplorer(this string directoryPath, bool createIfNotExisted)
        {
            directoryPath = directoryPath.ReplaceToDirectorySeparator();
            if (createIfNotExisted)
            {
                directoryPath.CreateDirectory();
            }

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "explorer.exe",
                Arguments = directoryPath,
                CreateNoWindow = true
            };
            
            Process.Start(startInfo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OpenFileInExplorer(this string filePath)
        {
            filePath = filePath.ReplaceToDirectorySeparator();

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "explorer.exe",
                Arguments = "/select," + filePath,
                CreateNoWindow = true
            };
            
            Process.Start(startInfo);
        }
    }
}