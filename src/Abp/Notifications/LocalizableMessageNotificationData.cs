using System;
using Abp.Localization;

namespace Abp.Notifications
{
    /// <summary>
    /// Can be used to store a simple message as notification data.
    /// 可以用来存储一个简单消息用作通知数据
    /// </summary>
    [Serializable]
    public class LocalizableMessageNotificationData : NotificationData
    {
        /// <summary>
        /// The message.
        /// 消息
        /// </summary>
        public LocalizableString Message { get; private set; }

        /// <summary>
        /// Needed for serialization.
        /// 为序列化
        /// </summary>
        private LocalizableMessageNotificationData()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizableMessageNotificationData"/> class.
        /// 初始化 <see cref="LocalizableMessageNotificationData"/> 类一个新的实例
        /// </summary>
        /// <param name="message">The message.</param>
        public LocalizableMessageNotificationData(LocalizableString message)
        {
            Message = message;
        }
    }
}