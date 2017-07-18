using System.Collections.Generic;
using System.Linq;
using Abp.Localization;
using Derrick.EntityFramework;

namespace Derrick.Migrations.Seed.Host
{
    /// <summary>
    /// 默认语言创造器
    /// </summary>
    public class DefaultLanguagesCreator
    {
        /// <summary>
        /// 语言列表
        /// </summary>
        public static List<ApplicationLanguage> InitialLanguages { get; private set; }

        /// <summary>
        /// DB上下文
        /// </summary>
        private readonly AbpZeroTemplateDbContext _context;

        /// <summary>
        /// 默认语言创造器
        /// </summary>
        static DefaultLanguagesCreator()
        {
            InitialLanguages = new List<ApplicationLanguage>
            {
                new ApplicationLanguage(null, "en", "English", "famfamfam-flag-gb"),
                new ApplicationLanguage(null, "ar", "العربية", "famfamfam-flag-sa"),
                new ApplicationLanguage(null, "de", "German", "famfamfam-flag-de"),
                new ApplicationLanguage(null, "it", "Italiano", "famfamfam-flag-it"),
                new ApplicationLanguage(null, "pt-BR", "Portuguese", "famfamfam-flag-br"),
                new ApplicationLanguage(null, "tr", "Türkçe", "famfamfam-flag-tr"),
                new ApplicationLanguage(null, "ru", "Русский", "famfamfam-flag-ru"),
                new ApplicationLanguage(null, "zh-CN", "简体中文", "famfamfam-flag-cn")
            };
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context">DB上下文</param>
        public DefaultLanguagesCreator(AbpZeroTemplateDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 创建
        /// </summary>
        public void Create()
        {
            CreateLanguages();
        }

        /// <summary>
        /// 创建语言
        /// </summary>
        private void CreateLanguages()
        {
            foreach (var language in InitialLanguages)
            {
                AddLanguageIfNotExists(language);
            }
        }

        /// <summary>
        /// 添加不存在的语言
        /// </summary>
        /// <param name="language">语言对象</param>
        private void AddLanguageIfNotExists(ApplicationLanguage language)
        {
            if (_context.Languages.Any(l => l.TenantId == language.TenantId && l.Name == language.Name))
            {
                return;
            }

            _context.Languages.Add(language);

            _context.SaveChanges();
        }
    }
}