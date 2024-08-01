using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.Procedure
{
    public partial class ProcedureManager
    {
        [ShowInInspector]
        public static bool IsLoading => initializerManager.isInitializing;

        [ShowInInspector]
        private static Dictionary<string, Dictionary<ProcedureLoadingType, List<IGameInitializer>>>
            _gameInitializers = new();

        [ShowInInspector]
        private static readonly InitializerManager initializerManager = new();

        public static void CollectGameInitializers()
        {
            foreach (var derivedClass in typeof(IGameInitializer).GetDerivedInstantiableClasses(false))
            {
                if (derivedClass.TryGetAttribute<GameInitializerRegister>(false,
                        out var register) == false)
                {
                    continue;
                }

                if (register.ProcedureID.IsNullOrEmpty())
                {
                    Debug.LogError(
                        $"{derivedClass}'s {nameof(GameInitializerRegister)} Attribute " +
                        $"has an empty {nameof(GameInitializerRegister.ProcedureID)}");
                    continue;
                }

                if (HasProcedure(register.ProcedureID) == false)
                {
                    Debug.LogError(
                        $"{nameof(ProcedureManager)} does not have a {nameof(IProcedure)} " +
                        $"with ID {register.ProcedureID} " +
                        $"required by {derivedClass}'s {nameof(GameInitializerRegister)} Attribute");
                    continue;
                }

                if (derivedClass.CreateInstance() is not IGameInitializer initializer)
                {
                    Debug.LogError(
                        $"Failed to create instance of {derivedClass} as an {nameof(IGameInitializer)}");
                    continue;
                }

                var initializersByLoadingType = _gameInitializers.GetValueOrAddNew(register.ProcedureID);

                var initializers = initializersByLoadingType.GetValueOrAddNew(register.LoadingType);
                
                initializers.Add(initializer);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IReadOnlyList<IGameInitializer> GetGameInitializers(string procedureID,
            ProcedureLoadingType loadingType)
        {
            if (_gameInitializers.TryGetValue(procedureID, out var initializersByLoadingType) == false)
            {
                return Array.Empty<IGameInitializer>();
            }

            if (initializersByLoadingType.TryGetValue(loadingType, out var initializers) == false)
            {
                return Array.Empty<IGameInitializer>();
            }

            return initializers;
        }

        private static async UniTask StartLoading(IReadOnlyList<IGameInitializer> initializers)
        {
            if (IsLoading)
            {
                Debugger.LogWarning("Loading already in progress.");
                return;
            }
            
            initializerManager.Set(initializers);
            await initializerManager.Initialize();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static async UniTaskVoid StartLoading(string procedureID, ProcedureLoadingType loadingType,
            Action onFinish = null)
        {
            var initializers = GetGameInitializers(procedureID, loadingType);
            await StartLoading(initializers);
            onFinish?.Invoke();
        }
    }
}