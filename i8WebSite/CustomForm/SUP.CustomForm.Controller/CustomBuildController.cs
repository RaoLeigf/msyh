using System;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Enterprise3.WebApi.ApiControllerBase;
using NG3.Log.Log4Net;
using SUP.CustomForm.Builder;

namespace SUP.CustomForm.Controller
{
    [MethodExceptionFilterAttribute]
    public class CustomBuildController: CustomAFController
    {
        #region 日志相关
        private static new ILogger _logger = null;
        internal static new ILogger Logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = Log4NetLoggerFactory.Instance.CreateLogger(typeof(CustomBuildController), LogType.logoperation);
                }
                return _logger;
            }
        }
        #endregion

        //生成单据
        public string BuildCustomForm()
        {
            string fileId = System.Web.HttpContext.Current.Request.Params["fileid"];
            List<string> fileIdList = new List<string>();
            string status = string.Empty;

            var buildPara = new SUP.CustomForm.DataEntity.BuildParameter();
            var build = new Build();

            try
            {
                //支持单据批量发布，id用@分隔
                if (fileId.Contains("@") == true)
                {
                    fileIdList = fileId.Split(new string[] { "@" }, StringSplitOptions.None).ToList();
                }
                else
                {
                    fileIdList.Add(fileId);
                }
                
                foreach (string pformName in fileIdList)
                {
                    buildPara.Id = pformName;
                    status = build.BuildCustomForm(buildPara);
                }
            }
            catch (Exception e)
            {
                //日志输出
                Logger.Error("BuildCustomForm，生成表单报错：" + e.Message + "\r\n 单据id：" + buildPara.Id);
                return "表单生成失败：" + e.Message + "\r\n 表单id：" + buildPara.Id + "\r\n 位置：" + e.StackTrace;
            }

            return "ok";
        }

        //发布单据
        public string ReleaseCustomForm()
        {
            string copydll = System.Web.HttpContext.Current.Request.Params["copydll"];  //是0就不拷贝dll，其余全部拷贝
            string fileId = System.Web.HttpContext.Current.Request.Params["fileid"];

            string customSiteName = AppDomain.CurrentDomain.BaseDirectory;
            string ngSiteName = customSiteName.Replace("CustomWebSite", "NGWebSite");
            string sFile = string.Empty;
            string dDirectory = string.Empty;
            string dFile = string.Empty;
            string rtnStr = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(copydll))
                {
                    copydll = "1";
                }

                List<string> fileIdList = new List<string>();

                //支持单据批量发布，id用@分隔
                if (fileId.Contains("@") == true)
                {
                    fileIdList = fileId.Split(new string[] { "@" }, StringSplitOptions.None).ToList();
                }
                else
                {
                    fileIdList.Add(fileId);
                }

                foreach (string pformName in fileIdList)
                {
                    //建eformJs文件夹
                    dDirectory = ngSiteName + "NG3Resource\\js\\eformJs\\";
                    if (!Directory.Exists(Path.GetDirectoryName(dDirectory)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(dDirectory));
                    }

                    //\Areas\SUP\Views\pform0000003003List\pform0000003003List.cshtml
                    sFile = string.Format(customSiteName + "Areas\\SUP\\Views\\{0}List\\{0}List.cshtml", pformName);
                    dDirectory = string.Format(ngSiteName + "Areas\\SUP\\Views\\{0}List\\", pformName);
                    dFile = string.Format(ngSiteName + "Areas\\SUP\\Views\\{0}List\\{0}List.cshtml", pformName);
                    XCopy(sFile, dFile, dDirectory);

                    //\Areas\SUP\Views\pform0000003003Edit\pform0000003003Edit.cshtml
                    sFile = string.Format(customSiteName + "Areas\\SUP\\Views\\{0}Edit\\{0}Edit.cshtml", pformName);
                    dDirectory = string.Format(ngSiteName + "Areas\\SUP\\Views\\{0}Edit\\", pformName);
                    dFile = string.Format(dDirectory + "{0}Edit.cshtml", pformName);
                    XCopy(sFile, dFile, dDirectory);

                    //\NG3Resource\js\eformJs\pform0000003003List.js
                    sFile = string.Format(customSiteName + "NG3Resource\\js\\eformJs\\{0}List.js", pformName);
                    dFile = string.Format(ngSiteName + "NG3Resource\\js\\eformJs\\{0}List.js", pformName);
                    XCopy(sFile, dFile, "");

                    //\NG3Resource\js\eformJs\pform0000003003Edit.js
                    sFile = string.Format(customSiteName + "NG3Resource\\js\\eformJs\\{0}Edit.js", pformName);
                    dFile = string.Format(ngSiteName + "NG3Resource\\js\\eformJs\\{0}Edit.js", pformName);
                    XCopy(sFile, dFile, "");

                    //\NG3Resource\js\eformJs\pform0000003003Ext.js
                    sFile = string.Format(customSiteName + "NG3Resource\\js\\eformJs\\{0}Ext.js", pformName);
                    dFile = string.Format(ngSiteName + "NG3Resource\\js\\eformJs\\{0}Ext.js", pformName);
                    XCopy(sFile, dFile, "");

                    if (copydll != "0")
                    {
                        //三个dll合并到control一个dll
                        //\bin\SUP.CustomForm.pform0000000012.Controller.dll
                        sFile = string.Format(customSiteName + "bin\\SUP.CustomForm.{0}.Controller.dll", pformName);
                        dFile = string.Format(ngSiteName + "bin\\SUP.CustomForm.{0}.Controller.dll", pformName);
                        XCopy(sFile, dFile, "");
                    }
                }
            }
            catch (Exception e)
            {
                //日志输出
                Logger.Error("ReleaseCustomForm，发布表单报错：" + e.Message + "\r\n 单据：" + sFile);
                return "表单发布失败：" + e.Message + "\r\n 文件：" + sFile;
            }

            return "ok";
        }

        //源文件复制到目标文件夹
        private void XCopy(string sFile, string dFile, string dDirectory)
        {
            if (!string.IsNullOrEmpty(dDirectory))
            {
                if (!Directory.Exists(Path.GetDirectoryName(dDirectory)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(dDirectory));
                }
            }

            //判断要拷贝的文件是否存在
            if (!System.IO.File.Exists(sFile))
            {
                //日志输出
                Logger.Error("ReleaseCustomForm.XCopy(), 发布表单文件不存在：" + sFile);
                throw new Exception("发布的表单文件不存在");           
            }
            else
            {
                System.IO.File.Copy(sFile, dFile, true);
            }    
        }
    }
}
