using System.Text.RegularExpressions;
using Abp.Extensions;

namespace Derrick.Security
{
    /// <summary>
    /// 密码复杂度检查器
    /// </summary>
    public class PasswordComplexityChecker
    {
        /// <summary>
        /// 检查密码
        /// </summary>
        /// <param name="setting">密码复杂度设置信息</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public bool Check(PasswordComplexitySetting setting, string password)
        {
            if (password.IsNullOrEmpty())
            {
                return false;
            }

            if (password.Length < setting.MinLength)
            {
                return false;
            }

            if (password.Length > setting.MaxLength)
            {
                return false;
            }

            if (setting.UseUpperCaseLetters)
            {
                var useUpperCaseLettersRegex = new Regex("[A-Z]");
                if (!useUpperCaseLettersRegex.IsMatch(password))
                {
                    return false;
                }
            }

            if (setting.UseLowerCaseLetters)
            {
                var useLowerCaseLettersRegex = new Regex("[a-z]");
                if (!useLowerCaseLettersRegex.IsMatch(password))
                {
                    return false;
                }
            }

            if (setting.UseNumbers)
            {
                var useNumbersRegex = new Regex("[0-9]");
                if (!useNumbersRegex.IsMatch(password))
                {
                    return false;
                }
            }

            if (setting.UsePunctuations)
            {
                var usePunctuations = new Regex(@"[\p{P}\p{S}]");
                if (!usePunctuations.IsMatch(password))
                {
                    return false;
                }
            }


            return true;
        }
    }
}
