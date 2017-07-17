using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Abp.IO;

namespace Derrick.IO
{
    /// <summary>
    /// APP文件帮助类
    /// </summary>
    public static class AppFileHelper
    {
        /// <summary>
        /// 读取行
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static IEnumerable<string> ReadLines(string path)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 0x1000, FileOptions.SequentialScan))
            using (var sr = new StreamReader(fs, Encoding.UTF8))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }
        /// <summary>
        /// 删除存在文件夹内的文件
        /// </summary>
        /// <param name="folderPath">文件夹路径</param>
        /// <param name="fileNameWithoutExtension">带扩展名的文件名</param>
        public static void DeleteFilesInFolderIfExists(string folderPath, string fileNameWithoutExtension)
        {
            var directory = new DirectoryInfo(folderPath);
            var tempUserProfileImages = directory.GetFiles(fileNameWithoutExtension + ".*", SearchOption.AllDirectories).ToList();
            foreach (var tempUserProfileImage in tempUserProfileImages)
            {
                FileHelper.DeleteIfExists(tempUserProfileImage.FullName);
            }
        }
    }
}
