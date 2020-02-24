using NG3.Report.Func.Core.Cfg;
using NG3.Report.Func.Core.Entity;
using NG3.Report.Func.Core.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NG3.Report.Func.Core.Supcan
{
    public static class XmlHelper
    {

        public static IList<FuncInfo>GetFuncs(string xml)
        {
            IList<FuncInfo> funcInfos = new List<FuncInfo>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            var xmlFuncs = xmlDoc.SelectNodes("/root/Functions/Function");
            foreach (XmlNode node in xmlFuncs)
            {

                XmlElement xmlFunc = node as XmlElement;

                FuncInfo funcInfo = new FuncInfo();
                string funcName = xmlFunc.GetAttribute("name");//函数名称
                funcInfo.Name = funcName;

                //函数参数
                var xmlFuncParams = xmlFunc.ChildNodes;
                int index = 0;
                IList<FuncParameter> paramList = new List<FuncParameter>(); 
                foreach (var p in xmlFuncParams)
                {
                    XmlElement xmlParam = p as XmlElement;

                    FuncParameter funcParam = new FuncParameter();
                    funcParam.Value = xmlParam.InnerText;
                    funcParam.ParamType = xmlParam.GetAttribute("dataType");
                    funcParam.FuncName = funcName;
                    funcParam.Index = index;

                    paramList.Add(funcParam);
                    
                    index++;
                }
                funcInfo.Paras = paramList;

                funcInfos.Add(funcInfo);

            }
            return funcInfos;
        }

        /// <summary>
        /// 
        ///生成函数返回
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        public static string GetFuncResult(IList<FuncCalcResult> results)
        {
            var xmlDoc = new XmlDocument();
            var xmlRoot = xmlDoc.CreateElement("Root");
            var xmlFuncs = xmlDoc.CreateElement("Functions");
            xmlRoot.AppendChild(xmlFuncs);
            xmlDoc.AppendChild(xmlRoot);

            results.ToList().ForEach(p =>
            {
                var xmlFunc = xmlDoc.CreateElement("Function");
                if (p.Status  == EnumFuncActionStatus.Success)
                { 
                    if(p.ResultDataType == EnumFuncDataType.Number)
                    {
                        xmlFunc.InnerText = p.Value;
                    }                   
                    else
                    {
                        //字符串类型，supcan要求返回时加单引号
                        xmlFunc.InnerText = "'"+p.Value+"'";
                    }
                }
                else
                {

                    var xmlFault = xmlDoc.CreateElement("fault");
                    xmlFunc.AppendChild(xmlFault);

                    var xmlFaultcode = xmlDoc.CreateElement("faultcode");
                    xmlFaultcode.InnerText = p.Fault.FaultCode;
                    xmlFunc.AppendChild(xmlFaultcode);

                    var xmlFaultString = xmlDoc.CreateElement("faultstring");
                    xmlFaultString.InnerText = p.Fault.Faultstring;
                    xmlFunc.AppendChild(xmlFaultString);

                    var xmldetail = xmlDoc.CreateElement("detail");
                    xmldetail.InnerText = p.Fault.Detail;
                    xmlFunc.AppendChild(xmldetail);

                    var xmlReason = xmlDoc.CreateElement("reason");
                    xmlReason.InnerText = p.Fault.Reason;
                    xmlFunc.AppendChild(xmlReason);

                    var xmlFaultactor = xmlDoc.CreateElement("faultactor");
                    xmlFaultactor.InnerText = p.Fault.Faultactor;
                    xmlFunc.AppendChild(xmlFaultactor);
                }
                xmlFuncs.AppendChild(xmlFunc);
            });


            return  ConvertXmlToString(xmlDoc);
        }

        /// <summary>
        /// 生成supcan报表控件函数的描述信息
        /// </summary>
        /// <returns></returns>
        public static string SupcanFuncsXmlDescriptor(string catalogs)
        {
            if (string.IsNullOrEmpty(catalogs)) return string.Empty;

            FuncConfigure.LoadFuncs(catalogs);
            string[] catalog = catalogs.Split(',');

            IList<DropDownList> dropLists = new List<DropDownList>();

            var xmlDoc = new XmlDocument();

            var xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", "");
            xmlDoc.AppendChild(xmlDecl);

            var xmlRoot = xmlDoc.CreateElement("root");
            var xmlFuncs = xmlDoc.CreateElement("functions");
            xmlRoot.AppendChild(xmlFuncs);
            xmlDoc.AppendChild(xmlRoot);
            
            foreach (var s in catalog)
            {
                //取得所有的函数
                var funcList = FuncCache.GetFuncInfoMetaDataByCatalog(s);
                if (funcList == null) continue;
                //catalogid对应的目录名
                if(funcList.Count >0)
                {
                    string catalogName = funcList[0].Module;
                    //输出XML
                    var xmlCatalog = xmlDoc.CreateElement("catagory");
                    xmlCatalog.SetAttribute("name", catalogName);
                    xmlFuncs.AppendChild(xmlCatalog);

                    funcList.ToList().ForEach(p => {
                        var xmlFunc = xmlDoc.CreateElement("function");
                        xmlFunc.SetAttribute("name",p.Name);
                        xmlCatalog.AppendChild(xmlFunc);

                        var xmlUsage = xmlDoc.CreateElement("usage");
                        xmlUsage.InnerText = p.Usage;
                        xmlFunc.AppendChild(xmlUsage);

                        var xmlDetail = xmlDoc.CreateElement("detail");
                        xmlDetail.InnerText = p.Detail;
                        xmlFunc.AppendChild(xmlDetail);

                        //函数参数
                        p.Paras.ToList().ForEach(parm =>
                        {
                            var xmlParam = xmlDoc.CreateElement("para");
                            xmlParam.SetAttribute("datatype", parm.ParamType);
                            xmlParam.SetAttribute("droplistID",parm.DroplistCode);
                            xmlParam.SetAttribute("edittype",parm.EditType);
                            xmlParam.InnerText = parm.Name;

                            //
                            xmlFunc.AppendChild(xmlParam);
                        });

                        var xmlExample = xmlDoc.CreateElement("example");
                        xmlExample.InnerText = p.Example;
                        xmlFunc.AppendChild(xmlExample);                       

                    });
                }

               //不同的模块的函数，下拉列表也有可能是一样的
                var dropList = FuncCache.GetDropDownList(s);
                dropList.ToList().ForEach(p => { dropLists.Add(p); });
                
            }
            //输出dropDwonList
            var xmlDroplists = xmlDoc.CreateElement("DropLists");
            xmlRoot.AppendChild(xmlDroplists);


            dropLists.Distinct().ToList().ForEach(p=>
            {
                var xmlDropList = xmlDoc.CreateElement("DropList");
                xmlDropList.SetAttribute("DisplayCol",p.DisplayCol);
                xmlDropList.SetAttribute("DataCol", p.DataCol);
                xmlDropList.SetAttribute("dataUrl", p.DataUrl);
                xmlDropList.SetAttribute("treelist", p.TreelistUrl);
                xmlDropList.SetAttribute("id", p.Id);

                //多个字符串参数解析
                foreach (var key in p.ParamsDic.AllKeys)
                {
                    xmlDropList.SetAttribute(key, p.ParamsDic[key]);
                }               
                xmlDroplists.AppendChild(xmlDropList);
            });

            return ConvertXmlToString(xmlDoc);
        }

        /// <summary>
        /// XML转换成字符串
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static string ConvertXmlToString(XmlDocument doc)
        {
            if (doc == null) return string.Empty;

            string xmlstring = string.Empty;
            using (MemoryStream stream = new MemoryStream())
            {
                XmlTextWriter writer = new XmlTextWriter(stream,Encoding.UTF8);
                writer.Formatting = Formatting.Indented;
                doc.Save(writer);

                using (StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8))
                {
                    stream.Position = 0;
                    xmlstring = sr.ReadToEnd();

                }
            }                
            return xmlstring;
        }
    }
}
