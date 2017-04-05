using System;
using System.Globalization;

namespace Abp.Localization.Sources
{
    /// <summary>
    /// Extension methods for <see cref="ILocalizationSource"/>.
    /// <see cref="ILocalizationSource"/>����չ����
    /// </summary>
    public static class LocalizationSourceExtensions
    {
        /// <summary>
        /// Get a localized string by formatting string.
        /// ͨ����ʽ���ַ���ȡ���ػ��ַ���
        /// </summary>
        /// <param name="source">Localization source / ���ػ�Դ</param>
        /// <param name="name">Key name / ����</param>
        /// <param name="args">Format arguments / ��ʽ������</param>
        /// <returns>Formatted and localized string / ��ʽ����ı��ػ�����</returns>
        public static string GetString(this ILocalizationSource source, string name, params object[] args)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return string.Format(source.GetString(name), args);
        }

        /// <summary>
        /// Get a localized string in given language by formatting string.
        /// ͨ����ʽ���ַ���ȡ�������Եı��ػ��ַ���
        /// </summary>
        /// <param name="source">Localization source / ���ػ�Դ</param>
        /// <param name="name">Key name / ����</param>
        /// <param name="culture">Culture / �����Ļ�</param>
        /// <param name="args">Format arguments / ��ʽ������</param>
        /// <returns>Formatted and localized string / ��ʽ����ı��ػ�����</returns>
        public static string GetString(this ILocalizationSource source, string name, CultureInfo culture, params object[] args)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return string.Format(source.GetString(name, culture), args);
        }
    }
}