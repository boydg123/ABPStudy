namespace Abp.Application.Services.Dto
{
    /// <summary>
    /// A shortcut of <see cref="IEntityDto{TPrimaryKey}"/> for most used primary key type (<see cref="int"/>).
    /// 大多数使用(<see cref="int"/>)作为主键类型的<see cref="EntityDto{TPrimaryKey}"/>的一个快捷方式).
    /// </summary>
    public interface IEntityDto : IEntityDto<int>
    {

    }

    /// <summary>
    /// Defines common properties for entity based DTOs.
    /// 为实体DTO定义通用属性
    /// </summary>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public interface IEntityDto<TPrimaryKey>
    {
        /// <summary>
        /// Id of the entity.
        /// 实体ID
        /// </summary>
        TPrimaryKey Id { get; set; }
    }
}