using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.Http.Controllers;
using Abp.Collections.Extensions;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Runtime.Validation.Interception;

namespace Abp.WebApi.Validation
{
    /// <summary>
    /// Web Api Action 调用验证器
    /// </summary>
    public class WebApiActionInvocationValidator : MethodInvocationValidator
    {
        /// <summary>
        /// Http Action 上下文
        /// </summary>
        protected HttpActionContext ActionContext { get; private set; }

        /// <summary>
        /// 是否之前验证
        /// </summary>
        private bool _isValidatedBefore;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="iocResolver"></param>
        public WebApiActionInvocationValidator(IValidationConfiguration configuration, IIocResolver iocResolver)
            : base(configuration, iocResolver)
        {

        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="methodInfo"></param>
        public void Initialize(HttpActionContext actionContext, MethodInfo methodInfo)
        {
            base.Initialize(
                methodInfo,
                GetParameterValues(actionContext, methodInfo)
            );

            ActionContext = actionContext;
        }

        /// <summary>
        /// 设置数据注释属性错误
        /// </summary>
        /// <param name="validatingObject"></param>
        protected override void SetDataAnnotationAttributeErrors(object validatingObject)
        {
            if (_isValidatedBefore || ActionContext.ModelState.IsValid)
            {
                return;
            }

            foreach (var state in ActionContext.ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    ValidationErrors.Add(new ValidationResult(error.ErrorMessage, new[] { state.Key }));
                }
            }

            _isValidatedBefore = true;
        }

        /// <summary>
        /// 获取参数值
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        protected virtual object[] GetParameterValues(HttpActionContext actionContext, MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            var parameterValues = new object[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                parameterValues[i] = actionContext.ActionArguments.GetOrDefault(parameters[i].Name);
            }

            return parameterValues;
        }
    }
}