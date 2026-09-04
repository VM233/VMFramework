using System;
using System.Collections.Generic;
using System.Text;

namespace VMFramework.GameLogicArchitecture
{
    public sealed class MissingGamePrefabReferencesException : InvalidOperationException
    {
        public IReadOnlyList<IGamePrefab> MissingGamePrefabs { get; }

        internal MissingGamePrefabReferencesException(
            IReadOnlyList<IGamePrefab> missingGamePrefabs)
            : base(BuildMessage(missingGamePrefabs))
        {
            MissingGamePrefabs = new List<IGamePrefab>(missingGamePrefabs)
                .AsReadOnly();
        }

        private static string BuildMessage(
            IReadOnlyList<IGamePrefab> missingGamePrefabs)
        {
            var message = new StringBuilder()
                .Append("VMFramework initialization cannot continue because ")
                .Append(missingGamePrefabs.Count)
                .AppendLine(" registered Game Prefab(s) have no valid Prefab reference:");

            foreach (var gamePrefab in missingGamePrefabs)
            {
                message.Append("- '")
                    .Append(gamePrefab.id)
                    .Append("' (")
                    .Append(gamePrefab.GetType().FullName)
                    .AppendLine(")");
            }

            return message
                .Append("Assign every IPrefabProvider.Prefab before startup.")
                .ToString();
        }
    }
}
