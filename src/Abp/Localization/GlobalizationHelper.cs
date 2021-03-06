﻿using System.Globalization;
using Abp.Extensions;

namespace Abp.Localization
{
    /// <summary>
    /// 全球化帮助类
    /// </summary>
    public static class GlobalizationHelper
    {
        /// <summary>
        /// 是否为有效的区域代码
        /// </summary>
        /// <param name="cultureCode">区域码</param>
        /// <returns></returns>
        public static bool IsValidCultureCode(string cultureCode)
        {
            if (cultureCode.IsNullOrWhiteSpace())
            {
                return false;
            }

            try
            {
                CultureInfo.GetCultureInfo(cultureCode);
                return true;
            }
            catch (CultureNotFoundException)
            {
                return false;
            }
        }
    }
}
