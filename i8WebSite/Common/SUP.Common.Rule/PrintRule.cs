using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SUP.Common.DataAccess;
using System.IO;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary; 

namespace SUP.Common.Rule
{
    public class PrintRule
    {
        #region 获取数据
        /// <summary>
        /// 获取打印相关模块
        /// </summary>
        /// <param name="PubConnectString"></param>
        /// <returns></returns>
        public DataTable GetModules(string conn)
        {
            return new PrintDac(conn).GetSuiteModules(); ;
        }
        /// <summary>
        /// 返回套打列表数据
        /// </summary>
        /// <param name="PubConnectString"></param>
        /// <param name="clientJson"></param>
        /// <param name="pageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public DataTable GetLFormList(string PubConnectString, DataTable dtModules, string clientJson, int pageSize, int PageIndex, ref int totalRecord)
        {
            return new PrintDac(PubConnectString).GetLFormList( dtModules, clientJson, pageSize, PageIndex, ref totalRecord); ;
        }

        /// <summary>
        /// 返回套打帮助列表数据
        /// </summary>
        /// <param name="PubConnectString"></param>
        /// <param name="typeFile"></param>
        /// <returns></returns>
        public DataTable GetFmtTemplateFromDb(string PubConnectString, DataTable dtModuels,string typeFile)
        {
            return new PrintDac(PubConnectString).GetFmtTemplateFromDB(dtModuels, typeFile);
        }

        /// <summary>
        /// 通过打印ID获取套打模板的路径
        /// </summary>
        /// <param name="PubConnectString"></param>
        /// <param name="printId"></param>
        /// <param name="tmpDir"></param>
        /// <returns></returns>
        public string GetTemplateById(string PubConnectString, string printId, string tmpDir,string fileName = "")
        {
            DataTable dTb = new PrintDac(PubConnectString).GetTemplateById(printId);
            if (dTb.Rows.Count > 0)
            {
                byte[] bTemp = (byte[])dTb.Rows[0]["bfile"];
                string sStr = Convert.ToBase64String(bTemp, 0, bTemp.Length);
                sStr = sStr.Replace("*", "/");
                bTemp = Convert.FromBase64String(sStr);
                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = dTb.Rows[0]["filename"].ToString();  
                }
                string sPath = Path.Combine(tmpDir, fileName);
                if (!Directory.Exists(tmpDir))
                {
                    Directory.CreateDirectory(tmpDir);
                }
                FileStream fsw = new FileStream(sPath, FileMode.Create, FileAccess.Write);
                fsw.Write(bTemp, 0, bTemp.Length);
                fsw.Close();
                return fileName;
            }
            return "";
        }
         /// <summary>
         /// 
         /// </summary>
         /// <param name="PubConnectString"></param>
         /// <param name="typefile"></param>
         /// <param name="tmpDir"></param>
         /// <returns></returns>
        public string GeteTemplatByTypeFile(string PubConnectString, string typefile, string def_int2, string tmpDir, string fileName = "")
        {
            DataTable dTb = new PrintDac(PubConnectString).GetePdfTemplatByTypeFile(typefile);
            if (dTb.Rows.Count > 0)
            {
                byte[] bTemp;                
                DataRow[] drs = dTb.Select("def_int2="+def_int2);
                if (drs != null && drs.Length > 0)
                {
                    bTemp = (byte[])drs[0]["bfile"];
                    if (string.IsNullOrEmpty(fileName))
                    {
                        fileName = drs[0]["filename"].ToString();
                    }
                }
                else
                {
                    fileName = "";
                    return "";
                    //bTemp = (byte[])dTb.Rows[0]["bfile"];
                    //if (string.IsNullOrEmpty(fileName))
                    //{
                    //    fileName = dTb.Rows[0]["filename"].ToString();
                    //}
                }
                string sStr = Convert.ToBase64String(bTemp, 0, bTemp.Length);
                sStr = sStr.Replace("*", "/");
                bTemp = Convert.FromBase64String(sStr);
                string sPath = Path.Combine(tmpDir, fileName);
                if (!Directory.Exists(tmpDir))
                {
                    Directory.CreateDirectory(tmpDir);
                }
                FileStream fsw = new FileStream(sPath, FileMode.Create, FileAccess.Write);
                fsw.Write(bTemp, 0, bTemp.Length);
                fsw.Close();
                return fileName;
            }
            return "";
        }

