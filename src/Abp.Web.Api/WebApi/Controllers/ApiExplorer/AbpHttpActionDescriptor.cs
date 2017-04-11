using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace Abp.WebApi.Controllers.ApiExplorer
{
    /// <summary>
    /// ABP Http Action描述器
    /// </summary>
    public class AbpHttpActionDescriptor : HttpActionDescriptor
    {
        /// <summary>
        /// Action名称
        /// </summary>
        public override string ActionName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 返回类型
        /// </summary>
        public override Type ReturnType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 异步执行
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="arguments"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<object> ExecuteAsync(HttpControllerContext controllerContext, IDictionary<string, object> arguments, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <returns></returns>
        public override Collection<HttpParameterDescriptor> GetParameters()
        {
            throw new NotImplementedException();
        }
    }
}
