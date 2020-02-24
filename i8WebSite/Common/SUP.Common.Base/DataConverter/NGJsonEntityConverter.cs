using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections;
using NHibernate.Cfg;
using NHibernate.Mapping;
using System.Data;
using NG3.Base;
using Newtonsoft.Json.Converters;
using NG3.Log.Log4Net;

namespace SUP.Common.Base
{

    /// <summary>
    /// Json、实体转换类
    /// </summary>
    public class NGJsonEntityConverter
    {


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

        public EntityInfo<T> JsonToEntity<T>(string json)
        {
            JObject obj = JsonConvert.DeserializeObject(json) as JObject;
            return JsonToEntity<T>(obj);
        }

        public EntityInfo<T> JsonToEntity<T>(JObject obj)
        {

            string root = "table";//默认值
            JToken rootobj = obj["table"];
            if (rootobj == null)
            {
                root = "form";
            }

            string keycolumn = obj[root]["key"].ToString();
            JToken modifyRow = obj[root]["modifiedRow"];
            JToken newRow = obj[root]["newRow"];
            JToken deleteRow = obj[root]["deletedRow"];
            JToken unchangedRow = obj[root]["unChangedRow"];


            EntityInfo<T> entityinfo = new EntityInfo<T>();
            if (modifyRow != null && modifyRow.First != null)
            {
                //key去掉表名
                string[] keyarr = keycolumn.Split('.');
                if (keyarr.Length > 1)
                {
                    keycolumn = keyarr[1];
                }

                string table = new MappingInfo().GetEntityMapTableName(typeof(T));

                if (string.IsNullOrWhiteSpace(table))
                {
                    DealModifyRow<T>(entityinfo, null, modifyRow, keycolumn);
                }
                else
                {
                    string selectSql = "select * from " + table;
                    string where = CommonUtil.GetModifyWhere(obj, root);
                    string sql = string.Empty;
                    if (!string.IsNullOrEmpty(where))
                    {
                        sql = selectSql + " where " + where;
                    }
                    else
                    {
                        sql = selectSql + " where 1=0";
                    }
                    DataTable dt = CommonUtil.GetDataTable(sql);//获取修改行的表信息

                    DealModifyRow<T>(entityinfo, dt, modifyRow, keycolumn);
                }
            }
            if (newRow != null)
            {
                DealNewRow<T>(entityinfo, newRow, keycolumn);
            }
            if (deleteRow != null)
            {
                DealDeleteRow<T>(entityinfo, deleteRow, keycolumn);
            }
            if (unchangedRow != null)
            {
                DealUnChangeRow<T>(entityinfo, unchangedRow, keycolumn);
            }

            if (entityinfo.NewRow.Count > 0)
            {
                entityinfo.AllRow.AddRange(entityinfo.NewRow);
            }
            if (entityinfo.ModifyRow.Count > 0)
            {
                entityinfo.AllRow.AddRange(entityinfo.ModifyRow);
            }
            if (entityinfo.DeleteRow.Count > 0)
            {
                entityinfo.AllRow.AddRange(entityinfo.DeleteRow);
            }
            if (entityinfo.UnChangeRow.Count > 0)
            {
                entityinfo.AllRow.AddRange(entityinfo.UnChangeRow);
            }

            return entityinfo;
        }

        public string EntityListToJson<T>(IList<T> entityList, long totalRecord,bool isDateTime=false)
        {
            JArray ja = new JArray();         
            if (entityList != null)
            {
                foreach (var item in entityList)
                {
                    JObject jo = EnToJson(item);
                    ja.Add(jo);                
                }
            }           

            //老的方法所有字段都toString，客户端控件没法正常显示
            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();//解决枚举、数字、bool类型的json转换
            timeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            string listjson = JsonConvert.SerializeObject(ja,timeConverter);

            StringBuilder strb = new StringBuilder();

            strb.Append("{totalRows:");
            strb.Append(totalRecord);
            strb.Append(",");
            strb.Append("Record : ");
            strb.Append(listjson);
            strb.Append("}");

            return strb.ToString();
        }

        public string EntityListToJson(IList entityList, long totalRecord, bool isDateTime = false)
        {
            JArray ja = new JArray();
            if (entityList != null)
            {
                foreach (var item in entityList)
                {
                    JObject jo = EnToJson(item);
                    ja.Add(jo);
                }
            }

            //老的方法所有字段都toString，客户端控件没法正常显示
            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();//解决枚举、数字、bool类型的json转换
            timeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            string listjson = JsonConvert.SerializeObject(ja, timeConverter);

            StringBuilder strb = new StringBuilder();

            strb.Append("{totalRows:");
            strb.Append(totalRecord);
            strb.Append(",");
            strb.Append("Record : ");
            strb.Append(listjson);
            strb.Append("}");

            return strb.ToString();
        }

        public string EntityListToJson<T>(IList<T> entityList, string status, string msg)
        {
            JArray ja = new JArray();
            if (entityList != null)
            {
                foreach (var item in entityList)
                {
                    JObject jo = EnToJson(item);
                    ja.Add(jo);
                }
            }

            //老的方法所有字段都toString，客户端控件没法正常显示
            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();//解决枚举、数字、bool类型的json转换
            timeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            string listjson = JsonConvert.SerializeObject(ja, timeConverter);

            StringBuilder strb = new StringBuilder();

            strb.Append("{status:\'");
            strb.Append(status);
            strb.Append("\',");
            strb.Append("data : ");
            strb.Append(listjson);
            strb.Append(",");
            strb.Append("msg : \'");
            strb.Append(msg);
            strb.Append("\'}");

            return strb.ToString();
        }

