using System;

namespace Abp
{
    /// <summary>
    /// Can be used to store Name/Value (or Key/Value) pairs.
    /// 可用于发送或接收Name/Value (或者 Key/Value)对
    /// </summary>
    [Serializable]
    public class NameValue : NameValue<string>
    {
        /// <summary>
        /// Creates a new <see cref="NameValue"/>.
        /// 创建一个新<see cref="NameValue"/>对象
        /// </summary>
        public NameValue()
        {

        }

        /// <summary>
        /// Creates a new <see cref="NameValue"/>.
        /// 创建一个新<see cref="NameValue"/>对象
        /// </summary>
        public NameValue(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }

    /// <summary>
    /// Can be used to store Name/Value (or Key/Value) pairs.
    /// 可用于发送或接收Name/Value (或者 Key/Value)对
    /// </summary>
    [Serializable]
    public class NameValue<T>
    {
        /// <summary>
        /// Name.
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Value.
        /// 值
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Creates a new <see cref="NameValue"/>.
        /// 创建一个新的<see cref="NameValue"/>对象
        /// </summary>
        public NameValue()
        {

        }

        /// <summary>
        /// Creates a new <see cref="NameValue"/>.
        /// 创建一个新的<see cref="NameValue"/>对象
        /// </summary>
        public NameValue(string name, T value)
        {
            Name = name;
            Value = value;
        }
    }
}