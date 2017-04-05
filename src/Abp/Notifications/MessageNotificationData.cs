using System;

namespace Abp.Notifications
{
    /// <summary>
    /// Can be used to store a simple message as notification data.
    /// 可以用于存储一个简单消息用作通知数据
    /// </summary>
    [Serializable]
    public class MessageNotificationData : NotificationData
    {
        /// <summary>
        /// The message.
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Needed for serialization.
        /// 为序列化
        /// </summary>
        private MessageNotificationData()
        {
            
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message"></param>
        public MessageNotificationData(string message)
        {
            Message = message;
        }
    }
}