using System;
using System.Reflection;

namespace Abp.Reflection.Extensions
{
    /// <summary>
    /// Extensions to <see cref="MemberInfo"/>.
    /// <see cref="MemberInfo"/>扩展方法.
    /// </summary>
    public static class MemberInfoExtensions
    {
        /// <summary>
        /// Gets a single attribute for a member.
        /// 获取成员的单个属性
        /// </summary>
        /// <typeparam name="TAttribute">Type of the attribute / 属性的类型</typeparam>
        /// <param name="memberInfo">The member that will be checked for the attribute / 将被检查特性的成员</param>
        /// <param name="inherit">Include inherited attributes / 是否包含继承的特性</param>
        /// <returns>Returns the attribute object if found. Returns null if not found. / 如果发现指定的特定则返回，否则返回null</returns>
        public static TAttribute GetSingleAttributeOrNull<TAttribute>(this MemberInfo memberInfo, bool inherit = true)
            where TAttribute : Attribute
        {
            if (memberInfo == null)
            {
                throw new ArgumentNullException("memberInfo");
            }

            var attrs = memberInfo.GetCustomAttributes(typeof(TAttribute), inherit);
            if (attrs.Length > 0)
            {
                return (TAttribute)attrs[0];
            }

            return default(TAttribute);
        }


        /// <summary>
        /// 获取类型的单个属性
        /// </summary>
        /// <typeparam name="TAttribute">属性的类型</typeparam>
        /// <param name="type">被检查的类型</param>
        /// <param name="inherit">是否包含继承的特性</param>
        /// <returns></returns>
        public static TAttribute GetSingleAttributeOfTypeOrBaseTypesOrNull<TAttribute>(this Type type, bool inherit = true)
            where TAttribute : Attribute
        {
            var attr = type.GetSingleAttributeOrNull<TAttribute>();
            if (attr != null)
            {
                return attr;
            }

            if (type.BaseType == null)
            {
                return null;
            }

            return type.BaseType.GetSingleAttributeOfTypeOrBaseTypesOrNull<TAttribute>(inherit);
        }
    }
}
