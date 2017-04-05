namespace Abp.BackgroundJobs
{
    /// <summary>
    /// Priority of a background job.
    /// 后台作业的优先级
    /// </summary>
    public enum BackgroundJobPriority : byte
    {
        /// <summary>
        /// Low.
        /// 低
        /// </summary>
        Low = 5,

        /// <summary>
        /// Below normal.
        /// 低于正常
        /// </summary>
        BelowNormal = 10,

        /// <summary>
        /// Normal (default).
        /// 正常(默认)
        /// </summary>
        Normal = 15,

        /// <summary>
        /// Above normal.
        /// 高于正常
        /// </summary>
        AboveNormal = 20,

        /// <summary>
        /// High.
        /// 高
        /// </summary>
        High = 25
    }
}