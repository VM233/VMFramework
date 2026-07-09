using System;

namespace VMFramework.Core.JSON
{
    public interface IJSONSerializationProvider
    {
        public ReadOnlySpan<IJSONSerializationReceiver> JSONSerializationReceivers { get; }
    }
}