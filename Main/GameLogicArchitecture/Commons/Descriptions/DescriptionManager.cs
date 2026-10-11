using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Localization;
using VMFramework.Core;

namespace VMFramework.GameLogicArchitecture
{
    public class DescriptionManager : MonoBehaviour
    {
        public delegate bool GenerateHandler(out string description);
        public delegate Task<DescriptionResult> GenerateAsyncHandler(CancellationToken cancellationToken);
        public delegate void GetHandler(string type, BindingRequest request);
        public event GetHandler OnGetDescription;

        public readonly struct DescriptionResult
        {
            public bool Success { get; }
            public string Description { get; }
            public DescriptionResult(bool success, string description)
            { Success = success; Description = description; }
        }

        protected sealed class Generator
        {
            public GenerateHandler Generate { get; }
            public GenerateAsyncHandler GenerateAsync { get; }
            public Generator(GenerateHandler generate, GenerateAsyncHandler generateAsync)
            {
                Generate = generate ?? throw new ArgumentNullException(nameof(generate));
                GenerateAsync = generateAsync ?? throw new ArgumentNullException(nameof(generateAsync));
            }
        }

        public abstract class BindingRequest
        {
            public abstract void Bind(Action synchronous, Func<CancellationToken, Task> asynchronous);
        }
        private sealed class SynchronousBindingRequest : BindingRequest
        {
            public override void Bind(Action synchronous, Func<CancellationToken, Task> asynchronous) => synchronous();
        }
        private sealed class AsynchronousBindingRequest : BindingRequest
        {
            private readonly CancellationToken cancellationToken;
            public Task Completion { get; private set; } = Task.CompletedTask;
            public AsynchronousBindingRequest(CancellationToken token) => cancellationToken = token;
            public override void Bind(Action synchronous, Func<CancellationToken, Task> asynchronous) =>
                Completion = BindAfterAsync(Completion, asynchronous);
            private async Task BindAfterAsync(Task preceding, Func<CancellationToken, Task> binding)
            {
                await preceding;
                cancellationToken.ThrowIfCancellationRequested();
                await binding(cancellationToken);
            }
        }

        [TitleGroup(ComponentNames.RUNTIME), ShowInInspector]
        protected readonly Dictionary<string, Generator> generators = new();
        [TitleGroup(ComponentNames.RUNTIME), ShowInInspector]
        protected readonly Dictionary<string, HashSet<LocalizedString>> localizedStringsLookup = new();
        protected readonly HashSet<LocalizedString> allLocalizedStrings = new();

        public virtual bool TryGetDescription(string type, out string description)
        {
            if (generators.TryGetValue(type, out var generator))
            {
                OnGetDescription?.Invoke(type, new SynchronousBindingRequest());
                return generator.Generate(out description);
            }
            description = null;
            return false;
        }
        public virtual async Task<DescriptionResult> TryGetDescriptionAsync(string type, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!generators.TryGetValue(type, out var generator)) return new DescriptionResult(false, null);
            var request = new AsynchronousBindingRequest(cancellationToken);
            Exception notificationFailure = null;
            try { OnGetDescription?.Invoke(type, request); }
            catch (Exception error) { notificationFailure = error; }
            try { await request.Completion; }
            catch (Exception bindingFailure)
            {
                if (notificationFailure != null)
                    throw new AggregateException("Description notification and its accepted binding failed.", notificationFailure, bindingFailure);
                throw;
            }
            if (notificationFailure != null) ExceptionDispatchInfo.Capture(notificationFailure).Throw();
            cancellationToken.ThrowIfCancellationRequested();
            return await generator.GenerateAsync(cancellationToken);
        }
        public virtual bool TryGetLocalizedStrings(string key, out IReadOnlyCollection<LocalizedString> localizedStrings)
        {
            if (localizedStringsLookup.TryGetValue(key, out var strings))
            { localizedStrings = strings; return true; }
            localizedStrings = null;
            return false;
        }
        public virtual IReadOnlyCollection<LocalizedString> GetLocalizedStrings() => allLocalizedStrings;
        public virtual void Register(string type, GenerateHandler generate, GenerateAsyncHandler generateAsync)
        {
            if (type.IsNullOrEmpty()) throw new ArgumentException("A description generator requires a nonempty type.", nameof(type));
            if (!generators.TryAdd(type, new Generator(generate, generateAsync)))
                throw new InvalidOperationException($"Description type '{type}' already has a registered generator.");
        }
        public virtual void Register(string type, LocalizedString localizedString)
        {
            if (type.IsNullOrEmpty()) throw new ArgumentException("A localized description requires a nonempty type.", nameof(type));
            var strings = localizedStringsLookup.GetOrCreate(type);
            strings.Add(localizedString);
            allLocalizedStrings.Add(localizedString);
        }
    }
}
