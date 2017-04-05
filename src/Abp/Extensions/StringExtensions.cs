using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Abp.Collections.Extensions;

namespace Abp.Extensions
{
    /// <summary>
    /// Extension methods for String class.
    /// String类扩展方法.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Adds a char to end of given string if it does not ends with the char.
        /// 如果给定的定符串不是以特定的字符结尾，就在字符串的末尾处添加该字符
        /// </summary>
        public static string EnsureEndsWith(this string str, char c)
        {
            return EnsureEndsWith(str, c, StringComparison.InvariantCulture);
        }

        /// <summary>
        /// Adds a char to end of given string if it does not ends with the char.
        /// 如果给定的定符串不是以特定的字符结尾，就在字符串的末尾处添加该字符
        /// </summary>
        public static string EnsureEndsWith(this string str, char c, StringComparison comparisonType)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            if (str.EndsWith(c.ToString(CultureInfo.InvariantCulture), comparisonType))
            {
                return str;
            }

            return str + c;
        }

        /// <summary>
        /// Adds a char to end of given string if it does not ends with the char.
        /// 如果给定的定符串不是以特定的字符结尾，就在字符串的末尾处添加该字符
        /// </summary>
        public static string EnsureEndsWith(this string str, char c, bool ignoreCase, CultureInfo culture)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            if (str.EndsWith(c.ToString(culture), ignoreCase, culture))
            {
                return str;
            }

            return str + c;
        }

        /// <summary>
        /// Adds a char to beginning of given string if it does not starts with the char.
        /// 如果给定的定符串不是以特定的字符开始，就在字符串的开始处添加该字符
        /// </summary>
        public static string EnsureStartsWith(this string str, char c)
        {
            return EnsureStartsWith(str, c, StringComparison.InvariantCulture);
        }

        /// <summary>
        /// Adds a char to beginning of given string if it does not starts with the char.
        /// 如果给定的定符串不是以特定的字符开始，就在字符串的开始处添加该字符
        /// </summary>
        public static string EnsureStartsWith(this string str, char c, StringComparison comparisonType)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            if (str.StartsWith(c.ToString(CultureInfo.InvariantCulture), comparisonType))
            {
                return str;
            }

            return c + str;
        }

        /// <summary>
        /// Adds a char to beginning of given string if it does not starts with the char.
        /// 如果给定的定符串不是以特定的字符开始，就在字符串的开始处添加该字符
        /// </summary>
        public static string EnsureStartsWith(this string str, char c, bool ignoreCase, CultureInfo culture)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            if (str.StartsWith(c.ToString(culture), ignoreCase, culture))
            {
                return str;
            }

            return c + str;
        }

        /// <summary>
        /// Indicates whether this string is null or an System.String.Empty string.
        /// 指示此字符串是否为NULL，或者是一个 System.String.Empty
        /// </summary>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// indicates whether this string is null, empty, or consists only of white-space characters.
        /// 指示此字符串是否为NULL，或者是一个只包含空格的字符串
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// Gets a substring of a string from beginning of the string.
        /// 从字符串的开始位置获取子字符串
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null / 如果 <paramref name="str"/> 为null时抛出</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="len"/> is bigger that string's length / 大于字符串的长度时抛出</exception>
        public static string Left(this string str, int len)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            if (str.Length < len)
            {
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            }

            return str.Substring(0, len);
        }

        /// <summary>
        /// Converts line endings in the string to <see cref="Environment.NewLine"/>.
        /// 将字符串中的行尾换行符转换成<see cref="Environment.NewLine"/>
        /// </summary>
        public static string NormalizeLineEndings(this string str)
        {
            return str.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", Environment.NewLine);
        }

        /// <summary>
        /// Gets index of nth occurence of a char in a string.
        /// 获取字符串中第N次出现指定字符的序号
        /// </summary>
        /// <param name="str">source string to be searched / 将被搜索的字符串</param>
        /// <param name="c">Char to search in <see cref="str"/> / 指定的字符 <see cref="str"/></param>
        /// <param name="n">Count of the occurence / 出现的次数</param>
        public static int NthIndexOf(this string str, char c, int n)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            var count = 0;
            for (var i = 0; i < str.Length; i++)
            {
                if (str[i] != c)
                {
                    continue;
                }

                if ((++count) == n)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Removes first occurrence of the given postfixes from end of the given string.
        /// 从给定字符串移除第一个给定后缀
        /// </summary>
        /// <param name="str">The string. / 字符串</param>
        /// <param name="postFixes">one or more postfix. / 后缀数组</param>
        /// <returns>Modified string or the same string if it has not any of given postfixes / 如果给定字符串没有任何给定后缀。返回修改后的字符串或相同的字符串</returns>
        public static string RemovePostFix(this string str, params string[] postFixes)
        {
            if (str.IsNullOrEmpty())
            {
                return null;
            }

            if (postFixes.IsNullOrEmpty())
            {
                return str;
            }

            foreach (var postFix in postFixes)
            {
                if (str.EndsWith(postFix))
                {
                    return str.Left(str.Length - postFix.Length);
                }
            }

            return str;
        }

        /// <summary>
        /// Removes first occurrence of the given prefixes from beginning of the given string.
        /// 从给定字符串移除第一次出现在给定后缀
        /// </summary>
        /// <param name="str">The string. / 字符串</param>
        /// <param name="preFixes">one or more prefix. / 后缀数组</param>
        /// <returns>Modified string or the same string if it has not any of given prefixes / 如果给定字符串没有任何给定后缀</returns>
        public static string RemovePreFix(this string str, params string[] preFixes)
        {
            if (str.IsNullOrEmpty())
            {
                return null;
            }

            if (preFixes.IsNullOrEmpty())
            {
                return str;
            }

            foreach (var preFix in preFixes)
            {
                if (str.StartsWith(preFix))
                {
                    return str.Right(str.Length - preFix.Length);
                }
            }

            return str;
        }

        /// <summary>
        /// Gets a substring of a string from end of the string.
        /// 从字符串的结尾位置获取子字符串
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null / 如果 <paramref name="str"/> 为null时抛出</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="len"/> is bigger that string's length / <paramref name="len"/> 大于字符串的长度时抛出</exception>
        public static string Right(this string str, int len)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            if (str.Length < len)
            {
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            }

            return str.Substring(str.Length - len, len);
        }

        /// <summary>
        /// Uses string.Split method to split given string by given separator.
        /// 使用方法string.Split，用给定的分隔字符分隔字符串
        /// </summary>
        public static string[] Split(this string str, string separator)
        {
            return str.Split(new[] { separator }, StringSplitOptions.None);
        }

        /// <summary>
        /// Uses string.Split method to split given string by given separator.
        /// 使用方法string.Split，用给定的分隔字符分隔字符串
        /// </summary>
        public static string[] Split(this string str, string separator, StringSplitOptions options)
        {
            return str.Split(new[] { separator }, options);
        }

        /// <summary>
        /// Uses string.Split method to split given string by <see cref="Environment.NewLine"/>.
        /// 使用方法string.Split，用<see cref="Environment.NewLine"/>分隔字符串.
        /// </summary>
        public static string[] SplitToLines(this string str)
        {
            return str.Split(Environment.NewLine);
        }

        /// <summary>
        /// Uses string.Split method to split given string by <see cref="Environment.NewLine"/>.
        /// 使用方法string.Split，用<see cref="Environment.NewLine"/>分隔字符串.
        /// </summary>
        public static string[] SplitToLines(this string str, StringSplitOptions options)
        {
            return str.Split(Environment.NewLine, options);
        }

        /// <summary>
        /// Converts PascalCase string to camelCase string.
        /// 将帕斯卡命名法形式的字符串转换为驼峰命名法形式的字符串
        /// </summary>
        /// <param name="str">String to convert / 将被转换的字符串</param>
        /// <returns>camelCase of the string / 驼峰命名法形式的字符串</returns>
        public static string ToCamelCase(this string str)
        {
            return str.ToCamelCase(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts PascalCase string to camelCase string in specified culture.
        /// 使用指定的文化将帕斯卡命名法形式的字符串转换为驼峰命名法形式的字符串
        /// </summary>
        /// <param name="str">String to convert / 将被转换的字符串</param>
        /// <param name="culture">An object that supplies culture-specific casing rules / 提供特定区域大小写规则的区域文化对象</param>
        /// <returns>camelCase of the string / 驼峰命名法形式的字符串</returns>
        public static string ToCamelCase(this string str, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            if (str.Length == 1)
            {
                return str.ToLower(culture);
            }

            return char.ToLower(str[0], culture) + str.Substring(1);
        }

        /// <summary>
        /// Converts given PascalCase/camelCase string to sentence (by splitting words by space).
        /// 将给定的 将帕斯卡命名/驼峰命名 转换成句子(通过空格分割)
        /// Example: "ThisIsSampleSentence" is converted to "This is a sample sentence".
        /// 例如："ThisIsSampleSentence" 将被转换成 "This is a sample sentence"
        /// </summary>
        /// <param name="str">String to convert. / 将转换的字符串</param>
        public static string ToSentenceCase(this string str)
        {
            return str.ToSentenceCase(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts given PascalCase/camelCase string to sentence (by splitting words by space).
        /// 将给定的 将帕斯卡命名/驼峰命名 转换成句子(通过空格分割)
        /// Example: "ThisIsSampleSentence" is converted to "This is a sample sentence".
        /// 例如："ThisIsSampleSentence" 将被转换成 "This is a sample sentence"
        /// </summary>
        /// <param name="str">String to convert. / 将转换的字符串</param>
        /// <param name="culture">An object that supplies culture-specific casing rules. / 提供特定区域大小写规则的区域文化对象</param>
        public static string ToSentenceCase(this string str, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            return Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1], culture));
        }

        /// <summary>
        /// Converts string to enum value.
        /// 将字符串转换为枚举值
        /// </summary>
        /// <typeparam name="T">Type of enum / 枚举类型</typeparam>
        /// <param name="value">String value to convert / 转被转换的字符串值</param>
        /// <returns>Returns enum object / 枚举对象</returns>
        public static T ToEnum<T>(this string value)
            where T : struct
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            return (T)Enum.Parse(typeof(T), value);
        }

        /// <summary>
        /// Converts string to enum value.
        /// 将字符串转换为枚举值
        /// </summary>
        /// <typeparam name="T">Type of enum / 枚举类型</typeparam>
        /// <param name="value">String value to convert / 转被转换的字符串值</param>
        /// <param name="ignoreCase">Ignore case / 是否忽略大小写</param>
        /// <returns>Returns enum object / 枚举对象</returns>
        public static T ToEnum<T>(this string value, bool ignoreCase)
            where T : struct
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str">加密字符串</param>
        /// <returns></returns>
        public static string ToMd5(this string str)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes(str);
                var hashBytes = md5.ComputeHash(inputBytes);

                var sb = new StringBuilder();
                foreach (var hashByte in hashBytes)
                {
                    sb.Append(hashByte.ToString("X2"));
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// Converts camelCase string to PascalCase string.
        /// 将驼峰命名法形式的字符串转换为帕斯卡命名法形式的字符串
        /// </summary>
        /// <param name="str">String to convert / 将被转换的字符串</param>
        /// <returns>PascalCase of the string / 帕斯卡命名法形式的字符串</returns>
        public static string ToPascalCase(this string str)
        {
            return str.ToPascalCase(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts camelCase string to PascalCase string in specified culture.
        /// 将驼峰命名法形式的字符串转换为帕斯卡命名法形式的字符串
        /// </summary>
        /// <param name="str">String to convert / 将被转换的字符串</param>
        /// <param name="culture">An object that supplies culture-specific casing rules / 提供特定区域大小写规则的区域文化对象</param>
        /// <returns>PascalCase of the string / 帕斯卡命名法形式的字符串</returns>
        public static string ToPascalCase(this string str, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            if (str.Length == 1)
            {
                return str.ToUpper(culture);
            }

            return char.ToUpper(str[0], culture) + str.Substring(1);
        }

        /// <summary>
        /// Gets a substring of a string from beginning of the string if it exceeds maximum length.
        /// 如果字符串超过指定的最大长度，从开始位置获取它的子符串，长度为指定的最大长度
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null / 字符串为Null则抛出异常</exception>
        public static string Truncate(this string str, int maxLength)
        {
            if (str == null)
            {
                return null;
            }

            if (str.Length <= maxLength)
            {
                return str;
            }

            return str.Left(maxLength);
        }

        /// <summary>
        /// Gets a substring of a string from beginning of the string if it exceeds maximum length.
        /// 如果字符串超过指定的最大长度，从开始位置获取它的子符串，
        /// It adds a "..." postfix to end of the string if it's truncated.
        /// 如果字符串被截取，在它的后加将加上“...”，
        /// Returning string can not be longer than maxLength.
        /// 返回一个不超过最大长度的字符串。
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception> / 如果 <paramref name="str"/>为 null抛出</exception>
        public static string TruncateWithPostfix(this string str, int maxLength)
        {
            return TruncateWithPostfix(str, maxLength, "...");
        }

        /// <summary>
        /// Gets a substring of a string from beginning of the string if it exceeds maximum length.
        /// 如果字符串超过指定的最大长度，从开始位置获取它的子符串，
        /// It adds given <paramref name="postfix"/> to end of the string if it's truncated.
        /// 如果字符串被截取，在它的后加将加上给定的后缀，
        /// Returning string can not be longer than maxLength.
        /// 返回一个不超过最大长度的字符串
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception> / <exception cref="ArgumentNullException">如果 <paramref name="str"/>为 null抛出</exception>
        public static string TruncateWithPostfix(this string str, int maxLength, string postfix)
        {
            if (str == null)
            {
                return null;
            }

            if (str == string.Empty || maxLength == 0)
            {
                return string.Empty;
            }

            if (str.Length <= maxLength)
            {
                return str;
            }

            if (maxLength <= postfix.Length)
            {
                return postfix.Left(maxLength);
            }

            return str.Left(maxLength - postfix.Length) + postfix;
        }
    }
}