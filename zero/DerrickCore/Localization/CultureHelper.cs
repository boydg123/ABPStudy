using System.Threading;

namespace Derrick.Localization
{
    /// <summary>
    /// 区域帮助类
    /// </summary>
    public static class CultureHelper
    {
        /// <summary>
        /// 获取一个值，该值指示当前 System.Globalization.TextInfo 对象是否表示一个文本书写方向为从右到左的书写体系。
        /// 如果文本书写方向为从右到左，则为 true；否则为 false。
        /// </summary>
        public static bool IsRtl
        {
            get { return Thread.CurrentThread.CurrentUICulture.TextInfo.IsRightToLeft; }
        }
    }
}