        public string EntityToJson<T>(T entity)
        {
            JObject jo = EnToJson(entity);

            string json = JsonConvert.SerializeObject(jo);

            return json;
            //return "{Status : \"success\", data:" + json + "}";            
        }

        private  JObject EnToJson(object entity)
        {
            if (entity is JObject) return (entity as JObject);

            JObject jo = new JObject();
            if (entity != null)
            {
                if ((entity is String) && entity.ToString().Length == 0) return jo;

                PropertyInfo[] propertys = entity.GetType().GetProperties();
                foreach (PropertyInfo pinfo in propertys)
                {
                    string key = string.Empty;
                    //if (pinfo.GetCustomAttributes(typeof(DBFieldAttribute), false).Length > 0)
                    //{
                    //    key = ((DBFieldAttribute)pinfo.GetCustomAttributes(typeof(DBFieldAttribute), false)[0]).Field;
                    //}
                    //else
                    //{
                    key = pinfo.Name;//.ToLower();//未设置DBFieldAttribute属性，则取属性名
                                     //}

                    #region 处理自定义字段

                    //处理自定义字段
                    if (key.ToLower() == "extendobjects")
                    {
                        Dictionary<string, object> extObj = pinfo.GetValue(entity, null) as Dictionary<string, object>;
                        if (extObj != null)
                        {
                            foreach (KeyValuePair<string, object> item in extObj)
                            {
                                JToken jt = null;
                                if (item.Value != null && item.Value != DBNull.Value)
                                {
                                    Type type = item.Value.GetType();
                                    if (type == typeof(DateTime))
                                    {
                                        string s = item.Value.ToString();
                                        if (s.IndexOf(" 0:00:00") > 0 || s.IndexOf("00:00:00") > 0)
                                        {
                                            jt = ((DateTime)item.Value).ToString("yyyy-MM-dd");
                                        }
                                        else
                                        {
                                            jt = ((DateTime)item.Value).ToString("yyyy-MM-dd HH:mm:ss");
                                        }
                                        jo.Add(item.Key, jt);
                                    }
                                    else if (type == typeof(bool))
                                    {
                                        jt = (bool)item.Value;
                                        jo.Add(item.Key, jt);
                                    }
                                    else if (type.IsEnum || type == typeof(Int16) || type == typeof(Int32) || type == typeof(Byte))
                                    {
                                        jt = Convert.ToInt32(item.Value);
                                        jo.Add(item.Key, jt);
                                    }
                                    else if (type == typeof(float) || type == typeof(Double))
                                    {
                                        jt = Convert.ToDouble(item.Value);
                                        jo.Add(item.Key, jt);
                                    }
                                    else
                                    {
                                        jo.Add(item.Key, item.Value.ToString());
                                    }

                                }
                            }
                        }
                    }
                    #endregion
                    else
                    {

                        JToken value;
                        if (pinfo.PropertyType.FullName.IndexOf("DateTime") > 0)
                        {
                            string s = (pinfo.GetValue(entity, null) == null) ? string.Empty : pinfo.GetValue(entity, null).ToString();

                            if (string.IsNullOrWhiteSpace(s))
                            {
                                jo.Add(key, s);
                                continue;
                            }

                            if (s.IndexOf(" 0:00:00") > 0 || s.IndexOf("00:00:00") > 0)
                            {
                                value = ((DateTime)pinfo.GetValue(entity, null)).ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                value = ((DateTime)pinfo.GetValue(entity, null)).ToString("yyyy-MM-dd HH:mm:ss");
                            }

                            jo.Add(key, value);
                        }
                        else if (pinfo.PropertyType.IsEnum)
                        {
                            value = Convert.ToInt32(pinfo.GetValue(entity, null));
                            jo.Add(key, value);
                        }
                        else if (pinfo.PropertyType == typeof(bool))
                        {
                            jo.Add(key, (bool)pinfo.GetValue(entity, null));
                        }
                        else if (pinfo.PropertyType == typeof(Int16) || pinfo.PropertyType == typeof(Int32)
                                   || pinfo.PropertyType == typeof(Byte))
                        {
                            value = Convert.ToInt64(pinfo.GetValue(entity, null));
                            jo.Add(key, value);
                        }
                        else if (pinfo.PropertyType == typeof(Int64) || pinfo.PropertyType == typeof(long))
                        {
                            //value = Convert.ToInt64(pinfo.GetValue(entity, null));
                            object obj = pinfo.GetValue(entity, null);
                            jo.Add(key, obj.ToString());//前端不支持int64，转成string
                        }
                        else if (pinfo.PropertyType == typeof(Decimal))
                        {
                            value = Convert.ToDecimal(pinfo.GetValue(entity, null));
                            jo.Add(key, value);
                        }
                        else if (pinfo.PropertyType == typeof(float) || pinfo.PropertyType == typeof(Double))
                        {
                            value = Convert.ToDouble(pinfo.GetValue(entity, null));
                            jo.Add(key, value);
                        }
                        else
                        {
                            try
                            {
                                value = (pinfo.GetValue(entity, null) == null) ? string.Empty : pinfo.GetValue(entity, null).ToString();
                                jo.Add(key, value);
                            }
                            catch (Exception ex)
                            {
                                Logger.Error("pinfo.GetValue" + ex.StackTrace);
                            }
                          
                        }
                    }
                    
                }
            }

            return jo;
        }

