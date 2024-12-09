using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Pools;

namespace VMFramework.Examples
{
    public sealed class RangeIntegerDemo : MonoBehaviour
    {
        private void Start()
        {
            // 创建一个[0, 2]的整数范围，用长度初始化
            // Create a range of integers from 0 to 2, with length initialization
            var a = new RangeInteger(3);

            // 创建一个[3, 7]的整数范围，用Vector2Int的x和y分别作为最小值和最大值初始化
            // Create a range of integers from 3 to 7, with Vector2Int's x and y as minimum and maximum values
            var b = new RangeInteger(new Vector2Int(3, 7));

            // 从b复制一个整数范围
            // Copy a range of integers from b
            var c = new RangeInteger(b);

            // 用RangeInteger.unit快速创建一个[0, 1]的整数范围
            // Create a range of integers from 0 to 1, with RangeInteger.unit
            var d = RangeInteger.Unit;

            // 与3相加，得到一个[3, 4]的整数范围
            // Add 3 to a range of integers, resulting in a range of integers from 3 to 4
            var e = d + 3;

            // 与3相乘，得到一个[0, 3]的整数范围
            // Multiply a range of integers by 3, resulting in a range of integers from 0 to 3
            var f = d * 3;

            // 创建一个[3, 7]的整数范围，用最小值和最大值初始化
            // Create a range of integers from 3 to 7, with minimum and maximum values
            var rangeInt = new RangeInteger(3, 7);

            // 获取[3, 7]内的5个随机整数
            // Get 5 random integers within the range of [3, 7]
            foreach (var point in rangeInt.GetRandomItems(5))
            {
                Debug.Log(point);
            }

            // 获取[3, 7]内的5个不同的随机整数
            // Get 5 unique random integers within the range of [3, 7]
            var randomPoints = ListPool<int>.Default.Get();
            randomPoints.Clear();
            rangeInt.GetRandomUniquePoints(5, randomPoints);
            foreach (var point in randomPoints)
            {
                Debug.Log(point);
            }
            randomPoints.ReturnToDefaultPool();

            // 获取[3, 7]内的所有整数
            // Get all integers within the range of [3, 7]
            foreach (var point in rangeInt)
            {
                Debug.Log(point);
            }

            var num = 2;

            // 判断2是否在[3, 7]内
            // Check if 2 is within the range of [3, 7]
            if (rangeInt.Contains(num))
            {
                Debug.Log($"{num}在{rangeInt}内");
            }
            else
            {
                Debug.Log($"{num}不在{rangeInt}内");
            }

            // 获取2相对于[3, 7]的位置，此处输出-1
            // Get the position of 2 relative to [3, 7], which outputs -1
            var relativeNum = rangeInt.GetRelativePos(num);

            // 确保2比[3, 7]的最小值大，此处输出3
            // Ensure that 2 is greater than the minimum value of [3, 7], which outputs 3
            var clampedMinNum = rangeInt.ClampMin(num);

            // 确保2比[3, 7]的最大值小，此处输出2
            // Ensure that 2 is less than the maximum value of [3, 7], which outputs 2
            var clampedMaxNum = rangeInt.ClampMax(num);

            // 获取[3, 7]的中心点，此处输出5
            // Get the center point of [3, 7], which outputs 5
            var pivot = rangeInt.Pivot;
        }
    }
}
