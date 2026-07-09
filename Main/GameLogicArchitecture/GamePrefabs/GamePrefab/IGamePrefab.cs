using System;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.Procedure;

namespace VMFramework.GameLogicArchitecture
{
    public partial interface IGamePrefab : IIDOwner<string>, INameOwner, IInitializer, ICheckableConfig, IGameTagsOwner
    {
        public const string NULL_ID = "null";
        
        public new string id { get; set; }

        string IIDOwner<string>.id => id;

        public bool IsActive { get; set; }
        
        public bool IsDebugging { get; set; }
        
        public Type GameItemType { get; }
        
        public int GameItemPrewarmCount { get; }
        
        public string IDPrefix { get; }
        public string IDSuffix { get; }
        
        public event Action<IGamePrefab, string, string> OnIDChangedEvent;

        public IGameItem GenerateGameItem()
        {
            GameItemType.AssertIsNotNull(nameof(GameItemType));
            
            return (IGameItem)Activator.CreateInstance(GameItemType);
        }
    }
}
