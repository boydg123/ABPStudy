using System.Reflection;
using Abp.Web;
using System.Web.Http.Filters;
using System.Linq;
using Abp.Reflection;
using System.Web.Http;

namespace Abp.WebApi.Controllers.Dynamic.Builders
{
    /// <summary>
    /// Used to build <see cref="DynamicApiActionInfo"/> object.
    /// 用于构建<see cref="DynamicApiActionInfo"/>对象
    /// </summary>
    /// <typeparam name="T">Type of the proxied object / 被代理对象的类型</typeparam>
    internal class ApiControllerActionBuilder<T> : IApiControllerActionBuilder<T>
    {
        /// <summary>
        /// Selected action name.
        /// 选中的action名称
        /// </summary>
        public string ActionName { get; }

        /// <summary>
        /// Underlying proxying method.
        /// 潜在的代理方法
        /// </summary>
        public MethodInfo Method { get; }

        /// <summary>
        /// Selected Http verb.
        /// 选中的Http请求动作
        /// </summary>
        public HttpVerb? Verb { get; set; }

        /// <summary>
        /// Is API Explorer enabled.
        /// 是否开启API浏览
        /// </summary>
        public bool? IsApiExplorerEnabled { get; set; }

        /// <summary>
        /// Action Filters for dynamic controller method.
        /// 动态控制器方法的action过滤器集合
        /// </summary>
        public IFilter[] Filters { get; set; }

        /// <summary>
        /// A flag to set if no action will be created for this method.
        /// 如果此方法不创建action，将设置一个标记
        /// </summary>
        public bool DontCreate { get; set; }

        /// <summary>
        /// Reference to the <see cref="ApiControllerBuilder{T}"/> which created this object.
        /// 创建此对象的<see cref="ApiControllerBuilder{T}"/>的引用
        /// </summary>
        public IApiControllerBuilder Controller
        {
            get { return _controller; }
        }
        private readonly ApiControllerBuilder<T> _controller;

        /// <summary>
        /// Creates a new <see cref="ApiControllerActionBuilder{T}"/> object.
        /// 构造函数
        /// </summary>
        /// <param name="apiControllerBuilder">Reference to the <see cref="ApiControllerBuilder{T}"/> which created this object / 创建此对象的<see cref="ApiControllerBuilder{T}"/>的引用</param>
        /// <param name="methodInfo">Method / 方法</param>
        public ApiControllerActionBuilder(ApiControllerBuilder<T> apiControllerBuilder, MethodInfo methodInfo)
        {
            _controller = apiControllerBuilder;
            Method = methodInfo;
            ActionName = Method.Name;
        }

        /// <summary>
        /// Used to specify Http verb of the action.
        /// 用于当前Action指定的http请求动作
        /// </summary>
        /// <param name="verb">Http very / http请求动作</param>
        /// <returns>Action builder / action构建器</returns>
        public IApiControllerActionBuilder<T> WithVerb(HttpVerb verb)
        {
            Verb = verb;
            return this;
        }

        /// <summary>
        /// Enables/Disables API Explorer for the action.
        /// 为action 启用/禁用 API浏览
        /// </summary>
        public IApiControllerActionBuilder<T> WithApiExplorer(bool isEnabled)
        {
            IsApiExplorerEnabled = isEnabled;
            return this;
        }

        /// <summary>
        /// Used to specify another method definition.
        /// 用于指定另一方法定义
        /// </summary>
        /// <param name="methodName">Name of the method in proxied type / 代理类型方法名</param>
        /// <returns>Action builder / action构建器</returns>
        public IApiControllerActionBuilder<T> ForMethod(string methodName)
        {
            return _controller.ForMethod(methodName);
        }

        /// <summary>
        /// Used to add action filters to apply to this method.
        /// 用于添加应用于此方法的action过滤器
        /// </summary>
        /// <param name="filters"> Action Filters to apply. / 应用过滤器</param>
        public IApiControllerActionBuilder<T> WithFilters(params IFilter[] filters)
        {
            Filters = filters;
            return this;
        }

        /// <summary>
        /// Tells builder to not create action for this method.
        /// 告诉生成器不为此方法创建action
        /// </summary>
        /// <returns>Controller builder / 控制器生成器</returns>
        public IApiControllerBuilder<T> DontCreateAction()
        {
            DontCreate = true;
            return _controller;
        }

        /// <summary>
        /// Builds the controller.This method must be called at last of the build operation.
        /// 构建控制器，此方法必须在生成操作最后调用
        /// </summary>
        public void Build()
        {
            _controller.Build();
        }

        /// <summary>
        /// Builds <see cref="DynamicApiActionInfo"/> object for this configuration.
        /// 为此配置构建<see cref="DynamicApiActionInfo"/>对象
        /// </summary>
        /// <param name="conventionalVerbs"></param>
        /// <returns></returns>
        internal DynamicApiActionInfo BuildActionInfo(bool conventionalVerbs)
        {
            return new DynamicApiActionInfo(
                ActionName,
                GetNormalizedVerb(conventionalVerbs),
                Method,
                Filters,
                IsApiExplorerEnabled
            );
        }

        /// <summary>
        /// 获取统一的操作
        /// </summary>
        /// <param name="conventionalVerbs">是否是通用的请求动作</param>
        /// <returns></returns>
        private HttpVerb GetNormalizedVerb(bool conventionalVerbs)
        {
            if (Verb != null)
            {
                return Verb.Value;
            }

            if (Method.IsDefined(typeof(HttpGetAttribute)))
            {
                return HttpVerb.Get;
            }

            if (Method.IsDefined(typeof(HttpPostAttribute)))
            {
                return HttpVerb.Post;
            }

            if (Method.IsDefined(typeof(HttpPutAttribute)))
            {
                return HttpVerb.Put;
            }

            if (Method.IsDefined(typeof(HttpDeleteAttribute)))
            {
                return HttpVerb.Delete;
            }

            if (Method.IsDefined(typeof(HttpPatchAttribute)))
            {
                return HttpVerb.Patch;
            }

            if (Method.IsDefined(typeof(HttpOptionsAttribute)))
            {
                return HttpVerb.Options;
            }

            if (Method.IsDefined(typeof(HttpHeadAttribute)))
            {
                return HttpVerb.Head;
            }

            if (conventionalVerbs)
            {
                var conventionalVerb = DynamicApiVerbHelper.GetConventionalVerbForMethodName(ActionName);
                if (conventionalVerb == HttpVerb.Get && !HasOnlyPrimitiveIncludingNullableTypeParameters(Method))
                {
                    conventionalVerb = DynamicApiVerbHelper.GetDefaultHttpVerb();
                }

                return conventionalVerb;
            }

            return DynamicApiVerbHelper.GetDefaultHttpVerb();
        }

        /// <summary>
        /// 仅仅原始的包括可空类型参数
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        private static bool HasOnlyPrimitiveIncludingNullableTypeParameters(MethodInfo methodInfo)
        {
            return methodInfo.GetParameters().All(p => TypeHelper.IsPrimitiveExtendedIncludingNullable(p.ParameterType) || p.IsDefined(typeof(FromUriAttribute)));
        }
    }
}