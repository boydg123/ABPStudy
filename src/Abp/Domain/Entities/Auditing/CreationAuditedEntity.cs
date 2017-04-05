using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Timing;

namespace Abp.Domain.Entities.Auditing
{
    /// <summary>
    /// A shortcut of <see cref="CreationAuditedEntity{TPrimaryKey}"/> for most used primary key type (<see cref="int"/>).
    /// 多数情况(<see cref="int"/>)作为主键类型时，使用<see cref="CreationAuditedEntity{TPrimaryKey}"/> 的快捷方式.
    /// </summary>
    [Serializable]
    public abstract class CreationAuditedEntity : CreationAuditedEntity<int>, IEntity
    {

    }

    /// <summary>
    /// This class can be used to simplify implementing <see cref="ICreationAudited"/>.
    /// 此类为 <see cref="ICreationAudited"/>的简单实现
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity / 实体的主键类型</typeparam>
    [Serializable]
    public abstract class CreationAuditedEntity<TPrimaryKey> : Entity<TPrimaryKey>, ICreationAudited
    {
        /// <summary>
        /// Creation time of this entity. / 实体的创建时间
        /// </summary>
        public virtual DateTime CreationTime { get; set; }

        /// <summary>
        /// Creator of this entity. / 实体的创建人ID
        /// </summary>
        public virtual long? CreatorUserId { get; set; }

        /// <summary>
        /// Constructor. / 构造函数
        /// </summary>
        protected CreationAuditedEntity()
        {
            CreationTime = Clock.Now;
        }
    }

    /// <summary>
    /// This class can be used to simplify implementing <see cref="ICreationAudited{TUser}"/>.
    /// 此类为 <see cref="ICreationAudited{TUser}"/>的简单实现
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity / 实体的主键类型</typeparam>
    /// <typeparam name="TUser">Type of the user / 用户类型</typeparam>
    [Serializable]
    public abstract class CreationAuditedEntity<TPrimaryKey, TUser> : CreationAuditedEntity<TPrimaryKey>, ICreationAudited<TUser>
        where TUser : IEntity<long>
    {
        /// <summary>
        /// Reference to the creator user of this entity.
        /// 实体创建人的引用
        /// </summary>
        [ForeignKey("CreatorUserId")]
        public virtual TUser CreatorUser { get; set; }
    }
}