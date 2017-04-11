using System.Reflection;
using System.Web.Http.Filters;
using Abp.Web;

namespace Abp.WebApi.Controllers.Dynamic
{
    /// <summary>
    /// Used to store an action information of a dynamic ApiController.
    /// 用于存储动态API控制器中的Action信息
    /// </summary>
    public class DynamicApiActionInfo
    {
        /// <summary>
        /// Name of the action in the controller.
        /// 控制器中Action的名称
        /// </summary>
        public string ActionName { get; private set; }

        /// <summary>
        /// The method which will be invoked when this action is called.
        /// 调用此操作时将调用的方法
        /// </summary>
        public MethodInfo Method { get; private set; }

        /// <summary>
        /// The HTTP verb that is used to call this action.
        /// 调用此方法的Http操作
        /// </summary>
        public HttpVerb Verb { get; private set; }

        /// <summary>
        /// Dynamic Action Filters for this Controller Action.
        /// 此控制器Action的动态Action过滤器
        /// </summary>
        public IFilter[] Filters { get; set; }

        /// <summary>
        /// Is API Explorer enabled.
        /// 是否启用API预览
        /// </summary>
        public bool? IsApiExplorerEnabled { get; set; }

        /// <summary>
        /// Createa a new <see cref="DynamicApiActionInfo"/> object.
        /// 构造函数
        /// </summary>
        /// <param name="actionName">Name of the action in the controller / 控制器中Action的名称</param>
        /// <param name="verb">The HTTP verb that is used to call this action / 调用此方法的Http操作</param>
        /// <param name="method">The method which will be invoked when this action is called / 调用此操作时将调用的方法</param>
        /// <param name="filters">Filters / 此控制器Action的动态Action过滤器</param>
        /// <param name="isApiExplorerEnabled">Is API explorer enabled / 是否启用API预览</param>
        public DynamicApiActionInfo(
            string actionName, 
            HttpVerb verb, 
            MethodInfo method, 
            IFilter[] filters = null,
            bool? isApiExplorerEnabled = null)
        {
            ActionName = actionName;
            Verb = verb;
            Method = method;
            IsApiExplorerEnabled = isApiExplorerEnabled;
            Filters = filters ?? new IFilter[] { }; //Assigning or initialzing the action filters.
        }
    }
}