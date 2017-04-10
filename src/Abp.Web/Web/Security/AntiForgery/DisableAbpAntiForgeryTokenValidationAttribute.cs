using System;

namespace Abp.Web.Security.AntiForgery
{
    /// <summary>
    /// 禁用ABP防伪令牌验证标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method)]
    public class DisableAbpAntiForgeryTokenValidationAttribute : Attribute
    {

    }
}
