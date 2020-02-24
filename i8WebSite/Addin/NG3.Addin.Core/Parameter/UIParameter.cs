using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NG3.Addin.Core.Parameter
{
    /// <summary>
    /// UI参数主要是由三部分构成
    /// </summary>
    public class UIParameter :IAddinParameter
    {
        public static string NO_DATA = "#_NO_DATA_#"; //表示传递上来的数据是空的

        private string[] values = null;
        //全名称
        public string Name { set; get; }

        //.
        public string FirstPart { set; get; }

        //
        public string SecondPart { set; get; }
        public string ThirdPart { set; get; }

        public string RootType { set; get; }

        /// <summary>
        /// 取值
        /// </summary>
        /// <returns></returns>
        public string[] GetValues()
        {

            if (values != null) return values;

            string paramvalue = string.Empty;

            NameValueCollection nvcols = System.Web.HttpContext.Current.Request.Params;


            //取值不区分大小写       
            if (string.IsNullOrEmpty(FirstPart))
            {
                //一段式，直接传参数
                paramvalue = nvcols[ThirdPart];
            }
            else
            {
                //数据来源于form/gird
                paramvalue = nvcols[FirstPart];
            }
            if (string.IsNullOrEmpty(paramvalue))
            {
                LogHelper<UIParameter>.Error("无法解析出UI参数[" + Name + "]");
                throw new AddinException("无法解析出UI参数[" + Name + "]");
            }
                

            if(string.IsNullOrEmpty(FirstPart))
            {
                var list = new List<string>();

                //直接取得是整个form/table的值则输出
                //判断取的值是否JSON，如果是JSON则转换成XML
                JObject xmlobj = JsonConvert.DeserializeObject(paramvalue) as JObject;                              
                if (xmlobj != null)
                {
                    //判断是否需要进行填充
                    var json = JsonParser.FillJsonStringWithPhid(paramvalue);
                    LogHelper<UIParameter>.Info("经过主键填充的参数是："+json);

                    var doc = JsonConvert.DeserializeXmlNode(json, ThirdPart);
                    if (doc == null)
                    {
                        LogHelper<UIParameter>.Error("无法将UI参数[" + Name + "]转换成XML格式"+json);
                        throw new AddinException("无法将UI参数[" + Name + "]转换成XML格式");
                    }
                    var xml = doc.InnerXml;
                    xml = xml.Replace("'", "''"); //进行单引号转义
                    list.Add(xml);
                }
                else
                {
                    list.Add(paramvalue);
                }                 
                values = list.ToArray();
                return values;
            }

            //判断是不是可以转换成JSON，如果可以转换则判断是不是集合
            JObject obj = JsonConvert.DeserializeObject(paramvalue) as JObject;

            if (obj != null)
            {
                
                RootType = JsonParser.GetRootType(obj);
                values = JsonParser.GetValues(obj, SecondPart, ThirdPart);
            }
            else
            {
                LogHelper<UIParameter>.Error("无法解析出UI参数[" + Name + "]");
                throw new AddinException("无法解析出UI参数[" + Name + "]");
            }

            if (values == null || values.Length == 0)
            {
                if(!JsonParser.IsEmptyData(obj))
                {
                    LogHelper<UIParameter>.Error("无法解析出UI参数[" + Name + "]");
                    //如果不是空的结果集无法解析出，则报错
                    throw new AddinException("无法解析出UI参数[" + Name + "]");
                }
                else
                {
                    //没有结果集
                    values = NoData();
                }
                
            }

            return values;
        }

        public string GetValue(int index)
        {
            if(values ==null)
            {
                GetValues();                
            }
            int i = index;
            if (i >= values.Length) i = 0;

            return values[i];

        }
        /// <summary>
        /// 空的结果集
        /// </summary>
        /// <returns></returns>
        private string[] NoData()
        {
            IList<string> data = new List<string>();
            data.Add(UIParameter.NO_DATA);
            return data.ToArray();
        }
    }
}
