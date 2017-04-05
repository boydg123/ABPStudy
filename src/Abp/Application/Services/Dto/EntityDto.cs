using System;

namespace Abp.Application.Services.Dto
{
    /// <summary>
    /// A shortcut of <see cref="EntityDto{TPrimaryKey}"/> for most used primary key type (<see cref="int"/>).
    /// 大多数使用(<see cref="int"/>)作为主键类型的<see cref="EntityDto{TPrimaryKey}"/>的一个快捷方式).
    /// </summary>
    [Serializable]
    public class EntityDto : EntityDto<int>, IEntityDto
    {
        /// <summary>
        /// Creates a new <see cref="EntityDto"/> object.
        /// 构造函数
        /// </summary>
        public EntityDto()
        {

        }

        /// <summary>
        /// Creates a new <see cref="EntityDto"/> object.
        /// 构造函数
        /// </summary>
        /// <param name="id">Id of the entity / 实体ID</param>
        public EntityDto(int id)
            : base(id)
        {
        }
    }

    /// <summary>
    /// Implements common properties for entity based DTOs.
    /// 为实体DTO实现通用属性
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key / 主键类型</typeparam>
    [Serializable]
    public class EntityDto<TPrimaryKey> : IEntityDto<TPrimaryKey>
    {
        /// <summary>
        /// Id of the entity.
        /// 实体的唯一标识
        /// </summary>
        public TPrimaryKey Id { get; set; }

        /// <summary>
        /// Creates a new <see cref="EntityDto{TPrimaryKey}"/> object.
        /// 创建一个新的<see cref="EntityDto{TPrimaryKey}"/> 对象.
        /// </summary>
        public EntityDto()
        {

        }

        /// <summary>
        /// Creates a new <see cref="EntityDto{TPrimaryKey}"/> object.
        /// 创建一个新的<see cref="EntityDto{TPrimaryKey}"/> 对象.
        /// </summary>
        /// <param name="id">Id of the entity / 实体ID</param>
        public EntityDto(TPrimaryKey id)
        {
            Id = id;
        }
    }
}