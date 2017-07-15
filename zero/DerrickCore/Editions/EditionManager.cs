using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Domain.Repositories;

namespace Derrick.Editions
{
    /// <summary>
    /// <see cref="AbpEditionManager"/>版本实现。版本管理器
    /// </summary>
    public class EditionManager : AbpEditionManager
    {
        /// <summary>
        /// 默认版本名称
        /// </summary>
        public const string DefaultEditionName = "Standard";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="editionRepository">版本仓储</param>
        /// <param name="featureValueStore">功能值存储</param>
        public EditionManager(
            IRepository<Edition> editionRepository, 
            IAbpZeroFeatureValueStore featureValueStore) 
            : base(
                editionRepository,
                featureValueStore
            )
        {

        }
    }
}
