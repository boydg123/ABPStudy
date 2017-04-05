using System;

namespace Abp.Auditing
{
    /// <summary>
    /// This informations are collected for an <see cref="AuditedAttribute"/> method.
    /// 从标记了特性<see cref="AuditedAttribute"/>的方法收集审计信息.
    /// </summary>
    public class AuditInfo
    {
        /// <summary>
        /// TenantId.
        /// 租户ID
        /// </summary>
        public int? TenantId { get; set; }

        /// <summary>
        /// UserId.
        /// 用户ID
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// ImpersonatorUserId.
        /// 模拟用户ID
        /// </summary>
        public long? ImpersonatorUserId { get; set; }

        /// <summary>
        /// ImpersonatorTenantId.
        /// 模拟租户ID
        /// </summary>
        public int? ImpersonatorTenantId { get; set; }

        /// <summary>
        /// Service (class/interface) name.
        /// 服务 (类/接口) 名称.
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// Executed method name.
        /// 执行的方法名称
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// Calling parameters.
        /// 调用的参数
        /// </summary>
        public string Parameters { get; set; }

        /// <summary>
        /// Start time of the method execution.
        /// 方法的开始执行时间
        /// </summary>
        public DateTime ExecutionTime { get; set; }

        /// <summary>
        /// Total duration of the method call.
        /// 方法的执行时长
        /// </summary>
        public int ExecutionDuration { get; set; }

        /// <summary>
        /// IP address of the client.
        /// 服务端的IP地址
        /// </summary>
        public string ClientIpAddress { get; set; }

        /// <summary>
        /// Name (generally computer name) of the client.
        /// 客户端名称 (一般为电脑名称）
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// Browser information if this method is called in a web request.
        /// 浏览器信息（如果是一个web请求）
        /// </summary>
        public string BrowserInfo { get; set; }

        /// <summary>
        /// Optional custom data that can be filled and used.
        /// 可选的自定义数据，可以填充和使用
        /// </summary>
        public string CustomData { get; set; }

        /// <summary>
        /// Exception object, if an exception occurred during execution of the method.
        /// 异常对象(如果执行期间发生异常）
        /// </summary>
        public Exception Exception { get; set; }

        public override string ToString()
        {
            var loggedUserId = UserId.HasValue
                                   ? "user " + UserId.Value
                                   : "an anonymous user";

            var exceptionOrSuccessMessage = Exception != null
                ? "exception: " + Exception.Message
                : "succeed";

            return $"AUDIT LOG: {ServiceName}.{MethodName} is executed by {loggedUserId} in {ExecutionDuration} ms from {ClientIpAddress} IP address with {exceptionOrSuccessMessage}.";
        }
    }
}