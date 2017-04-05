using System;

namespace Abp.Events.Bus.Entities
{
    /// <summary>
    /// This type of event is used to notify just before deletion of an Entity.
    /// 这种类型的事件用于实体删除前通知
    /// </summary>
    /// <typeparam name="TEntity">Entity type / 实体类型</typeparam>
    [Serializable]
    public class EntityDeletingEventData<TEntity> : EntityChangingEventData<TEntity>
    {
        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        /// <param name="entity">The entity which is being deleted / 正在删除的实体</param>
        public EntityDeletingEventData(TEntity entity)
            : base(entity)
        {

        }
    }
}