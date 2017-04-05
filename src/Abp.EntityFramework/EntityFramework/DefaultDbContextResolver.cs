using Abp.Dependency;

namespace Abp.EntityFramework
{
    /// <summary>
    /// <see cref="IDbContextResolver"/>的默认实现
    /// </summary>
    public class DefaultDbContextResolver : IDbContextResolver, ITransientDependency
    {
        /// <summary>
        /// IOC解析器
        /// </summary>
        private readonly IIocResolver _iocResolver;

        /// <summary>
        /// 数据库上下文类型匹配器
        /// </summary>
        private readonly IDbContextTypeMatcher _dbContextTypeMatcher;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iocResolver">IOC解析器</param>
        /// <param name="dbContextTypeMatcher">数据库上下文类型匹配器</param>
        public DefaultDbContextResolver(IIocResolver iocResolver, IDbContextTypeMatcher dbContextTypeMatcher)
        {
            _iocResolver = iocResolver;
            _dbContextTypeMatcher = dbContextTypeMatcher;
        }

        /// <summary>
        /// 解析数据库上下文
        /// </summary>
        /// <typeparam name="TDbContext">数据库上下文对象</typeparam>
        /// <param name="connectionString">连接字符串</param>
        /// <returns>数据库上下文</returns>
        public TDbContext Resolve<TDbContext>(string connectionString)
        {
            var dbContextType = typeof(TDbContext);

            if (!dbContextType.IsAbstract)
            {
                return _iocResolver.Resolve<TDbContext>(new
                {
                    nameOrConnectionString = connectionString
                });
            }
            else
            {
                var concreteType = _dbContextTypeMatcher.GetConcreteType(dbContextType);
                return (TDbContext)_iocResolver.Resolve(concreteType, new
                {
                    nameOrConnectionString = connectionString
                });
            }
        }
    }
}