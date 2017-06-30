using System.Threading.Tasks;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Castle.Core.Logging;

namespace Abp.Authorization
{
    /// <summary>
    /// Application should inherit this class to implement <see cref="IPermissionChecker"/>.
    /// 应用程序必须继承此类来实现<see cref="IPermissionChecker"/>
    /// </summary>
    /// <typeparam name="TTenant">商户</typeparam>
    /// <typeparam name="TRole">角色</typeparam>
    /// <typeparam name="TUser">用户</typeparam>
    public abstract class PermissionChecker<TTenant, TRole, TUser> : IPermissionChecker, ITransientDependency, IIocManagerAccessor
        where TRole : AbpRole<TUser>, new()
        where TUser : AbpUser<TUser>
        where TTenant : AbpTenant<TUser>
    {
        /// <summary>
        /// 用户管理
        /// </summary>
        private readonly AbpUserManager<TRole, TUser> _userManager;
        /// <summary>
        /// IOC管理器
        /// </summary>
        public IIocManager IocManager { get; set; }
        /// <summary>
        /// 日志引用
        /// </summary>
        public ILogger Logger { get; set; }
        /// <summary>
        /// ABP Session
        /// </summary>
        public IAbpSession AbpSession { get; set; }
        /// <summary>
        /// 当前工作单元提供者
        /// </summary>
        public ICurrentUnitOfWorkProvider CurrentUnitOfWorkProvider { get; set; }

        /// <summary>
        /// 构造函数.
        /// </summary>
        protected PermissionChecker(AbpUserManager<TRole, TUser> userManager)
        {
            _userManager = userManager;

            Logger = NullLogger.Instance;
            AbpSession = NullAbpSession.Instance;
        }
        /// <summary>
        /// 是否准许
        /// </summary>
        /// <param name="permissionName">权限名称</param>
        /// <returns></returns>
        public virtual async Task<bool> IsGrantedAsync(string permissionName)
        {
            return AbpSession.UserId.HasValue && await _userManager.IsGrantedAsync(AbpSession.UserId.Value, permissionName);
        }
        /// <summary>
        /// 是否准许
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="permissionName">权限名称</param>
        /// <returns></returns>
        public virtual async Task<bool> IsGrantedAsync(long userId, string permissionName)
        {
            return await _userManager.IsGrantedAsync(userId, permissionName);
        }
        /// <summary>
        /// 是否准许
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="permissionName">权限名称</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<bool> IsGrantedAsync(UserIdentifier user, string permissionName)
        {
            if (CurrentUnitOfWorkProvider == null || CurrentUnitOfWorkProvider.Current == null)
            {
                return await IsGrantedAsync(user.UserId, permissionName);
            }

            using (CurrentUnitOfWorkProvider.Current.SetTenantId(user.TenantId))
            {
                return await _userManager.IsGrantedAsync(user.UserId, permissionName);
            }
        }
    }
}