        public JObject ResponseResultToJson(object result, bool toLower)
        {
            JObject jo = new JObject();
            if (result != null && !result.Equals(""))
            {
                PropertyInfo[] propertys = result.GetType().GetProperties();
                foreach (PropertyInfo pinfo in propertys)
                {
                    string key = string.Empty;
                    //if (pinfo.GetCustomAttributes(typeof(DBFieldAttribute), false).Length > 0)
                    //{
                    //    key = ((DBFieldAttribute)pinfo.GetCustomAttributes(typeof(DBFieldAttribute), false)[0]).Field;
                    //}
                    //else
                    //{
                        //if (toLower == true)
                        //{
                        //    key = pinfo.Name.ToLower();//未设置DBFieldAttribute属性，则取属性名
                        //}
                        //else
                        //{
                        //    key = pinfo.Name;
                        //}
                        key = pinfo.Name;
                    //}

                    if (pinfo.Name == "Data")
                    {
                        //JObject jobject = ResponseResultToJson(pinfo.GetValue(result, null), false);
                        JObject jobject = EnToJson(pinfo.GetValue(result, null));
                        jo.Add(pinfo.Name, jobject);
                    }
                    else
                    {
                        JToken value = null;
                        if (pinfo.PropertyType.FullName.IndexOf("DateTime") > 0)
                        {
                            string s = (pinfo.GetValue(result, null) == null) ? string.Empty : pinfo.GetValue(result, null).ToString();
                            if (s.Length > 0)
                            {
                                if (s.IndexOf(" 0:00:00") > 0 || s.IndexOf("00:00:00") > 0)
                                {
                                    value = ((DateTime)pinfo.GetValue(result, null)).ToString("yyyy-MM-dd");
                                }
                                else
                                {
                                    value = ((DateTime)pinfo.GetValue(result, null)).ToString("yyyy-MM-dd HH:mm:ss");
                                }
                            }
                        }
                        else if (pinfo.PropertyType.IsEnum)
                        {
                            value = Convert.ToInt32(pinfo.GetValue(result, null));
                        }
                        else
                        {
                            value = (pinfo.GetValue(result, null) == null) ? string.Empty : pinfo.GetValue(result, null).ToString();
                        }
                        jo.Add(key, value);
                    }
                }
            }

            return jo;
        }

        private void DealModifyRow<T>(EntityInfo<T> entityinfo, DataTable dt, JToken jt, string key)
        {           

            if (dt == null)//不需要恢复数据
            {
                entityinfo.ModifyRow = SetEntityInfo<T>(jt, key, PersistentState.Modified);
                return;
            }

            Dictionary<string, string> mapdic = new MappingInfo().GetMappingInfo(typeof(T));
            Dictionary<string, T> dic = new Dictionary<string, T>();//暂存列表
            GetOriginalData<T>(dt, dic, mapdic, key);//恢复原始数据

            if (dic.Count == 0) {
                //throw new Exception("处理修改行，恢复原始数据失败，请检查otype设置是否正确！");
                Logger.Error("处理修改行，恢复原始数据失败，请检查otype设置是否正确！");
            }

            if (jt is JArray)//数组
            {
                JArray arr = jt as JArray;
                for (int i = 0; i < arr.Count; i++)
                {
                    JToken jo = arr[i];
                    string keyValue = string.Empty;// jo.Attr(key);//得到主键值
                    if (jo["row"] != null )
                    {                      
                        jo = jo["row"];
                    }     
         
                    keyValue = jo.Attr(key);//可能没有值，得从key获取
                    if (string.IsNullOrWhiteSpace(keyValue))
                    {
                        keyValue = jo.Attr("key");
                    }

                    Dictionary<string, object> extendObj = new Dictionary<string, object>();
                    //其他属性
                    foreach (JProperty jp in ((JObject)jo).Properties())
                    {                       
                        PropertyInfo[] propertys = dic[keyValue].GetType().GetProperties();
                        foreach (PropertyInfo pinfo in propertys)
                        {
                            if (!pinfo.CanWrite) continue;
                            string str = pinfo.Name;
                            if (jp.Name == pinfo.Name)
                            {
                                //SetPropertyValue(dic[keyValue], pinfo, jp.Value.ToString());
                                if (jp.Value is JObject)
                                {
                                    JObject jotext = jp.Value as JObject;
                                    if (jotext.Property("text") != null)//lg带原始值的
                                    {
                                        string value = jotext.Property("text").Value.ToString();
                                        SetPropertyValue(dic[keyValue], pinfo, value);
                                    }

                                }
                                else
                                {
                                    SetPropertyValue(dic[keyValue], pinfo, jp.Value.ToString());
                                }
                            }

                        }

                     
                        if (jp.Name.StartsWith("user_"))//收集自定义字段
                        {
                            extendObj.Add(jp.Name, jp.Value);
                        }                  

                    }

                    PropertyInfo p = dic[keyValue].GetType().GetProperty("ExtendObjects");//设置扩展属性
                    if (p != null)
                    {
                        p.SetValue(dic[keyValue], extendObj, null);
                    }
                }
            }
            else
            {
                JToken jo = jt;
                string keyValue = string.Empty;// jo.Attr(key);//得到主键值
                if (jo["row"] != null )
                {                   
                    jo = jo["row"];
                }
              
                keyValue = jo.Attr(key);//可能没有值，得从key获取
                if (string.IsNullOrWhiteSpace(keyValue))
                {
                    keyValue = jo.Attr("key");
                }

                Dictionary<string, object> extendObj = new Dictionary<string, object>();
                //处理其他属性
                foreach (JProperty jp in ((JObject)jo).Properties())
                {
                    PropertyInfo[] propertys = dic[keyValue].GetType().GetProperties();
                    foreach (PropertyInfo pinfo in propertys)
                    {
                        if (!pinfo.CanWrite) continue;
                        string str = pinfo.Name;
                        if (jp.Name == pinfo.Name)
                        {
                            //SetPropertyValue(dic[keyValue], pinfo, jp.Value.ToString());
                            if (jp.Value is JObject)
                            {
                                JObject jotext = jp.Value as JObject;
                                if (jotext.Property("text") != null)//lg带原始值的
                                {
                                    string value = jotext.Property("text").Value.ToString();
                                    SetPropertyValue(dic[keyValue], pinfo, value);
                                }

                            }
                            else
                            {
                                SetPropertyValue(dic[keyValue], pinfo, jp.Value.ToString());
                            }
                        }
                    }

                    if (jp.Name.StartsWith("user_"))//收集自定义字段
                    {
                        extendObj.Add(jp.Name, jp.Value);
                    }

                }

                PropertyInfo p = dic[keyValue].GetType().GetProperty("ExtendObjects");//设置扩展属性
                if (p != null)
                {
                    p.SetValue(dic[keyValue], extendObj, null);
                }
            }

            List<T> list = new List<T>();
            foreach (KeyValuePair<string, T> item in dic)
            {
                list.Add(item.Value);
            }

            entityinfo.ModifyRow = list;
        }

