#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Editor;
using VMFramework.OdinExtensions;
using Object = UnityEngine.Object;

namespace VMFramework.Editor
{
    public class SerializeReferenceMigrator : MonoBehaviour
    {
        [IsNotNullOrEmpty] public string oldClassName; // 仅基础名，如 "Example"
        [IsNotNullOrEmpty] public string oldNamespaceName; // 如 "Old.Namespace"

        [IsNotNullOrEmpty] public string newClassName; // 仅基础名，如 "Example"
        [IsNotNullOrEmpty] public string newNamespaceName; // 如 "New.Namespace"

        [Button]
        public void Migrate()
        {
            if (EditorSettings.serializationMode != SerializationMode.ForceText)
            {
                throw new ArgumentException("请先将项目的序列化模式设置为 Force Text");
            }

            string[] assetPaths = AssetDatabase.GetAllAssetPaths().Where(assetPath =>
                assetPath.EndsWith(".prefab", StringComparison.OrdinalIgnoreCase) ||
                assetPath.EndsWith(".asset", StringComparison.OrdinalIgnoreCase)).ToArray();

            int changedFileCount = 0;
            int totalReplaceCount = 0;
            List<string> assetPathsToImport = new List<string>();

            try
            {
                AssetDatabase.StartAssetEditing();

                foreach (string assetPath in assetPaths)
                {
                    string fileText = File.ReadAllText(assetPath, Encoding.UTF8);

                    int replacedCount = 0;
                    replacedCount += ReplaceTypeBlocks(ref fileText, oldClassName, oldNamespaceName, newClassName,
                        newNamespaceName);
                    replacedCount += ReplaceValueBlocks(ref fileText, oldClassName, oldNamespaceName, newClassName,
                        newNamespaceName);

                    if (replacedCount > 0)
                    {
                        File.WriteAllText(assetPath, fileText, new UTF8Encoding(false));
                        changedFileCount++;
                        totalReplaceCount += replacedCount;
                        assetPathsToImport.Add(assetPath);
                    }
                }
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
            }

            foreach (string assetPath in assetPathsToImport)
            {
                AssetDatabase.ImportAsset(assetPath);
            }

            AssetDatabase.SaveAssets();
            UnityEngine.Debug.Log($"完成替换 {totalReplaceCount} 个序列化引用，修改了 {changedFileCount} 个文件。");
            UnityEngine.Debug.Log($"修改的文件为：{string.Join("\n", assetPathsToImport)}");
        }

        [Button]
        public void Migrate(Object target)
        {
            string assetPath = target.GetAssetPath();
            if (assetPath.IsNullOrEmpty())
            {
                Debug.LogError($"无法获取 {target} 的路径。", target);
                return;
            }

            string fileText = File.ReadAllText(assetPath, Encoding.UTF8);
            int replacedCount = 0;
            replacedCount += ReplaceTypeBlocks(ref fileText, oldClassName, oldNamespaceName, newClassName,
                newNamespaceName);
            replacedCount += ReplaceValueBlocks(ref fileText, oldClassName, oldNamespaceName, newClassName,
                newNamespaceName);

            if (replacedCount > 0)
            {
                File.WriteAllText(assetPath, fileText, new UTF8Encoding(false));
                AssetDatabase.ImportAsset(assetPath);
                AssetDatabase.SaveAssets();
            }
        }

