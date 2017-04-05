using System.Collections.Generic;

namespace Abp.Runtime.Validation
{
    /// <summary>
    /// 值验证器
    /// </summary>
    public interface IValueValidator
    {
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 所引起
        /// </summary>
        /// <param name="key">通过key获取对象</param>
        /// <returns></returns>
        object this[string key] { get; set; }

        /// <summary>
        /// 属性字典
        /// </summary>
        IDictionary<string, object> Attributes { get; }

        /// <summary>
        /// 是否验证
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool IsValid(object value);
    }
}