using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System;
using System.Runtime.CompilerServices;
using VMFramework.Core.Generic;
using VMFramework.OdinExtensions;

namespace VMFramework.Configuration
{
    [PreviewComposite]
    [TypeValidation]
    public abstract partial class KCubeConfig<TPoint> : BaseConfig, IKCubeConfig<TPoint>, ICloneable
        where TPoint : struct, IEquatable<TPoint>
    {
        protected const string WRAPPER_GROUP = "WrapperGroup";

        protected const string MIN_MAX_VALUE_GROUP = WRAPPER_GROUP + "/MinMaxValueGroup";

        protected const string INFO_VALUE_GROUP = WRAPPER_GROUP + "/InfoValueGroup";

        protected virtual bool requireCheckSize => true;

        [HorizontalGroup(WRAPPER_GROUP), VerticalGroup(MIN_MAX_VALUE_GROUP)]
        [JsonProperty]
        public TPoint min;

        [VerticalGroup(MIN_MAX_VALUE_GROUP)]
        [InfoBox("The min value cannot be greater than the max value.", InfoMessageType.Error,
            nameof(displayMaxLessThanMinError))]
        [JsonProperty]
        public TPoint max;

        [VerticalGroup(INFO_VALUE_GROUP)]
        [ShowInInspector, DisplayAsString]
        public abstract TPoint Size
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        }

        [VerticalGroup(INFO_VALUE_GROUP)]
        [ShowInInspector, DisplayAsString]
        public abstract TPoint Pivot
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        }

        #region GUI

        private bool displayMaxLessThanMinError => requireCheckSize && max.AnyNumberBelow(min);

        #endregion

        public abstract object Clone();

        public override string ToString()
        {
            return $"[{min}, {max}]";
        }
    }
}