        private void GetOriginalData<T>(DataTable dt, Dictionary<string, T> dic, Dictionary<string, string> mapDic, string key)
        {
            object[] instances = new object[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                instances[i] = typeof(T).Assembly.CreateInstance(typeof(T).FullName);
                object entity = instances[i];
                PropertyInfo[] propertys = entity.GetType().GetProperties();

                PropertyInfo p = entity.GetType().GetProperty("PersistentState");//实体状态
                if (p != null)
                {
                    p.SetValue(entity, PersistentState.Modified, null);
                }

                foreach (PropertyInfo pinfo in propertys)
                {
                    if (!pinfo.CanWrite) continue;
                    string str = pinfo.Name;

                    if (mapDic.ContainsKey(str))
                    {
                        SetPropertyValue(entity, pinfo, dt.Rows[i][mapDic[str]].ToString());
                    }
                }


                dic.Add(dt.Rows[i][key].ToString(), (T)entity);
            }
        }

        private void DealUnChangeRow<T>(EntityInfo<T> entityinfo, JToken jt, string key)
        {
            entityinfo.UnChangeRow = SetEntityInfo<T>(jt, key, PersistentState.UnChanged);
        }

        private void DealNewRow<T>(EntityInfo<T> entityinfo, JToken jt, string key)
        {
            entityinfo.NewRow = SetEntityInfo<T>(jt, key, PersistentState.Added);
        }

        private void DealDeleteRow<T>(EntityInfo<T> entityinfo, JToken jt, string key)
        {
            entityinfo.DeleteRow = SetDeleteEntityInfo<T>(jt, key);
        }

