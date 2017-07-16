using System.Text.RegularExpressions;
using Abp.Extensions;

namespace Derrick.Validation
{
    /// <summary>
    /// 验证帮助类
    /// </summary>
    public class ValidationHelper
    {
        /// <summary>
        /// 邮件正则
        /// </summary>
        public const string EmailRegex = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

        /// <summary>
        /// 是否是邮件
        /// </summary>
        /// <param name="value">验证的值</param>
        /// <returns></returns>
        public bool IsEmail(string value)
        {
            if (value.IsNullOrEmpty())
            {
                return false;
            }

            var regex = new Regex(EmailRegex);
            return regex.IsMatch(value);
        }
    }
}
