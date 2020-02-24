using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;

namespace NGWebSite
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
        public string APIServer
        {
            get
            {
                return row["APIServer"].ToString();
            }
        }

        /// <summary>
        /// 验证服务器地址
        /// </summary>
        public string SecurityServer
        {
            get
            {
                return row["SecurityServer"].ToString();
            }
        }
    }
}
