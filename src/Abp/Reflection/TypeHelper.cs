using System;

namespace Abp.Reflection
{
    /// <summary>
    /// Some simple type-checking methods used internally.
    /// 一些内部使用的简单类型检查方法
    /// </summary>
    public static class TypeHelper
    {
        /// <summary>
        /// 检查对象是否是方法
        /// </summary>
        /// <param name="obj">被检测的对象</param>
        /// <returns></returns>
        public static bool IsFunc(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var type = obj.GetType();
            if (!type.IsGenericType)
            {
                return false;
            }

            return type.GetGenericTypeDefinition() == typeof(Func<>);
        }

        /// <summary>
        /// 检查对象是否是方法
        /// </summary>
        /// <typeparam name="TReturn">方法返回的类型</typeparam>
        /// <param name="obj">被检查的对象</param>
        /// <returns></returns>
        public static bool IsFunc<TReturn>(object obj)
        {
            return obj != null && obj.GetType() == typeof(Func<TReturn>);
        }

        /// <summary>
        /// 是否是原始类型扩展包含空对象
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static bool IsPrimitiveExtendedIncludingNullable(Type type)
        {
            if (IsPrimitiveExtended(type))
            {
                return true;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return IsPrimitiveExtended(type.GenericTypeArguments[0]);
            }

            return false;
        }

        /// <summary>
        /// 是否是原始类型的扩展
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        private static bool IsPrimitiveExtended(Type type)
        {
            if (type.IsPrimitive)
            {
                return true;
            }

            return type == typeof (string) ||
                   type == typeof (decimal) ||
                   type == typeof (DateTime) ||
                   type == typeof (DateTimeOffset) ||
                   type == typeof (TimeSpan) ||
                   type == typeof (Guid);
        }
    }
}
