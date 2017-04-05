using System.Collections.Generic;
using Abp.EntityFramework.GraphDiff.Mapping;

namespace Abp.EntityFramework.GraphDiff.Configuration
{
    /// <summary>
    /// <see cref="IAbpEntityFrameworkGraphDiffModuleConfiguration"/>的默认实现
    /// </summary>
    public class AbpEntityFrameworkGraphDiffModuleConfiguration : IAbpEntityFrameworkGraphDiffModuleConfiguration
    {
        /// <summary>
        /// 实体映射列表
        /// </summary>
        public List<EntityMapping> EntityMappings { get; set; }
    }
}