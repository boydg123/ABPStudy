using System;

namespace Abp.Application.Services.Dto
{
    /// <summary>
    /// Can be used to send/receive Name/Value (or Key/Value) pairs.
    /// 可用于发送或接收Name/Value (或者 Key/Value)对
    /// </summary>
    [Serializable]
    public class NameValueDto : NameValueDto<string>
    {
        /// <summary>
        /// Creates a new <see cref="NameValueDto"/>.
        /// 构造函数
        /// </summary>
        public NameValueDto()
        {

        }

        /// <summary>
        /// Creates a new <see cref="NameValueDto"/>.
        /// 构造函数
        /// </summary>
        public NameValueDto(string name, string value)
            : base(name, value)
        {

        }

        /// <summary>
        /// Creates a new <see cref="NameValueDto"/>.
        /// 构造函数
        /// </summary>
        /// <param name="nameValue">A <see cref="NameValue"/> object to get it's name and value / 获取对象的名称和值</param>
        public NameValueDto(NameValue nameValue)
            : this(nameValue.Name, nameValue.Value)
        {

        }
    }

    /// <summary>
    /// Can be used to send/receive Name/Value (or Key/Value) pairs.
    /// 可用于发送或接收Name/Value (或者 Key/Value)对
    /// </summary>
    [Serializable]
    public class NameValueDto<T> : NameValue<T>
    {
        /// <summary>
        /// Creates a new <see cref="NameValueDto"/>.
        /// 构造函数
        /// </summary>
        public NameValueDto()
        {

        }

        /// <summary>
        /// Creates a new <see cref="NameValueDto"/>.
        /// 构造函数
        /// </summary>
        public NameValueDto(string name, T value)
            : base(name, value)
        {

        }

        /// <summary>
        /// Creates a new <see cref="NameValueDto"/>.
        /// 构造函数
        /// </summary>
        /// <param name="nameValue">A <see cref="NameValue"/> object to get it's name and value / 获取对象的名称和值</param>
        public NameValueDto(NameValue<T> nameValue)
            : this(nameValue.Name, nameValue.Value)
        {

        }
    }
}