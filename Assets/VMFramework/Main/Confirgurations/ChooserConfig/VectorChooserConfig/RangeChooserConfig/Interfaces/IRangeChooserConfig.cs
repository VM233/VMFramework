using System;
using VMFramework.Core;

namespace VMFramework.Configuration
{
    public interface IRangeChooserConfig<TVector> : IVectorChooserConfig<TVector>, IMinMaxOwner<TVector> 
        where TVector : struct, IEquatable<TVector>
    {
        
    }
}