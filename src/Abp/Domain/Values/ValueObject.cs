using System;
using System.Linq;

namespace Abp.Domain.Values
{
    //Inspired from https://blogs.msdn.microsoft.com/cesardelatorre/2011/06/06/implementing-a-value-object-base-class-supertype-patternddd-patterns-related/
    /// <summary>
    /// Base class for value objects.
    /// 值对象的基类
    /// </summary>
    /// <typeparam name="TValueObject">The type of the value object. / 值类型的类型</typeparam>
    public abstract class ValueObject<TValueObject> : IEquatable<TValueObject>
        where TValueObject : ValueObject<TValueObject>
    {
        /// <summary>
        /// 相等操作
        /// </summary>
        /// <param name="other">值类型</param>
        /// <returns>是否相等</returns>
        public bool Equals(TValueObject other)
        {
            if ((object)other == null)
            {
                return false;
            }

            var publicProperties = GetType().GetProperties();
            if (!publicProperties.Any())
            {
                return true;
            }

            return publicProperties.All(property => Equals(property.GetValue(this, null), property.GetValue(other, null)));
        }

        /// <summary>
        /// 相等操作
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var item = obj as ValueObject<TValueObject>;
            return (object)item != null && Equals((TValueObject)item);

        }

        /// <summary>
        /// 获取HashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            const int index = 1;
            const int initialHasCode = 31;

            var publicProperties = GetType().GetProperties();

            if (!publicProperties.Any())
            {
                return initialHasCode;
            }

            var hashCode = initialHasCode;
            var changeMultiplier = false;

            foreach (var property in publicProperties)
            {
                var value = property.GetValue(this, null);

                if (value == null)
                {
                    //support {"a",null,null,"a"} != {null,"a","a",null}
                    hashCode = hashCode ^ (index * 13);
                    continue;
                }

                hashCode = hashCode * (changeMultiplier ? 59 : 114) + value.GetHashCode();
                changeMultiplier = !changeMultiplier;
            }

            return hashCode;
        }

        /// <summary>
        /// 等号操作符
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator ==(ValueObject<TValueObject> x, ValueObject<TValueObject> y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (((object)x == null) || ((object)y == null))
            {
                return false;
            }

            return x.Equals(y);
        }

        /// <summary>
        /// 非等操作符
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator !=(ValueObject<TValueObject> x, ValueObject<TValueObject> y)
        {
            return !(x == y);
        }
    }
}
