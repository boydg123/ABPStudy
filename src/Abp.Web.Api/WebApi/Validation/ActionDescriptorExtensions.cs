using System.Reflection;
using System.Web.Http.Controllers;
using Abp.Extensions;

namespace Abp.WebApi.Validation
{
    /// <summary>
    /// Action描述扩展
    /// </summary>
    public static class ActionDescriptorExtensions
    {
        /// <summary>
        /// 获取方法信息(没有获取到返回Null)
        /// </summary>
        /// <param name="actionDescriptor"></param>
        /// <returns></returns>
        public static MethodInfo GetMethodInfoOrNull(this HttpActionDescriptor actionDescriptor)
        {
            if (actionDescriptor is ReflectedHttpActionDescriptor)
            {
                return actionDescriptor.As<ReflectedHttpActionDescriptor>().MethodInfo;
            }
            
            return null;
        }

        /// <summary>
        /// 是否是动态Abp Action
        /// </summary>
        /// <param name="actionDescriptor"></param>
        /// <returns></returns>
        public static bool IsDynamicAbpAction(this HttpActionDescriptor actionDescriptor)
        {
            return actionDescriptor
                .ControllerDescriptor
                .Properties
                .ContainsKey("__AbpDynamicApiControllerInfo");
        }
    }
}