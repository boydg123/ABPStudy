namespace Abp.Localization
{
    /// <summary>
    /// Represents an available language for the application.
    /// 表示应用程序的可用语言
    /// </summary>
    public class LanguageInfo
    {
        /// <summary>
        /// Code name of the language.It should be valid culture code.
        /// 语言Code,应该是有效的区域代码
        /// Ex: "en-US" for American English, "tr-TR" for Turkey Turkish.
        /// 例子:"en-US"代表美国英语。"tr-TR"代表土耳其语
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Display name of the language in it's original language.
        /// 在它的原始语言中显示语言的名称
        /// Ex: "English" for English, "T黵k鏴" for Turkish.
        /// 例子:"English"代表英语
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// An icon can be set to display on the UI.
        /// 在UI上显示的图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Is this the default language?
        /// 是否是默认语言
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Creates a new <see cref="LanguageInfo"/> object.
        /// 构造函数
        /// </summary>
        /// <param name="name">
        /// Code name of the language.It should be valid culture code.
        /// 语言Code,应该是有效的区域代码
        /// Ex: "en-US" for American English, "tr-TR" for Turkey Turkish.
        /// 例子:"en-US"代表美国英语。"tr-TR"代表土耳其语
        /// </param>
        /// <param name="displayName">
        /// Display name of the language in it's original language.
        /// 在它的原始语言中显示语言的名称
        /// Ex: "English" for English, "T黵k鏴" for Turkish.
        /// 例子:"English"代表英语
        /// </param>
        /// <param name="icon">An icon can be set to display on the UI / 在UI上显示的图标</param>
        /// <param name="isDefault">Is this the default language? / 是否是默认语言</param>
        public LanguageInfo(string name, string displayName, string icon = null, bool isDefault = false)
        {
            Name = name;
            DisplayName = displayName;
            Icon = icon;
            IsDefault = isDefault;
        }
    }
}