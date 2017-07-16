using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Abp;
using Abp.Application.Features;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.IdentityFramework;
using Abp.MultiTenancy;
using Microsoft.AspNet.Identity;
using Derrick.Authorization.Roles;
using Derrick.Authorization.Users;
using Derrick.Editions;
using Derrick.MultiTenancy.Demo;
using Abp.Extensions;
using Abp.Notifications;
using Abp.Runtime.Security;
using Derrick.Notifications;

namespace Derrick.MultiTenancy
{
    /// <summary>
    /// 商户管理器
    /// </summary>
    public class TenantManager : AbpTenantManager<Tenant, User>
    {
        /// <summary>
        /// 工作单元管理器
        /// </summary>
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        /// <summary>
        /// 角色管理器
        /// </summary>
        private readonly RoleManager _roleManager;
        /// <summary>
        /// 用户管理器
        /// </summary>
        private readonly UserManager _userManager;
        /// <summary>
        /// 用户邮件管理器
        /// </summary>
        private readonly IUserEmailer _userEmailer;
        /// <summary>
        /// 商户Demo数据构建器
        /// </summary>
        private readonly TenantDemoDataBuilder _demoDataBuilder;
        /// <summary>
        /// 订阅通知管理器
        /// </summary>
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;
        /// <summary>
        /// APP通知接口
        /// </summary>
        private readonly IAppNotifier _appNotifier;
        /// <summary>
        /// ABP Zero数据迁移
        /// </summary>
        private readonly IAbpZeroDbMigrator _abpZeroDbMigrator;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tenantRepository">商户仓储</param>
        /// <param name="tenantFeatureRepository">商户功能仓储</param>
        /// <param name="editionManager">版本管理器</param>
        /// <param name="unitOfWorkManager">工作单元管理器</param>
        /// <param name="roleManager">角色管理器</param>
        /// <param name="userEmailer">用户邮件</param>
        /// <param name="demoDataBuilder">Demo数据构建器</param>
        /// <param name="userManager">用户管理器</param>
        /// <param name="notificationSubscriptionManager">订阅通知管理器</param>
        /// <param name="appNotifier">APP通知</param>
        /// <param name="featureValueStore">功能值存储器</param>
        /// <param name="abpZeroDbMigrator">ABP数据迁移</param>
        public TenantManager(
            IRepository<Tenant> tenantRepository,
            IRepository<TenantFeatureSetting, long> tenantFeatureRepository,
            EditionManager editionManager,
            IUnitOfWorkManager unitOfWorkManager,
            RoleManager roleManager,
            IUserEmailer userEmailer,
            TenantDemoDataBuilder demoDataBuilder,
            UserManager userManager,
            INotificationSubscriptionManager notificationSubscriptionManager,
            IAppNotifier appNotifier,
            IAbpZeroFeatureValueStore featureValueStore,
            IAbpZeroDbMigrator abpZeroDbMigrator)
            : base(
                  tenantRepository, 
                  tenantFeatureRepository, 
                  editionManager, 
                  featureValueStore)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _roleManager = roleManager;
            _userEmailer = userEmailer;
            _demoDataBuilder = demoDataBuilder;
            _userManager = userManager;
            _notificationSubscriptionManager = notificationSubscriptionManager;
            _appNotifier = appNotifier;
            _abpZeroDbMigrator = abpZeroDbMigrator;
        }

        /// <summary>
        /// 创建管理用户 - 异步
        /// </summary>
        /// <param name="tenancyName">商户名称</param>
        /// <param name="name">名称</param>
        /// <param name="adminPassword">管理员密码</param>
        /// <param name="adminEmailAddress">管理员邮箱</param>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="isActive">是否激活</param>
        /// <param name="editionId">版本ID</param>
        /// <param name="shouldChangePasswordOnNextLogin">在下次登录是否需要修改密码</param>
        /// <param name="sendActivationEmail">发送激活邮件</param>
        /// <returns></returns>
        public async Task<int> CreateWithAdminUserAsync(string tenancyName, string name, string adminPassword, string adminEmailAddress, string connectionString, bool isActive, int? editionId, bool shouldChangePasswordOnNextLogin, bool sendActivationEmail)
        {
            int newTenantId;
            long newAdminId;

            using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
            {
                //Create tenant
                var tenant = new Tenant(tenancyName, name)
                {
                    IsActive = isActive,
                    EditionId = editionId,
                    ConnectionString = connectionString.IsNullOrWhiteSpace() ? null : SimpleStringCipher.Instance.Encrypt(connectionString)
                };

                CheckErrors(await CreateAsync(tenant));
                await _unitOfWorkManager.Current.SaveChangesAsync(); //To get new tenant's id.

                //Create tenant database
                _abpZeroDbMigrator.CreateOrMigrateForTenant(tenant);

                //We are working entities of new tenant, so changing tenant filter
                using (_unitOfWorkManager.Current.SetTenantId(tenant.Id))
                {
                    //Create static roles for new tenant
                    CheckErrors(await _roleManager.CreateStaticRoles(tenant.Id));
                    await _unitOfWorkManager.Current.SaveChangesAsync(); //To get static role ids

                    //grant all permissions to admin role
                    var adminRole = _roleManager.Roles.Single(r => r.Name == StaticRoleNames.Tenants.Admin);
                    await _roleManager.GrantAllPermissionsAsync(adminRole);

                    //User role should be default
                    var userRole = _roleManager.Roles.Single(r => r.Name == StaticRoleNames.Tenants.User);
                    userRole.IsDefault = true;
                    CheckErrors(await _roleManager.UpdateAsync(userRole));

                    //Create admin user for the tenant
                    if (adminPassword.IsNullOrEmpty())
                    {
                        adminPassword = User.CreateRandomPassword();
                    }

                    var adminUser = User.CreateTenantAdminUser(tenant.Id, adminEmailAddress, adminPassword);
                    adminUser.ShouldChangePasswordOnNextLogin = shouldChangePasswordOnNextLogin;
                    adminUser.IsActive = true;

                    CheckErrors(await _userManager.CreateAsync(adminUser));
                    await _unitOfWorkManager.Current.SaveChangesAsync(); //To get admin user's id

                    //Assign admin user to admin role!
                    CheckErrors(await _userManager.AddToRoleAsync(adminUser.Id, adminRole.Name));

                    //Notifications
                    await _appNotifier.WelcomeToTheApplicationAsync(adminUser);

                    //Send activation email
                    if (sendActivationEmail)
                    {
                        adminUser.SetNewEmailConfirmationCode();
                        await _userEmailer.SendEmailActivationLinkAsync(adminUser, adminPassword);
                    }

                    await _unitOfWorkManager.Current.SaveChangesAsync();

                    await _demoDataBuilder.BuildForAsync(tenant);

                    newTenantId = tenant.Id;
                    newAdminId = adminUser.Id;
                }

                await uow.CompleteAsync();
            }

            //Used a second UOW since UOW above sets some permissions and _notificationSubscriptionManager.SubscribeToAllAvailableNotificationsAsync needs these permissions to be saved.
            using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
            {
                using (_unitOfWorkManager.Current.SetTenantId(newTenantId))
                {
                    await _notificationSubscriptionManager.SubscribeToAllAvailableNotificationsAsync(new UserIdentifier(newTenantId, newAdminId));
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    await uow.CompleteAsync();
                }
            }

            return newTenantId;
        }

        /// <summary>
        /// 检查错误
        /// </summary>
        /// <param name="identityResult">标识结果</param>
        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
