using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using NG3.Data.Service;
using SUP.Common.DataEntity;
using NG3.Data;
using SUP.Common.Base;
using System.IO;

namespace SUP.Common.DataAccess
{
    public class IndividualUIDac
    {

        public IndividualUIDac() {
 
        }

        public int Save(DataTable dt, List<string> dellist)
        {
            //foreach (DataRow dr in dt.Rows)
            //{
            //    if (dr.RowState == DataRowState.Deleted) continue;

            //    if (dr.RowState == DataRowState.Added)
            //    {
            //        dr["code"] = Guid.NewGuid().ToString();
            //    }
            //}

           string sql = "select * from fg_individualinfo";




            int del = 0;

            //保存的时候要根据phid将fg_individualinfo_allot中的数据删除
            if ( (dellist != null) && (dellist.Count != 0) )
            {
                StringBuilder strb = new StringBuilder();
                int count = dellist.Count;
                int index = count - 1;
                for (int i = 0; i < count; i++)
                {
                    string val = dellist[i];
                    if (i == index)
                    {
                        strb.Append("'" + val + "'");
                    }
                    else
                    {
                        strb.Append("'" + val + "',");
                    }
                }
                string delAllot = string.Format("delete from fg_individualinfo_allot where individualinfo_phid  in ( {0} )", strb.ToString());

                del = DbHelper.ExecuteNonQuery(delAllot);
            }


            int iret = DbHelper.Update(dt, sql);



            return iret+del;
        }

        /// <summary>
        /// 获取某一业务类型的自定义信息列表
        /// </summary>
        /// <param name="bustype">业务类型</param>
        /// <returns></returns>
        public DataTable GetIndividualInfoList(string bustype)
        {
            string sql = "select phid,name,bustype,defaultflg,isbackup,remark from fg_individualinfo where bustype={0}";

            IDataParameter[] p = new NGDataParameter[1];
            p[0] = new NGDataParameter("bustype", DbType.AnsiString);
            p[0].Value = bustype ?? string.Empty;

            return DbHelper.GetDataTable(sql,p);

        }

        /// <summary>
        /// 根据业务类型获取自定义界面信息
        /// </summary>
        /// <param name="bustype">业务类型</param>
        /// <returns></returns>
        public string GetIndividualUI(string bustype)
        {
            string sql = "select individualinfo from fg_individualinfo where bustype={0} and defaultflg='1'";

            IDataParameter[] p = new NGDataParameter[1];
            p[0] = new NGDataParameter("bustype", DbType.AnsiString);
            p[0].Value = bustype;

            object obj = DbHelper.ExecuteScalar(sql,p);

            string str = string.Empty;
            if(obj != null && obj != DBNull.Value)
            {
                str = obj.ToString();
            }

            return str;
        }

        /// <summary>
        /// 按组织获取方案
        /// </summary>
        /// <param name="bustype"></param>
        /// <returns></returns>
        public string GetIndividualUIByOrgID(string bustype)
        {
            string sql = @"select individualinfo from fg_individualinfo,fg_individualinfo_allot
                                where fg_individualinfo.phid = fg_individualinfo_allot.individualinfo_phid 
                                and bustype={0} and fg_individualinfo_allot.object_type=0 and
                                fg_individualinfo_allot.object_id = {1}";

            IDataParameter[] p = new NGDataParameter[2];
            p[0] = new NGDataParameter("bustype", DbType.AnsiString);
            p[0].Value = bustype;
            p[1] = new NGDataParameter("object_id", DbType.Int64);
            p[1].Value = NG3.AppInfoBase.OrgID;

            object obj = DbHelper.ExecuteScalar(sql, p);

            string str = string.Empty;
            if (obj != null && obj != DBNull.Value)
            {
                str = obj.ToString();
            }

            return str;
        }

        /// <summary>
        /// 获取某一个界面
        /// </summary>
        /// <param name="code">主键</param>
        /// <returns></returns>
        public string GetIndividualUIbyCode(Int64 phid)
        {
            string sql = "select individualinfo from fg_individualinfo where phid={0}";

            IDataParameter[] p = new NGDataParameter[1];
            p[0] = new NGDataParameter("code", DbType.UInt64);
            p[0].Value = phid;

            object obj = DbHelper.ExecuteScalar(sql, p);

            string str = string.Empty;
            if (obj != null && obj != DBNull.Value)
            {
                str = obj.ToString();
            }

            return str;
        }

