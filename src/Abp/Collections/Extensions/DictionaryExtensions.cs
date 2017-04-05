using System;
using System.Collections.Generic;

namespace Abp.Collections.Extensions
{
    /// <summary>
    /// Extension methods for Dictionary.
    /// Dictionary扩展方法
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// This method is used to try to get a value in a dictionary if it does exists.
        /// 此方法用于尝试从一个dictionary中获取一个值，看值是否存在
        /// </summary>
        /// <typeparam name="T">Type of the value / 值的类型</typeparam>
        /// <param name="dictionary">The collection object / 字典对象</param>
        /// <param name="key">Key / 键</param>
        /// <param name="value">Value of the key (or default value if key not exists) / 键值(不存在为默认值)</param>
        /// <returns>True if key does exists in the dictionary / 如果存在，返回true</returns>
        public static bool TryGetValue<T>(this IDictionary<string, object> dictionary, string key, out T value)
        {
            object valueObj;
            if (dictionary.TryGetValue(key, out valueObj) && valueObj is T)
            {
                value = (T)valueObj;
                return true;
            }

            value = default(T);
            return false;
        }

        /// <summary>
        /// Gets a value from the dictionary with given key. Returns default value if can not find.
        /// 从字典中获取给定键对应的值，如果不存在，返回默认值
        /// </summary>
        /// <param name="dictionary">Dictionary to check and get / 字典</param>
        /// <param name="key">Key to find the value / 键</param>
        /// <typeparam name="TKey">Type of the key / T键类型</typeparam>
        /// <typeparam name="TValue">Type of the value / 值类型</typeparam>
        /// <returns>Value if found, default if can not found. / 值，如果不存在</returns>
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            TValue obj;
            return dictionary.TryGetValue(key, out obj) ? obj : default(TValue);
        }

        /// <summary>
        /// Gets a value from the dictionary with given key. Returns default value if can not find.
        /// 从字典中获取给定键对应的值，如果不存在，返回默认值
        /// </summary>
        /// <param name="dictionary">Dictionary to check and get / 字典</param>
        /// <param name="key">Key to find the value / 键</param>
        /// <param name="factory">A factory method used to create the value if not found in the dictionary / 如果在字典中找不到值，则使用工厂方法创建值</param>
        /// <typeparam name="TKey">Type of the key / T键类型</typeparam>
        /// <typeparam name="TValue">Type of the value / 值类型</typeparam>
        /// <returns>Value if found, default if can not found. / 值，如果不存在</returns>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> factory)
        {
            TValue obj;
            if (dictionary.TryGetValue(key, out obj))
            {
                return obj;
            }

            return dictionary[key] = factory(key);
        }

        /// <summary>
        /// Gets a value from the dictionary with given key. Returns default value if can not find.
        /// 从字典中获取给定键对应的值，如果不存在，返回默认值
        /// </summary>
        /// <param name="dictionary">Dictionary to check and get / 字典</param>
        /// <param name="key">Key to find the value / 键</param>
        /// <param name="factory">A factory method used to create the value if not found in the dictionary / 如果在字典中找不到值，则使用工厂方法创建值</param>
        /// <typeparam name="TKey">Type of the key / T键类型</typeparam>
        /// <typeparam name="TValue">Type of the value / 值类型</typeparam>
        /// <returns>Value if found, default if can not found. / 值，如果不存在</returns>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> factory)
        {
            return dictionary.GetOrAdd(key, k => factory());
        }
    }
}