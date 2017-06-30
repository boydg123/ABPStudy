using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Extensions;

namespace Abp.Auditing
{
    /// <summary>
    /// 用于存储审计日志
    /// </summary>
    [Table("AbpAuditLogs")]
    public class AuditLog : Entity<long>, IMayHaveTenant
    {
        /// <summary>
        /// <see cref="ServiceName"/>属性的最大长度
        /// </summary>
        public const int MaxServiceNameLength = 256;

        /// <summary>
        /// <see cref="MethodName"/>属性的最大长度
        /// </summary>
        public const int MaxMethodNameLength = 256;

        /// <summary>
        /// <see cref="Parameters"/>属性的最大长度
        /// </summary>
        public const int MaxParametersLength = 1024;

        /// <summary>
        /// <see cref="ClientIpAddress"/>属性的最大长度
        /// </summary>
        public const int MaxClientIpAddressLength = 64;

        /// <summary>
        /// <see cref="ClientName"/>属性的最大长度
        /// </summary>
        public const int MaxClientNameLength = 128;

        /// <summary>
        /// <see cref="BrowserInfo"/>属性的最大长度
        /// </summary>
        public const int MaxBrowserInfoLength = 256;

        /// <summary>
        /// <see cref="Exception"/>属性的最大长度
        /// </summary>
        public const int MaxExceptionLength = 2000;

        /// <summary>
        /// <see cref="CustomData"/>属性的最大长度
        /// </summary>
        public const int MaxCustomDataLength = 2000;

        /// <summary>
        /// 租户ID
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public virtual long? UserId { get; set; }

        /// <summary>
        /// 服务(类/接口)名称
        /// </summary>
        [MaxLength(MaxServiceNameLength)]
        public virtual string ServiceName { get; set; }

        /// <summary>
        /// 执行的方法名称
        /// </summary>
        [MaxLength(MaxMethodNameLength)]
        public virtual string MethodName { get; set; }

        /// <summary>
        /// 调用参数
        /// </summary>
        [MaxLength(MaxParametersLength)]
        public virtual string Parameters { get; set; }

        /// <summary>
        /// 方法执行的开始时间
        /// </summary>
        public virtual DateTime ExecutionTime { get; set; }

        /// <summary>
        /// 方法调用的总持续时间(毫秒)
        /// </summary>
        public virtual int ExecutionDuration { get; set; }

        /// <summary>
        /// 客户端IP地址
        /// </summary>
        [MaxLength(MaxClientIpAddressLength)]
        public virtual string ClientIpAddress { get; set; }

        /// <summary>
        /// 客户机器名称(一般是电脑名称)
        /// </summary>
        [MaxLength(MaxClientNameLength)]
        public virtual string ClientName { get; set; }

        /// <summary>
        /// Web请求中调用此方法，浏览器信息
        /// </summary>
        [MaxLength(MaxBrowserInfoLength)]
        public virtual string BrowserInfo { get; set; }

        /// <summary>
        /// 如果在执行过程中出现异常，则为异常对象
        /// </summary>
        [MaxLength(MaxExceptionLength)]
        public virtual string Exception { get; set; }

        /// <summary>
        /// <see cref="AuditInfo.ImpersonatorUserId"/>.
        /// </summary>
        public virtual long? ImpersonatorUserId { get; set; }

        /// <summary>
        /// <see cref="AuditInfo.ImpersonatorTenantId"/>.
        /// </summary>
        public virtual int? ImpersonatorTenantId { get; set; }

        /// <summary>
        /// <see cref="AuditInfo.CustomData"/>.
        /// </summary>
        [MaxLength(MaxCustomDataLength)]
        public virtual string CustomData { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="auditInfo">源 <see cref="AuditInfo"/> 对象</param>
        /// <returns>使用<see cref="auditInfo"/>创建的<see cref="AuditLog"/>对象</returns>
        public static AuditLog CreateFromAuditInfo(AuditInfo auditInfo)
        {
            var exceptionMessage = auditInfo.Exception != null ? auditInfo.Exception.ToString() : null;
            return new AuditLog
            {
                TenantId = auditInfo.TenantId,
                UserId = auditInfo.UserId,
                ServiceName = auditInfo.ServiceName.TruncateWithPostfix(MaxServiceNameLength),
                MethodName = auditInfo.MethodName.TruncateWithPostfix(MaxMethodNameLength),
                Parameters = auditInfo.Parameters.TruncateWithPostfix(MaxParametersLength),
                ExecutionTime = auditInfo.ExecutionTime,
                ExecutionDuration = auditInfo.ExecutionDuration,
                ClientIpAddress = auditInfo.ClientIpAddress.TruncateWithPostfix(MaxClientIpAddressLength),
                ClientName = auditInfo.ClientName.TruncateWithPostfix(MaxClientNameLength),
                BrowserInfo = auditInfo.BrowserInfo.TruncateWithPostfix(MaxBrowserInfoLength),
                Exception = exceptionMessage.TruncateWithPostfix(MaxExceptionLength),
                ImpersonatorUserId = auditInfo.ImpersonatorUserId,
                ImpersonatorTenantId = auditInfo.ImpersonatorTenantId,
                CustomData = auditInfo.CustomData.TruncateWithPostfix(MaxCustomDataLength)
            };
        }

        public override string ToString()
        {
            return string.Format(
                "AUDIT LOG: {0}.{1} is executed by user {2} in {3} ms from {4} IP address.",
                ServiceName, MethodName, UserId, ExecutionDuration, ClientIpAddress
                );
        }
    }
}