        /// <summary>
        /// 删除自定义界面信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public int Delete(string code)
        {
            string sql = string.Format("delete from fg_individualinfo where code={0}", DbConvert.ToSqlString(code));

            return DbHelper.ExecuteNonQuery(sql);
        }

        public DataTable GetIndividualRegList(string clientJson, int pageSize, int PageIndex, ref int totalRecord)
        {
            string sql = "select * from fg_individualinfo_reg";

            string sortField = " bustype asc ";
            if (!string.IsNullOrEmpty(clientJson))
            {
                string query = string.Empty;
                IDataParameter[] p = DataConverterHelper.BuildQueryWithParam(clientJson, string.Empty, ref query);

                if (!string.IsNullOrEmpty(query))
                {
                    sql += " where " + query;
                }

                string sqlstr = PaginationAdapter.GetPageDataSql(sql, pageSize, ref PageIndex, ref totalRecord, sortField, p);
                return DbHelper.GetDataTable(sqlstr, p);
            }
            else
            {
                string sqlstr = PaginationAdapter.GetPageDataSql(sql, pageSize, ref PageIndex, ref totalRecord, sortField);
                return DbHelper.GetDataTable(sqlstr);
            }
        }
     

        //保存自定义界面
        public int SaveIndividualUI(Int64 phid,string uiinfo)
        {
            //string sql = string.Format("update fg_individualinfo set individualinfo={0} where phid={1}",DbConvert.ToSqlString(uiinfo),phid);
            //return DbHelper.ExecuteNonQuery(sql);

            string sql = "update fg_individualinfo set individualinfo={0} where phid={1}";
            IDataParameter[] p = new NGDataParameter[2];
            p[0] = new NGDataParameter("individualinfo", NGDbType.Text);
            p[0].Value = uiinfo;
            p[1] = new NGDataParameter("phid", DbType.Int64);
            p[1].Value = phid;
            return DbHelper.ExecuteNonQuery(sql,p);
        }
        
        
        //获取单据列表需要显示的自定义列
        public DataTable GetIndividualColumnForList(string bustype,List<string> tables)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" select fg_columns.c_name,fg_col_uireg.uixtype, fg_columns.c_fullname,");
            sb.Append(" fg_col_uireg.helpid,fg_col_uireg.combodata ");
            sb.Append(" from fg_col_uireg,fg_columns  ");
            sb.Append(" where fg_col_uireg.columnreg_code = fg_columns.c_code  ");
            sb.Append(" and bustype='{0}' and fg_col_uireg.listshow_flg='1' and fg_columns.c_bname in ({1})  ");

            StringBuilder insb = new StringBuilder();
            foreach (string str in tables)
            {
                insb.Append("'" + str + "',");                
            }

            string sql = string.Format(sb.ToString(), bustype, insb.ToString().TrimEnd(','));

