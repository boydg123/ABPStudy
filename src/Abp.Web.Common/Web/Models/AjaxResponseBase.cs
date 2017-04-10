namespace Abp.Web.Models
{
    /// <summary>
    /// AJAX响应基类
    /// </summary>
    public abstract class AjaxResponseBase
    {
        /// <summary>
        /// This property can be used to redirect user to a specified URL.
        /// 此属性可以将用户重定向到指定的URL。
        /// </summary>
        public string TargetUrl { get; set; }

        /// <summary>
        /// Indicates success status of the result.Set <see cref="Error"/> if this value is false.
        /// 指示结果的成功状态。如果是false，则将给<see cref="Error"/>设置值。
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Error details (Must and only set if <see cref="Success"/> is false).
        /// 错误详情(如果<see cref="Success"/>为false，则此属性必须且仅设置值)
        /// </summary>
        public ErrorInfo Error { get; set; }

        /// <summary>
        /// This property can be used to indicate that the current user has no privilege to perform this request.
        /// 此属性可用于指示当前用户没有执行此请求的权限。
        /// </summary>
        public bool UnAuthorizedRequest { get; set; }

        /// <summary>
        /// A special signature for AJAX responses. It's used in the client to detect if this is a response wrapped by ABP.
        /// AJAX响应的特殊签名。它用于在客户端检测这是否是通过ABP包装的一个响应。
        /// </summary>
        public bool __abp { get; } = true;
    }
}