        private List<T> SetEntityInfo<T>(object obj, string key, PersistentState state, DataTable dt = null)
        {
            List<T> list = new List<T>();
            if (obj is JArray)
            {
                JArray arr = obj as JArray;
                object[] instances = new object[arr.Count];
                for (int i = 0; i < arr.Count; i++)
                {
                    instances[i] = typeof(T).Assembly.CreateInstance(typeof(T).FullName);
                    object entity = instances[i];
                    PropertyInfo[] propertys = entity.GetType().GetProperties();

                    PropertyInfo p = entity.GetType().GetProperty("PersistentState");//实体状态
                    if (p != null)
                    {
                        p.SetValue(entity, state, null);
                    }

                    foreach (PropertyInfo pinfo in propertys)
                    {
                        if (!pinfo.CanWrite) continue;


                        string str = string.Empty;
                        if (pinfo.GetCustomAttributes(typeof(DBFieldAttribute), false).Length > 0)
                        {
                            str = ((DBFieldAttribute)pinfo.GetCustomAttributes(typeof(DBFieldAttribute), false)[0]).Field;
                        }
                        else
                        {
                            str = pinfo.Name;//.ToLower();//未设置DBFieldAttribute属性，则取属性名
                        }

                        if (key.IndexOf(',') > 0)//多主键
                        {
                            string[] allkeys = key.Split(',');
                            int keyindex = -1;
                            bool isKey = false;
                            foreach (string k in allkeys)
                            {
                                keyindex++;
                                if (str.Equals(k, StringComparison.OrdinalIgnoreCase))
                                {
                                    isKey = true;
                                    str = k;//搜到                                    
                                    break;
                                }
                            }

                            if (isKey)//处理主键属性
                            {
                                if (state == PersistentState.Added) continue;//新增不处理主键
                                if (arr[i]["row"]["key"] != null)
                                {
                                    string value = arr[i]["row"]["key"].ToString();
                                    if (!string.IsNullOrEmpty(value))
                                    {
                                        string[] allValues = value.Split(',');
                                        SetPropertyValue(entity, pinfo, allValues[keyindex]);
                                    }
                                }
                            }
                            else//处理非主键属性
                            {

                                if (arr[i]["row"][str] != null)
                                {
                                    string value = arr[i]["row"][str].ToString();//获取json的值                     
                                    SetPropertyValue(entity, pinfo, value);
                                }
                            }
                        }
                        else//单主键
                        {
                            //客户端在主从表在新增的时候做好对应关系，key不为空，新增时也需要还原key的值
                            //if (str.Equals(key, StringComparison.OrdinalIgnoreCase))
                            //{
                            //if (state == PersistentState.Added) continue;//新增不处理主键
                            //str = "key";//主键
                            //}                           

                            if (arr[i]["row"][str] != null)
                            {
                                string value = arr[i]["row"][str].ToString();//获取json的值                     
                                SetPropertyValue(entity, pinfo, value);
                            }
                        }
                    }

                    #region 设置自定义字段
                    
                    JToken jt = (arr[i]["row"] == null) ? arr[i] : arr[i]["row"];
                    Dictionary<string, object> extendObj = new Dictionary<string, object>();
                    foreach (JProperty jp in ((JObject)jt).Properties())
                    {
                        if (jp.Name.StartsWith("user_"))//收集自定义字段
                        {
                            extendObj.Add(jp.Name, jp.Value);
                        }                      
                    }
                    PropertyInfo extP = entity.GetType().GetProperty("ExtendObjects");//扩展属性
                    if (extP != null)
                    {
                        extP.SetValue(entity, extendObj, null);
                    }

                    #endregion

                    list.Add((T)entity);
                }
            }
            else
            {
                JObject jo = obj as JObject;
                object entity = typeof(T).Assembly.CreateInstance(typeof(T).FullName);
                PropertyInfo[] propertys = entity.GetType().GetProperties();//typeof(T).GetType().GetProperties()取出来的不对

                PropertyInfo p = entity.GetType().GetProperty("PersistentState");//实体状态
                if (p != null)
                {
                    p.SetValue(entity, state, null);
                }
                foreach (PropertyInfo pinfo in propertys)
                {
                    string str = string.Empty;
                    if (pinfo.GetCustomAttributes(typeof(DBFieldAttribute), false).Length > 0)
                    {
                        str = ((DBFieldAttribute)(pinfo.GetCustomAttributes(typeof(DBFieldAttribute), false)[0])).Field;
                    }
                    else
                    {
                        str = pinfo.Name;//.ToLower();//未设置DBFieldAttribute属性，则取属性名
                    }
                    if (key.IndexOf(',') > 0)//多主键
                    {
                        string[] allkeys = key.Split(',');
                        int keyindex = -1;
                        bool isKey = false;
                        foreach (string k in allkeys)
                        {
                            keyindex++;
                            if (str.Equals(k, StringComparison.OrdinalIgnoreCase))
                            {
                                isKey = true;
                                str = k;//搜到                               
                                break;
                            }
                        }
                        if (isKey)//处理主键属性
                        {
                            //if (state == PersistentState.Added) continue;//新增不处理主键

                            if (jo["row"] != null)//存在row层
                            {
                                if (jo["row"]["key"] != null)
                                {
                                    string value = jo["row"]["key"].ToString();
                                    if (!string.IsNullOrEmpty(value))
                                    {
                                        string[] allValues = value.Split(',');
                                        SetPropertyValue(entity, pinfo, allValues[keyindex]);
                                    }
                                }
                            }
                            else
                            {
                                if (jo["key"] != null)
                                {
                                    string value = jo["key"].ToString();
                                    if (!string.IsNullOrEmpty(value))
                                    {
                                        string[] allValues = value.Split(',');
                                        SetPropertyValue(entity, pinfo, allValues[keyindex]);
                                    }
                                }
                            }
                        }
                        else//处理非主键属性
                        {
                            if (jo["row"][str] != null)
                            {
                                string value = jo["row"][str].ToString();//获取json的值                        
                                SetPropertyValue(entity, pinfo, value);
                            }
                        }
                    }
                    else//单主键
                    {

                        //客户端在主从表在新增的时候做好对应关系，key不为空，新增时需要还原key的值
                        //if (str.Equals(key, StringComparison.OrdinalIgnoreCase))
                        //{
                        //if (state == PersistentState.Added) continue;//新增不处理主键
                        //str = "key";//主键
                        //}

                        string value = string.Empty;
                        if (jo["row"] != null)
                        {
                            if (jo["row"][str] != null)
                            {
                                value = jo["row"][str].ToString();//获取json的值                
                            }
                        }
                        else
                        {
                            if (jo[str] != null)
                            {
                                value = jo[str].ToString();//获取json的值 
                            }
                        }
                        SetPropertyValue(entity, pinfo, value);
                    }

                }

                #region 设置自定义字段

                JToken jt = (jo["row"] == null)? jo : jo["row"];
                Dictionary<string, object> extendObj = new Dictionary<string, object>();
                foreach (JProperty jp in ((JObject)jt).Properties())
                {
                    if (jp.Name.StartsWith("user_"))//收集自定义字段
                    {
                        extendObj.Add(jp.Name, jp.Value);
                    }
                }
                PropertyInfo extP = entity.GetType().GetProperty("ExtendObjects");//扩展属性
                if (extP != null)
                {
                    extP.SetValue(entity, extendObj, null);
                }

                #endregion

                list.Add((T)entity);
            }
            return list;
        }

