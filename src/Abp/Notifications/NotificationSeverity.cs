using System;

namespace Abp.Notifications
{
    /// <summary>
    /// 通知严重程度
    /// </summary>
    [Serializable]
    public enum NotificationSeverity : byte
    {
        /// <summary>
        /// 信息
        /// </summary>
        Info = 0,
        
        /// <summary>
        /// 成功
        /// </summary>
        Success = 1,
        
        /// <summary>
        /// 警告
        /// </summary>
        Warn = 2,
        
        /// <summary>
        /// 错误
        /// </summary>
        Error = 3,

        /// <summary>
        /// 致命
        /// </summary>
        Fatal = 4
    }
}