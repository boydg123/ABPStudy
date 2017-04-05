using System;
using System.Linq;
using System.Xml;

namespace Abp.Xml.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="XmlNode"/> class.
    /// <see cref="XmlNode"/>类的扩展方法.
    /// </summary>
    public static class XmlNodeExtensions
    {
        /// <summary>
        /// Gets an attribute's value from an Xml node.
        /// 从xml节点获取一个属性的值
        /// </summary>
        /// <param name="node">The Xml node / Xml节点</param>
        /// <param name="attributeName">Attribute name / 属性名</param>
        /// <returns>Value of the attribute / 属性值</returns>
        public static string GetAttributeValueOrNull(this XmlNode node, string attributeName)
        {
            if (node.Attributes == null || node.Attributes.Count <= 0)
            {
                throw new ApplicationException(node.Name + " node has not " + attributeName + " attribute");
            }

            return node.Attributes
                .Cast<XmlAttribute>()
                .Where(attr => attr.Name == attributeName)
                .Select(attr => attr.Value)
                .FirstOrDefault();
        }
    }
}
