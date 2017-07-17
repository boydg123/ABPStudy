using Abp.Application.Features;
using Abp.AutoMapper;
using Abp.UI.Inputs;

namespace Derrick.Editions.Dto
{
    /// <summary>
    /// 平级功能Dto
    /// </summary>
    [AutoMapFrom(typeof(Feature))]
    public class FlatFeatureDto
    {
        /// <summary>
        /// 父Name
        /// </summary>
        public string ParentName { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 显示名
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }
        /// <summary>
        /// Input类型
        /// </summary>
        public IInputType InputType { get; set; }
    }
}