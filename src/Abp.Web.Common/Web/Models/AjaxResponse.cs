using System;

namespace Abp.Web.Models
{
    /// <summary>
    /// This class is used to create standard responses for AJAX/remote requests.
    /// 此类用于为AJAX/remote请求创建标准的响应
    /// </summary>
    [Serializable]
    public class AjaxResponse : AjaxResponse<object>
    {
        /// <summary>
        /// Creates an <see cref="AjaxResponse"/> object.<see cref="AjaxResponseBase.Success"/> is set as true.
        /// 创建一个<see cref="AjaxResponse"/>对象，<see cref="AjaxResponseBase.Success"/>设置为true
        /// </summary>
        public AjaxResponse()
        {

        }

        /// <summary>
        /// Creates an <see cref="AjaxResponse"/> object with <see cref="AjaxResponseBase.Success"/> specified.
        /// 使用指定的<see cref="AjaxResponseBase.Success"/>创建一个<see cref="AjaxResponse"/>对象
        /// </summary>
        /// <param name="success">Indicates success status of the result / 指示结果的成功状态</param>
        public AjaxResponse(bool success)
            : base(success)
        {

        }

        /// <summary>
        /// Creates an <see cref="AjaxResponse"/> object with <see cref="AjaxResponse{TResult}.Result"/> specified.<see cref="AjaxResponseBase.Success"/> is set as true.
        /// 使用指定的<see cref="AjaxResponse{TResult}.Result"/>创建一个<see cref="AjaxResponse"/>对象。<see cref="AjaxResponseBase.Success"/>设置为true
        /// </summary>
        /// <param name="result">The actual result object / 实际结果对象</param>
        public AjaxResponse(object result)
            : base(result)
        {

        }

        /// <summary>
        /// Creates an <see cref="AjaxResponse"/> object with <see cref="AjaxResponseBase.Error"/> specified.<see cref="AjaxResponseBase.Success"/> is set as false.
        /// 使用指定的<see cref="AjaxResponseBase.Error"/>创建一个<see cref="AjaxResponse"/>对象。<see cref="AjaxResponseBase.Success"/>设置为false
        /// </summary>
        /// <param name="error">Error details / 错误详情</param>
        /// <param name="unAuthorizedRequest">Used to indicate that the current user has no privilege to perform this request / 用于指示当前用户没有权限执行此请求</param>
        public AjaxResponse(ErrorInfo error, bool unAuthorizedRequest = false)
            : base(error, unAuthorizedRequest)
        {

        }
    }
}