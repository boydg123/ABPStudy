using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;
using Abp.Application.Services;
using Abp.Dependency;
using Abp.WebApi.Configuration;
using Abp.WebApi.Controllers.Dynamic;
using Abp.WebApi.Controllers.Dynamic.Selectors;

//TODO: This code need to be refactored.

namespace Abp.WebApi.Controllers.ApiExplorer
{
    /// <summary>
    /// ABP Api帮助浏览类(为每个API提供描述信息)
    /// </summary>
    public class AbpApiExplorer : System.Web.Http.Description.ApiExplorer, IApiExplorer, ISingletonDependency
    {
        /// <summary>
        /// API描述类集合
        /// </summary>
        private readonly Lazy<Collection<ApiDescription>> _apiDescriptions;

        /// <summary>
        /// ABP Api配置信息
        /// </summary>
        private readonly IAbpWebApiConfiguration _abpWebApiConfiguration;

        /// <summary>
        /// 动态API控制器管理器
        /// </summary>
        private readonly DynamicApiControllerManager _dynamicApiControllerManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="abpWebApiConfiguration">ABP Api配置信息</param>
        /// <param name="dynamicApiControllerManager">动态API控制器管理器</param>
        public AbpApiExplorer(
            IAbpWebApiConfiguration abpWebApiConfiguration,
            DynamicApiControllerManager dynamicApiControllerManager
            ) : base(abpWebApiConfiguration.HttpConfiguration)
        {
            _apiDescriptions = new Lazy<Collection<ApiDescription>>(InitializeApiDescriptions);
            _abpWebApiConfiguration = abpWebApiConfiguration;
            _dynamicApiControllerManager = dynamicApiControllerManager;
        }

        /// <summary>
        /// API描述信息集合
        /// </summary>
        public new Collection<ApiDescription> ApiDescriptions
        {
            get
            {
                return _apiDescriptions.Value;
            }
        }

        /// <summary>
        /// 初始化API描述信息
        /// </summary>
        /// <returns></returns>
        private Collection<ApiDescription> InitializeApiDescriptions()
        {
            var apiDescriptions = new Collection<ApiDescription>();

            foreach (var item in base.ApiDescriptions)
            {
                apiDescriptions.Add(item);
            }

            var dynamicApiControllerInfos = _dynamicApiControllerManager.GetAll();
            foreach (var dynamicApiControllerInfo in dynamicApiControllerInfos)
            {
                if (IsApiExplorerDisabled(dynamicApiControllerInfo))
                {
                    continue;
                }

                foreach (var dynamicApiActionInfo in dynamicApiControllerInfo.Actions.Values)
                {
                    if (IsApiExplorerDisabled(dynamicApiActionInfo))
                    {
                        continue;
                    }

                    var apiDescription = new ApiDescription();

                    var controllerDescriptor = new DynamicHttpControllerDescriptor(_abpWebApiConfiguration.HttpConfiguration, dynamicApiControllerInfo);
                    var actionDescriptor = new DynamicHttpActionDescriptor(_abpWebApiConfiguration, controllerDescriptor, dynamicApiActionInfo);

                    apiDescription.ActionDescriptor = actionDescriptor;
                    apiDescription.HttpMethod = actionDescriptor.SupportedHttpMethods[0];

                    var actionValueBinder = _abpWebApiConfiguration.HttpConfiguration.Services.GetActionValueBinder();
                    var actionBinding = actionValueBinder.GetBinding(actionDescriptor);

                    apiDescription.ParameterDescriptions.Clear();
                    foreach (var apiParameterDescription in CreateParameterDescription(actionBinding, actionDescriptor))
                    {
                        apiDescription.ParameterDescriptions.Add(apiParameterDescription);
                    }

                    SetResponseDescription(apiDescription, actionDescriptor);

                    apiDescription.RelativePath = "api/services/" + dynamicApiControllerInfo.ServiceName + "/" + dynamicApiActionInfo.ActionName;

                    apiDescriptions.Add(apiDescription);
                }
            }

            return apiDescriptions;
        }

