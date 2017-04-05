using Abp.Dependency;

namespace Abp.Authorization
{
    /// <summary>
    /// This is the main interface to define permissions for an application.Implement it to define permissions for your module.
    /// 这是为一个应用定义权限的主要接口,实现对模块的权限定义
    /// </summary>
    public abstract class AuthorizationProvider : ITransientDependency
    {
        /// <summary>
        /// This method is called once on application startup to allow to define permissions.
        /// 这个方法在应用程序启动时调用，以允许定义权限。
        /// </summary>
        /// <param name="context">Permission definition context / 权限定义上下文</param>
        public abstract void SetPermissions(IPermissionDefinitionContext context);
    }
}