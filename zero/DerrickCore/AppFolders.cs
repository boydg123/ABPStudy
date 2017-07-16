using Abp.Dependency;

namespace Derrick
{
    /// <summary>
    /// <see cref="IAppFolders"/>实现，APP文件夹处理
    /// </summary>
    public class AppFolders : IAppFolders, ISingletonDependency
    {
        /// <summary>
        /// 临时文件下载文件夹
        /// </summary>
        public string TempFileDownloadFolder { get; set; }
        /// <summary>
        /// 实例图片文件夹
        /// </summary>
        public string SampleProfileImagesFolder { get; set; }
        /// <summary>
        /// web日志文件夹
        /// </summary>
        public string WebLogsFolder { get; set; }
    }
}