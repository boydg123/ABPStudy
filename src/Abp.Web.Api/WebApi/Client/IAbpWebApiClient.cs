using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;

namespace Abp.WebApi.Client
{
    /// <summary>
    /// Used to make requests to ABP based Web APIs.
    /// 用于基于Web API的请求到ABP
    /// </summary>
    public interface IAbpWebApiClient
    {
        /// <summary>
        /// Base URL for all request. 
        /// 所有请求的基地址
        /// </summary>
        string BaseUrl { get; set; }

        /// <summary>
        /// Timeout value for all requests (used if not supplied in the request method).Default: 90 seconds.
        /// 请求的超时值(如果在请求中没有提供)默认值：90秒
        /// </summary>
        TimeSpan Timeout { get; set; }

        /// <summary>
        /// Used to set cookies for requests.
        /// 用于为请求设置Cookies
        /// </summary>
        Collection<Cookie> Cookies { get; }

        /// <summary>
        /// Request headers.
        /// 请求头
        /// </summary>
        ICollection<NameValue> RequestHeaders { get; }

        /// <summary>
        /// Response headers.
        /// 响应头
        /// </summary>
        ICollection<NameValue> ResponseHeaders { get; }

        /// <summary>
        /// Makes post request that does not get or return value.
        /// 使POST请求不能直接被Get或返回值
        /// </summary>
        /// <param name="url">Url / Url</param>
        /// <param name="timeout">Timeout as milliseconds / 超时时间(毫秒)</param>
        Task PostAsync(string url, int? timeout = null);

        /// <summary>
        /// Makes post request that gets input but does not return value.
        /// 使POST请求得到输入，但是不返回值
        /// </summary>
        /// <param name="url">Url / Url</param>
        /// <param name="input">Input / 输入</param>
        /// <param name="timeout">Timeout as milliseconds / 超时时间(毫秒)</param>
        Task PostAsync(string url, object input, int? timeout = null);

        /// <summary>
        /// Makes post request that does not get input but returns value.
        /// 使POST请求不得到输入，但是返回值
        /// </summary>
        /// <param name="url">Url / Url</param>
        /// <param name="timeout">Timeout as milliseconds / 超时时间(毫秒)</param>
        Task<TResult> PostAsync<TResult>(string url, int? timeout = null) where TResult : class;

        /// <summary>
        /// Makes post request that gets input and returns value.
        /// 使POST请求能获取到输入以及返回值
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="input">Input / 输入</param>
        /// <param name="timeout">Timeout as milliseconds / 超时时间(毫秒)</param>
        Task<TResult> PostAsync<TResult>(string url, object input, int? timeout = null) where TResult : class;
    }
}