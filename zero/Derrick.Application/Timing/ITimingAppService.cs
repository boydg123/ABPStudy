using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Derrick.Timing.Dto;

namespace Derrick.Timing
{
    /// <summary>
    /// 定时服务
    /// </summary>
    public interface ITimingAppService : IApplicationService
    {
        /// <summary>
        /// 获取时区
        /// </summary>
        /// <param name="input">获取时区Input</param>
        /// <returns></returns>
        Task<ListResultDto<NameValueDto>> GetTimezones(GetTimezonesInput input);
        /// <summary>
        /// 获取时区Combobox项
        /// </summary>
        /// <param name="input">获取时区Combobox项Input</param>
        /// <returns></returns>
        Task<List<ComboboxItemDto>> GetTimezoneComboboxItems(GetTimezoneComboboxItemsInput input);
    }
}
