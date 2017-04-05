using System;

namespace Abp.Auditing
{
    /// <summary>
    /// This attribute is used to apply audit logging for a single method or all methods of a class or interface.
    /// 此特性用于为单一方法或类（接口）里的全部方法应用审计日志
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuditedAttribute : Attribute
    {

    }
}
