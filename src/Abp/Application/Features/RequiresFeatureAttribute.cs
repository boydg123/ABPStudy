using System;

namespace Abp.Application.Features
{
    /// <summary>
    /// This attribute can be used on a class/method to declare that given class/method is available only if required feature(s) are enabled.
    /// 这个特性可以在类/方法上使用，以声明给定的类/方法只有当需要的功能被启用时才可用。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequiresFeatureAttribute : Attribute
    {
        /// <summary>
        /// A list of features to be checked if they are enabled.
        /// 如果功能列表可用，它们将被检查
        /// </summary>
        public string[] Features { get; private set; }

        /// <summary>
        /// If this property is set to true, all of the <see cref="Features"/> must be enabled.
        /// 如果此属性设置为true，所有的功能必须可用
        /// If it's false, at least one of the <see cref="Features"/> must be enabled.
        /// 如果设置为false.至少一个功能必须可用
        /// Default: false.
        /// </summary>
        public bool RequiresAll { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="RequiresFeatureAttribute"/> class.
        /// 构造函数
        /// </summary>
        /// <param name="features">A list of features to be checked if they are enabled / 如果功能列表可用，它们将被检查</param>
        public RequiresFeatureAttribute(params string[] features)
        {
            Features = features;
        }
    }
}