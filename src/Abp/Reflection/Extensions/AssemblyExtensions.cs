using System.IO;
using System.Reflection;

namespace Abp.Reflection.Extensions
{
    /// <summary>
    /// 程序集的扩展方法
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Gets directory path of given assembly or returns null if can not find.
        /// 获取指定程序集的路径,如果没找到则返回<see cref="null"/>
        /// </summary>
        /// <param name="assembly">The assembly. / 程序集</param>
        public static string GetDirectoryPathOrNull(this Assembly assembly)
        {
            var location = assembly.Location;
            if (location == null)
            {
                return null;
            }

            var directory = new FileInfo(location).Directory;
            if (directory == null)
            {
                return null;
            }

            return directory.FullName;
        }
    }
}
