using System;

namespace Abp.Runtime.Validation
{
    /// <summary>
    /// Can be added to a method to enable auto validation if validation is disabled for it's class.
    /// 如果它的类被禁用，则可以添加到方法中启用自动验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class EnableValidationAttribute : Attribute
    {

    }
}