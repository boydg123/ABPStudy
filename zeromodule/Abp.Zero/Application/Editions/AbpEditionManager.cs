using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Features;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Runtime.Caching;

namespace Abp.Application.Editions
{
    /// <summary>
    /// ABP版本管理器
    /// </summary>
    public abstract class AbpEditionManager : IDomainService
    {
        /// <summary>
        /// ABP Zero功能值存储引用
        /// </summary>
        private readonly IAbpZeroFeatureValueStore _featureValueStore;
        /// <summary>
        /// 版本集合
        /// </summary>
        public IQueryable<Edition> Editions => EditionRepository.GetAll();
        /// <summary>
        /// 缓存引用
        /// </summary>
        public ICacheManager CacheManager { get; set; }
        /// <summary>
        /// 功能管理引用
        /// </summary>
        public IFeatureManager FeatureManager { get; set; }
        /// <summary>
        /// 版本仓储引用
        /// </summary>
        protected IRepository<Edition> EditionRepository { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="editionRepository">版本仓储引用</param>
        /// <param name="featureValueStore">ABP Zero功能值存储引用</param>
        protected AbpEditionManager(
            IRepository<Edition> editionRepository,
            IAbpZeroFeatureValueStore featureValueStore)
        {
            _featureValueStore = featureValueStore;
            EditionRepository = editionRepository;
        }
        /// <summary>
        /// 获取功能值(没有则返回Null)
        /// </summary>
        /// <param name="editionId">版本ID</param>
        /// <param name="featureName">功能名称</param>
        /// <returns></returns>
        public virtual Task<string> GetFeatureValueOrNullAsync(int editionId, string featureName)
        {
            return _featureValueStore.GetEditionValueOrNullAsync(editionId, featureName);
        }
        /// <summary>
        /// 设置功能值
        /// </summary>
        /// <param name="editionId">版本ID</param>
        /// <param name="featureName">功能名称</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public virtual Task SetFeatureValueAsync(int editionId, string featureName, string value)
        {
            return _featureValueStore.SetEditionFeatureValueAsync(editionId, featureName, value);
        }
        /// <summary>
        /// 获取功能值
        /// </summary>
        /// <param name="editionId">版本ID</param>
        /// <returns></returns>
        public virtual async Task<IReadOnlyList<NameValue>> GetFeatureValuesAsync(int editionId)
        {
            var values = new List<NameValue>();

            foreach (var feature in FeatureManager.GetAll())
            {
                values.Add(new NameValue(feature.Name, await GetFeatureValueOrNullAsync(editionId, feature.Name) ?? feature.DefaultValue));
            }

            return values;
        }
        /// <summary>
        /// 设置功能值
        /// </summary>
        /// <param name="editionId">版本ID</param>
        /// <param name="values">值集合</param>
        /// <returns></returns>
        public virtual async Task SetFeatureValuesAsync(int editionId, params NameValue[] values)
        {
            if (values.IsNullOrEmpty())
            {
                return;
            }

            foreach (var value in values)
            {
                await SetFeatureValueAsync(editionId, value.Name, value.Value);
            }
        }
        /// <summary>
        /// 创建版本
        /// </summary>
        /// <param name="edition"></param>
        /// <returns></returns>
        public virtual Task CreateAsync(Edition edition)
        {
            return EditionRepository.InsertAsync(edition);
        }
        /// <summary>
        /// 通过名称获取版本
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual Task<Edition> FindByNameAsync(string name)
        {
            return EditionRepository.FirstOrDefaultAsync(edition => edition.Name == name);
        }
        /// <summary>
        /// 通过ID查找版本
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task<Edition> FindByIdAsync(int id)
        {
            return EditionRepository.FirstOrDefaultAsync(id);
        }
        /// <summary>
        /// 通过ID获取版本
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task<Edition> GetByIdAsync(int id)
        {
            return EditionRepository.GetAsync(id);
        }
        /// <summary>
        /// 删除版本
        /// </summary>
        /// <param name="edition"></param>
        /// <returns></returns>
        public virtual Task DeleteAsync(Edition edition)
        {
            return EditionRepository.DeleteAsync(edition);
        }
    }
}
