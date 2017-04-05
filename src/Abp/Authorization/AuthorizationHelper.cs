using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;
using Abp.Application.Features;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Localization;
using Abp.Reflection;
using Abp.Runtime.Session;

namespace Abp.Authorization
{
    /// <summary>
    /// 授权帮助类
    /// </summary>
    internal class AuthorizationHelper : IAuthorizationHelper, ITransientDependency
    {
        /// <summary>
        /// ABP Session的引用
        /// </summary>
        public IAbpSession AbpSession { get; set; }

        /// <summary>
        /// 权限检查器
        /// </summary>
        public IPermissionChecker PermissionChecker { get; set; }

        /// <summary>
        /// 功能检查器
        /// </summary>
        public IFeatureChecker FeatureChecker { get; set; }

        /// <summary>
        /// 本地化管理器
        /// </summary>
        public ILocalizationManager LocalizationManager { get; set; }

        private readonly IFeatureChecker _featureChecker;
        /// <summary>
        /// 授权配置
        /// </summary>
        private readonly IAuthorizationConfiguration _configuration;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="featureChecker">功能检查器</param>
        /// <param name="configuration">授权配置</param>
        public AuthorizationHelper(IFeatureChecker featureChecker, IAuthorizationConfiguration configuration)
        {
            _featureChecker = featureChecker;
            _configuration = configuration;
            AbpSession = NullAbpSession.Instance;
            PermissionChecker = NullPermissionChecker.Instance;
            LocalizationManager = NullLocalizationManager.Instance;
        }

        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="authorizeAttributes">ABP授权特性</param>
        /// <returns></returns>
        public async Task AuthorizeAsync(IEnumerable<IAbpAuthorizeAttribute> authorizeAttributes)
        {
            if (!_configuration.IsEnabled)
            {
                return;
            }

            if (!AbpSession.UserId.HasValue)
            {
                throw new AbpAuthorizationException(
                    LocalizationManager.GetString(AbpConsts.LocalizationSourceName, "CurrentUserDidNotLoginToTheApplication")
                    );
            }

            foreach (var authorizeAttribute in authorizeAttributes)
            {
                await PermissionChecker.AuthorizeAsync(authorizeAttribute.RequireAllPermissions, authorizeAttribute.Permissions);
            }
        }

        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="methodInfo">方法信息</param>
        /// <returns></returns>
        public async Task AuthorizeAsync(MethodInfo methodInfo)
        {
            if (!_configuration.IsEnabled)
            {
                return;
            }

            if (AllowAnonymous(methodInfo))
            {
                return;
            }
            
            //Authorize
            await CheckFeatures(methodInfo);
            await CheckPermissions(methodInfo);
        }

        /// <summary>
        /// 检查功能
        /// </summary>
        /// <param name="methodInfo">方法信息</param>
        /// <returns></returns>
        private async Task CheckFeatures(MethodInfo methodInfo)
        {
            var featureAttributes =
                ReflectionHelper.GetAttributesOfMemberAndDeclaringType<RequiresFeatureAttribute>(
                    methodInfo
                    );

            if (featureAttributes.Count <= 0)
            {
                return;
            }

            foreach (var featureAttribute in featureAttributes)
            {
                await _featureChecker.CheckEnabledAsync(featureAttribute.RequiresAll, featureAttribute.Features);
            }
        }

        /// <summary>
        /// 检查权限
        /// </summary>
        /// <param name="methodInfo">方法信息</param>
        /// <returns></returns>
        private async Task CheckPermissions(MethodInfo methodInfo)
        {
            var authorizeAttributes =
                ReflectionHelper.GetAttributesOfMemberAndDeclaringType(
                    methodInfo
                ).OfType<IAbpAuthorizeAttribute>().ToArray();

            if (!authorizeAttributes.Any())
            {
                return;
            }

            await AuthorizeAsync(authorizeAttributes);
        }

        /// <summary>
        /// 匿名
        /// </summary>
        /// <param name="methodInfo">方法信息</param>
        /// <returns></returns>
        private static bool AllowAnonymous(MethodInfo methodInfo)
        {
            return ReflectionHelper.GetAttributesOfMemberAndDeclaringType(methodInfo)
                .OfType<IAbpAllowAnonymousAttribute>().Any();
        }
    }
}