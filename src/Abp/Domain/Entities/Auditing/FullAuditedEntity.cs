using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abp.Domain.Entities.Auditing
{
    /// <summary>
    /// A shortcut of <see cref="FullAuditedEntity{TPrimaryKey}"/> for most used primary key type (<see cref="int"/>).
    /// 多数情况(<see cref="int"/>)作为主键类型时，使用<see cref="FullAuditedEntity{TPrimaryKey}"/> 的快捷方式.
    /// </summary>
    [Serializable]
    public abstract class FullAuditedEntity : FullAuditedEntity<int>, IEntity
    {

    }

    /// <summary>
    /// Implements <see cref="IFullAudited"/> to be a base class for full-audited entities.
    /// 全部审计接口 <see cref="IFullAudited"/> 接口的实现基类
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity / 实体的主键类型</typeparam>
    [Serializable]
    public abstract class FullAuditedEntity<TPrimaryKey> : AuditedEntity<TPrimaryKey>, IFullAudited
    {
        /// <summary>
        /// Is this entity Deleted? / 实体是否删除
        /// </summary>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// Which user deleted this entity? / 删除实体的用户ID
        /// </summary>
        public virtual long? DeleterUserId { get; set; }

        /// <summary>
        /// Deletion time of this entity. / 实体的删除时间
        /// </summary>
        public virtual DateTime? DeletionTime { get; set; }
    }

    /// <summary>
    /// Implements <see cref="IFullAudited{TUser}"/> to be a base class for full-audited entities.
    /// 为需要全部审计功能的实体接口<see cref="IFullAudited{TUser}"/>的基类
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity / 实体主键类型</typeparam>
    /// <typeparam name="TUser">Type of the user / 用户类型</typeparam>
    [Serializable]
    public abstract class FullAuditedEntity<TPrimaryKey, TUser> : AuditedEntity<TPrimaryKey, TUser>, IFullAudited<TUser>
        where TUser : IEntity<long>
    {
        /// <summary>
        /// Is this entity Deleted? / 实体是否被删除
        /// </summary>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// Reference to the deleter user of this entity. / 实体删除者的引用
        /// </summary>
        [ForeignKey("DeleterUserId")]
        public virtual TUser DeleterUser { get; set; }

        /// <summary>
        /// Which user deleted this entity? / 实体删除者的ID
        /// </summary>
        public virtual long? DeleterUserId { get; set; }

        /// <summary>
        /// Deletion time of this entity. / 实体的删除时间
        /// </summary>
        public virtual DateTime? DeletionTime { get; set; }
    }
}