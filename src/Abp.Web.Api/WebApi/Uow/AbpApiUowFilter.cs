using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.WebApi.Configuration;
using Abp.WebApi.Validation;

namespace Abp.WebApi.Uow
{
    /// <summary>
    /// ABP Api工作单元过滤器
    /// </summary>
    public class AbpApiUowFilter : IActionFilter, ITransientDependency
    {
        /// <summary>
        /// 工作单元管理器
        /// </summary>
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        /// <summary>
        /// ABP WebApi配置引用
        /// </summary>
        private readonly IAbpWebApiConfiguration _configuration;

        /// <summary>
        /// 是否允许多选
        /// </summary>
        public bool AllowMultiple => false;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unitOfWorkManager"></param>
        /// <param name="configuration"></param>
        public AbpApiUowFilter(
            IUnitOfWorkManager unitOfWorkManager,
            IAbpWebApiConfiguration configuration
            )
        {
            _unitOfWorkManager = unitOfWorkManager;
            _configuration = configuration;
        }

        /// <summary>
        /// 执行过滤方法
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="continuation"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            var methodInfo = actionContext.ActionDescriptor.GetMethodInfoOrNull();
            if (methodInfo == null)
            {
                return await continuation();
            }

            if (actionContext.ActionDescriptor.IsDynamicAbpAction())
            {
                return await continuation();
            }

            var unitOfWorkAttr = UnitOfWorkAttribute.GetUnitOfWorkAttributeOrNull(methodInfo) ??
                                 _configuration.DefaultUnitOfWorkAttribute;

            if (unitOfWorkAttr.IsDisabled)
            {
                return await continuation();
            }

            using (var uow = _unitOfWorkManager.Begin(unitOfWorkAttr.CreateOptions()))
            {
                var result = await continuation();
                await uow.CompleteAsync();
                return result;
            }
        }
    }
}