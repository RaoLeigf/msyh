using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SUP3.Log.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUP3.Log.Controller
{
    /// <summary>
    /// 
    /// </summary>
    public static class JsonToModel
    {
        /// <summary>
        /// 获取Models
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static IList<LogCfgModel> GetModifiedLogCfgModels(string json)
        {
            if (string.IsNullOrEmpty(json)) return null;
           
            JObject obj = JsonConvert.DeserializeObject(json) as JObject;
            if (obj == null) return null;



            IList<LogCfgModel> datas = new List<LogCfgModel>();

            string root = "table";
            JToken mjt = obj[root]["modifiedRow"];
            if (mjt == null) return null;
            //数组
            if(mjt is JArray)
            {
                JArray arr = mjt as JArray;

                for (int i = 0; i < arr.Count; i++)
                {
                    JToken token = arr[i];
                    token = token["row"];
                    JObject row = token as JObject; ;
                   
                    LogCfgModel model = new LogCfgModel();
                   
                    model.Phid = Convert.ToInt64(row.Property("key").Value.ToString()); //主键
                    model.LogLevel = Convert.ToInt32(row.Property("LogLevel").Value.ToString()); //日志级别;
                    model.BizModule = row.Property("BizModule").Value.ToString(); //业务模块
                                                            
                    if(row.Property("LogId").Value!=null)
                    {
                        model.LogId = row.Property("LogId").Value.ToString();
                    }
                    else
                    {
                        model.LogId = null;
                    }
                    
                    model.PersistentState = SUP.Common.Base.PersistentState.Modified;
                    datas.Add(model);
                }
            }

            return datas;
        }

        
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static IList<LogOtherCfgModel> GetModifiedLogOtherCfgModel(string json)
        {
            if (string.IsNullOrEmpty(json)) return null;

            JObject obj = JsonConvert.DeserializeObject(json) as JObject;
            if (obj == null) return null;



            IList<LogOtherCfgModel> datas = new List<LogOtherCfgModel>();

            string root = "table";
            JToken mjt = obj[root]["modifiedRow"];
            if (mjt == null) return null;
            //数组
            if (mjt is JArray)
            {
                JArray arr = mjt as JArray;

                for (int i = 0; i < arr.Count; i++)
                {
                    JToken token = arr[i];
                    token = token["row"];
                    JObject row = token as JObject; ;

                    LogOtherCfgModel model = new LogOtherCfgModel();

                    model.Phid = Convert.ToInt64(row.Property("key").Value.ToString()); //主键
                    model.CfgKey = row.Property("CfgKey").Value.ToString(); //配置KEY;
                    model.CfgName = row.Property("CfgName").Value.ToString(); //配置名称

                    if (row.Property("CfgValue").Value != null)
                    {
                        model.CfgValue = row.Property("CfgValue").Value.ToString();
                    }
                    else
                    {
                        model.CfgValue = null;
                    }

                    model.PersistentState = SUP.Common.Base.PersistentState.Modified;
                    datas.Add(model);
                }
            }

            return datas;
        }



    }
}
