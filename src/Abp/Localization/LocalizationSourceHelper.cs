using System.Globalization;
using System.Text.RegularExpressions;
using Abp.Configuration.Startup;
using Abp.Extensions;
using Abp.Logging;

namespace Abp.Localization
{
    /// <summary>
    /// 本地化源帮助累
    /// </summary>
    public static class LocalizationSourceHelper
    {
        /// <summary>
        /// 返回指定名称或抛出异常
        /// </summary>
        /// <param name="configuration">本地化配置</param>
        /// <param name="sourceName">源名称</param>
        /// <param name="name">名称</param>
        /// <param name="culture">区域信息</param>
        /// <returns></returns>
        public static string ReturnGivenNameOrThrowException(ILocalizationConfiguration configuration, string sourceName, string name, CultureInfo culture)
        {
            var exceptionMessage = string.Format(
                "Can not find '{0}' in localization source '{1}'!",
                name, sourceName
                );

            if (!configuration.ReturnGivenTextIfNotFound)
            {
                throw new AbpException(exceptionMessage);
            }

            LogHelper.Logger.Warn(exceptionMessage);

            var notFoundText = configuration.HumanizeTextIfNotFound
                ? name.ToSentenceCase(culture)
                : name;

            return configuration.WrapGivenTextIfNotFound
                ? string.Format("[{0}]", notFoundText)
                : notFoundText;
        }
    }
}
