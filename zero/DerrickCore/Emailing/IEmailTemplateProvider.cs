namespace Derrick.Emailing
{
    /// <summary>
    /// 邮件模版提供者接口
    /// </summary>
    public interface IEmailTemplateProvider
    {
        /// <summary>
        /// 获取邮件模版
        /// </summary>
        /// <returns></returns>
        string GetDefaultTemplate();
    }
}
