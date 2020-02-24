using SUP.Frame.DataAccess;
using System;
using System.Data;

namespace SUP.Frame.Rule
{
    public class SysMaintainCallRule
    {

        private SysMaintainCallDac dac = null;

        public SysMaintainCallRule()
        {
            dac = new SysMaintainCallDac();
        }

        public DataTable GetSysMaintainCall(string clientJsonQuery)
        {
            DataTable dt = new DataTable();
            dt.TableName = "SysMaintainCall";

            dt.Columns.Add(new DataColumn("phid", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("sn", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("title", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("preenddate", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("filldate", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("endtype", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("state", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("startdate", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("starttime", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("enddate", Type.GetType("System.String")));

            DataTable sysMaintainCallDt = dac.GetQuerySysMaintainCallDt(clientJsonQuery);
            for (int i = 0; i < sysMaintainCallDt.Rows.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr["phid"] = sysMaintainCallDt.Rows[i]["phid"];
                dr["sn"] = i + 1;
                dr["title"] = sysMaintainCallDt.Rows[i]["title"].ToString();
                dr["preenddate"] = sysMaintainCallDt.Rows[i]["preenddate"].ToString();
                dr["filldate"] = sysMaintainCallDt.Rows[i]["filldate"].ToString();
                dr["endtype"] = sysMaintainCallDt.Rows[i]["endtype"].ToString() == "1" ? "手动结束" : "自动结束";
                dr["state"] = sysMaintainCallDt.Rows[i]["state"].ToString();
                dr["startdate"] = sysMaintainCallDt.Rows[i]["startdate"].ToString();
                dr["starttime"] = sysMaintainCallDt.Rows[i]["starttime"].ToString();
                dr["enddate"] = sysMaintainCallDt.Rows[i]["enddate"].ToString();
                dt.Rows.Add(dr);
            }

            return dt;
        }

        public string SaveSysMaintainCall(string phid, string title, string starttime, string preenddate, string endtype, string enddate, string runinfo, string endinfo, string netfreecall, string netfreecallucode, string allowlogin, string otype)
        {
            int iret = 0;
            if (otype == "add" || otype == "copy")
            {
                phid = SUP.Common.Rule.CommonUtil.GetPhId("fg3_sysmaintaincall").ToString();
                iret = dac.InsertSysMaintainCall(phid, title, starttime, preenddate, endtype, enddate, runinfo, endinfo, netfreecall, netfreecallucode, allowlogin);
            }
            else if (otype == "edit" || otype == "view")
            {
                iret = dac.UpdateSysMaintainCall(phid, title, starttime, preenddate, endtype, enddate, runinfo, endinfo, netfreecall, netfreecallucode, allowlogin);
            }

            return iret == 1 ? phid : "0";
        }

    }
}
