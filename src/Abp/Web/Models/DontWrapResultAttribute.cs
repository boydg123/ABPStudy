using System;

namespace Abp.Web.Models
{
    /// <summary>
    /// A shortcut for <see cref="WrapResultAttribute"/> to disable wrapping by default.
    /// 禁用默认封装的<see cref="WrapResultAttribute"/>一个快捷方式
    /// It sets false to <see cref="WrapResultAttribute.WrapOnSuccess"/> and <see cref="WrapResultAttribute.WrapOnError"/>  properties.
    /// 它设置为false于<see cref="WrapResultAttribute.WrapOnSuccess"/> 和 <see cref="WrapResultAttribute.WrapOnError"/>属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method)]
    public class DontWrapResultAttribute : WrapResultAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DontWrapResultAttribute"/> class.
        /// 初始化 <see cref="DontWrapResultAttribute"/> 类一个新的实例
        /// </summary>
        public DontWrapResultAttribute()
            : base(false, false)
        {

        }
    }
}