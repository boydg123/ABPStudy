using System.Threading.Tasks;
using Abp.Application.Services;
using Derrick.Configuration.Host.Dto;

namespace Derrick.Configuration.Host
{
    /// <summary>
    /// 宿主设置服务
    /// </summary>
    public interface IHostSettingsAppService : IApplicationService
    {
        /// <summary>
        /// 获取所有设置
        /// </summary>
        /// <returns></returns>
        Task<HostSettingsEditDto> GetAllSettings();
        /// <summary>
        /// 更新所有设置
        /// </summary>
        /// <param name="input">宿主设置编辑Dto</param>
        /// <returns></returns>
        Task UpdateAllSettings(HostSettingsEditDto input);
        /// <summary>
        /// 发送测试邮件
        /// </summary>
        /// <param name="input">发送测试邮件Input</param>
        /// <returns></returns>
        Task SendTestEmail(SendTestEmailInput input);
    }
}
