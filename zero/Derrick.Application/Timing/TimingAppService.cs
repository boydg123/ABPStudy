using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Configuration;
using Derrick.Timing.Dto;

namespace Derrick.Timing
{
    /// <summary>
    /// 定时服务实现
    /// </summary>
    public class TimingAppService : AbpZeroTemplateAppServiceBase, ITimingAppService
    {
        /// <summary>
        /// 时区服务
        /// </summary>
        private readonly ITimeZoneService _timeZoneService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="timeZoneService">时区服务</param>
        public TimingAppService(ITimeZoneService timeZoneService)
        {
            _timeZoneService = timeZoneService;
        }
        /// <summary>
        /// 获取时区
        /// </summary>
        /// <param name="input">获取时区Input</param>
        /// <returns></returns>
        public async Task<ListResultDto<NameValueDto>> GetTimezones(GetTimezonesInput input)
        {
            var timeZones = await GetTimezoneInfos(input.DefaultTimezoneScope);
            return new ListResultDto<NameValueDto>(timeZones);
        }
        /// <summary>
        /// 获取时区Combobox项
        /// </summary>
        /// <param name="input">获取时区Combobox项Input</param>
        /// <returns></returns>
        public async Task<List<ComboboxItemDto>> GetTimezoneComboboxItems(GetTimezoneComboboxItemsInput input)
        {
            var timeZones = await GetTimezoneInfos(input.DefaultTimezoneScope);
            var timeZoneItems = new ListResultDto<ComboboxItemDto>(timeZones.Select(e => new ComboboxItemDto(e.Value, e.Name)).ToList()).Items.ToList();

            if (!string.IsNullOrEmpty(input.SelectedTimezoneId))
            {
                var selectedEdition = timeZoneItems.FirstOrDefault(e => e.Value == input.SelectedTimezoneId);
                if (selectedEdition != null)
                {
                    selectedEdition.IsSelected = true;
                }
            }

            return timeZoneItems;
        }

        private async Task<List<NameValueDto>> GetTimezoneInfos(SettingScopes defaultTimezoneScope)
        {
            var defaultTimezoneId = await _timeZoneService.GetDefaultTimezoneAsync(defaultTimezoneScope, AbpSession.TenantId);
            var defaultTimezone = TimeZoneInfo.FindSystemTimeZoneById(defaultTimezoneId);
            var defaultTimezoneName = string.Format("{0} [{1}]", L("Default"), defaultTimezone.DisplayName);

            var timeZones = TimeZoneInfo.GetSystemTimeZones()
                                        .Select(tz => new NameValueDto(tz.DisplayName, tz.Id))
                                        .ToList();

            timeZones.Insert(0, new NameValueDto(defaultTimezoneName, string.Empty));
            return timeZones;
        }
    }
}