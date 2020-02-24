using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SUP.Frame.DataAccess;
using SUP.Common.Base;
using SUP.Frame.DataEntity;
using SUP.Common.Interface;
using System.Web;
using GE.BusinessRules.Common;

namespace SUP.Frame.Rule
{
    public class MyCommonFuncRule : IUserConfig
    {
        private MyCommonFuncDac dac = null;
        private PubCommonRule pcrule;
        public MyCommonFuncRule()
        {
            dac = new MyCommonFuncDac();
            pcrule = new PubCommonRule();
        }

        public DataTable LoadMyMenu()
        {
            return dac.LoadMyMenu();
        }

        public DataTable LoadMyMenuByType(string typecode)
        {
            DataTable FuncIconManager = pcrule.GetFuncIconDt();
            DataTable MyMenuDt = dac.LoadMyMenuByType(typecode);
            MyMenuDt.Columns.Add("icoUrl", typeof(string));
            DataRow[] icoDr;
            foreach (DataRow dr in MyMenuDt.Rows)
            {
                icoDr = FuncIconManager.Select(string.Format("busphid like'%{0}%'", dr["busphid"]));
                if (icoDr.Length > 0)
                {
                    dr["icoUrl"] = icoDr[0]["src"];
                }  
            }
            return MyMenuDt;
        }
        public int SaveMyMenu(DataTable masterdt)
        {
            return dac.SaveMyMenu(masterdt);
        }

        public DataTable LoadMyMenuType()
        {
            return dac.LoadMyMenuType();
        }

        public int SaveMyMenuType(string masterdt)
        {
            return dac.SaveMyMenuType(masterdt);
        }
        //public DataTable GetFuncIconDt()
        //{
        //    DataTable dt = new DataTable();
        //    dt.TableName = "FuncIconManager";
        //    dt.Columns.Add(new DataColumn("code", Type.GetType("System.String")));
        //    dt.Columns.Add(new DataColumn("id", Type.GetType("System.String")));
        //    dt.Columns.Add(new DataColumn("name", Type.GetType("System.String")));
        //    dt.Columns.Add(new DataColumn("src", Type.GetType("System.String")));
        //    string srcPre = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host
        //        + ":" + HttpContext.Current.Request.Url.Port + "/" + HttpContext.Current.Request.Url.Segments[1];
        //    DataTable funcIconDt = PubCommonDac.Instance.GetMenuFuncIcon();
        //    DataRow row;
        //    for (int i = 0; i < funcIconDt.Rows.Count; i++)
        //    {
        //        row = dt.NewRow();
        //        row["code"] = funcIconDt.Rows[i]["code"];
        //        row["id"] = funcIconDt.Rows[i]["id"];
        //        row["name"] = funcIconDt.Rows[i]["funcicondisplayname"];
        //        if (funcIconDt.Rows[i]["funciconid"].ToString() != String.Empty)
        //        {
        //            DataTable funcIconNameType = PubCommonDac.Instance.GetFuncIconNameType(funcIconDt.Rows[i]["funciconid"].ToString());
        //            if (funcIconNameType.Rows.Count > 0)
        //            {
        //                if (funcIconNameType.Rows[0]["icontype"].ToString() == "0")
        //                {
        //                    row["src"] = srcPre + "NG3Resource/FuncIcons/" + funcIconNameType.Rows[0]["name"];
        //                }
        //                else
        //                {
        //                    row["src"] = srcPre + "NG3Resource/CustomIcons/" + funcIconNameType.Rows[0]["name"];
        //                }
        //            }
        //            else
        //            {
        //                row["src"] = srcPre + "NG3Resource/FuncIcons/" + funcIconDt.Rows[i]["funciconname"];
        //            }
        //        }
        //        else
        //        {
        //            row["src"] = srcPre + "NG3Resource/FuncIcons/" + funcIconDt.Rows[i]["funciconname"];
        //        }
        //        dt.Rows.Add(row);
        //    }
        //    return dt;
        //}

        public bool CopyUserConfig(long fromUserId, int fromUserType, long toUserId, int toUserType)
        {
            return dac.UserConfigCopy(fromUserId, fromUserType, toUserId, toUserType);
        }

        public bool DeleteUserConfig(long userid, int usertype)
        {
            return dac.UserConfigDel(userid, usertype);
        }
    }
}
