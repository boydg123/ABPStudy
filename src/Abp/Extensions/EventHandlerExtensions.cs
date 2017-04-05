using System;

namespace Abp.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="EventHandler"/>.
    /// <see cref="EventHandler"/>扩展方法.
    /// </summary>
    public static class EventHandlerExtensions
    {
        /// <summary>
        /// Raises given event safely with given arguments.
        /// 使用给定的参数，安全激发给定的事件
        /// </summary>
        /// <param name="eventHandler">The event handler / 事件句柄(事件处理函数）</param>
        /// <param name="sender">Source of the event / 事件源</param>
        public static void InvokeSafely(this EventHandler eventHandler, object sender)
        {
            eventHandler.InvokeSafely(sender, EventArgs.Empty);
        }

        /// <summary>
        /// Raises given event safely with given arguments.
        /// 使用给定的参数，安全激发给定的事件
        /// </summary>
        /// <param name="eventHandler">The event handler / 事件句柄(事件处理函数）</param>
        /// <param name="sender">Source of the event / 事件源</param>
        /// <param name="e">Event argument / 事件参数</param>
        public static void InvokeSafely(this EventHandler eventHandler, object sender, EventArgs e)
        {
            if (eventHandler == null)
            {
                return;
            }

            eventHandler(sender, e);
        }

        /// <summary>
        /// Raises given event safely with given arguments.
        /// 使用给定的参数，安全激发给定的事件
        /// </summary>
        /// <typeparam name="TEventArgs">Type of the <see cref="EventArgs"/> / <see cref="EventArgs"/>的类型</typeparam>
        /// <param name="eventHandler">The event handler / 事件句柄(事件处理函数）</param>
        /// <param name="sender">Source of the event / 事件源</param>
        /// <param name="e">Event argument / 事件参数</param>
        public static void InvokeSafely<TEventArgs>(this EventHandler<TEventArgs> eventHandler, object sender, TEventArgs e)
            where TEventArgs : EventArgs
        {
            if (eventHandler == null)
            {
                return;
            }

            eventHandler(sender, e);
        }
    }
}