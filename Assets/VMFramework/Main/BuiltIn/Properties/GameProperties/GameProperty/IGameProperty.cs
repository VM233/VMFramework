using System;
using UnityEngine;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Properties
{
    public interface IGameProperty : ILocalizedGamePrefab
    {
        public Type TargetType { get; }
        
        public Sprite icon { get; }
        
        public string GetValueString(object target);
    }
}