namespace Derrick.Debugging
{
    /// <summary>
    /// Debug帮助累
    /// </summary>
    public static class DebugHelper
    {
        /// <summary>
        /// 是否是Debug
        /// </summary>
        public static bool IsDebug
        {
            get
            {
#pragma warning disable
#if DEBUG
                return true;
#endif
                return false;
#pragma warning restore
            }
        }
    }
}
