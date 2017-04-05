using System;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Runtime.Session;
using Abp.Threading;

namespace Abp.Application.Features
{
    /// <summary>
    /// Some extension methods for <see cref="IFeatureChecker"/>.
    /// <see cref="IFeatureChecker"/>扩展方法
    /// </summary>
    public static class FeatureCheckerExtensions
    {
        /// <summary>
        /// Gets value of a feature by it's name. This is sync version of <see cref="IFeatureChecker.GetValueAsync(string)"/>
        /// 根据名称获取功能值，这是<see cref="IFeatureChecker.GetValueAsync(string)"/>的异步版本
        /// This is a shortcut for <see cref="GetValue(IFeatureChecker, int, string)"/> that uses <see cref="IAbpSession.TenantId"/> as tenantId.So, this method should be used only if TenantId can be obtained from the session.
        /// <see cref="GetValue(IFeatureChecker, int, string)"/>用<see cref="IAbpSession.TenantId"/>作为租户ID的快捷方式，所以此方法应该被使用为那些可以从session获取租户ID值的
        /// </summary>
        /// <param name="featureChecker"><see cref="IFeatureChecker"/> instance / <see cref="IFeatureChecker"/>实例</param>
        /// <param name="featureName">Unique feature name / 功能的唯一名称</param>
        /// <returns>Feature's current value / 功能的当前值</returns>
        public static string GetValue(this IFeatureChecker featureChecker, string featureName)
        {
            return AsyncHelper.RunSync(() => featureChecker.GetValueAsync(featureName));
        }

        /// <summary>
        /// Gets value of a feature by it's name. This is sync version of <see cref="IFeatureChecker.GetValueAsync(int, string)"/>
        /// 根据名称获取功能值，这是<see cref="IFeatureChecker.GetValueAsync(int, string)"/>的异步版本
        /// </summary>
        /// <param name="featureChecker"><see cref="IFeatureChecker"/> instance  / <see cref="IFeatureChecker"/>实例</param>
        /// <param name="tenantId">Tenant's Id / 租户ID</param>
        /// <param name="featureName">Unique feature name / 功能的唯一名称</param>
        /// <returns>Feature's current value / 功能的当前值</returns>
        public static string GetValue(this IFeatureChecker featureChecker, int tenantId, string featureName)
        {
            return AsyncHelper.RunSync(() => featureChecker.GetValueAsync(tenantId, featureName));
        }

