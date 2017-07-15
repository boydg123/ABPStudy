using System.Reflection;
using System.Text;
using Abp.Dependency;
using Abp.IO.Extensions;

namespace Derrick.Emailing
{
    /// <summary>
    /// <see cref="IEmailTemplateProvider"/>实现。提供邮件模版
    /// </summary>
    public class EmailTemplateProvider : IEmailTemplateProvider, ITransientDependency
    {
        /// <summary>
        /// 获取默认模版
        /// </summary>
        /// <returns></returns>
        public string GetDefaultTemplate()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Derrick.Emailing.EmailTemplates.default.html"))
            {
                var bytes = stream.GetAllBytes();
                return Encoding.UTF8.GetString(bytes, 3, bytes.Length - 3);
            }
        }
    }
}