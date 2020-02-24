using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NG3.Data;
using NG3.Data.Service;
using NG3;
using SUP.Common.Base;

namespace SUP.Frame.DataAccess
{
    public class UserDac
    {

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="logid"></param>
        /// <param name="oldpwd"></param>
        /// <param name="newpwd"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string ChangePwd(string logid, string oldpwd, string newpwd, ref string msg)
        {
            int iret = -1;

            string sql = "select pwd from secuser where logid={0}";


            IDataParameter[] p = new NGDataParameter[1];
            p[0] = new NGDataParameter("logid", logid);

            string pwd = DbHelper.GetString(sql, p);

            //旧密码输入不正确
            if (oldpwd != NGEncode.DecodePassword(pwd, 128))
            {
                msg = "旧密码输入不正确";
                return "-1";
            }

            string endcodepwd = NGEncode.EncodePassword(newpwd, 128);

            sql = "update secuser set pwd='" + endcodepwd + "',chgpwd_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where logid='" + logid + "'";
            iret = DbHelper.ExecuteNonQuery(sql);

            return "1";

        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="logid"></param>   
        /// <param name="newpwd"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string ResetPwd(string logid, string newpwd, ref string msg)
        {
            int iret = -1;

            string endcodepwd = NGEncode.EncodePassword(newpwd, 128);

            string sql = "update secuser set pwd='" + endcodepwd + "',chgpwd_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where logid='" + logid + "'";
            iret = DbHelper.ExecuteNonQuery(sql);

            return "1";

        }

        /// <summary>
        /// 获取密码的限制，以"@"分割,格式为 密码必须由数字和字母组成@密码最小位数#新密码与老密码是否不能一致，
        /// 如1@6#1，表示密码必须由数字和字母组成，密码最小位数为6，新密码与老密码不能一致
        /// </summary>
        /// <returns></returns>
        public string GetPwdLimit()
        {
            string pwdLimit = string.Empty;
            //密码必须由数字和字母组成
            string strsql = "select info_int from fg_info where sysno = 'passstrength'";
            object obj = DbHelper.ExecuteScalar(strsql);
            if (obj != null)
            {
                pwdLimit = obj.ToString();
            }
            else 
            {
                pwdLimit = "0";
            }
            

            //密码最小位数
            strsql = "select info_int from fg_info where sysno = 'passlong'";
            obj = DbHelper.ExecuteScalar(strsql);
            if (obj != null)
            {
                pwdLimit += "#" + obj.ToString();
            }
            else
            {
                pwdLimit += "#0";
            }

            //新密码与老密码是否不能一致、
            strsql = "select info_int from fg_info where sysno = 'passnorepeat'";
            obj = DbHelper.ExecuteScalar(strsql);
            if (obj != null)
            {
                pwdLimit += "#" + obj.ToString();
            }
            else 
            {
                pwdLimit += "#1";
            }

            return pwdLimit;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="svrName">数据库服务器名</param>
        /// <param name="account">帐套</param>
        /// <param name="logid">登录id</param>
        /// <param name="pwd">密码</param>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public bool Login(string svrName, string account, string logid, string pwd,ref string msg)
        {
            string result;
            DBConnectionStringBuilder dbbuilder = new DBConnectionStringBuilder();
            string pubConn = string.Empty;
            string userConn = string.Empty;

            if (string.IsNullOrWhiteSpace(svrName))
            {
                pubConn = dbbuilder.GetMainConnStringElement(0, out result, false);//取第一个（默认）服务器
            }
            else
            {
                pubConn = dbbuilder.GetMainConnStringElement(svrName, out result);
            }

            if (string.IsNullOrWhiteSpace(account))
            {
                userConn = dbbuilder.GetDefaultConnString();//取默认连接串
            }
            else
            {
                userConn = dbbuilder.GetAccConnstringElement(svrName, account, pubConn, out result);
            }

            I6WebAppInfo appInfo = new I6WebAppInfo();
            appInfo.UserType = UserType.OrgUser;

            #region 校验用户是否存在
            
            object obj = DbHelper.ExecuteScalar(userConn, string.Format("select count(logid) from secuser where logid='{0}'", logid));
            if (obj == null || obj == DBNull.Value || obj.ToString() == "0")
            {
                //检测系统管理员
                obj = DbHelper.ExecuteScalar(pubConn, string.Format("select count(cname) from ngrights where cname='{0}'", logid));
                if (obj == null || obj == DBNull.Value || obj.ToString() == "0")
                {
                    //this.SetErrMsg(ps, "不存在该用户!");
                    //return false;

                    msg = "不存在该用户!";                   
                    return false;
                }
                else
                {
                    appInfo.UserType = SUP.Common.Base.UserType.System;
                }
            }
            #endregion

            #region 用户状态
            string sql = "select status from secuser where logid='"+ logid +"'";
            string ret = DbHelper.GetString(userConn,sql);

            if (ret == "1")
            {
                msg = "用户已锁定，请联系系统管理员!";
                return false;
            }
            #endregion

            #region 校验密码


            if (UserType.OrgUser == appInfo.UserType)
            {
                obj = DbHelper.ExecuteScalar(userConn, string.Format("select pwd from secuser where logid='{0}'", logid));
            }
            else
            {
                obj = DbHelper.ExecuteScalar(pubConn, string.Format("select cpwd from ngrights where cname='{0}'", logid));
            }

            if (string.IsNullOrEmpty(pwd))
            {
                if (obj != null && obj != DBNull.Value)
                {
                    if (obj.ToString().Length > 0)
                    {
                        msg = "密码不正确";                        
                        return false;
                    }
                }
            }
            else
            {
                if (obj == null || obj == DBNull.Value)
                {
                    msg = "密码不正确";                    
                    return false;
                }
                else
                {
                    string dbpwd = NG3.NGEncode.DecodePassword(obj.ToString(), 128);
                    if (dbpwd.Equals(pwd) == false)
                    {
                        msg = "密码不正确";                       
                        return false;
                    }
                }
            }

            #endregion

            return true;
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="logid">操作员id</param>
        /// <param name="username">操作员名字</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public bool AddUser(string logid, string username, string pwd,long maxid)
        {
            string count = DbHelper.GetString("select count(*) from secuser where logid='" + logid + "'");

            if (count.ToString() != "0")
            {
                throw new Exception("用户["+ logid +"]已存在！");
            }

            string endcodepwd = NGEncode.EncodePassword(pwd, 128);

            string sql = "insert into secuser (logid,u_name,pwd,lg_sign,creadate) values ({0},{1},{2},{3},{4})";

            IDataParameter[] p = new NGDataParameter[5];
            p[0] = new NGDataParameter("logid", DbType.AnsiString);
            p[0].Value = logid;
            p[1] = new NGDataParameter("username", DbType.AnsiString);
            p[1].Value = username;
            p[2] = new NGDataParameter("pwd", DbType.AnsiString);
            p[2].Value = endcodepwd;
            p[3] = new NGDataParameter("lg_sign", DbType.AnsiString);
            p[3].Value = "1";

            p[4] = new NGDataParameter("creadate", DbType.Date);
            p[4].Value = DateTime.Now;

            int iret = DbHelper.ExecuteNonQuery(sql,p);

            //处理fg_orgpop
            //string maxid = DbHelper.GetString("select MAX(id) from fg_orgpop");            
            //int id;
            //Int32.TryParse(maxid, out id);

            sql = "insert into fg_orgpop (id,ug_code,u_soft,ocode) values({0},{1},{2},{3})";

            IDataParameter[] param = new NGDataParameter[4];
            param[0] = new NGDataParameter("id", DbType.Int64);
            param[0].Value = maxid;
            param[1] = new NGDataParameter("ug_code", DbType.AnsiString);
            param[1].Value = username;
            param[2] = new NGDataParameter("u_soft", DbType.AnsiString);
            param[2].Value = "01";//类型，01是用户
            param[3] = new NGDataParameter("ocode", DbType.AnsiString);
            param[3].Value = NG3.AppInfoBase.OCode;
            iret += DbHelper.ExecuteNonQuery(sql,param);

           if (iret > 0)
           {
               return true;
           }
           else
           {
               return false;
           }
        }

        /// <summary>
        /// 锁定用户
        /// </summary>
        /// <param name="logid"></param>
        /// <returns></returns>
        public bool LockUser(string logid)
        {
            return this.UpdateUserStatus(logid,"1");
        }

        /// <summary>
        /// 解锁用户
        /// </summary>
        /// <param name="logid"></param>
        /// <returns></returns>
        public bool UnLockUser(string logid)
        {
            return this.UpdateUserStatus(logid, "0");
        }

        private bool UpdateUserStatus(string logid,string status)
        {
           string sql = "update secuser set status = '"+ status +"' where logid=" + DbConvert.ToSqlString(logid);//锁定：status=1；未锁定：status=0或null
           int iret = DbHelper.ExecuteNonQuery(sql);

           if (iret > 0)
           {
               return true;
           }
           else
           {
               return false;
           }
        }

       
    }
}
