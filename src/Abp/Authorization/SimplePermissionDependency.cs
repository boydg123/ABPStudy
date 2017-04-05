using System.Threading.Tasks;

namespace Abp.Authorization
{
    /// <summary>
    /// Most simple implementation of <see cref="IPermissionDependency"/>.It checks one or more permissions if they are granted.
    /// <see cref="IPermissionDependency"/>.最简单的实现，它检查一个或多个权限，如果他们被授予
    /// </summary>
    public class SimplePermissionDependency : IPermissionDependency
    {
        /// <summary>
        /// A list of permissions to be checked if they are granted.
        /// 要检查的权限列表，如果它们被授予
        /// </summary>
        public string[] Permissions { get; set; }

        /// <summary>
        /// If this property is set to true, all of the <see cref="Permissions"/> must be granted.
        /// 如果设置为true，所有的<see cref="Permissions"/>必须被授予
        /// If it's false, at least one of the <see cref="Permissions"/> must be granted.
        /// 如果设置为false，最少一个<see cref="Permissions"/>必须被授予
        /// Default: false.
        /// </summary>
        public bool RequiresAll { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimplePermissionDependency"/> class.
        /// 初始化<see cref="SimplePermissionDependency"/>类新实例
        /// </summary>
        /// <param name="permissions">The features. / 功能</param>
        public SimplePermissionDependency(params string[] permissions)
        {
            Permissions = permissions;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimplePermissionDependency"/> class.
        /// 初始化<see cref="SimplePermissionDependency"/>类新实例
        /// </summary>
        /// <param name="requiresAll">
        /// If this is set to true, all of the <see cref="Permissions"/> must be granted.
        /// 如果设置为true，所有的<see cref="Permissions"/>必须被授予
        /// If it's false, at least one of the <see cref="Permissions"/> must be granted.
        /// 如果设置为false，最少一个<see cref="Permissions"/>必须被授予
        /// </param>
        /// <param name="features">The features.</param>
        public SimplePermissionDependency(bool requiresAll, params string[] features)
            : this(features)
        {
            RequiresAll = requiresAll;
        }

        /// <summary>
        /// 检查是否满足权限依赖
        /// </summary>
        /// <param name="context">权限依赖上下文</param>
        /// <returns></returns>
        public Task<bool> IsSatisfiedAsync(IPermissionDependencyContext context)
        {
            return context.User != null
                ? context.PermissionChecker.IsGrantedAsync(context.User, RequiresAll, Permissions)
                : context.PermissionChecker.IsGrantedAsync(RequiresAll, Permissions);
        }
    }
}