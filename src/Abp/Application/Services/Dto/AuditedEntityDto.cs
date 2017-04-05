using System;
using Abp.Domain.Entities.Auditing;

namespace Abp.Application.Services.Dto
{
    /// <summary>
    /// A shortcut of <see cref="AuditedEntityDto{TPrimaryKey}"/> for most used primary key type (<see cref="int"/>).
    /// 多数使用(<see cref="int"/>)类型主键的<see cref="AuditedEntityDto{TPrimaryKey}"/>一个快键方式
    /// </summary>
    [Serializable]
    public abstract class AuditedEntityDto : AuditedEntityDto<int>
    {

    }

    /// <summary>
    /// This class can be inherited for simple Dto objects those are used for entities implement <see cref="IAudited{TUser}"/> interface.
    /// 此类可以被简单Dto对象继承，以实现接口 <see cref="IAudited{TUser}"/> 
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of primary key / 主键类型</typeparam>
    [Serializable]
    public abstract class AuditedEntityDto<TPrimaryKey> : CreationAuditedEntityDto<TPrimaryKey>, IAudited
    {
        /// <summary>
        /// Last modification date of this entity.
        /// 最近一次修改实体的时间
        /// </summary>
        public DateTime? LastModificationTime { get; set; }

        /// <summary>
        /// Last modifier user of this entity.
        /// 最近一次修改实体的用户
        /// </summary>
        public long? LastModifierUserId { get; set; }
    }
}