using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NG3.Data.Service;
using NG3.Data;
using NG3;

namespace SUP.Frame.DataAccess
{
    public class IndividualSettingDac
    {        
        public DataTable LoadSysSetting(long userid)
        {
            StringBuilder strbuilder = new StringBuilder();
            strbuilder.Append("select individualsetting,applogoname");
            strbuilder.Append(" from fg3_mainframe_individual");
            strbuilder.Append(" where userid = " + userid + " and usertype = 0 ");
            DataTable treetabconfig = DbHelper.GetDataTable(strbuilder.ToString());
            if (treetabconfig == null || treetabconfig.Rows.Count == 0)
            {
                strbuilder.Remove(0, strbuilder.Length);
                userid = UserConfigDac.ActorGet(userid);
                strbuilder.Append("select individualsetting");
                strbuilder.Append(" from fg3_mainframe_individual");
                strbuilder.Append(" where userid = " + userid + " and usertype = 1 ");
                treetabconfig = DbHelper.GetDataTable(strbuilder.ToString());
            }
            return treetabconfig;
        }
        public bool SaveSysSetting(long userid, string individualsetting, long phid)
        {
            string sql = "select count(*) from fg3_mainframe_individual where userid =" + userid + " and usertype = 0 ";
            string obj = DbHelper.GetString(sql).ToString();
            if (obj == "0")
            {
                //Int64 masterid = this.GetMaxID("fg3_mainframe_individual") + 1;
                sql = "insert into fg3_mainframe_individual (phid,userid,individualsetting,usertype) values ( " + phid + "," + userid + ",'" + individualsetting + "',0)";
            }
            else
            {
                sql = "update fg3_mainframe_individual set individualsetting = '" + individualsetting + "' where userid =" + userid + " and usertype = 0 ";
            }
            try
            {                
                DbHelper.ExecuteScalar(sql);
                string dateflag = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                UserConfigDac.UserConfigSave(userid, 0, dateflag);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }
        public DataTable LoadDefaultOpenTab(long userid)
        {
            StringBuilder strbuilder = new StringBuilder();
            DataTable menudt = new DataTable();

            strbuilder.Append("select * from ");
            strbuilder.Append("(select fg3_defaultopen_tab.phid,fg3_defaultopen_tab.userid,fg3_defaultopen_tab.id,fg3_defaultopen_tab.pid,fg3_defaultopen_tab.originalcode,fg3_defaultopen_tab.seq,fg3_defaultopen_tab.name,fg3_menu.url,fg3_menu.managername,fg3_menu.rightname,fg3_menu.suite,fg3_menu.moduleno,fg3_defaultopen_tab.urlparm");
            strbuilder.Append(" from fg3_defaultopen_tab,fg3_menu");
            strbuilder.Append(" where fg3_menu.code = fg3_defaultopen_tab.originalcode and (originalcode <> '' or originalcode is not null) and fg3_defaultopen_tab.usertype = 0 and userid = " + userid);
            strbuilder.Append(" union ");
            strbuilder.Append(" select  phid,userid,id,pid,originalcode,seq,name,url,null,null,null,null,urlparm");
            strbuilder.Append(" from fg3_defaultopen_tab");
            strbuilder.Append(" where (originalcode = '' or originalcode is null) and userid = " + userid + " and usertype = 0 ) t1");
            strbuilder.Append(" order by seq");
            menudt = DbHelper.GetDataTable(strbuilder.ToString());
            if (menudt.Rows.Count < 1)
            {
                strbuilder.Remove(0, strbuilder.Length);
                userid = UserConfigDac.ActorGet(userid);
                strbuilder.Append("select * from ");
                strbuilder.Append("(select fg3_defaultopen_tab.phid,fg3_defaultopen_tab.userid,fg3_defaultopen_tab.id,fg3_defaultopen_tab.pid,fg3_defaultopen_tab.originalcode,fg3_defaultopen_tab.seq,fg3_defaultopen_tab.name,fg3_menu.url,fg3_menu.managername,fg3_menu.rightname,fg3_menu.suite,fg3_menu.moduleno,fg3_defaultopen_tab.urlparm");
                strbuilder.Append(" from fg3_defaultopen_tab,fg3_menu");
                strbuilder.Append(" where fg3_menu.code = fg3_defaultopen_tab.originalcode and (originalcode <> '' or originalcode is not null) and fg3_defaultopen_tab.usertype = 1 and userid = " + userid);
                strbuilder.Append(" union ");
                strbuilder.Append(" select  phid,userid,id,pid,originalcode,seq,name,url,null,null,null,null,urlparm");
                strbuilder.Append(" from fg3_defaultopen_tab");
                strbuilder.Append(" where (originalcode = '' or originalcode is null) and userid = " + userid +" and usertype = 1 ) t1");
                strbuilder.Append(" order by seq");
                menudt = DbHelper.GetDataTable(strbuilder.ToString());
            }
            return menudt;
        }
        public int SaveDefaultOpenTab( long myUserid, DataTable masterdt, List<long> phid)
        {


            string sql = string.Format("delete from fg3_defaultopen_tab where userid={0} and usertype = 0", myUserid);
           
            int iret = DbHelper.ExecuteNonQuery(sql);

            if (masterdt.Rows.Count == 0)//删除用户我的功能树所有节点,返回1表示保存成功
            {
                return 1;
            }
            //Int64 masterid = this.GetMaxID("fg3_defaultopen_tab");
            int i = 0;
            //处理主表的主键
            foreach (DataRow dr in masterdt.Rows)
            {

                if (dr.RowState == DataRowState.Deleted) continue;

                if (dr.RowState == DataRowState.Added)
                {
                    //Guid.NewGuid().ToString();//主表的主键
                    //dr["phid"] = ++masterid;
                    dr["phid"] = phid[i++];
                    dr["userid"] = myUserid;
                    dr["usertype"] = 0;
                }

            }

            int m = DbHelper.Update(masterdt, "select * from fg3_defaultopen_tab");
            string dateflag = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            UserConfigDac.UserConfigSave(myUserid, 0, dateflag);
            return m;
        }
        
        public byte[] LoadServerSetting()
        {           

            string filekey = "NG_SERVERISP_CONFIG";
            string sql = "select file_value from fg_systemconfigfile where file_key='" + filekey + "'";
            DataTable dt = DbHelper.GetDataTable(NG3.AppInfoBase.PubConnectString,sql);
            byte[] fileValue = null;
            if (dt!=null&&dt.Rows.Count>0)
            {
                fileValue = dt.Rows[0]["file_value"] as byte[];
            }           
            return fileValue;
        }
        public DataTable LoadNetWorkIPMappingInfo()
        {
            string connstr = NG3.AppInfoBase.PubConnectString;
            DataTable data = new DataTable();
            byte[] obj = null;
            try
            {
                DbHelper.Open(connstr);
                string sql = "select file_value from fg_systemconfigfile where file_key='NG_NetWorkIPMapping_Data'";
                obj = (byte[])DbHelper.ExecuteScalar(connstr, sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbHelper.Close(connstr);
            }
            if (obj == null)
            {
                data = new DataTable("networkipmap");
                data.Columns.Add("ipaddress1", typeof(string));
                data.Columns.Add("ipaddress2", typeof(string));
                data.Columns.Add("ipaddress3", typeof(string));
                data.Columns.Add("ipaddress4", typeof(string));
                data.Columns.Add("ipaddress5", typeof(string));
                data.Columns.Add("ipaddress6", typeof(string));
                data.Columns.Add("connectType", typeof(int));
                DataRow dr = data.NewRow();
                dr["ipaddress1"] = dr["ipaddress2"] = dr["ipaddress3"] = dr["ipaddress4"] = dr["ipaddress5"] = dr["ipaddress6"] = "";
                dr["connectType"] = 0;
                data.Rows.Add(dr);
            }
            else
            {
                data = NG3.Runtime.Serialization.SerializerBase.DeSerialize(obj) as DataTable;
                if (data.Columns.IndexOf("connectType") == -1)
                {
                    data.Columns.Add("connectType", typeof(int));
                    if (data.Rows.Count > 0)
                    {
                        data.Rows[0]["connectType"] = 0;
                    }
                }
            }
            return data;
        }
        public bool SaveServerIpAndNetWorkIpConfig(DataTable dtServerISP, DataTable dtNetWorkIP)
        {
            string filekey = "NG_SERVERISP_CONFIG";
            string CACHEID = "NG_NetWorkIPMapping_Data";
            string connstr = NG3.AppInfoBase.PubConnectString;

            byte[] content1 = NG3.Runtime.Serialization.SerializerBase.Serialize(dtServerISP);
            byte[] content2 = NG3.Runtime.Serialization.SerializerBase.Serialize(dtNetWorkIP);

            try
            {
                DbHelper.Open(connstr);
                DbHelper.BeginTran(connstr);

                #region SaveServerISP
                string sql = "select * from fg_systemconfigfile where file_key='" + filekey + "'";
                DataTable dt = DbHelper.GetDataTable(connstr, sql);

                if (dt.Rows.Count > 0)//存在
                {
                    DataRow dr = dt.Rows[0];
                    dr["file_value"] = content1;
                    dr["remark"] = DateTime.Now.ToString();
                    DbHelper.Update(connstr, dt, "select * from fg_systemconfigfile");

                }
                else//新增
                {
                    string tempsql = "select * from fg_systemconfigfile where 1=0";
                    DataTable tempdt = DbHelper.GetDataTable(connstr, tempsql);

                    DataRow dr = dt.NewRow();
                    dr["code"] = Guid.NewGuid().ToString();
                    dr["file_key"] = filekey;
                    dr["file_value"] = content1;
                    dr["remark"] = DateTime.Now.ToString();
                    dt.Rows.Add(dr);

                    DbHelper.Update(connstr, dt, "select * from fg_systemconfigfile");
                }
                #endregion

                #region SaveNetWorkIP

                string sqlex = "select * from fg_systemconfigfile where file_key='" + CACHEID + "'";
                DataTable dtex = DbHelper.GetDataTable(connstr, sqlex);

                if (dtex.Rows.Count > 0)//存在
                {
                    DataRow dr = dtex.Rows[0];
                    dr["file_value"] = content2;
                    dr["remark"] = DateTime.Now.ToString();
                    DbHelper.Update(connstr, dtex, "select * from fg_systemconfigfile");

                }
                else//新增
                {
                    string tempsql = "select * from fg_systemconfigfile where 1=0";
                    DataTable tempdt = DbHelper.GetDataTable(connstr, tempsql);

                    DataRow dr = dtex.NewRow();
                    dr["code"] = Guid.NewGuid().ToString();
                    dr["file_key"] = CACHEID;
                    dr["file_value"] = content2;
                    dr["remark"] = DateTime.Now.ToString();
                    dtex.Rows.Add(dr);

                    DbHelper.Update(connstr, dtex, "select * from fg_systemconfigfile");
                }
                #endregion

                DbHelper.CommitTran(connstr);

            }
            catch (Exception ex)
            {
                DbHelper.RollbackTran(connstr);
                throw ex;
            }
            finally
            {
                DbHelper.Close(connstr);
            }
            return true;
        }

        public string[] LoadDisplaySetting(string ucode)
        {
            string sql = "select def_str2 from ngusers where ucode ='" + ucode + "'";
            string title = DbHelper.GetString(NG3.AppInfoBase.PubConnectString, sql);
            sql = "select file_value from fg_systemconfigfile where file_key = 'CODESHOWID'";
            //byte[] data = System.Text.Encoding.Default.GetBytes(DbHelper.GetString(NG3.AppInfoBase.PubConnectString, sql)) ;
            //string IfShowUcodeForIllegalUser = NG3.Runtime.Serialization.SerializerBase.DeSerialize(data) as DataTable;
            //DataTable dt = DbHelper.GetDataTable(NG3.AppInfoBase.PubConnectString, sql);
            string IfShowUcodeForIllegalUser;
            object fileValueObj = DbHelper.ExecuteScalar(NG3.AppInfoBase.PubConnectString, sql);
            if (fileValueObj != null && fileValueObj != DBNull.Value)
            {
                byte[] fileValue = (byte[])fileValueObj;
                //byte[] fileValue = dt.Rows[0]["file_value"] as byte[];
                //IfShowUcodeForIllegalUser = System.Text.Encoding.Default.GetString(fileValue);
                IfShowUcodeForIllegalUser = NG3.Runtime.Serialization.SerializerBase.DeSerialize(fileValue) as String;
            }
            else
            {
                IfShowUcodeForIllegalUser = "";
            }
            
            string[] s = { title, IfShowUcodeForIllegalUser };
            return s;
        }
        public bool SaveDisplaySettingUcode(string ucode,string s)
        {
            string sql = "select count(*) from ngusers where ucode ='" + ucode+"'";
            string obj = DbHelper.GetString(NG3.AppInfoBase.PubConnectString, sql).ToString();
            if (obj != "0")
            {
                sql = "update ngusers set def_str2 = '" + s + "' where ucode ='" + ucode + "'";
            }
            else
            {
                sql = "insert into ngusers (ucode,def_str2) values ( " + ucode + "," + s +"')";
            }
            try
            {
                DbHelper.ExecuteScalar(NG3.AppInfoBase.PubConnectString, sql);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        public bool SaveDisplaySettingCodeshowid(string content)
        {
            
            try
            {
                string sql = "select count(*) from fg_systemconfigfile where file_key = 'CODESHOWID'";
                byte[] s = NG3.Runtime.Serialization.SerializerBase.Serialize(content);
                string obj = DbHelper.GetString(NG3.AppInfoBase.PubConnectString, sql).ToString();
                if (obj != "0")
                {
                    sql = "update fg_systemconfigfile set file_value = {0},remark ={1} where file_key ='CODESHOWID'";
                    //sql = "update fg_systemconfigfile set remark = '" + DateTime.Now.ToString() + "' where file_key ='CODESHOWID'";
                    //sqlText = "update fg3_mydesktop set data = {0} where userid = '" + NG3.AppInfoBase.UserID + "' and usertype = '0'";
                    NGDataParameter[] dataparams = new NGDataParameter[2];
                    dataparams[0] = new NGDataParameter("file_value", DbType.Binary);
                    dataparams[0].Value = s;
                    dataparams[1] = new NGDataParameter("remark", DbType.String);
                    dataparams[1].Value = DateTime.Now.ToString();
                    DbHelper.ExecuteNonQuery(NG3.AppInfoBase.PubConnectString, sql, dataparams);
                }
                else
                {
                    //sql = "insert into fg_systemconfigfile (code,file_key,file_value,remark) values ( '" + Guid.NewGuid().ToString() + "','CODESHOWID','" + s + "','"+ DateTime.Now.ToString() + "')";
                    sql = "insert into fg_systemconfigfile (code,file_key,file_value,remark) values({0},{1},{2},{3})";
                    //sqlText = "insert into fg3_mydesktop(phid,data,userid,usertype) values({0},{1},{2},{3})";

                    NGDataParameter[] dataparams = new NGDataParameter[4];
                    dataparams[0] = new NGDataParameter("code", DbType.String);
                    dataparams[0].Value = Guid.NewGuid().ToString();
                    dataparams[1] = new NGDataParameter("file_key", DbType.String);
                    dataparams[1].Value = "CODESHOWID";
                    dataparams[2] = new NGDataParameter("file_value", DbType.Binary);
                    dataparams[2].Value = s;
                    dataparams[3] = new NGDataParameter("remark", DbType.String);
                    dataparams[3].Value = DateTime.Now.ToString();
                    DbHelper.ExecuteNonQuery(NG3.AppInfoBase.PubConnectString, sql, dataparams);

                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        public Int64 GetMaxID(string tableName)
        {
            string sql = "select max(phid) from " + tableName;
            object obj = DbHelper.ExecuteScalar(sql);

            Int64 iret = 0;
            if (obj != null && obj != DBNull.Value)
            {
                Int64.TryParse(obj.ToString(), out iret);
            }
            return iret;
        }

        public bool UserConfigCopy(long fromUserId, int fromUserType, long toUserId, int toUserType, List<long> phid)
        {
            try
            {
                string sqlText = "select * from fg3_defaultopen_tab where userid = '" + fromUserId + "' and usertype = '" + fromUserType + "'";
                DataTable dt = DbHelper.GetDataTable(sqlText);

                sqlText = "select count(*) from fg3_defaultopen_tab where userid = '" + toUserId + "' and usertype = '" + toUserType + "'";
                string obj = DbHelper.GetString(sqlText);
                if (obj != "0")
                {
                    sqlText = "delete from fg3_defaultopen_tab where userid = '" + toUserId + "' and usertype = '" + toUserType + "'";
                    DbHelper.ExecuteNonQuery(sqlText);
                }
                DataTable newTable = dt.Clone();
                //Int64 masterid = this.GetMaxID("fg3_defaultopen_tab");
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    DataRow newRow = newTable.NewRow();
                    newRow.BeginEdit();
                    newRow["phid"] = phid[i++];
                    newRow["userid"] = toUserId;
                    newRow["usertype"] = toUserType;
                    newRow["id"] = dr["id"];
                    newRow["pid"] = dr["pid"];
                    newRow["originalcode"] = dr["originalcode"];
                    newRow["seq"] = dr["seq"];
                    newRow["name"] = dr["name"];
                    newRow["url"] = dr["url"];
                    newRow["urlparm"] = dr["urlparm"];
                    newRow.EndEdit();
                    newTable.Rows.Add(newRow);
                }
                DbHelper.Update(newTable, "select * from fg3_defaultopen_tab");
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool UserConfigDel(long userid, int usertype)
        {
            try
            {
                string sqlText = "delete from fg3_defaultopen_tab where userid = '" + userid + "' and usertype = '" + usertype + "'";
                DbHelper.ExecuteNonQuery(sqlText);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //小铃铛设置存取
        public string LoadAlertItem()
        {
            long userid = AppInfoBase.UserID;
            StringBuilder strbuilder = new StringBuilder();

            strbuilder.Append("select alertconfig");
            strbuilder.Append(" from fg3_mainframe_individual");
            strbuilder.Append(" where userid = " + userid + " and usertype = 0 ");
            string alertconfig = DbHelper.GetString(strbuilder.ToString());
            if (alertconfig == "" || alertconfig == null)
            {
                strbuilder.Remove(0, strbuilder.Length);
                userid = UserConfigDac.ActorGet(userid);
                strbuilder.Append("select alertconfig");
                strbuilder.Append(" from fg3_mainframe_individual");
                strbuilder.Append(" where userid = " + userid + " and usertype = 1 ");
                alertconfig = DbHelper.GetString(strbuilder.ToString());
            }
            return alertconfig;

        }

        public bool SaveAlertItem(string alertconfig, long phid)
        {
            long userid = AppInfoBase.UserID;
            string s = NG3.Data.Service.ConnectionInfoService.GetSessionConnectString();
            string sql = "select count(*) from fg3_mainframe_individual where userid =" + userid + " and usertype = 0 ";
            string obj = DbHelper.GetString(sql).ToString();
            if (obj == "0")
            {
                //Int64 masterid = this.GetMaxID("fg3_mainframe_individual") + 1;
                sql = "insert into fg3_mainframe_individual (phid,userid,alertconfig,usertype) values ( " + phid + "," + userid + ",'" + alertconfig + "',0)";
            }
            else
            {
                sql = "update fg3_mainframe_individual set alertconfig = '" + alertconfig + "' where userid =" + userid + " and usertype = 0 ";
            }
            try
            {
                DbHelper.ExecuteScalar(sql);
                string dateflag = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                UserConfigDac.UserConfigSave(userid, 0, dateflag);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        ///// <summary>
        ///// 取得单点登录组织号
        ///// </summary>
        ///// <param name="logid"></param>
        ///// <returns></returns>
        //public string GetSSOOrg()
        //{
        //    string logid = AppInfoBase.LoginID;
        //    string sql = "select singlesignonorg from fg3_user where  userno='" + logid + "'";

        //    object obj = DbHelper.ExecuteScalar(sql);
        //    if (obj != null && obj != DBNull.Value)
        //    {
        //        return obj.ToString();
        //    }

        //    return string.Empty;
        //}

        ///// <summary>
        ///// 更新单点登录组织字段
        ///// </summary>
        ///// <param name="logid"></param>
        ///// <param name="orgcode"></param>
        ///// <returns></returns>
        //public int UpdateSSOOrg(string orgcode)
        //{
        //    string logid = AppInfoBase.LoginID;
        //    string sql = "update fg3_user set singlesignonorg='" + orgcode + "' where userno='" + logid + "'";
        //    return DbHelper.ExecuteNonQuery(sql);
        //}

        ///// <summary>
        ///// 代码转名称
        ///// </summary>
        ///// <param name="tname">表名</param>
        ///// <param name="namefield">名称字段</param>
        ///// <param name="valuefields">代码字段</param>
        ///// <param name="value">代码值</param>
        ///// <param name="filter">过滤条件</param>
        ///// <param name="constr">连接串</param>
        ///// <returns>名称</returns>
        //public string GetName(string tname, string namefield, string valuefields, string value, string filter, string constr)
        //{
        //    StringBuilder strSql = new StringBuilder();

        //    strSql.Append("select ");
        //    strSql.Append(namefield);
        //    strSql.Append(" from ");
        //    strSql.Append(tname);
        //    strSql.Append(" where ");
        //    strSql.Append(valuefields);
        //    strSql.Append("=");
        //    strSql.Append(DbConvert.ToSqlString(value));
        //    if (filter.Length > 0)
        //    {
        //        strSql.Append(" and ");
        //        strSql.Append(filter);
        //    }
        //    object obj;
        //    if (constr == "" || constr == null)
        //    {
        //        obj = DbHelper.ExecuteScalar(strSql.ToString());
        //    }
        //    else
        //    {
        //        obj = DbHelper.ExecuteScalar(constr, strSql.ToString());
        //    }
        //    if (obj != null && obj != DBNull.Value)
        //    {
        //        return obj.ToString();
        //    }
        //    return string.Empty;
        //}

        public string LoadPictureSet()
        {
            StringBuilder strbuilder = new StringBuilder();
            strbuilder.Append("select allowuser from fg3_loginpictureset where phid = 0");
            string allowuser = DbHelper.GetString(strbuilder.ToString());
            return allowuser;

        }

        public string GetAPPLogoAttachId(long userID)
        {
            return DbHelper.GetString("select applogoattachid from fg3_mainframe_individual where userid =" + userID);
        }

        public bool SaveAPPLogo(long phid, string APPlogo, string attachid, int isSys)
        {
            long userid = AppInfoBase.UserID;
            string dateflag = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string sql = "select count(*) from fg3_mainframe_individual where userid =" + userid + " and usertype = 0 ";
            string obj = DbHelper.GetString(sql).ToString();
            if (obj == "0")
            {
                sql = "insert into fg3_mainframe_individual (phid,userid,usertype,applogoname,applogoattachid,applogodateflg,issys) values ( "
                    + phid + "," + userid + ",0,'" + APPlogo + "','" + attachid + "','" + dateflag  + "',"+ isSys + ")";
            }
            else
            {
                sql = "update fg3_mainframe_individual set applogoname = '" + APPlogo + "', applogoattachid = '" + attachid +
                    "', applogodateflg = '" + dateflag + "', isSys = " + isSys + " where userid =" + userid + " and usertype = 0 ";
            }
            try
            {
                DbHelper.ExecuteScalar(sql);             
                UserConfigDac.UserConfigSave(userid, 0, dateflag);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        //i8web版主页取默认打开tab页
        public DataTable LoadDefaultOpenTabForMainFrame()
        {
            long userid = AppInfoBase.UserID;
            DataTable menudt = new DataTable();
            string sql = string.Format(@"select * from 
                                (select fg3_defaultopen_tab.originalcode,fg3_defaultopen_tab.name,fg3_menu.url,fg3_menu.managername,fg3_menu.rightname,fg3_menu.functionname,fg3_menu.suite,fg3_menu.moduleno,fg3_menu.rightkey,fg3_defaultopen_tab.urlparm 
                                from fg3_defaultopen_tab,fg3_menu 
                                where fg3_menu.code = fg3_defaultopen_tab.originalcode and (originalcode <> '' or originalcode is not null) and fg3_defaultopen_tab.usertype = 0 and userid = {0} 
                                union  
                                select originalcode,fg3_defaultopen_tab.name,fg3_defaultopen_tab.url,null,null,null,null,null,null,urlparm 
                                from fg3_defaultopen_tab 
                                where (originalcode = '' or originalcode is null) and userid = {0} and usertype = 0 
                                ) t1 ",userid);
            menudt = DbHelper.GetDataTable(sql);
            if (menudt.Rows.Count < 1)
            {
                userid = UserConfigDac.ActorGet(userid);
                sql = string.Format(@"select * from 
                                (select fg3_defaultopen_tab.originalcode,fg3_defaultopen_tab.name,fg3_menu.url,fg3_menu.managername,fg3_menu.rightname,fg3_menu.functionname,fg3_menu.suite,fg3_menu.moduleno,fg3_menu.rightkey,fg3_defaultopen_tab.urlparm 
                                from fg3_defaultopen_tab,fg3_menu 
                                where fg3_menu.code = fg3_defaultopen_tab.originalcode and (originalcode <> '' or originalcode is not null) and fg3_defaultopen_tab.usertype = 0 and userid = {0} 
                                union  
                                select originalcode,fg3_defaultopen_tab.name,fg3_defaultopen_tab.url,null,null,null,null,null,null,urlparm 
                                from fg3_defaultopen_tab 
                                where (originalcode = '' or originalcode is null) and userid = {0} and usertype = 0 
                                ) t1 ", userid);
                menudt = DbHelper.GetDataTable(sql);
            }
            return menudt;
        }
    }
}
