using System;

namespace Abp.Reflection
{
    /// <summary>
    /// 查找类型的接口
    /// </summary>
    public interface ITypeFinder
    {
        /// <summary>
        /// 类型查找
        /// </summary>
        /// <param name="predicate">晒现条件表达式</param>
        /// <returns></returns>
        Type[] Find(Func<Type, bool> predicate);

        /// <summary>
        /// 查找所有类型
        /// </summary>
        /// <returns></returns>
        Type[] FindAll();
    }
}