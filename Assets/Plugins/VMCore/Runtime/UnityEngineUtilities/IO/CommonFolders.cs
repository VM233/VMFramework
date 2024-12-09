using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public static class CommonFolders
    {
        /// <summary>
        /// 资源根目录
        /// 【读写权限】:pc可读写，移动端只读
        /// 【功能特点】:资源根目录，所有资源都在这里。
        /// 【Editor路径】:Assets
        /// 【平台路径】:
        /// <para>Win: E:/myGame/Assets</para>
        /// <para>Mac: /myGame/Assets/</para>
        /// <para>Android: /data/app/com.myCompany.myGame-1/base.apk!</para> 
        /// <para>ios: /var/containers/Application/E32134…3B123/myGame.app/Data</para>
        /// </summary>
        public static string AssetsFolderPath
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Application.dataPath;
        }
        
        public static string ResourcesFolderPath
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Application.dataPath.PathCombine("Resources");
        }

        /// <summary>
        /// 项目地址
        /// 是资源目录去掉了后面的Assets得到
        /// 如:E:/myGame/
        /// </summary>
        public static string ProjectFolderPath
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => AssetsFolderPath[..^"Assets".Length];
        }

        /// <summary>
        /// 普通资源目录
        /// 【读写权限】:pc可读写，移动端只读
        /// 【功能特点】:不压缩，外部可访问资源内容
        /// 【Editor路径】:Assets/StreamingAssets
        /// 【平台路径】:
        /// Win: D:/myGame/Assets/StreamingAssets
        /// Mac: /myGame/Assets/StreamingAssets
        /// Android: jar:file:///data/app/com.myCompany.myGame-1/base.apk!/assets
        /// ios: /var/containers/Application/E32134…3B123/myGame.app/Data/Raw
        /// </summary>
        public static string StreamingDataPath
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Application.streamingAssetsPath;
        }

        /// <summary>
        /// 自由资源目录
        /// 【读写权限】:全平台可读、可写
        /// 【功能特点】:不压缩，外部可任意体位访问。一般热更新、热补丁、热加载、存档的资源会选择存在这里。
        /// 【Editor路径】:Assets/PersistentDataPath
        /// 【平台路径】:
        /// Win: C:/Users/Administrator/Appdata/LocalLow/myCompany/myGame
        /// Mac: /Users/lodypig/Library/Application Support/myCompany/myGame
        /// Android: /data/data/com.myCompany.myGame/files
        /// ios: /var/mobile/Containers/Data/Application/E32134…3B123/Documents
        /// </summary>
        public static string PersistentDataPath
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Application.persistentDataPath;
        }

        /// <summary>
        /// 临时缓存目录
        /// 【读写权限】:全平台可读、可写
        /// 【功能特点】:临时缓存目录，用于存储缓存文件
        /// 【Editor路径】:Assets/TemporaryCachePath
        /// 【平台路径】:
        /// Win: C:/Users/Administrator/Appdata/Temp/myCompany/myGame
        /// Android: /data/data/com.myCompany.myGame/cache
        /// ios: /var/mobile/Containers/Data/Application/E32134…3B123/Library/Catches
        /// </summary>
        public static string CachePath
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Application.temporaryCachePath;
        }
    }
}