using System;

namespace Abp.Web.Security.AntiForgery
{
    /// <summary>
    /// ABP防伪令牌验证标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method)]
    public class ValidateAbpAntiForgeryTokenAttribute : Attribute
    {

    }
}