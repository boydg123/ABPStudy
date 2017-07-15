using System.Threading.Tasks;
using Abp.Dependency;
using Castle.Core.Logging;
using Microsoft.AspNet.Identity;

namespace Derrick.Identity
{
    /// <summary>
    /// 身份短信服务
    /// </summary>
    public class IdentitySmsMessageService : IIdentityMessageService, ITransientDependency
    {
        /// <summary>
        /// 日志引用
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public IdentitySmsMessageService()
        {
            Logger = NullLogger.Instance;
        }

        /// <summary>
        /// 发送身份消息
        /// </summary>
        /// <param name="message">身份消息</param>
        /// <returns></returns>
        public Task SendAsync(IdentityMessage message)
        {
            //TODO: Implement this service to send SMS to users. This is used by UserManager (ASP.NET Identity) on two factor auth.

            Logger.Warn("Sending SMS is not implemented! Message content:");
            Logger.Warn("Destination : " + message.Destination);
            Logger.Warn("Subject     : " + message.Subject);
            Logger.Warn("Body        : " + message.Body);

            return Task.FromResult(0);
        }
    }
}
