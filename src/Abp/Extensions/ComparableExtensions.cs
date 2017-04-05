using System;

namespace Abp.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IComparable{T}"/>.
    /// <see cref="IComparable{T}"/>扩展方法
    /// </summary>
    public static class ComparableExtensions
    {
        /// <summary>
        /// Checks a value is between a minimum and maximum value.
        /// 检查指定的值是否介于最大值和最小值之间
        /// </summary>
        /// <param name="value">The value to be checked / 检查的值</param>
        /// <param name="minInclusiveValue">Minimum (inclusive) value / 最小值（包含）</param>
        /// <param name="maxInclusiveValue">Maximum (inclusive) value / 最大值（包含）</param>
        public static bool IsBetween<T>(this T value, T minInclusiveValue, T maxInclusiveValue) where T : IComparable<T>
        {
            return value.CompareTo(minInclusiveValue) >= 0 && value.CompareTo(maxInclusiveValue) <= 0;
        }
    }
}