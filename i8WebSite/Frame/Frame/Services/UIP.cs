using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NG3.ESB;
using System.Data;
using NG3.Data.Service;

namespace NG3.SUP.Frame
{
    public partial class UIP : ServiceBase<UIP>
    {
        public int ChangePW(string pw, string newpw)
        {
            DataTable UserDt = DbHelper.GetDataTable(AppInfoBase.UserConnectString, string.Format("select logid,u_name,pwd,ocode,usergroup from secuser where logid='{0}' ", AppInfoBase.LoginID));
            if (UserDt.Rows.Count == 0)
            {
                return -1;
            }
            else
            {
                string oldpwd= UserDt.Rows[0]["pwd"] ==DBNull.Value ? string.Empty:UserDt.Rows[0]["pwd"].ToString();
                if (!oldpwd.Equals(NG3.NGEncode.EncodeMD5(pw)))
                {
                    return 0;
                }
                else
                {
                    DbHelper.ExecuteScalar(string.Format("update secuser set pwd='{0}' where logid='{1}' ", NG3.NGEncode.EncodeMD5(newpw), AppInfoBase.LoginID));
                    return 1;
                }
            }
        }
    }
}