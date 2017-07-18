using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace Derrick.EntityFramework.Repositories
{
    /// <summary>
    /// 应用程序自定义仓储基类
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public abstract class AbpZeroTemplateRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<AbpZeroTemplateDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected AbpZeroTemplateRepositoryBase(IDbContextProvider<AbpZeroTemplateDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add your common methods for all repositories
    }

    /// <summary>
    /// 应用程序自定义仓储基类。这个是<see cref="AbpZeroTemplateRepositoryBase{TEntity,TPrimaryKey}"/>主键类型为<see cref="int"/>的快捷方式
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public abstract class AbpZeroTemplateRepositoryBase<TEntity> : AbpZeroTemplateRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected AbpZeroTemplateRepositoryBase(IDbContextProvider<AbpZeroTemplateDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)!!!
    }
}
