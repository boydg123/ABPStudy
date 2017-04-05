using System.Threading.Tasks;
using Abp.Runtime.Session;

namespace Abp.Application.Features
{
    /// <summary>
    /// This interface should be used to get value of
    /// 此接口用于获取值(功能检查器?)
    /// </summary>
    public interface IFeatureChecker
    {
        /// <summary>
        /// Gets value of a feature by it's name.
        /// 通过功能名称获取值
        /// This is a shortcut for <see cref="GetValueAsync(int, string)"/> that uses <see cref="IAbpSession.TenantId"/> as tenantId.
        /// 使用<see cref="IAbpSession.TenantId"/>
        /// So, this method should be used only if TenantId can be obtained from the session.
        /// 所以，只有从Session中获取租户ID才能使用此方法
        /// </summary>
        /// <param name="name">Unique feature name / 功能的唯一名称</param>
        /// <returns>Feature's current value / 功能的当前值</returns>
        Task<string> GetValueAsync(string name);

        /// <summary>
        /// Gets value of a feature for a tenant by the feature name.
        /// 为一个租户的功能名称获取功能值
        /// </summary>
        /// <param name="tenantId">Tenant's Id / 租户的ID</param>
        /// <param name="name">Unique feature name / 功能的唯一名称</param>
        /// <returns>Feature's current value / 功能的当前值</returns>
        Task<string> GetValueAsync(int tenantId, string name);
    }
}