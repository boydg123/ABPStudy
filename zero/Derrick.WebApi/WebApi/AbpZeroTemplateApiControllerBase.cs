using Abp.WebApi.Controllers;

namespace Derrick.WebApi
{
    /// <summary>
    /// Web API 基类
    /// </summary>
    public abstract class AbpZeroTemplateApiControllerBase : AbpApiController
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        protected AbpZeroTemplateApiControllerBase()
        {
            LocalizationSourceName = AbpZeroTemplateConsts.LocalizationSourceName;
        }
    }
}