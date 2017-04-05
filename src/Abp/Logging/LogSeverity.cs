namespace Abp.Logging
{
    /// <summary>
    /// Indicates severity for log.
    /// 日志级别
    /// </summary>
    public enum LogSeverity
    {
        /// <summary>
        /// Debug.
        /// 表示调试的日志级别
        /// </summary>
        Debug,

        /// <summary>
        /// Info.
        /// 表示消息的日志级别
        /// </summary>
        Info,

        /// <summary>
        /// Warn.
        /// 表示警告的日志级别
        /// </summary>
        Warn,

        /// <summary>
        /// Error.
        /// 表示错误的日志级别
        /// </summary>
        Error,

        /// <summary>
        /// Fatal.
        /// 表示严重错误的日志级别
        /// </summary>
        Fatal
    }
}