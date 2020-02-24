
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SUP.Common.Base;
using System.Data;

namespace Enterprise3.WebApi.GBK3.BK.Controller
{
    /// <summary>
    /// 
    /// </summary>
    public class DCHelper
    {
        /// <summary>
        ///仿造I8返回格式，重新返回通用json格式
        /// </summary>
        public static String ModelListToJson<T>(IList<T> ts, Int64 count) {
            return "{\"totalRows\":" + count + ",\"Record\":" + JsonConvert.SerializeObject(ts) + "}";
        }

        /// <summary>
        ///仿造I8返回格式，重新返回通用json格式，分页
        /// </summary>
        public static String ModelListToJson<T>(IList<T> ts, Int64 count,Int64 pageindex,Int64 pagesize)
        {
            return "{\"totalRows\":" + count + ",\"Record\":" + JsonConvert.SerializeObject(ts) + ",\"index\":"+ pageindex + ",\"size\":" + pagesize + "}";
        }

        /// <summary>
        ///仿造I8返回格式，重新返回通用json格式，分页
        /// </summary>
        public static String ModelListToJson<T>(IList<T> ts, Int64 count, Int64 pageindex, Int64 pagesize,String CheckRes)
        {
            return "{\"totalRows\":" + count + ",\"Record\":" + JsonConvert.SerializeObject(ts) + ",\"index\":" + pageindex + ",\"size\":" + pagesize + ",\"CheckRes\":\""+CheckRes+"\"}";
        }

        /// <summary>
        ///仿造I8返回格式，重新返回通用json格式，分页
        /// </summary>
        //public static String ModelListToJson<T>(BaseEntityListReturn<T> baseEntityListReturn)
        //{
        //    return JsonConvert.SerializeObject(baseEntityListReturn);
        //}

        /// <summary>
        ///返回错误信息
        /// </summary>
        public static String ErrorMessage(string message) {
            return "{\"Status\":\"error\",\"Msg\":\"" + (message??"") +"\"}";
        }

        /// <summary>
        ///返回成功信息
        /// </summary>
        public static String SuccessMessage(string message)
        {
            return "{\"Status\":\"success\",\"Msg\":\"" + (message ?? "") + "\"}";
        }



        /// <summary>
        ///返回可变参数信息
        /// </summary>
        public static String Message(Dictionary<string,object> message)
        {
            return JsonConvert.SerializeObject(message);
        }

        //// <summary>
        ///改变消息状态码为正确
        /// </summary>
        public static void ConvertDic2Success(Dictionary<string, object> message) {
            try
            {
                message["Status"] = "success";
            }
            catch (Exception ex) {
                message.Add("Status", "success");
            }
        }

        //// <summary>
        ///改变消息状态码为错误
        /// </summary>
        public static void ConvertDic2Error(Dictionary<string, object> message)
        {
            try
            {
                message["Status"] = "error";
            }
            catch (Exception ex)
            {
                message.Add("Status", "error");
            }
        }

        //// <summary>
        ///为返回结果添加消息
        /// </summary>
        public static void AddMessage(Dictionary<string, object> message,string Msg)
        {
            try
            {
                message["Msg"] = Msg;
            }
            catch (Exception ex)
            {
                message.Add("Msg", Msg);
            }
        }

        //// <summary>
        ///为返回结果添加数据
        /// </summary>
        public static void AddData(Dictionary<string, object> message, object data)
        {
            try
            {
                message["Data"] = data;
            }
            catch (Exception ex)
            {
                message.Add("Data", data);
            }
        }


        /// <summary>
        ///返回信息数组
        /// </summary>
        public static String Message(String[] messages)
        {
            return JsonConvert.SerializeObject(messages);
        }

