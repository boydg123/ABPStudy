using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Abp.Reflection
{
    /// <summary>
    /// Defines helper methods for reflection.
    /// 定义反射的辅助方法
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// Checks whether <paramref name="givenType"/> implements/inherits <paramref name="genericType"/>.
        /// 检查 <paramref name="givenType"/> 是否实现/继承 <paramref name="genericType"/>.
        /// </summary>
        /// <param name="givenType">Type to check / 检查的类型</param>
        /// <param name="genericType">Generic type / 泛型类型</param>
        public static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }

            foreach (var interfaceType in givenType.GetInterfaces())
            {
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == genericType)
                {
                    return true;
                }
            }

            if (givenType.BaseType == null)
            {
                return false;
            }

            return IsAssignableToGenericType(givenType.BaseType, genericType);
        }

        /// <summary>
        /// Gets a list of attributes defined for a class member and it's declaring type including inherited attributes.
        /// 获取一个类成员的attribute定义列表，以及类的类型上的attribute，包含继承attributes
        /// </summary>
        /// <param name="inherit">Inherit attribute from base classes / 从基类继承的属性</param>
        /// <param name="memberInfo">MemberInfo / 成员信息</param>
        public static List<object> GetAttributesOfMemberAndDeclaringType(MemberInfo memberInfo, bool inherit = true)
        {
            var attributeList = new List<object>();

            attributeList.AddRange(memberInfo.GetCustomAttributes(inherit));

            //Add attributes on the class
            if (memberInfo.DeclaringType != null)
            {
                attributeList.AddRange(memberInfo.DeclaringType.GetCustomAttributes(inherit));
            }

            return attributeList;
        }

        /// <summary>
        /// Gets a list of attributes defined for a class member and it's declaring type including inherited attributes.
        /// 获取一个类成员的attribute定义列表，以及类的类型上的attribute，包含继承attributes
        /// </summary>
        /// <typeparam name="TAttribute">Type of the attribute / attribute的类型</typeparam>
        /// <param name="memberInfo">MemberInfo / 成员信息</param>
        /// <param name="inherit">Inherit attribute from base classes / 从基类继承的属性</param>
        public static List<TAttribute> GetAttributesOfMemberAndDeclaringType<TAttribute>(MemberInfo memberInfo, bool inherit = true)
            where TAttribute : Attribute
        {
            var attributeList = new List<TAttribute>();

            //Add attributes on the member
            if (memberInfo.IsDefined(typeof(TAttribute), inherit))
            {
                attributeList.AddRange(memberInfo.GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>());
            }

            //Add attributes on the class
            if (memberInfo.DeclaringType != null && memberInfo.DeclaringType.IsDefined(typeof(TAttribute), inherit))
            {
                attributeList.AddRange(memberInfo.DeclaringType.GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>());
            }

            return attributeList;
        }

        /// <summary>
        /// Tries to gets an of attribute defined for a class member and it's declaring type including inherited attributes.
        /// 试图获取为类成员定义的属性，它的声明类型包括继承属性
        /// Returns default value if it's not declared at all.
        /// 如果未声明默认值，则返回默认值
        /// </summary>
        /// <typeparam name="TAttribute">Type of the attribute / attribute的类型</typeparam>
        /// <param name="memberInfo">MemberInfo / 成员信息</param>
        /// <param name="defaultValue">Default value (null as default) / 默认值(null 为默认值)</param>
        /// <param name="inherit">Inherit attribute from base classes / 从基类继承的属性</param>
        public static TAttribute GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<TAttribute>(MemberInfo memberInfo, TAttribute defaultValue = default(TAttribute), bool inherit = true)
            where TAttribute : Attribute
        {
            //Get attribute on the member
            if (memberInfo.IsDefined(typeof(TAttribute), inherit))
            {
                return memberInfo.GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>().First();
            }

            //Get attribute from class
            if (memberInfo.DeclaringType != null && memberInfo.DeclaringType.IsDefined(typeof(TAttribute), inherit))
            {
                return memberInfo.DeclaringType.GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>().First();
            }

            return defaultValue;
        }

        /// <summary>
        /// Tries to gets an of attribute defined for a class member and it's declaring type including inherited attributes.
        /// 试图获取为类成员定义的属性，它的声明类型包括继承属性
        /// Returns default value if it's not declared at all.
        /// 如果未声明默认值，则返回默认值
        /// </summary>
        /// <typeparam name="TAttribute">Type of the attribute / attribute的类型</typeparam>
        /// <param name="memberInfo">MemberInfo / 成员信息</param>
        /// <param name="defaultValue">Default value (null as default) / 默认值(null 为默认值)</param>
        /// <param name="inherit">Inherit attribute from base classes / 从基类继承的属性</param>
        public static TAttribute GetSingleAttributeOrDefault<TAttribute>(MemberInfo memberInfo, TAttribute defaultValue = default(TAttribute), bool inherit = true)
            where TAttribute : Attribute
        {
            //Get attribute on the member
            if (memberInfo.IsDefined(typeof(TAttribute), inherit))
            {
                return memberInfo.GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>().First();
            }

            return defaultValue;
        }

        /// <summary>
        /// Gets value of a property by it's full path from given object
        /// 从给定对象的完整路径获取属性的值
        /// </summary>
        /// <param name="obj">Object to get value from / 对象获取值</param>
        /// <param name="objectType">Type of given object / 给定对象的类型</param>
        /// <param name="propertyPath">Full path of property / 属性的全路径</param>
        /// <returns></returns>
        public static object GetValueByPath(object obj, Type objectType, string propertyPath)
        {
            var value = obj;
            var currentType = objectType;
            var objectPath = currentType.FullName;
            var absolutePropertyPath = propertyPath;
            if (absolutePropertyPath.StartsWith(objectPath))
            {
                absolutePropertyPath = absolutePropertyPath.Replace(objectPath + ".", "");
            }

            foreach (var propertyName in absolutePropertyPath.Split('.'))
            {
                var property = currentType.GetProperty(propertyName);
                value = property.GetValue(value, null);
                currentType = property.PropertyType;
            }

            return value;
        }

        /// <summary>
        /// Sets value of a property by it's full path on given object
        /// 从给定对象的完整路径获取属性的值
        /// </summary>
        /// <param name="obj">对象获取值</param>
        /// <param name="objectType">给定对象的类型</param>
        /// <param name="propertyPath">属性的全路径</param>
        /// <param name="value">值</param>
        public static void SetValueByPath(object obj, Type objectType, string propertyPath, object value)
        {
            var currentType = objectType;
            PropertyInfo property;
            var objectPath = currentType.FullName;
            var absolutePropertyPath = propertyPath;
            if (absolutePropertyPath.StartsWith(objectPath))
            {
                absolutePropertyPath = absolutePropertyPath.Replace(objectPath + ".", "");
            }

            var properties = absolutePropertyPath.Split('.');

            if (properties.Length == 1)
            {
                property = objectType.GetProperty(properties.First());
                property.SetValue(obj, value);
                return;
            }

            for (int i = 0; i < properties.Length - 1; i++)
            {
                property = currentType.GetProperty(properties[i]);
                obj = property.GetValue(obj, null);
                currentType = property.PropertyType;
            }

            property = currentType.GetProperty(properties.Last());
            property.SetValue(obj, value);
        }
    }
}