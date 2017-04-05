using Abp.Domain.Repositories;

namespace Abp.EntityFramework.Repositories
{
    /// <summary>
    /// EF自动仓储类型
    /// </summary>
    public static class EfAutoRepositoryTypes
    {
        /// <summary>
        /// 自动仓储类型属性
        /// </summary>
        public static AutoRepositoryTypesAttribute Default { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        static EfAutoRepositoryTypes()
        {
            Default = new AutoRepositoryTypesAttribute(
                typeof(IRepository<>),
                typeof(IRepository<,>),
                typeof(EfRepositoryBase<,>),
                typeof(EfRepositoryBase<,,>)
            );
        }
    }
}