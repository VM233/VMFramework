using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.Procedure;

namespace VMFramework.Configuration
{
    public class ProcessorPipeline<TProcessor>
    {
        public IReadOnlyList<(TProcessor processor, int priority)> Processors => processors;
        
        [ShowInInspector]
        protected readonly List<(TProcessor processor, int priority)> processors = new();

        protected bool sortTag = false;

        public void AutoCollect()
        {
            processors.Clear();
            foreach (var processor in ReflectionUtility.GetInstancesOfDerivedClasses<TProcessor>(includingSelf: true))
            {
                processors.Add((processor, PriorityDefines.MEDIUM));
            }
            
            sortTag = true;
        }

        public void AddProcessor(TProcessor processor, int priority)
        {
            processors.Add((processor, priority));
            sortTag = true;
        }

        public async UniTask AutoCollectAsync(CancellationToken cancellationToken)
        {
            processors.Clear();
            var types = await InitializationTypeDiscovery.FindAsync(typeof(TProcessor), true, cancellationToken);
            foreach (var type in types)
                processors.Add(((TProcessor)type.CreateInstance(), PriorityDefines.MEDIUM));
            sortTag = true;
        }

        public void ClearProcessors()
        {
            processors.Clear();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void TrySort()
        {
            if (sortTag)
            {
                SortProcessors();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SortProcessors()
        {
            processors.Sort((a, b) => a.priority.CompareTo(b.priority));
        }
    }
}
