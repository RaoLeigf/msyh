using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GData.YQHL.Service.ICNBCNC
{    

    public class ICBCNCHelper
    {
        /// <summary>
        /// 交易模板xml字典
        /// </summary>
        static readonly Dictionary<string, string> transXmlDic = new Dictionary<string, string>();

        ICBCNCHelper() { }

        static readonly object obj = new object();
        static ICBCNCHelper _instance;

        public static ICBCNCHelper GetInstance(bool forceInit = false)
        {
            if (forceInit)
            {
                _instance = new ICBCNCHelper();
            }

            if (_instance == null)
            {
                lock (obj)
                {
                    if (_instance == null)
                    {
                        _instance = new ICBCNCHelper();
                    }
                }
            }
            return _instance;
        }


        /// <summary>
        /// 获得交易模板信息
        /// </summary>
        /// <param name="template">模板名称</param>
        /// <param name="forceInit">强制重新获取</param>
        /// <returns></returns>
        public static string GetXmlString(string templateName, bool forceInit = false) {
            //是否已经读取过
            if (transXmlDic.ContainsKey(templateName))
            {
                if (forceInit)
                {
                    transXmlDic[templateName] = ReadXmlStringFromFile(templateName);
                }                
            }
            else {
                //增加键值
                transXmlDic.Add(templateName, ReadXmlStringFromFile(templateName));
            }

            return transXmlDic[templateName];
        }

        /// <summary>
        /// 读取对应模板，返回对应的xml串
        /// </summary>
        /// <param name="templateName"></param>
        /// <returns></returns>
        private static string ReadXmlStringFromFile(string templateName) {
            string path = System.AppDomain.CurrentDomain.BaseDirectory + @"TransTemplate\ICBCNC\" + templateName + ".xml";
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(string.Format("模板{0}不存在，请检查模板名称是否正确!", templateName));
            }
            else
            {
                string xmlString = string.Empty;
                using (StreamReader sr = new StreamReader(path, Encoding.GetEncoding("GBK")))
                {
                    xmlString = sr.ReadToEnd();
                }
                return xmlString;
            }
        }

    }
}
