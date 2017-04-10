using System.Reflection;
using Abp.Reflection;

namespace Abp.Web.Security.AntiForgery
{
    /// <summary>
    /// ABP Web 防伪管理器扩展
    /// </summary>
    public static class AbpAntiForgeryManagerWebExtensions
    {
        /// <summary>
        /// 应该验证
        /// </summary>
        /// <param name="manager">ABP防伪管理器</param>
        /// <param name="antiForgeryWebConfiguration">ABP Web防伪管理</param>
        /// <param name="methodInfo">方法信息</param>
        /// <param name="httpVerb">Http请求方法</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static bool ShouldValidate(
            this IAbpAntiForgeryManager manager,
            IAbpAntiForgeryWebConfiguration antiForgeryWebConfiguration,
            MethodInfo methodInfo, 
            HttpVerb httpVerb, 
            bool defaultValue)
        {
            if (!antiForgeryWebConfiguration.IsEnabled)
            {
                return false;
            }

            if (methodInfo.IsDefined(typeof(ValidateAbpAntiForgeryTokenAttribute), true))
            {
                return true;
            }

            if (ReflectionHelper.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<DisableAbpAntiForgeryTokenValidationAttribute>(methodInfo) != null)
            {
                return false;
            }

            if (antiForgeryWebConfiguration.IgnoredHttpVerbs.Contains(httpVerb))
            {
                return false;
            }

            if (methodInfo.DeclaringType?.IsDefined(typeof(ValidateAbpAntiForgeryTokenAttribute), true) ?? false)
            {
                return true;
            }

            return defaultValue;
        }
    }
}
