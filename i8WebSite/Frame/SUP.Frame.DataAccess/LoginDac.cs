using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using NG3;
using NG3.Data;
using NG3.Data.Service;
using SUP.Common.Base;
using System.Web;
using NG3.Bill.Base;

namespace SUP.Frame.DataAccess
{
    public class LoginDac
    {
        private const string WebNGWebAppInfo = "webNGWebAppInfo";
        public LoginDac()
        {

        }

        public int GetErrTimes(string logid, string connStr)
        {
            int Myresult = 0;
            string Temp;
            string sql = "select errtimes from fg3_user where userno = '{0}'";
            sql = string.Format(sql, logid);

            try
            {
                Temp = DbHelper.GetString(connStr, sql);
                if ((Temp == null) || (Temp == ""))
                {
                    ClearErr(logid, connStr);
                }
                else
                {
                    Myresult = Convert.ToInt32(Temp);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Myresult;
        }

        public bool AddErrTimes(string logid, string connStr)
        {
            string sql = "update fg3_user set errtimes = errtimes+1 where userno = '{0}'";

            sql = string.Format(sql, logid);
            try
            {
                DbHelper.ExecuteScalar(connStr, sql);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        public bool ClearErr(string logid, string connStr)
        {
            string sql = "update fg3_user set errtimes = 0 where userno = '{0}'";

            sql = string.Format(sql, logid);
            try
            {
                DbHelper.ExecuteScalar(connStr, sql);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //public int GetSysErrTimes(string connStr)
        //{
        //    string Mysql;
        //    object result;
        //    int Times = 0;
        //    Mysql = "select c_value from fg_paraminit where c_code = '01008'";
        //    try
        //    {
        //        result = DbHelper.ExecuteScalar(connStr, Mysql);
        //        if (result == null)
        //            result = "0";
        //        Times = Convert.ToInt32(result.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    return Times;
        //}

        public int GetSysErrTimes(string connStr)
        {
            string Mysql;
            int check = 0;
            object result;
            int Times = 6;//防止不存在选项所有账号被锁定
            

            OptionSetting optionSetting = new OptionSetting();
            Mysql = "select c_value from fg_paraminit where c_code = '01008'";
            try
            {
                string option_value = optionSetting.GetValueByKey(connStr, "LOGIN", "largeErrorTime","");
                //新账套可能会没有选项
                if (Int32.TryParse(option_value, out check))
                {
                    Times = Convert.ToInt32(option_value) + 1;
                }

                else
                {
                    //result = DbHelper.ExecuteScalar(connStr,Mysql);
                    //if (result == null)
                    //    result = "6";
                    //Times = Convert.ToInt32(result.ToString());
                    Times = 6;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Times;
        }

        public bool SetUserStateOff(string logid, string connStr)
        {
            string sql = "update fg3_user set status = 3 where userno = '{0}'";//锁定：status: 0 删除 1 激活 2 禁用 3 锁定 4 过期
            sql = string.Format(sql, logid);
            try
            {
                DbHelper.ExecuteScalar(connStr, sql);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        public string GetLangInfo()
        {
            string sql = "select lang_key,lang_desc from ng3_use_lang";
            DataTable dt = DbHelper.GetDataTable(sql);
            int totalRecord = dt.Rows.Count > 0 ? dt.Rows.Count : 0;
            return DataConverterHelper.ToJson(dt, totalRecord);
        }

        public string GetLoginIDByMobileOrEmail(string userName)
        {
            string sql = string.Format("select userno from fg3_user where mobileno='{0}'", userName);
            DataTable dt = DbHelper.GetDataTable("select userno from fg3_user where userno='" + userName + "'");
            if (dt.Rows.Count > 0)
            {
                return userName;
            }
            else
            {
                if (userName.IndexOf("@") > 0)
                {
                    sql = string.Format("select userno from fg3_user where email='{0}'", userName);
                }

                dt = DbHelper.GetDataTable(sql);
                if (dt.Rows.Count > 1)
                {
                    return "1";
                }
                else if (dt.Rows.Count == 0)
                {
                    return "0";
                }
                else
                {
                    return dt.Rows[0]["userno"].ToString();
                }
            }


            //return dt.Rows.Count == 1 ? dt.Rows[0]["userno"].ToString() : ""; 
        }

        public void SetCookieValue()
        {
            Dictionary<string, string> setList = GetCookieValue(NG3.AppInfoBase.UserType, NG3.AppInfoBase.LoginID);
            if (setList.Count > 0)
            {
                if (setList.ContainsKey("ngloginimage"))
                {
                    SetCookie("ngloginimage", setList["ngloginimage"], DateTime.MaxValue);
                }

                SetCookie("showlogo", setList["showlogo"] == null ? "1" : setList["showlogo"], DateTime.MaxValue);
            }
        }

        /// <summary>
        /// 登录成功时给页面设置cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="cookieValue"></param>
        /// <param name="expiress"></param>
        public void SetCookie(string cookieName, string cookieValue, DateTime expiress)
        {
            if (HttpContext.Current.Request.Cookies["src"] != null)
            {
                HttpContext.Current.Response.Cookies["src"].Expires = DateTime.Now.AddDays(-1);
            }
            HttpCookie cookie = new HttpCookie(cookieName);
            bool isExist = false;
            if (HttpContext.Current.Request.Cookies[cookieName] != null)
            {
                HttpContext.Current.Response.Cookies.Add(cookie);
                isExist = true;
            }

            cookie.Name = cookieName;
            cookie.Value = cookieValue;
            cookie.Expires = expiress;

            if (!isExist)
            {
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            else
            {
                HttpContext.Current.Response.Cookies[cookieName].Value = cookie.Value;
                HttpContext.Current.Response.Cookies[cookieName].Expires = cookie.Expires;
            }
        }


        //public Dictionary<string, string> GetCookieValue(string userType, string userName)
        //{
        //    Dictionary<string, string> setList = new Dictionary<string, string>();
        //    DbHelper.Open();
        //    string sql = "";
        //    DataTable dt = new DataTable();
        //    if (userType == UserType.System)
        //    {
        //        sql = "select * from fg3_loginpictureset where phid=0";
        //    }
        //    else
        //    {
        //        DataTable userDt = DbHelper.GetDataTable("select phid from fg3_user where userno='" + userName + "'");
        //        if (userDt != null && userDt.Rows.Count > 0)
        //        {
        //            sql = "select * from fg3_loginpictureset where phid=" + Convert.ToInt64(userDt.Rows[0]["phid"]) + "";
        //        }
        //    }

        //    dt = DbHelper.GetDataTable(sql);
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        DataTable srcDt = new DataTable();
        //        setList.Add("showlogo", dt.Rows[0]["showlogo"] != DBNull.Value ? dt.Rows[0]["showlogo"].ToString() : "");

        //        if (dt.Rows[0]["showtype"].ToString() == "0" && dt.Rows[0]["showpic"] != DBNull.Value && dt.Rows[0]["showpic"].ToString() != "")
        //        {
        //            srcDt = DbHelper.GetDataTable("select src,attachid  from fg3_loginpicture where phid=" + Convert.ToInt64(dt.Rows[0]["showpic"]) + "");
        //            setList.Add("ngloginimage", srcDt.Rows.Count > 0 ? srcDt.Rows[0]["src"].ToString() : "");
        //        }
        //        else if (dt.Rows[0]["showtype"].ToString() == "1" && dt.Rows[0]["showpic2"] != DBNull.Value && dt.Rows[0]["showpic2"].ToString() != "")
        //        {
        //            string result = dt.Rows[0]["showpic2"].ToString();
        //            if (result.EndsWith(";"))
        //            {
        //                result = result.Substring(0, result.Length - 1);
        //            }
        //            string[] arr = result.Split(';');

        //            Int64[] phidList = Array.ConvertAll<string, Int64>(arr, delegate (string s)
        //            {
        //                return Int64.Parse(s);
        //            });
        //            string strsql = "select src,attachid from fg3_loginpicture where phid in (" + string.Join(",", phidList) + ")";
        //            srcDt = DbHelper.GetDataTable(strsql);
        //            if (srcDt.Rows.Count > 0)
        //            {
        //                string src = "";
        //                for (int i = 0; i < srcDt.Rows.Count; i++)
        //                {
        //                    if (srcDt.Rows[i]["src"].ToString() != "")
        //                    {
        //                        src = src + srcDt.Rows[i]["src"].ToString() + "#";
        //                    }
        //                }
        //                setList.Add("ngloginimage", src);
        //            }
        //        }
        //    }
        //    DbHelper.Close();
        //    return setList;
        //}

        
        public Dictionary<string, string> GetCookieValue(string userType, string userName)
        {
            Dictionary<string, string> setList = new Dictionary<string, string>();
            DbHelper.Open();
            string sql = "";
            DataTable dt = new DataTable();
            sql = "select * from fg3_loginpictureset where phid=0";
            dt = DbHelper.GetDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                //允许操作员自定义背景
                if (dt.Rows[0]["allowuser"].ToString() != "0" && userType != UserType.System)
                {
                    string phid = DbHelper.GetString("select phid from fg3_user where userno='" + userName + "'");
                    if (!string.IsNullOrEmpty(phid))
                    {
                        string strsql = "select * from fg3_loginpictureset where phid=" + Convert.ToInt64(phid) + "";
                        DataTable dtUser = DbHelper.GetDataTable(strsql);
                        //操作员有设置过自定义背景
                        if (dtUser != null && dtUser.Rows.Count > 0)
                        {
                            //操作员自定义背景不为空，为空继续取管理员设置的背景
                            if ((dtUser.Rows[0]["showtype"].ToString() == "0" && dtUser.Rows[0]["showpic"] != DBNull.Value) || (dtUser.Rows[0]["showtype"].ToString() == "1" && dtUser.Rows[0]["showpic2"] != DBNull.Value))
                            {
                                dt = dtUser.Copy();
                            }
                        }
                    }
                    //DataTable userDt = DbHelper.GetDataTable("select phid from fg3_user where userno='" + userName + "'");
                    //if (userDt != null && userDt.Rows.Count > 0)
                    //{
                    //    string strsql = "select * from fg3_loginpictureset where phid=" + Convert.ToInt64(userDt.Rows[0]["phid"]) + "";
                    //    DataTable setDt = DbHelper.GetDataTable(sql);
                    //    if (setDt != null && setDt.Rows.Count > 0)
                    //    {
                    //        dt = DbHelper.GetDataTable(sql);
                    //    }
                    //}
                }
            }
            setList = this.GetLoginPicSet(dt);
            DbHelper.Close();
            return setList;
        }

        public Dictionary<string, string> GetLoginPicSet(DataTable dt)
        {
            Dictionary<string, string> setList = new Dictionary<string, string>();
            if (dt != null && dt.Rows.Count > 0)
            {
                DataTable srcDt = new DataTable();
                setList.Add("showlogo", dt.Rows[0]["showlogo"] != DBNull.Value ? dt.Rows[0]["showlogo"].ToString() : "");

                //设置一张背景图
                if (dt.Rows[0]["showtype"].ToString() == "0")
                {
                    if (dt.Rows[0]["showpic"] != DBNull.Value && dt.Rows[0]["showpic"].ToString() != "")
                    {
                        srcDt = DbHelper.GetDataTable("select src,attachid  from fg3_loginpicture where phid=" + Convert.ToInt64(dt.Rows[0]["showpic"]) + "");
                        setList.Add("ngloginimage", srcDt.Rows.Count > 0 ? srcDt.Rows[0]["src"].ToString() : "");
                    }
                    //操作员自定义背景为空跑不到这一步
                    else
                    {
                        setList.Add("ngloginimage", "");
                    }
                }
                //设置多张背景图
                else if (dt.Rows[0]["showtype"].ToString() == "1" && dt.Rows[0]["showpic2"] != DBNull.Value && dt.Rows[0]["showpic2"].ToString() != "")
                {
                    string result = dt.Rows[0]["showpic2"].ToString();
                    if (result.EndsWith(";"))
                    {
                        result = result.Substring(0, result.Length - 1);
                    }
                    string[] arr = result.Split(';');

                    Int64[] phidList = Array.ConvertAll<string, Int64>(arr, delegate (string s)
                    {
                        return Int64.Parse(s);
                    });
                    string strsql = "select src,attachid from fg3_loginpicture where phid in (" + string.Join(",", phidList) + ")";
                    srcDt = DbHelper.GetDataTable(strsql);
                    if (srcDt.Rows.Count > 0)
                    {
                        string src = "";
                        for (int i = 0; i < srcDt.Rows.Count; i++)
                        {
                            if (srcDt.Rows[i]["src"].ToString() != "")
                            {
                                src = src + srcDt.Rows[i]["src"].ToString() + "#";
                            }
                        }
                        setList.Add("ngloginimage", src);
                    }
                }
            }
            return setList;
        }

        public string GetAttachid(string src)
        {
            string result = "";

            DataTable dt = DbHelper.GetDataTable("select attachid from fg3_loginpicture where src='" + src + "'");
            result = dt.Rows.Count > 0 ? dt.Rows[0]["attachid"].ToString() : "";
            return result;
        }

        public Dictionary<string, string> GetPortalListBarLanguage()
        {
            return Common.DataAccess.LangInfo.GetLabelLang("PortalListBar");
        }
        
        public void UpdateLastLoginOrg(string loginorg)
        {
            var appInfo = System.Web.HttpContext.Current.Session["webNGWebAppInfo"] as SUP.Common.Base.I6WebAppInfo;
            string sql = string.Format(@"UPDATE fg3_user
                            SET lastloginorg='{0}'
                            WHERE PHID={1}", loginorg, appInfo.UserID);
            try
            {
                DbHelper.Open();
                DbHelper.BeginTran();
                DbHelper.ExecuteNonQuery(sql);
                DbHelper.CommitTran();
            }
            catch
            {
                DbHelper.RollbackTran();
            }
            finally
            {
                DbHelper.Close();
            }
        }

        public DataTable GetLoginOrgList(out string msg, string userid, string database)
        {
            string sql = string.Empty;
            string username = string.Empty;
            string pubConn = string.Empty;
            string userConn = string.Empty;
            var dbbuilder = GetAcountDBConnectString("", database, out pubConn, out userConn);
            msg = "";
            sql = $@"select fg3_userorg.orgid,fg_orglist.ocode,fg_orglist.oname from fg3_user left outer join fg3_userorg on 
                     fg3_user.phid = fg3_userorg.userid left outer  join fg_orglist on fg3_userorg.orgid = fg_orglist.phid 
                     where userno = '{userid}' and inner_type = 1 order by fg_orglist.ocode ASC";
            DataTable dt = DbHelper.GetDataTable(userConn, sql);
            if (dt == null || dt.Rows.Count == 0)
                msg = "当前用户名没有可登录组织";
            return dt;

        }

        /// <summary>
        /// 设置在线用户
        /// </summary>
        public string SetLoginUsers(I6WebAppInfo appInfo)
        {
            string sql = string.Empty;
            var guid = Guid.NewGuid().ToString();
            switch (DbHelper.Vendor)
            {
                case DbVendor.Oracle:
                case DbVendor.Oracle10:
                case DbVendor.Oracle11:
                case DbVendor.Oracle9:
                    sql = $@"INSERT INTO FG_LOGINHISTORY (CODE, LOGINID, LOGINNAME, UCODE, OCODE, ONAME, LOGINDT, ORGID, USERID, DEVICETYPE)
VALUES('{guid}', '{appInfo.LoginID}', '{appInfo.UserName}', '{appInfo.UCode.Replace("NG", "")}', '{appInfo.OCode}', '{appInfo.OrgName}', TO_DATE('{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}', 'YYYY/MM/DD HH24:MI:SS '),{appInfo.OrgID},{appInfo.UserID},1)";
                    break;
                case DbVendor.SQLServer:
                case DbVendor.SQLServer2000:
                case DbVendor.SQLServer2005:
                case DbVendor.SQLServer2008:
                    sql = $@"INSERT INTO FG_LOGINHISTORY (CODE, LOGINID, LOGINNAME, UCODE, OCODE, ONAME, LOGINDT, ORGID, USERID, DEVICETYPE)
VALUES('{guid}', '{appInfo.LoginID}', '{appInfo.UserName}', '{appInfo.UCode.Replace("NG", "")}', '{appInfo.OCode}', '{appInfo.OrgName}', '{DateTime.Now}',{appInfo.OrgID},{appInfo.UserID},1)";
                    break;
            }
            if (!string.IsNullOrEmpty(sql))
            {
                try
                {
                    DbHelper.Open();
                    DbHelper.BeginTran();
                    DbHelper.ExecuteNonQuery(sql);
                    DbHelper.CommitTran();
                }
                catch
                {
                    DbHelper.RollbackTran();
                }
                finally
                {
                    DbHelper.Close();
                }
            }
            return guid;
        }

        /// <summary>
        /// 退出更新登录历史
        /// </summary>
        public void SetLoginUserOut(string guid)
        {
            string sql = string.Empty;
            switch (DbHelper.Vendor)
            {
                case DbVendor.Oracle:
                case DbVendor.Oracle10:
                case DbVendor.Oracle11:
                case DbVendor.Oracle9:
                    sql = $@"UPDATE FG_LOGINHISTORY
                            SET LOGOFFDT = TO_DATE('{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}', 'YYYY/MM/DD HH24:MI:SS ')
                            WHERE CODE = '{guid}'";
                    break;
                case DbVendor.SQLServer:
                case DbVendor.SQLServer2000:
                case DbVendor.SQLServer2005:
                case DbVendor.SQLServer2008:
                    sql = $@"UPDATE FG_LOGINHISTORY
                            SET LOGOFFDT = '{DateTime.Now}'
                            WHERE CODE = '{guid}'";
                    break;
            }
            if (!string.IsNullOrEmpty(sql))
            {
                try
                {
                    DbHelper.Open();
                    DbHelper.ExecuteNonQuery(sql);
                }
                catch
                {

                }
                finally
                {
                    DbHelper.Close();
                }
            }
        }

        /// <summary>
        /// 获取用户加密密码 在第三方中获取logid后，自己系统模拟登录时拉取下密码
        /// </summary>
        /// <param name="logid">用户名</param>
        /// <returns></returns>
        public string GetEncryPassWord(string svrName, string database, string logid)
        {
            string pubConn = string.Empty;
            string userConn = string.Empty;
            GetAcountDBConnectString(svrName, database, out pubConn, out userConn);
            try
            {
                DbHelper.Open(userConn);
                string sql = "select pwd from secuser where logid='" + logid + "'";
                return DbHelper.GetString(userConn, sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DbHelper.Close(userConn);
            }
        }

        public static DBConnectionStringBuilder GetAcountDBConnectString(string svrName, string database, out string pubConn, out string userConn)
        {
            var dbbuilder = new DBConnectionStringBuilder();
            string result;

            if (string.IsNullOrWhiteSpace(svrName))
            {
                pubConn = dbbuilder.GetMainConnStringElement(0, out result, false);//取第一个（默认）服务器
            }
            else
            {
                pubConn = dbbuilder.GetMainConnStringElement(svrName, out result);
            }
            if (string.IsNullOrWhiteSpace(database))
            {
                userConn = dbbuilder.GetDefaultConnString();//取默认连接串
            }
            else
                userConn = dbbuilder.GetAccConnstringElement(svrName, "NG" + database.Replace("NG", ""), pubConn, out result);
            return dbbuilder;
        }

        public string GetCustomTitle()
        {
            try
            {
                string sql = "select def_str2 from ngusers where ucode ='" + NG3.AppInfoBase.UCode + "'";
                string title = DbHelper.GetString(NG3.AppInfoBase.PubConnectString, sql);
                return string.IsNullOrEmpty(title) ? "新中大i8工程企业管理软件" : title;
            }
            catch (Exception ex)
            {
                return "新中大i8工程企业管理软件";
                throw new Exception(ex.Message);
            }
        }

        public string GetConnectType(string svrName, string account)
        {
            try
            {
                string connectType = "0";
                var dbbuilder = new DBConnectionStringBuilder();
                string pubConn = string.Empty;
                string result;
                if (string.IsNullOrWhiteSpace(svrName))
                {
                    pubConn = dbbuilder.GetMainConnStringElement(0, out result, false);//取第一个（默认）服务器
                }
                else
                {
                    pubConn = dbbuilder.GetMainConnStringElement(svrName, out result);
                }
                DataTable dt = new DataTable();
                byte[] obj = null;

                DbHelper.Open(pubConn);
                string sql = "select file_value from fg_systemconfigfile where file_key='NG_NetWorkIPMapping_Data'";
                obj = (byte[])DbHelper.ExecuteScalar(pubConn, sql);
                if (obj != null)
                {
                    dt = NG3.Runtime.Serialization.SerializerBase.DeSerialize(obj) as DataTable;
                    if (dt != null && dt.Rows.Count > 0 && dt.Columns.IndexOf("connectType") != -1)
                    {
                        connectType = dt.Rows[0]["connectType"].ToString();
                    }
                }

                return connectType;
            }
            catch (Exception ex)
            {
                return "0";
                throw new Exception(ex.Message);
            }
        }

    }
}