        /// <summary>
        /// 通过打印ID获取套打模板
        /// </summary>
        /// <param name="PubConnectString"></param>
        /// <param name="printId"></param>
        /// <param name="tmpDir"></param>
        /// <returns></returns>
        public DataTable GetModuleByID(string PubConnectString, string printId, string tmpDir)
        {
            DataTable dTb = new PrintDac(PubConnectString).GetTemplateById(printId);
            if (dTb.Rows.Count > 0)
            {
                byte[] bTemp = (byte[])dTb.Rows[0]["bfile"];
                string sStr = Convert.ToBase64String(bTemp, 0, bTemp.Length);
                sStr = sStr.Replace("*", "/");
                bTemp = Convert.FromBase64String(sStr);
                string fileName = "";
                if (dTb.Rows[0]["filename"] == DBNull.Value || string.IsNullOrEmpty(dTb.Rows[0]["filename"].ToString()))
                {
                    dTb.Rows[0]["filename"] = string.Format("{0}[{1}].xml", dTb.Rows[0]["billname"].ToString(), dTb.Rows[0]["typefile"].ToString());
                }
                fileName = dTb.Rows[0]["filename"].ToString();

                string sPath = Path.Combine(tmpDir, fileName);
                if (!Directory.Exists(tmpDir))
                {
                    Directory.CreateDirectory(tmpDir);
                }
                FileStream fsw = new FileStream(sPath, FileMode.Create, FileAccess.Write);
                fsw.Write(bTemp, 0, bTemp.Length);
                fsw.Close();
                dTb.Columns.Remove("bfile");
                return dTb;
            }
            return null;
        }
        #endregion

