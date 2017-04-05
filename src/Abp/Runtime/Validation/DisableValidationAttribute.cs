using System;

namespace Abp.Runtime.Validation
{
    /// <summary>
    /// Can be added to a method to disable auto validation.
    /// 可以添加到禁用自动验证的方法中
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Property)]
    public class DisableValidationAttribute : Attribute
    {
        
    }
}