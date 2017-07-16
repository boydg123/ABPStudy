using System;
using System.IO;

namespace Derrick
{
    /// <summary>
    /// Central point for application version.
    /// 应用程序的版本中心点
    /// </summary>
    public class AppVersionHelper
    {
        /// <summary>
        /// Gets current version of the application.All project's assembly versions are changed when this value is changed.
        /// 获取当前应用程序的版本。所有项目程序集版本该表当这个值改变的时候
        /// It's also shown in the web page.
        /// 它也应该显示在网页中
        /// </summary>
        public const string Version = "2.2.0.0";

        /// <summary>
        /// Gets release (last build) date of the application.It's shown in the web page.
        /// 获取应用程序的发布（最后一次生成）日期。
        /// </summary>
        public static DateTime ReleaseDate
        {
            get { return new FileInfo(typeof(AppVersionHelper).Assembly.Location).LastWriteTime; }
        }
    }
}