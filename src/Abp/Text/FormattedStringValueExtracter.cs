using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Abp.Collections.Extensions;
using Abp.Text.Formatting;

namespace Abp.Text
{
    /// <summary>
    /// This class is used to extract dynamic values from a formatted string.It works as reverse of <see cref="string.Format(string,object)"/>
    /// 此类用于从格式化的字符串从提取动态值对象。它是 <see cref="string.Format(string,object)"/>的反向。
    /// </summary>
    /// <example>
    /// Say that str is "My name is Neo." and format is "My name is {name}.".
    /// 假设字符串是"My name is Neo.",格式化后为"My name is {name}." 
    /// Then Extract method gets "Neo" as "name".  
    /// 然后，提取方法获取"Neo" 作为 "name"
    /// </example>
    public class FormattedStringValueExtracter
    {
        /// <summary>
        /// Extracts dynamic values from a formatted string.
        /// 从格式化的字符串从提取动态值对象
        /// </summary>
        /// <param name="str">String including dynamic values / 包含动态值的对象</param>
        /// <param name="format">Format of the string / 格式化后的字符串</param>
        /// <param name="ignoreCase">True, to search case-insensitive. / True,搜索时，忽略大小写.</param>
        public ExtractionResult Extract(string str, string format, bool ignoreCase = false)
        {
            var stringComparison = ignoreCase
                ? StringComparison.InvariantCultureIgnoreCase
                : StringComparison.InvariantCulture;

            if (str == format) //TODO: think on that!
            {
                return new ExtractionResult(true);
            }

            var formatTokens = new FormatStringTokenizer().Tokenize(format);
            if (formatTokens.IsNullOrEmpty())
            {
                return new ExtractionResult(str == "");
            }

            var result = new ExtractionResult(true);

            for (var i = 0; i < formatTokens.Count; i++)
            {
                var currentToken = formatTokens[i];
                var previousToken = i > 0 ? formatTokens[i - 1] : null;

                if (currentToken.Type == FormatStringTokenType.ConstantText)
                {
                    if (i == 0)
                    {
                        if (!str.StartsWith(currentToken.Text, stringComparison))
                        {
                            result.IsMatch = false;
                            return result;
                        }

                        str = str.Substring(currentToken.Text.Length);
                    }
                    else
                    {
                        var matchIndex = str.IndexOf(currentToken.Text, stringComparison);
                        if (matchIndex < 0)
                        {
                            result.IsMatch = false;
                            return result;
                        }

                        Debug.Assert(previousToken != null, "previousToken can not be null since i > 0 here");

                        result.Matches.Add(new NameValue(previousToken.Text, str.Substring(0, matchIndex)));
                        str = str.Substring(matchIndex + currentToken.Text.Length);
                    }
                }
            }

            var lastToken = formatTokens.Last();
            if (lastToken.Type == FormatStringTokenType.DynamicValue)
            {
                result.Matches.Add(new NameValue(lastToken.Text, str));
            }

            return result;
        }

        /// <summary>
        /// Checks if given <see cref="str"/> fits to given <see cref="format"/>.Also gets extracted values.
        /// 检查给定的<see cref="str"/> 是否符合给定的<see cref="format"/>.同时，提取值。
        /// </summary>
        /// <param name="str">String including dynamic values / 包含动态时的字符串</param>
        /// <param name="format">Format of the string / 字符串格式</param>
        /// <param name="values">Array of extracted values if matched / 提取值的数组（如果匹配）</param>
        /// <param name="ignoreCase">True, to search case-insensitive / True,忽略大小写</param>
        /// <returns>True, if matched. / True,如果匹配</returns>
        public static bool IsMatch(string str, string format, out string[] values, bool ignoreCase = false)
        {
            var result = new FormattedStringValueExtracter().Extract(str, format, ignoreCase);
            if (!result.IsMatch)
            {
                values = new string[0];
                return false;
            }

            values = result.Matches.Select(m => m.Value).ToArray();
            return true;
        }

        /// <summary>
        /// Used as return value of <see cref="Extract"/> method.
        /// 用作方法<see cref="Extract"/>的返回值类型.
        /// </summary>
        public class ExtractionResult
        {
            /// <summary>
            /// Is fully matched.
            /// 是否匹配
            /// </summary>
            public bool IsMatch { get; set; }

            /// <summary>
            /// List of matched dynamic values.
            /// 匹配的动态值列表
            /// </summary>
            public List<NameValue> Matches { get; private set; }

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="isMatch"></param>
            internal ExtractionResult(bool isMatch)
            {
                IsMatch = isMatch;
                Matches = new List<NameValue>();
            }
        }
    }
}