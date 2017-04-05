namespace Abp.Events.Bus.Entities
{
    /// <summary>
    /// Null-object implementation of <see cref="IEntityChangeEventHelper"/>
    /// <see cref="IEntityChangeEventHelper"/>的Null对象实现.
    /// </summary>
    public class NullEntityChangeEventHelper : IEntityChangeEventHelper
    {
        /// <summary>
        /// Gets single instance of <see cref="NullEventBus"/> class.
        /// 获取一个 <see cref="NullEventBus"/> 类的单例.
        /// </summary>
        public static NullEntityChangeEventHelper Instance { get { return SingletonInstance; } }
        private static readonly NullEntityChangeEventHelper SingletonInstance = new NullEntityChangeEventHelper();

        /// <summary>
        /// 构造函数
        /// </summary>
        private NullEntityChangeEventHelper()
        {

        }

        /// <summary>
        /// 触发实体创建事件
        /// </summary>
        /// <param name="entity">实体</param>
        public void TriggerEntityCreatingEvent(object entity)
        {
            
        }

        /// <summary>
        /// 在工作单元完成的时候触发实体创建事件
        /// </summary>
        /// <param name="entity">实体</param>
        public void TriggerEntityCreatedEventOnUowCompleted(object entity)
        {
            
        }

        /// <summary>
        /// 触发实体更新事件
        /// </summary>
        /// <param name="entity"></param>
        public void TriggerEntityUpdatingEvent(object entity)
        {
            
        }

        /// <summary>
        /// 在工作单元完成的时候触发实体更新事件
        /// </summary>
        /// <param name="entity"></param>
        public void TriggerEntityUpdatedEventOnUowCompleted(object entity)
        {
            
        }

        /// <summary>
        /// 触发实体删除事件
        /// </summary>
        /// <param name="entity"></param>
        public void TriggerEntityDeletingEvent(object entity)
        {
            
        }

        /// <summary>
        /// 在工作单元完成的时候触发实体删除事件
        /// </summary>
        /// <param name="entity"></param>
        public void TriggerEntityDeletedEventOnUowCompleted(object entity)
        {
            
        }
    }
}