using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Abp.Authorization.Roles;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq;
using Microsoft.AspNet.Identity;

namespace Abp.Authorization.Users
{
    /// <summary>
    /// Implements 'User Store' of ASP.NET Identity Framework.
    /// 实现了ASP.NET Identity框架的用户存储
    /// </summary>
    public abstract class AbpUserStore<TRole, TUser> :
        IUserStore<TUser, long>,
        IUserPasswordStore<TUser, long>,
        IUserEmailStore<TUser, long>,
        IUserLoginStore<TUser, long>,
        IUserRoleStore<TUser, long>,
        IQueryableUserStore<TUser, long>,
        IUserLockoutStore<TUser, long>,
        IUserPermissionStore<TUser>,
        IUserPhoneNumberStore<TUser, long>,
        IUserClaimStore<TUser, long>,
        IUserSecurityStampStore<TUser, long>,
        IUserTwoFactorStore<TUser, long>,

        ITransientDependency

        where TRole : AbpRole<TUser>
        where TUser : AbpUser<TUser>
    {
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        private readonly IRepository<TUser, long> _userRepository;
        private readonly IRepository<UserLogin, long> _userLoginRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<UserClaim, long> _userClaimRepository;
        private readonly IRepository<TRole> _roleRepository;
        private readonly IRepository<UserPermissionSetting, long> _userPermissionSettingRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        
        /// <summary>
        /// 构造函数.
        /// </summary>
        protected AbpUserStore(
            IRepository<TUser, long> userRepository,
            IRepository<UserLogin, long> userLoginRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<TRole> roleRepository,
            IRepository<UserPermissionSetting, long> userPermissionSettingRepository,
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<UserClaim, long> userClaimRepository)
        {
            _userRepository = userRepository;
            _userLoginRepository = userLoginRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _userClaimRepository = userClaimRepository;
            _userPermissionSettingRepository = userPermissionSettingRepository;

            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        }

        #region IQueryableUserStore
        /// <summary>
        /// 用户列表
        /// </summary>
        public virtual IQueryable<TUser> Users => _userRepository.GetAll();

        #endregion

        #region IUserStore
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        public virtual async Task CreateAsync(TUser user)
        {
            await _userRepository.InsertAsync(user);
        }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(TUser user)
        {
            await _userRepository.UpdateAsync(user);
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(TUser user)
        {
            await _userRepository.DeleteAsync(user.Id);
        }
        /// <summary>
        /// 根据用户ID查找用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public virtual async Task<TUser> FindByIdAsync(long userId)
        {
            return await _userRepository.FirstOrDefaultAsync(userId);
        }
        /// <summary>
        /// 根据UserName查找用户
        /// </summary>
        /// <param name="userName">UserName</param>
        /// <returns></returns>
        public virtual async Task<TUser> FindByNameAsync(string userName)
        {
            return await _userRepository.FirstOrDefaultAsync(
                user => user.UserName == userName
            );
        }
        /// <summary>
        /// 根据邮箱查找用户
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <returns></returns>
        public virtual async Task<TUser> FindByEmailAsync(string email)
        {
            return await _userRepository.FirstOrDefaultAsync(
                user => user.EmailAddress == email
            );
        }

        /// <summary>
        /// Tries to find a user with user name or email address in current tenant.
        /// 尝试去查找一个UserName或邮箱地址在当前商户中的用户
        /// </summary>
        /// <param name="userNameOrEmailAddress">User name or email address / UserName或邮箱地址</param>
        /// <returns>User or null</returns>
        public virtual async Task<TUser> FindByNameOrEmailAsync(string userNameOrEmailAddress)
        {
            return await _userRepository.FirstOrDefaultAsync(
                user => (user.UserName == userNameOrEmailAddress || user.EmailAddress == userNameOrEmailAddress)
                );
        }

        /// <summary>
        /// Tries to find a user with user name or email address in given tenant.
        /// 尝试去查找一个UserName或邮箱在指定的商户中的用户
        /// </summary>
        /// <param name="tenantId">Tenant Id / 商户ID</param>
        /// <param name="userNameOrEmailAddress">User name or email address / UserName或邮箱地址</param>
        /// <returns>User or null</returns>
        [UnitOfWork]
        public virtual async Task<TUser> FindByNameOrEmailAsync(int? tenantId, string userNameOrEmailAddress)
        {
            using (_unitOfWorkManager.Current.SetTenantId(tenantId))
            {
                return await FindByNameOrEmailAsync(userNameOrEmailAddress);
            }
        }

        #endregion
        
        #region IUserPasswordStore
        /// <summary>
        /// 设置密码Hash值
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="passwordHash">密码哈希值</param>
        /// <returns></returns>
        public virtual Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            user.Password = passwordHash;
            return Task.FromResult(0);
        }
        /// <summary>
        /// 获取给定用户的密码哈希值
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        public virtual Task<string> GetPasswordHashAsync(TUser user)
        {
            return Task.FromResult(user.Password);
        }
        /// <summary>
        /// 判断指定用户是否有密码
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual Task<bool> HasPasswordAsync(TUser user)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.Password));
        }

        #endregion

        #region IUserEmailStore
        /// <summary>
        /// 设置指定用户的邮箱
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="email">邮箱地址</param>
        /// <returns></returns>
        public virtual Task SetEmailAsync(TUser user, string email)
        {
            user.EmailAddress = email;
            return Task.FromResult(0);
        }
        /// <summary>
        /// 获取指定用户的邮箱
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        public virtual Task<string> GetEmailAsync(TUser user)
        {
            return Task.FromResult(user.EmailAddress);
        }
        /// <summary>
        /// 判断指定用户的邮箱是否确认
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        public virtual Task<bool> GetEmailConfirmedAsync(TUser user)
        {
            return Task.FromResult(user.IsEmailConfirmed);
        }
        /// <summary>
        /// 设置用户的邮箱确认信息
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="confirmed">是否确认</param>
        /// <returns></returns>
        public virtual Task SetEmailConfirmedAsync(TUser user, bool confirmed)
        {
            user.IsEmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        #endregion

        #region IUserLoginStore
        /// <summary>
        /// 添加用户的登录信息
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="login">登录信息对象</param>
        /// <returns></returns>
        public virtual async Task AddLoginAsync(TUser user, UserLoginInfo login)
        {
            await _userLoginRepository.InsertAsync(
                new UserLogin
                {
                    TenantId = user.TenantId,
                    LoginProvider = login.LoginProvider,
                    ProviderKey = login.ProviderKey,
                    UserId = user.Id
                });
        }
        /// <summary>
        /// 移除用户的登录信息
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="login">登录信息对象</param>
        /// <returns></returns>
        public virtual async Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            await _userLoginRepository.DeleteAsync(
                ul => ul.UserId == user.Id &&
                      ul.LoginProvider == login.LoginProvider &&
                      ul.ProviderKey == login.ProviderKey
            );
        }
        /// <summary>
        /// 获取用户的登录信息列表
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns>登录信息列表</returns>
        public virtual async Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            return (await _userLoginRepository.GetAllListAsync(ul => ul.UserId == user.Id))
                .Select(ul => new UserLoginInfo(ul.LoginProvider, ul.ProviderKey))
                .ToList();
        }
        /// <summary>
        /// 根据用户登录信息查找用户
        /// </summary>
        /// <param name="login">用户登录信息</param>
        /// <returns></returns>
        public virtual async Task<TUser> FindAsync(UserLoginInfo login)
        {
            var userLogin = await _userLoginRepository.FirstOrDefaultAsync(
                ul => ul.LoginProvider == login.LoginProvider && ul.ProviderKey == login.ProviderKey
            );

            if (userLogin == null)
            {
                return null;
            }

            return await _userRepository.FirstOrDefaultAsync(u => u.Id == userLogin.UserId);
        }
        /// <summary>
        /// 根据用户登录信息获取用户列表
        /// </summary>
        /// <param name="login">用户登录信息</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual Task<List<TUser>> FindAllAsync(UserLoginInfo login)
        {
            var query = from userLogin in _userLoginRepository.GetAll()
                join user in _userRepository.GetAll() on userLogin.UserId equals user.Id
                where userLogin.LoginProvider == login.LoginProvider && userLogin.ProviderKey == login.ProviderKey
                select user;

            return Task.FromResult(query.ToList());
        }
        /// <summary>
        /// 根据指定商户以及用户登录信息查找用户
        /// </summary>
        /// <param name="tenantId">商户ID</param>
        /// <param name="login">用户登录信息</param>
        /// <returns></returns>
        public virtual Task<TUser> FindAsync(int? tenantId, UserLoginInfo login)
        {
            using (_unitOfWorkManager.Current.SetTenantId(tenantId))
            {
                var query = from userLogin in _userLoginRepository.GetAll()
                            join user in _userRepository.GetAll() on userLogin.UserId equals user.Id
                            where userLogin.LoginProvider == login.LoginProvider && userLogin.ProviderKey == login.ProviderKey
                            select user;

                return Task.FromResult(query.FirstOrDefault());
            }
        }

        #endregion

        #region IUserRoleStore

        /// <summary>
        /// 给指定角色添加用户
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="roleName">角色名称</param>
        /// <returns></returns>
        public virtual async Task AddToRoleAsync(TUser user, string roleName)
        {
            var role = await GetRoleByNameAsync(roleName);
            await _userRoleRepository.InsertAsync(new UserRole(user.TenantId, user.Id, role.Id));
        }
        /// <summary>
        /// 给指定角色移除用户
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="roleName">角色名称</param>
        /// <returns></returns>
        public virtual async Task RemoveFromRoleAsync(TUser user, string roleName)
        {
            var role = await GetRoleByNameAsync(roleName);
            var userRole = await _userRoleRepository.FirstOrDefaultAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id);
            if (userRole == null)
            {
                return;
            }

            await _userRoleRepository.DeleteAsync(userRole);
        }
        /// <summary>
        /// 获取用户的所有角色信息
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<IList<string>> GetRolesAsync(TUser user)
        {
            var query = from userRole in _userRoleRepository.GetAll()
                join role in _roleRepository.GetAll() on userRole.RoleId equals role.Id
                where userRole.UserId == user.Id
                select role.Name;

            return await AsyncQueryableExecuter.ToListAsync(query);
        }
        /// <summary>
        /// 判断用户是否在指定角色中
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="roleName">角色名称</param>
        /// <returns></returns>
        public virtual async Task<bool> IsInRoleAsync(TUser user, string roleName)
        {
            var role = await GetRoleByNameAsync(roleName);
            return await _userRoleRepository.FirstOrDefaultAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id) != null;
        }

        #endregion

        #region IUserPermissionStore

        /// <summary>
        /// 给用户添加指定权限
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="permissionGrant">权限授予信息</param>
        /// <returns></returns>
        public virtual async Task AddPermissionAsync(TUser user, PermissionGrantInfo permissionGrant)
        {
            if (await HasPermissionAsync(user.Id, permissionGrant))
            {
                return;
            }

            await _userPermissionSettingRepository.InsertAsync(
                new UserPermissionSetting
                {
                    TenantId = user.TenantId,
                    UserId = user.Id,
                    Name = permissionGrant.Name,
                    IsGranted = permissionGrant.IsGranted
                });
        }
        /// <summary>
        /// 给用户移除权限
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="permissionGrant">权限授予信息</param>
        /// <returns></returns>
        public virtual async Task RemovePermissionAsync(TUser user, PermissionGrantInfo permissionGrant)
        {
            await _userPermissionSettingRepository.DeleteAsync(
                permissionSetting => permissionSetting.UserId == user.Id &&
                                     permissionSetting.Name == permissionGrant.Name &&
                                     permissionSetting.IsGranted == permissionGrant.IsGranted
            );
        }
        /// <summary>
        /// 获取用户权限列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public virtual async Task<IList<PermissionGrantInfo>> GetPermissionsAsync(long userId)
        {
            return (await _userPermissionSettingRepository.GetAllListAsync(p => p.UserId == userId))
                .Select(p => new PermissionGrantInfo(p.Name, p.IsGranted))
                .ToList();
        }
        /// <summary>
        /// 判断用户是否有指定的权限
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="permissionGrant">权限授予信息</param>
        /// <returns></returns>
        public virtual async Task<bool> HasPermissionAsync(long userId, PermissionGrantInfo permissionGrant)
        {
            return await _userPermissionSettingRepository.FirstOrDefaultAsync(
                       p => p.UserId == userId &&
                            p.Name == permissionGrant.Name &&
                            p.IsGranted == permissionGrant.IsGranted
                   ) != null;
        }
        /// <summary>
        /// 为用户移除所有权限设置
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        public virtual async Task RemoveAllPermissionSettingsAsync(TUser user)
        {
            await _userPermissionSettingRepository.DeleteAsync(s => s.UserId == user.Id);
        }

        #endregion

        #region IUserLockoutStore

        /// <summary>
        /// 获取用户的锁定最后日期
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        public Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user)
        {
            return Task.FromResult(
                user.LockoutEndDateUtc.HasValue
                    ? new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDateUtc.Value, DateTimeKind.Utc))
                    : new DateTimeOffset()
            );
        }
        /// <summary>
        /// 设置用户的锁定最后日期
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="lockoutEnd">锁定最后日期设置对象</param>
        /// <returns></returns>
        public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd)
        {
            user.LockoutEndDateUtc = lockoutEnd == DateTimeOffset.MinValue ? new DateTime?() : lockoutEnd.UtcDateTime;
            return Task.FromResult(0);
        }
        /// <summary>
        /// 增加用户访问失败的次数
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        public Task<int> IncrementAccessFailedCountAsync(TUser user)
        {
            return Task.FromResult(++user.AccessFailedCount);
        }
        /// <summary>
        /// 重置用户访问失败的次数
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        public Task ResetAccessFailedCountAsync(TUser user)
        {
            user.AccessFailedCount = 0;
            return Task.FromResult(0);
        }
        /// <summary>
        /// 获取用户访问失败的次数
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        public Task<int> GetAccessFailedCountAsync(TUser user)
        {
            return Task.FromResult(user.AccessFailedCount);
        }
        /// <summary>
        /// 获取用户锁定是否启用
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        public Task<bool> GetLockoutEnabledAsync(TUser user)
        {
            return Task.FromResult(user.IsLockoutEnabled);
        }
        /// <summary>
        /// 设置用户锁定是否启用
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="enabled">是否启用</param>
        /// <returns></returns>
        public Task SetLockoutEnabledAsync(TUser user, bool enabled)
        {
            user.IsLockoutEnabled = enabled;
            return Task.FromResult(0);
        }

        #endregion

        #region IUserPhoneNumberStore
        /// <summary>
        /// 设置用户电话号码
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="phoneNumber">电话号码</param>
        /// <returns></returns>
        public Task SetPhoneNumberAsync(TUser user, string phoneNumber)
        {
            user.PhoneNumber = phoneNumber;
            return Task.FromResult(0);
        }
        /// <summary>
        /// 获取用户电话号码
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        public Task<string> GetPhoneNumberAsync(TUser user)
        {
            return Task.FromResult(user.PhoneNumber);
        }
        /// <summary>
        /// 获取用户电话号码是否确认
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        public Task<bool> GetPhoneNumberConfirmedAsync(TUser user)
        {
            return Task.FromResult(user.IsPhoneNumberConfirmed);
        }
        /// <summary>
        /// 设置用户电话号码是否确认
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="confirmed">是否确认</param>
        /// <returns></returns>
        public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed)
        {
            user.IsPhoneNumberConfirmed = confirmed;
            return Task.FromResult(0);
        }

        #endregion

        #region IUserClaimStore
        /// <summary>
        /// 获取用户的声明
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        public async Task<IList<Claim>> GetClaimsAsync(TUser user)
        {
            var userClaims = await _userClaimRepository.GetAllListAsync(uc => uc.UserId == user.Id);
            return userClaims.Select(uc => new Claim(uc.ClaimType, uc.ClaimValue)).ToList();
        }
        /// <summary>
        /// 添加用户声明
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="claim">声明</param>
        /// <returns></returns>
        public Task AddClaimAsync(TUser user, Claim claim)
        {
            return _userClaimRepository.InsertAsync(new UserClaim(user, claim));
        }
        /// <summary>
        /// 移除用户声明
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="claim">声明</param>
        /// <returns></returns>
        public Task RemoveClaimAsync(TUser user, Claim claim)
        {
            return _userClaimRepository.DeleteAsync(
                uc => uc.UserId == user.Id &&
                      uc.ClaimType == claim.Type &&
                      uc.ClaimValue == claim.Value
            );
        }

        #endregion

        #region IDisposable
        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            //No need to dispose since using IOC.
        }

        #endregion

        #region Helpers
        /// <summary>
        /// 通过角色名称获取角色对象
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <returns></returns>
        private async Task<TRole> GetRoleByNameAsync(string roleName)
        {
            var role = await _roleRepository.FirstOrDefaultAsync(r => r.Name == roleName);
            if (role == null)
            {
                throw new AbpException("Could not find a role with name: " + roleName);
            }

            return role;
        }

        #endregion

        #region IUserSecurityStampStore
        /// <summary>
        /// 设置用户安全标记
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="stamp">安全标记</param>
        /// <returns></returns>
        public Task SetSecurityStampAsync(TUser user, string stamp)
        {
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }
        /// <summary>
        /// 获取用户安全标记
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        public Task<string> GetSecurityStampAsync(TUser user)
        {
            return Task.FromResult(user.SecurityStamp);
        }

        #endregion
        /// <summary>
        /// 设置用户是否启用双身份验证
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="enabled">是否启用双重身份验证</param>
        /// <returns></returns>
        public Task SetTwoFactorEnabledAsync(TUser user, bool enabled)
        {
            user.IsTwoFactorEnabled = enabled;
            return Task.FromResult(0);
        }
        /// <summary>
        /// 获取用户是否启用双重身份验证
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> GetTwoFactorEnabledAsync(TUser user)
        {
            return Task.FromResult(user.IsTwoFactorEnabled);
        }
    }
}
