using System;

namespace Abp.AutoMapper
{
    /// <summary>
    /// 映射来源特性
    /// </summary>
    public class AutoMapFromAttribute : AutoMapAttribute
    {
        /// <summary>
        /// 映射来源
        /// </summary>
        public override AutoMapDirection Direction
        {
            get { return AutoMapDirection.From; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="targetTypes">目标类型</param>
        public AutoMapFromAttribute(params Type[] targetTypes)
            : base(targetTypes)
        {

        }
    }
}