using Abp.Domain.Uow;

namespace Abp.EntityFramework
{
    /// <summary>
    /// <see cref="DbContextTypeMatcher{AbpDbContext}"/>的实现。
    /// </summary>
    public class DbContextTypeMatcher : DbContextTypeMatcher<AbpDbContext>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUnitOfWorkProvider"></param>
        public DbContextTypeMatcher(ICurrentUnitOfWorkProvider currentUnitOfWorkProvider) 
            : base(currentUnitOfWorkProvider)
        {
        }
    }
}