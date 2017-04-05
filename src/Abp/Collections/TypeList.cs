using System;
using System.Collections;
using System.Collections.Generic;

namespace Abp.Collections
{
    /// <summary>
    /// A shortcut for <see cref="TypeList{TBaseType}"/> to use object as base type.
    /// <see cref="TypeList{TBaseType}"/> 使用object作为基本类型的快捷方式
    /// </summary>
    public class TypeList : TypeList<object>, ITypeList
    {
    }

    /// <summary>
    /// Extends <see cref="List{Type}"/> to add restriction a specific base type.
    /// 扩展<see cref="List{Type}"/>,以添加一个指定的基本类型限制
    /// </summary>
    /// <typeparam name="TBaseType">Base Type of <see cref="Type"/>s in this list / 列表中的基本类型 <see cref="Type"/></typeparam>
    public class TypeList<TBaseType> : ITypeList<TBaseType>
    {
        /// <summary>
        /// Gets the count.
        /// 获取数量
        /// </summary>
        /// <value>The count. / 数量</value>
        public int Count { get { return _typeList.Count; } }

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// 获取一个值 ，指示值实例是否只读
        /// </summary>
        /// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>. / 如果此实例为只读返回<c>true</c>，否则返回<c>false</c>.</value>
        public bool IsReadOnly { get { return false; } }

        /// <summary>
        /// Gets or sets the <see cref="Type"/> at the specified index.
        /// 获取或设置指定索引对应元素 <see cref="Type"/>
        /// </summary>
        /// <param name="index">Index. / 索引</param>
        public Type this[int index]
        {
            get { return _typeList[index]; }
            set
            {
                CheckType(value);
                _typeList[index] = value;
            }
        }

        private readonly List<Type> _typeList;

        /// <summary>
        /// Creates a new <see cref="TypeList{T}"/> object.
        /// 创建一个新的 <see cref="TypeList{T}"/> 对象.
        /// </summary>
        public TypeList()
        {
            _typeList = new List<Type>();
        }

        /// <summary>
        /// 添加一个类型到列表
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        public void Add<T>() where T : TBaseType
        {
            _typeList.Add(typeof(T));
        }

        /// <summary>
        /// 添加一个类型到列表
        /// </summary>
        public void Add(Type item)
        {
            CheckType(item);
            _typeList.Add(item);
        }

        /// <summary>
        /// 插入一个类型到类型列表
        /// </summary>
        /// <param name="index">插入位置(索引)</param>
        /// <param name="item">类型</param>
        public void Insert(int index, Type item)
        {
            _typeList.Insert(index, item);
        }

        /// <summary>
        /// 在类型列表中查找类型的位置(索引)
        /// </summary>
        /// <param name="item">类型</param>
        /// <returns></returns>
        public int IndexOf(Type item)
        {
            return _typeList.IndexOf(item);
        }

        /// <summary>
        ///检查一个类型是否存在列表中
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <returns></returns>
        public bool Contains<T>() where T : TBaseType
        {
            return Contains(typeof(T));
        }

        /// <summary>
        ///检查一个类型是否存在列表中
        /// </summary>
        public bool Contains(Type item)
        {
            return _typeList.Contains(item);
        }

        /// <summary>
        /// 从列表中移除一个类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Remove<T>() where T : TBaseType
        {
            _typeList.Remove(typeof(T));
        }

        /// <summary>
        /// 从列表中移除一个类型
        /// </summary>
        public bool Remove(Type item)
        {
            return _typeList.Remove(item);
        }

        /// <summary>
        /// 移除指定索引的类型
        /// </summary>
        /// <param name="index">索引</param>
        public void RemoveAt(int index)
        {
            _typeList.RemoveAt(index);
        }

        /// <summary>
        /// 移除类型列表所有元素
        /// </summary>
        public void Clear()
        {
            _typeList.Clear();
        }

        /// <summary>
        /// 将整个泛型列表复制到兼容的一维数组中，从目标数组的指定索引位置开始放置
        /// </summary>
        /// <param name="array">类型数组</param>
        /// <param name="arrayIndex">目标数组索引</param>
        public void CopyTo(Type[] array, int arrayIndex)
        {
            _typeList.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// 返回循环访问的<see cref="List{T}"/>的枚举数
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Type> GetEnumerator()
        {
            return _typeList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _typeList.GetEnumerator();
        }

        /// <summary>
        /// 检查类型是否与基本类型兼容
        /// </summary>
        /// <param name="item"></param>
        private static void CheckType(Type item)
        {
            if (!typeof(TBaseType).IsAssignableFrom(item))
            {
                throw new ArgumentException("Given item is not type of " + typeof(TBaseType).AssemblyQualifiedName, "item");
            }
        }
    }
}