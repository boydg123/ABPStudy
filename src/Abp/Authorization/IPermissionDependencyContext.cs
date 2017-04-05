using Abp.Application.Features;
using Abp.Dependency;

namespace Abp.Authorization
{
    /// <summary>
    /// Permission dependency context.
    /// 权限依赖上下文
    /// </summary>
    public interface IPermissionDependencyContext
    {
        /// <summary>
        /// The user which requires permission. Can be null if no user.
        /// 需要权限的用户。如果没有用户可以为空
        /// </summary>
        UserIdentifier User { get; }

        /// <summary>
        /// Gets the <see cref="IIocResolver"/>.
        /// 获取IOC解析器
        /// </summary>
        /// <value>
        /// The ioc resolver.
        /// IOC解析器
        /// </value>
        IIocResolver IocResolver { get; }

        /// <summary>
        /// Gets the <see cref="IFeatureChecker"/>.
        /// 获取用户权限检查者
        /// </summary>
        /// <value>
        /// The feature checker.
        /// 功能检查者
        /// </value>
        IPermissionChecker PermissionChecker { get; }
    }
}