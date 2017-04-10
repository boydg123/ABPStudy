using System;

namespace Abp.Web.Security.AntiForgery
{
    /// <summary>
    /// 验证ABP防伪令牌标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method)]
    public class ValidateAbpAntiForgeryTokenAttribute : Attribute
    {

    }
}