using Castle.Facilities.Logging;

namespace Abp.Castle.Logging.Log4Net
{
    /// <summary>
    /// 日志支持设施<see cref="LoggingFacility"/>扩展方法。
    /// </summary>
    public static class LoggingFacilityExtensions
    {
        public static LoggingFacility UseAbpLog4Net(this LoggingFacility loggingFacility)
        {
            return loggingFacility.LogUsing<Log4NetLoggerFactory>();
        }
    }
}