        private List<T> SetDeleteEntityInfo<T>(object obj, string key)
        {
            List<T> list = new List<T>();

            if (obj is JArray)
            {
                JArray arr = obj as JArray;
                object[] instances = new object[arr.Count];
                for (int i = 0; i < arr.Count; i++)
                {
                    instances[i] = typeof(T).Assembly.CreateInstance(typeof(T).FullName);
                    object entity = instances[i];
                    PropertyInfo[] propertys = entity.GetType().GetProperties();

                    PropertyInfo p = entity.GetType().GetProperty("PersistentState");
                    if (p != null)
                    {
                        //SetPropertyValue(entity, p, "2");
                        p.SetValue(entity, PersistentState.Deleted, null);
                    }

                    foreach (PropertyInfo pinfo in propertys)
                    {
                        if (!pinfo.CanWrite) continue;

                        string str = string.Empty;
                        if (pinfo.GetCustomAttributes(typeof(DBFieldAttribute), false).Length > 0)
                        {
                            str = ((DBFieldAttribute)pinfo.GetCustomAttributes(typeof(DBFieldAttribute), false)[0]).Field;
                        }
                        else
                        {
                            str = pinfo.Name;//.ToLower();//未设置DBFieldAttribute属性，则取属性名
                        }

                        if (key.IndexOf(',') > 0)//多主键
                        {
                            string[] allkeys = key.Split(',');
                            int keyindex = -1;
                            bool isKey = false;
                            foreach (string k in allkeys)
                            {
                                keyindex++;
                                if (str.Equals(k, StringComparison.OrdinalIgnoreCase))
                                {
                                    isKey = true;
                                    str = k;//搜到                                   
                                    break;
                                }
                            }
                            if (isKey)
                            {
                                if (arr[i]["row"]["key"] != null)
                                {
                                    string value = arr[i]["row"]["key"].ToString();
                                    if (!string.IsNullOrEmpty(value))
                                    {
                                        string[] allValues = value.Split(',');
                                        SetPropertyValue(entity, pinfo, allValues[keyindex]);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (str.Equals(key, StringComparison.OrdinalIgnoreCase))
                            {
                                str = "key";//主键
                                if (arr[i]["row"] != null)
                                {
                                    if (arr[i]["row"][str] != null)
                                    {
                                        string value = arr[i]["row"][str].ToString();//获取json的值                          

                                        SetPropertyValue(entity, pinfo, value);
                                    }
                                }
                                else
                                {
                                    if (arr[i][str] != null)
                                    {
                                        string value = arr[i][str].ToString();//获取json的值                      
                                        SetPropertyValue(entity, pinfo, value);
                                    }
                                }
                            }
                        }
                    }
                    list.Add((T)entity);
                }
            }
            else
            {
                JObject jo = obj as JObject;
                object entity = typeof(T).Assembly.CreateInstance(typeof(T).FullName);

                PropertyInfo p = entity.GetType().GetProperty("PersistentState");
                if (p != null)
                {
                    //SetPropertyValue(entity, p, "2");
                    p.SetValue(entity, PersistentState.Deleted, null);
                }

                PropertyInfo[] propertys = entity.GetType().GetProperties();
                foreach (PropertyInfo pinfo in propertys)
                {
                    string str = string.Empty;
                    if (pinfo.GetCustomAttributes(typeof(DBFieldAttribute), false).Length > 0)
                    {
                        str = ((DBFieldAttribute)(pinfo.GetCustomAttributes(typeof(DBFieldAttribute), false)[0])).Field;
                    }
                    else
                    {
                        str = pinfo.Name;//.ToLower();//未设置DBFieldAttribute属性，则取属性名
                    }

                    if (key.IndexOf(',') > 0)//多主键
                    {
                        string[] allkeys = key.Split(',');
                        int keyindex = -1;
                        foreach (string k in allkeys)
                        {
                            keyindex++;
                            if (str.Equals(k, StringComparison.OrdinalIgnoreCase))
                            {
                                str = k;//搜到                              
                                break;
                            }
                        }
                        if (jo["row"]["key"] != null)
                        {
                            string value = jo["row"]["key"].ToString();
                            if (!string.IsNullOrEmpty(value))
                            {
                                string[] allValues = value.Split(',');
                                SetPropertyValue(entity, pinfo, allValues[keyindex]);
                            }
                        }
                    }
                    else
                    {
                        if (str.Equals(key, StringComparison.OrdinalIgnoreCase))
                        {
                            str = "key";//主键

                            if (jo["row"] != null)
                            {
                                if (jo["row"][str] != null)
                                {
                                    string value = jo["row"][str].ToString();//获取json的值                        
                                    SetPropertyValue(entity, pinfo, value);
                                }
                            }
                            else
                            {
                                if (jo[str] != null)
                                {
                                    string value = jo[str].ToString();//获取json的值                        
                                    SetPropertyValue(entity, pinfo, value);
                                }
                            }
                        }
                    }
                }
                list.Add((T)entity);
            }
            return list;
        }

        private void SetPropertyValue(object entity, PropertyInfo pinfo, string value)
        {
            try
            {


                if (IsType(pinfo.PropertyType, typeof(String).ToString(), typeof(String).ToString()))
                {
                    pinfo.SetValue(entity, value, null);
                    return;
                }

                if (IsType(pinfo.PropertyType, typeof(Boolean).ToString(), "System.Nullable`1[System.Boolean]"))
                {
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        pinfo.SetValue(entity, Boolean.Parse(value), null);
                    }
                    return;
                }

                if (IsType(pinfo.PropertyType, typeof(Int32).ToString(), "System.Nullable`1[System.Int32]"))
                {
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        pinfo.SetValue(entity, Int32.Parse(value), null);
                    }
                    else
                    {
                        pinfo.SetValue(entity, 0, null);//checkbox控件去掉勾用到
                    }
                    return;
                }

                if (IsType(pinfo.PropertyType, typeof(Int16).ToString(), "System.Nullable`1[System.Int16]"))
                {
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        //checkcolumn传过来的true或者false，值转换一下,数据库中统一用整形0和1表示          
                        if (value.ToLower() == "true")
                        {
                            value = "1";
                        }
                        if (value.ToLower() == "false")
                        {
                            value = "0";
                        }
                        pinfo.SetValue(entity, Int16.Parse(value), null);
                    }
                    else
                    {
                        Int16 i = 0;
                        pinfo.SetValue(entity, i, null);
                    }
                    return;
                }

                if (IsType(pinfo.PropertyType, typeof(Int64).ToString(), "System.Nullable`1[System.Int64]"))
                {
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        pinfo.SetValue(entity, Int64.Parse(value), null);
                    }
                    else
                    {
                        pinfo.SetValue(entity, 0, null);
                    }
                    return;
                }

                if (IsType(pinfo.PropertyType, typeof(Byte).ToString(), "System.Nullable`1[System.Byte]"))
                {
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        //checkcolumn传过来的true或者false，值转换一下,数据库中统一用整形0和1表示          
                        if (value.ToLower() == "true")
                        {
                            value = "1";
                        }
                        if (value.ToLower() == "false")
                        {
                            value = "0";
                        }
                        pinfo.SetValue(entity, Byte.Parse(value), null);
                    }
                    return;
                }

