using System;

namespace Abp.Events.Bus.Entities
{
    /// <summary>
    /// This type of event is used to notify just before update of an Entity.
    /// 这种类型的事件用于实体更新前通知
    /// </summary>
    /// <typeparam name="TEntity">Entity type / 实体类型</typeparam>
    [Serializable]
    public class EntityUpdatingEventData<TEntity> : EntityChangingEventData<TEntity>
    {
        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        /// <param name="entity">The entity which is being updated / 正在更新的实体</param>
        public EntityUpdatingEventData(TEntity entity)
            : base(entity)
        {

        }
    }
}