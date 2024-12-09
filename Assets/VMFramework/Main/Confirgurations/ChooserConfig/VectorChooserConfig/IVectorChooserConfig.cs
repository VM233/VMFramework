using System;

namespace VMFramework.Configuration
{
    public interface IVectorChooserConfig<out TVector> : IChooserConfig<TVector>
        where TVector : struct, IEquatable<TVector>
    {
        
    }
}