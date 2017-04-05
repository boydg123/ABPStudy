using System;

namespace Abp.Domain.Entities.Auditing
{
    /// <summary>
    /// An entity can implement this interface if <see cref="LastModificationTime"/> of this entity must be stored.
    /// <see cref="LastModificationTime"/> is automatically set when updating <see cref="Entity"/>.
    /// 实现此接口的实体必须拥有 <see cref="LastModificationTime"/>字段，它在数据修改时被自动赋值
    /// </summary>
    public interface IHasModificationTime
    {
        /// <summary>
        /// The last modified time for this entity. / 实体的最后修改时间
        /// </summary>
        DateTime? LastModificationTime { get; set; }
    }
}