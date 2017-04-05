using System;
using Abp.Domain.Entities;

namespace Abp.Events.Bus.Entities
{
    /// <summary>
    /// Used to pass data for an event that is related to with an <see cref="IEntity"/> object.
    /// 给一个事件传递数据，这个事件与<see cref="IEntity"/>对象相关
    /// </summary>
    /// <typeparam name="TEntity">Entity type / 实体类型</typeparam>
    [Serializable]
    public class EntityEventData<TEntity> : EventData , IEventDataWithInheritableGenericArgument
    {
        /// <summary>
        /// Related entity with this event.
        /// 当前事件相关联的实体
        /// </summary>
        public TEntity Entity { get; private set; }

        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        /// <param name="entity">Related entity with this event / 当前事件相关联的实体</param>
        public EntityEventData(TEntity entity)
        {
            Entity = entity;
        }

        /// <summary>
        /// 获取构造函数的参数
        /// </summary>
        /// <returns></returns>
        public virtual object[] GetConstructorArgs()
        {
            return new object[] { Entity };
        }
    }
}