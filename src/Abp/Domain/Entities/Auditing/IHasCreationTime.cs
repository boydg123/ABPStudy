using System;

namespace Abp.Domain.Entities.Auditing
{
    /// <summary>
    /// An entity can implement this interface if <see cref="CreationTime"/> of this entity must be stored.
    /// <see cref="CreationTime"/> is automatically set when saving <see cref="Entity"/> to database.
    /// 实现此接口的实体必须拥有 <see cref="CreationTime"/>字段，它在保存到数据时能被自动设置值
    /// </summary>
    public interface IHasCreationTime
    {
        /// <summary>
        /// Creation time of this entity. / 实体的创建时间
        /// </summary>
        DateTime CreationTime { get; set; }
    }
}