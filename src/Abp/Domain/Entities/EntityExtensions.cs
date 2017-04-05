using Abp.Domain.Entities.Auditing;
using Abp.Extensions;

namespace Abp.Domain.Entities
{
    /// <summary>
    /// Some useful extension methods for Entities.
    /// 实体扩展类，实体一些有用的扩展方法
    /// </summary>
    public static class EntityExtensions
    {
        /// <summary>
        /// Check if this Entity is null of marked as deleted.
        /// 检查给定实体是否为Null或者是否是标记为已删除的
        /// </summary>
        public static bool IsNullOrDeleted(this ISoftDelete entity)
        {
            return entity == null || entity.IsDeleted;
        }

        /// <summary>
        /// Undeletes this entity by setting <see cref="ISoftDelete.IsDeleted"/> to false and <see cref="IDeletionAudited"/> properties to null.
        /// 取消当前实体的软删除设置 <see cref="ISoftDelete.IsDeleted"/> 为flase，并且<see cref="IDeletionAudited"/>属性为null
        /// </summary>
        public static void UnDelete(this ISoftDelete entity)
        {
            entity.IsDeleted = false;
            if (entity is IDeletionAudited)
            {
                var deletionAuditedEntity = entity.As<IDeletionAudited>();
                deletionAuditedEntity.DeletionTime = null;
                deletionAuditedEntity.DeleterUserId = null;
            }
        }
    }
}