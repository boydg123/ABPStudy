using System;

namespace Abp.MultiTenancy
{
    /// <summary>
    /// Used to declare multi tenancy side of an object.
    /// 用于声明对象的多租户标记
    /// </summary>
    public class MultiTenancySideAttribute : Attribute
    {
        /// <summary>
        /// Multitenancy side.
        /// 多租户双方中的一方
        /// </summary>
        public MultiTenancySides Side { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiTenancySideAttribute"/> class.
        /// 初始化<see cref="MultiTenancySideAttribute"/> 类的一个新实例
        /// </summary>
        /// <param name="side">Multitenancy side. / 多租户双方中的一方</param>
        public MultiTenancySideAttribute(MultiTenancySides side)
        {
            Side = side;
        }
    }
}