using System;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.Procedure;

namespace VMFramework.GameLogicArchitecture
{
    public partial interface IGamePrefab : IIDOwner, INameOwner, IInitializer, ICheckableConfig
    {
        public const string NULL_ID = "null";
        
        public new string id { get; set; }

        string IIDOwner<string>.id => id;

        public bool IsActive { get; set; }
        
        public bool IsDebugging { get; set; }
        
        public Type GameItemType { get; }
        
        public event Action<IGamePrefab, string, string> OnIDChangedEvent;
    }
}
