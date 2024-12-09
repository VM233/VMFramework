using System;
using System.Collections.Generic;
using System.Linq;
using VMFramework.Core.Linq;

namespace VMFramework.Core
{
    public partial class ListUtility
    {
        public static IEnumerable<T> CircularlyRepeat<T>(this IList<T> list, int length, int repeatStart = 0)
        {
            if (list.IsNullOrEmpty() || length <= 0)
            {
                yield break;
            }

            repeatStart = repeatStart.Clamp(0, list.Count - 1);

            int times = (length - repeatStart) / (list.Count - repeatStart);
            int extraLength = (length - repeatStart) % (list.Count - repeatStart);

            for (int i = 0; i < repeatStart; i++)
            {
                yield return list[i];
            }

            for (int t = 0; t < times; t++)
            {
                for (int i = repeatStart; i < list.Count; i++)
                {
                    yield return list[i];
                }
            }

            for (int i = 0; i < extraLength; i++)
            {
                yield return list[repeatStart + i];
            }
        }

        public static IEnumerable<T> PingPongRepeat<T>(this IList<T> list, int length, int repeatStart = 0)
        {
            if (list.IsNullOrEmpty() || length <= 0)
            {
                yield break;
            }

            repeatStart = repeatStart.Clamp(list.Count);

            if (length > repeatStart)
            {
                for (int i = 0; i < repeatStart; i++)
                {
                    yield return list[i];
                }
            }
            else
            {
                for (int i = 0; i < length; i++)
                {
                    yield return list[i];
                }

                yield break;
            }

            int pingPongLength = length - repeatStart;
            bool forward = true;
            int index = repeatStart;
            int count = 0;
            while (count < pingPongLength)
            {
                if (index < 0 || index >= list.Count)
                {
                    throw new IndexOutOfRangeException($"{nameof(index)}:{index}");
                }

                yield return list[index];
                count++;

                if (forward)
                {
                    index++;
                    if (index >= list.Count)
                    {
                        index = list.Count - 2;
                        forward = false;
                    }
                }
                else
                {
                    if (index <= repeatStart)
                    {
                        index++;
                        forward = true;
                    }
                    else
                    {
                        index--;
                    }
                }
            }
        }
    }
}