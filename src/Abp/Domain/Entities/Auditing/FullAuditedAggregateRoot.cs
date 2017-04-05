using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abp.Domain.Entities.Auditing
{
    /// <summary>
    /// A shortcut of <see cref="FullAuditedAggregateRoot{TPrimaryKey}"/> for most used primary key type (<see cref="int"/>).
    /// �������(<see cref="int"/>)��Ϊ��������ʱ��ʹ��<see cref="FullAuditedAggregateRoot{TPrimaryKey}"/> �Ŀ�ݷ�ʽ.
    /// </summary>
    [Serializable]
    public abstract class FullAuditedAggregateRoot : FullAuditedAggregateRoot<int>
    {

    }

    /// <summary>
    /// Implements <see cref="IFullAudited"/> to be a base class for full-audited aggregate roots.
    /// Ϊ��Ҫȫ����ƹ��ܾۺϸ��Ľӿ�<see cref="IFullAudited"/>�Ļ���
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity / ʵ�����������</typeparam>
    [Serializable]
    public abstract class FullAuditedAggregateRoot<TPrimaryKey> : AuditedAggregateRoot<TPrimaryKey>, IFullAudited
    {
        /// <summary>
        /// Is this entity Deleted? / ʵ���Ƿ�ɾ��
        /// </summary>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// Which user deleted this entity? / ɾ��ʵ����û�ID
        /// </summary>
        public virtual long? DeleterUserId { get; set; }

        /// <summary>
        /// Deletion time of this entity. / ʵ���ɾ��ʱ��
        /// </summary>
        public virtual DateTime? DeletionTime { get; set; }
    }

    /// <summary>
    /// Implements <see cref="IFullAudited{TUser}"/> to be a base class for full-audited aggregate roots.
    /// Ϊ��Ҫȫ����ƹ��ܾۺϸ��Ľӿ�<see cref="IFullAudited{TUser}"/>�Ļ���
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity / ʵ�����������</typeparam>
    /// <typeparam name="TUser">Type of the user / �û�����</typeparam>
    [Serializable]
    public abstract class FullAuditedAggregateRoot<TPrimaryKey, TUser> : AuditedAggregateRoot<TPrimaryKey, TUser>, IFullAudited<TUser>
        where TUser : IEntity<long>
    {
        /// <summary>
        /// Is this entity Deleted? / ʵ���Ƿ�ɾ��
        /// </summary>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// Reference to the deleter user of this entity. / ʵ��ɾ���˵�����
        /// </summary>
        [ForeignKey("DeleterUserId")]
        public virtual TUser DeleterUser { get; set; }

        /// <summary>
        /// Which user deleted this entity? / ɾ��ʵ����û�ID
        /// </summary>
        public virtual long? DeleterUserId { get; set; }

        /// <summary>
        /// Deletion time of this entity. / ʵ���ɾ��ʱ��
        /// </summary>
        public virtual DateTime? DeletionTime { get; set; }
    }
}