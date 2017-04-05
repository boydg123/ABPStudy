using System;

namespace Abp.EntityFramework
{
    /// <summary>
    /// 实体类型信息
    /// </summary>
    public class EntityTypeInfo
    {
        /// <summary>
        /// Type of the entity.
        /// 实体的类型
        /// </summary>
        public Type EntityType { get; private set; }

        /// <summary>
        /// DbContext type that has DbSet property.
        /// 拥有DbSet属性的数据库上下文类型
        /// </summary>
        public Type DeclaringType { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="entityType">实体的类型</param>
        /// <param name="declaringType">拥有DbSet属性的数据库上下文类型</param>
        public EntityTypeInfo(Type entityType, Type declaringType)
        {
            EntityType = entityType;
            DeclaringType = declaringType;
        }
    }
}