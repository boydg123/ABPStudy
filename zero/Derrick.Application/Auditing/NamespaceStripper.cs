using Abp.Dependency;
using Abp.Extensions;
using System.Linq;

namespace Derrick.Auditing
{
    /// <summary>
    /// <see cref="INamespaceStripper"/>实现，命名空间剥离器
    /// </summary>
    public class NamespaceStripper : INamespaceStripper, ITransientDependency
    {
        /// <summary>
        /// 剥离命名空间
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns></returns>
        public string StripNameSpace(string serviceName)
        {
            if (serviceName.IsNullOrEmpty() || !serviceName.Contains("."))
            {
                return serviceName;
            }

            if (serviceName.Contains("["))
            {
                return StripGenericNamespace(serviceName);
            }

            return GetTextAfterLastDot(serviceName);
        }

        /// <summary>
        /// 获取最后一个点后的文本
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns></returns>
        private static string GetTextAfterLastDot(string text)
        {
            var lastDotIndex = text.LastIndexOf('.');
            return text.Substring(lastDotIndex + 1);
        }

        /// <summary>
        /// 剥离通用的命名空间
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns></returns>
        private static string StripGenericNamespace(string serviceName)
        {
            var serviceNameParts = serviceName.Split('[').Where(s => !s.IsNullOrEmpty()).ToList();
            var genericServiceName = "";
            var openBracketCount = 0;

            for (var i = 0; i < serviceNameParts.Count; i++)
            {
                var serviceNamePart = serviceNameParts[i];
                if (serviceNamePart.Contains("`"))
                {
                    genericServiceName += GetTextAfterLastDot(serviceNamePart.Substring(0, serviceNamePart.IndexOf('`'))) + "<";
                    openBracketCount++;
                }

                if (serviceNamePart.Contains(","))
                {
                    genericServiceName += GetTextAfterLastDot(serviceNamePart.Substring(0, serviceNamePart.IndexOf(',')));
                    if (i + 1 < serviceNameParts.Count && serviceNameParts[i + 1].Contains(","))
                    {
                        genericServiceName += ", ";
                    }
                    else
                    {
                        genericServiceName += ">";
                        openBracketCount--;
                    }
                }
            }

            for (int i = 0; i < openBracketCount; i++)
            {
                genericServiceName += ">";
            }

            return genericServiceName;
        }
    }
}
