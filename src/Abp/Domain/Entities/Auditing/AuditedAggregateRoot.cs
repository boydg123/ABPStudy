using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abp.Domain.Entities.Auditing
{
    /// <summary>
    /// A shortcut of <see cref="AuditedAggregateRoot{TPrimaryKey}"/> for most used primary key type (<see cref="int"/>).
    /// 多数情况(<see cref="int"/>)作为主键类型时，使用<see cref="CreationAuditedAggregateRoot{TPrimaryKey}"/> 的快捷方式.
    /// </summary>
    [Serializable]
    public abstract class AuditedAggregateRoot : AuditedAggregateRoot<int>
    {

    }

    /// <summary>
    /// This class can be used to simplify implementing <see cref="IAudited"/> for aggregate roots.
    /// 此类为 <see cref="IAudited"/> 接口的聚合根简单实现
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity / 实体的主键类型</typeparam>
    [Serializable]
    public abstract class AuditedAggregateRoot<TPrimaryKey> : CreationAuditedAggregateRoot<TPrimaryKey>, IAudited
    {
        /// <summary>
        /// Last modification date of this entity. / 实体的最后修改时间
        /// </summary>
        public virtual DateTime? LastModificationTime { get; set; }

        /// <summary>
        /// Last modifier user of this entity. / 实体的最后修改人ID
        /// </summary>
        public virtual long? LastModifierUserId { get; set; }
    }

    /// <summary>
    /// This class can be used to simplify implementing <see cref="IAudited{TUser}"/> for aggregate roots.
    /// 此类为 <see cref="IAudited{TUser}"/> 接口的聚合根简单实现
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity  / 实体的主键类型</typeparam>
    /// <typeparam name="TUser">Type of the user / 用户类型</typeparam>
    [Serializable]
    public abstract class AuditedAggregateRoot<TPrimaryKey, TUser> : AuditedAggregateRoot<TPrimaryKey>, IAudited<TUser>
        where TUser : IEntity<long>
    {
        /// <summary>
        /// Reference to the creator user of this entity.
        /// 实体创建人的引用
        /// </summary>
        [ForeignKey("CreatorUserId")]
        public virtual TUser CreatorUser { get; set; }

        /// <summary>
        /// Reference to the last modifier user of this entity.
        /// 实体最后修改人引用
        /// </summary>
        [ForeignKey("LastModifierUserId")]
        public virtual TUser LastModifierUser { get; set; }
    }
}