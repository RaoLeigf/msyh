using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NG3.Data.Service;
using SUP.Common.Base;
using SUP.Common.DataEntity;
using NG3.Data;

namespace SUP.Common.DataAccess
{
    public class PrintDac
    {
        private string PubConnectString = "";  //连接串
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_pubConnectString"></param>
        public PrintDac(string _pubConnectString)
        {
            this.PubConnectString = _pubConnectString;
        }

        #region
        /// <summary>
        ///  创建套件模块树
        /// </summary>
        /// <returns></returns>
        public IList<Base.TreeJSONBase> BuildSuiteModuleTree(string product)
        {
            DataTable dt = GetSuiteModules();
            return new PrintTreeBuilder().GetExtTreeList(dt, "pid", "id", "pid='-1'", TreeDataLevelType.TopLevel); 
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetSuiteModules()
        {
            string product = "";
            string query = " 1=1 ";
            if (!string.IsNullOrEmpty(product))
            {
                 query+= " and product = " + NG3.Data.DbConvert.ToSqlString(product);
            }
            else if (NG3.AppInfoBase.UP != null)
            {
                product = NG3.AppInfoBase.UP.Product;
                if (!string.IsNullOrEmpty(NG3.AppInfoBase.UP.Series))
                {
                    product += NG3.AppInfoBase.UP.Series;
                }
                query += " and product = " + NG3.Data.DbConvert.ToSqlString(product);
            }
            string sql = "SELECT  DISTINCT suitno id,'-1' pid,suitname name,0 ismodule, 0 porderid,suitorder orderid FROM ngproducts where " + query +
 " UNION SELECT  moduleno id, suitno pid,modulename name,1 ismodule,suitorder porderid, moduleorder orderid FROM ngproducts WHERE " + query +
 " ORDER BY porderid,orderid";
            DataTable dt = DbHelper.GetDataTable(PubConnectString, sql);
            return dt;
        }
        #endregion

        #region 获取数据
        
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
            sb.Append("(ispub='0' and def_str1=").Append(DbConvert.ToSqlString(NG3.AppInfoBase.UCode)).Append(")").Append(" or ");
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
        public DataTable GetLFormList(DataTable dtModules,string clientJson,int pageSize, int pageIndex, ref int totalRecord)
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
                string sqlstr = PaginationAdapter.GetPageDataSql(sql, pageSize, ref pageIndex, ref totalRecord, sortField, p,PubConnectString);
                return DbHelper.GetDataTable(PubConnectString, sqlstr, p);
            }
            else
            {
                string sqlstr = PaginationAdapter.GetPageDataSql(sql, pageSize, ref pageIndex, ref totalRecord, sortField,null, PubConnectString);
                return DbHelper.GetDataTable(PubConnectString, sqlstr);
            }
        }

        /// <summary>
        /// 返回套打帮助列表数据
        /// </summary>
        /// <param name="typeFile"></param>
        /// <returns></returns>
        public DataTable GetFmtTemplateFromDB(DataTable dtModules,string typeFile)
        {
            string sqlstr = @"select printid,typefile,billname,moduleno,filename,dateflg,def_int2,remarks,previeweditflg,def_int1
                            from printfm where typefile='" + typeFile + "' and " + GetDefaultSqlFilter(dtModules) + " and (hide is null or hide<>'1') order by printid ";
            return DbHelper.GetDataTable(PubConnectString, sqlstr);
        }

        /// <summary>
        /// 通过打印ID获取套打模板
        /// </summary>
        /// <param name="printId"></param>
        /// <returns></returns>
        public DataTable GetTemplateById(string printId)
        {
            string sqlstr = @"select * from printfm where printid=" + NG3.Data.DbConvert.ToSqlString(printId);
            return DbHelper.GetDataTable(PubConnectString, sqlstr);
        }

        /// <summary>
        /// 通过typefile获取套打模板
        /// </summary>
        /// <param name="printId"></param>
        /// <returns></returns>
        public DataTable GetePdfTemplatByTypeFile(string typefile)
        {
            string sqlstr = @"select * from printfm where typefile="+NG3.Data.DbConvert.ToSqlString(typefile);
            return DbHelper.GetDataTable(PubConnectString, sqlstr);
        }
        #endregion

