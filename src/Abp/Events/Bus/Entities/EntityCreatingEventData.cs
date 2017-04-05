using System;

namespace Abp.Events.Bus.Entities
{
    /// <summary>
    /// This type of event is used to notify just before creation of an Entity.
    /// 这种类型的事件用于实体创建前通知
    /// </summary>
    /// <typeparam name="TEntity">Entity type / 实体类型</typeparam>
    [Serializable]
    public class EntityCreatingEventData<TEntity> : EntityChangingEventData<TEntity>
    {
        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        /// <param name="entity">The entity which is being created / 正在被创建的实体</param>
        public EntityCreatingEventData(TEntity entity)
            : base(entity)
        {

        }
    }
}