using System;
using System.Collections.Generic;
using NG3.Data.Service;
using SUP.Common.Base;

namespace SUP.Frame.DataAccess
{
    public class ChangePwdDac
    {

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

            //密码最小位数
            strsql = "select info_int from fg_info where sysno = 'passlong'";
            obj = DbHelper.ExecuteScalar(strsql);
            if (obj != null)
            {
                pwdLimit += "," + obj.ToString();
            }
            else
            {
                pwdLimit += ",";
            }

            //新密码与老密码是否不能一致、
            strsql = "select info_int from fg_info where sysno = 'passnorepeat'";
            obj = DbHelper.ExecuteScalar(strsql);
            if (obj != null)
            {
                pwdLimit += "," + obj.ToString();
            }
            else
            {
                pwdLimit += ",";
            }

            //密码是否允许为空
            strsql = "select info_int from fg_info where sysno = 'passneed'";
            obj = DbHelper.ExecuteScalar(strsql);
            if (obj != null)
            {
                pwdLimit += "," + obj.ToString();
            }
            else
            {
                pwdLimit += ",";
            }

            return pwdLimit;
        }

        /// <summary>
        /// 取得密码
        /// </summary>
        /// <param name="logid"></param>
        /// <returns></returns>
        public string GetPassWord(string pubconnstr,string logid)
        {
            string sql = string.Empty;
            object obj = null;

            if (NG3.AppInfoBase.UserType != UserType.System)
            {
                sql = "select pwd from fg3_user where userno='" + logid + "'";
                obj = DbHelper.ExecuteScalar(sql);
            }
            else
            {
                sql = "select cpwd from ngrights where cname='" + logid + "'";
                obj = DbHelper.ExecuteScalar(pubconnstr, sql);
            }

            if (obj != null)
            {
                return obj.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="logid"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public int SetPassWord(string pubconnstr, string logid, string password)
        {
            string sql = string.Empty;
            int iret = -1;

            if (NG3.AppInfoBase.UserType != UserType.System)
            {
                sql = "update fg3_user set pwd='" + password + "',chgpwd_date='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', mucpwd = 0 where userno='" + logid + "'";
                iret = DbHelper.ExecuteNonQuery(sql);
            }
            else
            {
                sql = "update ngrights set cpwd='" + password + "' where cname='" + logid + "'";
                iret = DbHelper.ExecuteNonQuery(pubconnstr, sql);
            }

            return iret;
        }

    }


}
