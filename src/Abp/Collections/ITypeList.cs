using System;
using System.Collections.Generic;

namespace Abp.Collections
{
    /// <summary>
    /// A shortcut for <see cref="ITypeList{TBaseType}"/> to use object as base type.
    /// <see cref="ITypeList{TBaseType}"/> 的快捷对象，使用object作为TBaseType的类型.
    /// </summary>
    public interface ITypeList : ITypeList<object>
    {

    }

    /// <summary>
    /// Extends <see cref="IList{Type}"/> to add restriction a specific base type.
    /// 扩展 <see cref="IList{Type}"/> 以限制添加一个特定的基类型.
    /// </summary>
    /// <typeparam name="TBaseType">Base Type of <see cref="Type"/>s in this list / 列表中元素的基类型 <see cref="Type"/></typeparam>
    public interface ITypeList<in TBaseType> : IList<Type>
    {
        /// <summary>
        /// Adds a type to list.
        /// 添加一个类型到列表
        /// </summary>
        /// <typeparam name="T">Type / 类型</typeparam>
        void Add<T>() where T : TBaseType;

        /// <summary>
        /// Checks if a type exists in the list.
        /// 检查一个类型是否存在列表中
        /// </summary>
        /// <typeparam name="T">Type / 类型</typeparam>
        /// <returns></returns>
        bool Contains<T>() where T : TBaseType;

        /// <summary>
        /// Removes a type from list
        /// 从列表中移除一个类型
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        void Remove<T>() where T : TBaseType;
    }
}