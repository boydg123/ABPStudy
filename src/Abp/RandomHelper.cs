using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Collections.Extensions;

namespace Abp
{
    /// <summary>
    /// A shortcut to use <see cref="Random"/> class.Also provides some useful methods.
    /// <see cref="Random"/>的快捷使用类，同时提供一些有用的方法
    /// </summary>
    public static class RandomHelper
    {
        private static readonly Random Rnd = new Random();

        /// <summary>
        /// Returns a random number within a specified range.
        /// 返回一个指定范围的随机数
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned. / 返回的随机数的下界（随机数可取该下界值）。</param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue. / 返回的随机数的上界（随机数不能取该上界值）。 maxValue 必须大于或等于 minValue</param>
        /// <returns>
        /// A 32-bit signed integer greater than or equal to minValue and less than maxValue; 
        /// that is, the range of return values includes minValue but not maxValue.If minValue equals maxValue, minValue is returned. 
        ///  一个大于等于 minValue 且小于 maxValue 的 32 位带符号整数，即：返回的值范围包括 minValue 但不包括 maxValue。
        ///  如果 minValue 等于 maxValue，则返回 minValue。
        /// </returns>
        public static int GetRandom(int minValue, int maxValue)
        {
            lock (Rnd)
            {
                return Rnd.Next(minValue, maxValue);
            }
        }

        /// <summary>
        /// Returns a nonnegative random number less than the specified maximum.
        /// 返回一个小于所指定最大值的非负随机数
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated. maxValue must be greater than or equal to zero.  / T要生成的随机数的上限（随机数不能取该上限值）。 maxValue 必须大于或等于零</param>
        /// <returns>
        /// A 32-bit signed integer greater than or equal to zero, and less than maxValue; 
        /// that is, the range of return values ordinarily includes zero but not maxValue. However, if maxValue equals zero, maxValue is returned.
        /// 大于等于零且小于 maxValue 的 32 位带符号整数，即：返回值的范围通常包括零但不包括 maxValue。 不过，如果 maxValue.等于零，则返回 maxValue。
        /// </returns>
        public static int GetRandom(int maxValue)
        {
            lock (Rnd)
            {
                return Rnd.Next(maxValue);
            }
        }

        /// <summary>
        /// Returns a nonnegative random number.
        /// 返回一个非负随机数
        /// </summary>
        /// <returns>A 32-bit signed integer greater than or equal to zero and less than <see cref="int.MaxValue"/>. / 大于等于零且小于 System.Int32.MaxValue 的 32 位带符号整数.</returns>
        public static int GetRandom()
        {
            lock (Rnd)
            {
                return Rnd.Next();
            }
        }

        /// <summary>
        /// Gets random of given objects.
        /// 获取给定的对象集合中的随机对象
        /// </summary>
        /// <typeparam name="T">Type of the objects / 对象集合中的元素类型</typeparam>
        /// <param name="objs">List of object to select a random one / 从其中产生随机项的集合</param>
        public static T GetRandomOf<T>(params T[] objs)
        {
            if (objs.IsNullOrEmpty())
            {
                throw new ArgumentException("objs can not be null or empty!", "objs");
            }

            return objs[GetRandom(0, objs.Length)];
        }

        /// <summary>
        /// Generates a randomized list from given enumerable.
        /// 从给定的可枚举生成一个随机的列表
        /// </summary>
        /// <typeparam name="T">Type of items in the list / 列表项类型</typeparam>
        /// <param name="items">items / 列表集合</param>
        public static List<T> GenerateRandomizedList<T>(IEnumerable<T> items)
        {
            var currentList = new List<T>(items);
            var randomList = new List<T>();

            while (currentList.Any())
            {
                var randomIndex = RandomHelper.GetRandom(0, currentList.Count);
                randomList.Add(currentList[randomIndex]);
                currentList.RemoveAt(randomIndex);
            }

            return randomList;
        }
    }
}
