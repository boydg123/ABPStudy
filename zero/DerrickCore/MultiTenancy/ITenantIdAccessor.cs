using Abp.Runtime.Session;

namespace Derrick.MultiTenancy
{
    /// <summary>
    /// Used to get current tenant id where <see cref="IAbpSession"/> is not usable.
    /// 当<see cref="IAbpSession"/>是不可用时，用来获取当前商户的ID
    /// </summary>
    public interface ITenantIdAccessor
    {
        /// <summary>
        /// Gets current tenant id.Use <see cref="IAbpSession.TenantId"/> wherever possible (if user logged in).
        /// 获取当前商户ID，在任何可能的地方使用<see cref="IAbpSession.TenantId"/>(如果用户已登录)
        /// This method tries to get current tenant id even if current user did not log in.
        /// 这个方法是当用户没有登录的时候用来尝试获取当前商户的ID
        /// </summary>
        /// <param name="useSession">Set false to skip session usage</param>
        int? GetCurrentTenantIdOrNull(bool useSession = true);
    }
}