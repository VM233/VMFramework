using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Pools;

namespace VMFramework.Examples
{
    public sealed class RectangleIntegerDemo : MonoBehaviour
    {
        private void Start()
        {
            // 创建一个[0, 2]x[0, 3]的整数矩形，用尺寸初始化
            // 这里乘号是笛卡尔积，相当于一个从坐标(0, 0)到(2, 3)的矩形
            // Create a rectangle with size [0, 2]x[0, 3] using dimensions
            // The multiplication symbol is the Cartesian product,
            // which means a rectangle from the coordinate (0, 0) to (2, 3)
            var a = new RectangleInteger(new Vector2Int(3, 4));

            // 创建一个[2, 3]x[5, 6]的整数矩形，用最小值和最大值初始化
            // Create a rectangle with minimum value (2, 3) and maximum value (5, 6),
            // which is equivalent to [2, 3]x[5, 6]
            var b = new RectangleInteger(new Vector2Int(2, 3), new Vector2Int(5, 6));

            // 复制一个整数矩形
            // Copy a rectangle
            var c = new RectangleInteger(a);

            // 用RectangleInteger.unit快速创建一个[0, 1]x[0, 1]的整数矩形
            // Create a rectangle with size [0, 1]x[0, 1] using RectangleInteger.unit
            var d = RectangleInteger.Unit;

            // [0, 2]x[0, 3]与(3, 4)相加，得到一个[3, 6]x[3, 7]的整数矩形
            // Add a vector to a rectangle to get a new rectangle with shifted coordinates
            var e = a + new Vector2Int(3, 4);

            // [3, 6]x[3, 7]加负号，得到一个[-6, -3]x[-7, -3]的整数矩形
            // 与相对于坐标原点作镜像一样
            // Negate a rectangle to get a new rectangle with negated coordinates,
            // which is equivalent to mirroring it around the coordinate origin
            var f = -e;

            // 创建一个[0, 2]x[0, 3]的整数矩形，即从坐标(0, 0)到(2, 3)的矩形
            // Create a [0, 2]x[0, 3] rectangle with minimum value (0, 0) and maximum value (2, 3)
            var rangeX = RangeInteger.Unit * 2;
            var rangeY = RangeInteger.Unit * 3;
            var rectangle = rangeX * rangeY;

            // 获取[0, 2]x[0, 3]这个矩形上所有边界上的整数点
            // Get all integer points on the boundaries of the [0, 2]x[0, 3] rectangle
            foreach (var point in rectangle.GetBoundary())
            {
                Debug.Log(point);
            }

            // 获取[0, 2]x[0, 3]这个矩形内的所有整数点（包括边界）
            // Get all integer points inside the [0, 2]x[0, 3] rectangle, including the boundaries
            foreach (var point in rectangle)
            {
                Debug.Log(point);
            }

            // 从[0, 2]x[0, 3]这个矩形内随机取5个不同的随机点（包括边界）
            // Get 5 unique random points inside the [0, 2]x[0, 3] rectangle, including the boundaries
            var randomPoints = ListPool<Vector2Int>.Default.Get();
            randomPoints.Clear();
            rectangle.GetRandomUniquePoints(5, randomPoints);
            foreach (var point in randomPoints)
            {
                Debug.Log(point);
            }
            randomPoints.ReturnToDefaultPool();
        }
    }
}
