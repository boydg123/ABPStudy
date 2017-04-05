using System;
using Abp.Domain.Entities.Auditing;
using Abp.Timing;

namespace Abp.Application.Services.Dto
{
    /// <summary>
    ///  A shortcut of <see cref="CreationAuditedEntityDto"/> for most used primary key type (<see cref="int"/>).
    ///  绝大多数使用 (<see cref="int"/>)作为主键类型的<see cref="CreationAuditedEntityDto{TPrimaryKey}"/>的一个快捷方式
    /// </summary>
    [Serializable]
    public abstract class CreationAuditedEntityDto : CreationAuditedEntityDto<int>
    {
        
    }

    /// <summary>
    /// This class can be inherited for simple Dto objects those are used for entities implement <see cref="ICreationAudited"/> interface.
    /// 此类可以被简单的DTO对象继承，以实现接口 <see cref="ICreationAudited"/>
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of primary key / 主键类型</typeparam>
    [Serializable]
    public abstract class CreationAuditedEntityDto<TPrimaryKey> : EntityDto<TPrimaryKey>, ICreationAudited
    {
        /// <summary>
        /// Creation date of this entity.
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Creator user's id for this entity.
        /// 创建者
        /// </summary>
        public long? CreatorUserId { get; set; }

        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        protected CreationAuditedEntityDto()
        {
            CreationTime = Clock.Now;
        }
    }
}