using System.Data.Entity;
using Abp.Domain.Uow;

namespace Abp.EntityFramework.Uow
{
    /// <summary>
    /// EF 工作单元过滤器执行者
    /// </summary>
    public interface IEfUnitOfWorkFilterExecuter : IUnitOfWorkFilterExecuter
    {
        /// <summary>
        /// 应用当前过滤器
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="dbContext">数据库上下文</param>
        void ApplyCurrentFilters(IUnitOfWork unitOfWork, DbContext dbContext);
    }
}