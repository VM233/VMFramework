﻿using System;
using System.Collections.Generic;
using VMFramework.Core;
using Sirenix.OdinInspector;
using UnityEngine.Scripting;
using VMFramework.GameLogicArchitecture;
using VMFramework.Procedure;

namespace VMFramework.Recipes
{
    [ManagerCreationProvider(ManagerType.OtherCore)]
    public class RecipeQueryManager : UniqueMonoBehaviour<RecipeQueryManager>
    {
        [ShowInInspector]
        public static List<Func<object, IEnumerable<IRecipe>>> recipeInputQueryHandlers =
            new();

        [ShowInInspector]
        public static List<Func<object, IEnumerable<IRecipe>>> recipeOutputQueryHandlers =
            new();

        public static void RegisterRecipeInputQueryHandler(
            Func<object, IEnumerable<IRecipe>> handler)
        {
            recipeInputQueryHandlers.Add(handler);
        }

        public static void RegisterRecipeOutputQueryHandler(
            Func<object, IEnumerable<IRecipe>> handler)
        {
            recipeOutputQueryHandlers.Add(handler);
        }

        public static IEnumerable<IRecipe> GetRecipesByInput(object item)
        {
            foreach (var handler in recipeInputQueryHandlers)
            {
                foreach (var recipe in handler(item))
                {
                    yield return recipe;
                }
            }
        }

        public static IEnumerable<IRecipe> GetRecipesByOutput(object item)
        {
            foreach (var handler in recipeOutputQueryHandlers)
            {
                foreach (var recipe in handler(item))
                {
                    yield return recipe;
                }
            }
        }
    }

    [GameInitializerRegister(GameInitializationDoneProcedure.ID, ProcedureLoadingType.OnEnter)]
    [Preserve]
    public sealed class RecipeQueryInitializer : IGameInitializer
    {
        IEnumerable<InitializationAction> IInitializer.GetInitializationActions()
        {
            yield return new(InitializationOrder.Init, OnInit, this);
        }

        private static void OnInit(Action onDone)
        {
            foreach (var recipe in GamePrefabManager.GetAllGamePrefabs<IRecipe>())
            {
                foreach (var recipeInputQueryPattern in recipe.GetInputQueryPatterns())
                {
                    recipeInputQueryPattern.RegisterCache(recipe);
                }

                foreach (var recipeOutputQueryPattern in recipe
                             .GetOutputQueryPatterns())
                {
                    recipeOutputQueryPattern.RegisterCache(recipe);
                }
            }

            foreach (var recipeInputQueryPatternType in
                     typeof(IRecipeInputQueryPattern).GetDerivedClasses(false,
                         false))
            {
                var method = recipeInputQueryPatternType
                    .GetStaticMethodByAttribute<RecipeInputQueryHandlerAttribute>(false);

                if (method != null)
                {
                    var handler = (Func<object, IEnumerable<IRecipe>>)Delegate
                        .CreateDelegate(typeof(Func<object, IEnumerable<IRecipe>>), method);

                    RecipeQueryManager.RegisterRecipeInputQueryHandler(handler);
                }
                else
                {
                    throw new Exception(
                        $"{recipeInputQueryPatternType}没找到带有" +
                        $"{nameof(RecipeInputQueryHandlerAttribute)}的静态方法");
                }
            }

            foreach (var recipeOutputQueryPatternType in
                     typeof(IRecipeOutputQueryPattern).GetDerivedClasses(false,
                         false))
            {
                var method = recipeOutputQueryPatternType
                    .GetStaticMethodByAttribute<RecipeOutputQueryHandlerAttribute>(
                        false);

                if (method != null)
                {
                    var handler = (Func<object, IEnumerable<IRecipe>>)Delegate
                        .CreateDelegate(typeof(Func<object, IEnumerable<IRecipe>>),
                            method);

                    RecipeQueryManager.RegisterRecipeOutputQueryHandler(handler);
                }
                else
                {
                    throw new Exception(
                        $"{recipeOutputQueryPatternType}没找到带有" +
                        $"{nameof(RecipeOutputQueryHandlerAttribute)}的静态方法");
                }
            }

            onDone();
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class RecipeInputQueryHandlerAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class RecipeOutputQueryHandlerAttribute : Attribute
    {

    }
}
