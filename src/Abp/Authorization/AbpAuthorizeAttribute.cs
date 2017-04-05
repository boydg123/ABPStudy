using System;
using Abp.Application.Services;

namespace Abp.Authorization
{
    /// <summary>
    /// This attribute is used on a method of an Application Service (A class that implements <see cref="IApplicationService"/>) to make that method usable only by authorized users.
    /// 此特性用于应用服务（实现接口<see cref="IApplicationService"/>的类）的方法，使用方法只有授权用户才能访问
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AbpAuthorizeAttribute : Attribute, IAbpAuthorizeAttribute
    {
        /// <summary>
        /// A list of permissions to authorize.
        /// 需要授权验证的权限列表
        /// </summary>
        public string[] Permissions { get; }

        /// <summary>
        /// If this property is set to true, all of the <see cref="Permissions"/> must be granted.If it's false, at least one of the <see cref="Permissions"/> must be granted.
        /// 如果这个属性设置为true，<see cref="Permissions"/>中所有的权限都必须授予。如果这个属性设置为false，<see cref="Permissions"/>中只需有一个权限被授予便可。
        /// Default: false.
        /// </summary>
        public bool RequireAllPermissions { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="AbpAuthorizeAttribute"/> class.
        /// 创建一个新的 <see cref="AbpAuthorizeAttribute"/> 实例.
        /// </summary>
        /// <param name="permissions">A list of permissions to authorize / 授权权限列表</param>
        public AbpAuthorizeAttribute(params string[] permissions)
        {
            Permissions = permissions;
        }
    }
}
