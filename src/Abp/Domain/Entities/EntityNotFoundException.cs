using System;
using System.Runtime.Serialization;

namespace Abp.Domain.Entities
{
    /// <summary>
    /// This exception is thrown if an entity excepted to be found but not found.
    /// 如果查询实体，实体没有被找到则抛出此异常
    /// </summary>
    [Serializable]
    public class EntityNotFoundException : AbpException
    {
        /// <summary>
        /// Type of the entity.
        /// 实体类型
        /// </summary>
        public Type EntityType { get; set; }

        /// <summary>
        /// Id of the Entity.
        /// 实体的ID
        /// </summary>
        public object Id { get; set; }

        /// <summary>
        /// Creates a new <see cref="EntityNotFoundException"/> object.
        /// 无参构造函数:创建一个新的 <see cref="EntityNotFoundException"/> 对象
        /// </summary>
        public EntityNotFoundException()
        {

        }

        /// <summary>
        /// Creates a new <see cref="EntityNotFoundException"/> object.
        /// 构造函数:创建一个新的<see cref="EntityNotFoundException"/> 对象
        /// </summary>
        public EntityNotFoundException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Creates a new <see cref="EntityNotFoundException"/> object.
        /// 构造函数:创建一个新的<see cref="EntityNotFoundException"/> 对象
        /// </summary>
        public EntityNotFoundException(Type entityType, object id)
            : this(entityType, id, null)
        {

        }

        /// <summary>
        /// Creates a new <see cref="EntityNotFoundException"/> object.
        /// 构造函数:创建一个新的<see cref="EntityNotFoundException"/> 对象
        /// </summary>
        public EntityNotFoundException(Type entityType, object id, Exception innerException)
            : base($"There is no such an entity. Entity type: {entityType.FullName}, id: {id}", innerException)
        {
            EntityType = entityType;
            Id = id;
        }

        /// <summary>
        /// Creates a new <see cref="EntityNotFoundException"/> object.
        /// 构造函数:创建一个新的<see cref="EntityNotFoundException"/> 对象
        /// </summary>
        /// <param name="message">Exception message / 异常消息</param>
        public EntityNotFoundException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Creates a new <see cref="EntityNotFoundException"/> object.
        /// 构造函数:创建一个新的<see cref="EntityNotFoundException"/> 对象
        /// </summary>
        /// <param name="message">Exception message / 异常消息</param>
        /// <param name="innerException">Inner exception / 内部异常</param>
        public EntityNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
