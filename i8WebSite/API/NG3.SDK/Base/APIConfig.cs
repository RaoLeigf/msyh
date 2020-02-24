using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;

namespace NG3.SDK
{
    internal class APIConfig
    {
        internal const string APPKEY = "appkey";
        internal const string APPPARM = "appparm";
        internal const string FORMAT = "format";
        internal const string METHOD = "method";
        internal const string TIMESTAMP = "timestamp";
        internal const string VERSION = "v";
        internal const string SIGN = "sign";
        internal const string FORMAT_XML = "xml";
        internal const string FORMAT_JSON = "json";
        internal const string APPSECRET = "appsecret";

        private DataRow row;
        private string pwd = string.Empty;
        private string conn = string.Empty;
        private DataSet dataSet;
        private string path;

        public APIConfig()
        {
            Init();
        }

        protected virtual void Init()
        {
            try
            {
                if (HttpContext.Current == null)
                {
                    path = AppDomain.CurrentDomain.BaseDirectory + @"config\APIConfig.xml";
                }
                else
                {
                    path = HttpContext.Current.Request.PhysicalApplicationPath + "config\\APIConfig.xml";
                }
                if (System.IO.File.Exists(path))
                {
                    dataSet = new DataSet();
                    dataSet.ReadXml(path);
                    row = dataSet.Tables[0].Rows[0];
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 是否开启调试信息
        /// </summary>
        public bool Debug
        {
            get
            {
                return row["Debug"].ToString() == "1";
            }
        }

        /// <summary>
        /// 服务器地址
        /// </summary>
        public string LogAPIServer
        {
            get
            {
                return row["LogAPIServer"].ToString();
            }
        }

        /// <summary>
        /// 服务器地址
        /// </summary>
        public string W3APIServer
        {
            get
            {
                return row["W3APIServer"].ToString();
            }
        }
        
        /// <summary>
        /// 服务器地址
        /// </summary>
        public string I6PServer
        {
            get
            {
                return row["I6PServer"].ToString();
            }
        }
        /// <summary>
        /// 服务器地址
        /// </summary>
        public string WorkFlowServer
        {
            get
            {
                return row["WorkFlowServer"].ToString();
            }
        }

        /// <summary>
        /// 服务器地址
        /// </summary>
        public string TaskListServer
        {
            get
            {
                return row["TaskListServer"].ToString();
            }
        }
        
        /// <summary>
        /// 服务器地址
        /// </summary>
        public string SSOServer
        {
            get
            {
                return row["SSOServer"].ToString();
            }
        }

        /// <summary>
        /// 权限数据库连接
        /// </summary>
        public string RightDbConn
        {
            get
            {
                return row["RightDbConn"].ToString();
            }
        }
    }
}
