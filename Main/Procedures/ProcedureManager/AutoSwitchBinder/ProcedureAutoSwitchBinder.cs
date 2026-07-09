using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VMFramework.Core;

namespace VMFramework.Procedure
{
    /// <summary>
    /// This class is used to bind one procedure to another, so that when the first procedure is completed,
    /// the second procedure is automatically entered.
    /// </summary>
    public static class ProcedureAutoSwitchBinder
    {
        private static readonly Dictionary<string, string> autoSwitchBindings = new();

        public static void Init(IEnumerable<IProcedure> procedures)
        {
            autoSwitchBindings.Clear();
            foreach (var procedure in procedures)
            {
                var procedureType = procedure.GetType();

                var autoSwitchAttribute =
                    procedureType.GetAttribute<ProcedureDefaultAutoSwitchAttribute>(false);
                
                if (autoSwitchAttribute == null)
                {
                    continue;
                }
                
                var toProcedureID = autoSwitchAttribute?.toProcedureID;
                RegisterBinding(procedure.id, toProcedureID);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RegisterBinding(string fromProcedureID, string toProcedureID)
        {
            if (autoSwitchBindings.TryAdd(fromProcedureID, toProcedureID))
            {
                return;
            }
            
            UnityEngine.Debug.LogWarning($"Binding already exists for {fromProcedureID} to {toProcedureID}. " +
                             $"Overwriting with new binding : {fromProcedureID}->{toProcedureID}.");
            autoSwitchBindings[fromProcedureID] = toProcedureID;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnregisterBinding(string fromProcedureID, string toProcedureID)
        {
            if (autoSwitchBindings.TryGetValue(fromProcedureID, out string existingToProcedureID) == false)
            {
                UnityEngine.Debug.LogWarning($"No binding exists for {fromProcedureID}");
                return;
            }

            if (existingToProcedureID != toProcedureID)
            {
                UnityEngine.Debug.LogWarning($"Binding for {fromProcedureID}->{existingToProcedureID} " +
                                 $"does not match {fromProcedureID}->{toProcedureID}.");
                return;
            }
            
            autoSwitchBindings.Remove(fromProcedureID);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnregisterBinding(string procedureID)
        {
            if (autoSwitchBindings.Remove(procedureID) == false)
            {
                UnityEngine.Debug.LogWarning($"No binding exists for {procedureID}");
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetNextProcedureID(string currentProcedureID)
        {
            return autoSwitchBindings.GetValueOrDefault(currentProcedureID);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetNextProcedureID(string currentProcedureID, out string nextProcedureID)
        {
            return autoSwitchBindings.TryGetValue(currentProcedureID, out nextProcedureID);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasBinding(string fromProcedureID)
        {
            return autoSwitchBindings.ContainsKey(fromProcedureID);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasBinding(string fromProcedureID, string toProcedureID)
        {
            if (autoSwitchBindings.TryGetValue(fromProcedureID, out string existingToProcedureID) == false)
            {
                return false;
            }
            
            return existingToProcedureID == toProcedureID;
        }
    }
}