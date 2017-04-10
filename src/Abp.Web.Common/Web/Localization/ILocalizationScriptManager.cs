using System.Globalization;

namespace Abp.Web.Localization
{
    /// <summary>
    /// Define interface to get localization javascript.
    /// 定义获取本地化Javascript脚本的接口
    /// </summary>
    public interface ILocalizationScriptManager
    {
        /// <summary>
        /// Gets Javascript that contains all localization information in current culture.
        /// 获取包含当前文化中的所有本地化信息的JavaScript。
        /// </summary>
        string GetScript();

        /// <summary>
        /// Gets Javascript that contains all localization information in given culture.
        /// 获取在给定的文化中包含所有本地化信息的JavaScript。
        /// </summary>
        /// <param name="cultureInfo">Culture to get script / 获取脚本的文化信息</param>
        string GetScript(CultureInfo cultureInfo);
    }
}