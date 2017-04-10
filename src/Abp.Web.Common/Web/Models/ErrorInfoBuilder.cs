using System;
using Abp.Dependency;
using Abp.Localization;
using Abp.Web.Configuration;

namespace Abp.Web.Models
{
    /// <summary>
    /// <see cref="ErrorInfo"/>对象构建器
    /// </summary>
    public class ErrorInfoBuilder : IErrorInfoBuilder, ISingletonDependency
    {
        /// <summary>
        /// <see cref="Exception"/>to<see cref="ErrorInfo"/>转换器
        /// </summary>
        private IExceptionToErrorInfoConverter Converter { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration">ABP Web Common模块配置</param>
        /// <param name="localizationManager">本地化管理器</param>
        public ErrorInfoBuilder(IAbpWebCommonModuleConfiguration configuration, ILocalizationManager localizationManager)
        {
            Converter = new DefaultErrorInfoConverter(configuration, localizationManager);
        }

        /// <summary>
        /// 使用给定的<paramref name="exception"/>对象创建一个<see cref="ErrorInfo"/>新的实例
        /// </summary>
        /// <param name="exception">异常对象</param>
        /// <returns>创建的<see cref="ErrorInfo"/>对象</returns>
        public ErrorInfo BuildForException(Exception exception)
        {
            return Converter.Convert(exception);
        }

        /// <summary>
        /// Adds an exception converter that is used by <see cref="BuildForException"/> method.
        /// 添加一个使用<see cref="BuildForException"/>方法的异常转换器
        /// </summary>
        /// <param name="converter">Converter object / 转换器对象</param>
        public void AddExceptionConverter(IExceptionToErrorInfoConverter converter)
        {
            converter.Next = Converter;
            Converter = converter;
        }
    }
}
