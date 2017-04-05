using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Events.Bus;

namespace Abp.Domain.Entities
{
    /// <summary>
    /// 主键类型为int的聚合根
    /// </summary>
    public class AggregateRoot : AggregateRoot<int>, IAggregateRoot
    {

    }

    /// <summary>
    /// 聚合根类
    /// </summary>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public class AggregateRoot<TPrimaryKey> : Entity<TPrimaryKey>, IAggregateRoot<TPrimaryKey>
    {
        /// <summary>
        /// 领域事件数据集合
        /// </summary>
        [NotMapped]
        public virtual ICollection<IEventData> DomainEvents { get; }

        /// <summary>
        /// 初始化领域事件数据
        /// </summary>
        public AggregateRoot()
        {
            DomainEvents = new Collection<IEventData>();
        }
    }
}