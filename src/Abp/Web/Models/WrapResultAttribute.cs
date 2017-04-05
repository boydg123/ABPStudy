using System;

namespace Abp.Web.Models
{
    /// <summary>
    /// Used to determine how ABP should wrap response on the web layer.
    /// 用于确定ABP如何在Web层上封装响应
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method)]
    public class WrapResultAttribute : Attribute
    {
        /// <summary>
        /// Wrap result on success.
        /// 成功的结果
        /// </summary>
        public bool WrapOnSuccess { get; set; }

        /// <summary>
        /// Wrap result on error.
        /// 错误的结果
        /// </summary>
        public bool WrapOnError { get; set; }

        /// <summary>
        /// Log errors.Default: true.
        /// 错误日志.默认:true
        /// </summary>
        public bool LogError { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WrapResultAttribute"/> class.
        /// 初始化<see cref="WrapResultAttribute"/>类新的实例
        /// </summary>
        /// <param name="wrapOnSuccess">Wrap result on success. / 成功的结果</param>
        /// <param name="wrapOnError">Wrap result on error. / 错误的结果</param>
        public WrapResultAttribute(bool wrapOnSuccess = true, bool wrapOnError = true)
        {
            WrapOnSuccess = wrapOnSuccess;
            WrapOnError = wrapOnError;

            LogError = true;
        }
    }
}
