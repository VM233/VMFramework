#if UNITY_EDITOR
using UnityEditor;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.Tools.Editor;

namespace VMFramework.Editor
{
    public static class GamePrefabTemplateCreator
    {
        private static readonly GamePrefabScriptPostProcessor gamePrefabPostProcessor = new();

        private static readonly GamePrefabInterfaceScriptPostProcessor gamePrefabInterfacePostProcessor = new();

        private static readonly GameItemScriptPostProcessor gameItemPostProcessor = new();

        private static readonly GameItemInterfaceScriptPostProcessor gameItemInterfacePostProcessor = new();

        private static readonly GamePrefabGeneralSettingScriptPostProcessor gamePrefabGeneralSettingPostProcessor =
            new();
        
        private static readonly GameItemSerializerScriptPostProcessor gameItemSerializerPostProcessor = new();

        [MenuItem(UnityMenuItemNames.SCRIPT_TEMPLATE + "Derived Game Prefab", false, 0)]
        private static void CreateDerivedGamePrefabScript()
        {
            ScriptTemplatesCreatorTools.CreateScript<DerivedGamePrefabScriptCreationViewer>(info =>
            {
                var parentGamePrefabName = info.parentGamePrefabType.Name;
                var parentGamePrefabInterfaceName = "I" + parentGamePrefabName;
                var parentGameItemType = GamePrefabTypeQuery.GetGameItemType(info.parentGamePrefabType);
                var parentGameItemName = parentGameItemType.Name;
                var parentGameItemInterfaceName = "I" + parentGameItemName;

                var gamePrefabName = info.ClassName;
                var gamePrefabInterfaceName = "I" + gamePrefabName;
                var gameItemName = info.name;
                var gameItemInterfaceName = "I" + gameItemName;

                var gamePrefabFolder = info.assetFolderPath;
                var gameItemFolder = info.assetFolderPath;

                var createGameItem = info.withGameItem && GamePrefabTypeQuery.HasGameItem(info.parentGamePrefabType);

                if (info.createSubFolders)
                {
                    gamePrefabFolder = gamePrefabFolder.PathCombine(gamePrefabName);
                    gameItemFolder = gameItemFolder.PathCombine(gameItemName);
                }

                ScriptCreator.CreateScriptAssets(ScriptTemplatesNames.GAME_PREFAB, gamePrefabName, gamePrefabFolder,
                    extraInfo: new GamePrefabScriptExtraInfo()
                    {
                        NamespaceName = info.namespaceName,
                        EnableParentInterfaceRegion = info.withGamePrefabInterface,
                        ParentInterfaceName = gamePrefabInterfaceName,
                        ParentClassName = parentGamePrefabName,
                        EnableIDSuffixOverrideRegion = false,
                        EnableGameItemTypeOverrideRegion = createGameItem,
                        GameItemType = createGameItem ? $"typeof({gameItemName})" : "null"
                    }, postProcessor: gamePrefabPostProcessor);

                if (info.withGamePrefabInterface)
                {
                    ScriptCreator.CreateScriptAssets(ScriptTemplatesNames.GAME_PREFAB_INTERFACE,
                        gamePrefabInterfaceName, gamePrefabFolder, extraInfo: new GamePrefabInterfaceScriptExtraInfo()
                        {
                            NamespaceName = info.namespaceName,
                            parentInterfaceName = parentGamePrefabInterfaceName
                        }, postProcessor: gamePrefabInterfacePostProcessor);
                }

                if (createGameItem)
                {
                    ScriptCreator.CreateScriptAssets(ScriptTemplatesNames.GAME_ITEM, gameItemName, gameItemFolder,
                        extraInfo: new GameItemScriptExtraInfo()
                        {
                            NamespaceName = info.namespaceName,
                            parentClassName = parentGameItemName,
                            enableParentInterfaceRegion = info.withGameItemInterface,
                            parentInterfaceName = gameItemInterfaceName,
                            gamePrefabInterfaceName = info.withGamePrefabInterface
                                ? gamePrefabInterfaceName
                                : gamePrefabName,
                            gamePrefabFieldName = gamePrefabName.ToPascalCase()
                        }, postProcessor: gameItemPostProcessor);

                    if (info.withGameItemInterface)
                    {
                        ScriptCreator.CreateScriptAssets(ScriptTemplatesNames.GAME_ITEM_INTERFACE,
                            gameItemInterfaceName, gameItemFolder, extraInfo: new GameItemInterfaceScriptExtraInfo()
                            {
                                NamespaceName = info.namespaceName,
                                parentInterfaceName = parentGameItemInterfaceName
                            }, postProcessor: gameItemInterfacePostProcessor);
                    }
                }
            });
        }

