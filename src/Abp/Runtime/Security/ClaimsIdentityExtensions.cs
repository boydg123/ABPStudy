using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Abp.Extensions;
using JetBrains.Annotations;

namespace Abp.Runtime.Security
{
    /// <summary>
    /// <see cref="ClaimsIdentity"/>的扩展
    /// </summary>
    public static class ClaimsIdentityExtensions
    {
        /// <summary>
        /// 获取用户标识
        /// </summary>
        /// <param name="identity">标识对象</param>
        /// <returns>用户标识，没找到返回Null</returns>
        public static UserIdentifier GetUserIdentifierOrNull(this IIdentity identity)
        {
            Check.NotNull(identity, nameof(identity));

            var userId = identity.GetUserId();
            if (userId == null)
            {
                return null;
            }

            return new UserIdentifier(identity.GetTenantId(), userId.Value);
        }

        /// <summary>
        /// 获取用户ID
        /// </summary>
        /// <param name="identity">标识对象</param>
        /// <returns></returns>
        public static long? GetUserId([NotNull] this IIdentity identity)
        {
            Check.NotNull(identity, nameof(identity));

            var claimsIdentity = identity as ClaimsIdentity;

            var userIdOrNull = claimsIdentity?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdOrNull == null || userIdOrNull.Value.IsNullOrWhiteSpace())
            {
                return null;
            }

            return Convert.ToInt64(userIdOrNull.Value);
        }

        /// <summary>
        /// 基于标识信息获取租户ID
        /// </summary>
        /// <param name="identity">标识信息</param>
        /// <returns></returns>
        public static int? GetTenantId(this IIdentity identity)
        {
            Check.NotNull(identity, nameof(identity));

            var claimsIdentity = identity as ClaimsIdentity;

            var tenantIdOrNull = claimsIdentity?.Claims?.FirstOrDefault(c => c.Type == AbpClaimTypes.TenantId);
            if (tenantIdOrNull == null || tenantIdOrNull.Value.IsNullOrWhiteSpace())
            {
                return null;
            }

            return Convert.ToInt32(tenantIdOrNull.Value);
        }

        /// <summary>
        /// 获取模拟用户ID
        /// </summary>
        /// <param name="identity">标识信息</param>
        /// <returns></returns>
        public static long? GetImpersonatorUserId(this IIdentity identity)
        {
            Check.NotNull(identity, nameof(identity));

            var claimsIdentity = identity as ClaimsIdentity;

            var userIdOrNull = claimsIdentity?.Claims?.FirstOrDefault(c => c.Type == AbpClaimTypes.ImpersonatorUserId);
            if (userIdOrNull == null || userIdOrNull.Value.IsNullOrWhiteSpace())
            {
                return null;
            }

            return Convert.ToInt64(userIdOrNull.Value);
        }

        /// <summary>
        /// 获取模拟租户ID
        /// </summary>
        /// <param name="identity">标识信息</param>
        /// <returns></returns>
        public static int? GetImpersonatorTenantId(this IIdentity identity)
        {
            Check.NotNull(identity, nameof(identity));

            var claimsIdentity = identity as ClaimsIdentity;

            var tenantIdOrNull = claimsIdentity?.Claims?.FirstOrDefault(c => c.Type == AbpClaimTypes.ImpersonatorTenantId);
            if (tenantIdOrNull == null || tenantIdOrNull.Value.IsNullOrWhiteSpace())
            {
                return null;
            }

            return Convert.ToInt32(tenantIdOrNull.Value);
        }
    }
}