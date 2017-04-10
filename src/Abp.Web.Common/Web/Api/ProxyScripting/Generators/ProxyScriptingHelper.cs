using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Collections.Extensions;
using Abp.Extensions;
using Abp.Web.Api.Modeling;

namespace Abp.Web.Api.ProxyScripting.Generators
{
    /// <summary>
    /// 代理脚本帮助类
    /// </summary>
    internal static class ProxyScriptingHelper
    {
        /// <summary>
        /// 默认Http请求：POST
        /// </summary>
        public const string DefaultHttpVerb = "POST";

        /// <summary>
        /// 基于给定的参数生成Url
        /// </summary>
        /// <param name="action">API Action描述模型</param>
        /// <returns></returns>
        public static string GenerateUrlWithParameters(ActionApiDescriptionModel action)
        {
            //TODO: Can be optimized using StringBuilder?
            var url = ReplacePathVariables(action.Url, action.Parameters);
            url = AddQueryStringParameters(url, action.Parameters);
            return url;
        }

        /// <summary>
        /// 生成Header
        /// </summary>
        /// <param name="action">API Action描述模型</param>
        /// <param name="indent">缩进</param>
        /// <returns></returns>
        public static string GenerateHeaders(ActionApiDescriptionModel action, int indent = 0)
        {
            var parameters = action
                .Parameters
                .Where(p => p.BindingSourceId == ParameterBindingSources.Header)
                .ToArray();

            if (!parameters.Any())
            {
                return null;
            }

            return ProxyScriptingJsFuncHelper.CreateJsObjectLiteral(parameters, indent);
        }

        /// <summary>
        /// 生成Body
        /// </summary>
        /// <param name="action">API Action描述模型</param>
        /// <returns></returns>
        public static string GenerateBody(ActionApiDescriptionModel action)
        {
            var parameters = action
                .Parameters
                .Where(p => p.BindingSourceId == ParameterBindingSources.Body)
                .ToArray();

            if (parameters.Length <= 0)
            {
                return null;
            }

            if (parameters.Length > 1)
            {
                throw new AbpException(
                    $"Only one complex type allowed as argument to a controller action that's binding source is 'Body'. But {action.Name} ({action.Url}) contains more than one!"
                    );
            }

            return ProxyScriptingJsFuncHelper.GetParamNameInJsFunc(parameters[0]);
        }

        /// <summary>
        /// 基于给定的POST数据生成表单
        /// </summary>
        /// <param name="action">API Action描述模型</param>
        /// <param name="indent">缩进</param>
        /// <returns></returns>
        public static string GenerateFormPostData(ActionApiDescriptionModel action, int indent = 0)
        {
            var parameters = action
                .Parameters
                .Where(p => p.BindingSourceId == ParameterBindingSources.Form)
                .ToArray();

            if (!parameters.Any())
            {
                return null;
            }

            return ProxyScriptingJsFuncHelper.CreateJsObjectLiteral(parameters, indent);
        }

        /// <summary>
        /// 替换路径变量
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="actionParameters">API参数模型列表</param>
        /// <returns></returns>
        private static string ReplacePathVariables(string url, IList<ParameterApiDescriptionModel> actionParameters)
        {
            var pathParameters = actionParameters
                .Where(p => p.BindingSourceId == ParameterBindingSources.Path)
                .ToArray();

            if (!pathParameters.Any())
            {
                return url;
            }

            foreach (var pathParameter in pathParameters)
            {
                url = url.Replace($"{{{pathParameter.Name}}}", $"' + {ProxyScriptingJsFuncHelper.GetParamNameInJsFunc(pathParameter)} + '");
            }

            return url;
        }

        /// <summary>
        /// 添加Query String参数
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="actionParameters">API参数模型列表</param>
        /// <returns></returns>
        private static string AddQueryStringParameters(string url, IList<ParameterApiDescriptionModel> actionParameters)
        {
            var queryStringParameters = actionParameters
                .Where(p => p.BindingSourceId.IsIn(ParameterBindingSources.ModelBinding, ParameterBindingSources.Query))
                .ToArray();

            if (!queryStringParameters.Any())
            {
                return url;
            }

            var qsBuilderParams = queryStringParameters
                .Select(p => $"{{ name: '{p.Name.ToCamelCase()}', value: {ProxyScriptingJsFuncHelper.GetParamNameInJsFunc(p)} }}")
                .JoinAsString(", ");

            return url + $"' + abp.utils.buildQueryString([{qsBuilderParams}]) + '";
        }

        /// <summary>
        /// 为方法名称获取常用的请求动作
        /// </summary>
        /// <param name="methodName">方法名称</param>
        /// <returns></returns>
        public static string GetConventionalVerbForMethodName(string methodName)
        {
            if (methodName.StartsWith("Get", StringComparison.InvariantCultureIgnoreCase))
            {
                return "GET";
            }

            if (methodName.StartsWith("Put", StringComparison.InvariantCultureIgnoreCase) ||
                methodName.StartsWith("Update", StringComparison.InvariantCultureIgnoreCase))
            {
                return "PUT";
            }

            if (methodName.StartsWith("Delete", StringComparison.InvariantCultureIgnoreCase) ||
                methodName.StartsWith("Remove", StringComparison.InvariantCultureIgnoreCase))
            {
                return "DELETE";
            }

            if (methodName.StartsWith("Patch", StringComparison.InvariantCultureIgnoreCase))
            {
                return "PATCH";
            }

            if (methodName.StartsWith("Post", StringComparison.InvariantCultureIgnoreCase) ||
                methodName.StartsWith("Create", StringComparison.InvariantCultureIgnoreCase) ||
                methodName.StartsWith("Insert", StringComparison.InvariantCultureIgnoreCase))
            {
                return "POST";
            }

            //Default
            return DefaultHttpVerb;
        }
    }
}