        // 形式一：type: { class: X, ns: Y, asm: Z }
        static int ReplaceTypeBlocks(ref string fileText, string oldBaseClassName, string oldNamespaceName,
            string newBaseClassName, string newNamespaceName)
        {
            int totalReplaceCount = 0;
            Regex typeBlockRegex = new Regex(@"type:\s*\{[^}]*\}", RegexOptions.Singleline);

            fileText = typeBlockRegex.Replace(fileText, match =>
            {
                string typeBlockText = match.Value;

                // 提取 class 值（先匹配带引号，再匹配无引号）
                string classValue = null;
                Group classValueGroup = null;
                Match classWithQuoteMatch =
                    Regex.Match(typeBlockText, @"class:\s*(?<quote>['""])(?<value>.*?)\k<quote>");
                if (classWithQuoteMatch.Success)
                {
                    classValue = classWithQuoteMatch.Groups["value"].Value;
                    classValueGroup = classWithQuoteMatch.Groups["value"];
                }
                else
                {
                    Match classNoQuoteMatch = Regex.Match(typeBlockText, @"class:\s*(?<value>[^,}\r\n]+)");
                    if (classNoQuoteMatch.Success)
                    {
                        classValue = classNoQuoteMatch.Groups["value"].Value.Trim();
                        classValueGroup = classNoQuoteMatch.Groups["value"];
                    }
                }

                if (string.IsNullOrEmpty(classValue))
                    return typeBlockText;

                // 提取 ns 值（先匹配带引号，再匹配无引号）
                string namespaceValue = null;
                Group namespaceValueGroup = null;
                Match namespaceWithQuoteMatch =
                    Regex.Match(typeBlockText, @"ns:\s*(?<quote>['""])(?<value>.*?)\k<quote>");
                if (namespaceWithQuoteMatch.Success)
                {
                    namespaceValue = namespaceWithQuoteMatch.Groups["value"].Value.Trim();
                    namespaceValueGroup = namespaceWithQuoteMatch.Groups["value"];
                }
                else
                {
                    Match namespaceNoQuoteMatch = Regex.Match(typeBlockText, @"ns:\s*(?<value>[^,}\r\n\s]+)");
                    if (namespaceNoQuoteMatch.Success)
                    {
                        namespaceValue = namespaceNoQuoteMatch.Groups["value"].Value.Trim();
                        namespaceValueGroup = namespaceNoQuoteMatch.Groups["value"];
                    }
                }

                if (string.IsNullOrEmpty(namespaceValue))
                    return typeBlockText;

                if (!string.Equals(namespaceValue, oldNamespaceName, StringComparison.Ordinal))
                    return typeBlockText;

                // 解析 class 基础名 + 泛型后缀（`1[[...]] / `2[[...]] / 无泛型）
                string originalBaseClassName;
                string originalGenericSuffix;
                int backtickIndex = classValue.IndexOf('`');
                if (backtickIndex >= 0)
                {
                    originalBaseClassName = classValue.Substring(0, backtickIndex);
                    originalGenericSuffix = classValue.Substring(backtickIndex);
                }
                else
                {
                    originalBaseClassName = classValue;
                    originalGenericSuffix = string.Empty;
                }

                if (!string.Equals(originalBaseClassName, oldBaseClassName, StringComparison.Ordinal))
                    return typeBlockText;

                string newClassValue = newBaseClassName + originalGenericSuffix;

                if (classValueGroup != null)
                    typeBlockText = ReplaceRange(typeBlockText, classValueGroup.Index, classValueGroup.Length,
                        newClassValue);

                if (namespaceValueGroup != null)
                    typeBlockText = ReplaceRange(typeBlockText, namespaceValueGroup.Index, namespaceValueGroup.Length,
                        newNamespaceName);

                totalReplaceCount++;
                return typeBlockText;
            });

            return totalReplaceCount;
        }

        // 形式二：支持多行 value（示例中泛型参数换行）
        //   propertyPath: 'managedReferences[6271235643731083702]'
        //   value: 'Assembly-CSharp Old.Namespace.OldName`1[[Some.Type,
        //           Assembly-CSharp]]'
        //   objectReference: {fileID: 0}
        // 同时支持：非泛型（无 `n）、任意泛型参数个数（`1, `2, ...）
        static int ReplaceValueBlocks(ref string fileText, string oldBaseClassName, string oldNamespaceName,
            string newBaseClassName, string newNamespaceName)
        {
            int totalReplaceCount = 0;

            // 捕获从 value: ' 到匹配的收尾引号为止（允许跨行）
            Regex valueBlockRegex = new Regex(
                @"(?m)^(?<indent>\s*)value:\s*(?<quote>['""])(?<content>.*?)(?<!\\)\k<quote>", RegexOptions.Singleline);

            fileText = valueBlockRegex.Replace(fileText, match =>
            {
                string indentText = match.Groups["indent"].Value;
                string quoteText = match.Groups["quote"].Value;
                string
                    contentText =
                        match.Groups["content"]
                            .Value; // 例如：Assembly-CSharp OldNs.OldName`1[[VM.Type,\n  Assembly-CSharp]]

                // 拆分为程序集名 与 类型全名；类型部分允许跨行
                Match splitMatch = Regex.Match(contentText, @"^(?<assemblyName>[^\s'""]+)\s+(?<typeFullName>.+)$",
                    RegexOptions.Singleline);
                if (!splitMatch.Success)
                    return match.Value;

                string assemblyName = splitMatch.Groups["assemblyName"].Value;
                string typeFullName = splitMatch.Groups["typeFullName"].Value;

                string oldFullyQualifiedPrefix = oldNamespaceName + "." + oldBaseClassName;
                if (!typeFullName.StartsWith(oldFullyQualifiedPrefix, StringComparison.Ordinal))
                    return match.Value;

                // 只替换命名空间+基础类名；保留泛型后缀（`n[[...]]）或无后缀
                string newTypeFullName = newNamespaceName + "." + newBaseClassName +
                                         typeFullName.Substring(oldFullyQualifiedPrefix.Length);

                totalReplaceCount++;
                return $"{indentText}value: {quoteText}{assemblyName} {newTypeFullName}{quoteText}";
            });

            return totalReplaceCount;
        }

        static string ReplaceRange(string sourceText, int startIndex, int length, string replacementText)
        {
            StringBuilder stringBuilder = new StringBuilder(sourceText.Length - length + replacementText.Length);
            stringBuilder.Append(sourceText, 0, startIndex);
            stringBuilder.Append(replacementText);
            stringBuilder.Append(sourceText, startIndex + length, sourceText.Length - startIndex - length);
            return stringBuilder.ToString();
        }
    }
}
#endif