using System;
using Abp.Dependency;
using Abp.Domain.Uow;

namespace Abp.Events.Bus.Entities
{
    /// <summary>
    /// Used to trigger entity change events.
    /// 用于触发实体改变事件
    /// </summary>
    public class EntityChangeEventHelper : ITransientDependency, IEntityChangeEventHelper
    {
        /// <summary>
        /// 事件总线
        /// </summary>
        public IEventBus EventBus { get; set; }

        /// <summary>
        /// 工作单元管理器
        /// </summary>
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unitOfWorkManager">工作单元管理器</param>
        public EntityChangeEventHelper(IUnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
            EventBus = NullEventBus.Instance;
        }

        /// <summary>
        /// 触发实体正在创建事件
        /// </summary>
        /// <param name="entity"></param>
        public void TriggerEntityCreatingEvent(object entity)
        {
            TriggerEventWithEntity(typeof(EntityCreatingEventData<>), entity, true);
        }

        /// <summary>
        /// 当工作单元完成时触发实体创建事件
        /// </summary>
        /// <param name="entity"></param>
        public void TriggerEntityCreatedEventOnUowCompleted(object entity)
        {
            TriggerEventWithEntity(typeof(EntityCreatedEventData<>), entity, false);
        }

        /// <summary>
        /// 触发实体正在更新事件
        /// </summary>
        /// <param name="entity"></param>
        public void TriggerEntityUpdatingEvent(object entity)
        {
            TriggerEventWithEntity(typeof(EntityUpdatingEventData<>), entity, true);
        }

        /// <summary>
        /// 当工作单元完成时触发实体更新事件
        /// </summary>
        /// <param name="entity"></param>
        public void TriggerEntityUpdatedEventOnUowCompleted(object entity)
        {
            TriggerEventWithEntity(typeof(EntityUpdatedEventData<>), entity, false);
        }

        /// <summary>
        /// 触发实体正在删除事件
        /// </summary>
        /// <param name="entity"></param>
        public void TriggerEntityDeletingEvent(object entity)
        {
            TriggerEventWithEntity(typeof(EntityDeletingEventData<>), entity, true);
        }

        /// <summary>
        /// 当工作单元完成时触发实体删除事件
        /// </summary>
        /// <param name="entity"></param>
        public void TriggerEntityDeletedEventOnUowCompleted(object entity)
        {
            TriggerEventWithEntity(typeof(EntityDeletedEventData<>), entity, false);
        }

        /// <summary>
        /// 触发实体事件
        /// </summary>
        /// <param name="genericEventType">泛型事件类型</param>
        /// <param name="entity">实体</param>
        /// <param name="triggerImmediately">是否立即触发</param>
        private void TriggerEventWithEntity(Type genericEventType, object entity, bool triggerImmediately)
        {
            var entityType = entity.GetType();
            var eventType = genericEventType.MakeGenericType(entityType);

            if (triggerImmediately || _unitOfWorkManager.Current == null)
            {
                EventBus.Trigger(eventType, (IEventData)Activator.CreateInstance(eventType, new[] { entity }));
                return;
            }

            _unitOfWorkManager.Current.Completed += (sender, args) => EventBus.Trigger(eventType, (IEventData)Activator.CreateInstance(eventType, new[] { entity }));
        }
    }
}