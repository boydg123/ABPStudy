using System;
using System.Reflection;
using Abp.Application.Services;
using Abp.Domain.Repositories;

namespace Abp.Domain.Uow
{
    /// <summary>
    /// A helper class to simplify unit of work process.
    /// 工作单元辅助类
    /// </summary>
    internal static class UnitOfWorkHelper
    {
        /// <summary>
        /// Returns true if UOW must be used for given type as convention.
        /// 如果工作单元必须按约定用于给定的类型，返回true
        /// </summary>
        /// <param name="type">Type to check / 类型</param>
        public static bool IsConventionalUowClass(Type type)
        {
            return typeof(IRepository).IsAssignableFrom(type) || typeof(IApplicationService).IsAssignableFrom(type);
        }

        /// <summary>
        /// Returns true if given method has UnitOfWorkAttribute attribute.
        /// 如果给定的方法应用了特性UnitOfWorkAttribute,那么返回true
        /// </summary>
        /// <param name="methodInfo">Method info to check / 需要检查的方法</param>
        public static bool HasUnitOfWorkAttribute(MemberInfo methodInfo)
        {
            return methodInfo.IsDefined(typeof(UnitOfWorkAttribute), true);
        }

        /// <summary>
        /// Returns UnitOfWorkAttribute it exists.
        /// 如果给定的方法应用了特性UnitOfWorkAttribute,返回此特性，否则返回Null
        /// </summary>
        /// <param name="methodInfo">Method info to check / 需要检查的方法</param>
        public static UnitOfWorkAttribute GetUnitOfWorkAttributeOrNull(MemberInfo methodInfo)
        {
            var attrs = methodInfo.GetCustomAttributes(typeof (UnitOfWorkAttribute), false);
            if (attrs.Length <= 0)
            {
                return null;
            }

            return (UnitOfWorkAttribute) attrs[0];
        }
    }
}