using System;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.OdinExtensions;
using VMFramework.Timers;

namespace VMFramework.Properties
{
    [PreviewComposite]
    public class TickTimerProperty : ITimerProperty<uint>, ITimer<ulong>, IResettable
    {
        public string Name { get; set; }

        [ShowInInspector]
        public object Owner { get; private set; }

        [DelayedProperty]
        [ShowInInspector]
        public uint Value
        {
            get => GetValue();
            set => SetValue(value, initial: false);
        }

        public object ObjectValue => GetValue();

        [DelayedProperty]
        [ShowInInspector]
        public float Scale
        {
            get => GetScale();
            set => SetScale(value);
        }

        [ShowInInspector, ReadOnly]
        public bool IsTimerActive { get; private set; } = false;

        public event TimerEndHandler OnEnd;
        public event PropertyDirtyHandler OnDirty;

        protected ulong expectedTime = 0;
        protected uint existingValue = 0;
        protected float scale = 1;

        public Func<uint> initialValueGetter;

        public virtual void SetInitialValue(uint value)
        {
            initialValueGetter = () => value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetOwner(object owner)
        {
            Owner = owner;
        }

        public void SetObjectValue(object value, bool initial)
        {
            SetValue((uint)value, initial);
        }

        public void SetValue(uint value, bool initial)
        {
            if (IsTimerActive == false)
            {
                existingValue = value;
                return;
            }

            if (value <= 0)
            {
                if (GetValue() > 0)
                {
                    LogicTickTimerManager.Instance.Stop(this);
                    OnDirty?.Invoke(this, initial);
                    OnEnd?.Invoke(Owner);
                }

                return;
            }

            LogicTickTimerManager.Instance.TryStop(this);
            LogicTickTimerManager.Instance.Add(this, GetActualValue(value));
            OnDirty?.Invoke(this, initial);
        }

        public uint GetValue()
        {
            if (IsTimerActive == false)
            {
                return existingValue;
            }

            return (uint)expectedTime.MinusAndClampZero(LogicTickManager.Instance.Tick);
        }

        public float GetOriginalValue()
        {
            return GetValue() / scale;
        }

        public void SetScale(float scale)
        {
            var oldScale = this.scale;
            this.scale = scale;
            if (IsTimerActive)
            {
                var value = GetValue();
                if (value > 0)
                {
                    LogicTickTimerManager.Instance.Stop(this);
                    var originalValue = value / oldScale;
                    var actualValue = GetActualValue(originalValue);
                    LogicTickTimerManager.Instance.Add(this, actualValue);
                }
            }

            OnDirty?.Invoke(this, initial: false);
        }

        public float GetScale()
        {
            return scale;
        }

        public void ClearState()
        {
            if (IsTimerActive)
            {
                UnityEngine.Debug.LogError($"Timer {nameof(TickTimerProperty)} of {Owner} cannot clear state while active.");
                return;
            }

            expectedTime = 0;
            existingValue = 0;
            scale = 1;
        }

        public bool TryReset()
        {
            if (initialValueGetter == null)
            {
                return false;
            }

            var newValue = initialValueGetter();
            SetValue(newValue, initial: true);
            return true;
        }

        [Button]
        public bool StartTimer()
        {
            if (IsTimerActive)
            {
                UnityEngine.Debug.LogWarning($"Timer {this} of {Owner} is already active.");
                return false;
            }

            IsTimerActive = true;

            if (existingValue > 0)
            {
                LogicTickTimerManager.Instance.Add(this, GetActualValue(existingValue));
            }

            return true;
        }

        [Button]
        public bool StopTimer()
        {
            if (IsTimerActive == false)
            {
                return false;
            }

            IsTimerActive = false;
            LogicTickTimerManager.Instance.TryStop(this);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected uint GetActualValue(double value)
        {
            return (uint)System.Math.Round(value * scale);
        }

        public override string ToString()
        {
            return $"Value: {Value}, Scale: {Scale}";
        }

        void ITimer<ulong>.OnStart(ulong startedTime, ulong expectedTime)
        {
            this.expectedTime = expectedTime;
        }

        void ITimer<ulong>.OnStopped(ulong stoppedTime)
        {
            existingValue = (uint)expectedTime.MinusAndClampZero(stoppedTime);
            expectedTime = stoppedTime;
        }

        void ITimer<ulong>.OnTimed()
        {
            existingValue = 0;
            OnDirty?.Invoke(this, initial: false);
            OnEnd?.Invoke(Owner);
        }

        #region Proprity Queue Node

        ulong IGenericPriorityQueueNode<ulong>.Priority { get; set; }

        int IGenericPriorityQueueNode<ulong>.QueueIndex { get; set; }

        long IGenericPriorityQueueNode<ulong>.InsertionIndex { get; set; }

        #endregion
    }
}