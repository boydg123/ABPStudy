namespace Abp.Events.Bus.Entities
{
    /// <summary>
    /// Used to trigger entity change events.
    /// 用于触发实体改变事件
    /// </summary>
    public interface IEntityChangeEventHelper
    {
        /// <summary>
        /// 触发实体创建事件
        /// </summary>
        /// <param name="entity">实体</param>
        void TriggerEntityCreatingEvent(object entity);

        /// <summary>
        /// 在工作单元完成的时候触发实体创建事件
        /// </summary>
        /// <param name="entity">实体</param>
        void TriggerEntityCreatedEventOnUowCompleted(object entity);

        /// <summary>
        /// 触发实体更新事件
        /// </summary>
        /// <param name="entity"></param>
        void TriggerEntityUpdatingEvent(object entity);

        /// <summary>
        /// 在工作单元完成的时候触发实体更新事件
        /// </summary>
        /// <param name="entity"></param>
        void TriggerEntityUpdatedEventOnUowCompleted(object entity);

        /// <summary>
        /// 触发实体删除事件
        /// </summary>
        /// <param name="entity"></param>
        void TriggerEntityDeletingEvent(object entity);

        /// <summary>
        /// 在工作单元完成的时候触发实体删除事件
        /// </summary>
        /// <param name="entity"></param>
        void TriggerEntityDeletedEventOnUowCompleted(object entity);
    }
}