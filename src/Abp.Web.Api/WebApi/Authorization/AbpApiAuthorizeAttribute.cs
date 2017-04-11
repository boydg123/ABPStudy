using System;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Abp.Authorization;

namespace Abp.WebApi.Authorization
{
    /// <summary>
    /// This attribute is used on a method of an <see cref="ApiController"/>,to make that method usable only by authorized users.
    /// 此特性用于在<see cref="ApiController"/>的方法，为了使方法被授权用户使用
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AbpApiAuthorizeAttribute : AuthorizeAttribute, IAbpAuthorizeAttribute
    {
        /// <summary>
        /// 需要授权验证的权限列表
        /// </summary>
        public string[] Permissions { get; set; }

        /// <summary>
        /// 如果这个属性设置为true，<see cref="Permissions"/>中所有的权限都必须授予。
        /// 如果这个属性设置为false，<see cref="Permissions"/>中只需有一个权限被授予便可。
        /// Default：false
        /// </summary>
        public bool RequireAllPermissions { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="AbpApiAuthorizeAttribute"/> class.
        /// 构造函数
        /// </summary>
        /// <param name="permissions">A list of permissions to authorize / 需要授权验证的权限列表</param>
        public AbpApiAuthorizeAttribute(params string[] permissions)
        {
            Permissions = permissions;
        }

        /// <summary>
        /// 处理没有授权的请求
        /// </summary>
        /// <param name="actionContext">HttpAction上下文</param>
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            var httpContext = HttpContext.Current;
            if (httpContext == null)
            {
                base.HandleUnauthorizedRequest(actionContext);
                return;
            }

            httpContext.Response.StatusCode = httpContext.User.Identity.IsAuthenticated == false
                                      ? (int)System.Net.HttpStatusCode.Unauthorized
                                      : (int)System.Net.HttpStatusCode.Forbidden;

            httpContext.Response.SuppressFormsAuthenticationRedirect = true;
            httpContext.Response.End();
        }
    }
}
