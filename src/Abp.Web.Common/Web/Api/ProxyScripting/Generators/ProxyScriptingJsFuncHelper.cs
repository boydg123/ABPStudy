using System;
using System.Linq;
using System.Text;
using Abp.Collections.Extensions;
using Abp.Extensions;
using Abp.Web.Api.Modeling;

namespace Abp.Web.Api.ProxyScripting.Generators
{
    /// <summary>
    /// 代理脚本JS方法帮助类
    /// </summary>
    internal static class ProxyScriptingJsFuncHelper
    {
        private const string ValidJsVariableNameChars = "abcdefghijklmnopqrstuxwvyzABCDEFGHIJKLMNOPQRSTUXWVYZ0123456789_";

        /// <summary>
        /// 规范化的JS变量名
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="additionalChars">额外字符串</param>
        /// <returns></returns>
        public static string NormalizeJsVariableName(string name, string additionalChars = "")
        {
            var validChars = ValidJsVariableNameChars + additionalChars;

            var sb = new StringBuilder(name);

            sb.Replace('-', '_');

            //Delete invalid chars
            foreach (var c in name)
            {
                if (!validChars.Contains(c))
                {
                    sb.Replace(c.ToString(), "");
                }
            }

            if (sb.Length == 0)
            {
                return "_" + Guid.NewGuid().ToString("N").Left(8);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 在JS方法中获取参数名
        /// </summary>
        /// <param name="parameterInfo">参数名对象</param>
        /// <returns></returns>
        public static string GetParamNameInJsFunc(ParameterApiDescriptionModel parameterInfo)
        {
            return parameterInfo.Name == parameterInfo.NameOnMethod
                       ? NormalizeJsVariableName(parameterInfo.Name.ToCamelCase(), ".")
                       : NormalizeJsVariableName(parameterInfo.NameOnMethod.ToCamelCase()) + "." + NormalizeJsVariableName(parameterInfo.Name.ToCamelCase(), ".");
        }

        /// <summary>
        /// 创建JS原义对象
        /// </summary>
        /// <param name="parameters">参数列表</param>
        /// <param name="indent">缩进</param>
        /// <returns></returns>
        public static string CreateJsObjectLiteral(ParameterApiDescriptionModel[] parameters, int indent = 0)
        {
            var sb = new StringBuilder();

            sb.AppendLine("{");

            foreach (var prm in parameters)
            {
                sb.AppendLine($"{new string(' ', indent)}  '{prm.Name}': {GetParamNameInJsFunc(prm)}");
            }

            sb.Append(new string(' ', indent) + "}");

            return sb.ToString();
        }

        /// <summary>
        /// 为JS方法参数列表
        /// </summary>
        /// <param name="action">参数模型</param>
        /// <param name="ajaxParametersName">ajax参数名称</param>
        /// <returns></returns>
        public static string GenerateJsFuncParameterList(ActionApiDescriptionModel action, string ajaxParametersName)
        {
            var methodParamNames = action.Parameters.Select(p => p.NameOnMethod).Distinct().ToList();
            methodParamNames.Add(ajaxParametersName);
            return methodParamNames.Select(prmName => NormalizeJsVariableName(prmName.ToCamelCase())).JoinAsString(", ");
        }
    }
}