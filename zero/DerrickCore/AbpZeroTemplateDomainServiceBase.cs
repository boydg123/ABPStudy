using Abp.Domain.Services;

namespace Derrick
{
    /// <summary>
    /// ABP Zero领域服务基类
    /// </summary>
    public abstract class AbpZeroTemplateDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        /// <summary>
        /// 构造函数
        /// </summary>
        protected AbpZeroTemplateDomainServiceBase()
        {
            LocalizationSourceName = AbpZeroTemplateConsts.LocalizationSourceName;
        }
    }
}
