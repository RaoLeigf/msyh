using System.Web.Mvc;
using NG3.Aop.Transaction;
using NG3.Web.Controller;
//using SUP.Common.Facade;
using SUP.Common.Base;
using System.Collections.Generic;
using System.IO;
using System;
using System.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using NG3;
using SUP.Frame.Facade.Interface;
using SUP.Frame.Facade;
using NG3.Log.Log4Net;

namespace SUP.Frame.Controller
{
    public class ExcelImportController : AFController
    {
        private IExcelImportFacade proxy;
        //private IIndividualUIFacade uiproxy;

        public ExcelImportController()
        {
            proxy = AopObjectProxy.GetObject<IExcelImportFacade>(new ExcelImportFacade());
            //uiproxy = AopObjectProxy.GetObject<IIndividualUIFacade>(new IndividualUIFacade());
        }


        #region 日志相关
        private ILogger _logger = null;
        internal ILogger Logger
        {
            get
            {
                if (_logger == null)
                {                    
                    _logger = Log4NetLoggerFactory.Instance.CreateLogger(typeof(ExcelImportController), LogType.logoperation);
                }
                return _logger;
            }
        }
        #endregion


        #region  list

        public ActionResult Index()
        {
            return View("List");
        }

        public string LoadMenu()
        {
            string nodeid = System.Web.HttpContext.Current.Request.Params["node"];
            string id = System.Web.HttpContext.Current.Request.Params["id"];
            IList<TreeJSONBase> list = proxy.LoadMenu(nodeid,id);
            string s = Newtonsoft.Json.JsonConvert.SerializeObject(list);
            return s;
        }


        /// <summary>
        /// 导入（上传）excel文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string Upload()
        {
            string templateId = System.Web.HttpContext.Current.Request.Params["templateId"];
            var file = System.Web.HttpContext.Current.Request.Files[0];

            string filename = file.FileName;
            string filepath = Server.MapPath("~//ExcelImportTemp//UpLoad//") + Path.GetFileName(filename);

            string directoryName = Server.MapPath("~//ExcelImportTemp//UpLoad//");
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            try
            {
                if (file.ContentLength > 0)
                {
                    file.SaveAs(filepath);
                    proxy.BindFile(templateId, filename, filepath);
                    return "{success:\"true\",message:\"" + filename + "已导入，请加载到页面修改" + "\"}";
                }
                else
                {
                    return "{success:\"false\",message:\"" + "文件无内容" + "\"}";
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.StackTrace);
                return "{success:\"false\",message:\"" + ex.Message + "\"}";
            }
        }

