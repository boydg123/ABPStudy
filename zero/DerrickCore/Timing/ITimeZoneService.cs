using System.Threading.Tasks;
using Abp.Configuration;

namespace Derrick.Timing
{
    /// <summary>
    /// 时区服务
    /// </summary>
    public interface ITimeZoneService
    {
        /// <summary>
        /// 获取默认时区
        /// </summary>
        /// <param name="scope">设置作用域</param>
        /// <param name="tenantId">商户ID</param>
        /// <returns></returns>
        Task<string> GetDefaultTimezoneAsync(SettingScopes scope, int? tenantId);
    }
}
