using Abp;

namespace Derrick
{
    /// <summary>
    /// This class can be used as a base class for services in this application.
    /// 此类被用作当前应用程序的服务基类。
    /// It has some useful objects property-injected and has some basic methods most of services may need to.
    /// 它有一些有用的对象属性注入，并且有大部分服务可能需要的基本方法。
    /// It's suitable for non domain nor application service classes.
    /// 它只用于非领域或应用程序服务类
    /// For domain services inherit <see cref="AbpZeroTemplateDomainServiceBase"/>.
    /// 领域服务继承自<see cref="AbpZeroTemplateDomainServiceBase"/>.
    /// For application services inherit AbpZeroTemplateAppServiceBase.
    /// 应用程序服务继承自<see cref="AbpZeroTemplateAppServiceBase"/>
    /// </summary>
    public abstract class AbpZeroTemplateServiceBase : AbpServiceBase
    {
        protected AbpZeroTemplateServiceBase()
        {
            LocalizationSourceName = AbpZeroTemplateConsts.LocalizationSourceName;
        }
    }
}