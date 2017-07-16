using System;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.AutoMapper;

namespace Derrick.Auditing.Dto
{
    /// <summary>
    /// 审计日志列表Dto
    /// </summary>
    [AutoMapFrom(typeof(AuditLog))]
    public class AuditLogListDto : EntityDto<long>
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long? UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 模拟商户ID
        /// </summary>
        public int? ImpersonatorTenantId { get; set; }
        /// <summary>
        /// 模拟用户ID
        /// </summary>
        public long? ImpersonatorUserId { get; set; }
        /// <summary>
        /// 服务名
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// 方法名
        /// </summary>
        public string MethodName { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        public string Parameters { get; set; }
        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime ExecutionTime { get; set; }
        /// <summary>
        /// 执行时间
        /// </summary>
        public int ExecutionDuration { get; set; }
        /// <summary>
        /// 客户端IP
        /// </summary>
        public string ClientIpAddress { get; set; }
        /// <summary>
        /// 客户端名称
        /// </summary>
        public string ClientName { get; set; }
        /// <summary>
        /// 浏览器信息
        /// </summary>
        public string BrowserInfo { get; set; }
        /// <summary>
        /// 异常信息
        /// </summary>
        public string Exception { get; set; }
        /// <summary>
        /// 自定义数据
        /// </summary>
        public string CustomData { get; set; }
    }
}