        [MenuItem(UnityMenuItemNames.SCRIPT_TEMPLATE + "Game Prefab")]
        private static void CreateGamePrefabScript()
        {
            ScriptTemplatesCreatorTools.CreateScript<GamePrefabScriptCreationViewer>(info =>
            {
                var gamePrefabName = info.ClassName;
                var gamePrefabInterfaceName = "I" + gamePrefabName;

                var gameItemName = info.name;
                var gameItemInterfaceName = "I" + gameItemName;

                var generalSettingName = gameItemName + "GeneralSetting";

                var gameItemType = info.withGameItem ? $"typeof({gameItemName})" : "null";

                var gamePrefabFolder = info.assetFolderPath;
                var gameItemFolder = info.assetFolderPath;
                var gamePrefabGeneralSettingFolder = info.assetFolderPath;

                if (info.createSubFolders)
                {
                    gamePrefabFolder = gamePrefabFolder.PathCombine(gamePrefabName);
                    gameItemFolder = gameItemFolder.PathCombine(gameItemName);
                    gamePrefabGeneralSettingFolder = gamePrefabGeneralSettingFolder.PathCombine(generalSettingName);
                }

                ScriptCreator.CreateScriptAssets(ScriptTemplatesNames.GAME_PREFAB, gamePrefabName, gamePrefabFolder,
                    extraInfo: new GamePrefabScriptExtraInfo()
                    {
                        NamespaceName = info.namespaceName,
                        EnableParentInterfaceRegion = true,
                        ParentInterfaceName = gamePrefabInterfaceName,
                        ParentClassName = info.gamePrefabBaseType.GetName(),
                        EnableIDSuffixOverrideRegion = true,
                        IDSuffix = info.name.ToSnakeCase(),
                        EnableGameItemTypeOverrideRegion = info.withGameItem,
                        GameItemType = gameItemType,
                    }, postProcessor: gamePrefabPostProcessor);

                ScriptCreator.CreateScriptAssets(ScriptTemplatesNames.GAME_PREFAB_INTERFACE, gamePrefabInterfaceName,
                    gamePrefabFolder, extraInfo: new GamePrefabInterfaceScriptExtraInfo()
                    {
                        NamespaceName = info.namespaceName,
                        parentInterfaceName = info.gamePrefabBaseType.GetInterfaceName()
                    }, postProcessor: gamePrefabInterfacePostProcessor);

                if (info.withGameItem)
                {
                    ScriptCreator.CreateScriptAssets(ScriptTemplatesNames.GAME_ITEM, gameItemName, gameItemFolder,
                        extraInfo: new GameItemScriptExtraInfo()
                        {
                            NamespaceName = info.namespaceName,
                            parentClassName = info.gameItemBaseType.GetName(),
                            enableParentInterfaceRegion = true,
                            parentInterfaceName = gameItemInterfaceName,
                            gamePrefabInterfaceName = gamePrefabInterfaceName,
                            gamePrefabFieldName = info.ClassName.ToPascalCase()
                        }, postProcessor: gameItemPostProcessor);

                    ScriptCreator.CreateScriptAssets(ScriptTemplatesNames.GAME_ITEM_INTERFACE, gameItemInterfaceName,
                        gameItemFolder, extraInfo: new GameItemInterfaceScriptExtraInfo()
                        {
                            NamespaceName = info.namespaceName,
                            parentInterfaceName = info.gameItemBaseType.GetInterfaceName()
                        }, postProcessor: gameItemInterfacePostProcessor);
                }

                if (info.withGamePrefabGeneralSetting)
                {
                    ScriptCreator.CreateScriptAssets(ScriptTemplatesNames.GAME_PREFAB_GENERAL_SETTING,
                        generalSettingName, gamePrefabGeneralSettingFolder,
                        extraInfo: new GamePrefabGeneralSettingScriptExtraInfo()
                        {
                            NamespaceName = info.namespaceName,
                            BaseGamePrefabType = gamePrefabInterfaceName,
                            NameInGameEditor = gameItemName.ToPascalCase(" "),
                            EnableGameItemNameOverrideRegion = info.withGameItem,
                            GameItemName = gameItemName
                        }, postProcessor: gamePrefabGeneralSettingPostProcessor);
                }

#if FISHNET
                if (info.withGameItem && info.withSerializer)
                {
                    var serializerName = gameItemName + "Serializer";
                    ScriptCreator.CreateScriptAssets(ScriptTemplatesNames.GAME_ITEM_SERIALIZER, serializerName,
                        gameItemFolder, extraInfo: new GameItemSerializerScriptExtraInfo()
                        {
                            NamespaceName = info.namespaceName,
                            GameItemName = gameItemName,
                            GameItemInterfaceName = gameItemInterfaceName,
                            GameItemFieldName = gameItemName.ToCamelCase()
                        }, postProcessor: gameItemSerializerPostProcessor);
                }
#endif
            });
        }
    }
}
#endif