        /// <summary>
        /// 加载tab页
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetTableName()
        {
            List<string> tableNameList = null;
            try
            {
                tableNameList = proxy.GetTableName();
                string s = Newtonsoft.Json.JsonConvert.SerializeObject(tableNameList);

                return "{success:\"true\"," + "tableName:  " + s + "}";
            }
            catch (Exception ex)
            {
                Logger.Error(ex.StackTrace);
                return "{success:\"false\"," + "message: \" " + ex.Message + "\"}";
            }
           
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string Save()
        {
            string data = System.Web.HttpContext.Current.Request.Params["data"];
            string tableName = System.Web.HttpContext.Current.Request.Params["tableName"];
            string count = System.Web.HttpContext.Current.Request.Params["count"];
            string selected = System.Web.HttpContext.Current.Request.Params["selected"];

            string message = "";
            try
            {
                Dictionary<string, string> tableDic = ToDictionary(data, tableName, count);
                if (tableDic.Count == 0)
                {
                    return "0#无数据";
                    //return "{\"success\":\"false\",\"message\":\"" + "无数据" + "\"}";
                }

                DataSet ds = new DataSet();
                foreach (var kv in tableDic)
                {
                    string s = proxy.GetSelectSql(kv.Value, kv.Key);//模板里定义了数据不存在的列，需要合并到sql中,数据原样传给插件
                    DataTable dt = DataConverterHelper.ToDataTable(kv.Value, s);
                    //DataTable dt = DataConverterHelper.ToDataTable(kv.Value, "select * from  " + kv.Key);
                    dt.TableName = kv.Key;
                    ds.Tables.Add(dt);                   
                }

                bool flag = proxy.Save(ds, selected, ref message);

                if (false == flag)
                {
                    return "0#"+message+"";
                    //return "{\"success\":\"false\",\"message\":\"" + message + "\"}";
                }

                //return "{\"success\":\"true\",\"message\":\"" + message + "\"}";
                return "1#"+message+"";

            }
            catch (Exception ex)
            {
                Logger.Error(ex.StackTrace);
                //return "{\"success\":\"false\",\"message\":\"" + ex.Message + "\"}";
                return "0#"+ ex.Message + "";

            }

        }

        private Dictionary<string, string> ToDictionary(string data, string tableName, string cot)

        {
            int count = Convert.ToInt16(cot);
            JObject dataJo = JsonConvert.DeserializeObject(data) as JObject;
            JObject tableNameJo = JsonConvert.DeserializeObject(tableName) as JObject;

            Dictionary<string, string> dic = new Dictionary<string, string>();

            try
            {
                for (int i = 0; i < count; i++)
                {
                    var temp = tableNameJo[i.ToString()];
                    var name = temp.ToString();
                    var da = JsonConvert.SerializeObject(dataJo[name]);
                    dic.Add(name, da);
                }

            }
            catch
            {
                throw new Exception("在ExcelListController中方法ToDictionary抛异常");
            }

            return dic;
        }
        #endregion

        #region grid

        private static Dictionary<string, DataTable> dictionary;

        public ActionResult GridIndex()
        {
            string tableName = System.Web.HttpContext.Current.Request.Params["tableName"];
            ViewBag.tableName = tableName;
            dictionary = new Dictionary<string, DataTable>();
            return View("Grid");
        }

        //public string GetExcelData()
        //{
        //    string tableName = System.Web.HttpContext.Current.Request.Params["tableName"];
        //    DataTable dt;
        //    dictionary.TryGetValue(tableName, out dt);//从静态变量中获取数据
        //    string json = DataConverterHelper.ToJson(dt, dt.Rows.Count);
        //    return json;
        //}

        //public string GetFieldNames()

        /// <summary>
        /// 获取grid列信息和数据
        /// </summary>
        /// <returns></returns>
        public string GetColumInfoAndData()
        {
            try
            {
                string tableName = System.Web.HttpContext.Current.Request.Params["tableName"];
                string jsonStr;
                DataTable dt = proxy.GetExcelDataAndColInfo(tableName, out jsonStr);
                //dictionary.Add(tableName, dt);//获取数据，缓存在静态变量中，多w3wp.exe会有问题,应当放到全局缓存中
                //return jsonStr;
                string data = DataConverterHelper.ToJsonData(dt, dt.Rows.Count,false);

                return "{status:true,data:{exceldata:"+ data + ",columinfo:" +jsonStr + "}}";
            }
            catch (Exception ex)
            {
                Logger.Error(ex.StackTrace);
                return "{status:false,message:\"" + ex.Message + "\"}";
            }
        }

        #endregion

        #region edit

        private static string filePath;
        private static string fileName;

        public ActionResult EditIndex()
        {
            ViewBag.OType = System.Web.HttpContext.Current.Request.Params["otype"];
            ViewBag.ID = System.Web.HttpContext.Current.Request.Params["id"];

            return View("Edit");
        }

        public string GetXmlStoreData(string id)
        {
            DataTable dt = proxy.GetTemplate(id);
            string json = DataConverterHelper.ToJson(dt, dt.Rows.Count);

            return json;
        }

        [HttpPost]
        public string GetFormData(string id)
        {
            try
            {
                DataTable dt = proxy.GetFormData(id);
                if (dt.Rows.Count > 0)
                {
                    JObject jo = dt.Rows[0].ToJObject();
                    string json = JsonConvert.SerializeObject(jo);
                    return "{status : \"ok\", data:" + json + "}";
                }
                else
                {
                    return "{status : \"error\"}";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public string ExportTemplate()
        {
            string griddata = System.Web.HttpContext.Current.Request.Params["griddata"];
            string multipleSheet = System.Web.HttpContext.Current.Request.Params["multipleSheet"];
            string name = System.Web.HttpContext.Current.Request.Params["name"];

            DataTable dt = ToDataTable(griddata);
            try
            {
                filePath = Server.MapPath("~//ExcelImportTemp//DownLoad//" + name + ".xls");
                fileName = Path.GetFileName(filePath);
                MemoryStream ms = proxy.ExportTemplate(dt, multipleSheet);
                WriteBufferToFile(filePath, ms);
                return "{status: \"ok\",fileName:\"" + fileName + "\"}";
            }
            catch (Exception ex)
            {
                return "{status: \"" + ex.Message + "\"}";
            }
        }

        public FileResult Download()
        {
            return File(filePath, "application/ms-excel", fileName);
        }

        private DataTable ToDataTable(string json)
        {
            DataTable dt = new DataTable();
            JObject jo = JsonConvert.DeserializeObject(json) as JObject;

            string root = "table";//默认值
            JToken newRow = jo[root]["newRow"];
            if (newRow is JArray)
            {
                JArray arr = newRow as JArray;

                foreach (JObject item in arr)
                {
                    DataRow dr = dt.NewRow();
                    JToken row = item["row"];//AF定位到行
                    JObject temp = row as JObject;

                    foreach (JProperty property in temp.Properties())
                    {
                        string field = property.Name;
                        if (!dt.Columns.Contains(field))
                        {
                            dt.Columns.Add(field);
                        }
                        string value = property.Value.ToString();
                        dr[field] = value;
                    }
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }

        private void WriteBufferToFile(string filePath, MemoryStream ms)
        {
            string directoryName = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                byte[] buff = ms.ToArray();
                fs.Write(buff, 0, buff.Length);
                ms.Close();
            }
        }


        #endregion

    }
}
