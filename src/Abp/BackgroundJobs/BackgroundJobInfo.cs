using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.MultiTenancy;
using Abp.Timing;

namespace Abp.BackgroundJobs
{
    /// <summary>
    /// Represents a background job info that is used to persist jobs.
    /// 表示用于继续工作的后台作业信息
    /// </summary>
    [Table("AbpBackgroundJobs")]
    [MultiTenancySide(MultiTenancySides.Host)]
    public class BackgroundJobInfo : CreationAuditedEntity<long>
    {
        /// <summary>
        /// Maximum length of <see cref="JobType"/>.Value: 512.
        /// <see cref="JobType"/>的最大长度。值:512
        /// </summary>
        public const int MaxJobTypeLength = 512;

        /// <summary>
        /// Maximum length of <see cref="JobArgs"/>.Value: 1 MB (1,048,576 bytes).
        /// <see cref="JobArgs"/>的最大长度。Value: 1 MB (1,048,576 bytes).
        /// </summary>
        public const int MaxJobArgsLength = 1024 * 1024;

        /// <summary>
        /// Default duration (as seconds) for the first wait on a failure.Default value: 60 (1 minutes).
        /// 故障的第一次等待的默认持续时间（如秒）。默认值：60(1分钟)
        /// </summary>
        public static int DefaultFirstWaitDuration { get; set; }

        /// <summary>
        /// Default timeout value (as seconds) for a job before it's abandoned (<see cref="IsAbandoned"/>).Default value: 172,800 (2 days).
        /// 作业被放弃(<see cref="IsAbandoned"/>)前的默认超时时间(秒).默认值：172,800(2天)
        /// </summary>
        public static int DefaultTimeout { get; set; }

        /// <summary>
        /// Default wait factor for execution failures.
        /// 执行失败的默认等待因子
        /// This amount is multiplated by last wait time to calculate next wait time.
        /// 这个数量是通过多次最后等待时间来估算下一次时间
        /// Default value: 2.0.
        /// </summary>
        public static double DefaultWaitFactor { get; set; }

        /// <summary>
        /// Type of the job.It's AssemblyQualifiedName of job type.
        /// 作业的类型，作业类型的程序集名称
        /// </summary>
        [Required]
        [StringLength(MaxJobTypeLength)]
        public virtual string JobType { get; set; }

        /// <summary>
        /// Job arguments as JSON string.
        /// 作业参数(JSON字符串)
        /// </summary>
        [Required]
        [MaxLength(MaxJobArgsLength)]
        public virtual string JobArgs { get; set; }

        /// <summary>
        /// Try count of this job.A job is re-tried if it fails.
        /// 作业尝试执行的次数，作业如果执行失败一直重新执行
        /// </summary>
        public virtual short TryCount { get; set; }

        /// <summary>
        /// Next try time of this job.
        /// 作业下次尝试执行的时间
        /// </summary>
        //[Index("IX_IsAbandoned_NextTryTime", 2)]
        public virtual DateTime NextTryTime { get; set; }

        /// <summary>
        /// Last try time of this job.
        /// 作业最后尝试执行的时间
        /// </summary>
        public virtual DateTime? LastTryTime { get; set; }

        /// <summary>
        /// This is true if this job is continously failed and will not be executed again.
        /// 这个为Ture，如果这个作业执行失败将不会再执行
        /// </summary>
        //[Index("IX_IsAbandoned_NextTryTime", 1)]
        public virtual bool IsAbandoned { get; set; }

        /// <summary>
        /// Priority of this job.
        /// 作业的属性
        /// </summary>
        public virtual BackgroundJobPriority Priority { get; set; }

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static BackgroundJobInfo()
        {
            DefaultFirstWaitDuration = 60;
            DefaultTimeout = 172800;
            DefaultWaitFactor = 2.0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundJobInfo"/> class.
        /// 构造函数(初始化<see cref="BackgroundJobInfo"/>类新的实例)
        /// </summary>
        public BackgroundJobInfo()
        {
            NextTryTime = Clock.Now;
            Priority = BackgroundJobPriority.Normal;
        }

        /// <summary>
        /// Calculates next try time if a job fails.
        /// 如果作业失败则计算下一次执行事件
        /// Returns null if it will not wait anymore and job should be abandoned.
        /// 如果它不会再等待和工作被放弃将返回NULL
        /// </summary>
        /// <returns></returns>
        protected internal virtual DateTime? CalculateNextTryTime()
        {
            var nextWaitDuration = DefaultFirstWaitDuration * (Math.Pow(DefaultWaitFactor, TryCount - 1));
            var nextTryDate = LastTryTime.HasValue
                ? LastTryTime.Value.AddSeconds(nextWaitDuration)
                : Clock.Now.AddSeconds(nextWaitDuration);

            if (nextTryDate.Subtract(CreationTime).TotalSeconds > DefaultTimeout)
            {
                return null;
            }

            return nextTryDate;
        }
    }
}