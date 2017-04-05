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
    /// String����չ����.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Adds a char to end of given string if it does not ends with the char.
        /// ��������Ķ������������ض����ַ���β�������ַ�����ĩβ�����Ӹ��ַ�
        /// </summary>
        public static string EnsureEndsWith(this string str, char c)
        {
            return EnsureEndsWith(str, c, StringComparison.InvariantCulture);
        }

        /// <summary>
        /// Adds a char to end of given string if it does not ends with the char.
        /// ��������Ķ������������ض����ַ���β�������ַ�����ĩβ�����Ӹ��ַ�
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
        /// ��������Ķ������������ض����ַ���β�������ַ�����ĩβ�����Ӹ��ַ�
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
        /// ��������Ķ������������ض����ַ���ʼ�������ַ����Ŀ�ʼ�����Ӹ��ַ�
        /// </summary>
        public static string EnsureStartsWith(this string str, char c)
        {
            return EnsureStartsWith(str, c, StringComparison.InvariantCulture);
        }

        /// <summary>
        /// Adds a char to beginning of given string if it does not starts with the char.
        /// ��������Ķ������������ض����ַ���ʼ�������ַ����Ŀ�ʼ�����Ӹ��ַ�
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
        /// ��������Ķ������������ض����ַ���ʼ�������ַ����Ŀ�ʼ�����Ӹ��ַ�
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
        /// ָʾ���ַ����Ƿ�ΪNULL��������һ�� System.String.Empty
        /// </summary>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// indicates whether this string is null, empty, or consists only of white-space characters.
        /// ָʾ���ַ����Ƿ�ΪNULL��������һ��ֻ�����ո���ַ���
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// Gets a substring of a string from beginning of the string.
        /// ���ַ����Ŀ�ʼλ�û�ȡ���ַ���
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null / ��� <paramref name="str"/> Ϊnullʱ�׳�</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="len"/> is bigger that string's length / �����ַ����ĳ���ʱ�׳�</exception>
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
        /// ���ַ����е���β���з�ת����<see cref="Environment.NewLine"/>
        /// </summary>
        public static string NormalizeLineEndings(this string str)
        {
            return str.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", Environment.NewLine);
        }

        /// <summary>
        /// Gets index of nth occurence of a char in a string.
        /// ��ȡ�ַ����е�N�γ���ָ���ַ������
        /// </summary>
        /// <param name="str">source string to be searched / �����������ַ���</param>
        /// <param name="c">Char to search in <see cref="str"/> / ָ�����ַ� <see cref="str"/></param>
        /// <param name="n">Count of the occurence / ���ֵĴ���</param>
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
        /// �Ӹ����ַ����Ƴ���һ��������׺
        /// </summary>
        /// <param name="str">The string. / �ַ���</param>
        /// <param name="postFixes">one or more postfix. / ��׺����</param>
        /// <returns>Modified string or the same string if it has not any of given postfixes / ��������ַ���û���κθ�����׺�������޸ĺ���ַ�������ͬ���ַ���</returns>
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
        /// �Ӹ����ַ����Ƴ���һ�γ����ڸ�����׺
        /// </summary>
        /// <param name="str">The string. / �ַ���</param>
        /// <param name="preFixes">one or more prefix. / ��׺����</param>
        /// <returns>Modified string or the same string if it has not any of given prefixes / ��������ַ���û���κθ�����׺</returns>
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
        /// ���ַ����Ľ�βλ�û�ȡ���ַ���
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null / ��� <paramref name="str"/> Ϊnullʱ�׳�</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="len"/> is bigger that string's length / <paramref name="len"/> �����ַ����ĳ���ʱ�׳�</exception>
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
        /// ʹ�÷���string.Split���ø����ķָ��ַ��ָ��ַ���
        /// </summary>
        public static string[] Split(this string str, string separator)
        {
            return str.Split(new[] { separator }, StringSplitOptions.None);
        }

        /// <summary>
        /// Uses string.Split method to split given string by given separator.
        /// ʹ�÷���string.Split���ø����ķָ��ַ��ָ��ַ���
        /// </summary>
        public static string[] Split(this string str, string separator, StringSplitOptions options)
        {
            return str.Split(new[] { separator }, options);
        }

        /// <summary>
        /// Uses string.Split method to split given string by <see cref="Environment.NewLine"/>.
        /// ʹ�÷���string.Split����<see cref="Environment.NewLine"/>�ָ��ַ���.
        /// </summary>
        public static string[] SplitToLines(this string str)
        {
            return str.Split(Environment.NewLine);
        }

        /// <summary>
        /// Uses string.Split method to split given string by <see cref="Environment.NewLine"/>.
        /// ʹ�÷���string.Split����<see cref="Environment.NewLine"/>�ָ��ַ���.
        /// </summary>
        public static string[] SplitToLines(this string str, StringSplitOptions options)
        {
            return str.Split(Environment.NewLine, options);
        }

        /// <summary>
        /// Converts PascalCase string to camelCase string.
        /// ����˹����������ʽ���ַ���ת��Ϊ�շ���������ʽ���ַ���
        /// </summary>
        /// <param name="str">String to convert / ����ת�����ַ���</param>
        /// <returns>camelCase of the string / �շ���������ʽ���ַ���</returns>
        public static string ToCamelCase(this string str)
        {
            return str.ToCamelCase(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts PascalCase string to camelCase string in specified culture.
        /// ʹ��ָ�����Ļ�����˹����������ʽ���ַ���ת��Ϊ�շ���������ʽ���ַ���
        /// </summary>
        /// <param name="str">String to convert / ����ת�����ַ���</param>
        /// <param name="culture">An object that supplies culture-specific casing rules / �ṩ�ض������Сд����������Ļ�����</param>
        /// <returns>camelCase of the string / �շ���������ʽ���ַ���</returns>
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
        /// �������� ����˹������/�շ����� ת���ɾ���(ͨ���ո�ָ�)
        /// Example: "ThisIsSampleSentence" is converted to "This is a sample sentence".
        /// ���磺"ThisIsSampleSentence" ����ת���� "This is a sample sentence"
        /// </summary>
        /// <param name="str">String to convert. / ��ת�����ַ���</param>
        public static string ToSentenceCase(this string str)
        {
            return str.ToSentenceCase(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts given PascalCase/camelCase string to sentence (by splitting words by space).
        /// �������� ����˹������/�շ����� ת���ɾ���(ͨ���ո�ָ�)
        /// Example: "ThisIsSampleSentence" is converted to "This is a sample sentence".
        /// ���磺"ThisIsSampleSentence" ����ת���� "This is a sample sentence"
        /// </summary>
        /// <param name="str">String to convert. / ��ת�����ַ���</param>
        /// <param name="culture">An object that supplies culture-specific casing rules. / �ṩ�ض������Сд����������Ļ�����</param>
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
        /// ���ַ���ת��Ϊö��ֵ
        /// </summary>
        /// <typeparam name="T">Type of enum / ö������</typeparam>
        /// <param name="value">String value to convert / ת��ת�����ַ���ֵ</param>
        /// <returns>Returns enum object / ö�ٶ���</returns>
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
        /// ���ַ���ת��Ϊö��ֵ
        /// </summary>
        /// <typeparam name="T">Type of enum / ö������</typeparam>
        /// <param name="value">String value to convert / ת��ת�����ַ���ֵ</param>
        /// <param name="ignoreCase">Ignore case / �Ƿ���Դ�Сд</param>
        /// <returns>Returns enum object / ö�ٶ���</returns>
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
        /// MD5����
        /// </summary>
        /// <param name="str">�����ַ���</param>
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
        /// ���շ���������ʽ���ַ���ת��Ϊ��˹����������ʽ���ַ���
        /// </summary>
        /// <param name="str">String to convert / ����ת�����ַ���</param>
        /// <returns>PascalCase of the string / ��˹����������ʽ���ַ���</returns>
        public static string ToPascalCase(this string str)
        {
            return str.ToPascalCase(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts camelCase string to PascalCase string in specified culture.
        /// ���շ���������ʽ���ַ���ת��Ϊ��˹����������ʽ���ַ���
        /// </summary>
        /// <param name="str">String to convert / ����ת�����ַ���</param>
        /// <param name="culture">An object that supplies culture-specific casing rules / �ṩ�ض������Сд����������Ļ�����</param>
        /// <returns>PascalCase of the string / ��˹����������ʽ���ַ���</returns>
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
        /// ����ַ�������ָ������󳤶ȣ��ӿ�ʼλ�û�ȡ�����ӷ���������Ϊָ������󳤶�
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null / �ַ���ΪNull���׳��쳣</exception>
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
        /// ����ַ�������ָ������󳤶ȣ��ӿ�ʼλ�û�ȡ�����ӷ�����
        /// It adds a "..." postfix to end of the string if it's truncated.
        /// ����ַ�������ȡ�������ĺ�ӽ����ϡ�...����
        /// Returning string can not be longer than maxLength.
        /// ����һ����������󳤶ȵ��ַ�����
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception> / ��� <paramref name="str"/>Ϊ null�׳�</exception>
        public static string TruncateWithPostfix(this string str, int maxLength)
        {
            return TruncateWithPostfix(str, maxLength, "...");
        }

        /// <summary>
        /// Gets a substring of a string from beginning of the string if it exceeds maximum length.
        /// ����ַ�������ָ������󳤶ȣ��ӿ�ʼλ�û�ȡ�����ӷ�����
        /// It adds given <paramref name="postfix"/> to end of the string if it's truncated.
        /// ����ַ�������ȡ�������ĺ�ӽ����ϸ����ĺ�׺��
        /// Returning string can not be longer than maxLength.
        /// ����һ����������󳤶ȵ��ַ���
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception> / <exception cref="ArgumentNullException">��� <paramref name="str"/>Ϊ null�׳�</exception>
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