using System;
using System.Collections.Generic;
using VMFramework.Core;

namespace VMFramework.Configuration
{
    public interface ISingleKCubeChooserConfig<TVector, TRange> : IRangeChooserConfig<TVector>, 
        IKCubeConfig<TVector>, IWrapperChooserConfig<TVector, TVector>
        where TVector : struct, IEquatable<TVector>
        where TRange : IMinMaxOwner<TVector>, IRandomItemProvider<TVector>
    {
        IEnumerable<TVector> ICountableChooserConfig<TVector>.GetAvailableValues()
        {
            yield return Min;
            yield return Max;
        }

        IEnumerable<TVector> IWrapperChooserConfig<TVector, TVector>.GetAvailableWrappers()
        {
            yield return Min;
            yield return Max;
        }

        void IWrapperChooserConfig<TVector, TVector>.SetAvailableValues(Func<TVector, TVector> setter)
        {
            Min = setter(Min);
            Max = setter(Max);
        }

        void IChooser.ResetChooser()
        {
            // Do nothing, as the range is already set.
        }

        IChooser<TVector> IChooserGenerator<TVector>.GenerateNewChooser() => this;
    }
}