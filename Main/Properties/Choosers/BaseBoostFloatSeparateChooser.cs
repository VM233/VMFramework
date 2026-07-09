using System;
using UnityEngine;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.OdinExtensions;
using Random = System.Random;

namespace VMFramework.Properties
{
    [Serializable]
    public class BaseBoostFloatSeparateChooser : IChooser<BaseBoostFloat>
    {
        [SerializeReference]
        [IsNotNullOrEmpty]
        public IChooser<float> baseValue = new RangeFloat();

        [SerializeReference]
        [IsNotNullOrEmpty]
        public IChooser<float> boostValue = new SingleValueChooser<float>(0);

        public BaseBoostFloatSeparateChooser()
        {

        }

        public BaseBoostFloatSeparateChooser(IChooser<float> baseValue, IChooser<float> boostValue)
        {
            this.baseValue = baseValue;
            this.boostValue = boostValue;
        }

        public BaseBoostFloat GetRandomItem(Random random)
        {
            return new BaseBoostFloat(baseValue.GetRandomItem(random), boostValue.GetRandomItem(random));
        }

        public IChooser<BaseBoostFloat> GenerateNewChooser()
        {
            return new BaseBoostFloatSeparateChooser(baseValue.GenerateNewChooser(),
                boostValue.GenerateNewChooser());
        }
    }
}