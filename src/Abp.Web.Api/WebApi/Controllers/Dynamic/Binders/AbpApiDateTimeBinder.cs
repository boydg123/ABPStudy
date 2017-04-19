using System;
using System.Globalization;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Abp.Timing;

namespace Abp.WebApi.Controllers.Dynamic.Binders
{
    /// <summary>
    /// Binds datetime values from api requests to model
    /// 将时间值从Api请求绑定到Model
    /// </summary>
    public class AbpApiDateTimeBinder : IModelBinder
    {
        /// <summary>
        /// 模型绑定
        /// </summary>
        /// <param name="actionContext">Http Action 上下文</param>
        /// <param name="bindingContext">模型绑定上下文</param>
        /// <returns></returns>
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var date = value?.ConvertTo(typeof(DateTime?), CultureInfo.CurrentCulture) as DateTime?;
            if (date != null)
            {
                bindingContext.Model = Clock.Normalize(date.Value);
            }

            return true;
        }
    }
}
