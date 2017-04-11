using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Extensions;
using Abp.Web.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Abp.WebApi.Client
{
    /// <summary>
    /// 用于基于Web API的请求到ABP
    /// </summary>
    public class AbpWebApiClient : ITransientDependency, IAbpWebApiClient
    {
        /// <summary>
        /// 请求的超时值(如果在请求中没有提供)默认值：90秒
        /// </summary>
        public static TimeSpan DefaultTimeout { get; set; }

        /// <summary>
        /// 所有请求的基地址
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// 请求的超时值(如果在请求中没有提供)默认值：90秒
        /// </summary>
        public TimeSpan Timeout { get; set; }

        /// <summary>
        /// 用于为请求设置Cookies
        /// </summary>
        public Collection<Cookie> Cookies { get; private set; }

        /// <summary>
        /// 请求头
        /// </summary>
        public ICollection<NameValue> RequestHeaders { get; set; }

        /// <summary>
        /// 响应头
        /// </summary>
        public ICollection<NameValue> ResponseHeaders { get; set; }

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static AbpWebApiClient()
        {
            DefaultTimeout = TimeSpan.FromSeconds(90);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public AbpWebApiClient()
        {
            Timeout = DefaultTimeout;
            Cookies = new Collection<Cookie>();
            RequestHeaders = new List<NameValue>();
            ResponseHeaders = new List<NameValue>();
        }

        /// <summary>
        /// 使POST请求不能直接被Get或返回值
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="timeout">超时时间(毫秒)</param>
        /// <returns></returns>
        public virtual async Task PostAsync(string url, int? timeout = null)
        {
            await PostAsync<object>(url, timeout);
        }

        /// <summary>
        /// 使POST请求得到输入，但是不返回值
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="input">输入</param>
        /// <param name="timeout">超时时间(毫秒)</param>
        /// <returns></returns>
        public virtual async Task PostAsync(string url, object input, int? timeout = null)
        {
            await PostAsync<object>(url, input, timeout);
        }

        /// <summary>
        /// 使POST请求不得到输入，但是返回值
        /// </summary>
        /// <typeparam name="TResult">请求类型</typeparam>
        /// <param name="url">Url</param>
        /// <param name="timeout">超时时间(毫秒)</param>
        /// <returns></returns>
        public virtual async Task<TResult> PostAsync<TResult>(string url, int? timeout = null)
            where TResult : class
        {
            return await PostAsync<TResult>(url, null, timeout);
        }

        /// <summary>
        /// 使POST请求能获取到输入以及返回值
        /// </summary>
        /// <typeparam name="TResult">请求类型</typeparam>
        /// <param name="url">Url</param>
        /// <param name="input">输入</param>
        /// <param name="timeout">超时时间(毫秒)</param>
        /// <returns></returns>
        public virtual async Task<TResult> PostAsync<TResult>(string url, object input, int? timeout = null)
            where TResult : class
        {
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler {CookieContainer = cookieContainer})
            {
                using (var client = new HttpClient(handler))
                {
                    client.Timeout = timeout.HasValue ? TimeSpan.FromMilliseconds(timeout.Value) : Timeout;

                    if (!BaseUrl.IsNullOrEmpty())
                    {
                        client.BaseAddress = new Uri(BaseUrl);
                    }

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    foreach (var header in RequestHeaders)
                    {
                        client.DefaultRequestHeaders.Add(header.Name, header.Value);
                    }
                    
                    using (var requestContent = new StringContent(Object2JsonString(input), Encoding.UTF8, "application/json"))
                    {
                        foreach (var cookie in Cookies)
                        {
                            if (!BaseUrl.IsNullOrEmpty())
                            {
                                cookieContainer.Add(new Uri(BaseUrl), cookie);
                            }
                            else
                            {
                                cookieContainer.Add(cookie);
                            }
                        }

                        using (var response = await client.PostAsync(url, requestContent))
                        {
                            SetResponseHeaders(response);

                            if (!response.IsSuccessStatusCode)
                            {
                                throw new AbpException("Could not made request to " + url + "! StatusCode: " + response.StatusCode + ", ReasonPhrase: " + response.ReasonPhrase);
                            }

                            var ajaxResponse = JsonString2Object<AjaxResponse<TResult>>(await response.Content.ReadAsStringAsync());
                            if (!ajaxResponse.Success)
                            {
                                throw new AbpRemoteCallException(ajaxResponse.Error);
                            }

                            return ajaxResponse.Result;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 设置响应头
        /// </summary>
        /// <param name="response">Http响应消息</param>
        private void SetResponseHeaders(HttpResponseMessage response)
        {
            ResponseHeaders.Clear();
            foreach (var header in response.Headers)
            {
                foreach (var headerValue in header.Value)
                {
                    ResponseHeaders.Add(new NameValue(header.Key, headerValue));
                }
            }
        }

        /// <summary>
        /// 序列化对象到Json字符串
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        private static string Object2JsonString(object obj)
        {
            if (obj == null)
            {
                return "";
            }

            return JsonConvert.SerializeObject(obj,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
        }

        /// <summary>
        /// 将Json字符串转换成对象
        /// </summary>
        /// <typeparam name="TObj">对象类型</typeparam>
        /// <param name="str">Json字符串</param>
        /// <returns></returns>
        private static TObj JsonString2Object<TObj>(string str)
        {
            return JsonConvert.DeserializeObject<TObj>(str,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
        }
    }
}