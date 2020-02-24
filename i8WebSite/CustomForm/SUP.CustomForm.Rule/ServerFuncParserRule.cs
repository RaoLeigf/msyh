using System;
using System.Data;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NG3.Log.Log4Net;
using SUP.Common.Base;
using SUP.CustomForm.DataAccess;

namespace SUP.CustomForm.Rule
{
    public class ServerFuncParserRule
    {
        private ServerFuncParserDac dac = new ServerFuncParserDac();

        #region 日志相关
        private static ILogger _logger = null;
        internal static ILogger Logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = Log4NetLoggerFactory.Instance.CreateLogger(typeof(ServerFuncParserRule), LogType.logrules);
                }
                return _logger;
            }
        }
        #endregion

        /// <summary>
        /// 业务Facade传过来执行元数据函数
        /// </summary>
        /// <returns></returns>
        public string FuncParser(string busname, string extendParam)
        {
            //{ funcname: 'updateOrder', paramstr: { a: 'a', b: 'b' } }
            JObject jo = JObject.Parse(extendParam);
            string funcname = jo["funcname"].ToString();
            string paramstr = jo["paramstr"].ToString();

            return FuncParser(busname, funcname, paramstr, string.Empty);
        }

        /// <summary>
        /// 解析执行元数据函数
        /// </summary>
        /// <returns></returns>
        public string FuncParser(string busname, string funcname, string paramstr, string rtntype)
        {
            try
            {
                var sql = string.Format("select * from p_form_funcmetadata where busname='{0}' and funcname='{1}'", busname, funcname);
                DataTable dt = dac.GetDataTable(sql);

                if (dt == null || dt.Rows.Count == 0)
                {
                    return PackJsonStr("fail", "功能号不存在", 0, "[]");
                }

                //功能类型(1：执行sql；2：执行dll中自定义函数)
                string functype = dt.Rows[0]["functype"].ToString();

                if (functype == "1")
                {
                    //例select compname from enterprise where compno=@aaa.compno and ocode=@ocode
                    string sqlstr = dt.Rows[0]["sqlstr"].ToString().Trim();
                    string sqlori = sqlstr;
                    bool isSelect = false;  //是否查询语句
                    int rtn = 0;
                    DataTable sqlDt = new DataTable();
                    DataTable totalDt = new DataTable();

                    //判断是select语句还是update、insert、delete语句
                    if (sqlori.IndexOf("select", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        isSelect = true;
                    }

                    //[{ itemno: '0001', phid: '1' }, { itemno: '03', phid: '37632' }, { itemno: '0301', phid: '37633' }]
                    JArray paramArr = (JArray)JsonConvert.DeserializeObject(paramstr);

                    if (paramArr.Count > 0) //有参数
                    {
                        foreach (var param in paramArr)
                        {
                            JObject jo = JObject.Parse(param.ToString());

                            foreach (var item in jo)
                            {
                                sqlstr = sqlstr.Replace("@" + item.Key.ToString(), item.Value.ToString());
                            }

                            //日志输出
                            Logger.Error("FuncParser: 执行注册的sql \r\n sql:" + sqlstr + "\r\n 参数:" + paramstr);

                            if (isSelect)
                            {
                                sqlDt = dac.GetDataTable(sqlstr);
                                totalDt.Merge(sqlDt);  //sqlDt数据导入到汇总dt
                            }
                            else
                            {
                                rtn = dac.ExecuteSql(sqlstr);
                            }

                            sqlstr = sqlori;
                        }

                        //返回json结果串
                        if (isSelect)
                        {
                            string json = string.Empty;

                            if (rtntype == "tree")
                            {
                                CustomTreeBuilder builder = new CustomTreeBuilder();

                                //s_tree_id和s_tree_pid类型可能是int，保险起见都强制转成string
                                DataTable dtResult = new DataTable();
                                dtResult = totalDt.Clone();  //克隆表结构
                                foreach (DataColumn col in dtResult.Columns)
                                {
                                    if (col.ColumnName == "s_tree_id" || col.ColumnName == "s_tree_pid")
                                    {
                                        col.DataType = typeof(string);  //修改列类型
                                    }
                                }
                                foreach (DataRow row in totalDt.Rows)  //数据拷贝
                                {
                                    DataRow dr = dtResult.NewRow();

                                    foreach (DataColumn dc in totalDt.Columns)
                                    {
                                        dr[dc.ColumnName] = row[dc.ColumnName];
                                    }

                                    dtResult.Rows.Add(dr);
                                }
                                totalDt = dtResult;

                                string filter = "(s_tree_pid is null or s_tree_pid='' or s_tree_pid='0')";
                                JArray ja = builder.GetTreeList(totalDt, "s_tree_pid", "s_tree_id", filter, "", TreeDataLevelType.TopLevel, 5);
                                json = JsonConvert.SerializeObject(ja);

                                return json;
                            }
                            else
                            {
                                json = JsonConvert.SerializeObject(totalDt);
                            }

                            return PackJsonStr("ok", "返回datatable转json串", totalDt.Rows.Count, json);
                        }
                        else
                        {
                            if (rtn != (-1))
                            {
                                return PackJsonStr("ok", "执行sql成功", rtn, "");
                            }
                            else
                            {
                                return PackJsonStr("fail", "执行sql失败", rtn, "");
                            }
                        }
                    }
                    else //没有参数
                    {
                        if (isSelect)
                        {
                            sqlDt = dac.GetDataTable(sqlstr);
                            string json = JsonConvert.SerializeObject(sqlDt);

                            return PackJsonStr("ok", "返回datatable转json串", sqlDt.Rows.Count, json);
                        }
                        else
                        {
                            rtn = dac.ExecuteSql(sqlstr);

                            if (rtn != (-1))
                            {
                                return PackJsonStr("ok", "执行sql成功", rtn, "");
                            }
                            else
                            {
                                return PackJsonStr("fail", "执行sql失败", rtn, "");
                            }
                        }
                    }
                }
                else
                {
                    //dll名称， 增行时填默认值，只读列，格式如SUP.CustomForm.pform0000000130Extend.dll
                    //类名，增行时填默认值，只读列，格式如pform0000000130Extend
                    string dllname = dt.Rows[0]["dllname"].ToString();
                    string classname = dt.Rows[0]["classname"].ToString();
                    string methodname = dt.Rows[0]["methodname"].ToString();
                    string namespacename = "SUP.CustomForm." + classname;

                    //日志输出
                    Logger.Error("FuncParser: 执行注册dll函数 \r\n dll:" + dllname + "\r\n 函数名:" + methodname);

                    //加载程序集(dll文件地址)，使用Assembly类   
                    Assembly assembly = Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + "I6Rules\\" + dllname);

                    //获取类型，参数（名称空间+类）
                    Type type = assembly.GetType(namespacename + "." + classname);

                    //创建该对象的实例
                    object instance = Activator.CreateInstance(type);

                    //设置要调用方法中的参数类型，如有多个参数可以追加多个   
                    Type[] params_type = new Type[1];
                    params_type[0] = Type.GetType("System.String");

                    //传递给调用函数的参数
                    object[] objParams = new object[1];
                    objParams[0] = paramstr;

                    MethodInfo mi = type.GetMethod(methodname, params_type);
                    string resultStr = (string)mi.Invoke(instance, objParams);

                    return PackJsonStr("ok", resultStr, 0, "");
                }
            }
            catch (Exception e)
            {
                Logger.Error("FuncParser: " + e.Message + "\r\n" + e.StackTrace);
                throw new Exception("FuncParser: " + e.Message);
            }
        }

        /// <summary>
        /// 组装json字符串
        /// </summary>
        /// <returns></returns>
        public static string PackJsonStr(string status, string message, long count, string record)
        {
            //JObject jo = new JObject();
            //jo.Add("status", status);
            //jo.Add("message", message);
            //jo.Add("count", count);
            //jo.Add("record", record);
            //return JsonConvert.SerializeObject(jo);

            StringBuilder strb = new StringBuilder();

            strb.Append("{status:");
            strb.Append("'" + status + "'");
            strb.Append(",");
            strb.Append("message:");
            strb.Append("'" + message + "'");
            strb.Append(",");
            strb.Append("count:");
            strb.Append(count);
            strb.Append(",");
            strb.Append("record:");
            strb.Append(record);
            strb.Append("}");

            return strb.ToString();
        }
    }

    public class CustomTreeBuilder : TreeBuilderBase
    {
        public override JObject BuildTreeNode(DataRow dr)
        {
            JObject jo = new JObject();
            jo.Add("pid", dr["s_tree_pid"].ToString());
            jo.Add("id", dr["s_tree_id"].ToString());
            jo.Add("text", dr["s_tree_name"].ToString());

            foreach (DataColumn item in dr.Table.Columns)
            {
                jo.Add(item.ColumnName, dr[item.ColumnName].ToString());
            }

            return jo;
        }
    }
}
