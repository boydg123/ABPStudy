using System;
using System.Collections.Generic;
using System.IO;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Derrick.Dto;
using Derrick.Net.MimeTypes;
using OfficeOpenXml;

namespace Derrick.DataExporting.Excel.EpPlus
{
    /// <summary>
    /// EpPlus Excel导出器
    /// </summary>
    public abstract class EpPlusExcelExporterBase : AbpZeroTemplateServiceBase, ITransientDependency
    {
        /// <summary>
        /// APP文件夹
        /// </summary>
        public IAppFolders AppFolders { get; set; }
        /// <summary>
        /// 创建Excel包
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="creator">Excel包创建器</param>
        /// <returns></returns>
        protected FileDto CreateExcelPackage(string fileName, Action<ExcelPackage> creator)
        {
            var file = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);

            using (var excelPackage = new ExcelPackage())
            {
                creator(excelPackage);
                Save(excelPackage, file);
            }

            return file;
        }
        /// <summary>
        /// 添加Header
        /// </summary>
        /// <param name="sheet">Sheet</param>
        /// <param name="headerTexts">Header文本</param>
        protected void AddHeader(ExcelWorksheet sheet, params string[] headerTexts)
        {
            if (headerTexts.IsNullOrEmpty())
            {
                return;
            }

            for (var i = 0; i < headerTexts.Length; i++)
            {
                AddHeader(sheet, i + 1, headerTexts[i]);
            }
        }
        /// <summary>
        /// 添加Header
        /// </summary>
        /// <param name="sheet">Sheet</param>
        /// <param name="columnIndex">列Index</param>
        /// <param name="headerText">Header文本</param>
        protected void AddHeader(ExcelWorksheet sheet, int columnIndex, string headerText)
        {
            sheet.Cells[1, columnIndex].Value = headerText;
            sheet.Cells[1, columnIndex].Style.Font.Bold = true;
        }
        /// <summary>
        /// 添加对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="sheet">Sheet</param>
        /// <param name="startRowIndex">开始行Index</param>
        /// <param name="items">对象列表</param>
        /// <param name="propertySelectors">属性选择器</param>
        protected void AddObjects<T>(ExcelWorksheet sheet, int startRowIndex, IList<T> items, params Func<T, object>[] propertySelectors)
        {
            if (items.IsNullOrEmpty() || propertySelectors.IsNullOrEmpty())
            {
                return;
            }

            for (var i = 0; i < items.Count; i++)
            {
                for (var j = 0; j < propertySelectors.Length; j++)
                {
                    sheet.Cells[i + startRowIndex, j + 1].Value = propertySelectors[j](items[i]);
                }
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="excelPackage">Excel包</param>
        /// <param name="file">文件Dto</param>
        protected void Save(ExcelPackage excelPackage, FileDto file)
        {
            var filePath = Path.Combine(AppFolders.TempFileDownloadFolder, file.FileToken);
            excelPackage.SaveAs(new FileInfo(filePath));
        }
    }
}