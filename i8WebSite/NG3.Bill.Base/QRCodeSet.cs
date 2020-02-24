using SUP.Frame.DataEntity;
using SUP.Frame.Rule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SUP.Common.Base;
namespace NG3.Bill.Base
{
    public class QRCodeSet
    {
        private QRCodeSetRule rule = null;

        public QRCodeSet()
        {
            rule = new QRCodeSetRule();
        }
        public string GetDataByForm(string code, string formJson)
        {
            string phid = rule.getPhidByCode(code);
            JArray arr = JsonConvert.DeserializeObject(formJson) as JArray;
            DataTable masterDt = rule.GetMaster(phid);
            DataTable detailDt = rule.GetDetailField(phid);

            JObject result = new JObject();
            result.Add("code", code);

            JObject resultData = new JObject();
            for (int i = 0; i < arr.Count; i++)
            {
                string tablename = arr[i].ToObject<JObject>()["bindtable"].ToString();
                DataRow[] drs = detailDt.Select("tablename ='" + tablename + "'");
                if (drs.Length < 1)
                {
                    continue;
                }
                JToken data = arr[i].ToObject<JObject>()["data"];
                JObject dataObj = data as JObject;
                if (dataObj != null)
                {
                    foreach (var item in dataObj)
                    {
                        DataRow[] dataDrs = detailDt.Select("tablename ='" + tablename + "' and property ='" + item.Key + "'");
                        if (dataDrs.Length > 0)
                        {
                            resultData.Add(dataDrs[0]["name"].ToString(), item.Value);
                        }
                    }
                }
            }
            result.Add("data", resultData);
            string resultString = JsonConvert.SerializeObject(result);
            return resultString;
        }
    }
}
