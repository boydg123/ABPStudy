using System;

namespace Abp.Application.Services.Dto
{
    /// <summary>
    /// This DTO can be used as a simple item for a combobox/list.
    /// 此DTO能用于combobox/list的一个项
    /// </summary>
    [Serializable]
    public class ComboboxItemDto
    {
        /// <summary>
        /// Value of the item.
        /// 项对应的值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Display text of the item.
        /// 项对应的显示文本
        /// </summary>
        public string DisplayText { get; set; }

        /// <summary>
        /// Is selected?
        /// 是否选中
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// Creates a new <see cref="ComboboxItemDto"/>.
        /// 构造函数
        /// </summary>
        public ComboboxItemDto()
        {

        }

        /// <summary>
        /// Creates a new <see cref="ComboboxItemDto"/>.
        /// 构造函数
        /// </summary>
        /// <param name="value">Value of the item / 项对应的值</param>
        /// <param name="displayText">Display text of the item / 项对应的显示文本</param>
        public ComboboxItemDto(string value, string displayText)
        {
            Value = value;
            DisplayText = displayText;
        }
    }
}