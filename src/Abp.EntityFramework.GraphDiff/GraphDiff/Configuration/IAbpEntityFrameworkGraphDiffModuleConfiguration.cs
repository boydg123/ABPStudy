using System.Collections.Generic;
using Abp.EntityFramework.GraphDiff.Mapping;

namespace Abp.EntityFramework.GraphDiff.Configuration
{
    /// <summary>
    /// Abp EF GraphDiff模块配置
    /// </summary>
    public interface IAbpEntityFrameworkGraphDiffModuleConfiguration
    {
        /// <summary>
        /// 实体映射列表
        /// </summary>
        List<EntityMapping> EntityMappings { get; set; }
    }
}