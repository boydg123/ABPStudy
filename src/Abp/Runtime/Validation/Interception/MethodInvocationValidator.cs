using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Abp.Collections.Extensions;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Reflection;

namespace Abp.Runtime.Validation.Interception
{
    /// <summary>
    /// This class is used to validate a method call (invocation) for method arguments.
    /// 此类用于调用方法时验证参数
    /// </summary>
    public class MethodInvocationValidator : ITransientDependency
    {
        /// <summary>
        /// 方法信息
        /// </summary>
        protected MethodInfo Method { get; private set; }

        /// <summary>
        /// 参数值
        /// </summary>
        protected object[] ParameterValues { get; private set; }

        /// <summary>
        /// 参数信息
        /// </summary>
        protected ParameterInfo[] Parameters { get; private set; }

        /// <summary>
        /// 验证结果集合
        /// </summary>
        protected List<ValidationResult> ValidationErrors { get; }

        /// <summary>
        /// 规范化接口
        /// </summary>
        protected List<IShouldNormalize> ObjectsToBeNormalized { get; }

        /// <summary>
        /// 验证配置
        /// </summary>
        private readonly IValidationConfiguration _configuration;

        /// <summary>
        /// IOC解析器
        /// </summary>
        private readonly IIocResolver _iocResolver;

        /// <summary>
        /// Creates a new <see cref="MethodInvocationValidator"/> instance.
        /// 构造函数
        /// </summary>
        public MethodInvocationValidator(IValidationConfiguration configuration, IIocResolver iocResolver)
        {
            _configuration = configuration;
            _iocResolver = iocResolver;

            ValidationErrors = new List<ValidationResult>();
            ObjectsToBeNormalized = new List<IShouldNormalize>();
        }

        /// <param name="method">Method to be validated / 验证的方法</param>
        /// <param name="parameterValues">List of arguments those are used to call the <paramref name="method"/>. / 那些调用方法<paramref name="method"/>的参数集合</param>
        public virtual void Initialize(MethodInfo method, object[] parameterValues)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            if (parameterValues == null)
            {
                throw new ArgumentNullException(nameof(parameterValues));
            }

            Method = method;
            ParameterValues = parameterValues;
            Parameters = method.GetParameters();
        }

        /// <summary>
        /// Validates the method invocation.
        /// 验证方法调用
        /// </summary>
        public void Validate()
        {
            CheckInitialized();

            if (!Method.IsPublic)
            {
                return;
            }

            if (IsValidationDisabled())
            {
                return;                
            }

            if (Parameters.IsNullOrEmpty())
            {
                return;
            }

            if (Parameters.Length != ParameterValues.Length)
            {
                throw new Exception("Method parameter count does not match with argument count!");
            }

            for (var i = 0; i < Parameters.Length; i++)
            {
                ValidateMethodParameter(Parameters[i], ParameterValues[i]);
            }

            if (ValidationErrors.Any())
            {
                throw new AbpValidationException(
                    "Method arguments are not valid! See ValidationErrors for details.",
                    ValidationErrors
                    );
            }

            foreach (var objectToBeNormalized in ObjectsToBeNormalized)
            {
                objectToBeNormalized.Normalize();
            }
        }

        /// <summary>
        /// 检查初始化
        /// </summary>
        private void CheckInitialized()
        {
            if (Method == null)
            {
                throw new AbpException("This object has not been initialized. Call Initialize method first.");
            }
        }

        /// <summary>
        /// 是否禁用验证
        /// </summary>
        /// <returns></returns>
        protected virtual bool IsValidationDisabled()
        {
            if (Method.IsDefined(typeof(EnableValidationAttribute), true))
            {
                return false;
            }

            return ReflectionHelper.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<DisableValidationAttribute>(Method) != null;
        }

        /// <summary>
        /// Validates given parameter for given value.
        /// 为给定值验证给定参数
        /// </summary>
        /// <param name="parameterInfo">Parameter of the method to validate / 待验证的方法的参数信息</param>
        /// <param name="parameterValue">Value to validate / 验证的值</param>
        protected virtual void ValidateMethodParameter(ParameterInfo parameterInfo, object parameterValue)
        {
            if (parameterValue == null)
            {
                if (!parameterInfo.IsOptional && 
                    !parameterInfo.IsOut && 
                    !TypeHelper.IsPrimitiveExtendedIncludingNullable(parameterInfo.ParameterType))
                {
                    ValidationErrors.Add(new ValidationResult(parameterInfo.Name + " is null!", new[] { parameterInfo.Name }));
                }

                return;
            }

            ValidateObjectRecursively(parameterValue);
        }

        /// <summary>
        /// 验证对象递归
        /// </summary>
        /// <param name="validatingObject"></param>
        protected virtual void ValidateObjectRecursively(object validatingObject)
        {
            if (validatingObject == null)
            {
                return;
            }

            SetDataAnnotationAttributeErrors(validatingObject);

            //Validate items of enumerable
            if (validatingObject is IEnumerable && !(validatingObject is IQueryable))
            {
                foreach (var item in (validatingObject as IEnumerable))
                {
                    ValidateObjectRecursively(item);
                }
            }

            //Custom validations
            (validatingObject as ICustomValidate)?.AddValidationErrors(
                new CustomValidationContext(
                    ValidationErrors,
                    _iocResolver
                )
            );

            //Add list to be normalized later
            if (validatingObject is IShouldNormalize)
            {
                ObjectsToBeNormalized.Add(validatingObject as IShouldNormalize);
            }

            //Do not recursively validate for enumerable objects
            if (validatingObject is IEnumerable)
            {
                return;
            }

            var validatingObjectType = validatingObject.GetType();

            //Do not recursively validate for primitive objects
            if (TypeHelper.IsPrimitiveExtendedIncludingNullable(validatingObjectType))
            {
                return;
            }

            if (_configuration.IgnoredTypes.Any(t => t.IsInstanceOfType(validatingObject)))
            {
                return;
            }

            var properties = TypeDescriptor.GetProperties(validatingObject).Cast<PropertyDescriptor>();
            foreach (var property in properties)
            {
                if (property.Attributes.OfType<DisableValidationAttribute>().Any())
                {
                    continue;
                }

                ValidateObjectRecursively(property.GetValue(validatingObject));
            }
        }

        /// <summary>
        /// Checks all properties for DataAnnotations attributes.
        /// 检查数据注释属性的所有属性
        /// </summary>
        protected virtual void SetDataAnnotationAttributeErrors(object validatingObject)
        {
            var properties = TypeDescriptor.GetProperties(validatingObject).Cast<PropertyDescriptor>();
            foreach (var property in properties)
            {
                var validationAttributes = property.Attributes.OfType<ValidationAttribute>().ToArray();
                if (validationAttributes.IsNullOrEmpty())
                {
                    continue;
                }

                var validationContext = new ValidationContext(validatingObject)
                {
                    DisplayName = property.DisplayName,
                    MemberName = property.Name
                };

                foreach (var attribute in validationAttributes)
                {
                    var result = attribute.GetValidationResult(property.GetValue(validatingObject), validationContext);
                    if (result != null)
                    {
                        ValidationErrors.Add(result);
                    }
                }
            }

            if (validatingObject is IValidatableObject)
            {
                var results = (validatingObject as IValidatableObject).Validate(new ValidationContext(validatingObject));
                ValidationErrors.AddRange(results);
            }
        }
    }
}
