using System;

namespace Abp.Authorization
{
    /// <summary>
    /// Used to allow a method to be accessed by any user.
    /// 用于允许任何用户访问的方法
    /// Suppress <see cref="AbpAuthorizeAttribute"/> defined in the class containing that method.
    /// 抑制含有授权方法的类<see cref="AbpAuthorizeAttribute"/>的定义
    /// </summary>
    public class AbpAllowAnonymousAttribute : Attribute, IAbpAllowAnonymousAttribute
    {

    }
}