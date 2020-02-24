using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using Newtonsoft.Json.Linq;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using SUP.Frame.DataAccess;
using SUP.Common.DataAccess;
using SUP.Common.DataEntity.Individual;
using Newtonsoft.Json;
using NG3.Log.Log4Net;
using NG.Cache.Client;

namespace SUP.Frame.Rule
{
    public class ExcelImportRule
    {

        #region 私有字段
        private const string EXCELTEMPLATEID = "_exceltemplateId";
        private const string EXCELUPFILEPATH = "_excelupFilePath";  

        private ExcelImportDac dac = new ExcelImportDac();

        //private static string upFileName;
        //private static string upFilePath;
        //private static string templateId;
        //private static string jObjectStr;
        //private static DataTable template;
        //private static DataSet origData;
        //private static DataSet ownData;
        
        
        //private static Dictionary<string, List<KeyValuePair<string, string>>> fieldDic;//所有列
        //private static Dictionary<string, List<bool>> mustInputDic;//必输列
        //private static Dictionary<string, List<string>> helpIdDic;//通用帮助
        //private static Dictionary<string, List<string>> buskeyDic;//主键

        #endregion

        #region 日志相关
        private ILogger _logger = null;
        internal ILogger Logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = Log4NetLoggerFactory.Instance.CreateLogger(typeof(ExcelImportRule), LogType.logoperation);
                }
                return _logger;
            }
        }
        #endregion

        #region 属性

        //模板id
        public string TemplateId
        {
            get
            {
                string cachedID = System.Web.HttpContext.Current.Session.SessionID + EXCELTEMPLATEID;
                return CacheClient.Instance.GetData(cachedID).ToString();
            }
            set
            {
                string cachedID = System.Web.HttpContext.Current.Session.SessionID + EXCELTEMPLATEID;
                CacheClient.Instance.Add(cachedID, value);
            }
        }

        //上传的文件路径
        public string UpFilePath
        {
            get
            {
                string cachedID = System.Web.HttpContext.Current.Session.SessionID + EXCELUPFILEPATH;
                return CacheClient.Instance.GetData(cachedID).ToString();               
            }
            set
            {                
                string cachedID = System.Web.HttpContext.Current.Session.SessionID + EXCELUPFILEPATH;
                CacheClient.Instance.Add(cachedID, value);
            }
        }
                

        #endregion

        #region 单据模板导出
        /// <summary>
        /// 单据模板导出
        /// </summary>
        /// <param name="dt">从前端传来的gridPanel中的数据转化的datatable</param>
        /// <param name="multipleSheet">前端formPanel中radio的选项，是否为多表</param>
        public MemoryStream ExportTemplate(DataTable dt, string multipleSheet)
        {
            try
            {
                if (multipleSheet == "2")
                {
                    return ExportMultipleSheetTemplate(dt);
                }
                else
                {
                    return ExportSingleSheetTemplate(dt);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 导出多表Excel模板
        /// </summary>
        /// <param name="file"></param>
        private MemoryStream ExportMultipleSheetTemplate(DataTable dt)
        {
            try
            {
                return CreateExcelFile(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 导出单表Excel模板
        /// </summary>
        /// <param name="file"></param>
        private MemoryStream ExportSingleSheetTemplate(DataTable dt)
        {
            try
            {
                return CreateExcelFile(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 通过NPOI构建excel文件
        /// </summary>
        /// <param name="filename"></param>
        private MemoryStream CreateExcelFile(DataTable template)
        {
            MemoryStream ms = new MemoryStream();

            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            List<string> bodytablelist = GetTemplateTableName(template);
            for (int i = 0; i < bodytablelist.Count; i++)//每一个表对应一个sheet页
            {
                HSSFSheet worksheet = null;
                worksheet = hssfworkbook.CreateSheet(bodytablelist[i]) as HSSFSheet;
                int tempcount = 0;//用于存放通用帮助数据的列数，（tempcount+drs.Length）必须小于等于24
                DataRow[] drs = template.Select("tablename='" + bodytablelist[i] + "' and check='true'", "primarykey DESC");//先1后0
                if (drs.Length > 0)
                {
                    HSSFRow row = worksheet.CreateRow(0) as HSSFRow;
                    for (int j = 0; j < drs.Length; j++)
                    {
                        #region 设置某一列的默认单元格格式为文本，字体为10
                        HSSFCellStyle style = hssfworkbook.CreateCellStyle() as HSSFCellStyle;
                        HSSFDataFormat format = hssfworkbook.CreateDataFormat() as HSSFDataFormat;
                        //if (drs[j]["datatype"].ToString() == string.Empty)
                        //{
                        //    style.DataFormat = format.GetFormat("@");
                        //}
                        //else
                        //{
                        //    style.DataFormat = format.GetFormat(drs[j]["datatype"].ToString());
                        //}
                        if (template.Columns.Contains("dataformat"))
                        {
                            if (drs[j]["dataformat"].ToString() == string.Empty)
                            {
                                style.DataFormat = format.GetFormat("@");
                            }
                            else
                            {
                                style.DataFormat = format.GetFormat(drs[j]["dataformat"].ToString());
                            }
                        }
                        else {
                            style.DataFormat = format.GetFormat("@");
                        }

                        HSSFFont font = hssfworkbook.CreateFont() as HSSFFont;
                        font.FontHeightInPoints = (short)10;
                        style.SetFont(font);
                        worksheet.SetDefaultColumnStyle((short)j, style);
                        #endregion

                        HSSFCell cell = row.CreateCell(j) as HSSFCell;
                        if (drs[j]["mustinput"].ToString() == "1")
                        {
                            cell.SetCellValue(drs[j]["name"].ToString() + "（*）");
                        }
                        else
                        {
                            cell.SetCellValue(drs[j]["name"].ToString());
                        }

                        HSSFCellStyle cellstyle = hssfworkbook.CreateCellStyle() as HSSFCellStyle;
                        cellstyle.FillForegroundColor = HSSFColor.Yellow.Index;
                        cellstyle.FillPattern = FillPattern.SolidForeground;//填充整个单元格
                        HSSFFont cellfont = hssfworkbook.CreateFont() as HSSFFont;
                        short BOLDWEIGHT_BLD = 0x2bc;
                        cellfont.Boldweight = BOLDWEIGHT_BLD;//设置粗体显示
                        cellstyle.SetFont(cellfont);
                        cell.CellStyle = cellstyle;

                        if (drs[j]["remark"].ToString() != string.Empty)//不为空才加摘要
                        {
                            HSSFPatriarch patr = worksheet.CreateDrawingPatriarch() as HSSFPatriarch;
                            HSSFComment comment1 = patr.CreateComment(new HSSFClientAnchor());//重载函数可设置批注显示的位置，前四个参数是X,Y轴的偏移，后四个参数是批注框左上角和右下角的位置
                            comment1.String = new HSSFRichTextString(drs[j]["remark"].ToString());
                            comment1.Visible = false;//鼠标移上去显示
                            //comment1.Author = i6.Common.Util.i6AppInfoEntity.UserName;
                            cell.CellComment = comment1;
                        }

                        worksheet.SetColumnWidth(j, (drs[j]["name"].ToString().Length + 10) * 256);
                        //worksheet.AutoSizeColumn(j);//自动调整列宽

                        if (!string.IsNullOrEmpty(drs[j]["helptype"].ToString()))
                        {
                            string helpdata = string.Empty;
                            string helptype = drs[j]["helptype"].ToString();
                            try
                            {
                                helpdata = GetCommonHelpData(helptype);
                            }
                            catch
                            {
                                throw new Exception("获取通用帮助【" + helptype + "】出错");
                            }

                            //设置通用帮助列宽度
                            string[] strs = helpdata.Split(',');
                            int max = drs[j]["name"].ToString().Length + 10;
                            foreach (var str in strs)
                            {
                                if (str.Length > max)
                                {
                                    max = str.Length;
                                }
                            }
                            worksheet.SetColumnWidth(j, (max + 2) * 256);

                            if (helpdata != string.Empty && helpdata.Split(',').Length < 32768)
                            {
                                if (helpdata.Length < 255)//如果长度小于255可以直接插入
                                {
                                    CellRangeAddressList regions = new CellRangeAddressList(1, 65535, j, j);
                                    DVConstraint constraint = DVConstraint.CreateExplicitListConstraint(helpdata.Split(','));
                                    HSSFDataValidation dataValidate = new HSSFDataValidation(regions, constraint);
                                    worksheet.AddValidationData(dataValidate);
                                }
                                else
                                {
                                    string[] helpdataarray = helpdata.Split(',');
                                    for (int k = 0; k < helpdataarray.Length; k++)
                                    {
                                        HSSFRow kRow = worksheet.GetRow(k) as HSSFRow;
                                        if (kRow == null)
                                        {
                                            kRow = worksheet.CreateRow(k) as HSSFRow;
                                        }
                                        HSSFCell helpcell = kRow.CreateCell(tempcount + drs.Length) as HSSFCell;
                                        helpcell.SetCellValue(helpdataarray[k]);
                                    }
                                    string culumnname = GetCulumnName(tempcount + drs.Length);

                                    HSSFName range = hssfworkbook.CreateName() as HSSFName;
                                    range.RefersToFormula = bodytablelist[i] + "!$" + culumnname + "$" + "1:$" + culumnname + "$" + helpdataarray.Length.ToString();
                                    range.NameName = "dicRange" + bodytablelist[i] + j.ToString();

                                    CellRangeAddressList regions = new CellRangeAddressList(1, 65535, j, j);
                                    DVConstraint constraint = DVConstraint.CreateFormulaListConstraint("dicRange" + bodytablelist[i] + j.ToString());
                                    HSSFDataValidation dataValidate = new HSSFDataValidation(regions, constraint);
                                    worksheet.AddValidationData(dataValidate);

                                    //隐藏用来存放通用帮助的数据列
                                    worksheet.SetColumnHidden(tempcount + drs.Length, true);
                                    tempcount++;
                                }
                            }
                        }
                    }
                }
                worksheet.CreateFreezePane(0, 1, 0, 1);//冻结第一行
            }
            try
            {
                hssfworkbook.Write(ms);
            }
            catch
            {
                ms = new MemoryStream();
            }
            return ms;
        }


        /// <summary>
        /// 获取模板中包含的表名
        /// </summary>
        /// <returns></returns>
        private List<string> GetTemplateTableName(DataTable template)
        {
            List<string> returnList = new List<string>();
            List<string> bodytablelist = new List<string>();
            List<string> titletablelist = new List<string>();
            foreach (DataRow dr in template.Rows)
            {
                if (((dr["type"].ToString() == "表头") || (dr["type"].ToString() == "title")) && (dr["check"].ToString().Equals("True")))
                {
                    if (!titletablelist.Contains(dr["tablename"].ToString().Trim()))
                    {
                        titletablelist.Add(dr["tablename"].ToString().Trim());
                    }
                }
                else if (((dr["type"].ToString() == "表体") || (dr["type"].ToString() == "body")) && (dr["check"].ToString().Equals("True")))
                {
                    if (!bodytablelist.Contains(dr["tablename"].ToString().Trim()))
                    {
                        bodytablelist.Add(dr["tablename"].ToString().Trim());
                    }
                }
            }
            foreach (string str in titletablelist)
            {
                bodytablelist.Add(str);
            }
            for (int i = bodytablelist.Count - 1; i >= 0; i--)
            {
                returnList.Add(bodytablelist[i]);
            }
            return returnList;
        }

        /// <summary>
        /// 获取通用帮助数据
        /// 格式为： code1|name1,code2|name2,code3|name3
        /// </summary>
        /// <param name="helpType"></param>
        /// <returns></returns>
        private string GetCommonHelpData(string helpType)
        {
            if (helpType.ToLower().StartsWith("customdata:"))//自定义通用帮助数据
            {
                return helpType.Substring(11);
            }
            RichHelpDac dac = new RichHelpDac();
            StringBuilder sb = new StringBuilder();
            DataTable dt = dac.GetHelpResult(helpType);
            if (dt.Rows.Count < 1)
                return string.Empty;//未能取得通用帮助数据 
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                {
                    sb.Append(dt.Rows[i][0].ToString() + "|" + dt.Rows[i][1].ToString());
                    if (dt.Columns.Count == 3)
                    {
                        sb.Append("|" + dt.Rows[i][2].ToString());
                    }
                }
                else
                {
                    sb.Append("," + dt.Rows[i][0].ToString() + "|" + dt.Rows[i][1].ToString());
                    if (dt.Columns.Count == 3)
                    {
                        sb.Append("|" + dt.Rows[i][2].ToString());
                    }
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 根据sheet页总列数与当前用来当做数据有效性中间列的个数之和转换为Excel中实际的列名称
        /// 列名规则 A、B...Z、AA、AB...AZ
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private string GetCulumnName(int num)
        {
            int i = num / 26;
            int j = num % 26;
            string firstchar = string.Empty;
            string secondchar = string.Empty;
            if (i != 0)
            {
                firstchar = Convert.ToChar(i + 64).ToString();
            }
            secondchar = Convert.ToChar(j + 65).ToString();
            return firstchar.ToString() + secondchar.ToString();
        }

        #endregion


        #region 单据数据导入

        /// <summary>
        /// 绑定导入的Excel表格，用于读取表格内容到前端
        /// </summary>
        /// <param name="id">模板id</param>
        /// <param name="fileName">导入的Excel文件名</param>
        /// <param name="filePath">文件在服务端的路径，包含文件名</param>
        public void BindFile(string id, string fileName, string filePath)
        {
            //templateId = id;
            TemplateId = id;
            //upFileName = fileName;
            //upFilePath = filePath;
            UpFilePath = filePath;
            //template = null;
            //origData = null;
            //ownData = null;
            //fieldDic = null;
            //mustInputDic = null;
            //helpIdDic = null;
        }

        /// <summary>
        /// 返回Excel文件中存在的sheet名，即表名
        /// </summary>
        /// <returns></returns>
        public List<string> GetTableName()
        {
            string upFilePath = UpFilePath;//缓存中读取
            if (upFilePath == null)           
            {
                throw new Exception("没有上传的文件！");
            }
            List<string> tableNames = new List<string>();

            IWorkbook workbook = null;
            FileStream fileStream = null;

            try
            {
                using (fileStream = new FileStream(upFilePath, FileMode.Open, FileAccess.Read))               
                {
                    workbook = new HSSFWorkbook(fileStream);
                }
            }
            catch
            {
                if (workbook == null)
                {
                    using (fileStream = new FileStream(upFilePath, FileMode.Open, FileAccess.Read))                    
                    {
                        workbook = new XSSFWorkbook(fileStream);
                    }
                }
            }
            for (int i = 0; i < workbook.NumberOfSheets; i++)
            {
                string sheetname = workbook.GetSheetName(i);

                if (sheetname.IndexOf("Macro") >= 0) continue;//启用宏产生的sheet页

                tableNames.Add(sheetname);
            }

            try
            {
                //GetExcelData();//这个有必要么？
                bool success = true;
                bool ifMultipleSheet = true;
                DataTable template = dac.GetTemplate(TemplateId);
                CheckExcelExportType(template,out ifMultipleSheet, out success);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return tableNames;
        }

        /// <summary>
        /// 根据绑定的文件名，路径，模板等加载Excel数据，
        /// 并生成各张表表头字段的散列表，和必输项散列表
        /// </summary>
        /// <param name="fieldDic"></param>
        /// <param name="mustInputDic"></param>
        /// <returns></returns>
        public DataSet GetExcelDataAndColInfo(string tablename,out string jsonStr)
        {

            //多tab页，导致界面始终显示第一个界面的grid

            //if (ownData != null)
            //{
            //    return ownData;
            //}

            DataTable template;//模板数据
            DataSet tempOrigData;//excel的原始数据

            Dictionary<string, List<KeyValuePair<string, string>>> fieldDic = new Dictionary<string, List<KeyValuePair<string, string>>>();//所有列
            Dictionary<string, List<bool>> mustInputDic = new Dictionary<string, List<bool>>();//必输列
            Dictionary<string, List<string>> buskeyDic = new Dictionary<string, List<string>>();//主键
            Dictionary<string, List<string>> helpIdDic = new Dictionary<string, List<string>>();//通用帮助

            bool success = true;
            bool ifMultipleSheet = true;
            template = dac.GetTemplate(TemplateId);
            try
            {
                CheckExcelExportType(template,out ifMultipleSheet, out success);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //template = dac.GetTemplate(templateId);            
            tempOrigData = GetOrigData(template, ref success, ifMultipleSheet);
            if (!success)
            {
                throw new Exception("从Excel中取数据时出现错误！");
            }

            CheckColumn(tempOrigData, template);


            //中文字段名转换为英文字段名，并生成字段字典
            ConvertFieldName(tempOrigData, template, fieldDic, mustInputDic, buskeyDic);
            //生成帮助列散列表
            GetColumnHelpId(tempOrigData, template, helpIdDic);

            ////生成列类型散列表
            //GetColumnType(columnTypeDic);

            //列信息
            jsonStr = GetGridColumnInfo(template,tablename, fieldDic, mustInputDic, buskeyDic, helpIdDic);
            //转换origData为自己拼接的table
            DataSet ownData = ConvertOrigDataToOwnTable(tempOrigData, helpIdDic);

            return ownData;
        }

        private DataSet GetOrigData(DataTable template, ref bool success, bool ifMultipleSheet)
        {
            DataSet tempOrigData;
            if (ifMultipleSheet)
            {
                //origData = GetDataTableFromExcel(Template, ref success);
                tempOrigData = GetDataTableFromExcel(template, ref success);
            }
            else
            {
                //origData = GetDataTableFromExcelSingleSheet(Template, ref success);
                tempOrigData = GetDataTableFromExcelSingleSheet(template, ref success);
            }

            return tempOrigData;
        }


        //转换origData为自己拼接的table
        private DataSet ConvertOrigDataToOwnTable(DataSet tempOrigData, Dictionary<string, List<string>> helpIdDic)
        {
            DataSet ds = new DataSet();

            List<string> helpColumList = new List<string>();//有帮助的列的集合
            List<string> list;
            //foreach (DataTable dt in OrigData.Tables)
            foreach (DataTable dt in tempOrigData.Tables)                
            {
                DataTable owndt = new DataTable(dt.TableName);
                helpIdDic.TryGetValue(dt.TableName, out list);
                int index = 0;
                foreach (DataColumn dc in dt.Columns)
                {
                    owndt.Columns.Add(new DataColumn(dc.ColumnName));
                    if (!string.IsNullOrEmpty(list[index++]))
                    {
                        owndt.Columns.Add(new DataColumn(dc.ColumnName + "_ngname"));
                        helpColumList.Add(dc.ColumnName);
                    }
                }
                foreach (DataRow dr in dt.Rows)
                {
                    DataRow owndr = owndt.NewRow();
                    foreach (DataColumn dc in dt.Columns)
                    {
                        string temp = dr[dc.ColumnName].ToString();
                        if (helpColumList.Contains(dc.ColumnName))//是帮助列
                        {
                            if (temp.Split('|').Length > 1)//包含通用帮助数据
                            {
                                string[] t = temp.Split('|');
                                if (t.Length > 2)
                                {
                                    owndr[dc.ColumnName] = t[0];//格式为： 代码|编码|名称
                                    owndr[dc.ColumnName + "_ngname"] = t[2];
                                }
                                else
                                {
                                    owndr[dc.ColumnName] = t[0];//格式为： 代码|名称
                                    owndr[dc.ColumnName + "_ngname"] = t[1];
                                }
                            }
                        }
                        else
                        {
                            owndr[dc.ColumnName] = temp;
                        }
                    }
                    owndt.Rows.Add(owndr);
                }
                ds.Tables.Add(owndt);
            }
            return ds;
        }
        

        /// <summary>
        /// 校验列名是否在模板中存在
        /// </summary>
        /// <param name="origData"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        private void CheckColumn(DataSet tempOrigData, DataTable template)
        {
            //foreach (DataTable dt in origData.Tables)
            foreach (DataTable dt in tempOrigData.Tables)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    string name = dc.ColumnName;
                    if (name.EndsWith("（*）"))
                        name = name.Substring(0, name.Length - 3);
                    if (template.Select("name='" + name + "'").Length <= 0)
                    {
                        throw new Exception("表: '" + dt.TableName + "' 中的列：'" + name + "'，不在模板中, 无法上传");
                    }
                }
            }
        }

       

        /// <summary>
        ///  根据模板和Excel文件，生成字段映射散列表，字段必输项散列表
        /// </summary>
        /// <param name="tempOrigData">excel原始数据</param>
        /// <param name="template">模板</param>
        /// <param name="fieldDic">所有字段</param>
        /// <param name="mustInputDic">必输字段</param>
        /// <param name="buskeyDic">主键</param>
        private void ConvertFieldName(DataSet tempOrigData,DataTable template, Dictionary<string, List<KeyValuePair<string, string>>> fieldDic,
           Dictionary<string, List<bool>> mustInputDic, Dictionary<string, List<string>> buskeyDic)
        {
            //fieldDic = new Dictionary<string, List<KeyValuePair<string, string>>>();
            //mustInputDic = new Dictionary<string, List<bool>>();
            //buskeyDic = new Dictionary<string, List<string>>();

            //foreach (DataTable dt in OrigData.Tables)
            foreach (DataTable dt in tempOrigData.Tables)
            {
                KeyValuePair<string, string> dic;
                List<KeyValuePair<string, string>> fieldList = new List<KeyValuePair<string, string>>();
                List<bool> mustInputLIst = new List<bool>();
                List<string> buskeyList = new List<string>();

                foreach (DataColumn dc in dt.Columns)
                {
                    string name = dc.ColumnName;
                    string temp = name;

                    //获取必输项
                    if (name.EndsWith("（*）"))
                    {
                        name = name.Substring(0, name.Length - 3);
                        mustInputLIst.Add(true);
                    }
                    else
                    {
                        mustInputLIst.Add(false);
                    }

                    //获取列的字段名
                    DataRow dr = template.Select("name='" + name + "' and tablename='" + dt.TableName + "'")[0];
                    dc.ColumnName = dr["filedname"].ToString();
                    dic = new KeyValuePair<string, string>(dc.ColumnName, temp);
                    fieldList.Add(dic);

                    //获取主键列
                    if (dr["primarykey"].ToString() == "1")
                    {
                        buskeyList.Add(dc.ColumnName);
                    }

                }

                fieldDic.Add(dt.TableName, fieldList);
                mustInputDic.Add(dt.TableName, mustInputLIst);
                buskeyDic.Add(dt.TableName, buskeyList);

            }
        }
        
        /// <summary>
        /// 根据模板和Excel文件，生成帮助列的helpId映射散列表
        /// </summary>
        /// <param name="helpId"></param>
        /// <param name="columnType"></param>
        private void GetColumnHelpId(DataSet tempOrigData, DataTable template, Dictionary<string, List<string>> helpIdDic)
        {
            //helpIdDic = new Dictionary<string, List<string>>();
            //foreach (DataTable dt in OrigData.Tables)
            foreach (DataTable dt in tempOrigData.Tables)
            {
                List<string> helpIdList = new List<string>();

                foreach (DataColumn dc in dt.Columns)
                {
                    string name = dc.ColumnName;

                    DataRow[] drs = template.Select("filedname='" + name + "' and tablename='" + dt.TableName + "'");
                    string helptype = drs[0]["helptype"].ToString();
                    helpIdList.Add(helptype);
                }
                helpIdDic.Add(dt.TableName, helpIdList);
            }

        }


             
        /// <summary>
        /// 动态拼接前端的store的fields和gridcolumns
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldDic">所有列</param>
        /// <param name="mustInputDic">必输列</param>
        /// <param name="buskeyDic">主键</param>
        /// <param name="helpIdDic">帮助列</param>
        /// <returns></returns>
        public string GetGridColumnInfo(DataTable template, string tableName, Dictionary<string, List<KeyValuePair<string, string>>> fieldDic,
           Dictionary<string, List<bool>> mustInputDic, Dictionary<string, List<string>> buskeyDic, Dictionary<string, List<string>> helpIdDic)
        {
            //多tab页，导致界面始终显示第一个界面的grid
            //if (jObjectStr != null)
            //{
            //    return jObjectStr;
            //}

            List<KeyValuePair<string, string>> fieldList = null;
            List<bool> mustInputList = null;
            List<string> helpIdList = null;
            List<string> buskeyList = null;
            try
            {
                fieldDic.TryGetValue(tableName, out fieldList);
                mustInputDic.TryGetValue(tableName, out mustInputList);
                helpIdDic.TryGetValue(tableName, out helpIdList);
                buskeyDic.TryGetValue(tableName, out buskeyList);
            }
            catch
            {
                throw new Exception("Excel文件与模板不符，请检查要导入的Excel文件");
            }
            if (fieldList == null)
            {
                throw new Exception("Excel文件与模板不符，请检查要导入的Excel文件");
            }

            //必须有主键的要求暂不启用，因为新增时主键可以不录入值
            //if (buskeyList == null || buskeyList.Count == 0)
            //{
            //    throw new Exception("模板文件中没有设置主键");
            //}

            JObject jObject = GetColumn(template,tableName,fieldList, mustInputList, helpIdList, buskeyList);
            string jObjectStr = Newtonsoft.Json.JsonConvert.SerializeObject(jObject);
            return jObjectStr;
        }

        /// <summary>
        /// 组装grid的列信息
        /// </summary>
        /// <param name="fieldList"></param>
        /// <param name="mustInputList"></param>
        /// <param name="helpIdList"></param>
        /// <param name="buskeyList"></param>
        /// <returns></returns>
        private JObject GetColumn(DataTable template, string tableName, List<KeyValuePair<string, string>> fieldList,
            List<bool> mustInputList, List<string> helpIdList, List<string> buskeyList)
        {

            List<KeyValuePair<string, string>> helpData = GetHelpData(helpIdList);

            JObject roots = new JObject();
            JArray fields = new JArray();
            JArray columns = new JArray();

            JObject rownumber = new JObject
            {
                {"xtype","rownumberer" },
                {"minWidth",40 },
            };
            columns.Add(rownumber);

            int i = -1;
            foreach (KeyValuePair<string, string> kv in fieldList)
            {
                i++;
                bool flag = mustInputList[i];

                string dataType = "string";//数据类型                   
                DataRow[] drs = template.Select(string.Format("tablename='{0}' and filedname='{1}'", tableName, kv.Key));//获取数据类型
                if (drs.Length > 0)
                {
                    if (drs[0]["datatype"] != null && drs[0]["datatype"] != DBNull.Value)
                    {
                        dataType = drs[0]["datatype"].ToString();
                    }
                }

                string type = "string";
                switch (dataType)
                {
                    case "date":
                        type = "date"; //日期                        
                        break;
                    case "number":
                    default:
                        type = "string";//数字也用string,兼容64位                      
                        break;
                }

                #region field拼接
                JObject field = new JObject
                {
                    {"name",kv.Key },
                    { "type",type},
                    { "mapping",kv.Key}
                };
                fields.Add(field);

                if (helpData[i].Key != null)
                {
                    JObject f_name = new JObject
                    {
                        {"name",kv.Key+"_ngname"}
                    };
                    fields.Add(f_name);
                };
                #endregion

                #region  columns拼接
                JObject editor;
                JObject column;
                if (helpData[i].Key != null)
                {
                    if (helpData[i].Key == "code")//combox下拉
                    {
                        editor = new JObject
                       {
                            { "allowBlank",!flag },
                            { "xtype","ngComboBox" },
                            { "valueField",helpData[i].Key},
                            { "displayField",helpData[i].Value},
                            { "queryMode", "local"},
                            { "isInGrid", true},
                            { "data", TranslateData(helpIdList[i].Substring(11),',','|')}
                       };

                        column = new JObject()
                        {
                            {"header",kv.Value },
                            {"dataIndex",kv.Key },
                            {"mustInput",flag},
                            {"editor",editor},
                            {"width",168 },
                            {"renderer",GetComboxRenderer() }
                        };

                    }
                    else//richhelp帮助
                    {
                      JObject codeColumn = new JObject()
                      {
                        {"header",kv.Value },
                        {"dataIndex",kv.Key},
                        {"mustInput",flag},
                        {"hidden",true},
                        {"width",40 },
                      };

                        columns.Add(codeColumn);//先加代码列

                        //处理名称列
                        string str = "function (obj) {var data = this.findParentByType('ngGridPanel').getSelectionModel().getSelection();"
                                 + string.Format(" data[0].set('{0}', obj.code);data[0].set('{1}', obj.name); ", kv.Key, kv.Key + "_ngname") + "}";
                        JObject listeners = new JObject
                       {
                          { "helpselected",str }
                       };

                        editor = new JObject
                     {
                        { "allowBlank",!flag },
                        { "xtype","ngRichHelp" },
                        { "valueField",helpData[i].Key},
                        { "displayField",helpData[i].Value},
                        { "helpid", helpIdList[i]},
                        { "isInGrid", true},
                        { "pickerWidth", 400},
                        { "ORMMode", false},
                        {"listeners",listeners }
                     };
                        column = new JObject()
                      {
                        {"header",kv.Value },
                        {"dataIndex",kv.Key+"_ngname"},
                        {"mustInput",flag},
                        {"editor",editor},
                        {"width",168 },
                      };
                    }
                }
                else//日期、数字、文本
                {
                    //string dataType = "string";//数据类型                   
                    //DataRow[] drs = template.Select(string.Format("tablename='{0}' and filedname='{1}'",tableName,kv.Key));//获取数据类型
                    //if (drs.Length > 0)
                    //{
                    //    if (drs[0]["datatype"] != null && drs[0]["datatype"] != DBNull.Value)
                    //    {
                    //        dataType = drs[0]["datatype"].ToString();
                    //    }
                    //}

                    int decimalPrecision = 2;//小数位数
                    //小数位数
                    if ("number" == dataType)
                    {
                        if (template.Columns.Contains("decimalPrecision"))
                        {
                            if (drs[0]["decimalPrecision"] != null && drs[0]["decimalPrecision"] != DBNull.Value)
                            {
                                Int32.TryParse(drs[0]["decimalPrecision"].ToString(), out decimalPrecision);
                            }
                        }
                    }

                   
                    switch (dataType)
                    {
                        case "number"://数字 
                            editor = new JObject
                            {
                                { "xtype","numberfield" },
                                { "decimalPrecision",decimalPrecision },
                                { "allowBlank",!flag },
                            };
                            break;
                        case "date": //日期                         
                            editor = new JObject
                            {
                                { "xtype","datefield" },
                                { "allowBlank",!flag },                                
                            };
                            break;
                        default:
                            editor = new JObject
                            {
                                { "xtype","textfield" },
                                { "allowBlank",!flag },
                            };
                            break;
                    }


                    if ("date" == dataType)
                    {
                        column = new JObject()
                        {
                            {"header",kv.Value },
                            {"dataIndex",kv.Key },
                            {"mustInput",flag},
                            {"editor",editor},
                            {"renderer",GetDateRenderer() },//日期多个renderer
                            {"width",168 }
                        };
                    }
                    else
                    {

                        column = new JObject()
                        {
                            {"header",kv.Value },
                            {"dataIndex",kv.Key },
                            {"mustInput",flag},
                            {"editor",editor},
                            //{"flex",1 },
                            {"width",168 }
                        };
                    }
                }

                columns.Add(column);
                #endregion

            }

            //拼接主键
            string buskey = string.Empty;
            for (int j = 0; j < buskeyList.Count; j++)
            {
                buskey += buskeyList[j];
                if (j < buskeyList.Count - 1)
                {
                    buskey += ",";
                }
            }

            //根拼接
            roots.Add("success", "true");
            roots.Add("fieldNames", fields);
            roots.Add("columnNames", columns);
            roots.Add("buskeyInfo", buskey);

            return roots;

        }


        /// <summary>
        /// 根据导出的Excel模板的标题判断是单sheet页导出还是多sheet页导出
        /// </summary>
        /// <param name="isMultiSheetPage"></param>
        /// <param name="success"></param>
        private void CheckExcelExportType(DataTable templatedt,out bool isMultiSheetPage, out bool success)
        {
            string upFilePath = UpFilePath;//缓存中读取
            isMultiSheetPage = false;
            success = true;

            IWorkbook workbook = null;
            IRow row = null;
            FileStream filestream = null;
            try
            {
                using (filestream = new FileStream(upFilePath, FileMode.Open, FileAccess.Read))                
                {
                    workbook = new HSSFWorkbook(filestream);
                    ISheet workSheet = workbook.GetSheetAt(workbook.ActiveSheetIndex);
                    row = workSheet.GetRow(0);
                    ICell cell = row.GetCell(0);
                    string value = cell.StringCellValue;
                    int count = value.Split('|').Length;
                    if (count == 1)
                    {
                        isMultiSheetPage = true;
                    }
                    else
                    {
                        isMultiSheetPage = false;
                    }
                }
            }
            catch
            {
                if (filestream == null)
                {
                    success = false;
                    throw new Exception("当前Excel文件已打开，请关闭后再导入");
                }
                if (workbook == null)
                {
                    filestream = new FileStream(upFilePath, FileMode.Open, FileAccess.Read);                    
                    workbook = new XSSFWorkbook(filestream);
                    ISheet workSheet = workbook.GetSheetAt(workbook.ActiveSheetIndex);
                    row = workSheet.GetRow(0);
                    ICell cell = row.GetCell(0);
                    string value = cell.StringCellValue;
                    int count = value.Split('|').Length;
                    if (count == 1)
                    {
                        isMultiSheetPage = true;
                    }
                    else
                    {
                        isMultiSheetPage = false;
                    }
                }
                else if (row == null)
                {
                    success = false;
                    throw new Exception("Excel文件与模板不符，请检查要导入的Excel文件");
                }
            }

            //得到所有表名，即sheet页名称
            List<string> templateTableList = new List<string>();
            foreach (DataRow dr in templatedt.Rows)
            {
                if (!templateTableList.Contains(dr["tablename"].ToString()))
                {
                    templateTableList.Add(dr["tablename"].ToString());
                }
            }

            #region 检测excel sheet页名称是否全部来自模板中
            bool existflag = true;
            for (int i = 1; i < workbook.NumberOfSheets; i++)
            {
                string sheetname = workbook.GetSheetName(i);
                if (!templateTableList.Contains(sheetname))
                {
                    existflag = false;
                    break;
                }
            }
            if (!existflag)
            {
                success = false;
                throw new Exception("Excel文件与模板不符，请检查要导入的Excel文件");
            }
            #endregion
        }

        /// <summary>
        /// 从excel中读取数据
        /// </summary>
        /// <param name="templatedt">模板文件</param>
        /// <param name="success"></param>
        /// <returns></returns>
        private DataSet GetDataTableFromExcel(DataTable templatedt, ref bool success)
        {
            string upFilePath = UpFilePath;//缓存中读取
            DataSet ds = new DataSet();

            IWorkbook workbook;
            FileStream filestream = null;
            try
            {
                using (filestream = new FileStream(upFilePath, FileMode.Open, FileAccess.Read))                
                {
                    workbook = new HSSFWorkbook(filestream);
                }
            }
            catch
            {
                if (filestream == null)
                {
                    success = false;
                    //MessageBox.Show("当前Excel文件已打开，请关闭后再导入", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    throw new Exception("当前Excel文件已打开，请关闭后再导入");
                }
                else
                {
                    filestream = new FileStream(upFilePath, FileMode.Open, FileAccess.Read);                   
                    workbook = new XSSFWorkbook(filestream);
                    filestream.Close();
                }
            }

            //得到所有表名，即sheet页名称
            List<string> templateTableList = new List<string>();
            foreach (DataRow dr in templatedt.Rows)
            {
                if (!templateTableList.Contains(dr["tablename"].ToString()))
                {
                    templateTableList.Add(dr["tablename"].ToString());
                }
            }

            #region 检测excel sheet页名称是否全部来自模板中
            bool existflag = true;
            for (int i = 1; i < workbook.NumberOfSheets; i++)
            {
                string sheetname = workbook.GetSheetName(i);
                if (!templateTableList.Contains(sheetname))
                {
                    existflag = false;
                    break;
                }
            }
            if (!existflag)
            {
                success = false;
                throw new Exception("Excel文件与模板不符，请检查要导入的Excel文件");
            }
            #endregion


            foreach (string sheetName in templateTableList)
            {
                ISheet workSheet = workbook.GetSheet(sheetName);
                if (workSheet == null)//可能某个表所有列都是非必选的
                {
                    continue;
                }
                int culumnCount = templatedt.Select("tablename='" + sheetName + "'").Length;
                DataTable dt = new DataTable(sheetName);
                for (int i = 0; i < culumnCount; i++)
                {
                    ICell cell = workSheet.GetRow(0).GetCell(i);
                    if (cell == null) break;
                    if (cell.CellStyle.FillForegroundColor != HSSFColor.Yellow.Index) break;
                    dt.Columns.Add(cell.StringCellValue);
                }
                for (int j = 1; j < workSheet.LastRowNum + 1; j++)
                {
                    DataRow dr = dt.NewRow();
                    bool flag = false;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        object currentCellValue;
                        if (workSheet.GetRow(j) == null) break;
                        if (workSheet.GetRow(j).GetCell(i) == null)
                        {
                            currentCellValue = string.Empty;
                        }
                        else
                        {
                            switch (workSheet.GetRow(j).GetCell(i).CellType)
                            {

                                case CellType.Boolean:
                                    currentCellValue = workSheet.GetRow(j).GetCell(i).BooleanCellValue;
                                    break;
                                case CellType.Error:
                                    currentCellValue = string.Empty;
                                    break;
                                case CellType.Numeric:
                                //currentCellValue = workSheet.GetRow(j).GetCell(i).NumericCellValue;
                                //break;
                                case CellType.Formula:
                                case CellType.Blank:
                                case CellType.String:
                                case CellType.Unknown:
                                default:
                                    workSheet.GetRow(j).GetCell(i).SetCellType(CellType.String);
                                    currentCellValue = workSheet.GetRow(j).GetCell(i).StringCellValue;
                                    break;
                            }
                        }
                        if (currentCellValue.ToString() != string.Empty)
                            flag = true;
                        dr[i] = currentCellValue;
                    }
                    if (flag)
                    {
                        dt.Rows.Add(dr);
                    }
                    else
                    {
                        break;
                    }
                }

                ds.Tables.Add(dt);
            }
            return ds;
        }

        /// <summary>
        /// 通过单sheet页导出的excel模板中获取数据
        /// </summary>
        /// <param name="templatedt"></param>
        /// <param name="success"></param>
        /// <returns></returns>
        private DataSet GetDataTableFromExcelSingleSheet(DataTable templatedt, ref bool success)
        {
            DataSet ds = new DataSet();
            //取文件路径
            //string file = upFilePath;
            string file = UpFilePath;//从缓存中获取
            if (!File.Exists(file))
            {
                success = false;
                throw new Exception("请选择文件");
                //MessageBox.Show("请选择文件");
            }
            IWorkbook workbook;
            FileStream filestream = null;
            try
            {
                using (filestream = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    workbook = new HSSFWorkbook(filestream);
                }
            }
            catch
            {
                if (filestream == null)
                {
                    success = false;
                    throw new Exception("当前Excel文件已打开，请关闭后再导入");
                }
                else
                {
                    filestream = new FileStream(file, FileMode.Open, FileAccess.Read);
                    workbook = new XSSFWorkbook(filestream);
                    filestream.Close();
                }
            }
            //得到所有表名，即sheet页名称
            List<string> templateTableList = GetTemplateTableName(templatedt);

            #region 检测excel sheet页名称是否全部来自模板中
            bool existflag = true;
            List<string> tableNames = new List<string>();
            IRow row = workbook.GetSheetAt(workbook.ActiveSheetIndex).GetRow(0);
            for (int ColumnNum = 0; ColumnNum < row.Cells.Count; ColumnNum++)
            {
                ICell cell = row.GetCell(ColumnNum);
                if (cell.CellStyle.FillForegroundColor == HSSFColor.Yellow.Index)
                {
                    string tableName = cell.StringCellValue.Split('|')[1];
                    if (!tableNames.Contains(tableName))
                    {
                        tableNames.Add(tableName);
                    }
                }

            }
            for (int i = 0; i < tableNames.Count; i++)
            {
                string tableName = tableNames[i];
                if (!templateTableList.Contains(tableName))
                {
                    existflag = false;
                    break;
                }
            }
            if (!existflag)
            {
                success = false;
                throw new Exception("Excel文件与模板不符，请检查要导入的Excel文件");
            }
            #endregion

            ISheet workSheet = workbook.GetSheetAt(workbook.ActiveSheetIndex);

            foreach (string sheetName in templateTableList)
            {
                int startNum = 0;
                for (int i = 0; i < workSheet.GetRow(0).LastCellNum; i++)
                {
                    ICell cell = workSheet.GetRow(0).GetCell(i);
                    if (cell == null) break;
                    if (cell.CellStyle.FillForegroundColor != HSSFColor.Yellow.Index) continue;
                    string[] cellstring = cell.StringCellValue.Split('|');
                    if (cellstring[cellstring.Length - 1].Equals(sheetName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        startNum = i;
                        break;
                    }
                }
                if (workSheet == null)//可能某个表所有列都是非必选的
                {
                    continue;
                }
                int culumnCount = templatedt.Select("tablename='" + sheetName + "'").Length;
                DataTable dt = new DataTable(sheetName);
                for (int i = startNum; i < workSheet.GetRow(0).LastCellNum; i++)
                {
                    ICell cell = workSheet.GetRow(0).GetCell(i);
                    if (cell == null) break;
                    if (cell.CellStyle.FillForegroundColor != HSSFColor.Yellow.Index) continue;
                    string[] cellstring = cell.StringCellValue.Split('|');
                    if (cellstring[cellstring.Length - 1].Equals(sheetName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        dt.Columns.Add(cellstring[0]);
                    }
                    else
                    {
                        break;
                    }
                    //dt.Columns.Add(cell.StringCellValue);
                }

                for (int j = 1; j < workSheet.LastRowNum + 1; j++)
                {
                    int currentNum = startNum;
                    DataRow dr = dt.NewRow();
                    bool flag = false;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        object currentCellValue = null;
                        if (workSheet.GetRow(j) == null) break;

                        if (workSheet.GetRow(0).GetCell(currentNum) == null)
                        {
                            //一行数据取完
                            break;
                        }

                        if (workSheet.GetRow(0).GetCell(currentNum).CellStyle.FillForegroundColor != HSSFColor.Yellow.Index)
                        {
                            //这一行是注释
                            currentNum++;
                        }
                        if (workSheet.GetRow(j).GetCell(currentNum) == null)
                        {
                            currentCellValue = string.Empty;
                        }
                        else
                        {
                            switch (workSheet.GetRow(j).GetCell(currentNum).CellType)
                            {
                                case CellType.Boolean:
                                    currentCellValue = workSheet.GetRow(j).GetCell(currentNum).BooleanCellValue;
                                    break;
                                case CellType.Error:
                                    currentCellValue = string.Empty;
                                    break;
                                case CellType.Numeric:
                                    ICell cell = workSheet.GetRow(j).GetCell(currentNum);
                                    if (cell.CellStyle.DataFormat != 0)
                                    {
                                        try
                                        {
                                            currentCellValue = cell.DateCellValue.ToString();
                                        }
                                        catch
                                        {
                                            currentCellValue = cell.NumericCellValue.ToString();
                                        }
                                    }
                                    else
                                    {
                                        cell.SetCellType(CellType.String);
                                        currentCellValue = workSheet.GetRow(j).GetCell(currentNum).StringCellValue;
                                    }
                                    break;
                                case CellType.Formula:
                                case CellType.Blank:
                                case CellType.String:
                                case CellType.Unknown:
                                default:
                                    workSheet.GetRow(j).GetCell(currentNum).SetCellType(CellType.String);
                                    currentCellValue = workSheet.GetRow(j).GetCell(currentNum).StringCellValue;
                                    break;
                            }

                        }
                        currentNum++;
                        if (currentCellValue.ToString() != string.Empty)
                            flag = true;
                        dr[i] = currentCellValue;
                    }
                    if (flag)
                    {
                        dt.Rows.Add(dr);
                    }
                    else
                    {
                        //break;
                    }
                }
                ds.Tables.Add(dt);
            }
            return ds;
        }

       

        private List<KeyValuePair<string, string>> GetHelpData(List<string> helpIdList)
        {
            List<KeyValuePair<string, string>> helpData = new List<KeyValuePair<string, string>>();
            foreach (string helpId in helpIdList)
            {
                if (!string.IsNullOrEmpty(helpId))
                {
                    if (helpId.ToLower().StartsWith("customdata"))//静态下拉数据源
                    {
                        helpData.Add(new KeyValuePair<string, string>("code", "name"));
                    }
                    else
                    {
                        helpData.Add(dac.GetHelpData(helpId));
                    }
                }
                else
                {
                    helpData.Add(new KeyValuePair<string, string>(null, null));
                }
            }
            return helpData;
        }

        /// <summary>
        /// 转换combox数据,格式为: 1|个人,0|企业组织,2|非企业组织
        /// </summary>
        /// <param name="comboData"></param>
        /// <param name="firstSplitChar"></param>
        /// <param name="secondSplitChar"></param>
        /// <returns></returns>
        public static JArray TranslateData(string comboData, char firstSplitChar, char secondSplitChar)
        {
            if (comboData.IndexOf(firstSplitChar) < 0 && comboData.IndexOf(secondSplitChar) < 0)
            {
                throw new Exception("下拉数据CustomData的格式不正确，格式应如:1|男,0|女");
            }
            string[] arr = comboData.Split(firstSplitChar);

            JArray jarr = new JArray();
            //List<KeyValueEntity> list = new List<KeyValueEntity>();
            foreach (string item in arr)
            {
                string[] strArr = item.Split(secondSplitChar);
                jarr.Add(new JObject() { { "code", strArr[0] }, { "name", strArr[1] } });
            }

            return jarr;
            //return JsonConvert.SerializeObject(list);
        }

        /// <summary>
        /// 下拉框的渲染器
        /// </summary>
        /// <returns></returns>
        public string GetComboxRenderer()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" function (val, cell, record, rowIndex, colIndex, gridstore) { ");
            sb.Append(" var col = this.columns[colIndex];");
            sb.Append(" var store = col.getEditor().getStore();");
            sb.Append(" var ret; ");
            sb.Append(" var index = store.find('code', val);");
            sb.Append(" var record = store.getAt(index);");
            sb.Append("  if (record) { ");
            sb.Append("     ret = record.data.name;");
            sb.Append("  } ");
            sb.Append(" return ret; }");
            return sb.ToString();
        }

        /// <summary>
        /// 日期的渲染器
        /// </summary>
        /// <returns></returns>
        public string GetDateRenderer()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" function (val) {  ");
            sb.Append("  if (val) { ");
            sb.Append("  var str = Ext.util.Format.date(val, 'Y-m-d');");
            //sb.Append("  alert(str); ");
            sb.Append("  return str; ");
            sb.Append("  } else {");
            sb.Append("  return '';");
            sb.Append("  } ");
            sb.Append("}");          
            return sb.ToString();
        }

        #endregion


        #region 单据数据保存

        /// <summary>
        /// 调用插件，保存数据到数据库中。
        /// </summary>
        /// <param name="ds"></param>
        public bool Save(DataSet ds, ref string message, string selected)
        {
            //string pluginname = dac.GetPluginInfo(templateId);
            string pluginname = dac.GetPluginInfo(TemplateId);

            object[] pars = new object[3];
            pars[0] = ds;
            switch (selected)
            {
                case "0":
                    pars[1] = "0";
                    break;
                case "1":
                    pars[1] = "1";
                    break;
                case "2":
                    pars[1] = "2";
                    break;
            }
            //添加登入实体
            AddCurrentInfo(ds);

            return ReflectPlugin(pluginname, pars, ref message);

        }


        /// <summary>
        /// 反射插件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ds"></param>
        private bool ReflectPlugin(string pluginname, object[] pars, ref string message)
        {
            //string dllpath =  AppDomain.CurrentDomain.BaseDirectory + string.Format("/I6Rules/{0}", dllname);

            bool result = false;
            try
            {
                string binPath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + @"Bin\";
                string rulePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + @"I6Rules\";
                int n = pluginname.LastIndexOf('.');
                int lenth = pluginname.Trim().Length;
                string classname = pluginname.Substring(n + 1, lenth - n - 1);
                rulePath = rulePath + pluginname.Substring(0, n) + ".dll";//i6Rule目录
                binPath = binPath + pluginname.Substring(0, n) + ".dll";//bin目录

                string path = string.Empty;
                if (File.Exists(rulePath))//优先搜索i6Rule目录
                {
                    path = rulePath;
                }
                else
                {
                    path = binPath;
                }

                if (File.Exists(path))
                {
                    Assembly asbly = Assembly.LoadFile(path);
                    Type classType = asbly.GetType(pluginname);
                    object obj = Activator.CreateInstance(classType);
                    if (obj is SUP.Common.Interface.Excel.IExcelImportPlugin)
                    {
                        MethodInfo method = classType.GetMethod("ImportBill");
                        if (method != null)
                        {
                            result = (bool)method.Invoke(obj, pars);
                            message = pars[2] as string;
                        }
                        else
                        {
                            message = "插件未实现ImportBill方法，或者ImportBill方法不是public的";
                        }

                    }
                    else
                    {
                        message = "插件" + pluginname + "未实现SUP.Common.Interface.Excel.IExcelImportPlugin接口的ImportBill方法!";
                    }
                    //return result;
                }
                else
                {
                    message = "导入失败，插件不存在！";
                    //return result;
                }
                return result;
            }
            catch (Exception ex)
            {
                string msg = string.Empty;
                if (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message)) {
                    msg += "\r\n" + ex.InnerException.Message;
                }
                msg += ex.Message;
                message = "插件：" + pluginname + "遇到异常：" + msg;
                Logger.Error("插件处理异常：" + ex.StackTrace);
                return result;
            }

        }


        /// <summary>
        /// 添加登陆实体
        /// </summary>
        /// <param name="ds"></param>
        private void AddCurrentInfo(DataSet ds)
        {
            DataTable dt = new DataTable("CurrentInfo");
            dt.Columns.Add("loginid");
            dt.Columns.Add("username");
            dt.Columns.Add("ocode");
            dt.Columns.Add("ucode");
            dt.Columns.Add("product");
            DataRow dr = dt.NewRow();
            dr["loginid"] = NG3.AppInfoBase.LoginID; //i6AppInfoEntity.LoginID;
            dr["username"] = NG3.AppInfoBase.UserName;// i6AppInfoEntity.UserName;
            dr["ocode"] = NG3.AppInfoBase.OCode; //i6AppInfoEntity.OCode;
            dr["ucode"] = NG3.AppInfoBase.UCode; //i6AppInfoEntity.UCode;
            //dr["product"] = i6AppInfoEntity.Product + i6AppInfoEntity.Series;
            dt.Rows.Add(dr);
            ds.Tables.Add(dt);
        }

        #endregion



        /// <summary>
        /// 获取数据库表不存在的列，拼装到select语句中
        /// </summary>
        /// <param name="json"></param>
        /// <param name="tname"></param>
        /// <returns>select '' myname,* from tname</returns>
        public string GetSelectSql(string json, string tname)
        {
            DataTable dt = dac.GetTableInfo(tname);

            JObject jo = JsonConvert.DeserializeObject(json) as JObject;
            JToken newRow = jo["table"]["newRow"];

            bool haveData = false;//判断是否有数据

            StringBuilder strb = new StringBuilder("select ");
            if (newRow is JArray)
            {
                JArray ja = newRow as JArray;
                if (ja.Count > 0)
                {
                    haveData = true;
                }

                JToken jt = newRow[0]["row"];
                foreach (JProperty item in jt)
                {
                    //JProperty jObj = item as JProperty;
                    if (!dt.Columns.Contains(item.Name))//不存在的字段也加到表中
                    {
                        if ((item.Name != "key") && !item.Name.EndsWith("_ngname"))//关键字不能用
                        {
                            strb.Append(string.Format(" '' {0},", item.Name));//假列
                        }
                    }
                    else
                    {
                        strb.Append(string.Format("{0},", item.Name));//表里存在的
                    }
                }
            }
            //strb.Append(" * from " + tname);          

            string s = string.Empty;
            if (haveData)
            {
                s = strb.ToString().TrimEnd(',') + "  from " + tname;
            }
            else
            {
                s = "select *  from " + tname;
            }

            return s;
        }
    }
}
