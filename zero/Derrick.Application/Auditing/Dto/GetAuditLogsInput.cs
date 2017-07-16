using System;
using Abp.Extensions;
using Abp.Runtime.Validation;
using Abp.Timing;
using Derrick.Dto;

namespace Derrick.Auditing.Dto
{
    /// <summary>
    /// 审计日志输入实体
    /// </summary>
    public class GetAuditLogsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 服务名
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// 方法名
        /// </summary>
        public string MethodName { get; set; }
        /// <summary>
        /// 浏览器信息
        /// </summary>
        public string BrowserInfo { get; set; }
        /// <summary>
        /// 是否有异常
        /// </summary>
        public bool? HasException { get; set; }
        /// <summary>
        /// 最小执行时间
        /// </summary>
        public int? MinExecutionDuration { get; set; }
        /// <summary>
        /// 最大执行时间
        /// </summary>
        public int? MaxExecutionDuration { get; set; }

        /// <summary>
        /// 规范
        /// </summary>
        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "ExecutionTime DESC";
            }

            if (Sorting.IndexOf("UserName", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                Sorting = "User." + Sorting;
            }
            else
            {
                Sorting = "AuditLog." + Sorting;
            }
        }
    }
}