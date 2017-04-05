using System;

namespace Abp.Auditing
{
    /// <summary>
    /// Used to disable auditing for a single method or all methods of a class or interface.
    /// 为一个方法或是一个类（接口）中的所有方法禁用审计
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
    public class DisableAuditingAttribute : Attribute
    {

    }
}