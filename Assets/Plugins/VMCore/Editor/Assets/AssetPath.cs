namespace VMFramework.Core.Editor
{
    public readonly struct AssetPath
    {
        public readonly string path;

        public readonly string folderPath;
        
        public readonly string fileName;
        
        public readonly string extension;
        
        public readonly string fullFileName;

        public AssetPath(string path)
        {
            this.path = path;
            this.folderPath = path.GetDirectoryPath();
            this.fileName = path.GetFileNameWithoutExtensionFromPath();
            this.extension = path.GetExtensionFromPath();
            this.fullFileName = fileName + extension;
        }

        public AssetPath(string folderPath, string fileName, string extension)
        {
            this.folderPath = folderPath;
            this.fileName = fileName;
            this.extension = extension;
            this.fullFileName = fileName + extension;
            this.path = folderPath.PathCombine(fullFileName);
        }

        public AssetPath(string folderPath, string fileName)
        {
            this.folderPath = folderPath;
            this.fileName = fileName;
            this.extension = string.Empty;
            this.fullFileName = fileName;
            this.path = folderPath.PathCombine(fullFileName);
        }
        
        public static implicit operator AssetPath(string path)
        {
            return new(path);
        }
    }
}