        /// <summary>
        ///递归挖掘基本表达式
        /// </summary>
        public static void ConvertDic<T>(Dictionary<string, object> dic) where T : class
        {
            if (!dic.ContainsKey("[or-dictionary0]*dictionary*or")) return;  //貌似只有使用or才会出错，所以没有or时就不出来了

            //放入泛型T是为了下面方便数据类型转换，不然nhbinert底层映射会出错
            IList<String> keys = dic.Keys.ToList<String>();
            foreach (string key in keys)
            {
                if (key.Equals("[or-dictionary0]*dictionary*or"))
                {
                    var value = dic[key];
                    if (value is JObject)
                    {
                        IEnumerable<JProperty> Properties = (value as JObject).Properties();
                        //转换为dictionary的list
                        List<Dictionary<string, object>> diclist = new List<Dictionary<string, object>>(Properties.Count());
                        //创建新dic
                        foreach (JProperty jp in Properties)
                        {

                            Dictionary<string, object> newdic = new Dictionary<string, object>();
                            JToken JValue = null;
                            (value as JObject).TryGetValue(jp.Name, out JValue);
                            
                            //判断是否是基本表达式了
                            if (!(JValue is JObject))
                            {
                                //tempPname是参数名，就是 属性名*属性类型 * 运算符
                                string[] tempPname = jp.Name.Split('*');
                                string TPorterName = tempPname[0];
                                //获取到对应参数在系统中的类型
                                Type Ptype = Type.GetType(typeof(T).GetProperty(TPorterName).PropertyType.FullName);
                                //通过调用对应参数类型的Parse方法，进行对应类型转换，异常的情况：String没有parse方法
                                try
                                {
                                    var ref_value = Ptype.GetMethod("Parse", new Type[] { typeof(string) }).Invoke(null, new object[] { JValue.ToString() });
                                    newdic.Add(jp.Name, ref_value);
                                }
                                catch (NullReferenceException Nullex)
                                {
                                    newdic.Add(jp.Name, JValue.ToObject<String>());
                                }
                            }
                            else
                            {
                                newdic.Add(jp.Name, JValue);
                            }

                            //创建完毕后，递归调用ConvertDic
                            ConvertDic<T>(newdic);
                            diclist.Add(newdic);
                        }
                        //将dicList替换掉原来的值
                        dic[key] = diclist;
                    }
                }
                if (key.Equals("AND"))
                {
                    var value = dic[key];
                    IEnumerable<JProperty> Properties = (value as JObject).Properties();
                    //创建新dic
                    foreach (JProperty jp in Properties)
                    {
                        JToken JValue = null;
                        (value as JObject).TryGetValue(jp.Name, out JValue);
                        //判断是否是基本表达式了
                        if (!(JValue is JObject))
                        {
                            //tempPname是参数名，就是 属性名*属性类型 * 运算符
                            string[] tempPname = jp.Name.Split('*');
                            string TPorterName = tempPname[0];
                            //获取到对应参数在系统中的类型
                            Type Ptype = Type.GetType(typeof(T).GetProperty(TPorterName).PropertyType.FullName);
                            //通过调用对应参数类型的Parse方法，进行对应类型转换，异常的情况：String没有parse方法
                            try
                            {
                                var ref_value = Ptype.GetMethod("Parse", new Type[] { typeof(string) }).Invoke(null, new object[] { JValue.ToString() });
                                dic.Add(jp.Name, ref_value);
                            }
                            catch (NullReferenceException Nullex)
                            {
                                dic.Add(jp.Name, JValue.ToObject<String>());
                            }
                        }
                        else
                        {
                            dic.Add(jp.Name, JValue);
                        }
                    }
                    //将子项全部都添加进来以后，直接移除原来的and项
                    dic.Remove("AND");
                    ConvertDic<T>(dic);
                }
            }


        }

        /// <summary>
        ///通过Json获得Session中保存的对象，并更新其值
        /// </summary>
        public static T JsonToEntity<T>(JObject model, T entity) where T:class
        {
            //获得要变更的属性集合
            var Properties = model.Properties();
            foreach (JProperty propertyInfo in Properties) {

                //给持久化对象赋值
                JToken JValue = null;
                model.TryGetValue(propertyInfo.Name, out JValue);
                Type Ptype = Type.GetType(typeof(T).GetProperty(propertyInfo.Name).PropertyType.FullName);
                try
                {
                    var ref_value = Ptype.GetMethod("Parse", new Type[] { typeof(string) }).Invoke(null, new object[] { JValue.ToString() });
                    typeof(T).GetProperty(propertyInfo.Name).SetValue(entity,ref_value);
                }
                catch (NullReferenceException Nullex)
                {
                    typeof(T).GetProperty(propertyInfo.Name).SetValue(entity, JValue.ToString());
                }
                
            }
            typeof(T).GetProperty("PersistentState").SetValue(entity, PersistentState.Modified);
            return entity;
        }

        /// <summary>
        ///DataTable转换成List<T>,要求,列名必须与字段名对应
        /// </summary>
        public static IList<T> DataTable2List<T>(DataTable table) {
            try
            {
                string JsonStr = JsonConvert.SerializeObject(table);
                IList<T> list = JsonConvert.DeserializeObject<IList<T>>(JsonStr);
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "/r/n" + "table转对象集合失败");
            }
        }
    }
}
