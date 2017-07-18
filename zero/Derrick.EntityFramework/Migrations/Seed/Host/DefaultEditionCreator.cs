using System.Linq;
using Abp.Application.Editions;
using Abp.Application.Features;
using Derrick.Editions;
using Derrick.EntityFramework;
using Derrick.Features;

namespace Derrick.Migrations.Seed.Host
{
    /// <summary>
    /// 默认版本创造器
    /// </summary>
    public class DefaultEditionCreator
    {
        /// <summary>
        /// DB上下文
        /// </summary>
        private readonly AbpZeroTemplateDbContext _context;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context">DB上下文</param>
        public DefaultEditionCreator(AbpZeroTemplateDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 创建
        /// </summary>
        public void Create()
        {
            CreateEditions();
        }

        /// <summary>
        /// 创建版本
        /// </summary>
        private void CreateEditions()
        {
            var defaultEdition = _context.Editions.FirstOrDefault(e => e.Name == EditionManager.DefaultEditionName);
            if (defaultEdition == null)
            {
                defaultEdition = new Edition { Name = EditionManager.DefaultEditionName, DisplayName = EditionManager.DefaultEditionName };
                _context.Editions.Add(defaultEdition);
                _context.SaveChanges();

                //TODO: Add desired features to the standard edition, if wanted!
            }

            if (defaultEdition.Id > 0)
            {
                CreateFeatureIfNotExists(defaultEdition.Id, AppFeatures.ChatFeature, true);
                CreateFeatureIfNotExists(defaultEdition.Id, AppFeatures.TenantToTenantChatFeature, true);
                CreateFeatureIfNotExists(defaultEdition.Id, AppFeatures.TenantToHostChatFeature, true);
            }
        }

        /// <summary>
        /// 创建不存在的功能
        /// </summary>
        /// <param name="editionId">版本ID</param>
        /// <param name="featureName">功能名称</param>
        /// <param name="isEnabled">是否启用</param>
        private void CreateFeatureIfNotExists(int editionId, string featureName, bool isEnabled)
        {
            var defaultEditionChatFeature = _context.EditionFeatureSettings
                                                        .FirstOrDefault(ef => ef.EditionId == editionId && ef.Name == featureName);

            if (defaultEditionChatFeature == null)
            {
                _context.EditionFeatureSettings.Add(new EditionFeatureSetting
                {
                    Name = featureName,
                    Value = isEnabled.ToString(),
                    EditionId = editionId
                });
            }
        }
    }
}