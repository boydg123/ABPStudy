using System;
using Abp.Domain.Entities.Auditing;

namespace Abp.Application.Services.Dto
{
    /// <summary>
    /// A shortcut of <see cref="FullAuditedEntityDto{TPrimaryKey}"/> for most used primary key type (<see cref="int"/>).
    /// 多数主键类型为(<see cref="int"/>)的<see cref="FullAuditedEntityDto{TPrimaryKey}"/>的一个快捷方式
    /// </summary>
    [Serializable]
    public abstract class FullAuditedEntityDto : FullAuditedEntityDto<int>
    {

    }

    /// <summary>
    /// This class can be inherited for simple Dto objects those are used for entities implement <see cref="IFullAudited{TUser}"/> interface.
    /// 继承此类可以为Dto对象实现接口<see cref="IFullAudited{TUser}"/>
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of primary key / 主键的类型</typeparam>
    [Serializable]
    public abstract class FullAuditedEntityDto<TPrimaryKey> : AuditedEntityDto<TPrimaryKey>, IFullAudited
    {
        /// <summary>
        /// Is this entity deleted?
        /// 此实体对象是否已被删除
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Deleter user's Id, if this entity is deleted,
        /// 删除实体的用户Id
        /// </summary>
        public long? DeleterUserId { get; set; }

        /// <summary>
        /// Deletion time, if this entity is deleted,
        /// 删除时间
        /// </summary>
        public DateTime? DeletionTime { get; set; }
    }
}