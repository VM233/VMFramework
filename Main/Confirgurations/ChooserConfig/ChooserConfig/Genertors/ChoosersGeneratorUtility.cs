using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VMFramework.Core;

namespace VMFramework.Configuration
{
    public static class ChoosersGeneratorUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Generate<T, TChoosers>(this IReadOnlyCollection<IChoosersGenerator<T>> generators,
            ref TChoosers[] choosersArray)
            where TChoosers : ICollection<IChooser<T>>, new()
        {
            choosersArray = new TChoosers[generators.Count];

            int i = 0;
            foreach (var generator in generators)
            {
                if (generator != null)
                {
                    var choosers = new TChoosers();
                    generator.GenerateChoosers(choosers);

                    choosersArray[i] = choosers;
                }
                else
                {
                    choosersArray[i] = default;
                }

                i++;
            }
        }
    }
}