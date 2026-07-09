
using System;
using Newtonsoft.Json;
using VMFramework.OdinExtensions;

namespace VMFramework.Properties
{
    [PreviewComposite]
    [Serializable]
    public struct BaseBoostFloat : IEquatable<BaseBoostFloat>, IComparable<float>
    {
        public float baseValue;
        public float boostValue;

        /// <summary>
        /// <para>值 = 基值 * (1 + 增益)</para>
        /// value = baseValue * (1 + boostValue)
        /// </summary>
        [JsonIgnore]
        public float Value => baseValue * (1 + boostValue);

        public BaseBoostFloat(float baseValue)
        {
            this.baseValue = baseValue;
            boostValue = 0;
        }

        public BaseBoostFloat(float baseValue, float boostValue)
        {
            this.baseValue = baseValue;
            this.boostValue = boostValue;
        }

        public override string ToString()
        {
            return $"Base: {baseValue}, Boost: {boostValue}, Total: {Value}";
        }

        public bool Equals(BaseBoostFloat other)
        {
            return baseValue.Equals(other.baseValue) && boostValue.Equals(other.boostValue);
        }

        public int CompareTo(float other)
        {
            return Value.CompareTo(other);
        }

        public override bool Equals(object obj)
        {
            return obj is BaseBoostFloat other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(baseValue, boostValue);
        }

        public static bool operator ==(BaseBoostFloat left, BaseBoostFloat right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BaseBoostFloat left, BaseBoostFloat right)
        {
            return !left.Equals(right);
        }

        public static BaseBoostFloat operator +(BaseBoostFloat a, BaseBoostFloat b)
        {
            return new BaseBoostFloat(a.baseValue + b.baseValue, a.boostValue + b.boostValue);
        }

        public static BaseBoostFloat operator -(BaseBoostFloat a, BaseBoostFloat b)
        {
            return new BaseBoostFloat(a.baseValue - b.baseValue, a.boostValue - b.boostValue);
        }

        public static BaseBoostFloat operator *(BaseBoostFloat a, float b)
        {
            return new BaseBoostFloat(a.baseValue * b, a.boostValue * b);
        }

        public static BaseBoostFloat operator *(float a, BaseBoostFloat b)
        {
            return new BaseBoostFloat(a * b.baseValue, a * b.boostValue);
        }

        public static BaseBoostFloat operator -(BaseBoostFloat a)
        {
            return new BaseBoostFloat(-a.baseValue, a.boostValue);
        }
    }
}