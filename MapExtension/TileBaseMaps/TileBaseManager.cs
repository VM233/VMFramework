using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using VMFramework.Procedure;

namespace VMFramework.Maps
{
    [ManagerCreationProvider(ManagerType.ResourcesCore)]
    public class TileBaseManager : ManagerBehaviour<TileBaseManager>
    {
        [ShowInInspector]
        public static TileBase EmptyTileBase { get; private set; }

        [ShowInInspector]
        private static readonly Dictionary<Sprite, TileBase> allTileBases = new();

        protected override void Awake()
        {
            base.Awake();
            
            EmptyTileBase = ScriptableObject.CreateInstance<Tile>();

            ClearBuffer();
        }

        public static void ClearBuffer()
        {
            foreach (var tileBase in allTileBases.Values)
            {
                DestroyImmediate(tileBase);
            }
            allTileBases.Clear();
        }

        public static TileBase GetTileBase(Sprite sprite)
        {
            if (sprite == null)
            {
                return EmptyTileBase;
            }

            if (allTileBases.TryGetValue(sprite, out var tileBase))
            {
                return tileBase;
            }

            var newTileBase = ScriptableObject.CreateInstance<Tile>();
            newTileBase.name = sprite.name;
            newTileBase.sprite = sprite;

            allTileBases[sprite] = newTileBase;

            return newTileBase;
        }
    }
}
