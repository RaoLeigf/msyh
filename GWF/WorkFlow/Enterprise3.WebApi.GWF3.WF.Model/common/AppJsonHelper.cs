using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using NG3;

namespace Enterprise3.WebApi.GWF3.WF.Model.common
{
    public class AppJsonHelper
    {
        #region toJsonString
      

        public static string ConvertObjectListToJsonStr(List<object> dts)
        {
            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
            //去掉日期格式的年份和秒
            timeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm";

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("{");
            foreach (object obj in dts)
            {
                if (obj is DataTable)
                {
                    DataTable dt = obj as DataTable;
                    sb.Append(string.Format("\"{0}\":{1}", dt.TableName, JsonConvert.SerializeObject(dt, timeConverter))).Append(",");
                }
                else if (obj.GetType().FullName.Contains("MobileAppBiz"))
                {
                    sb.Append(string.Format("\"{0}\":{1}", "bizData", JsonConvert.SerializeObject(obj, timeConverter))).Append(",");
                }
            }
            sb.Append("\"status\":\"succeed\",");
            sb.Append("\"errmsg\":\"\"}");

            return sb.ToString();
        }

        public static string ConvertErrorInfoToJsonStr(Exception ex)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("{");
            //foreach (string s in datanames)
            //{
                sb.Append("\"data\":[]").Append(",");
            //}
            sb.Append("\"status\":\"error\",");
            if (ex.InnerException != null)
            {
                sb.Append("\"errmsg\":\"" + ex.InnerException.Message + "\"}");
            }
            else
            {
                sb.Append("\"errmsg\":\"" + ex.Message + "\"}");
            }
            

            return sb.ToString();
        }

    
        #endregion

        public static JObject ConvertDtToJson(DataTable dt, string dtName="data", int rowCount = 0)
        {
            JObject data = new JObject();
            data.Add("status", "succeed");
            if (rowCount > 0)
            {
                data.Add("rowcount", rowCount);
            }
            data.Add("errmsg", string.Empty);
            JArray dtList = new JArray();
            foreach (DataRow r in dt.Rows)
            {
                dtList.Add(r.ToJObject());
            }
            data.Add(dtName, dtList);
            return data;
        }

        public static JObject ConvertErrorInfoToJson(Exception ex)
        {
            string msg= ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            JObject data = new JObject();
            data.Add("status", "error");
            data.Add("data", new JArray());
            data.Add("errmsg", msg);
            data.Add("stacktrace", ex.StackTrace);

            return data;
        }

        public static JObject ConvertResultToJson(bool isSucced, string errmsg)
        {
            JObject data = new JObject();
            data.Add("status", isSucced ? "succeed" : "error");
            //data.Add("data", new JArray());
            data.Add("errmsg", errmsg);
            //data.Add("stacktrace", ex.StackTrace);
            return data;           
        }
    }
}