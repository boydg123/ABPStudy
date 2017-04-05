using System;
using Abp.Dependency;

namespace Abp.EntityFramework.Repositories
{
    /// <summary>
    /// EF通用仓储注册接口
    /// </summary>
    public interface IEntityFrameworkGenericRepositoryRegistrar
    {
        /// <summary>
        /// 为数据库上下文注册
        /// </summary>
        /// <param name="dbContextType">数据库上下文类型</param>
        /// <param name="iocManager">IOC管理器</param>
        void RegisterForDbContext(Type dbContextType, IIocManager iocManager);
    }
}