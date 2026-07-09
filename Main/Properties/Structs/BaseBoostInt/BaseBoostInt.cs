using System;
using Newtonsoft.Json;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.Properties
{
    [PreviewComposite]
    [Serializable]
    public struct BaseBoostInt : IEquatable<BaseBoostInt>
    {
        public int baseValue;
        public float boostValue;

        /// <summary>
        /// 值 = (基值 * (1 + 增益))的向下取整
        /// value = (baseValue * (1 + boostValue)).FloorToZero()
        /// </summary>
        /// <seealso cref="NearToIntegerUtility.FloorToZero(float)"/>
        [JsonIgnore]
        public int Value => (baseValue * (1 + boostValue)).FloorToZero();

        public BaseBoostInt(int baseValue)
        {
            this.baseValue = baseValue;
            boostValue = 0;
        }
        
        public BaseBoostInt(int baseValue, float boostValue)
        {
            this.baseValue = baseValue;
            this.boostValue = boostValue;
        }

        public override string ToString()
        {
            return $"Base: {baseValue}, Boost: {boostValue}, Total: {Value}";
        }

        public bool Equals(BaseBoostInt other)
        {
            return baseValue == other.baseValue && boostValue.Equals(other.boostValue);
        }

        public override bool Equals(object obj)
        {
            return obj is BaseBoostInt other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(baseValue, boostValue);
        }

        public static bool operator ==(BaseBoostInt left, BaseBoostInt right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BaseBoostInt left, BaseBoostInt right)
        {
            return !left.Equals(right);
        }
        
        public static BaseBoostInt operator +(BaseBoostInt a, BaseBoostInt b)
        {
            return new BaseBoostInt(a.baseValue + b.baseValue, a.boostValue + b.boostValue);
        }

        public static BaseBoostInt operator -(BaseBoostInt a, BaseBoostInt b)
        {
            return new BaseBoostInt(a.baseValue - b.baseValue, a.boostValue - b.boostValue);
        }
        
        public static BaseBoostInt operator *(BaseBoostInt a, int b)
        {
            return new BaseBoostInt(a.baseValue * b, a.boostValue * b);
        }
        
        public static BaseBoostInt operator *(int b, BaseBoostInt a)
        {
            return new BaseBoostInt(a.baseValue * b, a.boostValue * b);
        }
        
        public static implicit operator BaseBoostFloat(BaseBoostInt a) => new(a.baseValue, a.boostValue);
    }
}