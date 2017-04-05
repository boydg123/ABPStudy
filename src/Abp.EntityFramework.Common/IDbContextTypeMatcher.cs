using System;

namespace Abp.EntityFramework
{
    /// <summary>
    /// 数据库上下文类型匹配器接口
    /// </summary>
    public interface IDbContextTypeMatcher
    {
        /// <summary>
        /// 填充数据库上下文类型
        /// </summary>
        /// <param name="dbContextTypes">数据库上下文类型</param>
        void Populate(Type[] dbContextTypes);

        /// <summary>
        /// 获取具体的类型
        /// </summary>
        /// <param name="sourceDbContextType">源数据库上下文类型</param>
        /// <returns>具体类型</returns>
        Type GetConcreteType(Type sourceDbContextType);
    }
}