using System.Collections;
using System.Collections.Generic;

namespace VMFramework.Core
{
    public partial struct RangeInteger
    {
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<int> GetEnumerator()
        {
            return new Enumerator(this);
        }
        
        public struct Enumerator : IEnumerator<int>
        {
            private readonly RangeInteger range;
            private int x;

            public Enumerator(RangeInteger range)
            {
                this.range = range;
                x = range.min - 1;
            }

            public int Current => x;

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                x++;
                
                return x <= range.max;
            }

            public void Reset()
            {
                x = range.min - 1;
            }

            public void Dispose() { }
        }
    }
}