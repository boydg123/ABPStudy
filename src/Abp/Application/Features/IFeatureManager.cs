using System.Collections.Generic;

namespace Abp.Application.Features
{
    /// <summary>
    /// Feature manager.
    /// 功能管理器
    /// </summary>
    public interface IFeatureManager
    {
        /// <summary>
        /// Gets the <see cref="Feature"/> by specified name.
        /// 通过特定的名称获取<see cref="Feature"/>
        /// </summary>
        /// <param name="name">Unique name of the feature. / 功能的唯一名称</param>
        Feature Get(string name);

        /// <summary>
        /// Gets the <see cref="Feature"/> by specified name or returns null if not found.
        /// 通过特定的名称获取<see cref="Feature"/>，如果没有找到则返回Null
        /// </summary>
        /// <param name="name">The name. / 名称</param>
        Feature GetOrNull(string name);

        /// <summary>
        /// Gets all <see cref="Feature"/>s.
        /// 获取所有的<see cref="Feature"/>
        /// </summary>
        /// <returns></returns>
        IReadOnlyList<Feature> GetAll();
    }
}