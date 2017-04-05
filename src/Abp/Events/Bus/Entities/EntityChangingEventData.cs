using System;
using Abp.Domain.Entities;

namespace Abp.Events.Bus.Entities
{
    /// <summary>
    /// Used to pass data for an event when an entity (<see cref="IEntity"/>) is being changed (creating, updating or deleting).
    /// See <see cref="EntityCreatingEventData{TEntity}"/>, <see cref="EntityDeletingEventData{TEntity}"/> and <see cref="EntityUpdatingEventData{TEntity}"/> classes.
    /// 用于传递数据给事件,当实体<see cref="IEntity"/>正在改变的时候(创建，修改，删除)
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    [Serializable]
    public class EntityChangingEventData<TEntity> : EntityEventData<TEntity>
    {
        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        /// <param name="entity">Changing entity in this event / 事件中正在改变的实体</param>
        public EntityChangingEventData(TEntity entity)
            : base(entity)
        {

        }
    }
}