namespace Abp.Domain.Entities
{
    /// <summary>
    /// Defines interface for base entity type. All entities in the system must implement this interface.
    /// 定义基本实体类型的接口,系统中所有的实体必须实现此接口
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity / 实体的主键类型</typeparam>
    public interface IEntity<TPrimaryKey>
    {
        /// <summary>
        /// Unique identifier for this entity.
        /// 实体的唯一标识
        /// </summary>
        TPrimaryKey Id { get; set; }

        /// <summary>
        /// Checks if this entity is transient (not persisted to database and it has not an <see cref="Id"/>).
        /// 检查此实体是否是暂时的(不存在数据库中而且它没有主键<see cref="Id"/>)
        /// </summary>
        /// <returns>True, if this entity is transient / True,如果此实体是暂时的</returns>
        bool IsTransient();
    }
}
