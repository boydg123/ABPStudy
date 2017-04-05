using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Timing;

namespace Abp.Domain.Entities.Auditing
{
    /// <summary>
    /// A shortcut of <see cref="CreationAuditedAggregateRoot{TPrimaryKey}"/> for most used primary key type (<see cref="int"/>).
    /// 多数情况(<see cref="int"/>)作为主键类型时，使用<see cref="CreationAuditedAggregateRoot{TPrimaryKey}"/> 的快捷方式.
    /// </summary>
    [Serializable]
    public abstract class CreationAuditedAggregateRoot : CreationAuditedAggregateRoot<int>
    {
        
    }

    /// <summary>
    /// This class can be used to simplify implementing <see cref="ICreationAudited"/> for aggregate roots.
    /// 此类为 <see cref="ICreationAudited"/> 接口的聚合根简单实现
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity / 实体的主键类型</typeparam>
    [Serializable]
    public abstract class CreationAuditedAggregateRoot<TPrimaryKey> : AggregateRoot<TPrimaryKey>, ICreationAudited
    {
        /// <summary>
        /// Creation time of this entity. 
        /// 实体的创建时间
        /// </summary>
        public virtual DateTime CreationTime { get; set; }

        /// <summary>
        /// Creator of this entity. 
        /// 实体创建人ID
        /// </summary>
        public virtual long? CreatorUserId { get; set; }

        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        protected CreationAuditedAggregateRoot()
        {
            CreationTime = Clock.Now;
        }
    }

    /// <summary>
    /// This class can be used to simplify implementing <see cref="ICreationAudited{TUser}"/> for aggregate roots.
    /// 此类为 <see cref="ICreationAudited{TUser}"/> 接口的聚合根简单实现
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity / 实体的主键类型</typeparam>
    /// <typeparam name="TUser">Type of the user / 用户类型</typeparam>
    [Serializable]
    public abstract class CreationAuditedAggregateRoot<TPrimaryKey, TUser> : CreationAuditedAggregateRoot<TPrimaryKey>, ICreationAudited<TUser>
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
