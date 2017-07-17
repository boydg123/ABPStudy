using System.Threading.Tasks;
using Abp.Application.Services;
using Derrick.Configuration.Tenants.Dto;

namespace Derrick.Configuration.Tenants
{
    /// <summary>
    /// 商户设置服务
    /// </summary>
    public interface ITenantSettingsAppService : IApplicationService
    {
        /// <summary>
        /// 获取所有商户设置
        /// </summary>
        /// <returns></returns>
        Task<TenantSettingsEditDto> GetAllSettings();
        /// <summary>
        /// 更新所有商户设置
        /// </summary>
        /// <param name="input">商户设置编辑Dto</param>
        /// <returns></returns>
        Task UpdateAllSettings(TenantSettingsEditDto input);
    }
}