                if (IsType(pinfo.PropertyType, typeof(Double).ToString(), "System.Nullable`1[System.Double]"))
                {
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        //pinfo.SetValue(entity, Double.Parse(value), null);
                        var d = Convert.ToDouble(decimal.Parse(value, System.Globalization.NumberStyles.Float));//包含科学计数法
                        pinfo.SetValue(entity, d, null);                        
                    }
                    else
                    {
                        pinfo.SetValue(entity, 0, null);
                    }
                    return;
                }

                if (IsType(pinfo.PropertyType, typeof(Decimal).ToString(), "System.Nullable`1[System.Decimal]"))
                {
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        //pinfo.SetValue(entity, decimal.Parse(value), null);//5位小数点，json数值里包含科学计数法，报错
                        var d = Convert.ToDecimal(decimal.Parse(value, System.Globalization.NumberStyles.Float));//包含科学计数法
                        pinfo.SetValue(entity, d, null);                        
                    }
                    else
                    {
                        pinfo.SetValue(entity, new Decimal(0), null);
                    }
                    return;
                }

                if (IsType(pinfo.PropertyType, typeof(DateTime).ToString(), "System.Nullable`1[System.DateTime]"))
                {
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        try
                        {
                            //pinfo.SetValue(entity, DateTime.ParseExact(value, "yyyy-MM-dd HH:mm:ss", null), null);
                            pinfo.SetValue(entity, Convert.ToDateTime(value), null);
                        }
                        catch (Exception)
                        {
                            //pinfo.SetValue(entity, (DateTime?)DateTime.ParseExact(value, "yyyy-MM-dd", null), null);
                            pinfo.SetValue(entity, Convert.ToDateTime(value), null);
                        }

                    }
                    else
                    {
                        pinfo.SetValue(entity, null, null);
                    }
                    return;
                }

                if (pinfo.PropertyType.IsEnum)
                {
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        pinfo.SetValue(entity, Convert.ToInt32(value), null);
                    }
                    //if (!string.IsNullOrWhiteSpace(value))
                    //{
                    //    if (Enum.GetUnderlyingType(pinfo.GetValue(entity, null).GetType()) == typeof(int))
                    //    {
                    //        try
                    //        {
                    //            pinfo.SetValue(entity, Convert.ToInt32(value), null);
                    //        }
                    //        catch
                    //        {
                    //            var enumValues =
                    //                pinfo.GetValue(entity, null)
                    //                    .GetType()
                    //                    .GetFields(BindingFlags.Static | BindingFlags.Public);

                    //            foreach (var ev in enumValues.Where(ev => ev.Name == value))
                    //            {
                    //                pinfo.SetValue(entity, ev, null);
                    //            }
                    //        }
                    //    }
                    //    else
                    //    {
                    //        pinfo.SetValue(entity, value, null);
                    //    }
                    //}
                    //else
                    //{
                    //    pinfo.SetValue(entity, new Decimal(0), null);
                    //}
                    return;
                }

            }
            catch (Exception ex)
            {
                string msg = string.Format("SetPropertyValue数据转换失败，属性名:[{0}],数据值为：{1},", pinfo.Name, value);
                Logger.Error(msg + "详细错误: " + ex.StackTrace);
                throw new Exception(msg,ex);
            }
        }

        private Decimal ToDecimal(string data)
        {
            decimal d = 0.0M;
            if (data.Contains("E"))//科学计数法
            {
                d = Convert.ToDecimal(decimal.Parse(data, System.Globalization.NumberStyles.Float));
            }
            else
            {
                d = Convert.ToDecimal(data);
            }
            return d;
        }

        public bool IsType(Type type, string typeName, string aliaName)
        {
            if (type == null) return false;

            if (type.ToString() == typeName || type.ToString() == aliaName)
                return true;
            if (type.ToString() == "System.Object")
                return false;

            return IsType(type.BaseType, typeName, aliaName);
        }

    }
        

    public class EntityInfo<T>
    {

        private List<T> newRow = new List<T>();//新增行
        private List<T> modifyRow = new List<T>();//修改行
        private List<T> deleteRow = new List<T>();//删除行
        private List<T> unChangeRow = new List<T>();//删除行
        private List<T> allRow = new List<T>();//删除行

        public List<T> UnChangeRow
        {
            get { return unChangeRow; }
            set { unChangeRow = value; }
        }

        public List<T> NewRow
        {
            get { return newRow; }
            set { newRow = value; }
        }

        public List<T> ModifyRow
        {
            get { return modifyRow; }
            set { modifyRow = value; }
        }

        public List<T> DeleteRow
        {
            get { return deleteRow; }
            set { deleteRow = value; }
        }

        public List<T> AllRow
        {
            get { return allRow; }
            set { allRow = value; }
        }
    }

    [Serializable]
    public enum PersistentState
    {
        /// <summary>
        /// 游离状态
        /// </summary>
        UnChanged = 0,
        /// <summary>
        /// 新增
        /// </summary>
        Added = 1,
        /// <summary>
        /// 修改
        /// </summary>
        Modified = 2,
        /// <summary>
        /// 删除
        /// </summary>
        Deleted = 3,
    }


    /// <summary>
    /// 实体属性和数据库字段对应 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DBFieldAttribute : Attribute
    {
        public DBFieldAttribute()
        {

        }

        public string Field
        {
            get;
            set;
        }
    }

    public class MappingInfo
    {
        static Configuration _cfg;
        private static List<string> typeAssemblys;
        public MappingInfo()
        {
            if (_cfg == null)
            {
                typeAssemblys = new List<string>();
                _cfg = new Configuration().Configure();
            }
        }

        /// <summary>
        /// 获取字段、属性映射信息
        /// </summary>
        /// <param name="type">实体类类型</param>
        /// <returns>字典</returns>
        public Dictionary<string, string> GetMappingInfo(Type type)
        {
            Dictionary<string, string> colMappings = new Dictionary<string, string>();
            //Configuration cfg = new Configuration().Configure();

            if (!typeAssemblys.Contains(type.Assembly.FullName))
            {
                typeAssemblys.Add(type.Assembly.FullName);
                _cfg.AddAssembly(type.Assembly);
            }

            var persistentClass = _cfg.GetClassMapping(type);
            string accessorName = string.Empty;

            if (persistentClass == null)
                return colMappings;

            if (persistentClass.Identifier.GetType().FullName == "NHibernate.Mapping.Component")
            {
                foreach (var item in ((NHibernate.Mapping.Component)(persistentClass.Identifier)).PropertyIterator)
                {
                    accessorName = string.Empty;
                    foreach (Column selectable in item.ColumnIterator)
                    {
                        if (accessorName == selectable.Name)
                            break;

                        accessorName = selectable.Name;
                    }

                    colMappings.Add(item.Name, accessorName);

                }
            }
            else if (persistentClass.Identifier.GetType().FullName == "NHibernate.Mapping.SimpleValue")
            {
                Property ip = persistentClass.IdentifierProperty;

                accessorName = string.Empty;
                foreach (Column selectable in ip.ColumnIterator)
                {
                    if (accessorName == selectable.Name)
                        break;
                    else
                        accessorName = selectable.Name;
                }

                colMappings.Add(ip.Name, accessorName);

            }

            foreach (var item in persistentClass.PropertyClosureIterator)
            {
                accessorName = string.Empty;
                foreach (Column selectable in item.ColumnIterator)
                {
                    if (accessorName == selectable.Name)
                        break;
                    else
                        accessorName = selectable.Name;
                }

                colMappings.Add(item.Name, accessorName);
            }

            return colMappings;
        }

        /// <summary>
        /// 获取类映射的表名
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <returns></returns>
        public String GetEntityMapTableName(Type type)
        {
            if (!typeAssemblys.Contains(type.Assembly.FullName))
            {
                typeAssemblys.Add(type.Assembly.FullName);
                _cfg.AddAssembly(type.Assembly);
            }

            var persistentClass = _cfg.GetClassMapping(type);

            if (persistentClass == null)
            {
                return string.Empty;
            }
            else
            {
                return persistentClass.Table.Name;
            }
        }
    }
}
