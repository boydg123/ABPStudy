using System;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Net.Mail;
using Abp.Runtime.Security;
using Derrick.Chat;
using Derrick.Emailing;
using Derrick.MultiTenancy;
using Derrick.Web;

namespace Derrick.Authorization.Users
{
    /// <summary>
    /// 用于给用户发送邮件
    /// </summary>
    public class UserEmailer : AbpZeroTemplateServiceBase, IUserEmailer, ITransientDependency
    {
        /// <summary>
        /// 邮件模版提供者
        /// </summary>
        private readonly IEmailTemplateProvider _emailTemplateProvider;
        /// <summary>
        /// 邮件发送者
        /// </summary>
        private readonly IEmailSender _emailSender;
        /// <summary>
        /// Web Url 服务
        /// </summary>
        private readonly IWebUrlService _webUrlService;
        /// <summary>
        /// 商户仓储
        /// </summary>
        private readonly IRepository<Tenant> _tenantRepository;
        /// <summary>
        /// 当前工作单元提供者
        /// </summary>
        private readonly ICurrentUnitOfWorkProvider _unitOfWorkProvider;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="emailTemplateProvider">邮件模版提供者</param>
        /// <param name="emailSender">邮件发送者</param>
        /// <param name="webUrlService">Web Url 服务</param>
        /// <param name="tenantRepository">商户仓储</param>
        /// <param name="unitOfWorkProvider">当前工作单元提供者</param>
        public UserEmailer(IEmailTemplateProvider emailTemplateProvider,
            IEmailSender emailSender,
            IWebUrlService webUrlService,
            IRepository<Tenant> tenantRepository,
            ICurrentUnitOfWorkProvider unitOfWorkProvider)
        {
            _emailTemplateProvider = emailTemplateProvider;
            _emailSender = emailSender;
            _webUrlService = webUrlService;
            _tenantRepository = tenantRepository;
            _unitOfWorkProvider = unitOfWorkProvider;
        }

        /// <summary>
        /// 发送电子邮件激活链接到用户的邮箱
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="plainPassword">
        /// 可以设置为用户的普通密码用以包含在电子邮件中.
        /// </param>
        [UnitOfWork]
        public virtual async Task SendEmailActivationLinkAsync(User user, string plainPassword = null)
        {
            if (user.EmailConfirmationCode.IsNullOrEmpty())
            {
                throw new ApplicationException("EmailConfirmationCode should be set in order to send email activation link.");
            }

            var tenancyName = GetTenancyNameOrNull(user.TenantId);

            var link = _webUrlService.GetSiteRootAddress(tenancyName) + "Account/EmailConfirmation" +
                       "?userId=" + Uri.EscapeDataString(SimpleStringCipher.Instance.Encrypt(user.Id.ToString())) +
                       "&tenantId=" + (user.TenantId == null ? "" : Uri.EscapeDataString(SimpleStringCipher.Instance.Encrypt(user.TenantId.Value.ToString()))) +
                       "&confirmationCode=" + Uri.EscapeDataString(user.EmailConfirmationCode);

            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetDefaultTemplate());
            emailTemplate.Replace("{EMAIL_TITLE}", L("EmailActivation_Title"));
            emailTemplate.Replace("{EMAIL_SUB_TITLE}", L("EmailActivation_SubTitle"));

            var mailMessage = new StringBuilder();

            mailMessage.AppendLine("<b>" + L("NameSurname") + "</b>: " + user.Name + " " + user.Surname + "<br />");

            if (!tenancyName.IsNullOrEmpty())
            {
                mailMessage.AppendLine("<b>" + L("TenancyName") + "</b>: " + tenancyName + "<br />");
            }

            mailMessage.AppendLine("<b>" + L("UserName") + "</b>: " + user.UserName + "<br />");

            if (!plainPassword.IsNullOrEmpty())
            {
                mailMessage.AppendLine("<b>" + L("Password") + "</b>: " + plainPassword + "<br />");
            }

            mailMessage.AppendLine("<br />");
            mailMessage.AppendLine(L("EmailActivation_ClickTheLinkBelowToVerifyYourEmail") + "<br /><br />");
            mailMessage.AppendLine("<a href=\"" + link + "\">" + link + "</a>");

            emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());

            await _emailSender.SendAsync(user.EmailAddress, L("EmailActivation_Subject"), emailTemplate.ToString());
        }

        /// <summary>
        /// 发送一个密码重置链接到用户的邮箱.
        /// </summary>
        /// <param name="user">用户</param>
        public async Task SendPasswordResetLinkAsync(User user)
        {
            if (user.PasswordResetCode.IsNullOrEmpty())
            {
                throw new ApplicationException("PasswordResetCode should be set in order to send password reset link.");
            }

            var tenancyName = GetTenancyNameOrNull(user.TenantId);

            var link = _webUrlService.GetSiteRootAddress(tenancyName) + "Account/ResetPassword" +
                       "?userId=" + Uri.EscapeDataString(SimpleStringCipher.Instance.Encrypt(user.Id.ToString())) +
                       "&tenantId=" + (user.TenantId == null ? "" : Uri.EscapeDataString(SimpleStringCipher.Instance.Encrypt(user.TenantId.Value.ToString()))) +
                       "&resetCode=" + Uri.EscapeDataString(user.PasswordResetCode);

            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetDefaultTemplate());
            emailTemplate.Replace("{EMAIL_TITLE}", L("PasswordResetEmail_Title"));
            emailTemplate.Replace("{EMAIL_SUB_TITLE}", L("PasswordResetEmail_SubTitle"));

            var mailMessage = new StringBuilder();

            mailMessage.AppendLine("<b>" + L("NameSurname") + "</b>: " + user.Name + " " + user.Surname + "<br />");

            if (!tenancyName.IsNullOrEmpty())
            {
                mailMessage.AppendLine("<b>" + L("TenancyName") + "</b>: " + tenancyName + "<br />");
            }

            mailMessage.AppendLine("<b>" + L("UserName") + "</b>: " + user.UserName + "<br />");

            mailMessage.AppendLine("<br />");
            mailMessage.AppendLine(L("PasswordResetEmail_ClickTheLinkBelowToResetYourPassword") + "<br /><br />");
            mailMessage.AppendLine("<a href=\"" + link + "\">" + link + "</a>");

            emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());

            await _emailSender.SendAsync(user.EmailAddress, L("PasswordResetEmail_Subject"), emailTemplate.ToString());
        }

        /// <summary>
        /// 发送一个未读的聊天信息电子邮件到用户邮箱
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="senderUsername">发送者用户名</param>
        /// <param name="senderTenancyName">发送者商户名</param>
        /// <param name="chatMessage">聊天信息</param>
        public void TryToSendChatMessageMail(User user, string senderUsername, string senderTenancyName, ChatMessage chatMessage)
        {
            try
            {
                var emailTemplate = new StringBuilder(_emailTemplateProvider.GetDefaultTemplate());
                emailTemplate.Replace("{EMAIL_TITLE}", L("NewChatMessageEmail_Title"));
                emailTemplate.Replace("{EMAIL_SUB_TITLE}", L("NewChatMessageEmail_SubTitle"));

                var mailMessage = new StringBuilder();
                mailMessage.AppendLine("<b>" + L("Sender") + "</b>: " + senderTenancyName + "/" + senderUsername + "<br />");
                mailMessage.AppendLine("<b>" + L("Time") + "</b>: " + chatMessage.CreationTime.ToString("yyyy-MM-dd HH:mm:ss") + "<br />");
                mailMessage.AppendLine("<b>" + L("Message") + "</b>: " + chatMessage.Message + "<br />");
                mailMessage.AppendLine("<br />");

                emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());

                _emailSender.Send(user.EmailAddress, L("NewChatMessageEmail_Subject"), emailTemplate.ToString());
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, exception);
            }
        }
        /// <summary>
        /// 获取商户名或Null
        /// </summary>
        /// <param name="tenantId">商户ID</param>
        /// <returns></returns>
        private string GetTenancyNameOrNull(int? tenantId)
        {
            if (tenantId == null)
            {
                return null;
            }

            using (_unitOfWorkProvider.Current.SetTenantId(null))
            {
                return _tenantRepository.Get(tenantId.Value).TenancyName;
            }
        }
    }
}