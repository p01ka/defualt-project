using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IdxZero.Utils.Extensions
{
    public static class VectorExtensions
    {
        public static Vector3 SetZ(this Vector2 original, float z)
        {
            return new Vector3(original.x, original.y, z);
        }

        public static Vector3 SetZ(this Vector3 original, float z)
        {
            return new Vector3(original.x, original.y, z);
        }

        public static Vector3 SetX(this Vector3 original, float x)
        {
            return new Vector3(x, original.y, original.z);
        }

        public static Vector3 SetY(this Vector3 original, float y)
        {
            return new Vector3(original.x, y, original.z);
        }

        public static Vector3 SetZeroPos(this Vector3 original)
        {
            return new Vector3(0, 0, original.z);
        }

        public static Vector2 SetX(this Vector2 original, float x)
        {
            return new Vector2(x, original.y);
        }

        public static Vector3 GetVectorFromFloat(this float number)
        {
            return new Vector3(number, number, number);
        }

        public static Vector3 TransformToWorldByParentPosition(this Vector3 original, Vector3 parentPosition)
        {
            return new Vector3(original.x + parentPosition.x, original.y + parentPosition.y, original.z + parentPosition.z);
        }

        public static Vector2 TransformToWorldByParentPosition(this Vector2 original, Vector2 parentPosition)
        {
            return new Vector2(original.x + parentPosition.x, original.y + parentPosition.y);
        }

        public static Vector3 GetSignsVector(this Vector3 original)
        {
            return new Vector3(Math.Sign(original.x), Math.Sign(original.y), Math.Sign(original.z));
        }

        public static Vector3 GetYVectorFromFloat(this float value)
        {
            return new Vector3(0, value, 0);
        }

        public static Vector3 GetZVectorFromFloat(this float value)
        {
            return new Vector3(0, 0, value);
        }

        public static Vector3 Subtract(this Vector3 original, Vector3 other)
        {
            return new Vector3(original.x - other.x,
                               original.y - other.y,
                               original.z - other.z);
        }

        public static Vector3 Add(this Vector3 original, Vector3 other)
        {
            return new Vector3(original.x + other.x,
                               original.y + other.y,
                               original.z + other.z);
        }

        public static Vector3 Multiply(this Vector3 original, float factor)
        {
            return new Vector3(original.x * factor,
                               original.y * factor,
                               original.z * factor);
        }

        public static Color SetAlpha(this Color original, float alpha)
        {
            return new Color(original.r, original.g, original.b, alpha);
        }
    }
    public static class ListExtension
    {
        //    list: List<T> to resize
        //    size: desired new size
        // element: default value to insert

        public static void Resize<T>(this List<T> list, int size, T element = default(T))
        {
            int count = list.Count;

            if (size < count)
            {
                list.RemoveRange(size, count - size);
            }
            else if (size > count)
            {
                if (size > list.Capacity)   // Optimization
                    list.Capacity = size;

                list.AddRange(Enumerable.Repeat(element, size - count));
            }
        }

        public static void AddWithResize<T>(this List<T> list, T element = default(T))
        {
            int count = list.Count;
            list.Capacity = count + 1;
            list.Add(element);
        }
    }
}