using Abp.Dependency;

namespace Abp.Authorization
{
    /// <summary>
    /// 权限依赖上下文
    /// </summary>
    internal class PermissionDependencyContext : IPermissionDependencyContext, ITransientDependency
    {
        /// <summary>
        /// 需要权限的用户。如果没有用户可以为空
        /// </summary>
        public UserIdentifier User { get; set; }

        /// <summary>
        /// IOC解析器
        /// </summary>
        public IIocResolver IocResolver { get; }
        
        /// <summary>
        /// 权限检查器
        /// </summary>
        public IPermissionChecker PermissionChecker { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iocResolver">IOC解析器</param>
        public PermissionDependencyContext(IIocResolver iocResolver)
        {
            IocResolver = iocResolver;
            PermissionChecker = NullPermissionChecker.Instance;
        }
    }
}