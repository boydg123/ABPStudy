using System;

namespace Abp.Events.Bus.Entities
{
    /// <summary>
    /// This type of event can be used to notify just after update of an Entity.
    /// 这种类型的事件用于实体更新后通知
    /// </summary>
    /// <typeparam name="TEntity">Entity type / 实体类型</typeparam>
    [Serializable]
    public class EntityUpdatedEventData<TEntity> : EntityChangedEventData<TEntity>
    {
        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        /// <param name="entity">The entity which is updated / 已经更新的实体</param>
        public EntityUpdatedEventData(TEntity entity)
            : base(entity)
        {

        }
    }
}