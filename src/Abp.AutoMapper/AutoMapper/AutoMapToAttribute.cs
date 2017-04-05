using System;

namespace Abp.AutoMapper
{
    /// <summary>
    /// 映射目标特性
    /// </summary>
    public class AutoMapToAttribute : AutoMapAttribute
    {
        /// <summary>
        /// 映射目标
        /// </summary>
        public override AutoMapDirection Direction
        {
            get { return AutoMapDirection.To; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="targetTypes">目标类型</param>
        public AutoMapToAttribute(params Type[] targetTypes)
            : base(targetTypes)
        {

        }
    }
}