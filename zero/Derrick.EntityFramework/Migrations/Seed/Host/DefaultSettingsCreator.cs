using System.Linq;
using Abp.Configuration;
using Abp.Localization;
using Abp.Net.Mail;
using Derrick.EntityFramework;

namespace Derrick.Migrations.Seed.Host
{
    /// <summary>
    /// 默认设置创造器
    /// </summary>
    public class DefaultSettingsCreator
    {
        /// <summary>
        /// DB上下文
        /// </summary>
        private readonly AbpZeroTemplateDbContext _context;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context">DB上下文</param>
        public DefaultSettingsCreator(AbpZeroTemplateDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 创建
        /// </summary>
        public void Create()
        {
            //Emailing
            AddSettingIfNotExists(EmailSettingNames.DefaultFromAddress, "admin@mydomain.com");
            AddSettingIfNotExists(EmailSettingNames.DefaultFromDisplayName, "mydomain.com mailer");

            //Languages
            AddSettingIfNotExists(LocalizationSettingNames.DefaultLanguage, "en");
        }
        /// <summary>
        /// 添加不存在的设置
        /// </summary>
        /// <param name="name">设置名称</param>
        /// <param name="value">设置值</param>
        /// <param name="tenantId">商户ID</param>
        private void AddSettingIfNotExists(string name, string value, int? tenantId = null)
        {
            if (_context.Settings.Any(s => s.Name == name && s.TenantId == tenantId && s.UserId == null))
            {
                return;
            }

            _context.Settings.Add(new Setting(tenantId, null, name, value));
            _context.SaveChanges();
        }
    }
}