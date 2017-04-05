using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Dependency;

namespace Abp.Runtime.Validation
{
    /// <summary>
    /// 自定义验证上下文
    /// </summary>
    public class CustomValidationContext
    {
        /// <summary>
        /// List of validation results (errors). Add validation errors to this list.
        /// 验证错误结果集合，添加验证错误到这个集合
        /// </summary>
        public List<ValidationResult> Results { get; }

        /// <summary>
        /// Can be used to resolve dependencies on validation.
        /// 可以用于解析验证的依赖
        /// </summary>
        public IIocResolver IocResolver { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="results">验证错误结果集合，添加验证错误到这个集合</param>
        /// <param name="iocResolver">可以用于解析验证的依赖</param>
        public CustomValidationContext(List<ValidationResult> results, IIocResolver iocResolver)
        {
            Results = results;
            IocResolver = iocResolver;
        }
    }
}