using System;
using Abp.Domain.Entities;

namespace Abp.Events.Bus.Entities
{
    /// <summary>
    /// Used to pass data for an event when an entity (<see cref="IEntity"/>) is changed (created, updated or deleted).
    /// See <see cref="EntityCreatedEventData{TEntity}"/>, <see cref="EntityDeletedEventData{TEntity}"/> and <see cref="EntityUpdatedEventData{TEntity}"/> classes.
    /// 用于传递数据给事件,当实体<see cref="IEntity"/>改变的时候(创建，修改，删除)
    /// </summary>
    /// <typeparam name="TEntity">Entity type / 实体类型</typeparam>
    [Serializable]
    public class EntityChangedEventData<TEntity> : EntityEventData<TEntity>
    {
        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        /// <param name="entity">Changed entity in this event / 事件中发生改变的实体</param>
        public EntityChangedEventData(TEntity entity)
            : base(entity)
        {

        }
    }
}