using System.Collections.Generic;
using Abp.Authorization.Users;
using Abp.Threading;
using Microsoft.AspNet.Identity;

namespace Abp.MultiTenancy
{
    /// <summary>
    /// ABP商户管理扩展
    /// </summary>
    public static class AbpTenantManagerExtensions
    {
        /// <summary>
        /// 创建商户
        /// </summary>
        /// <typeparam name="TTenant">商户类型</typeparam>
        /// <typeparam name="TUser">用户类型</typeparam>
        /// <param name="tenantManager">商户管理</param>
        /// <param name="tenant">商户</param>
        /// <returns></returns>
        public static IdentityResult Create<TTenant, TUser>(this AbpTenantManager<TTenant, TUser> tenantManager, TTenant tenant)
            where TTenant : AbpTenant<TUser>
            where TUser : AbpUser<TUser>
        {
            return AsyncHelper.RunSync(() => tenantManager.CreateAsync(tenant));
        }
        /// <summary>
        /// 修改商户
        /// </summary>
        /// <typeparam name="TTenant">商户类型</typeparam>
        /// <typeparam name="TUser">用户类型</typeparam>
        /// <param name="tenantManager">商户管理</param>
        /// <param name="tenant">商户</param>
        /// <returns></returns>
        public static IdentityResult Update<TTenant, TUser>(this AbpTenantManager<TTenant, TUser> tenantManager, TTenant tenant)
            where TTenant : AbpTenant<TUser>
            where TUser : AbpUser<TUser>
        {
            return AsyncHelper.RunSync(() => tenantManager.UpdateAsync(tenant));
        }
        /// <summary>
        /// 根据ID查找商户
        /// </summary>
        /// <typeparam name="TTenant">商户类型</typeparam>
        /// <typeparam name="TUser">用户类型</typeparam>
        /// <param name="tenantManager">商户管理</param>
        /// <param name="id">商户ID</param>
        /// <returns></returns>
        public static TTenant FindById<TTenant, TUser>(this AbpTenantManager<TTenant, TUser> tenantManager, int id)
            where TTenant : AbpTenant<TUser>
            where TUser : AbpUser<TUser>
        {
            return AsyncHelper.RunSync(() => tenantManager.FindByIdAsync(id));
        }
        /// <summary>
        /// 根据商户ID获取商户
        /// </summary>
        /// <typeparam name="TTenant">商户类型</typeparam>
        /// <typeparam name="TUser">用户类型</typeparam>
        /// <param name="tenantManager">商户管理</param>
        /// <param name="id">商户ID</param>
        /// <returns></returns>
        public static TTenant GetById<TTenant, TUser>(this AbpTenantManager<TTenant, TUser> tenantManager, int id)
            where TTenant : AbpTenant<TUser>
            where TUser : AbpUser<TUser>
        {
            return AsyncHelper.RunSync(() => tenantManager.GetByIdAsync(id));
        }
        /// <summary>
        /// 根据商户名称查找商户
        /// </summary>
        /// <typeparam name="TTenant">商户类型</typeparam>
        /// <typeparam name="TUser">用户类型</typeparam>
        /// <param name="tenantManager">商户管理</param>
        /// <param name="tenancyName">商户名称</param>
        /// <returns></returns>
        public static TTenant FindByTenancyName<TTenant, TUser>(this AbpTenantManager<TTenant, TUser> tenantManager, string tenancyName)
            where TTenant : AbpTenant<TUser>
            where TUser : AbpUser<TUser>
        {
            return AsyncHelper.RunSync(() => tenantManager.FindByTenancyNameAsync(tenancyName));
        }
        /// <summary>
        /// 删除商户
        /// </summary>
        /// <typeparam name="TTenant">商户类型</typeparam>
        /// <typeparam name="TUser">用户类型</typeparam>
        /// <param name="tenantManager">商户管理</param>
        /// <param name="tenant">商户对象</param>
        /// <returns></returns>
        public static IdentityResult Delete<TTenant, TUser>(this AbpTenantManager<TTenant, TUser> tenantManager, TTenant tenant)
            where TTenant : AbpTenant<TUser>
            where TUser : AbpUser<TUser>
        {
            return AsyncHelper.RunSync(() => tenantManager.DeleteAsync(tenant));
        }
        /// <summary>
        /// 获取功能值或Null
        /// </summary>
        /// <typeparam name="TTenant">商户类型</typeparam>
        /// <typeparam name="TUser">用户类型</typeparam>
        /// <param name="tenantManager">商户管理</param>
        /// <param name="tenantId">商户ID</param>
        /// <param name="featureName">功能名称</param>
        /// <returns></returns>
        public static string GetFeatureValueOrNull<TTenant, TUser>(this AbpTenantManager<TTenant, TUser> tenantManager, int tenantId, string featureName)
            where TTenant : AbpTenant<TUser>
            where TUser : AbpUser<TUser>
        {
            return AsyncHelper.RunSync(() => tenantManager.GetFeatureValueOrNullAsync(tenantId, featureName));
        }
        /// <summary>
        /// 获取功能值
        /// </summary>
        /// <typeparam name="TTenant">商户类型</typeparam>
        /// <typeparam name="TUser">用户类型</typeparam>
        /// <param name="tenantManager">商户管理</param>
        /// <param name="tenantId">商户ID</param>
        /// <returns></returns>
        public static IReadOnlyList<NameValue> GetFeatureValues<TTenant, TUser>(this AbpTenantManager<TTenant, TUser> tenantManager, int tenantId)
            where TTenant : AbpTenant<TUser>
            where TUser : AbpUser<TUser>
        {
            return AsyncHelper.RunSync(() => tenantManager.GetFeatureValuesAsync(tenantId));
        }
        /// <summary>
        /// 设置功能值
        /// </summary>
        /// <typeparam name="TTenant">商户类型</typeparam>
        /// <typeparam name="TUser">用户类型</typeparam>
        /// <param name="tenantManager">商户管理</param>
        /// <param name="tenantId">商户ID</param>
        /// <param name="values">值数组</param>
        public static void SetFeatureValues<TTenant, TUser>(this AbpTenantManager<TTenant, TUser> tenantManager, int tenantId, params NameValue[] values)
            where TTenant : AbpTenant<TUser>
            where TUser : AbpUser<TUser>
        {
            AsyncHelper.RunSync(() => tenantManager.SetFeatureValuesAsync(tenantId, values));
        }
        /// <summary>
        /// 设置功能值
        /// </summary>
        /// <typeparam name="TTenant">商户类型</typeparam>
        /// <typeparam name="TUser">用户类型</typeparam>
        /// <param name="tenantManager">商户管理</param>
        /// <param name="tenantId">商户ID</param>
        /// <param name="featureName">功能值</param>
        /// <param name="value">值</param>
        public static void SetFeatureValue<TTenant, TUser>(this AbpTenantManager<TTenant, TUser> tenantManager, int tenantId, string featureName, string value)
            where TTenant : AbpTenant<TUser>
            where TUser : AbpUser<TUser>
        {
            AsyncHelper.RunSync(() => tenantManager.SetFeatureValueAsync(tenantId, featureName, value));
        }
        /// <summary>
        /// 设置功能值
        /// </summary>
        /// <typeparam name="TTenant">商户类型</typeparam>
        /// <typeparam name="TUser">用户类型</typeparam>
        /// <param name="tenantManager">商户管理</param>
        /// <param name="tenant">商户对象</param>
        /// <param name="featureName">功能名称</param>
        /// <param name="value">值</param>
        public static void SetFeatureValue<TTenant, TUser>(this AbpTenantManager<TTenant, TUser> tenantManager, TTenant tenant, string featureName, string value)
            where TTenant : AbpTenant<TUser>
            where TUser : AbpUser<TUser>
        {
            AsyncHelper.RunSync(() => tenantManager.SetFeatureValueAsync(tenant, featureName, value));
        }
        /// <summary>
        /// 重置所有功能
        /// </summary>
        /// <typeparam name="TTenant">商户类型</typeparam>
        /// <typeparam name="TUser">用户类型</typeparam>
        /// <param name="tenantManager">商户管理</param>
        /// <param name="tenantId">商户ID</param>
        public static void ResetAllFeatures<TTenant, TUser>(this AbpTenantManager<TTenant, TUser> tenantManager, int tenantId)
            where TTenant : AbpTenant<TUser>
            where TUser : AbpUser<TUser>
        {
            AsyncHelper.RunSync(() => tenantManager.ResetAllFeaturesAsync(tenantId));
        }

    }
}