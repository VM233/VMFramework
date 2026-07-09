using VMFramework.GameLogicArchitecture;

namespace VMFramework.Editor
{
    public interface IScriptCreationViewer : INameOwner
    {
        public string AssetFolderPath { get; set; }
        
        public string ClassName { get; }
        
        public string NamespaceName { get; }
    }
}