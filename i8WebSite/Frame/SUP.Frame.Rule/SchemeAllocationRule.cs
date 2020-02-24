using NG3.Log.Log4Net;
using SUP.Frame.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SUP.Frame.Rule
{
    public class SchemeAllocationRule
    {
        private SchemeAllocationDac dac = new SchemeAllocationDac();
        private ILogger _logger = null;
        private ILogger Logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = Log4NetLoggerFactory.Instance.CreateLogger(typeof(SchemeAllocationRule), LogType.logoperation);
                }
                return _logger;
            }
        }

        public DataTable GetUserSchemeAllocation()
        {
            try
            {
                DataTable dt = dac.GetUserSchemeAllocation();
                //将角色的编码和名称合并到操作员以便于前台处理
                if (dt != null && dt.Rows.Count > 0)
                {
                    for(int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["usertype"].ToString() == "1")
                        {
                            dt.Rows[i]["userno"] = dt.Rows[i]["roleno"];
                            dt.Rows[i]["username"] = dt.Rows[i]["rolename"];
                        }
                    }
                }
                dt.Columns.Remove("roleno");
                dt.Columns.Remove("rolename");
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //public string SaveUserSchemeAllocation(string oriuserid, string oriusertype, string userid, string usertype)
        //{
        //    return dac.SaveUserSchemeAllocation(oriuserid, oriusertype, userid, usertype);
        //}

        public string SaveUserSchemeAllocation(string oriuserid, string oriusertype, string userid, string usertype)
        {
            string[] useridArr = userid.Split('#');
            for (int i = 0; i < useridArr.Length; i++)
            {
                bool flag = dac.CopyIndividualSchema(oriuserid, oriusertype, useridArr[i], usertype);
                if (!flag)
                {
                    return string.Empty;
                }

                string path = AppDomain.CurrentDomain.BaseDirectory + @"CONFIG\UserConfig.xml";
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNodeList xmlNodeList = doc.SelectNodes("UserConfig/item");
                XmlElement xmlElement;
                bool result = true;
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    xmlElement = (XmlElement)xmlNode;
                    string dllpath = AppDomain.CurrentDomain.BaseDirectory + xmlElement.SelectSingleNode("path").InnerText;
                    object[] pars = new object[4];
                    pars[0] = long.Parse(oriuserid);
                    pars[1] = int.Parse(oriusertype);
                    pars[2] = long.Parse(useridArr[i]);
                    pars[3] = int.Parse(usertype);
                    try
                    {
                        result = (bool)ReflectDll(dllpath, xmlElement.SelectSingleNode("classname").InnerText, "CopyUserConfig", pars);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("[CopyIndividualSchema] " + xmlElement.SelectSingleNode("classname").InnerText + ":" + ex.Message);
                        return xmlElement.SelectSingleNode("classname").InnerText + "调用失败";
                    }
                    if (!result)
                    {
                        return xmlElement.SelectSingleNode("classname").InnerText + "调用失败";
                    }
                }
            }
           
            return "success";
        }


        public string DeleteUserSchemeAllocation(string phid)
        {
            DataTable dt = dac.DeleteIndividualSchema(phid);

            string path = AppDomain.CurrentDomain.BaseDirectory + @"CONFIG\UserConfig.xml";
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNodeList xmlNodeList = doc.SelectNodes("UserConfig/item");
            XmlElement xmlElement;
            bool result = true;
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                xmlElement = (XmlElement)xmlNode;
                string dllpath = AppDomain.CurrentDomain.BaseDirectory + xmlElement.SelectSingleNode("path").InnerText;
                object[] pars = new object[2];
                pars[0] = dt.Rows[0]["userid"];
                pars[1] = dt.Rows[0]["usertype"];
                try
                {
                    result = (bool)ReflectDll(dllpath, xmlElement.SelectSingleNode("classname").InnerText, "DeleteUserConfig", pars);
                }
                catch (Exception ex)
                {
                    Logger.Error("[DeleteIndividualSchema] " + xmlElement.SelectSingleNode("classname").InnerText + ":" + ex.Message);
                    return xmlElement.SelectSingleNode("classname").InnerText + "调用失败";
                }
                if (!result)
                {
                    return xmlElement.SelectSingleNode("classname").InnerText + "调用失败";
                }
            }
            return "success";
        }

        private object ReflectDll(string dllpath, string classname, string method, object[] pars)
        {
            Assembly ass = Assembly.LoadFile(dllpath);  //加载dll文件
            Type tp = ass.GetType(classname);  //获取类名，必须 命名空间+类名
            Object obj = Activator.CreateInstance(tp);  //建立实例
            MethodInfo meth = tp.GetMethod(method);  //获取方法
            return meth.Invoke(obj, pars);
        }

    }
}
