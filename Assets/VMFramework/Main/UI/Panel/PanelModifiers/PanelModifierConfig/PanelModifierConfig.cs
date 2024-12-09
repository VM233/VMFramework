using System;
using UnityEngine;
using VMFramework.Configuration;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.UI
{
    public abstract class PanelModifierConfig : SubGamePrefab, IPanelModifierConfig
    {
        protected const string MODIFIER_CATEGORY = "Modifier";
        
        public override Type GameItemType => typeof(PanelModifier);

        IGameItem IGamePrefab.GenerateGameItem()
        {
            var newGameObject = new GameObject(GameItemType.Name);
            newGameObject.transform.SetParent(UIPanelManager.UIContainer);
            var gameItem = (IGameItem)newGameObject.AddComponent(GameItemType);
            return gameItem;
        }
    }
}