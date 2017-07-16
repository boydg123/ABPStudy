namespace Derrick
{
    /// <summary>
    /// APP文件夹接口
    /// </summary>
    public interface IAppFolders
    {
        /// <summary>
        /// 临时文件下载文件夹
        /// </summary>
        string TempFileDownloadFolder { get; }
        /// <summary>
        /// 实例图片文件夹
        /// </summary>
        string SampleProfileImagesFolder { get; }
        /// <summary>
        /// web日志文件夹
        /// </summary>
        string WebLogsFolder { get; set; }
    }
}