        #region 保存用户自定义模板
        /// <summary>
        /// 保存用户自定义模板
        /// </summary>
        /// <param name="PubConnectString"></param>
        /// <param name="printId"></param>
        /// <param name="typeFile"></param>
        /// <param name="mTitle"></param>
        /// <param name="xmlDoc"></param>
        /// <param name="tmpDir"></param>
        /// <returns></returns>
        public string SaveUserDefTemplate(string PubConnectString, ref string printId, string typeFile, string mTitle, string xmlDoc, string tmpDir)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlDoc);

            string fName = Guid.NewGuid().ToString() + ".xml"; //临时文件
            string XmlFileName = Path.Combine(tmpDir, fName);
            if (!Directory.Exists(tmpDir))
            {
                Directory.CreateDirectory(tmpDir);
            }
            doc.Save(XmlFileName);
            FileStream fStream = File.OpenRead(XmlFileName);
            byte[] bTemp = new byte[fStream.Length];
            fStream.Read(bTemp, 0, bTemp.Length);
            fStream.Flush();
            fStream.Close();

            string moduleno = "";
            string newFileName = new PrintDac(PubConnectString).SaveUserDefTemplate(ref printId, typeFile, mTitle, bTemp, ref moduleno);
            if (!string.IsNullOrEmpty(newFileName))
            {
                fName = newFileName;
                string newXmlFileName = Path.Combine(tmpDir, moduleno, newFileName);
                if (File.Exists(newXmlFileName))
                {
                    File.Delete(newXmlFileName);
                }
                else if (!Directory.Exists(Path.Combine(tmpDir, moduleno)))
                {
                    Directory.CreateDirectory(Path.Combine(tmpDir, moduleno));
                }
                File.Move(XmlFileName, newXmlFileName);
            }
            return moduleno + "/" + fName;
        }
        #endregion

        #region 套打管理
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PubConnectString"></param>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        public IList<Base.TreeJSONBase> LoadTplTree(string PubConnectString,string nodeid)
        {
            return new PrintDac(PubConnectString).LoadTplTree(nodeid);
        }
        /// <summary>
        /// 导入模板
        /// </summary>
        /// <param name="PubConnectString"></param>
        /// <param name="sysPath"></param>
        /// <param name="allMoudleType"></param>
        /// <param name="mType"></param>
        /// <param name="errTxt"></param>
        /// <returns></returns>
        public int ImportModule(string PubConnectString, string sysPath, string allMoudleType, string mType, ref string errTxt)
        {
            string[] mArr = allMoudleType.Split(',');
            string tmpPath = "", fileName = "";
            int numCount = 0;
            PrintDac prt = new PrintDac(PubConnectString);
            foreach (string moduleno in mArr)
            {
                if (moduleno.Length == 0) { continue; }
                tmpPath = Path.Combine(sysPath, moduleno);
                DirectoryInfo df = new DirectoryInfo(tmpPath);
                if (df.Exists)
                {
                    FileInfo[] fis = df.GetFiles("*.xml");
                    foreach (FileInfo f in fis)
                    {
                        fileName = f.Name;
                        string def_int2 = "0";
                        switch (mType)
                        {
                            case "sys":
                                def_int2 = "1";
                                break;
                            case "sys_pdf":
                                def_int2 = "3";
                                break;
                            default:
                                def_int2 = "0";
                                break;
                        }
                        if (!prt.CheckFtmTemplate(fileName, def_int2)) { continue; }
                        FileStream fStream = File.OpenRead(f.FullName);
                        byte[] bTemp = new byte[fStream.Length];
                        fStream.Read(bTemp, 0, bTemp.Length);
                        fStream.Flush();
                        fStream.Close();
                        string billname = fileName.Substring(0, fileName.Length - 4);
                        int b = billname.LastIndexOf("[") + 1;
                        int e = billname.Length - 1;
                        string typeFile = billname.Substring(b, e - b);                        
                        billname = billname.Replace("[" + typeFile + "]", "");
                        if (def_int2 == "0") {
                            string ret = prt.Insert(new Dictionary<string, object>() {
                                { "typefile",typeFile},
                                { "moduleno",moduleno},
                                { "billname",billname},
                                { "filename",fileName},
                                { "def_int2",def_int2},
                                { "def_str3","supcan"},
                                { "remarks",""},
                                { "bfile",bTemp},
                                { "iscompress","0"},
                                { "ispub","0"},
                                { "def_str1",NG3.AppInfoBase.UCode},
                                { "def_int1","1"},
                                { "previeweditflg","0"}
                            }, ref fileName,true);
                            if (!string.IsNullOrEmpty(ret))
                            {
                                numCount++;
                            }
                        }
                        else
                        {
                            string ret = prt.InsertTemplate(moduleno, typeFile, billname, ref fileName, bTemp, def_int2);
                            if (!string.IsNullOrEmpty(ret))
                            {
                                numCount++;
                            }
                        }

                    }
                }
            }
            return numCount;
        }

        public int UpdateModule(string PubConnectString, string sysPath, string allMoudleType, string mType, ref string errTxt)
        {
            string[] mArr = allMoudleType.Split(',');
            string tmpPath = "", fileName = "";
            int numCount = 0;
            PrintDac prt = new PrintDac(PubConnectString);
            foreach (string moduleno in mArr)
            {
                if (moduleno.Length == 0) { continue; }
                tmpPath = Path.Combine(sysPath, moduleno);
                DirectoryInfo df = new DirectoryInfo(tmpPath);
                if (df.Exists)
                {
                    FileInfo[] fis = df.GetFiles("*.xml");
                    foreach (FileInfo f in fis)
                    {
                        fileName = f.Name;
                        string def_int2 = "0";
                        switch (mType)
                        {
                            case "sys":
                                def_int2 = "1";
                                break;
                            case "sys_pdf":
                                def_int2 = "3";
                                break;
                            case "sys_pdf_0"://不更新 有更新的系统PDF    isnew=1 系统PDF有更新
                                def_int2 = "3";
                                break;
                            default:
                                def_int2 = "0";
                                break;
                        }
                        int printid = prt.GetPrintId(fileName, def_int2, mType == "sys_pdf_0" ? "0" : "");
                        if (printid != -1)
                        {
                            FileStream fStream = File.OpenRead(f.FullName);
                            byte[] bTemp = new byte[fStream.Length];
                            fStream.Read(bTemp, 0, bTemp.Length);
                            fStream.Flush();
                            fStream.Close();
                            string filename = "";
                            string oldXmlFileName = "";
                            string ret = prt.Update(printid.ToString(), new Dictionary<string, object>() {  
                                        {"bfile",bTemp },                                        
                                        {"isnew",(mType == "sys_pdf"||mType == "sys_pdf_0") ? "0" : "" }

                            }, ref oldXmlFileName, ref filename,true);
                            if (!string.IsNullOrEmpty(ret))
                            {
                                numCount++;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
            return numCount;
        }

        public void UpdateFlag(string PubConnectString)
        {
            //DbHelper.ExecuteNonQuery(PubConnectString, "update printfm set hide=0 where  def_str3='supcan' and hide is null");
            //DbHelper.ExecuteNonQuery(PubConnectString, "update printfm set ispub=1 where def_str3='supcan' and ispub is null");

            new PrintDac(PubConnectString).UpdateFlag();
        }
        /// <summary>
        /// 删除模板
        /// </summary>
        /// <param name="PubConnectString"></param>
        /// <param name="printId"></param>
        /// <returns></returns>
        public int DeleteModule(string PubConnectString, string printId)
        {
            return new PrintDac(PubConnectString).DeleteModule(printId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PubConnectString"></param>
        /// <param name="otype"></param>
        /// <param name="printId"></param>
        /// <param name="moduleno"></param>
        /// <param name="tmpPath"></param>
        /// <param name="xmlDoc"></param>
        /// <param name="rowData"></param>
        /// <returns></returns>
        public string Update(string PubConnectString, string otype,string printId,string moduleno, string tmpPath, string xmlDoc, Dictionary<string, object> rowData)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlDoc);

            string fName = Guid.NewGuid().ToString() + ".xml"; //临时文件
            string XmlFileName = Path.Combine(tmpPath, fName);
            if (!Directory.Exists(tmpPath))
            {
                Directory.CreateDirectory(tmpPath);
            }
            doc.Save(XmlFileName);
            FileStream fStream = File.OpenRead(XmlFileName);
            byte[] bTemp = new byte[fStream.Length];
            fStream.Read(bTemp, 0, bTemp.Length);
            fStream.Flush();
            fStream.Close();
            string oldXmlFileName = "";
            string fileName = "";
            //if (bTemp != null && bTemp.Length > 0)
            //{
            //    string strDoc = Convert.ToBase64String(bTemp, 0, bTemp.Length);
            //    strDoc = strDoc.Replace("*", "/");
            //    bTemp = Convert.FromBase64String(strDoc);
            //}
            rowData.Add("bfile", bTemp);
            if (otype == "add")
            {
                //printId = Insert(PubConnectString,rowData, ref fileName);
                printId = new PrintDac(PubConnectString).Insert(rowData, ref fileName,false);
            }
            else
            {
                //printId = Update(PubConnectString,printId, rowData, ref oldXmlFileName, ref fileName);
                printId = new PrintDac(PubConnectString).Update(printId,rowData,ref oldXmlFileName, ref fileName);
            }
            #region 更新物理用户模板文件
            if (otype == "add" || otype == "edit")
            {
                if (!string.IsNullOrEmpty(fileName))
                {
                    tmpPath = Path.Combine(tmpPath, moduleno);
                    string newXmlFileName = Path.Combine(tmpPath, fileName);                 
                    if (!Directory.Exists(tmpPath))
                    {
                        Directory.CreateDirectory(tmpPath);
                    }
                    if (File.Exists(newXmlFileName))
                    {
                        File.Delete(newXmlFileName);
                    }
                    if (!string.IsNullOrEmpty(oldXmlFileName))
                    {
                        oldXmlFileName = Path.Combine(tmpPath, oldXmlFileName);
                        if (File.Exists(oldXmlFileName))
                        {
                            File.Delete(oldXmlFileName);
                        }
                    }                    
                    File.Move(XmlFileName, newXmlFileName);
                }
            }
            #endregion


            return printId;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="printId"></param>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        public string UpdateStatus(string PubConnectString,string printId, string paramName, string paramValue)
        {
            new PrintDac(PubConnectString).UpdateStatus(printId, paramName, paramValue);
            //DbHelper.ExecuteNonQuery(PubConnectString, "update printfm set " + paramName + "='" + paramValue + "' where printid=" + printId);
             return "";
        }         
       
        #endregion

        #region    测试
        /* 
        /// <summary>
        /// 获取过滤条件
        /// </summary>
        /// <returns></returns>
        private string GetDefaultSqlFilter(DataTable dt)
        {
            List<string> moduleList = new List<string>();
            moduleList.Add("'PUB'");
            foreach (DataRow dr in dt.Rows)
            {
                moduleList.Add(string.Format("'{0}'", dr["id"].ToString()));

            }
            StringBuilder sb = new StringBuilder();
            sb.Append("def_str3='supcan'").Append(" and ");
            sb.Append("moduleno in(").Append(string.Join(",", moduleList)).Append(")").Append(" and ");
            sb.Append("(");
            sb.Append("(ispub='0' and def_str1='").Append(NG3.AppInfoBase.UCode).Append("')").Append(" or ");
            sb.Append("(ispub is null or ispub<>'0' ").Append(")");
            sb.Append(")");
            return sb.ToString();
            //return " (moduleno in (" + sqlFilter + ") and def_str3='supcan' and (def_int2 is null or def_int2<>'2' or def_int2=2 and def_str1='" + NG3.AppInfoBase.UCode + "' and def_str2='" + NG3.AppInfoBase.LoginID + "')) ";
        }


        /// <summary>
        /// 返回套打列表数据
        /// </summary>
        /// <param name="clientJson"></param>
        /// <param name="pageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public DataTable GetLFormList(string PubConnectString, DataTable dtModules, string clientJson, int pageSize, int pageIndex, ref int totalRecord)
        {
            string sql = "select printid,typefile,billname,moduleno,filename,dateflg,def_int2,remarks,previeweditflg, def_int1,ispub,hide from printfm where " + GetDefaultSqlFilter(dtModules);
            string sortField = " order by moduleno,printid";
            if (!string.IsNullOrEmpty(clientJson))
            {
                string query = string.Empty;
                IDataParameter[] p = DataConverterHelper.BuildQueryWithParam(clientJson, ref query);
                if (!string.IsNullOrEmpty(query))
                {
                    sql += " and " + query;
                }
                string sqlstr = PaginationAdapter.GetPageDataSql(sql, pageSize, ref pageIndex, ref totalRecord, sortField, p, PubConnectString);
                return DbHelper.GetDataTable(PubConnectString, sqlstr, p);
            }
            else
            {
                string sqlstr = PaginationAdapter.GetPageDataSql(sql, pageSize, ref pageIndex, ref totalRecord, sortField, null, PubConnectString);
                return DbHelper.GetDataTable(PubConnectString, sqlstr);
            }
        }

        /// <summary>
        /// 返回套打帮助列表数据
        /// </summary>
        /// <param name="typeFile"></param>
        /// <returns></returns>
        public DataTable GetFmtTemplateFromDb(string PubConnectString,DataTable dtModules, string typeFile)
        {
            string sqlstr = @"select printid,typefile,billname,moduleno,filename,dateflg,def_int2,remarks,previeweditflg,def_int1
                            from printfm where typefile='" + typeFile + "' and " + GetDefaultSqlFilter(dtModules) + " and (hide is null or hide<>'1') order by printid ";
            return DbHelper.GetDataTable(PubConnectString, sqlstr);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowData"></param>
        /// <param name="printId"></param>
        /// <returns></returns>
        public string Update(string PubConnectString, string printId, Dictionary<string, object> rowData, ref string oldXmlFileName, ref string fileName)
        {
            string sqlstr = @"select * from printfm where printid='" + printId + "'";
            DataTable tmpDT = DbHelper.GetDataTable(PubConnectString, sqlstr);
            fileName = "";
            if (tmpDT.Rows.Count > 0)
            {
                oldXmlFileName = tmpDT.Rows[0]["filename"].ToString();

                string colname = "";
                foreach (DataColumn col in tmpDT.Columns)
                {
                    colname = col.ColumnName.ToLower();
                    if (rowData.ContainsKey(colname))
                    {
                        tmpDT.Rows[0][colname] = rowData[colname];
                    }
                }
                string billname = tmpDT.Rows[0]["billname"].ToString();
                if (tmpDT.Rows[0]["def_int2"].ToString() == "0") //用户模板
                {
                    fileName = billname + printId + "[" + tmpDT.Rows[0]["typefile"].ToString() + "].xml";
                    tmpDT.Rows[0]["filename"] = fileName;
                }
                else if (tmpDT.Rows[0]["def_int2"].ToString() == "2") //自定义模板
                {
                    fileName = billname + printId + "-自定义模板.xml";
                    tmpDT.Rows[0]["filename"] = fileName;
                }
                else if (tmpDT.Rows[0]["def_int2"].ToString() == "3") //PDF
                {
                    tmpDT.Rows[0]["isnew"] = "1";
                }
                tmpDT.Rows[0]["dateflg"] = DateTime.Now;
                DbHelper.Update(PubConnectString, tmpDT, "select * from printfm");
            }
            return printId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowData"></param>
        /// <returns></returns>

        public string Insert(string PubConnectString, Dictionary<string, object> rowData, ref string fileName)
        {
            string sqlstr = @"select * from printfm where 1>1";
            DataTable tmpDT = DbHelper.GetDataTable(PubConnectString, sqlstr);
            sqlstr = "select printid from printfm order by printid desc";
            DataTable dt = DbHelper.GetDataTable(PubConnectString, sqlstr);
            int sPrintId = 1;
            if (dt.Rows.Count > 0)
            {
                sPrintId = Convert.ToInt32(dt.Rows[0]["printid"]) + 1;
            }
            DataRow dr = tmpDT.NewRow();
            string colname = "";
            dr["printid"] = sPrintId;
            foreach (DataColumn col in tmpDT.Columns)
            {
                colname = col.ColumnName.ToLower();
                if (rowData.ContainsKey(colname))
                {
                    dr[colname] = rowData[colname];
                }
            }
            string billname = dr["billname"].ToString();
            if (dr["def_int2"].ToString() == "0") //用户模板
            {
                fileName = billname + sPrintId + "[" + dr["typefile"].ToString() + "].xml";
                dr["filename"] = fileName;
            }
            else if (dr["def_int2"].ToString() == "2") //自定义模板
            {
                fileName = billname + sPrintId + "-自定义模板.xml";
                dr["filename"] = fileName;
            }
            dr["dateflg"] = DateTime.Now;
            tmpDT.Rows.Add(dr);
            if (DbHelper.Update(PubConnectString, tmpDT, "select * from printfm") > 0)
            {
                return sPrintId.ToString();
            }
            return "";


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleno"></param>
        /// <param name="typefile"></param>
        /// <param name="billname"></param>
        /// <param name="filename"></param>
        /// <param name="def_int2"></param>
        /// <param name="bTemp"></param>
        /// <param name="def_str3"></param>
        /// <param name="iscompress"></param>
        /// <param name="remarks"></param>
        /// <returns></returns>
        public string InsertTemplate(string PubConnectString, string moduleno, string typefile, string billname, ref string fileName, byte[] bTemp, string def_int2, string def_str3 = "supcan", string iscompress = "0", string remarks = "")
        {

            Dictionary<string, object> rowData = new Dictionary<string, object>() {
                { "typefile",typefile},
                { "moduleno",moduleno},
                { "billname",billname},
                { "filename",fileName},
                { "def_int2",def_int2},
                { "def_str3",def_str3},
                { "remarks",remarks},
                { "bfile",bTemp},
                { "iscompress",iscompress}

            };
            return Insert(PubConnectString, rowData, ref fileName);
        }
            */
        #endregion


        #region 打印设置
        /// <summary>
        /// 保存打印设置信息
        /// </summary>
        /// <param name="UserConnectString"></param>
        /// <param name="ctype"></param>
        /// <param name="PrintPage"></param>
        public void SetPrintSetup(string UserConnectString, string ctype, string PrintPage)
        {
            new PrintDac(UserConnectString).SetPrintSetup(ctype, PrintPage);
        }

        /// <summary>
        /// 获取打印设置信息
        /// </summary>
        /// <param name="UserConnectString"></param>
        /// <param name="ctype"></param>
        /// <returns></returns>
        public string GetPrintSetup(string UserConnectString, string ctype)
        {
            return new PrintDac(UserConnectString).GetPrintSetup(ctype);
        }
        #endregion
    }
}