            return DbHelper.GetDataTable(sql);
        }


        #region 脚本插件处理

        /// <summary>
        /// 获取注册的自定义信息js文件地址
        /// </summary>
        /// <param name="bustype">业务类型</param>
        /// <returns></returns>
        public string GetIndividualRegUrl(string bustype)
        {

            string sql = "select url from fg_individualinfo_reg where bustype={0}";

            IDataParameter[] p = new NGDataParameter[1];
            p[0] = new NGDataParameter("bustype", DbType.AnsiString);
            p[0].Value = bustype;

            object obj = DbHelper.ExecuteScalar(sql, p);

            string str = string.Empty;
            if (obj != null && obj != DBNull.Value)
            {
                str = obj.ToString();
            }

            return str;

        }

        /// <summary>
        /// 获取用户自定义js文件地址
        /// </summary>
        /// <param name="bustype">业务类型</param>
        /// <returns></returns>
        public string GetUserDefScriptUrl(string bustype)
        {
            if (IsDeveloper())
            {
                string sql = string.Format("select url from fg_individualinfo_reg where bustype='{0}'", bustype);
                string url = DbHelper.GetString(sql);//注册的插件的相对路径
                if (string.IsNullOrWhiteSpace(url)) return string.Empty;

                string phid = GetUIPhIdByOrgID(bustype);//先找分配到的方案
                if (string.IsNullOrWhiteSpace(phid))
                {
                    string s = string.Format("select phid from fg_individualinfo where bustype='{0}' and defaultflg='1'", bustype);//取默认方案的
                    phid = DbHelper.GetString(s);
                } 
                               
                string fileName = phid + "_" + NG3.AppInfoBase.LoginID + ".js";//设计时版本按用户名生成，不带时间戳，想看效果需清理浏览器缓存
                int index = url.LastIndexOf('/');
                return url.Substring(0, index) + @"\udef\" + fileName;//二次开发脚本（草稿）文件相对路径          
            }
            else
            {
                string scriptUrl = GetUserDefScriptUrlByOrgID(bustype);//先找分配到的方案
                if (!string.IsNullOrEmpty(scriptUrl))
                {
                    return scriptUrl;
                }

                string sql = "select userdef_scripturl from fg_individualinfo where bustype={0} and defaultflg='1'";
                IDataParameter[] p = new NGDataParameter[1];
                p[0] = new NGDataParameter("bustype", DbType.AnsiString);
                p[0].Value = bustype;
                return DbHelper.GetString(sql, p);
            }
        }

        /// <summary>
        /// 根据组织获取二开脚本的phid
        /// </summary>
        /// <param name="bustype"></param>
        /// <returns></returns>
        public string GetUIPhIdByOrgID(string bustype)
        {
            string sql = @"select fg_individualinfo.phid from fg_individualinfo,fg_individualinfo_allot
                                where fg_individualinfo.phid = fg_individualinfo_allot.individualinfo_phid 
                                and bustype={0} and fg_individualinfo_allot.object_type=0 and
                                fg_individualinfo_allot.object_id = {1}";

            IDataParameter[] p = new NGDataParameter[2];
            p[0] = new NGDataParameter("bustype", DbType.AnsiString);
            p[0].Value = bustype;
            p[1] = new NGDataParameter("object_id", DbType.Int64);
            p[1].Value = NG3.AppInfoBase.OrgID;

            object obj = DbHelper.ExecuteScalar(sql, p);

            string str = string.Empty;
            if (obj != null && obj != DBNull.Value)
            {
                str = obj.ToString();
            }

            return str;
        }

        /// <summary>
        /// 根据组织获取二开脚本的url
        /// </summary>
        /// <param name="bustype"></param>
        /// <returns></returns>
        public string GetUserDefScriptUrlByOrgID(string bustype)
        {
            string sql = @"select userdef_scripturl from fg_individualinfo,fg_individualinfo_allot
                                where fg_individualinfo.phid = fg_individualinfo_allot.individualinfo_phid 
                                and bustype={0} and fg_individualinfo_allot.object_type=0 and
                                fg_individualinfo_allot.object_id = {1}";

            IDataParameter[] p = new NGDataParameter[2];
            p[0] = new NGDataParameter("bustype", DbType.AnsiString);
            p[0].Value = bustype;
            p[1] = new NGDataParameter("object_id", DbType.Int64);
            p[1].Value = NG3.AppInfoBase.OrgID;

            object obj = DbHelper.ExecuteScalar(sql, p);

            string str = string.Empty;
            if (obj != null && obj != DBNull.Value)
            {
                str = obj.ToString();
            }

            return str;
        }

        /// <summary>
        /// 是否是开发人员
        /// </summary>
        /// <returns></returns>
        public bool IsDeveloper()
        {
            string sql = string.Format("select count(*) from addin_method_operator where loginid='{0}'", NG3.AppInfoBase.LoginID);

            if (DbHelper.GetString(sql) == "0") {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取界面方案名
        /// </summary>
        /// <param name="phid"></param>
        /// <returns></returns>
        public string GetSchemaName(Int64 phid)
        {
            return DbHelper.GetString(string.Format("select name from fg_individualinfo where phid={0}", phid));
        }

        //获取脚本代码
        public string GetScriptCode(Int64 phid)
        {
            string sql = string.Format("select scriptcode_draft from fg_individualinfo where phid={0}", phid);
            string base64Str = DbHelper.GetString(sql);
            return NG3.NGEncode.FromBase64(base64Str);
        }

        /// <summary>
        /// 保存，只保存草稿，和生成带用户名后缀的字段，不更新scripturl字段
        /// </summary>
        /// <param name="busType"></param>
        /// <param name="scriptCode"></param>
        /// <returns></returns>
        public int SaveScript(string busType, Int64 phid,string scriptCode)
        {
            string scriptFileName = GenerateScriptFile(busType,phid, scriptCode,false);
            string base64Str = NG3.NGEncode.ToBase64(scriptCode);

            //string sql = string.Format("update fg_individualinfo  set scriptcode_draft={0} where phid={1}", DbConvert.ToSqlString(base64Str), phid);
            //int iret = DbHelper.ExecuteNonQuery(sql);//保存草稿

            string sql = "update fg_individualinfo  set scriptcode_draft={0} where phid={1}";
            IDataParameter[] p = new NGDataParameter[2];
            p[0] = new NGDataParameter("scriptcode_draft", NGDbType.Text);
            p[0].Value = base64Str;
            p[1] = new NGDataParameter("phid", DbType.Int64);
            p[1].Value = phid;
            return DbHelper.ExecuteNonQuery(sql, p);            
        }

        /// <summary>
        /// 发布脚本代码,所有用户生效
        /// </summary>
        /// <param name="busType"></param>
        /// <param name="scriptCode"></param>
        /// <returns></returns>
        public int PublishScript(string busType, Int64 phid,string scriptCode)
        {
            string scriptFileName = GenerateScriptFile(busType,phid, scriptCode,true);
            string base64Str = NG3.NGEncode.ToBase64(scriptCode);

            //orcale会截断，要参数化保存clob
            //string sql = string.Format("update fg_individualinfo  set scriptcode_draft={0} where phid={1}", DbConvert.ToSqlString(base64Str), phid);
            //string sql2 = string.Format("update fg_individualinfo   set scriptcode_pub={0} where phid={1}", DbConvert.ToSqlString(base64Str), phid);
            //int iret = DbHelper.ExecuteNonQuery(sql);//草稿
            //iret += DbHelper.ExecuteNonQuery(sql2);//发布版本

            string sql = "update fg_individualinfo  set scriptcode_draft={0} where phid={1}";
            IDataParameter[] p = new NGDataParameter[2];
            p[0] = new NGDataParameter("scriptcode_draft", NGDbType.Text);
            p[0].Value = base64Str;
            p[1] = new NGDataParameter("phid", DbType.Int64);
            p[1].Value = phid;
            int iret = DbHelper.ExecuteNonQuery(sql, p);

            string sql2 = "update fg_individualinfo   set scriptcode_pub={0} where phid={1}";
            IDataParameter[] parm = new NGDataParameter[2];
            parm[0] = new NGDataParameter("scriptcode_pub", NGDbType.Text);
            parm[0].Value = base64Str;
            parm[1] = new NGDataParameter("phid", DbType.Int64);
            parm[1].Value = phid;
            iret += DbHelper.ExecuteNonQuery(sql2, parm);

            string sqlStr = string.Format("update fg_individualinfo  set userdef_scripturl={0} where phid={1}", DbConvert.ToSqlString(scriptFileName), phid);
            iret += DbHelper.ExecuteNonQuery(sqlStr);

            return iret;
        }
        
        /// <summary>
        /// 生成脚本文件
        /// </summary>
        /// <param name="busType"></param>
        /// <param name="scriptCode"></param>
        /// <param name="isPublish">是否发布</param>
        private string GenerateScriptFile(string busType, Int64 phid, string scriptCode, bool isPublish)
        {
            string sql = string.Format("select url from fg_individualinfo_reg where bustype='{0}'", busType);
            string url = DbHelper.GetString(sql);//注册的插件的相对路径

            int index = url.LastIndexOf('/');
            if (index < 0)
            {
                index = url.LastIndexOf('\\');//有些注册成反斜杠
            }

            string path = AppDomain.CurrentDomain.BaseDirectory + @"NG3Resource\IndividualInfo\" + url.Substring(0, index) + @"\udef\";
            string fileName = phid.ToString() + "_"  + NG3.AppInfoBase.LoginID + ".js";//设计时版本按用户名生成，不带时间戳，想看效果需清理浏览器缓存
            if (isPublish)//发布版本按时间戳生成，无需清浏览器缓存
            {
                fileName = phid.ToString() + "_UDef" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".js";
            }
            string fullPath = path + fileName;

            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
            byte[] b = Encoding.UTF8.GetBytes(scriptCode);
            using (FileStream fs = File.Create(fullPath))
            {
                fs.Write(b, 0, b.Length);               
            }
            return url.Substring(0, index) + @"\udef\" + fileName;//脚本文件相对路径
        }


        #endregion

        #region 脚本同步
        public int SyncScript()
        {
            int successCount = 0;

            string sqlIndividualinfo = "select * from fg_individualinfo";
            DataTable IndividualInfo = DbHelper.GetDataTable(sqlIndividualinfo);

            foreach (DataRow dr in IndividualInfo.Rows)
            {
                if (!String.IsNullOrEmpty(dr["userdef_scripturl"].ToString()))
                {
                    //PMS / PCM / HT / CntM\udef\522180505000007_UDef20180510164032.js                    
                    string OrgUrl = dr["userdef_scripturl"].ToString();
                    string OrgJSFilename = OrgUrl.Substring(OrgUrl.LastIndexOf(@"\") + 1);   //522180505000007_UDef20180510164032.js
                    //先去路径下查找有没有这个文件夹
                    int index = OrgUrl.IndexOf(@"\");
                    string path = AppDomain.CurrentDomain.BaseDirectory + @"NG3Resource\IndividualInfo\" + OrgUrl.Substring(0, index) + @"\udef\";

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string filename = path + "\\" + OrgJSFilename;

                    //存在这个文件夹就去寻找文件夹下有没有这个文件
                    if (!File.Exists(filename))
                    {
                        string base64Str = dr["scriptcode_pub"].ToString();
                        string base64PubCode = NG3.NGEncode.FromBase64(base64Str);


                        string fullPath = path + OrgJSFilename;
                        byte[] b = Encoding.UTF8.GetBytes(base64PubCode);
                        using (FileStream fs = File.Create(fullPath))
                        {
                            fs.Write(b, 0, b.Length);
                            successCount++;
                        }
                    }
                                       
                }
            }
            return successCount;
        }
        #endregion




        #region 升级


        /// <summary>
        /// 待检测列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetToCheckList(string bustype)
        {
            string sql = @"select fg_individualinfo.phid,fg_individualinfo.name,fg_individualinfo_reg.bustype,
                                 fg_individualinfo_reg.busname,fg_individualinfo_reg.url
                                 FROM fg_individualinfo,fg_individualinfo_reg
                                 where fg_individualinfo.bustype = fg_individualinfo_reg.bustype
                                 and fg_individualinfo.individualinfo IS NOT null and (isbackup IS NULL or isbackup='0') ";

            if (!string.IsNullOrWhiteSpace(bustype))
            {
                sql += string.Format(" and fg_individualinfo.bustype='{0}'", bustype);
            }

            return DbHelper.GetDataTable(sql);

        }

        /// <summary>
        /// 获取自定义信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetCheckInfo(string bustype, string ids)
        {
            string sql = string.Format(@"select fg_individualinfo.phid,fg_individualinfo.name,fg_individualinfo_reg.bustype,
                                 fg_individualinfo_reg.busname,fg_individualinfo_reg.url,fg_individualinfo.individualinfo 
                                  FROM fg_individualinfo,fg_individualinfo_reg
                                 where fg_individualinfo.bustype = fg_individualinfo_reg.bustype
                                 and fg_individualinfo.individualinfo IS NOT null and (isbackup IS NULL or isbackup='0') and fg_individualinfo.phid in({0})", ids);

            if (!string.IsNullOrWhiteSpace(bustype))
            {
                sql += string.Format(" and fg_individualinfo.bustype='{0}'",bustype);
            }

            return DbHelper.GetDataTable(sql);

        }

        /// <summary>
        /// 获取所有自定义模板信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllUserTemplateInfo()
        {
            string sql = @"select fg_individualinfo.phid,fg_individualinfo.name,fg_individualinfo_reg.bustype,
                                 fg_individualinfo_reg.busname,fg_individualinfo.individualinfo 
                                  FROM fg_individualinfo,fg_individualinfo_reg
                                 where fg_individualinfo.bustype = fg_individualinfo_reg.bustype
                                 and fg_individualinfo.individualinfo IS NOT null and (isbackup IS NULL or isbackup='0')";

            return DbHelper.GetDataTable(sql);

        }

        public DataTable GetSysTemplateInfo()
        {
            string sql = @"select * FROM fg_individualinfo_reg,metadata_bustree
                                 where fg_individualinfo_reg.bustype = metadata_bustree.code ORDER BY bustype";

            return DbHelper.GetDataTable(sql);
        }


        /// <summary>
        /// 获取待升级的自定义信息
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public DataTable GetUpdateInfo(string idList)
        {
            string sql = string.Format(@"select fg_individualinfo.phid,fg_individualinfo.name,fg_individualinfo_reg.bustype,
                                 fg_individualinfo_reg.busname,fg_individualinfo_reg.url,fg_individualinfo.individualinfo 
                                  FROM fg_individualinfo,fg_individualinfo_reg
                                 where fg_individualinfo.bustype = fg_individualinfo_reg.bustype and fg_individualinfo.phid in({0})", idList);

            return DbHelper.GetDataTable(sql);

        }


        public DataTable GetUpdate(List<Int64> idList)
        {
            string ids = string.Join(",", idList.ToArray());

            //string sql = string.Format(@"select phid,name,bustype,defaultflg,scriptcode_pub,scriptcode_draft,userdef_scripturl,individualinfo,isbackup,remark 
            //                 from fg_individualinfo where phid in({0})", ids);

            string sql = string.Format(@"select * from fg_individualinfo where phid in({0})", ids);
            return DbHelper.GetDataTable(sql);
        }

        public DataTable GetSchema()
        {
            //string sql = @"select phid,name,bustype,defaultflg,scriptcode_pub,scriptcode_draft,userdef_scripturl,individualinfo,isbackup,remark 
            //                    from fg_individualinfo where 1=2";
            string sql = @"select * from fg_individualinfo where 1=2";
            return DbHelper.GetDataTable(sql);
        }

        public int Update(DataTable backDt, DataTable updateDt)
        {
            int iret = 0;
            iret += DbHelper.Update(backDt, "select * from fg_individualinfo");
            iret += DbHelper.Update(updateDt, "select * from fg_individualinfo");
            return iret;
        }

        public DataTable GetInfoById(Int64 phid)
        {
            DataTable dt =DbHelper.GetDataTable(string.Format("select * from fg_individualinfo where phid={0}", phid));
            return dt;
        }

        //保存副本
        public int SaveCopy(DataTable copyDt)
        {
            int iret = 0;
            iret += DbHelper.Update(copyDt, "select * from fg_individualinfo");
            return iret;
        }


        //界面组织树相关

        //根据phid获取组织树
        public DataTable GetOrgNumberByPhid(string bustypephid)
        {
            string sql = "select * from fg_individualinfo_allot where busphid='{0}'";
            sql = string.Format(sql, bustypephid);
            return DbHelper.GetDataTable(sql);
        }
        //保存组织树
        public int SaveOrg(DataTable dtAddOrg, List<string> delOrg,string phid)
        {
            //新增
            string sqlAdd = "select * from fg_individualinfo_allot";
            int iret = DbHelper.Update(dtAddOrg, sqlAdd);

            //删除
            if (delOrg != null && delOrg.Count != 0)
            {
                StringBuilder del = new StringBuilder();
                foreach (string str in delOrg)
                {
                    del.Append("'" + str + "',");
                }
                string sqlDel = string.Format(@"delete from fg_individualinfo_allot where individualinfo_phid='{0}' and object_id in({1})", phid, del.ToString().TrimEnd(','));
                DbHelper.ExecuteNonQuery(sqlDel);
            }            


            return iret;
        }



        #endregion
    }
}
