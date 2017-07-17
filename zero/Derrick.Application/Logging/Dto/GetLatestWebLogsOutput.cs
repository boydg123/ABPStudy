using System.Collections.Generic;

namespace Derrick.Logging.Dto
{
    /// <summary>
    /// 最新的web日志Output
    /// </summary>
    public class GetLatestWebLogsOutput
    {
        /// <summary>
        /// 最新的web日志行
        /// </summary>
        public List<string> LatesWebLogLines { get; set; }
    }
}