        /// <summary>
        /// API帮助预览是否可用
        /// </summary>
        /// <param name="dynamicApiControllerInfo">动态API控制器信息</param>
        /// <returns></returns>
        private static bool IsApiExplorerDisabled(DynamicApiControllerInfo dynamicApiControllerInfo)
        {
            if (dynamicApiControllerInfo.IsApiExplorerEnabled == false)
            {
                if (!RemoteServiceAttribute.IsMetadataExplicitlyEnabledFor(dynamicApiControllerInfo.ServiceInterfaceType))
                {
                    return true;
                }
            }
            else
            {
                if (RemoteServiceAttribute.IsMetadataExplicitlyDisabledFor(dynamicApiControllerInfo.ServiceInterfaceType))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// API帮助预览是否可用
        /// </summary>
        /// <param name="dynamicApiActionInfo">动态API Action信息</param>
        /// <returns></returns>
        private static bool IsApiExplorerDisabled(DynamicApiActionInfo dynamicApiActionInfo)
        {
            if (dynamicApiActionInfo.IsApiExplorerEnabled == false)
            {
                if (!RemoteServiceAttribute.IsMetadataExplicitlyEnabledFor(dynamicApiActionInfo.Method))
                {
                    return true;
                }
            }
            else
            {
                if (RemoteServiceAttribute.IsMetadataExplicitlyDisabledFor(dynamicApiActionInfo.Method))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 设置响应描述
        /// </summary>
        /// <param name="apiDescription">API描述信息</param>
        /// <param name="actionDescriptor">动态Http Action描述器</param>
        private void SetResponseDescription(ApiDescription apiDescription, DynamicHttpActionDescriptor actionDescriptor)
        {
            var responseDescription = CreateResponseDescription(actionDescriptor);
            var prop2 = typeof(ApiDescription).GetProperties().Single(p => p.Name == "ResponseDescription");
            prop2.SetValue(apiDescription, responseDescription);
        }

        /// <summary>
        /// 创建参数描述
        /// </summary>
        /// <param name="actionBinding">Http Action绑定</param>
        /// <param name="actionDescriptor">Http Action描述器</param>
        /// <returns></returns>
        private IList<ApiParameterDescription> CreateParameterDescription(HttpActionBinding actionBinding, HttpActionDescriptor actionDescriptor)
        {
            IList<ApiParameterDescription> parameterDescriptions = new List<ApiParameterDescription>();
            // try get parameter binding information if available
            if (actionBinding != null)
            {
                HttpParameterBinding[] parameterBindings = actionBinding.ParameterBindings;
                if (parameterBindings != null)
                {
                    foreach (HttpParameterBinding parameter in parameterBindings)
                    {
                        parameterDescriptions.Add(CreateParameterDescriptionFromBinding(parameter));
                    }
                }
            }
            else
            {
                Collection<HttpParameterDescriptor> parameters = actionDescriptor.GetParameters();
                if (parameters != null)
                {
                    foreach (HttpParameterDescriptor parameter in parameters)
                    {
                        parameterDescriptions.Add(CreateParameterDescriptionFromDescriptor(parameter));
                    }
                }
            }


            return parameterDescriptions;
        }
        
        /// <summary>
        /// 从描述器创建参数描述信息
        /// </summary>
        /// <param name="parameter">Http 参数描述器</param>
        /// <returns></returns>
        private ApiParameterDescription CreateParameterDescriptionFromDescriptor(HttpParameterDescriptor parameter)
        {

            return new ApiParameterDescription
            {
                ParameterDescriptor = parameter,
                Name = parameter.Prefix ?? parameter.ParameterName,
                Documentation = GetApiParameterDocumentation(parameter),
                Source = ApiParameterSource.Unknown,
            };
        }

        /// <summary>
        /// 从Http参数绑定创建参数描述
        /// </summary>
        /// <param name="parameterBinding">Http参数绑定</param>
        /// <returns></returns>
        private ApiParameterDescription CreateParameterDescriptionFromBinding(HttpParameterBinding parameterBinding)
        {
            ApiParameterDescription parameterDescription = CreateParameterDescriptionFromDescriptor(parameterBinding.Descriptor);
            if (parameterBinding.WillReadBody)
            {
                parameterDescription.Source = ApiParameterSource.FromBody;
            }
            else if (parameterBinding.WillReadUri())
            {
                parameterDescription.Source = ApiParameterSource.FromUri;
            }

            return parameterDescription;
        }

        /// <summary>
        /// 创建响应描述
        /// </summary>
        /// <param name="actionDescriptor">Http Action描述器</param>
        /// <returns></returns>
        private ResponseDescription CreateResponseDescription(HttpActionDescriptor actionDescriptor)
        {
            Collection<ResponseTypeAttribute> responseTypeAttribute = actionDescriptor.GetCustomAttributes<ResponseTypeAttribute>();
            Type responseType = responseTypeAttribute.Select(attribute => attribute.ResponseType).FirstOrDefault();

            return new ResponseDescription
            {
                DeclaredType = actionDescriptor.ReturnType,
                ResponseType = responseType,
                Documentation = GetApiResponseDocumentation(actionDescriptor)
            };
        }

        /// <summary>
        /// 获取API响应文档
        /// </summary>
        /// <param name="actionDescriptor">Http Action描述器</param>
        /// <returns></returns>
        private string GetApiResponseDocumentation(HttpActionDescriptor actionDescriptor)
        {
            IDocumentationProvider documentationProvider = DocumentationProvider ?? actionDescriptor.Configuration.Services.GetDocumentationProvider();
            if (documentationProvider != null)
            {
                return documentationProvider.GetResponseDocumentation(actionDescriptor);
            }

            return null;
        }

        /// <summary>
        /// 获取API参数文档
        /// </summary>
        /// <param name="parameterDescriptor">Http参数描述器</param>
        /// <returns></returns>
        private string GetApiParameterDocumentation(HttpParameterDescriptor parameterDescriptor)
        {
            IDocumentationProvider documentationProvider = DocumentationProvider ?? parameterDescriptor.Configuration.Services.GetDocumentationProvider();
            if (documentationProvider != null)
            {
                return documentationProvider.GetDocumentation(parameterDescriptor);
            }

            return null;
        }
    }

    /// <summary>
    /// Http参数绑定扩展
    /// </summary>
    internal static class HttpParameterBindingExtensions
    {
        /// <summary>
        /// 将读取Uri
        /// </summary>
        /// <param name="parameterBinding">Http参数绑定对象</param>
        /// <returns></returns>
        public static bool WillReadUri(this HttpParameterBinding parameterBinding)
        {
            if (parameterBinding == null)
            {
                throw new ArgumentNullException(nameof(parameterBinding));
            }

            IValueProviderParameterBinding valueProviderParameterBinding = parameterBinding as IValueProviderParameterBinding;
            if (valueProviderParameterBinding != null)
            {
                IEnumerable<ValueProviderFactory> valueProviderFactories = valueProviderParameterBinding.ValueProviderFactories;
                if (valueProviderFactories.Any() && valueProviderFactories.All(factory => factory is IUriValueProviderFactory))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