        #region 保存用户自定义模板
        /// <summary>
        /// 保存用户自定义模板
        /// </summary>
        /// <param name="printId"></param>
        /// <param name="typeFile"></param>
        /// <param name="mTitle"></param>
        /// <param name="bTemp"></param>
        /// <param name="moduleno">模块</param>
        /// <returns></returns>
        public string SaveUserDefTemplate(ref string printId, string typeFile, string mTitle, byte[] bTemp, ref string moduleno)
        {
            if (!isCanUpdate(PubConnectString, printId))
            {
                return "";
            }
            string fname = "";
            moduleno = "Other";
            if (!string.IsNullOrEmpty(typeFile)&&typeFile.IndexOf("_") > 0)
            {
                moduleno = typeFile.Split('_')[0].ToUpper();
            }
            string sqlstr = @"select * from printfm where printid="+NG3.Data.DbConvert.ToSqlString(printId);
            DataTable tmpDT = DbHelper.GetDataTable(PubConnectString, sqlstr);
            if (tmpDT.Rows.Count == 0)
            {
                #region 新增
                sqlstr = "select printid from printfm order by printid desc";
                DataTable dt = DbHelper.GetDataTable(PubConnectString, sqlstr);
                int sPrintId = 1;
                if (dt.Rows.Count > 0)
                {
                    sPrintId = Convert.ToInt32(dt.Rows[0]["printid"]) + 1;
                }
                printId = sPrintId.ToString();
                DataRow dr = tmpDT.NewRow();
                dr["printid"] = sPrintId;
                dr["typefile"] = typeFile;
                dr["moduleno"] = moduleno;
                dr["billname"] = mTitle;
                dr["remarks"] = "";
                dr["def_int2"] = "2";
                dr["def_str1"] = NG3.AppInfoBase.UCode;
                dr["def_str2"] = NG3.AppInfoBase.LoginID;
                dr["def_str3"] = "supcan";
                fname = mTitle + sPrintId + "-自定义模板.xml";
                dr["filename"] = fname;
                if (bTemp != null && bTemp.Length > 0)
                {
                    string xmlDoc = Convert.ToBase64String(bTemp, 0, bTemp.Length);
                    xmlDoc = xmlDoc.Replace("*", "/");
                    bTemp = Convert.FromBase64String(xmlDoc);
                }
                dr["bfile"] = bTemp;
                dr["iscompress"] = "0";
                dr["dateflg"] = System.DateTime.Now.ToString();
                tmpDT.Rows.Add(dr);
                #endregion
            }
            else
            {
                #region 修改
                tmpDT.Rows[0]["printid"] = printId;
                tmpDT.Rows[0]["typefile"] = typeFile;
                tmpDT.Rows[0]["moduleno"] = moduleno;
                tmpDT.Rows[0]["def_int2"] = "2";
                tmpDT.Rows[0]["def_str1"] = NG3.AppInfoBase.UCode;
                tmpDT.Rows[0]["def_str2"] = NG3.AppInfoBase.LoginID;
                tmpDT.Rows[0]["def_str3"] = "supcan";
                fname = tmpDT.Rows[0]["filename"].ToString();
                if (bTemp != null && bTemp.Length > 0)
                {
                    string xmlDoc = Convert.ToBase64String(bTemp, 0, bTemp.Length);
                    xmlDoc = xmlDoc.Replace("*", "/");
                    bTemp = Convert.FromBase64String(xmlDoc);
                }
                tmpDT.Rows[0]["bfile"] = bTemp;
                tmpDT.Rows[0]["iscompress"] = "0";
                tmpDT.Rows[0]["dateflg"] = System.DateTime.Now.ToString();
                #endregion
            }
            DbHelper.Update(PubConnectString, tmpDT, "select * from printfm");
            return fname;
        }

