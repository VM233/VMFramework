using System;
using System.Runtime.ExceptionServices;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Localization.Settings;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace VMFramework.Localization
{
    public static class LocalizedStringUtility
    {
        public static async Task<string> GetLocalizedTextAsync(this LocalizedString localizedString, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var initialization = LocalizationSettings.InitializationOperation;
            Addressables.ResourceManager.Acquire(initialization);
            try
            {
                await initialization.Task;
                if (initialization.Status != AsyncOperationStatus.Succeeded)
                {
                    if (initialization.OperationException != null) ExceptionDispatchInfo.Capture(initialization.OperationException).Throw();
                    throw new InvalidOperationException("Localization initialization failed.");
                }
                cancellationToken.ThrowIfCancellationRequested();
            }
            finally { Addressables.Release(initialization); }
            var completion = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            bool? automaticRelease = null;
            var operation = localizedString.GetLocalizedStringAsync();
            Addressables.ResourceManager.Acquire(operation);
            bool failureSubscribed = false;
            void Failed(AsyncOperationHandle actual, Exception failure)
            {
                if (actual.Equals((AsyncOperationHandle)operation)) automaticRelease = Application.isPlaying;
            }
            void Completed(AsyncOperationHandle<string> actual)
            {
                automaticRelease ??= Application.isPlaying;
                completion.TrySetResult(true);
            }
            try
            {
                if (operation.IsDone) Completed(operation);
                else
                {
                    // Failed Addressables operations defer Completed; capture their actual failure boundary.
                    ResourceManager.ExceptionHandler += Failed;
                    failureSubscribed = true;
                    operation.Completed += Completed;
                }
                // An accepted Localization request has no cancellation API. Join its actual terminal result.
                await completion.Task;
                if (operation.Status != AsyncOperationStatus.Succeeded)
                {
                    if (operation.OperationException != null) ExceptionDispatchInfo.Capture(operation.OperationException).Throw();
                    throw new InvalidOperationException($"Localized string operation failed: {localizedString.TableReference}/{localizedString.TableEntryReference}.");
                }
                cancellationToken.ThrowIfCancellationRequested();
                return operation.Result;
            }
            finally
            {
                if (failureSubscribed) ResourceManager.ExceptionHandler -= Failed;
                operation.Completed -= Completed;
                Addressables.Release(operation);
                // Unity only schedules its original caller reference for auto-release when the operation completes in Play.
                if (automaticRelease == false) Addressables.Release(operation);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValid(this LocalizedString localizedString)
        {
            if (localizedString.TableReference.ReferenceType == TableReference.Type.Empty)
            {
                return false;
            }

            if (localizedString.TableEntryReference.ReferenceType == TableEntryReference.Type.Empty)
            {
                return false;
            }

            return true;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetGeneralString(this LocalizedString localizedString, IList<object> arguments = null)
        {
            if (localizedString == null)
            {
                return null;
            }
                
#if UNITY_EDITOR
            if (Application.isPlaying == false)
            {
                return localizedString.GetLocalizedStringInEditor(arguments);
            }
#endif
            if (localizedString.TableReference.ReferenceType == TableReference.Type.Empty)
            {
                return null;
            }
            
            return localizedString.GetLocalizedString(arguments ?? localizedString.Arguments);
        }
    }
}
