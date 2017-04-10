using System;
using Abp.Web.Models;

namespace Abp.Web.Mvc.Models
{
    /// <summary>
    /// 错误信息视图模型
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        public ErrorInfo ErrorInfo { get; set; }

        public Exception Exception { get; set; }

        public ErrorViewModel()
        {
            
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="errorInfo">错误信息</param>
        /// <param name="exception">异常对象</param>
        public ErrorViewModel(ErrorInfo errorInfo, Exception exception = null)
        {
            ErrorInfo = errorInfo;
            Exception = exception;
        }
    }
}
