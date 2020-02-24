using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SUP.Common.Rule;
using NG3.Data.Service;

namespace SUP.Common.Facade
{
    public class PrintFacade
    {
        private string PubConnectString
        {
            get
            {
                string conn = NG3.AppInfoBase.PubConnectString;
                if (string.IsNullOrEmpty(conn))
                {
                    throw new Exception("NGSOFT connection is null");
                }
                return conn;
            }
        }      
        #region 获取数据
        /// <summary>
        /// 获取打印相关模块
        /// </summary>
        /// <returns></returns>
        public DataTable GetModules()
        {
            string UserConnectString = NG3.AppInfoBase.UserConnectString;
            try
            {
                DbHelper.Open(UserConnectString);
                return new PrintRule().GetModules(UserConnectString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbHelper.Close(UserConnectString);
            }
        }
        /// <summary>
        /// 返回套打列表数据
        /// </summary>
        /// <param name="clientJson"></param>
        /// <param name="pageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public DataTable GetLFormList(string clientJson, int pageSize, int pageIndex, ref int totalRecord)
        {
            System.Diagnostics.Debug.Write(string.Format("【打印】\t{0}\t{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "获取模板列表-获取模块"));
            DataTable dt = GetModules();  
            try
            {
                DbHelper.Open();   //分页无法获取DbVender
                System.Diagnostics.Debug.Write(string.Format("【打印】\t{0}\t{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "获取模板列表-获取数据"));
                DbHelper.Open(PubConnectString); 
                return new PrintRule().GetLFormList(PubConnectString, dt,clientJson, pageSize, pageIndex, ref totalRecord);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(string.Format("【打印】\t{0}\t{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "获取模板列表出错"+ex.Message+ex.StackTrace));  
                throw ex;
            }
            finally
            {
                System.Diagnostics.Debug.Write(string.Format("【打印】\t{0}\t{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "获取模板列表-关闭数据库连接"));

                DbHelper.Close(PubConnectString);
                DbHelper.Close();
            }
        }

        /// <summary>
        /// 返回套打帮助列表数据
        /// </summary>
        /// <param name="typeFile"></param>
        /// <returns></returns>
        public DataTable GetFmtTemplateFromDB(string typeFile)
        {
            DataTable dt = GetModules();
            try
            {
                DbHelper.Open(PubConnectString);

                return new PrintRule().GetFmtTemplateFromDb(PubConnectString,dt, typeFile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbHelper.Close(PubConnectString);
            }
        }

        /// <summary>
        /// 通过打印ID获取套打模板的路径
        /// </summary>
        /// <param name="printId"></param>
        /// <param name="tmpDir"></param>
        /// <returns></returns>
        public string GetTemplateById(string printId, string tmpDir, string fileName = "")
        {
           
            try
            {
                DbHelper.Open(PubConnectString);
                return new PrintRule().GetTemplateById(PubConnectString, printId, tmpDir,fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbHelper.Close(PubConnectString);
            }
        }
        /// <summary>
        /// 通过打印ID获取套打模板的路径
        /// </summary>
        /// <param name="printId"></param>
        /// <param name="tmpDir"></param>
        /// <returns></returns>
        public string GeteTemplatByTypeFile(string typefile, string def_Int2, string tmpDir, string fileName = "")
        {

            try
            {
                DbHelper.Open(PubConnectString);
                return new PrintRule().GeteTemplatByTypeFile(PubConnectString, typefile, def_Int2, tmpDir, fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbHelper.Close(PubConnectString);
            }
        }

        /// <summary>
        /// 通过打印ID获取套打模板
        /// </summary>
        /// <param name="printId"></param>
        /// <param name="tmpDir"></param>
        /// <returns></returns>
        public DataTable GetModuleById(string printId, string tmpDir)
        {
          
            try
            {
                DbHelper.Open(PubConnectString);
                return new PrintRule().GetModuleByID(PubConnectString, printId, tmpDir);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbHelper.Close(PubConnectString);
            }
        }
        #endregion

        #region 保存用户自定义模板
        /// <summary>
        /// 保存用户自定义模板
        /// </summary>
        /// <param name="printId"></param>
        /// <param name="typeFile"></param>
        /// <param name="mTitle"></param>
        /// <param name="xmlDoc"></param>
        /// <param name="tmpDir"></param>
        /// <returns></returns>
        public string SaveUserDefTemplate(ref string printId, string typeFile, string mTitle, string xmlDoc, string tmpDir)
        {
          
            try
            {
                DbHelper.Open(PubConnectString);
                DbHelper.BeginTran(PubConnectString);
                string fName = new PrintRule().SaveUserDefTemplate(PubConnectString, ref printId, typeFile, mTitle, xmlDoc, tmpDir);
                DbHelper.CommitTran(PubConnectString);
                return fName;
            }
            catch (Exception ex)
            {
                DbHelper.RollbackTran(PubConnectString);
                throw ex;
            }
            finally
            {
                DbHelper.Close(PubConnectString);
            }
        }
        #endregion

        #region 套打管理
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        public IList<Base.TreeJSONBase> LoadTplTree(string nodeid)
        {           
            try
            {
                DbHelper.Open(PubConnectString);
                return new PrintRule().LoadTplTree(PubConnectString,nodeid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbHelper.Close(PubConnectString);
            }            
        }       
        /// <summary>
        /// dbcnt 导入更新系统模板
        /// </summary>
        /// <param name="userConn"></param>
        /// <param name="pubConn"></param>
        /// <returns></returns>
        public int DbcntImport(string userConn,string pubConn,string baseDir)
        {
            NG3.Data.Service.ConnectionInfoService.SetSessionConnectString(userConn);
            string UserConnectString = userConn;
            string allMoudleType = "";
            #region 模块
            try
            {
                DbHelper.Open(UserConnectString);
                DataTable dt = new PrintRule().GetModules(UserConnectString);               
                string sFileUrl = baseDir + "/Product.xml";
                DataSet ds = new DataSet();
                ds.ReadXml(sFileUrl);
                DataTable dtSuit = ds.Tables["SuitInfo"];
                DataRow[] drs = dtSuit.Select("code='PUB'");
                StringBuilder keys = new StringBuilder(), values = new StringBuilder();
                if (drs != null && drs.Length > 0)
                {
                    if (keys.Length > 0)
                    {
                        keys.Append(",");
                        values.Append(",");
                    }
                    keys.Append(drs[0]["code"].ToString());
                    values.Append(drs[0]["name"].ToString());

                }
                DataRow[] rows = dt.Select("ismodule=1");
                foreach (DataRow dr in rows)
                {
                    if (keys.Length > 0)
                    {
                        keys.Append(",");
                        values.Append(",");
                    }
                    keys.Append(dr["id"].ToString());
                    values.Append(dr["name"].ToString());
                }
                allMoudleType = keys.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbHelper.Close(UserConnectString);
            }
            #endregion
            int retInt = -1;
            string errTxt = "";
            try
            {              
                string sysTemplatePath = baseDir + "\\ng3\\reports\\sysConfig";
                string sysPdfTemplatePath = baseDir + "\\ng3\\reports\\pdfConfig";
                NG3.Data.Service.ConnectionInfoService.SetSessionConnectString(pubConn);
                DbHelper.Open(pubConn);
                DbHelper.BeginTran(pubConn);
                retInt = new PrintRule().ImportModule(pubConn, sysTemplatePath, allMoudleType, "sys", ref errTxt);
                retInt += new PrintRule().ImportModule(pubConn, sysPdfTemplatePath, allMoudleType, "sys_pdf", ref errTxt);
                retInt += new PrintRule().UpdateModule(pubConn, sysTemplatePath, allMoudleType, "sys", ref errTxt);
                retInt += new PrintRule().UpdateModule(pubConn, sysPdfTemplatePath, allMoudleType, "sys_pdf_0", ref errTxt);
                new PrintRule().UpdateFlag(pubConn);
                DbHelper.CommitTran(pubConn);
            }
            catch (Exception ex)
            {
                retInt = -1;
                DbHelper.RollbackTran(pubConn);
                errTxt = ex.Message.Replace("\r\n", "");
            }
            finally
            {
                DbHelper.Close(pubConn);
            }
            return retInt;
        }
        /// <summary>
        /// 导入打印模板
        /// </summary>
        /// <param name="sysPath"></param>
        /// <param name="allMoudleType"></param>
        /// <param name="mType"></param>
        /// <param name="errTxt"></param>
        /// <returns></returns>
        public int ImportModule(string sysPath, string allMoudleType, string mType, ref string errTxt)
        {
            int retInt = -1;
         
            try
            {
                DbHelper.Open(PubConnectString);
                DbHelper.BeginTran(PubConnectString);
                retInt = new PrintRule().ImportModule(PubConnectString, sysPath, allMoudleType, mType, ref errTxt);
                new PrintRule().UpdateFlag(PubConnectString);
                DbHelper.CommitTran(PubConnectString);
            }
            catch (Exception ex)
            {
                retInt = -1;
                DbHelper.RollbackTran(PubConnectString);
                errTxt = ex.Message.Replace("\r\n", "");
            }
            finally
            {
                DbHelper.Close(PubConnectString);
            }
            return retInt;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysPath"></param>
        /// <param name="allMoudleType"></param>
        /// <param name="mType"></param>
        /// <param name="errTxt"></param>
        /// <returns></returns>
        public int UpdateModule(string sysPath, string allMoudleType, string mType, ref string errTxt)
        {
            int retInt = -1;

            try
            {
                DbHelper.Open(PubConnectString);
                DbHelper.BeginTran(PubConnectString);
                retInt = new PrintRule().UpdateModule(PubConnectString, sysPath, allMoudleType, mType, ref errTxt);
                new PrintRule().UpdateFlag(PubConnectString);
                DbHelper.CommitTran(PubConnectString);
            }
            catch (Exception ex)
            {
                retInt = -1;
                DbHelper.RollbackTran(PubConnectString);
                errTxt = ex.Message.Replace("\r\n", "");
            }
            finally
            {
                DbHelper.Close(PubConnectString);
            }
            return retInt;
        }
        /// <summary>
        /// 删除模板
        /// </summary>
        /// <param name="printId"></param>
        /// <param name="errTxt"></param>
        /// <returns></returns>
        public int DeleteModule(string printId, ref string errTxt)
        {
            int retInt = -1;
          
            try
            {
                DbHelper.Open(PubConnectString);
                DbHelper.BeginTran(PubConnectString);
                retInt = new PrintRule().DeleteModule(PubConnectString, printId);
                DbHelper.CommitTran(PubConnectString);
            }
            catch (Exception ex)
            {
                retInt = -1;
                DbHelper.RollbackTran(PubConnectString);
                errTxt = ex.Message.Replace("\r\n", "");
            }
            finally
            {
                DbHelper.Close(PubConnectString);
            }
            return retInt;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="otype"></param>
        /// <param name="printId"></param>
        /// <param name="moduleno"></param>
        /// <param name="tmpPath"></param>
        /// <param name="xmlDoc"></param>
        /// <param name="rowData"></param>
        /// <returns></returns>
        public string Update(string otype, string printId, string moduleno, string tmpPath, string xmlDoc, Dictionary<string, object> rowData, ref string errTxt)
        {
            string retStr = "";

            try
            {
                DbHelper.Open(PubConnectString);
                DbHelper.BeginTran(PubConnectString);
                retStr = new PrintRule().Update(PubConnectString, otype, printId, moduleno, tmpPath, xmlDoc,rowData);
                DbHelper.CommitTran(PubConnectString);
            }
            catch (Exception ex)
            {
                retStr = "";
                DbHelper.RollbackTran(PubConnectString);
                errTxt = ex.Message.Replace("\r\n", "");
            }
            finally
            {
                DbHelper.Close(PubConnectString);
            }
            return retStr;
        }
        public string UpdateStatus(string printId, string paramName,string paramValue)
        {
            string retStr = ""; 
            try
            {
                DbHelper.Open(PubConnectString);
                DbHelper.BeginTran(PubConnectString);
                retStr = new PrintRule().UpdateStatus(PubConnectString, printId, paramName, paramValue);
                DbHelper.CommitTran(PubConnectString);
            }
            catch (Exception ex)
            {
                retStr = "";
                DbHelper.RollbackTran(PubConnectString);
                retStr = ex.Message.Replace("\r\n", "");
            }
            finally
            {
                DbHelper.Close(PubConnectString);
            }
            return retStr;
        }
        
        #endregion

        #region 打印设置
        /// <summary>
        /// 保存打印设置信息
        /// </summary>
        /// <param name="ctype"></param>
        /// <param name="PrintPage"></param>
        public void SetPrintSetup(string ctype, string printPage)
        {
            string UserConnectString = NG3.AppInfoBase.UserConnectString;
            try
            {
                DbHelper.Open(UserConnectString);
                DbHelper.BeginTran(UserConnectString);
                new PrintRule().SetPrintSetup(UserConnectString, ctype, printPage);
                DbHelper.CommitTran(UserConnectString);
            }
            catch
            {
                DbHelper.RollbackTran(UserConnectString);
            }
            finally
            {
                DbHelper.Close(UserConnectString);
            }
        }

        /// <summary>
        /// 获取打印设置信息
        /// </summary>
        /// <param name="ctype"></param>
        /// <returns></returns>
        public string GetPrintSetup(string ctype)
        {
            string UserConnectString = NG3.AppInfoBase.UserConnectString;
            DbHelper.Open(UserConnectString);
            string v = new PrintRule().GetPrintSetup(UserConnectString, ctype);
            DbHelper.Close(UserConnectString);
            return v;
        }
        #endregion

        #region 打印测试
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tname"></param>
        /// <returns></returns>
        public DataTable GetTestMst(string tname)
        {           
            try
            {
                DbHelper.Open();
                string sql = "select * from ctl_tbl where 1=1";
                if (!string.IsNullOrEmpty(tname))
                {
                    sql += " and tname='" + tname + "'";
                }
                return DbHelper.GetDataTable(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbHelper.Close();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tname"></param>
        /// <param name="pageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public DataTable GetTestDtl(string tname, int pageSize, int pageIndex, ref int totalRecord)
        {           
            try
            {
                DbHelper.Open();
                string sortField = " order by seq";
                string sql = "select * from ctl_fld where 1=1";
                if (!string.IsNullOrEmpty(tname))
                {
                    sql += " and tname='" + tname + "'";
                }
                string sqlstr = SUP.Common.Base.PaginationAdapter.GetPageDataSql(sql, pageSize, ref pageIndex, ref totalRecord, sortField);
                return DbHelper.GetDataTable(sqlstr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbHelper.Close();
            }
        }
        #endregion
    }
}
