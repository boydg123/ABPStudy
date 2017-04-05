using Abp.Domain.Entities;

namespace Abp.Domain.Repositories
{
    /// <summary>
    /// A shortcut of <see cref="IRepository{TEntity,TPrimaryKey}"/> for most used primary key type (<see cref="int"/>).
    /// <see cref="IRepository{TEntity,TPrimaryKey}"/>的一个快捷实现， 因为最大多数据的主键为 (<see cref="int"/>).
    /// </summary>
    /// <typeparam name="TEntity">Entity type / 实体类型</typeparam>
    public interface IRepository<TEntity> : IRepository<TEntity, int> where TEntity : class, IEntity<int>
    {

    }
}
