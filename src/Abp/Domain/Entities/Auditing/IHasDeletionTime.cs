using System;

namespace Abp.Domain.Entities.Auditing
{
    /// <summary>
    /// An entity can implement this interface if <see cref="DeletionTime"/> of this entity must be stored.
    /// <see cref="DeletionTime"/> is automatically set when deleting <see cref="Entity"/>.
    /// 实现此接口的实体必须拥有 <see cref="DeletionTime"/>字段，它在数据删除时被自动赋值
    /// </summary>
    public interface IHasDeletionTime : ISoftDelete
    {
        /// <summary>
        /// Deletion time of this entity. / 实体的删除时间
        /// </summary>
        DateTime? DeletionTime { get; set; }
    }
}