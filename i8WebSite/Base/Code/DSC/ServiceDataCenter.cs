using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NG3;
using System.Text;
using System.Data;
using NG3.Data;
using Newtonsoft.Json.Linq;
using NG3.Data.Builder;
using NG3.ESB;
using NG3.Base;

namespace NG3.SUP.Base
{
    public class ServiceDataCenter : ServiceBase<ServiceDataCenter>
    {
        private static string DataCenter_FullPath = Pub.AppPath + "SUP/Base/DataCenter.aspx";

        protected override void Init()
        {
            base.Init();

            this.DataServicePath.Add("Base/DataCenter.xml");
        }

        protected override object Run(object[] ps)
        {
            var action = ps.GetOrDefault<string>(0, string.Empty).ToLower();
            var id = ps.GetOrDefault<string>(1);
            switch (action)
            {
                case "list":
                    return List(id);
                case "data":
                    return GetData(id);
                case "desc":
                    return GetDesc(id);
            }

            return base.Run(ps);
        }

        private string List(string pid = null)
        {
            QueryParam query = new QueryParam();
            query.Add(new NameValuePair<object>("pid", pid));
            var dt = this.DataService.GetExecuteCmd("List").AsBuilder(this.Buider, query).GetDataTable();

            StringBuilder sb = new StringBuilder();
            //sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
            sb.Append("<root>");

            foreach (DataRow dr in dt.Rows)
            {
                sb.AppendFormat("<ds text=\"{0}\" PID=\"{1}\" ", dr["ds_name"], dr["pid"]);

                if (dr["ds_isfolder"].TryParseToString() == "1")
                {//目录
                    sb.AppendFormat(" lazyLoad=\"{0}?pid={1}\"", DataCenter_FullPath, dr["id"]);
                }
                else
                {
                    sb.AppendFormat(" id=\"{0}\" descURL=\"{1}/desc/{2}\" dataURL=\"{1}/data/{2}\"", dr["id"], DataCenter_FullPath, dr["dsc_id"]);
                }
                sb.Append("/>");
            }

            sb.Append("</root>");

            return sb.ToString();
        }

        private string GetData(string id)
        {
            return null;
        }

        private string GetDesc(string id)
        {
            return null;
        }

        public string BreakToJson(string dsXML)
        {
            int i;
            DataServiceMeta dsm = new DataServiceMeta(new string[] { dsXML });
            JObject jo = new JObject();

            #region Tables
            JArray joTables = new JArray();
            for (i = 0; i < dsm.Tables.TableCount; i++)
            {
                var tb = dsm.Tables.GetTable(i);
                JObject joTable = (JObject)tb.ToJToken();
                JArray jaFields = new JArray();
                var fs = dsm.Fields.GetAllFields(SqlMode.Select, tb.Key);
                foreach (var f in fs)
                {
                    jaFields.Add(f.ToJToken());
                }
                joTable.Add("Fields", jaFields);

                joTables.Add(joTable);
            }
            jo.Add("Tables", joTables);
            #endregion

            //Read
            jo.Add(dsm.GetReadCmd("Read").ToJToken());

            return jo.ToString();
        }

        public string GetServiceXML(string serviceID)
        {
            string sql = @"select dsc_xmlcontent from esb_dataservice_main 
where id in (select dsc_id from esb_report_datasource where dsc_id like 'S%' and id='{0}')
union all
select dsc_xmlcontent from esb_dataservice_custom
where id in (select dsc_id from esb_report_datasource where dsc_id like 'C%' and id='{1}')";
            sql = string.Format(sql, serviceID, serviceID);

            string xml = this.Buider.SQL(sql).ExecuteScalar().TryParseToString();

            return xml;
        }
    }
}