using System.ComponentModel.DataAnnotations.Schema;
using Abp.Application.Editions;

namespace Abp.Application.Features
{
    /// <summary>
    /// Feature setting for an <see cref="Edition"/>.
    /// 一个<see cref="Edition"/>的功能设置
    /// </summary>
    public class EditionFeatureSetting : FeatureSetting
    {
        /// <summary>
        /// Gets or sets the edition.
        /// 获取或设置版本
        /// </summary>
        /// <value>
        /// The edition.
        /// 版本
        /// </value>
        [ForeignKey("EditionId")]
        public virtual Edition Edition { get; set; }

        /// <summary>
        /// Gets or sets the edition Id.
        /// </summary>
        /// <value>
        /// 版本ID
        /// </value>
        public virtual int EditionId { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public EditionFeatureSetting()
        {
            
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="editionId">版本ID</param>
        /// <param name="name">功能名称</param>
        /// <param name="value">功能值</param>
        public EditionFeatureSetting(int editionId, string name, string value)
            :base(name, value)
        {
            EditionId = editionId;
        }
    }
}