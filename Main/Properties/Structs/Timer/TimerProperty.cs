using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.OdinExtensions;
using VMFramework.Timers;

namespace VMFramework.Properties
{
    [PreviewComposite]
    public class TimerProperty : ITimerProperty<float>, ITimer<double>
    {
        public string Name { get; set; }

        public object Owner { get; private set; }

        [DelayedProperty]
        [ShowInInspector]
        public float Value
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

        protected double expectedTime = 0;
        protected float existingValue = 0;
        protected float scale = 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetOwner(object owner)
        {
            Owner = owner;
        }

        public void SetObjectValue(object value, bool initial)
        {
            SetValue((float)value, initial);
        }

        public void SetValue(float value, bool initial)
        {
            if (IsTimerActive == false)
            {
                existingValue = value.ClampMin(0);
                return;
            }

            if (value <= 0)
            {
                if (GetValue() > 0)
                {
                    TimerManager.Instance.Stop(this);
                    OnDirty?.Invoke(this, initial);
                    OnEnd?.Invoke(Owner);
                }

                return;
            }

            TimerManager.Instance.TryStop(this);
            TimerManager.Instance.Add(this, GetActualValue(value));
            OnDirty?.Invoke(this, initial);
        }

        public float GetValue()
        {
            if (IsTimerActive == false)
            {
                return existingValue;
            }

            return (float)(expectedTime - TimerManager.Instance.CurrentTime).ClampMin(0);
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
                    TimerManager.Instance.Stop(this);
                    var originalValue = value / oldScale;
                    var actualValue = GetActualValue(originalValue);
                    TimerManager.Instance.Add(this, actualValue);
                }
            }

            OnDirty?.Invoke(this, initial: false);
        }

        public float GetScale()
        {
            return scale;
        }

        public void Reset()
        {
            if (IsTimerActive)
            {
                UnityEngine.Debug.LogError($"Timer {nameof(TimerProperty)} of {Owner} cannot be reset while active.");
                return;
            }

            expectedTime = 0;
            existingValue = 0;
            scale = 1;
        }

        [Button]
        public void StartTimer()
        {
            if (IsTimerActive)
            {
                UnityEngine.Debug.LogWarning($"Timer {this} of {Owner} is already active.");
                return;
            }

            IsTimerActive = true;

            if (existingValue > 0)
            {
                TimerManager.Instance.Add(this, GetActualValue(existingValue));
            }
        }

        [Button]
        public void StopTimer()
        {
            if (IsTimerActive == false)
            {
                UnityEngine.Debug.LogWarning($"Timer {this} of {Owner} is already stopped.");
                return;
            }

            IsTimerActive = false;
            TimerManager.Instance.TryStop(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected float GetActualValue(double value)
        {
            return (float)System.Math.Round(value * scale);
        }

        public override string ToString()
        {
            return $"Value: {Value}, Scale: {Scale}";
        }

        void ITimer<double>.OnStart(double startedTime, double expectedTime)
        {
            this.expectedTime = expectedTime;
        }

        void ITimer<double>.OnStopped(double stoppedTime)
        {
            existingValue = (float)(expectedTime - stoppedTime).ClampMin(0);
            expectedTime = stoppedTime;
        }

        void ITimer<double>.OnTimed()
        {
            existingValue = 0;
            OnDirty?.Invoke(this, initial: false);
            OnEnd?.Invoke(Owner);
        }

        #region Proprity Queue Node

        double IGenericPriorityQueueNode<double>.Priority { get; set; }

        int IGenericPriorityQueueNode<double>.QueueIndex { get; set; }

        long IGenericPriorityQueueNode<double>.InsertionIndex { get; set; }

        #endregion
    }
}