        /// <summary>
        /// Checks if given feature is enabled.This should be used for boolean-value features.
        /// 检查给定的功能是否可用，这应为bool值的功能使用
        /// This is a shortcut for <see cref="IsEnabledAsync(IFeatureChecker, int, string)"/> that uses <see cref="IAbpSession.TenantId"/> as tenantId.
        /// So, this method should be used only if TenantId can be obtained from the session.
        /// <see cref="GetValue(IFeatureChecker, int, string)"/>用<see cref="IAbpSession.TenantId"/>作为租户ID的快捷方式，所以此方法应该被使用为那些可以从session获取租户ID值的
        /// </summary>
        /// <param name="featureChecker"><see cref="IFeatureChecker"/> instance / <see cref="IFeatureChecker"/>实例</param>
        /// <param name="featureName">Unique feature name / 功能的唯一名称</param>
        /// <returns>True, if current feature's value is "true". / true,如果功能的值是true</returns>
        public static async Task<bool> IsEnabledAsync(this IFeatureChecker featureChecker, string featureName)
        {
            return string.Equals(await featureChecker.GetValueAsync(featureName), "true", StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Checks if given feature is enabled.This should be used for boolean-value features.
        /// 检查给定的功能是否可用，这应为bool值的功能使用
        /// </summary>
        /// <param name="featureChecker"><see cref="IFeatureChecker"/> instance / <see cref="IFeatureChecker"/>实例</param>
        /// <param name="tenantId">Tenant's Id / 租户ID</param>
        /// <param name="featureName">Unique feature name / 功能的唯一名称</param>
        /// <returns>True, if current feature's value is "true". / true,如果功能的值是true</returns>
        public static async Task<bool> IsEnabledAsync(this IFeatureChecker featureChecker, int tenantId, string featureName)
        {
            return string.Equals(await featureChecker.GetValueAsync(tenantId, featureName), "true", StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Checks if given feature is enabled.This should be used for boolean-value features.
        /// 检查给定的功能是否可用，这应为bool值的功能使用
        /// This is a shortcut for <see cref="IsEnabled(IFeatureChecker, int, string)"/> that uses <see cref="IAbpSession.TenantId"/> as tenantId.
        /// So, this method should be used only if TenantId can be obtained from the session.
        /// <see cref="GetValue(IFeatureChecker, int, string)"/>用<see cref="IAbpSession.TenantId"/>作为租户ID的快捷方式，所以此方法应该被使用为那些可以从session获取租户ID值的
        /// </summary>
        /// <param name="featureChecker"><see cref="IFeatureChecker"/> instance / <see cref="IFeatureChecker"/>实例</param>
        /// <param name="name">Unique feature name / 功能的唯一名称</param>
        /// <returns>True, if current feature's value is "true". / true,如果功能的值是true</returns>
        public static bool IsEnabled(this IFeatureChecker featureChecker, string name)
        {
            return AsyncHelper.RunSync(() => featureChecker.IsEnabledAsync(name));
        }

        /// <summary>
        /// Checks if given feature is enabled.This should be used for boolean-value features.
        /// 检查给定的功能是否可用，这应为bool值的功能使用
        /// </summary>
        /// <param name="featureChecker"><see cref="IFeatureChecker"/> instance / <see cref="IFeatureChecker"/>实例</param>
        /// <param name="tenantId">Tenant's Id / 租户ID</param>
        /// <param name="featureName">Unique feature name / 功能的唯一名称</param>
        /// <returns>True, if current feature's value is "true". / true,如果功能的值是true</returns>
        public static bool IsEnabled(this IFeatureChecker featureChecker, int tenantId, string featureName)
        {
            return AsyncHelper.RunSync(() => featureChecker.IsEnabledAsync(tenantId, featureName));
        }

        /// <summary>
        /// Used to check if one of all given features are enabled.
        /// 检查给定的功能是否可用
        /// </summary>
        /// <param name="featureChecker"><see cref="IFeatureChecker"/> instance / <see cref="IFeatureChecker"/>实例</param>
        /// <param name="requiresAll">True, to require all given features are enabled. False, to require one or more. / true,所有给定的功能都必须可用，false，一个或多个可用即可</param>
        /// <param name="featureNames">Name of the features / 功能名称</param>
        public static async Task<bool> IsEnabledAsync(this IFeatureChecker featureChecker, bool requiresAll, params string[] featureNames)
        {
            if (featureNames.IsNullOrEmpty())
            {
                return true;
            }

            if (requiresAll)
            {
                foreach (var featureName in featureNames)
                {
                    if (!(await featureChecker.IsEnabledAsync(featureName)))
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                foreach (var featureName in featureNames)
                {
                    if (await featureChecker.IsEnabledAsync(featureName))
                    {
                        return true;
                    }
                }

                return false;
            }
        }


        /// <summary>
        /// Used to check if one of all given features are enabled.
        /// 检查给定的功能是否可用
        /// </summary>
        /// <param name="featureChecker"><see cref="IFeatureChecker"/> instance / <see cref="IFeatureChecker"/>实例</param>
        /// <param name="tenantId">Tenant id / 租户ID</param>
        /// <param name="requiresAll">True, to require all given features are enabled. False, to require one or more. / true,所有给定的功能都必须可用，false，一个或多个可用即可</param>
        /// <param name="featureNames">Name of the features / 功能名称</param>
        public static async Task<bool> IsEnabledAsync(this IFeatureChecker featureChecker, int tenantId, bool requiresAll, params string[] featureNames)
        {
            if (featureNames.IsNullOrEmpty())
            {
                return true;
            }

            if (requiresAll)
            {
                foreach (var featureName in featureNames)
                {
                    if (!(await featureChecker.IsEnabledAsync(tenantId, featureName)))
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                foreach (var featureName in featureNames)
                {
                    if (await featureChecker.IsEnabledAsync(tenantId, featureName))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Used to check if one of all given features are enabled.
        /// 检查给定的功能是否可用
        /// </summary>
        /// <param name="featureChecker"><see cref="IFeatureChecker"/> instance / <see cref="IFeatureChecker"/>实例</param>
        /// <param name="requiresAll">True, to require all given features are enabled. False, to require one or more. / true,所有给定的功能都必须可用，false，一个或多个可用即可</param>
        /// <param name="featureNames">Name of the features / 功能名称</param>
        public static bool IsEnabled(this IFeatureChecker featureChecker, bool requiresAll, params string[] featureNames)
        {
            return AsyncHelper.RunSync(() => featureChecker.IsEnabledAsync(requiresAll, featureNames));
        }

        /// <summary>
        /// Used to check if one of all given features are enabled.
        /// 检查给定的功能是否可用
        /// </summary>
        /// <param name="featureChecker"><see cref="IFeatureChecker"/> instance / <see cref="IFeatureChecker"/>实例</param>
        /// <param name="tenantId">Tenant id / 租户ID</param>
        /// <param name="requiresAll">True, to require all given features are enabled. False, to require one or more. / true,所有给定的功能都必须可用，false，一个或多个可用即可</param>
        /// <param name="featureNames">Name of the features / 功能名称</param>
        public static bool IsEnabled(this IFeatureChecker featureChecker, int tenantId, bool requiresAll, params string[] featureNames)
        {
            return AsyncHelper.RunSync(() => featureChecker.IsEnabledAsync(tenantId, requiresAll, featureNames));
        }

        /// <summary>
        /// Checks if given feature is enabled. Throws <see cref="AbpAuthorizationException"/> if not.
        /// 检查给定的功能是否可用，如果不可用抛出<see cref="AbpAuthorizationException"/>
        /// </summary>
        /// <param name="featureChecker"><see cref="IFeatureChecker"/> instance / <see cref="IFeatureChecker"/>实例</param>
        /// <param name="featureName">Unique feature name / 功能名称</param>
        public static async Task CheckEnabledAsync(this IFeatureChecker featureChecker, string featureName)
        {
            if (!(await featureChecker.IsEnabledAsync(featureName)))
            {
                throw new AbpAuthorizationException("Feature is not enabled: " + featureName);
            }
        }

        /// <summary>
        /// Checks if given feature is enabled. Throws <see cref="AbpAuthorizationException"/> if not.
        /// 检查给定的功能是否可用，如果不可用抛出<see cref="AbpAuthorizationException"/>
        /// </summary>
        /// <param name="featureChecker"><see cref="IFeatureChecker"/> instance / <see cref="IFeatureChecker"/>实例</param>
        /// <param name="featureName">Unique feature name / 功能名称</param>
        public static void CheckEnabled(this IFeatureChecker featureChecker, string featureName)
        {
            if (!featureChecker.IsEnabled(featureName))
            {
                throw new AbpAuthorizationException("Feature is not enabled: " + featureName);
            }
        }

        /// <summary>
        /// Checks if one of all given features are enabled. Throws <see cref="AbpAuthorizationException"/> if not.
        /// 检查给定所有功能中一个是否可用，如果不可用抛出<see cref="AbpAuthorizationException"/>
        /// </summary>
        /// <param name="featureChecker"><see cref="IFeatureChecker"/> instance / <see cref="IFeatureChecker"/>实例</param>
        /// <param name="requiresAll">True, to require all given features are enabled. False, to require one or more. / true,所有给定的功能都必须可用，false，一个或多个可用即可</param>
        /// <param name="featureNames">Name of the features / 功能名称</param>
        public static async Task CheckEnabledAsync(this IFeatureChecker featureChecker, bool requiresAll, params string[] featureNames)
        {
            if (featureNames.IsNullOrEmpty())
            {
                return;
            }

            if (requiresAll)
            {
                foreach (var featureName in featureNames)
                {
                    if (!(await featureChecker.IsEnabledAsync(featureName)))
                    {
                        throw new AbpAuthorizationException(
                            "Required features are not enabled. All of these features must be enabled: " +
                            string.Join(", ", featureNames)
                            );
                    }
                }
            }
            else
            {
                foreach (var featureName in featureNames)
                {
                    if (await featureChecker.IsEnabledAsync(featureName))
                    {
                        return;
                    }
                }

                throw new AbpAuthorizationException(
                    "Required features are not enabled. At least one of these features must be enabled: " +
                    string.Join(", ", featureNames)
                    );
            }
        }

        /// <summary>
        /// Checks if one of all given features are enabled. Throws <see cref="AbpAuthorizationException"/> if not.
        /// 检查给定所有功能中一个是否可用，如果不可用抛出<see cref="AbpAuthorizationException"/>
        /// </summary>
        /// <param name="featureChecker"><see cref="IFeatureChecker"/> instance / <see cref="IFeatureChecker"/>实例</param>
        /// <param name="tenantId">Tenant id / 租户ID</param>
        /// <param name="requiresAll">True, to require all given features are enabled. False, to require one or more. / true,所有给定的功能都必须可用，false，一个或多个可用即可</param>
        /// <param name="featureNames">Name of the features / 功能名称</param>
        public static async Task CheckEnabledAsync(this IFeatureChecker featureChecker, int tenantId, bool requiresAll, params string[] featureNames)
        {
            if (featureNames.IsNullOrEmpty())
            {
                return;
            }

            if (requiresAll)
            {
                foreach (var featureName in featureNames)
                {
                    if (!(await featureChecker.IsEnabledAsync(tenantId, featureName)))
                    {
                        throw new AbpAuthorizationException(
                            "Required features are not enabled. All of these features must be enabled: " +
                            string.Join(", ", featureNames)
                            );
                    }
                }
            }
            else
            {
                foreach (var featureName in featureNames)
                {
                    if (await featureChecker.IsEnabledAsync(tenantId, featureName))
                    {
                        return;
                    }
                }

                throw new AbpAuthorizationException(
                    "Required features are not enabled. At least one of these features must be enabled: " +
                    string.Join(", ", featureNames)
                    );
            }
        }

        /// <summary>
        /// Checks if one of all given features are enabled. Throws <see cref="AbpAuthorizationException"/> if not.
        /// 检查给定所有功能中一个是否可用，如果不可用抛出<see cref="AbpAuthorizationException"/>
        /// </summary>
        /// <param name="featureChecker"><see cref="IFeatureChecker"/> instance / <see cref="IFeatureChecker"/>实例</param>
        /// <param name="requiresAll">True, to require all given features are enabled. False, to require one or more. / true,所有给定的功能都必须可用，false，一个或多个可用即可</param>
        /// <param name="featureNames">Name of the features / 功能名称</param>
        public static void CheckEnabled(this IFeatureChecker featureChecker, bool requiresAll, params string[] featureNames)
        {
            AsyncHelper.RunSync(() => featureChecker.CheckEnabledAsync(requiresAll, featureNames));
        }

        /// <summary>
        /// Checks if one of all given features are enabled. Throws <see cref="AbpAuthorizationException"/> if not.
        /// 检查给定所有功能中一个是否可用，如果不可用抛出<see cref="AbpAuthorizationException"/>
        /// </summary>
        /// <param name="featureChecker"><see cref="IFeatureChecker"/> instance / <see cref="IFeatureChecker"/>实例</param>
        /// <param name="tenantId">Tenant id / 租户ID</param>
        /// <param name="requiresAll">True, to require all given features are enabled. False, to require one or more. / true,所有给定的功能都必须可用，false，一个或多个可用即可</param>
        /// <param name="featureNames">Name of the features / 功能名称</param>
        public static void CheckEnabled(this IFeatureChecker featureChecker, int tenantId, bool requiresAll, params string[] featureNames)
        {
            AsyncHelper.RunSync(() => featureChecker.CheckEnabledAsync(tenantId, requiresAll, featureNames));
        }
    }
}