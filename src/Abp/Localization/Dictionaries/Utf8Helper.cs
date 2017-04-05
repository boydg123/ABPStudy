using System.IO;
using System.Text;
using Abp.IO.Extensions;

namespace Abp.Localization.Dictionaries
{
    /// <summary>
    /// UTF8帮助类
    /// </summary>
    internal static class Utf8Helper
    {
        /// <summary>
        /// 从流中读取字符串
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string ReadStringFromStream(Stream stream)
        {
            var bytes = stream.GetAllBytes();
            var skipCount = HasBom(bytes) ? 3 : 0;
            return Encoding.UTF8.GetString(bytes, skipCount, bytes.Length - skipCount);
        }

        /// <summary>
        /// 是否有bom
        /// </summary>
        /// <param name="bytes">字节</param>
        /// <returns></returns>
        private static bool HasBom(byte[] bytes)
        {
            if (bytes.Length < 3)
            {
                return false;
            }

            if (!(bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF))
            {
                return false;
            }

            return true;
        }
    }
}
