#if UNITY_EDITOR
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.Tools.Editor
{
    public abstract class ScriptCreationPostProcessor<TExtraInfo> : ScriptCreationPostProcessor
    {
        protected sealed override void PostProcess(string scriptAbsolutePath, ref string scriptContent, ScriptCreationExtraInfo extraInfo)
        {
            base.PostProcess(scriptAbsolutePath, ref scriptContent, extraInfo);

            if (extraInfo is not TExtraInfo typedExtraInfo)
            {
                Debug.LogError($"{nameof(extraInfo)} is not of type {typeof(TExtraInfo)}!");
                return;
            }
            
            PostProcess(scriptAbsolutePath, ref scriptContent, typedExtraInfo);
        }

        protected abstract void PostProcess(string scriptAbsolutePath, ref string scriptContent, TExtraInfo extraInfo);
    }
    
    public class ScriptCreationPostProcessor : IScriptCreationPostProcessor
    {
        protected virtual void PostProcess(string scriptAbsolutePath, ref string scriptContent,
            ScriptCreationExtraInfo extraInfo)
        {
            var fileName = scriptAbsolutePath.GetFileNameFromPath();

            fileName = fileName[..fileName.IndexOf('.')];

            var namespaceName = extraInfo.NamespaceName;
            string namespaceBegin = "";
            string namespaceEnd = "";

            if (namespaceName.IsNullOrWhiteSpace())
            {
                namespaceName = ScriptTemplateTags.NAMESPACE_NAME;
            }
            else
            {
                namespaceBegin = $"namespace {namespaceName} \n{{";
                namespaceEnd = "}";
            }

            string usingNamespacesContent = "";

            if (extraInfo.UsingNamespaces != null)
            {
                usingNamespacesContent = extraInfo.UsingNamespaces.Where(name => name.IsNullOrWhiteSpace() == false)
                    .Select(name => $"using {name};").Join("\n");
            }

            Replace(ref scriptContent, ScriptTemplateTags.CLASS_NAME, fileName);

            Replace(ref scriptContent, ScriptTemplateTags.NAMESPACE_BEGIN, namespaceBegin);
            Replace(ref scriptContent, ScriptTemplateTags.NAMESPACE_END, namespaceEnd);
            Replace(ref scriptContent, ScriptTemplateTags.NAMESPACE_NAME, namespaceName);
            Replace(ref scriptContent, ScriptTemplateTags.USING_NAMESPACES, usingNamespacesContent);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void Replace(ref string scriptContent, string tag, string value)
        {
            if (tag.IsNullOrWhiteSpace())
            {
                return;
            }
            
            string pattern = $"#{tag}#";

            if (value.IsNullOrWhiteSpace() == false)
            {
                scriptContent = scriptContent.Replace(pattern, value);
                return;
            }

            while (true)
            {
                var index = scriptContent.IndexOf(pattern, StringComparison.Ordinal);

                if (index == -1)
                {
                    break;
                }
                
                scriptContent = scriptContent.Remove(index, pattern.Length);

                if (index > 0)
                {
                    scriptContent = scriptContent.RemoveNewLine(index - 1);
                }
            }
        }

        protected static void Region(ref string scriptContent, string regionName, bool enabled)
        {
            var tag = $"#{regionName}#";
            var beginTag = tag + "@begin@";
            var endTag = tag + "@end@";
            
            if (enabled)
            {
                var beginTagLength = beginTag.Length;
                var endTagLength = endTag.Length;
                
                while (true)
                {
                    var startIndex = scriptContent.IndexOf(beginTag, StringComparison.Ordinal);

                    if (startIndex == -1)
                    {
                        break;
                    }
                    
                    scriptContent = scriptContent.Remove(startIndex, beginTagLength);

                    if (startIndex > 0)
                    {
                        scriptContent = scriptContent.RemoveNewLine(startIndex - 1);
                    }
                }
                
                while (true)
                {
                    var endIndex = scriptContent.IndexOf(endTag, StringComparison.Ordinal);

                    if (endIndex == -1)
                    {
                        break;
                    }
                    
                    scriptContent = scriptContent.Remove(endIndex, endTagLength);

                    if (endIndex > 0)
                    {
                        scriptContent = scriptContent.RemoveNewLine(endIndex - 1);
                    }
                }
                
                return;
            }
            
            while (true)
            {
                var startIndex = scriptContent.IndexOf(beginTag, StringComparison.Ordinal);

                if (startIndex == -1)
                {
                    scriptContent = scriptContent.Replace(endTag, "");
                    break;
                }
                
                var endIndex = scriptContent.IndexOf(endTag, startIndex, StringComparison.Ordinal);

                if (endIndex == -1)
                {
                    throw new ArgumentException($"Incorrect region tag: {endTag} not found!");
                }
                
                scriptContent = scriptContent.Remove(startIndex, endIndex - startIndex + endTag.Length);

                if (startIndex > 0)
                {
                    scriptContent = scriptContent.RemoveNewLine(startIndex - 1);
                }
            }
        }
        
        void IScriptCreationPostProcessor.PostProcess(string scriptAbsolutePath, ref string scriptContent,
            ScriptCreationExtraInfo extraInfo)
        {
            PostProcess(scriptAbsolutePath, ref scriptContent, extraInfo);
        }
    }
}
#endif