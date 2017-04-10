using System;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Extensions;
using Abp.Timing;
using Abp.Timing.Timezone;

namespace Abp.Web.Timing
{
    /// <summary>
    /// This class is used to build timing script.
    /// 此类用于生成时间脚本
    /// </summary>
    public class TimingScriptManager : ITimingScriptManager, ITransientDependency
    {
        /// <summary>
        /// 设置管理器
        /// </summary>
        private readonly ISettingManager _settingManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="settingManager">设置管理器</param>
        public TimingScriptManager(ISettingManager settingManager)
        {
            _settingManager = settingManager;
        }

        /// <summary>
        /// 获取包含所有功能信息的Javascript脚本
        /// </summary>
        public async Task<string> GetScriptAsync()
        {
            var script = new StringBuilder();

            script.AppendLine("(function(){");

            script.AppendLine("    abp.clock.provider = abp.timing." + Clock.Provider.GetType().Name.ToCamelCase() + " || abp.timing.localClockProvider;");
            script.AppendLine("    abp.clock.provider.supportsMultipleTimezone = " + Clock.SupportsMultipleTimezone.ToString().ToLower(CultureInfo.InvariantCulture) + ";");

            if (Clock.SupportsMultipleTimezone)
            {
                script.AppendLine("    abp.timing.timeZoneInfo = " + await GetUsersTimezoneScriptsAsync());
            }

            script.Append("})();");

            return script.ToString();
        }

        /// <summary>
        /// 获取用户时区的脚本
        /// </summary>
        private async Task<string> GetUsersTimezoneScriptsAsync()
        {
            var timezoneId = await _settingManager.GetSettingValueAsync(TimingSettingNames.TimeZone);
            var timezone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);

            return " {" +
                   "        windows: {" +
                   "            timeZoneId: '" + timezoneId + "'," +
                   "            baseUtcOffsetInMilliseconds: '" + timezone.BaseUtcOffset.TotalMilliseconds + "'," +
                   "            currentUtcOffsetInMilliseconds: '" + timezone.GetUtcOffset(Clock.Now).TotalMilliseconds + "'," +
                   "            isDaylightSavingTimeNow: '" + timezone.IsDaylightSavingTime(Clock.Now) + "'" +
                   "        }," +
                   "        iana: {" +
                   "            timeZoneId:'" + TimezoneHelper.WindowsToIana(timezoneId) + "'" +
                   "        }," +
                   "    }";
        }
    }
}