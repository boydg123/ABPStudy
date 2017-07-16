namespace Derrick.Security
{
    /// <summary>
    /// 密码复杂度设置
    /// </summary>
    public class PasswordComplexitySetting
    {
        /// <summary>
        /// 密码最小长度
        /// </summary>
        public int MinLength { get; set; }

        /// <summary>
        /// 密码最大长度
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// 密码中强制使用数字
        /// </summary>
        public bool UseNumbers { get; set; }

        /// <summary>
        /// 密码中强制使用大写字母
        /// </summary>
        public bool UseUpperCaseLetters { get; set; }

        /// <summary>
        /// 密码中强制使用小写字母
        /// </summary>
        public bool UseLowerCaseLetters { get; set; }

        /// <summary>
        /// 密码中强制使用标点符号
        /// </summary>
        public bool UsePunctuations { get; set; }
    }
}
