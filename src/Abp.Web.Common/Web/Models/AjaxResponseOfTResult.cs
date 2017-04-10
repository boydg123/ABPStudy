using System;

namespace Abp.Web.Models
{
    /// <summary>
    /// This class is used to create standard responses for AJAX requests.
    /// 此类用于为AJAX请求创建标准响应
    /// </summary>
    [Serializable]
    public class AjaxResponse<TResult>: AjaxResponseBase
    {
        /// <summary>
        /// The actual result object of AJAX request.It is set if <see cref="AjaxResponseBase.Success"/> is true.
        /// AJAX请求的实际结果对象。<see cref="AjaxResponseBase.Success"/>为true的花，它将被设置值
        /// </summary>
        public TResult Result { get; set; }

        /// <summary>
        /// Creates an <see cref="AjaxResponse"/> object with <see cref="Result"/> specified.<see cref="AjaxResponseBase.Success"/> is set as true.
        /// 使用指定的<see cref="Result"/>创建一个<see cref="AjaxResponse"/>对象.<see cref="AjaxResponseBase.Success"/>设置为true
        /// </summary>
        /// <param name="result">The actual result object of AJAX request / AJAX请求的实际结果对象</param>
        public AjaxResponse(TResult result)
        {
            Result = result;
            Success = true;
        }

        /// <summary>
        /// Creates an <see cref="AjaxResponse"/> object.<see cref="AjaxResponseBase.Success"/> is set as true.
        /// 创建一个<see cref="AjaxResponse"/>对象。<see cref="AjaxResponseBase.Success"/>设置为true
        /// </summary>
        public AjaxResponse()
        {
            Success = true;
        }

        /// <summary>
        /// Creates an <see cref="AjaxResponse"/> object with <see cref="AjaxResponseBase.Success"/> specified.
        /// 使用指定的<see cref="AjaxResponseBase.Success"/>创建一个<see cref="AjaxResponse"/>对象.
        /// </summary>
        /// <param name="success">Indicates success status of the result / 指示结果的成功状态</param>
        public AjaxResponse(bool success)
        {
            Success = success;
        }

        /// <summary>
        /// Creates an <see cref="AjaxResponse"/> object with <see cref="AjaxResponseBase.Error"/> specified.<see cref="AjaxResponseBase.Success"/> is set as false.
        /// 使用指定的<see cref="AjaxResponseBase.Success"/>创建一个<see cref="AjaxResponse"/>对象.<see cref="AjaxResponseBase.Success"/>设置为false
        /// </summary>
        /// <param name="error">Error details / 错误详情</param>
        /// <param name="unAuthorizedRequest">Used to indicate that the current user has no privilege to perform this request / 用于指示当前用户没有权限执行此请求</param>
        public AjaxResponse(ErrorInfo error, bool unAuthorizedRequest = false)
        {
            Error = error;
            UnAuthorizedRequest = unAuthorizedRequest;
            Success = false;
        }
    }
}