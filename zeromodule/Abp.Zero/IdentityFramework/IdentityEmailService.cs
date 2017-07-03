using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Net.Mail;
using Microsoft.AspNet.Identity;

namespace Abp.IdentityFramework
{
    /// <summary>
    /// Identity邮箱消息服务
    /// </summary>
    public class IdentityEmailMessageService : IIdentityMessageService, ITransientDependency
    {
        /// <summary>
        /// 邮件发送者引用
        /// </summary>
        private readonly IEmailSender _emailSender;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="emailSender">邮件发送对象</param>
        public IdentityEmailMessageService(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="message">身份消息</param>
        /// <returns></returns>
        public virtual Task SendAsync(IdentityMessage message)
        {
            return _emailSender.SendAsync(message.Destination, message.Subject, message.Body);
        }
    }
}
