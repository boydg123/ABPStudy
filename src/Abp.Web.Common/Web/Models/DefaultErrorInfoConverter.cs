using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Collections.Extensions;
using Abp.Domain.Entities;
using Abp.Extensions;
using Abp.Localization;
using Abp.Runtime.Validation;
using Abp.UI;
using Abp.Web.Configuration;

namespace Abp.Web.Models
{
    //TODO@Halil: I did not like constructing ErrorInfo this way. It works wlll but I think we should change it later...
    /// <summary>
    /// 异常转换错误信息转换器默认实现
    /// </summary>
    internal class DefaultErrorInfoConverter : IExceptionToErrorInfoConverter
    {
        /// <summary>
        /// ABP Web Common模块配置
        /// </summary>
        private readonly IAbpWebCommonModuleConfiguration _configuration;

        /// <summary>
        /// 本地化管理器
        /// </summary>
        private readonly ILocalizationManager _localizationManager;

        /// <summary>
        /// 下一个转换器
        /// </summary>
        public IExceptionToErrorInfoConverter Next { set; private get; }

        /// <summary>
        /// 是否发送所有异常至客户端
        /// </summary>
        private bool SendAllExceptionsToClients
        {
            get
            {
                return _configuration.SendAllExceptionsToClients;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration">ABP Web Common模块配置</param>
        /// <param name="localizationManager">本地化管理器</param>
        public DefaultErrorInfoConverter(
            IAbpWebCommonModuleConfiguration configuration, 
            ILocalizationManager localizationManager)
        {
            _configuration = configuration;
            _localizationManager = localizationManager;
        }

        /// <summary>
        /// 将异常对象转换至错误信息对象
        /// </summary>
        /// <param name="exception">异常对象</param>
        /// <returns>错误信息对象</returns>
        public ErrorInfo Convert(Exception exception)
        {
            var errorInfo = CreateErrorInfoWithoutCode(exception);

            if (exception is IHasErrorCode)
            {
                errorInfo.Code = (exception as IHasErrorCode).Code;
            }

            return errorInfo;
        }

        /// <summary>
        /// 创建错误信息不使用错误码
        /// </summary>
        /// <param name="exception">异常对象</param>
        /// <returns></returns>
        private ErrorInfo CreateErrorInfoWithoutCode(Exception exception)
        {
            if (SendAllExceptionsToClients)
            {
                return CreateDetailedErrorInfoFromException(exception);
            }

            if (exception is AggregateException && exception.InnerException != null)
            {
                var aggException = exception as AggregateException;
                if (aggException.InnerException is UserFriendlyException ||
                    aggException.InnerException is AbpValidationException)
                {
                    exception = aggException.InnerException;
                }
            }

            if (exception is UserFriendlyException)
            {
                var userFriendlyException = exception as UserFriendlyException;
                return new ErrorInfo(userFriendlyException.Message, userFriendlyException.Details);
            }

            if (exception is AbpValidationException)
            {
                return new ErrorInfo(L("ValidationError"))
                {
                    ValidationErrors = GetValidationErrorInfos(exception as AbpValidationException),
                    Details = GetValidationErrorNarrative(exception as AbpValidationException)
                };
            }

            if (exception is EntityNotFoundException)
            {
                var entityNotFoundException = exception as EntityNotFoundException;

                return new ErrorInfo(
                    string.Format(
                        L("EntityNotFound"),
                        entityNotFoundException.EntityType.Name,
                        entityNotFoundException.Id
                    )
                );
            }

            if (exception is Abp.Authorization.AbpAuthorizationException)
            {
                var authorizationException = exception as Abp.Authorization.AbpAuthorizationException;
                return new ErrorInfo(authorizationException.Message);
            }

            return new ErrorInfo(L("InternalServerError"));
        }

        /// <summary>
        /// 使用给定的异常对象创建详细的错误信息
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        private ErrorInfo CreateDetailedErrorInfoFromException(Exception exception)
        {
            var detailBuilder = new StringBuilder();

            AddExceptionToDetails(exception, detailBuilder);

            var errorInfo = new ErrorInfo(exception.Message, detailBuilder.ToString());

            if (exception is AbpValidationException)
            {
                errorInfo.ValidationErrors = GetValidationErrorInfos(exception as AbpValidationException);
            }

            return errorInfo;
        }

        /// <summary>
        /// 添加异常消息至详细信息
        /// </summary>
        /// <param name="exception">异常对象</param>
        /// <param name="detailBuilder"></param>
        private void AddExceptionToDetails(Exception exception, StringBuilder detailBuilder)
        {
            //Exception Message
            detailBuilder.AppendLine(exception.GetType().Name + ": " + exception.Message);

            //Additional info for UserFriendlyException
            if (exception is UserFriendlyException)
            {
                var userFriendlyException = exception as UserFriendlyException;
                if (!string.IsNullOrEmpty(userFriendlyException.Details))
                {
                    detailBuilder.AppendLine(userFriendlyException.Details);
                }
            }

            //Additional info for AbpValidationException
            if (exception is AbpValidationException)
            {
                var validationException = exception as AbpValidationException;
                if (validationException.ValidationErrors.Count > 0)
                {
                    detailBuilder.AppendLine(GetValidationErrorNarrative(validationException));
                }
            }

            //Exception StackTrace
            if (!string.IsNullOrEmpty(exception.StackTrace))
            {
                detailBuilder.AppendLine("STACK TRACE: " + exception.StackTrace);
            }

            //Inner exception
            if (exception.InnerException != null)
            {
                AddExceptionToDetails(exception.InnerException, detailBuilder);
            }

            //Inner exceptions for AggregateException
            if (exception is AggregateException)
            {
                var aggException = exception as AggregateException;
                if (aggException.InnerExceptions.IsNullOrEmpty())
                {
                    return;
                }

                foreach (var innerException in aggException.InnerExceptions)
                {
                    AddExceptionToDetails(innerException, detailBuilder);
                }
            }
        }

        /// <summary>
        /// 获取验证的错误信息集合
        /// </summary>
        /// <param name="validationException">验证异常</param>
        /// <returns></returns>
        private ValidationErrorInfo[] GetValidationErrorInfos(AbpValidationException validationException)
        {
            var validationErrorInfos = new List<ValidationErrorInfo>();

            foreach (var validationResult in validationException.ValidationErrors)
            {
                var validationError = new ValidationErrorInfo(validationResult.ErrorMessage);

                if (validationResult.MemberNames != null && validationResult.MemberNames.Any())
                {
                    validationError.Members = validationResult.MemberNames.Select(m => m.ToCamelCase()).ToArray();
                }

                validationErrorInfos.Add(validationError);
            }

            return validationErrorInfos.ToArray();
        }

        /// <summary>
        /// 获取验证错误叙述
        /// </summary>
        /// <param name="validationException">验证异常</param>
        /// <returns></returns>
        private string GetValidationErrorNarrative(AbpValidationException validationException)
        {
            var detailBuilder = new StringBuilder();
            detailBuilder.AppendLine(L("ValidationNarrativeTitle"));
            
            foreach (var validationResult in validationException.ValidationErrors)
            {
                detailBuilder.AppendFormat(" - {0}", validationResult.ErrorMessage);
                detailBuilder.AppendLine();
            }

            return detailBuilder.ToString();
        }

        private string L(string name)
        {
            try
            {
                return _localizationManager.GetString(AbpWebConsts.LocalizaionSourceName, name);
            }
            catch (Exception)
            {
                return name;
            }
        }
    }
}