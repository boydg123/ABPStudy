using System;

namespace Abp.Runtime.Validation
{
    /// <summary>
    /// 验证器特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ValidatorAttribute : Attribute
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        public ValidatorAttribute(string name)
        {
            Name = name;
        }
    }
}