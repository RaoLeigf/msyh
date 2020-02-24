using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using Newtonsoft.Json.Linq;
using NG3.Data.Service;
using Newtonsoft.Json;
using NG3.Base;
using NG3;
using System.Reflection;
using NG3.Log.Log4Net;

namespace SUP.Common.Base
{
    public class NGJsonConverter
    {

        private bool needmapping = false;//是否需要映射
        public bool NeedMapping
        {
            get { return needmapping; }
            set { needmapping = value; }
        }
        private Dictionary<string, string> fieldmapping = null;//字段映射信息
        
        public Dictionary<string, string> FieldMapping
        {
            get { return fieldmapping; }
            set { fieldmapping = value; }
        }


        #region 日志相关
        private ILogger _logger = null;
        internal ILogger Logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = Log4NetLoggerFactory.Instance.CreateLogger(this.GetType(), LogType.logoperation);
                }
                return _logger;
            }
        }
        #endregion

        #region Json to DataTable


        public DataTable ToDataTable(string json, string selectSql)
        {
            if (string.IsNullOrEmpty(json))
            {
                //throw new ArgumentException("json is empty");
                return new DataTable();//返回不存在数据行的表，防止业务报错
            }

            JObject jo = JsonConvert.DeserializeObject(json) as JObject;

            return this.ToDataTable(jo, selectSql);
        }

        /// <summary>
        /// 把json转化为datatable
        /// </summary>
        /// <param name="jo">json数据</param>
        /// <param name="selectSql">取表数据的sql语句</param>
        /// <returns></returns>
        public  DataTable ToDataTable(JObject jo, string selectSql)
        {
            if (jo == null )
            {
                throw new ArgumentException("no json data");
            }
            if (string.IsNullOrEmpty(selectSql))
            {
                throw new ArgumentException("selectSql is empty");
            }

            string root = "table";//默认值
            JToken rootobj = jo["table"];
            if (rootobj == null)
            {
                root = "form";
            }

            string keycolumn = jo[root].Attr("key");
            JToken modifyRow = jo[root]["modifiedRow"];
            JToken newRow = jo[root]["newRow"];
            JToken deleteRow = jo[root]["deletedRow"];
            JToken unchangedRow = jo[root]["unChangedRow"];

            //key去掉表名
            string[] keyarr = keycolumn.Split('.');
            if (keyarr.Length > 1)
            {
                keycolumn = keyarr[1];
            }

            string where = CommonUtil.GetWhere(jo, root);
             string sql = string.Empty;
            if (!string.IsNullOrEmpty(where))
            {
                if (selectSql.ToLower().IndexOf("where") > 0)
                {
                    sql = selectSql + " and " + where;
                }
                else {
                    sql = selectSql + " where " + where;
                }               
            }
            else
            {
                if (selectSql.ToLower().IndexOf("where") > 0)
                {
                    sql = selectSql + " and 1=0";
                }
                else
                {
                    sql = selectSql + " where 1=0";
                }                   
            }

            DataTable dt = CommonUtil.GetDataTable(sql);

            if (unchangedRow != null)
            {
                this.DealModifyRow(unchangedRow, dt,keycolumn,true);
            }
            if (newRow != null)
            {
                this.DealNewRow(newRow, dt);
            }

            if (modifyRow != null)
            {
                this.DealModifyRow(modifyRow, dt, keycolumn,false);
            }

            if (deleteRow != null)
            {
                this.DealDeleteRow(deleteRow, dt, keycolumn);
            }

            return dt;
        }

        //新增行
        private  void DealNewRow(JToken jt, DataTable dt)
        {
            if (jt is JArray)
            {
                JArray arr = jt as JArray;

                foreach (JObject item in arr)
                {
                    DataRow dr = dt.NewRow();

                    JToken row = item["row"];//AF定位到行
                    JObject jo = row as JObject;

                    JObject tempjo = null;
                    if (jo != null)//AF
                    {
                        tempjo = jo;
                    }
                    else
                    {
                        tempjo = item;
                    }

                    foreach (JProperty property in tempjo.Properties())
                    {
                        string field = property.Name;
                        if (this.needmapping)
                        {
                            if (this.FieldMapping.ContainsKey(property.Name))
                            {
                                field = this.FieldMapping[property.Name];
                            }
                        }
                        if (dt.Columns.Contains(field))
                        {
                            SetNewRow(dt, dr, property);
                        }
                    }

                    dt.Rows.Add(dr);
                }
            }
            else
            {
                DataRow dr = dt.NewRow();
                JToken row = jt["row"];//定位到行
                JObject jo = row as JObject;

                JObject tempjo = null;
                if (jo != null)//AF
                {
                    tempjo = jo;
                }
                else
                {
                    tempjo = jt as JObject;
                }

               
                foreach (JProperty property in tempjo.Properties())
                {
                    string field = property.Name;
                    if (this.needmapping)
                    {
                        if (this.FieldMapping.ContainsKey(property.Name))
                        {
                            field = this.FieldMapping[property.Name];
                        }
                    }
                    if (dt.Columns.Contains(field))
                    {
                        SetNewRow(dt, dr, property);
                    }
                }
                dt.Rows.Add(dr);
            }

        }

        private  void SetNewRow(DataTable dt, DataRow dr, JProperty property)
        {
            string field = property.Name;
            if (this.needmapping)
            {
                if (this.FieldMapping.ContainsKey(property.Name))
                {
                    field = this.FieldMapping[property.Name];
                }
            }

            Type t = dt.Columns[field].DataType;

            string value = property.Value.ToString();

            try
            {


                if (!string.IsNullOrEmpty(value))
                {
                    switch (t.Name)
                    {
                        case "Int32":
                        case "Int16":
                            if (value.ToLower() == "true")//checkcolumn在新增时可能会传true或者false
                            { value = "1"; }
                            else if (value.ToLower() == "false")
                            { value = "0"; }
                            dr[field] = Convert.ToInt32(value);
                            break;
                        case "Int64":
                            dr[field] = Convert.ToInt64(value);
                            break;
                        case "Decimal":
                            dr[field] = Convert.ToDecimal(value);
                            break;
                        case "Double":
                        case "float":
                            dr[field] = Convert.ToDouble(value);
                            break;
                        case "DateTime":
                            if (value.IndexOf("UTC") > 0 || value.IndexOf("GMT") > 0)//utc时间格式
                            {
                                value = CommonUtil.UTCtimeToDateTime(value);
                            }
                            dr[field] = Convert.ToDateTime(value);
                            break;
                        default:
                            dr[field] = value;
                            break;
                    }
                }
                else
                {
                    //if (t.Name == "String")
                    //{
                    //    dr[property.Name] = string.Empty;
                    //}
                    dr[field] = DBNull.Value;

                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("SetPropertyValue数据转换失败，字段名:[{0}],数据值为：{1},", field, value);
                Logger.Error(msg + "详细错误: " + ex.StackTrace);
                throw new Exception(msg, ex);                
            }
        }

        //修改行
        private void DealModifyRow(JToken jt, DataTable dt, string key, bool acceptChanges)
        {
            if (jt is JArray)
            {
                JArray arr = jt as JArray;
                foreach (JObject item in arr)
                {
                    JToken row = item["row"];//定位到行
                    JObject jo = row as JObject;

                    string keyvalue = string.Empty;
                    JObject tempjo = null;
                    if (jo != null)
                    {
                        tempjo = jo;
                        keyvalue = row.Attr("key");
                    }
                    else
                    {
                        tempjo = item;
                        keyvalue = item.Attr("key");
                    }
                

                    string where = GetFilter(key, keyvalue);
                    DataRow[] dr = dt.Select(where);

                    if (dr.Length > 0)
                    {
                        DataRow drow = dr[0];
                        foreach (JProperty property in tempjo.Properties())
                        {
                            string field = property.Name;
                            if (this.needmapping)
                            {
                                if (this.FieldMapping.ContainsKey(property.Name))
                                {
                                    field = this.FieldMapping[property.Name];
                                }
                            }
                            if (dt.Columns.Contains(field))
                            {
                                SetUpdateRow(dt, drow, property);
                            }
                        }
                        if (acceptChanges)
                        {
                            drow.AcceptChanges();
                        }
                    }
                }
            }
            else
            {
                JToken row = jt["row"];//定位到
                JObject jo = row as JObject;

                string keyvalue = string.Empty;
                JObject thejo = null;
                if (jo != null)
                {
                    thejo = jo;
                    keyvalue = row.Attr("key");
                }
                else
                {
                    thejo = jt as JObject;
                    keyvalue = jt.Attr("key");
                }

                string where = GetFilter(key, keyvalue);

                DataRow[] dr = dt.Select(where);

                if (dr.Length > 0)
                {
                    DataRow drow = dr[0];
                    foreach (JProperty property in thejo.Properties())
                    {
                        string field = property.Name;
                        if (this.needmapping)
                        {
                            if (this.FieldMapping.ContainsKey(property.Name))
                            {
                                field = this.FieldMapping[property.Name];
                            }
                        }
                        if (dt.Columns.Contains(field))
                        {
                            SetUpdateRow(dt, drow, property);
                        }
                    }
                    if (acceptChanges)
                    {
                        drow.AcceptChanges();
                    }
                }
            }
        }

        private  void SetUpdateRow(DataTable dt, DataRow dr, JProperty property)
        {
            string value = string.Empty;

            string field = property.Name;
            if (this.needmapping)
            {
                if (this.FieldMapping.ContainsKey(property.Name))
                {
                    field = this.FieldMapping[property.Name];
                }
            }
            Type t = dt.Columns[field].DataType;

            if (property.Value is JValue)//仅包含现值
            {
                value = property.Value.ToString();
            }
            if (property.Value is JObject)//包含原始值,一般会进
            {
                JObject jotext = property.Value as JObject;

                if (jotext.Property("text") != null)
                {
                    value = jotext.Property("text").Value.ToString();
                }
                else 
                {                   
                    value = "";
                }
            }

            try
            {
                
                if (!string.IsNullOrEmpty(value))
                {
                    switch (t.Name)
                    {
                        case "Int32":
                        case "Int16":
                            if (value.ToLower() == "true")//checkcolumn在新增时可能会传true或者false
                            { value = "1"; }
                            else if (value.ToLower() == "false")
                            { value = "0"; }
                            dr[field] = Convert.ToInt32(value);
                            break;
                        //case "Int32": dr[field] = Convert.ToInt32(value);
                        //    break;
                        case "Int64":
                            dr[field] = Convert.ToInt64(value);
                            break;
                        case "Decimal":
                            dr[field] = Convert.ToDecimal(value);
                            break;
                        case "Double":
                        case "float":
                            dr[field] = Convert.ToDouble(value);
                            break;
                        case "DateTime":
                            if (value.IndexOf("UTC") > 0 || value.IndexOf("GMT") > 0)//utc时间格式
                            {
                                value = CommonUtil.UTCtimeToDateTime(value);
                            }
                            dr[field] = Convert.ToDateTime(value);
                            break;
                        default:
                            dr[field] = value;
                            break;
                    }
                }
                else
                {
                    //if (t.Name == "String")
                    //{
                    //    dr[property.Name] = string.Empty;//字符修改为空
                    //}   

                    dr[field] = DBNull.Value;
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("SetPropertyValue数据转换失败，字段名:[{0}],数据值为：{1},", field, value);
                Logger.Error(msg + "详细错误: " + ex.StackTrace);
                throw new Exception(msg, ex);
            }

        }

        private static string GetFilter(string key, string keyvalue)
        {
            string where = string.Empty;

            string[] keyarr = key.Split(',');
            string[] valuearr = keyvalue.Split(',');
            if (keyarr.Length > 1)
            {
                for (int i = 0; i < keyarr.Length; i++)
                {
                    where += keyarr[i] + "='" + valuearr[i] + "'";
                    if (i < keyarr.Length - 1)
                    {
                        where += " and ";
                    }
                }
            }
            else
            {
                where = key + "='" + keyvalue + "'";
            }
            return where;
        }


        //删除行
        private  void DealDeleteRow(JToken jt, DataTable dt, string key)
        {
            if (jt is JArray)
            {
                JArray arr = jt as JArray;

                foreach (JObject item in arr)
                {
                    JToken j = item["row"];//定位到主键列                    

                    string keyvalue = string.Empty;
                    if (j != null)
                    {
                        JToken row = item["row"]["key"];//定位到主键列                    
                        JValue jv = row as JValue;
                        keyvalue = jv.Value.ToString();
                    }
                    else
                    {
                        keyvalue = item.Attr<String>("key");
                    }

                    string where = GetFilter(key, keyvalue);

                    DataRow[] dr = dt.Select(where);

                    if (dr.Length > 0)
                    {
                        //dt.Rows.Remove(dr[0]);    
                        dr[0].Delete();
                    }

                }
            }
            else
            {
                //JToken row = jt["row"]["key"];//定位到主键列                
                //JValue jv = row as JValue;

                JToken j = jt["row"];
                string keyvalue = string.Empty;
                if (j != null)
                {
                    keyvalue = jt["row"].Attr<string>("key");
                }
                else
                {
                    keyvalue = jt.Attr("key");
                }

                string where = GetFilter(key, keyvalue);

                DataRow[] dr = dt.Select(where);


                if (dr.Length > 0)
                {
                    //dt.Rows.Remove(dr[0]);
                    dr[0].Delete();
                }
            }
        }



        #endregion

        #region DataTable To Json

        public string ToJson(DataTable dt, int totalRecord,bool format = true)
        {

            List<JObject> listdr = new List<JObject>();

            foreach (DataRow dr in dt.Rows)
            {
                listdr.Add(dr.ToJObject());
            }

            string listjson = JsonConvert.SerializeObject(listdr);

            if (format)
            {
                StringBuilder strb = new StringBuilder();

                strb.Append("{totalRows:");
                strb.Append(totalRecord);
                strb.Append(",");
                strb.Append("Record:");
                strb.Append(listjson);
                strb.Append("}");

                return strb.ToString();
            }
            else
            {
               return listjson;
            }
        }

        public string ToJsonData(DataTable dt, int totalRecord, bool format = true)
        {            
            if (format)
            {
                //List<JObject> listdr = new List<JObject>();
                //foreach (DataRow dr in dt.Rows)
                //{
                //    listdr.Add(dr.ToJObject());
                //}

                //JSON.parse转json报错

                //StringBuilder strb = new StringBuilder();
                //strb.Append("{total:");
                //strb.Append(totalRecord);
                //strb.Append(",");
                //strb.Append("rows:");
                //strb.Append(listjson);
                //strb.Append("}");
                //return strb.ToString();

                JArray ja = new JArray();
                foreach (DataRow dr in dt.Rows)
                {                    
                    ja.Add(dr.ToJObject());
                }

                JObject jo = new JObject();
                jo.Add("total", totalRecord);
                jo.Add("rows", ja);

                return jo.ToString();
            }
            else
            {
                JArray ja = new JArray();             
                foreach (DataRow dr in dt.Rows)
                {
                    ja.Add(dr.ToJObject());
                }

                
                return ja.ToString();
            }
        }

        #endregion

    }
}
