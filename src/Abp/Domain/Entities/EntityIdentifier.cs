using System;

namespace Abp.Domain.Entities
{
    /// <summary>
    /// Used to identify an entity.Can be used to store an entity <see cref="Type"/> and <see cref="Id"/>.
    /// 用来标识实体.可以用来存储一个实体 <see cref="Type"/> 以及实体的 <see cref="Id"/>
    /// </summary>
    [Serializable]
    public class EntityIdentifier
    {
        /// <summary>
        /// Entity Type.
        /// 实体类型
        /// </summary>
        public Type Type { get; private set; }

        /// <summary>
        /// Entity's Id.
        /// 实体的Id
        /// </summary>
        public object Id { get; private set; }

        /// <summary>
        /// Added for serialization purposes.
        /// 为序列化而添加
        /// </summary>
        private EntityIdentifier()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityIdentifier"/> class.
        /// 初始化一个新的 <see cref="EntityIdentifier"/> 实例类
        /// </summary>
        /// <param name="type">Entity type. / 实体类型</param>
        /// <param name="id">Id of the entity. / 实体的Id</param>
        public EntityIdentifier(Type type, object id)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            Type = type;
            Id = id;
        }
    }
}