        /// <summary>
        /// 是否允许更新
        /// </summary>
        /// <param name="PubConnectString"></param>
        /// <param name="printId"></param>
        /// <returns></returns>
        private bool isCanUpdate(string PubConnectString, string printId)
        {
            if (string.IsNullOrEmpty(printId))
            {
                return true;
            }
            else
            {
                string sqlstr = @"select printid,def_int2 from printfm where printid='" + printId + "'";
                DataTable tmpDT = DbHelper.GetDataTable(PubConnectString, sqlstr);
                if (tmpDT.Rows[0]["def_int2"].ToString() == "2")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion

        #region 模板导入  
        public int GetPrintId(string fileName, string def_int2,string isnew="")
        {
            if (!string.IsNullOrEmpty(fileName) && fileName.IndexOf("[") > 0 && fileName.IndexOf("]") > 0)
            {
                string sqlstr = " select max(printid) from printfm where filename='" + fileName + "' and def_int2=" + def_int2 + " and def_str3='supcan'";
                if (!string.IsNullOrEmpty(isnew)&&isnew=="0")
                {
                    sqlstr = sqlstr + " and (isnew is null or isnew='0' or isnew='')";
                }
                object obj = DbHelper.ExecuteScalar(PubConnectString, sqlstr);
                if (obj==DBNull.Value||obj==null||string.IsNullOrEmpty(obj.ToString()))
                {
                    return -1;
                }
                else
                {
                    return Convert.ToInt32(obj);
                }
            }
            else   //文件名称不符合系统规范 ****(*_***).xml
            {
                return -1;
            }
        }
        /// <summary>
        /// 检查模板是否需要导入
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="mType"></param>
        /// <returns></returns>
        public bool CheckFtmTemplate(string fileName, string def_int2)
        {            
            if (!string.IsNullOrEmpty(fileName) && fileName.IndexOf("[") > 0 && fileName.IndexOf("]") > 0)
            {
                string sqlstr = "select count(printid) from printfm where  filename='" + fileName + "' and def_int2=" + def_int2 + " and def_str3='supcan'";
                //if (def_int2 == "0") {
                //    sqlstr = "select count(printid) from printfm where  filename='" + fileName + "' and def_int2=" + def_int2 + " and def_str3='supcan' and  def_str1 ="+DbConvert.ToSqlString(NG3.AppInfoBase.UCode);
                //}
                object obj = DbHelper.ExecuteScalar(PubConnectString, sqlstr);
                if (null == obj || obj.ToString() == "" || obj.ToString() == "0")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else   //文件名称不符合系统规范 ****(*_***).xml
            {
                return false;
            }
        }

        public void UpdateFlag()
        {
            DbHelper.ExecuteNonQuery(PubConnectString, "update printfm set hide=0 where  def_str3='supcan' and hide is null");
            DbHelper.ExecuteNonQuery(PubConnectString, "update printfm set ispub=1 where def_str3='supcan' and ispub is null");
        }
        #endregion

        #region 套打管理
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <returns></returns>
    //    public DataTable GetModules()
    //    {
    //        string product = NG3.AppInfoBase.UP.Product;
    //        if (!string.IsNullOrEmpty(NG3.AppInfoBase.UP.Series))
    //        {
    //            product += NG3.AppInfoBase.UP.Series;
    //        }
    //        string sql = @"SELECT  DISTINCT suitno moduleno,suitname modulename,0 ismodule FROM ngproducts WHERE moduleno IN (SELECT distinct moduleno FROM printfm) AND product=" + NG3.Data.DbConvert.ToSqlString(product) +
    //" union SELECT DISTINCT moduleno,modulename,1 ismodule FROM ngproducts WHERE suitno IN (SELECT distinct moduleno FROM printfm) AND product=" + NG3.Data.DbConvert.ToSqlString(product);
    //        return DbHelper.GetDataTable(PubConnectString, sql);
    //    }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        public IList<Base.TreeJSONBase> LoadTplTree(string nodeid)
        {            
            string product=NG3.AppInfoBase.UP.Product;
            if (!string.IsNullOrEmpty(NG3.AppInfoBase.UP.Series))
            {
                product += NG3.AppInfoBase.UP.Series;
            }
            string sql_module = "SELECT moduleno ,modulename FROM ngproducts WHERE product=" + NG3.Data.DbConvert.ToSqlString(product) + " AND moduleno IN (SELECT distinct moduleno FROM printfm) ORDER BY suitorder,moduleorder";

            string sql = @"SELECT printid,typefile, moduleno ,billname FROM printfm WHERE printid IN(
SELECT max(printid) FROM printfm WHERE def_int2=1 GROUP BY moduleno,typefile)";

            DataTable dtModule= DbHelper.GetDataTable(PubConnectString, sql_module);
            DataTable dtP = DbHelper.GetDataTable(PubConnectString, sql);

            DataTable dt = new DataTable();
            dt.Columns.Add("printid", typeof(string));           
            dt.Columns.Add("typefile", typeof(string));
            dt.Columns.Add("moduleno", typeof(string));
            dt.Columns.Add("billname", typeof(string));
            dt.Columns.Add("ntype", typeof(int));
            DataRow newRow = dt.NewRow();
            newRow["printid"] ="0000";          
            newRow["billname"] ="全部";
            newRow["ntype"] = 0;
            dt.Rows.Add(newRow);
            foreach (DataRow dr in dtModule.Rows)
            {
                newRow = dt.NewRow();
                newRow["printid"] = dr["moduleno"];
                newRow["moduleno"] = "0000";
                newRow["billname"] = dr["modulename"];
                newRow["ntype"] = 0;
                dt.Rows.Add(newRow);
            }
            foreach (DataRow dr in dtP.Rows)
            {
                newRow = dt.NewRow();
                newRow["printid"] = dr["printid"];
                newRow["moduleno"] = dr["moduleno"];
                newRow["typefile"] = dr["typefile"];
                newRow["billname"] = dr["billname"];
                newRow["ntype"] = 1;
                dt.Rows.Add(newRow);
            }
            string filter = "(moduleno is null or moduleno='')";          
            return new PrintTreeBuilder().GetExtTreeList(dt, "moduleno", "printid", filter, TreeDataLevelType.TopLevel);
        }
        /// <summary>
        /// 删除模板
        /// </summary>
        /// <param name="printId"></param>
        public int DeleteModule(string printId)
        {
            string sqlstr = @"delete from printfm where printid='" + printId + "'";
            return DbHelper.ExecuteNonQuery(PubConnectString, sqlstr);
        }
               
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowData"></param>
        /// <param name="printId"></param>
        /// <returns></returns>
        public string Update(string printId, Dictionary<string, object> rowData,ref string oldXmlFileName,ref string fileName,bool isImport=false)
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
                if (!isImport)
                {
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

        public string Insert(Dictionary<string, object> rowData,ref string fileName,bool isImport)
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
            if (!isImport)
            {
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
        /// <param name="printId"></param>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        public void UpdateStatus(string printId, string paramName, string paramValue)
        {
            DbHelper.ExecuteNonQuery(PubConnectString, "update printfm set " + paramName + "=" + DbConvert.ToSqlString(paramValue) + " where printid=" + printId);
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
        public string InsertTemplate(string moduleno, string typefile, string billname,ref string fileName, byte[] bTemp, string def_int2, string def_str3="supcan" ,string iscompress="0", string remarks = "")
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
            return Insert(rowData,ref fileName,true);
        }
        /*
        /// <summary>
        /// 新增用户模板
        /// </summary>
        /// <param name="moduleNo"></param>
        /// <param name="typeFile"></param>
        /// <param name="billName"></param>
        /// <param name="fileName"></param>
        /// <param name="remarks"></param>
        /// <param name="bTemp"></param>
        /// <returns></returns>
        public string AddLformTemplate(string moduleNo, string typeFile, string billName, ref string fileName, string remarks, byte[] bTemp)
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
            dr["printid"] = sPrintId;
            dr["typefile"] = typeFile;
            dr["moduleno"] = moduleNo;
            dr["billname"] = billName;
            dr["remarks"] = remarks;
            dr["def_int2"] = "0";
            dr["def_str3"] = "supcan";
            fileName = billName + sPrintId + "[" + typeFile + "].xml";
            dr["filename"] = fileName;
            string xmlDoc = Convert.ToBase64String(bTemp, 0, bTemp.Length);
            xmlDoc = xmlDoc.Replace("*", "/");
            bTemp = Convert.FromBase64String(xmlDoc);
            dr["bfile"] = bTemp;
            dr["iscompress"] = "0";
            dr["dateflg"] = System.DateTime.Now.ToString();
            tmpDT.Rows.Add(dr);
            if (DbHelper.Update(PubConnectString, tmpDT, "select * from printfm") > 0)
            {
                return sPrintId.ToString();
            }
            return "";
        }
        */
        #endregion

        #region 打印设置
        /// <summary>
        /// 保存打印设置信息
        /// </summary>
        /// <param name="ctype"></param>
        /// <param name="PrintPage"></param>
        public void SetPrintSetup(string ctype, string PrintPage)
        {
            string logid = NG3.AppInfoBase.LoginID;
            string sqlstr = "select * from c_sys_userparm where c_type='" + ctype + "' and logid='" + logid + "' and c_name='print'";
            DataTable tmpDT = DbHelper.GetDataTable(PubConnectString, sqlstr);
            if (tmpDT.Rows.Count == 0)
            {
                sqlstr = "select c_code from c_sys_userparm order by c_code desc";
                DataTable dt = DbHelper.GetDataTable(PubConnectString, sqlstr);
                int c_code = 1;
                if (dt.Rows.Count > 0)
                {
                    c_code = Convert.ToInt32(dt.Rows[0]["c_code"]) + 1;
                }
                DataRow dr = tmpDT.NewRow();
                dr["c_code"] = c_code.ToString("0000000000");
                dr["logid"] = logid;
                dr["c_name"] = "print";
                dr["c_value"] = PrintPage;
                dr["c_type"] = ctype;
                tmpDT.Rows.Add(dr);
            }
            else
            {
                tmpDT.Rows[0]["c_value"] = PrintPage;
            }
            DbHelper.Update(PubConnectString, tmpDT, "select * from c_sys_userparm");
        }

        /// <summary>
        /// 获取打印设置信息
        /// </summary>
        /// <param name="ctype"></param>
        public string GetPrintSetup(string ctype)
        {
            string logid = NG3.AppInfoBase.LoginID;
            string sqlstr = "select c_value from c_sys_userparm where c_type='" + ctype + "' and logid='" + logid + "' and c_name='print'";
            DataTable tmpDT = DbHelper.GetDataTable(PubConnectString, sqlstr);
            if (tmpDT.Rows.Count == 0)
            {
                return "";
            }
            else
            {
                return tmpDT.Rows[0]["c_value"].ToString();
            }
        }
        #endregion
    }
    class PrintTreeJSONBase : TreeJSONBase
    {
        public virtual string pid { get; set; }
        public virtual string typefile { get; set; }
        public virtual int ntype { get; set; }
    }
    public class PrintTreeBuilder : ExtJsTreeBuilderBase
    {
        public override TreeJSONBase BuildTreeNode(DataRow dr)
        {
            PrintTreeJSONBase node = new PrintTreeJSONBase();
            if (dr != null)
            {
                node.pid = dr["moduleno"]==DBNull.Value?"": dr["moduleno"].ToString();
                node.expanded = string.IsNullOrEmpty(node.pid);
                node.id = dr["printid"] == DBNull.Value ? "":dr["printid"].ToString();
                node.text = dr["billname"] == DBNull.Value ? "": dr["billname"].ToString();
                node.typefile = dr["typefile"] == DBNull.Value ? "":dr["typefile"].ToString();
                node.ntype = dr["ntype"] == DBNull.Value ?0:Convert.ToInt32(dr["ntype"]);
            }
            return node;
        }
    }
}
