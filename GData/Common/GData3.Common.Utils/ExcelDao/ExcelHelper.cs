using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GData3.Common.Utils
{
    public class ExcelHelper
    {
        public static HSSFWorkbook OutExcel(IList<CellRangeModel> cellRanges,IList<Cell> cells,String title) {
            HSSFWorkbook workbook = new HSSFWorkbook();
            //创建sheet表
            ISheet sheet = workbook.CreateSheet(title);

            //先合并单元格

            return null;
        }

        public static ISheet OutExcel<T>(T[] datas, String[] title,short DefaultRowHeight,int DefaultColumnWidth, ICellStyle[,] cellstyle, CellRangeAddress[] cellRangeAddresses,HSSFWorkbook workbook,Dictionary<int,int> ColumnWidth)
        {
            //根据属性顺序输出表格（没有样式以及标题）
            //创建sheet表
            ISheet sheet = workbook.CreateSheet();
            sheet.DefaultRowHeight =(short) DefaultRowHeight;
            sheet.DefaultColumnWidth = DefaultColumnWidth;
            foreach (CellRangeAddress rangeAddress in cellRangeAddresses) {
                sheet.AddMergedRegion(rangeAddress);
            }
            foreach (int index in ColumnWidth.Keys) {
                sheet.SetColumnWidth(index, ColumnWidth[index]);
            }

            for (int i = 0; i < datas.Length; i++) {
                IRow row = sheet.CreateRow(i);
                for (int j = 0; j < title.Length; j++) {
                    ICell cell = row.CreateCell(j);
                    PropertyInfo property = typeof(T).GetProperty(title[j]);
                    //cell.SetCellValue(((property.GetValue(datas[i])) ?? "").ToString());
                    //cell.CellStyle = cellstyle[i, j];
                    if (property.PropertyType.Name == "Decimal" || property.PropertyType.Name == "Double")
                    {
                        cell.SetCellValue(Convert.ToDouble(property.GetValue(datas[i]).ToString()));
                        ICellStyle moneyStyle = CreateStyle(workbook, HorizontalAlignment.Right, VerticalAlignment.Center, 70, 12, true);
                        IDataFormat dataFormat = workbook.CreateDataFormat();
                        moneyStyle.DataFormat = dataFormat.GetFormat("#,###0.00");
                        cell.CellStyle = moneyStyle;
                    }
                    else
                    {
                        cell.SetCellValue(((property.GetValue(datas[i])) ?? "").ToString());
                        cell.CellStyle = cellstyle[i, j];
                    }
                }
            }
            return sheet;
        }
        /// <summary>
        /// 有表头表尾表体的拼接
        /// </summary>
        /// <param name="head"></param>
        /// <param name="body"></param>
        /// <param name="end"></param>
        /// <param name="workbook"></param>
        /// <param name="DefaultRowHeight"></param>
        /// <param name="DefaultColumnWidth"></param>
        /// <param name="ColumnWidth"></param>
        /// <param name="cellRangeAddresses"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static HSSFWorkbook Splice_Sheet(ISheet head, ISheet body, ISheet end, HSSFWorkbook workbook,short DefaultRowHeight, int DefaultColumnWidth, Dictionary<int, int> ColumnWidth, CellRangeAddress[] cellRangeAddresses,String title)
        {
            while (true)
            {
                try
                {
                    workbook.RemoveSheetAt(workbook.ActiveSheetIndex);
                }
                catch (System.ArgumentException ex) {
                    break;
                }
            }
            ISheet sheet = workbook.CreateSheet(title);
            sheet.DefaultRowHeight = (short)DefaultRowHeight;
            sheet.DefaultColumnWidth = DefaultColumnWidth;
           
            for (int i = 0; i <= head.LastRowNum; i++)
            {
                IRow row =sheet.CreateRow(i);
                for (int j = 0; j < head.GetRow(i).LastCellNum; j++) {
                    ICell cell = row.CreateCell(j);
                    if (head.GetRow(i).GetCell(j).CellType == CellType.Numeric)
                    {
                        cell.SetCellValue(head.GetRow(i).GetCell(j).NumericCellValue);
                    }
                    else if (head.GetRow(i).GetCell(j).CellType == CellType.String)
                    {
                        cell.SetCellValue(head.GetRow(i).GetCell(j).StringCellValue);
                    }
                    cell.CellStyle= head.GetRow(i).GetCell(j).CellStyle;
                }
            }
            if (body.LastRowNum != 0)
            {
                for (int i = 0; i <= body.LastRowNum; i++)
                {
                    IRow row = sheet.CreateRow(head.LastRowNum + i + 1);
                    for (int j = 0; j < body.GetRow(i).LastCellNum; j++)
                    {
                        ICell cell = row.CreateCell(j);
                        if (body.GetRow(i).GetCell(j).CellType == CellType.Numeric)
                        {
                            cell.SetCellValue(body.GetRow(i).GetCell(j).NumericCellValue);
                        }
                        else if (body.GetRow(i).GetCell(j).CellType == CellType.String) {
                            cell.SetCellValue(body.GetRow(i).GetCell(j).StringCellValue);
                        }
                        cell.CellStyle = body.GetRow(i).GetCell(j).CellStyle;
                    }
                }
            }
            for (int i = 0; i <= end.LastRowNum; i++)
            {
                IRow row = sheet.CreateRow(body.LastRowNum + i + 2+ head.LastRowNum);
                for (int j = 0; j < end.GetRow(i).LastCellNum; j++)
                {
                    ICell cell = row.CreateCell(j);
                    if (end.GetRow(i).GetCell(j).CellType == CellType.Numeric)
                    {
                        cell.SetCellValue(end.GetRow(i).GetCell(j).NumericCellValue);
                    }
                    else if (end.GetRow(i).GetCell(j).CellType == CellType.String)
                    {
                        cell.SetCellValue(end.GetRow(i).GetCell(j).StringCellValue);
                    }
                    cell.CellStyle = end.GetRow(i).GetCell(j).CellStyle;
                }
            }
            foreach (CellRangeAddress rangeAddress in cellRangeAddresses)
            {
                sheet.AddMergedRegion(rangeAddress);
            }
            CellRangeAddress[] endRang = new CellRangeAddress[1];
            endRang[0] = new CellRangeAddress(body.LastRowNum + 2 + head.LastRowNum, body.LastRowNum + 2 + head.LastRowNum, 0, body.GetRow(0).LastCellNum-1);
            sheet.AddMergedRegion(endRang[0]);
            foreach (int index in ColumnWidth.Keys)
            {
                sheet.SetColumnWidth(index, ColumnWidth[index]);
            }
            return workbook;
        }

        /// <summary>
        /// 只有表头表体
        /// </summary>
        /// <param name="head"></param>
        /// <param name="body"></param>
        /// <param name="workbook"></param>
        /// <param name="DefaultRowHeight"></param>
        /// <param name="DefaultColumnWidth"></param>
        /// <param name="ColumnWidth"></param>
        /// <param name="cellRangeAddresses"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static HSSFWorkbook Splice_SheetTwo(ISheet head, ISheet body, HSSFWorkbook workbook, short DefaultRowHeight, int DefaultColumnWidth, Dictionary<int, int> ColumnWidth, CellRangeAddress[] cellRangeAddresses, String title)
        {
            while (true)
            {
                try
                {
                    workbook.RemoveSheetAt(workbook.ActiveSheetIndex);
                }
                catch (System.ArgumentException ex)
                {
                    break;
                }
            }
            ISheet sheet = workbook.CreateSheet(title);
            sheet.DefaultRowHeight =(short) DefaultRowHeight;
            sheet.DefaultColumnWidth = DefaultColumnWidth;

            for (int i = 0; i <= head.LastRowNum; i++)
            {
                IRow row = sheet.CreateRow(i);
                for (int j = 0; j < head.GetRow(i).LastCellNum; j++)
                {
                    ICell cell = row.CreateCell(j);
                    if (head.GetRow(i).GetCell(j).CellType == CellType.Numeric)
                    {
                        cell.SetCellValue(head.GetRow(i).GetCell(j).NumericCellValue);
                    }
                    else if (head.GetRow(i).GetCell(j).CellType == CellType.String)
                    {
                        cell.SetCellValue(head.GetRow(i).GetCell(j).StringCellValue);
                    }
                    cell.CellStyle = head.GetRow(i).GetCell(j).CellStyle;
                }
            }
            if (body.LastRowNum != 0)
            {
                for (int i = 0; i <= body.LastRowNum; i++)
                {
                    IRow row = sheet.CreateRow(head.LastRowNum + i + 1);
                    for (int j = 0; j < body.GetRow(i).LastCellNum; j++)
                    {
                        ICell cell = row.CreateCell(j);
                        if (body.GetRow(i).GetCell(j).CellType == CellType.Numeric)
                        {
                            cell.SetCellValue(body.GetRow(i).GetCell(j).NumericCellValue);
                        }
                        else if (body.GetRow(i).GetCell(j).CellType == CellType.String)
                        {
                            cell.SetCellValue(body.GetRow(i).GetCell(j).StringCellValue);
                        }
                        cell.CellStyle = body.GetRow(i).GetCell(j).CellStyle;
                    }
                }
            }           
            foreach (CellRangeAddress rangeAddress in cellRangeAddresses)
            {
                sheet.AddMergedRegion(rangeAddress);
            }
            foreach (int index in ColumnWidth.Keys)
            {
                sheet.SetColumnWidth(index, ColumnWidth[index]);
            }
            return workbook;
        }
        public static HSSFWorkbook OutExcel<T>(T[] datas, String[] title)
        {
            //根据属性顺序输出表格（没有样式以及标题）
            HSSFWorkbook workbook = new HSSFWorkbook();
            //创建sheet表
            ISheet sheet = workbook.CreateSheet();

            for (int i = 0; i < datas.Length; i++)
            {
                IRow row = sheet.CreateRow(i);
                for (int j = 0; j < title.Length; j++)
                {
                    ICell cell = row.CreateCell(j);
                    cell.SetCellValue(typeof(T).GetProperty(title[j]).GetValue(datas[i]).ToString());
                }
            }
            //MemoryStream ms = new MemoryStream();
            //workbook.Write(ms);
            //var buf = ms.ToArray();
            //string path = System.AppDomain.CurrentDomain.BaseDirectory + @"\\DownLoadFiles\\zcfz";
            //string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            //using (FileStream fs = File.OpenWrite(path + "\\" + filename))
            //{
            //    fs.Write(buf, 0, buf.Length);
            //    fs.Flush();
            //}
            //workbook = null;
            //ms.Close();
            //ms.Dispose();
            return workbook;
        }

        /// <summary>
        /// 获取单元格的值
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public static object GetCellValue(ICell cell)
        {
            if (cell == null)
                return null;
            switch (cell.CellType)
            {
                case CellType.Blank: //BLANK:  
                    return null;
                case CellType.Boolean: //BOOLEAN:  
                    return cell.BooleanCellValue;
                case CellType.Numeric: //NUMERIC:  
                    if (HSSFDateUtil.IsCellDateFormatted(cell)) { 
                        //日期型会被判为Numeric,所以要做处理
                        return cell.DateCellValue;
                    }
                    return cell.NumericCellValue;
                case CellType.String: //STRING:  
                    return cell.StringCellValue;
                case CellType.Error: //ERROR:  
                    return cell.ErrorCellValue;
                case CellType.Formula: //FORMULA:  
                default:
                    return "=" + cell.CellFormula;
            }
        }

        /// <summary>
        /// 设置单元格样式
        /// </summary>
        /// <param name="workbook">excel表格</param>
        /// <param name="horizontal">水平布局</param>
        /// <param name="vertical">垂直布局</param>
        /// <param name="boldWeight">字体加粗</param>
        /// <param name="fontPoint">字体大小</param>
        /// <param name="isBorder">是否需要边框</param>
        /// <param name="fontName">字体名称：宋体、黑体、微软雅黑，默认微软雅黑</param>
        /// <returns>返回单元格样式</returns>
        public static HSSFCellStyle CreateStyle(HSSFWorkbook workbook, HorizontalAlignment horizontal, VerticalAlignment vertical, short boldWeight, short fontPoint, bool isBorder, string fontName = "微软雅黑")
        {
            HSSFCellStyle cellStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            cellStyle.Alignment = horizontal;
            cellStyle.VerticalAlignment = vertical;
            if (isBorder)
            {
                cellStyle.BorderTop = BorderStyle.Thin;
                cellStyle.BorderLeft = BorderStyle.Thin;
                cellStyle.BorderRight = BorderStyle.Thin;
                cellStyle.BorderBottom = BorderStyle.Thin;
            }
            //创建字体
            HSSFFont cellStyleFont = (HSSFFont)workbook.CreateFont();
            //字体加粗
            cellStyleFont.Boldweight = boldWeight;
            cellStyleFont.FontHeightInPoints = fontPoint;
            cellStyleFont.FontName = fontName;
            cellStyle.SetFont(cellStyleFont);
            return cellStyle;
        }


        /// <summary>
        /// 设置单元格样式(数据格式为数字型)
        /// </summary>
        /// <param name="workbook">excel表格</param>
        /// <param name="horizontal">水平布局</param>
        /// <param name="vertical">垂直布局</param>
        /// <param name="boldWeight">字体加粗</param>
        /// <param name="fontPoint">字体大小</param>
        /// <param name="isBorder">是否需要边框</param>
        /// <param name="fontName">字体名称：宋体、黑体、微软雅黑，默认微软雅黑</param>
        /// <returns>返回单元格样式</returns>
        public static HSSFCellStyle CreateStyle2(HSSFWorkbook workbook, HorizontalAlignment horizontal, VerticalAlignment vertical, short boldWeight, short fontPoint, bool isBorder, string fontName = "微软雅黑")
        {
            HSSFCellStyle cellStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            cellStyle.Alignment = horizontal;
            cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");
            cellStyle.VerticalAlignment = vertical;
            if (isBorder)
            {
                cellStyle.BorderTop = BorderStyle.Thin;
                cellStyle.BorderLeft = BorderStyle.Thin;
                cellStyle.BorderRight = BorderStyle.Thin;
                cellStyle.BorderBottom = BorderStyle.Thin;
            }
            //创建字体
            HSSFFont cellStyleFont = (HSSFFont)workbook.CreateFont();
            //字体加粗
            cellStyleFont.Boldweight = boldWeight;
            cellStyleFont.FontHeightInPoints = fontPoint;
            cellStyleFont.FontName = fontName;
            cellStyle.SetFont(cellStyleFont);
            return cellStyle;
        }

        /// <summary>
        /// 设置单元格样式(数据格式为数字型)(红色)
        /// </summary>
        /// <param name="workbook">excel表格</param>
        /// <param name="horizontal">水平布局</param>
        /// <param name="vertical">垂直布局</param>
        /// <param name="boldWeight">字体加粗</param>
        /// <param name="fontPoint">字体大小</param>
        /// <param name="isBorder">是否需要边框</param>
        /// <param name="fontName">字体名称：宋体、黑体、微软雅黑，默认微软雅黑</param>
        /// <returns>返回单元格样式</returns>
        public static HSSFCellStyle CreateStyle3(HSSFWorkbook workbook, HorizontalAlignment horizontal, VerticalAlignment vertical, short boldWeight, short fontPoint, bool isBorder, string fontName = "微软雅黑")
        {
            HSSFCellStyle cellStyle = (HSSFCellStyle)workbook.CreateCellStyle();
            cellStyle.Alignment = horizontal;
            cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");
            cellStyle.VerticalAlignment = vertical;
            if (isBorder)
            {
                cellStyle.BorderTop = BorderStyle.Thin;
                cellStyle.BorderLeft = BorderStyle.Thin;
                cellStyle.BorderRight = BorderStyle.Thin;
                cellStyle.BorderBottom = BorderStyle.Thin;
            }
            //创建字体
            HSSFFont cellStyleFont = (HSSFFont)workbook.CreateFont();
            //字体加粗
            cellStyleFont.Boldweight = boldWeight;
            cellStyleFont.FontHeightInPoints = fontPoint;
            cellStyleFont.FontName = fontName;
            cellStyleFont.Color = NPOI.HSSF.Util.HSSFColor.Red.Index;
            cellStyle.SetFont(cellStyleFont);
            return cellStyle;
        }
    }
}
