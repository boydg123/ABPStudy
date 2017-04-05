using System.Collections.Generic;
using Abp.Events.Bus;

namespace Abp.Domain.Entities
{
    /// <summary>
    /// 聚合根接口
    /// </summary>
    public interface IAggregateRoot : IAggregateRoot<int>, IEntity
    {

    }

    /// <summary>
    /// 聚合根接口(泛型)
    /// </summary>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public interface IAggregateRoot<TPrimaryKey> : IEntity<TPrimaryKey>, IGeneratesDomainEvents
    {

    }

    /// <summary>
    /// 生成领域事件接口
    /// </summary>
    public interface IGeneratesDomainEvents
    {
        /// <summary>
        /// 领域事件数据集合
        /// </summary>
        ICollection<IEventData> DomainEvents { get; }
    }
}