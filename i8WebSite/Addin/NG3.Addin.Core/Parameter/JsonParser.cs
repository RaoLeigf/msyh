using Enterprise3.WebApi.SDK.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NG3.Addin.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core.Parameter
{
    public class JsonParser
    {
        public static readonly string TABLE_ROOT = "table";
        public static readonly string FORM_ROOT = "form";


        /// <summary>
        /// 取
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string GetRootType(object json)
        {
            JObject obj = json as JObject;

            if (obj == null) throw new AddinException("无法解析JSon字符串");
            string root = TABLE_ROOT;//grid传上来的数据
            JToken token = obj[TABLE_ROOT];
            if (token == null)
            {
                root = FORM_ROOT; //form传上来的数据
            }

            return root;

        }

        /// <summary>
        /// 判断是否是空的JSON
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static bool IsEmptyData(object json)
        {
            JObject obj = json as JObject;

            if (obj == null) throw new AddinException("无法解析JSon字符串");

            string root = "table";//grid传上来的数据
            JToken token = obj["table"];
            if (token == null)
            {
                root = "form"; //form传上来的数据
            }

            JToken jt = obj[root]["modifiedRow"];
            JToken token3 = obj[root]["newRow"];
            JToken token4 = obj[root]["deletedRow"];
            JToken token5 = obj[root]["unChangedRow"];

            if (jt == null && token3 == null && token4 == null && token5 == null) return true;

            return false;

        }

        public static string[] GetValues(object json,string rowstype,string field)
        {
            JObject obj = json as JObject;

            if (obj == null) throw new AddinException("无法解析JSon字符串");

            string root = "table";//grid传上来的数据
            JToken token = obj["table"];
            if (token == null)
            {
                root = "form"; //form传上来的数据
            }

            string key = obj[root]["key"].ToString();
            JToken jt = obj[root]["modifiedRow"];
            JToken token3 = obj[root]["newRow"];
            JToken token4 = obj[root]["deletedRow"];
            JToken token5 = obj[root]["unChangedRow"];

            string[] values;

            //删除数据源
            if(EnumUIDataSourceType.Del.ToString().Equals(rowstype,StringComparison.OrdinalIgnoreCase))
            {
                    values = GetDeletedValues(token4,field, key).ToArray<string>();
                    return values;

            }
            //新增数据源
            if (EnumUIDataSourceType.New.ToString().Equals(rowstype, StringComparison.OrdinalIgnoreCase))
            {
                values = GetNewRowValues(token3, field,key,root).ToArray<string>();
                
                return values;
            }
            //修改数据源
            if (EnumUIDataSourceType.Mod.ToString().Equals(rowstype, StringComparison.OrdinalIgnoreCase))
            {

                values = GetModifiedValues(token3, field,key).ToArray<string>();
                return values;

            }
            //所有数据源
            if (EnumUIDataSourceType.All.ToString().Equals(rowstype, StringComparison.OrdinalIgnoreCase))
            {

                List<string> datas = new List<string>();

                var dels = GetDeletedValues(token4, field,key);
                if(dels!=null)
                {
                    datas.AddRange(dels);
                }                
                var news = GetNewRowValues(token3, field,key,root);
                if(news!=null)
                {
                    datas.AddRange(news);
                }
                var mods = GetModifiedValues(jt, field,key);
                if(mods!=null)
                {
                    datas.AddRange(mods);
                }
                //未改变的值
                var unchanged = GetunChangedValues(token5, field, key);
                if(unchanged!=null)
                {
                    datas.AddRange(unchanged);
                }
                                
                return datas.ToArray<string>();
                
               
            }

            throw new AddinException("无法解析出指定的["+ rowstype + "]的数据");


        }


        /// <summary>
        /// 用主键值来填充phid列
        /// </summary>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static string FillJsonStringWithPhid(string jsonStr)
        {
            string json = jsonStr;
            if (string.IsNullOrEmpty(jsonStr)) return string.Empty;

            JObject xmlobj = JsonConvert.DeserializeObject(jsonStr) as JObject;
            if (xmlobj == null) return json;

            JToken jt = xmlobj["tablename"];
            if (jt == null) return json;
            //更新的表名
            string tname = jt.ToString();
            //不处理直接返回
            if (string.IsNullOrEmpty(tname)) return json;

            JToken data = xmlobj["data"];
            if (data == null) return json;

            string root = "table";//grid传上来的数据
            JToken token = data["table"];
            if (token == null)
            {
                root = "form"; //form传上来的数据
            }

            JToken newRows = data[root]["newRow"];
            if (newRows == null) goto rtn;

            BillNoHelper billno = new BillNoHelper();

            if (newRows is JArray)
            {
               var array = newRows as JArray;

                //取得PHID，根据步长取得
               var pks = billno.GetBillId(tname, "phid",array.Count);

                for (int i = 0; i < array.Count; i++)
                {

                    JToken row = (array[i]["row"] == null) ? array[i] : array[i]["row"];
                    row["phid"] = pks.BillIdList[i];
                }
            }
            else
            {                
                JToken row = (newRows["row"] == null) ? newRows : newRows["row"];
                row["phid"] = billno.GetBillId(tname, "phid"); //主键
            }

            rtn:
            string newJson = JsonConvert.SerializeObject(data);
            //返回已经填充好的json字符串
            return newJson; 
        }

        /// <summary>
        /// 取删除行数据
        /// </summary>
        /// <param name="token"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        private static IList<string> GetDeletedValues(JToken token,string field,string key)
        {
            if (token == null) return null;
            //删除的时候，行中保留的是key值，而不是具体的列名
            if (key.Equals(field, StringComparison.OrdinalIgnoreCase))
            {
                return GetJSonValues(token, field).ToArray<string>();
            }
            throw new AddinException("Del类型的数据,只支持主键数据");
        }

        /// <summary>
        /// 取修改行所有数据
        /// </summary>
        /// <param name="token"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        private static  IList<string> GetModifiedValues(JToken token, string field,string key)
        {
            if (token == null) return null;
            //删除的时候，行中保留的是key值，而不是具体的列名
            if (key.Equals(field, StringComparison.OrdinalIgnoreCase))
            {

                var data = GetJSonValues(token, "key");
                return data.ToArray<string>();
                
            }
            else
            {
                return GetJSonValues(token, field).ToArray<string>();
                
            }

            
        }
        /// <summary>
        /// 取所有未改变
        /// </summary>
        /// <param name="token"></param>
        /// <param name="field"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static IList<string> GetunChangedValues(JToken token,string field,string key)
        {
            if (token == null) return null;
            //删除的时候，行中保留的是key值，而不是具体的列名
            if (key.Equals(field, StringComparison.OrdinalIgnoreCase))
            {

                var data = GetJSonValues(token, "key");
                return data.ToArray<string>();

            }
            else
            {
                return GetJSonValues(token, field).ToArray<string>();

            }
        }

        /// <summary>
        /// 取新增行所有数据
        /// </summary>
        /// <param name="token"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        private static IList<string> GetNewRowValues(JToken obj,string field,string key,string root)
        {
            if (obj == null) return null;

            if (TABLE_ROOT == root.ToLower() && field.Equals(key,StringComparison.OrdinalIgnoreCase))
            {
                throw new AddinException("无法获取明细行的[" + key+ "]值.");
            }

            var data =  GetJSonValues(obj, field);
            if(FORM_ROOT ==root.ToLower() && field.Equals(key, StringComparison.OrdinalIgnoreCase))
            {
                if(data.Count<1) throw new AddinException("无法获取表头[" + key + "]值.");
                string phid = data[0];
                                
                if (string.IsNullOrEmpty(phid)|| phid=="0")
                {
                    //尝试获取线程上下的注入方法的结果集
                    var returnobject = CallContext.GetData("returnobject");
                    if(returnobject == null) throw new AddinException("无法获取表头[" + key + "]值.");
                    phid = Convert.ToString(returnobject);
                    if (string.IsNullOrEmpty(phid)) throw new AddinException("无法获取表头[" + key + "]值.");

                    data[0] = phid;

                }


            }          
            return data.ToArray<string>();
            
        }

        private static IList<string> GetJSonValues(JToken obj, string field)
        {
            if (obj == null) return null;

            List<string> list = new List<string>();
            if (obj is JArray)
            {
                JArray array = obj as JArray;

                for (int i = 0; i < array.Count; i++)
                {

                    JToken token = (array[i]["row"] == null) ? array[i] : array[i]["row"];
                    foreach (JProperty property in ((JObject)token).Properties())
                    {
                        //属性
                        if (property.Name.Equals(field, StringComparison.OrdinalIgnoreCase))
                        {
                            list.Add(property.Value.ToString());
                        }
                    }
                }

            }
            else
            {
                JObject obj3 = obj as JObject;
                JToken token2 = (obj3["row"] == null) ? obj3 : obj3["row"];

                foreach (JProperty property2 in ((JObject)token2).Properties())
                {
                    //属性
                    if (property2.Name.Equals(field, StringComparison.OrdinalIgnoreCase))
                    {
                        list.Add(property2.Value.ToString());
                    }


                }

            }
            return list.ToArray<string>();
        }

    }
}
