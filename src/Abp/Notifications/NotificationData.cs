using System;
using System.Collections.Generic;
using Abp.Json;

namespace Abp.Notifications
{
    /// <summary>
    /// Used to store data for a notification.It can be directly used or can be derived.
    /// 用于存储通知数据，它可以直接使用或用于派生
    /// </summary>
    [Serializable]
    public class NotificationData
    {
        /// <summary>
        /// Gets notification data type name.It returns the full class name by default.
        /// 根据类型名称获取通知数据，它返回默认的全类名
        /// </summary>
        public virtual string Type
        {
            get { return GetType().FullName; }
        }

        /// <summary>
        /// Shortcut to set/get <see cref="Properties"/>.
        /// 获取/设置<see cref="Properties"/>的快捷方式
        /// </summary>
        public object this[string key]
        {
            get { return Properties[key]; }
            set { Properties[key] = value; }
        }

        /// <summary>
        /// Can be used to add custom properties to this notification.
        /// 可以用于为当前通知添加自定义属性
        /// </summary>
        public Dictionary<string, object> Properties
        {
            get { return _properties; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                _properties = value;
            }
        }
        private Dictionary<string, object> _properties;

        /// <summary>
        /// Createa a new NotificationData object.
        /// 创建一个新的通知数据对象
        /// </summary>
        public NotificationData()
        {
            Properties = new Dictionary<string, object>();
        }

        public override string ToString()
        {
            return this.ToJsonString();
        }
    }
}