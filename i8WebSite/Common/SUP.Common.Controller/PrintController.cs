using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NG3.Web.Controller;
using System.Web.Mvc;
using System.Data;
using System.Xml;
using System.IO;
using SUP.Common.Base;
using NG3;
using NG3.Aop.Transaction;
using SUP.Common.Facade;
using SUP.Common.DataEntity;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using NG3.Web.Mvc;
using System.Web.SessionState;

namespace SUP.Common.Controller
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class PrintController : AFController
    {
        PrintFacade proxy;
        // GET: /Print/
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public PrintController()
        {
            proxy = AopObjectProxy.GetObject<PrintFacade>(new PrintFacade());
        }
        #endregion

        #region 公共常量、变量及私有方法
        #region 常量
        private const string supCanTemplate = "~/ng3/reports/tmpConfig"; //临时模板路径
        private const string supCanSysTemplate = "~/ng3/reports/sysConfig"; //系统模板路径
        private const string supCanUserTemplate = "~/ng3/reports/userConfig"; //用户模板路径
        private const string supCanPdfTemplate = "~/ng3/reports/pdfConfig";     //pdf 打印模板

        private const string currDataFormat = "=now(&quot;制单日期：%Y年%m月%d日&quot;)"; //报表的制单日期

        /// <summary>
        /// 默认打印设置
        /// </summary>
        private const string defaultPrintPage = "<PrintPage><Paper><Margin left=\"10\" top=\"20\" right=\"10\" bottom=\"20\"/></Paper><Page isIgnoreValidBorder=\"true\"><Page-break><FixedRowCols headerRows=\"1\"/></Page-break><PageCode><Font faceName=\"宋体\" charSet=\"134\" height=\"-14\" weight=\"400\"/></PageCode></Page></PrintPage>";

        /// <summary>
        /// xml头信息
        /// </summary>
        private const string defaultHeadPage = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?><Report><WorkSheet name=\"HEADTITLEFLG\" isDefaultPrint=\"true\"><Properties><BackGround bgColor=\"#FFFFFF\"/><DefaultTD><TD fontIndex=\"0\" textColor=\"#000000\" leftBorder=\"0\" topBorder=\"0\" leftBorderColor=\"#C0C0C0\" leftBorderStyle=\"solid\" topBorderColor=\"#C0C0C0\" topBorderStyle=\"solid\" decimal=\"2\" align=\"center\" vAlign=\"middle\" transparent=\"true\" isDataSource=\"false\" isProtected=\"false\" isThousandSeparat=\"true\" isRound=\"true\" isPrint=\"true\"/></DefaultTD><Other isShowZero=\"false\" LineDistance=\"0\" isRowHeightAutoExtendAble=\"true\"/></Properties><Fonts><Font faceName=\"微软雅黑\" height=\"-15\" weight=\"400\"/><Font faceName=\"微软雅黑\" charSet=\"134\" height=\"-27\" weight=\"700\"/><Font faceName=\"微软雅黑\" height=\"-15\" weight=\"700\"/><Font faceName=\"微软雅黑\" height=\"-13\" weight=\"400\"/><Font faceName=\"宋体\" charSet=\"134\" height=\"-14\" weight=\"700\"/><Font faceName=\"宋体\" charSet=\"134\" height=\"-14\" weight=\"400\"/></Fonts><Table><Col width=\"14\"/>";
        #endregion

        #region 变量
        private string allMoudleType = ""; //模块
        #endregion

        #region 私有方法
        /// <summary>
        /// 设置全局变量
        /// </summary>
        private void SetPublicValue()
        {
            if (!string.IsNullOrEmpty(allMoudleType)) { return; }
            DataTable dt = proxy.GetModules();
            string sFileUrl = Pub.HostPath + System.Web.HttpContext.Current.Request.ApplicationPath;
            sFileUrl = sFileUrl + "/Product.xml";
            DataSet ds = new DataSet();
            ds.ReadXml(sFileUrl);
            DataTable dtSuit = ds.Tables["SuitInfo"];
            DataRow[] drs = dtSuit.Select("code='PUB'");          
            StringBuilder keys = new StringBuilder(), values = new StringBuilder();
            if (drs != null && drs.Length > 0)
            {
                if (keys.Length > 0)
                {
                    keys.Append(",");
                    values.Append(",");
                }
                keys.Append(drs[0]["code"].ToString());
                values.Append(drs[0]["name"].ToString());
                
            }
            DataRow[] rows = dt.Select("ismodule=1");
            foreach (DataRow dr in rows)
            {
                if (keys.Length > 0)
                {
                    keys.Append(",");
                    values.Append(",");
                }
                keys.Append(dr["id"].ToString());
                values.Append(dr["name"].ToString());                
            }
            allMoudleType = keys.ToString();
            ViewBag.moudleNos = allMoudleType;
            ViewBag.moudleNames = values.ToString();           
        }
        #endregion
        #endregion

        #region Print--------------打印当前页列表数据
        /// <summary>
        /// Print
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult Index()
        {
            ViewBag.PageId = System.Web.HttpContext.Current.Request.Params["pageid"];
            ViewBag.FileName = System.Web.HttpContext.Current.Request.Params["fileName"];
            ViewBag.TempDir = supCanTemplate;
            return View("Print");
        }

        /// <summary>
        /// 直接生成打印需要的全部XML文件（不需要在客户端填充数据源）
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        [ValidateInput(false)]
        public string CreateXml()
        {
            string re_json = "";
            string mycls = System.Web.HttpContext.Current.Request.Form["mycls"];
            string mydata = System.Web.HttpContext.Current.Request.Form["mydata"];
            string headtitle = System.Web.HttpContext.Current.Request.Form["printtitle"];
            DataTable myclsdt = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(mycls);
            DataTable mydatadt = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(mydata);
            XmlDocument doc = new XmlDocument();
            StringBuilder MasksXml = new StringBuilder();
            int N = myclsdt.Rows.Count + 2;
            int seqIndex = 0;
            #region 构建Xml
            System.Text.StringBuilder xmlSB = new System.Text.StringBuilder();
            xmlSB.Append(defaultHeadPage.Replace("HEADTITLEFLG", headtitle));
            foreach (DataRow dr in myclsdt.Rows)
            {
                xmlSB.Append("<Col width=\"" + dr["width"] + "\"/>");
            }
            xmlSB.Append("<Col width=\"15\"/>");

            xmlSB.Append("<TR height=\"81\" sequence=\"" + seqIndex++ + "\">")
                 .Append("<TD fontIndex=\"1\">" + headtitle + "</TD>");
            for (int i = 0; i < N - 1; i++)
            {
                xmlSB.Append("<TD fontIndex=\"1\"/>");
            }
            xmlSB.Append("</TR>");

            xmlSB.Append("<TR height=\"24\" sequence=\"" + seqIndex++ + "\">").Append("<TD fontIndex=\"2\" datatype=\"1\"/>");
            foreach (DataRow dr in myclsdt.Rows)
            {
                xmlSB.Append("<TD fontIndex=\"2\" bgColor=\"#E0E0E0\" leftBorder=\"1\" topBorder=\"1\" transparent=\"false\" datatype=\"1\">" + dr["text"].ToString() + "</TD>");
            }
            xmlSB.Append("<TD fontIndex=\"2\" leftBorder=\"1\"/>").Append("</TR>");

            int gridHight_Tr = 24;
            string picColumnHeight = System.Web.HttpContext.Current.Request.Form["gridcls_Height"];
            if (!string.IsNullOrEmpty(picColumnHeight))
            {
                gridHight_Tr = int.Parse(picColumnHeight);
                if (MasksXml.Length < 1)
                {
                    MasksXml.Append("<Masks><mask id=\"1\" datatype=\"1\">picture()</mask></Masks>");
                }
            }

            foreach (DataRow dr in mydatadt.Rows)
            {
                xmlSB.Append("<TR height=\"" + gridHight_Tr + "\" sequence=\"" + seqIndex++ + "\">").Append("<TD fontIndex=\"3\" align=\"left\" datatype=\"1\"/>");
                foreach (DataRow _dr in myclsdt.Rows)
                {
                    xmlSB.Append("<TD fontIndex=\"3\" textColor=\"#000000\" leftBorder=\"1\" topBorder=\"1\"" + (_dr["picflg"].ToString() == "1" ? " maskid=\"1\"" : "") + " align=\"left\" datatype=\"1\" " + (dr[_dr["dataIndex"].ToString()].ToString().Length > 0 ? (">" + dr[_dr["dataIndex"].ToString()].ToString() + "</TD>") : "/>"));
                }
                xmlSB.Append("<TD fontIndex=\"3\" leftBorder=\"1\" align=\"left\"/>").Append("</TR>");
            }

            xmlSB.Append("<TR height=\"24\" sequence=\"" + seqIndex++ + "\">").Append("<TD align=\"left\"/>");  //插入空行
            for (int i = 0; i < N - 2; i++)
            {
                xmlSB.Append("<TD topBorder=\"1\" align=\"left\"/>");
            }
            xmlSB.Append("<TD align=\"left\"/>").Append("</TR>");

            xmlSB.Append("<TR height=\"26\" sequence=\"" + seqIndex++ + "\">").Append("<TD align=\"left\"/>")
                 .Append("<TD align=\"right\" formula=\"" + currDataFormat + "\"></TD>");  //插入制单日期
            for (int i = 0; i < N - 2; i++)
            {
                xmlSB.Append("<TD align=\"right\"/>");
            }
            xmlSB.Append("</TR>");

            xmlSB.Append("<TR height=\"13\" sequence=\"" + seqIndex++ + "\">").Append("<TD align=\"left\"/>");
            for (int i = 0; i < N - 2; i++)
            {
                xmlSB.Append("<TD align=\"left\"/>");
            }

            xmlSB.Append("<TD align=\"left\"/>").Append("</TR>").Append("</Table>");
            xmlSB.Append("<Merges>")
                 .Append("<Range row1=\"0\" col1=\"0\" row2=\"0\" col2=\"" + (N - 1).ToString() + "\"/>")
                 .Append("<Range row1=\"" + (seqIndex - 2) + "\" col1=\"1\" row2=\"" + (seqIndex - 2) + "\" col2=\"" + (N - 2).ToString() + "\"/>")
                 .Append("</Merges>");
            xmlSB.Append(GetPrintSetup(System.Web.HttpContext.Current.Request.Form["ctype"]));
            xmlSB.Append("</WorkSheet>").Append(MasksXml.ToString()).Append("</Report>");
            doc.LoadXml(xmlSB.ToString());
            #endregion
            string fname = "TempReport" + DateTime.Now.ToString("yyyyMMdd") + ".xml";
            string tmpDir = System.Web.HttpContext.Current.Server.MapPath(supCanTemplate);
            string XmlFileName = Path.Combine(tmpDir, fname);
            try
            {
                if (!Directory.Exists(tmpDir))
                {
                    Directory.CreateDirectory(tmpDir);
                }
                doc.Save(XmlFileName);
                re_json = "{\"status\":\"ok\",\"xmlName\":\"" + supCanTemplate + "/" + fname + "\"}";
            }
            catch (Exception ex)
            {
                re_json = "{\"status\":\"" + ex.Message + "\"}";
            }
            return re_json;
        }
        #endregion

        #region PrintEx------------打印所有页列表数据
        /// <summary>
        /// PrintEx
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult PrintEx()
        {
            ViewBag.PageId = System.Web.HttpContext.Current.Request.Params["pageid"];
            ViewBag.FileName = System.Web.HttpContext.Current.Request.Params["fileName"];
            ViewBag.TempDir = supCanTemplate;
            return View("PrintEx");
        }

        /// <summary>
        /// 获取连续打印相关信息
        /// </summary>
        /// <returns></returns>
        public string GetPrintStoreInfo()
        {
            string filename = System.Web.HttpContext.Current.Request.Form["filename"];
            string dname = filename + ".url.txt";
            string tmpDir = System.Web.HttpContext.Current.Server.MapPath(supCanTemplate);
            string dataFullPath = Path.Combine(tmpDir, dname);
            return System.IO.File.ReadAllText(dataFullPath);

        }

        /// <summary>
        /// 生成打印需要的Xml文件（需要客户端SetSource数据源）
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public string CreateXmlMould()
        {
            string re_json = "";
            int seqIndex = 0;
            string mycls = System.Web.HttpContext.Current.Request.Form["mycls"];
            string headtitle = System.Web.HttpContext.Current.Request.Form["printtitle"];
            DataTable myclsdt = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(mycls);
            int N = myclsdt.Rows.Count + 2;
            XmlDocument doc = new XmlDocument();
            StringBuilder MasksXml = new StringBuilder();
            #region 构建Xml
            System.Text.StringBuilder xmlSB = new System.Text.StringBuilder();
            xmlSB.Append(defaultHeadPage.Replace("HEADTITLEFLG", headtitle));
            foreach (DataRow dr in myclsdt.Rows)
            {
                xmlSB.Append("<Col width=\"" + dr["width"] + "\"/>");
            }
            xmlSB.Append("<Col width=\"15\"/>");

            xmlSB.Append("<TR height=\"81\" sequence=\"" + seqIndex++ + "\">")
                 .Append("<TD fontIndex=\"1\">" + headtitle + "</TD>");
            for (int i = 0; i < N - 1; i++)
            {
                xmlSB.Append("<TD fontIndex=\"1\"/>");
            }
            xmlSB.Append("</TR>");

            xmlSB.Append("<TR height=\"24\" sequence=\"" + seqIndex++ + "\">").Append("<TD fontIndex=\"2\" datatype=\"1\"/>");
            foreach (DataRow dr in myclsdt.Rows)
            {
                xmlSB.Append("<TD fontIndex=\"2\" bgColor=\"#E0E0E0\" leftBorder=\"1\" topBorder=\"1\" transparent=\"false\" datatype=\"1\">" + dr["text"].ToString() + "</TD>");
            }
            xmlSB.Append("<TD fontIndex=\"2\" leftBorder=\"1\"/>").Append("</TR>");

            int gridHight_Tr = 24;
            string picColumnHeight = System.Web.HttpContext.Current.Request.Form["gridcls_Height"];
            if (!string.IsNullOrEmpty(picColumnHeight))
            {
                gridHight_Tr = int.Parse(picColumnHeight);
                if (MasksXml.Length < 1)
                {
                    MasksXml.Append("<Masks><mask id=\"1\" datatype=\"1\">picture()</mask></Masks>");
                }
            }
            xmlSB.Append("<TR height=\"" + gridHight_Tr + "\" sequence=\"" + seqIndex++ + "\">").Append("<TD fontIndex=\"3\" align=\"left\" datatype=\"1\"/>")
                 .Append("<TD fontIndex=\"3\" textColor=\"#000000\" leftBorder=\"1\" topBorder=\"1\" align=\"left\"" + (myclsdt.Rows[0]["picflg"].ToString() == "1" ? " maskid=\"1\"" : "") + " datatype=\"1\" formula=\"=datarow(&apos;ds1\\jsonobject&apos;)\"></TD>");
            for (int i = 0; i < N - 3; i++)
            {
                xmlSB.Append("<TD fontIndex=\"3\" textColor=\"#000000\" leftBorder=\"1\" topBorder=\"1\" align=\"left\" isDataSource=\"true\"" + (myclsdt.Rows[i + 1]["picflg"].ToString() == "1" ? " maskid=\"1\"" : "") + "  datatype=\"1\"></TD>");
            }
            xmlSB.Append("<TD fontIndex=\"3\" leftBorder=\"1\" align=\"left\"/>").Append("</TR>");

            xmlSB.Append("<TR height=\"24\" sequence=\"" + seqIndex++ + "\">").Append("<TD align=\"left\"/>");  //插入空行
            for (int i = 0; i < N - 2; i++)
            {
                xmlSB.Append("<TD topBorder=\"1\" align=\"left\"/>");
            }
            xmlSB.Append("<TD align=\"left\"/>").Append("</TR>");

            xmlSB.Append("<TR height=\"26\" sequence=\"" + seqIndex++ + "\">").Append("<TD align=\"left\"/>")
                 .Append("<TD align=\"right\" formula=\"" + currDataFormat + "\"></TD>");  //插入制单日期
            for (int i = 0; i < N - 2; i++)
            {
                xmlSB.Append("<TD align=\"right\"/>");
            }
            xmlSB.Append("</TR>");

            xmlSB.Append("<TR height=\"13\" sequence=\"" + seqIndex++ + "\">").Append("<TD align=\"left\"/>");
            for (int i = 0; i < N - 2; i++)
            {
                xmlSB.Append("<TD align=\"left\"/>");
            }

            xmlSB.Append("<TD align=\"left\"/>").Append("</TR>").Append("</Table>");
            xmlSB.Append("<Merges>")
                 .Append("<Range row1=\"0\" col1=\"0\" row2=\"0\" col2=\"" + (N - 1).ToString() + "\"/>")
                 .Append("<Range row1=\"" + (seqIndex - 2) + "\" col1=\"1\" row2=\"" + (seqIndex - 2) + "\" col2=\"" + (N - 2).ToString() + "\"/>")
                 .Append("</Merges>");
            xmlSB.Append(GetPrintSetup(System.Web.HttpContext.Current.Request.Form["ctype"]));
            xmlSB.Append("</WorkSheet>");
            xmlSB.Append("<DataSources Version=\"255\" isAutoCalculateWhenOpen=\"false\" isSaveCalculateResult=\"false\"><DataSource type=\"4\">")
                 .Append("<Data><ID>ds1</ID><Version>2</Version><Type>4</Type><TypeMeaning>JSON</TypeMeaning><Source></Source><Memo></Memo>")
                 .Append("<XML_RecordAble_Nodes><Node><name alias=\"\">jsonobject</name></Node></XML_RecordAble_Nodes>");
            xmlSB.Append("<Columns>");
            int sequence = 1;
            foreach (DataRow dr in myclsdt.Rows)
            {
                xmlSB.Append("<Column><name>jsonobject\\" + dr["dataIndex"].ToString() + "</name><text>" + dr["text"].ToString() + "</text><type>string</type><visible>true</visible><sequence>" + sequence++ + "</sequence></Column>");
            }
            xmlSB.Append("</Columns></Data></DataSource></DataSources>").Append(MasksXml.ToString()).Append("</Report>");
            doc.LoadXml(xmlSB.ToString());
            #endregion

            string fname = "TempReportMould" + DateTime.Now.ToString("yyyyMMdd") + ".xml";
            string tmpDir = System.Web.HttpContext.Current.Server.MapPath(supCanTemplate);
            string XmlFileName = Path.Combine(tmpDir, fname);
            try
            {
                if (!Directory.Exists(tmpDir))
                {
                    Directory.CreateDirectory(tmpDir);
                }
                doc.Save(XmlFileName);
                re_json = "{\"status\":\"ok\",\"xmlName\":\"" + supCanTemplate + "/" + fname + "\"}";
            }
            catch (Exception ex)
            {
                re_json = "{\"status\":\"" + ex.Message + "\"}";
            }
            return re_json;
        }

        #endregion

        /// <summary>
        /// 检测打印相关文件是否生成
        /// </summary>
        /// <returns></returns>
        public string CheckPrintFileCreated()
        {
            string filename = System.Web.HttpContext.Current.Request.Form["filename"];
            string dname = filename + ".txt";
            string tmpDir = System.Web.HttpContext.Current.Server.MapPath(supCanTemplate);
            string dataFullPath = Path.Combine(tmpDir, dname);

            if (System.IO.File.Exists(dataFullPath))
            {
                return "{\"status\":\"ok\"}";
            }
            else     
            {
                return "{\"status\":\"false\"}";
            }  
        }       

        /// <summary>
        /// 生成打印xml及data文件
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        public string CreatePrintInfo()
        {             
            string printId = System.Web.HttpContext.Current.Request.Form["printid"];              
            string filename = System.Web.HttpContext.Current.Request.Form["filename"];
            string data = System.Web.HttpContext.Current.Request.Form["data"];
            string gridprintmode = System.Web.HttpContext.Current.Request.Form["gridprintmode"];
            string fname = filename + ".xml";//mTitle + DateTime.Now.ToString("yyyyMMdd") + ".xml";
            string dname = filename + ".txt";
         
            string tmpDir = System.Web.HttpContext.Current.Server.MapPath(supCanTemplate);
            if (!Directory.Exists(tmpDir))
            {
                Directory.CreateDirectory(tmpDir);
            }
            string xmlFileName = Path.Combine(tmpDir, fname);
            string dataFullPath = Path.Combine(tmpDir, dname);  
            ClearTmp(7);
            if (!string.IsNullOrEmpty(gridprintmode))
            {
                string gridprintstore = System.Web.HttpContext.Current.Request.Form["gridprintstore"];
                string urlpath = Path.Combine(tmpDir, filename + ".url.txt");
                System.IO.File.WriteAllText(urlpath, gridprintstore);
            }  
            if (!string.IsNullOrEmpty(data))
            {
                System.IO.File.WriteAllText(dataFullPath, data); 
            }
            else
            {
                System.IO.File.WriteAllText(dataFullPath, "{}");
            }
            if (!string.IsNullOrEmpty(printId))
            {
                string printFile = proxy.GetTemplateById(printId, System.Web.HttpContext.Current.Server.MapPath(supCanTemplate), xmlFileName);
                return "{\"status\":\"ok\",\"xmlName\":\"" + supCanTemplate + "/" + printFile + "\",\"dataName\":\"" + supCanTemplate + "/" + dname + "\"}";
            }
            string reJson = "";
            try
            {
                string mTitle = System.Web.HttpContext.Current.Request.Form["mtitle"];
                string hasGrid = System.Web.HttpContext.Current.Request.Form["hasgrid"];
                string hasForm = System.Web.HttpContext.Current.Request.Form["hasform"];
                string dTitle = System.Web.HttpContext.Current.Request.Form["dTitle"];
                int oW = int.Parse(System.Web.HttpContext.Current.Request.Form["width"]);
                string buskey = System.Web.HttpContext.Current.Request.Form["buskey"];
                StringBuilder GraphicObjects = new StringBuilder();
                StringBuilder TableObjects = new StringBuilder();
                StringBuilder MergCells = new StringBuilder();
                StringBuilder NodeXml = new StringBuilder();
                StringBuilder ColumnsXml = new StringBuilder();
                StringBuilder MasksXml = new StringBuilder();
                int gObjectTrHeight = 30, gObjectTdWidth = oW, gObjectfontIndex_t = 4, gObjectfontIndex_v = 5, seqIndex = 1;
                int N = 6, gridCount = 0;
                if (hasForm == "true")
                {
                    #region hasForm
                    string mF = System.Web.HttpContext.Current.Request.Form["mform"];
                    int rows = int.Parse(System.Web.HttpContext.Current.Request.Form["rows"]);
                    int cols = int.Parse(System.Web.HttpContext.Current.Request.Form["cols"]);
                    string imgStr = System.Web.HttpContext.Current.Request.Form["hasImg"];
                    bool hasImg = string.IsNullOrEmpty(imgStr) ? false : bool.Parse(imgStr);
                    DataTable mForm = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(mF);
                    gObjectTrHeight *= rows;
                    GraphicObjects.Append("<GraphicObjects>");
                    gObjectTdWidth = hasImg ? (gObjectTdWidth - int.Parse(System.Web.HttpContext.Current.Request.Form["imgWidth"]) - 15) : gObjectTdWidth;
                    int tmpX = gObjectTdWidth;
                    gObjectTdWidth = gObjectTdWidth / cols;
                    int x1 = 0, x2 = 0, y1 = 0, y2 = 0;
                    int r_col = 0, r_index = 0, seq = 1;
                    if (!string.IsNullOrEmpty(buskey))
                    {
                        if (mForm.Select("name='" + buskey + "'").Length <= 0)
                        {

                            ColumnsXml.Append("<Column><name>" + buskey + "</name><text>主键</text><type>string</type><visible>true</visible><sequence>" + seq++ + "</sequence></Column>");
                        }
                    }
                    for (int i = 0; i < mForm.Rows.Count; i++)
                    {
                        DataRow dr = mForm.Rows[i];
                        int colspan = int.Parse(dr["colspan"].ToString());
                        if (r_col + colspan > cols)
                        {
                            r_col = 0;
                            r_index++;
                        }
                        if (r_col == 0)
                        {
                            x1 = 20;
                        }
                        else
                        {
                            x1 += gObjectTdWidth * int.Parse(mForm.Rows[i - 1]["colspan"].ToString());
                        }
                        r_col += colspan;
                        y1 = 65 + r_index * 30;
                        x2 = x1 + 70;
                        y2 = y1 + 26;
                        seq = seq + i;
                        GraphicObjects.Append("<TextBox transparent=\"true\" fontIndex=\"" + gObjectfontIndex_t + "\" align=\"left\" vAlign=\"middle\" text=\"" + dr["label"] + ":\" datatype=\"1\">")
                            .Append("<Rect x1=\"" + x1 + "\" y1=\"" + y1 + "\" x2=\"" + x2 + "\" y2=\"" + y2 + "\"/></TextBox>");
                        GraphicObjects.Append("<TextBox transparent=\"true\" fontIndex=\"" + gObjectfontIndex_v + "\" align=\"left\" vAlign=\"middle\" formula=\"=data(&apos;ds1&apos;, 1, &apos;" + dr["name"] + "&apos;)\" datatype=\"1\">")
                            .Append("<Rect x1=\"" + (x2 + 5) + "\" y1=\"" + y1 + "\" x2=\"" + (x2 + gObjectTdWidth * colspan - 60) + "\" y2=\"" + y2 + "\"/></TextBox>");
                        ColumnsXml.Append("<Column><name>" + dr["name"] + "</name><text>" + dr["label"].ToString() + "</text><type>string</type><visible>true</visible><sequence>" + seq + "</sequence></Column>");
                    }
                    if (hasImg) //打印表单图片对象，可以是多个
                    {
                        DataTable imgObj = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(System.Web.HttpContext.Current.Request.Form["imgObj"]);
                        foreach (DataRow dr in imgObj.Rows)
                        {
                            int imgw = int.Parse(dr["width"].ToString());
                            GraphicObjects.Append("<TextBox fontIndex=\"" + gObjectfontIndex_v + "\" align=\"center\" vAlign=\"middle\" formula=\"=data(&apos;ds1&apos;, 1, &apos;" + dr["name"] + "&apos;)\" maskid=\"1\" datatype=\"1\">")
                                          .Append("<Rect x1=\"" + tmpX + "\" y1=\"66\" x2=\"" + (tmpX + imgw) + "\" y2=\"" + y2 + "\"/></TextBox>");
                            tmpX += imgw + 2;
                            ColumnsXml.Append("<Column><name>" + dr["name"] + "</name><text>" + dr["title"].ToString() + "</text><type>string</type><visible>true</visible><sequence>" + (++seq) + "</sequence></Column>");
                        }
                        MasksXml.Append("<Masks><mask id=\"1\" datatype=\"1\">picture()</mask></Masks>");
                    }
                    GraphicObjects.Append("</GraphicObjects>");
                    NodeXml.Append("<Node><name alias=\"\"/></Node>");
                    #endregion
                }
                else
                {
                    if (!string.IsNullOrEmpty(buskey))
                    {
                        NodeXml.Append("<Node><name alias=\"\"/></Node>");
                        ColumnsXml.Append("<Column><name>" + buskey + "</name><text>主键</text><type>string</type><visible>true</visible><sequence>1</sequence></Column>");
                    }
                }
                if (hasGrid == "true")
                {
                    #region hasGrid
                    string[] dT = null;
                    if (!string.IsNullOrEmpty(dTitle))
                    {
                        dT = dTitle.Split(new string[] { "$jkd$" }, StringSplitOptions.None);
                    }
                    if (hasForm == "true")
                    {
                        seqIndex = 2;
                    }
                    N = int.Parse(System.Web.HttpContext.Current.Request.Form["maxcols"]) + 2;
                    string grid = System.Web.HttpContext.Current.Request.Form["gridcls_0"];
                    while (!string.IsNullOrEmpty(grid))
                    {
                        DataTable gridTable = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(grid);
                        int gridN = gridTable.Rows.Count;
                        if (gridCount == 0)
                        {
                            TableObjects.Append("<TR height=\"20\" sequence=\"" + seqIndex++ + "\">").Append("<TD align=\"left\"/>");  //插入空行
                            for (int i = 0; i < N - 2; i++)
                            {
                                TableObjects.Append("<TD topBorder=\"0\" align=\"left\"/>");
                            }
                            TableObjects.Append("<TD align=\"left\"/>").Append("</TR>");
                        }
                        string alias = "gridcls_" + gridCount;
                        if (null != dT && dT.Length > gridCount + 1)
                        {
                            string t = dT[gridCount + 1];
                            if (t.Length > 0)
                            {
                                alias = t;
                                TableObjects.Append("<TR height=\"23\" sequence=\"" + seqIndex++ + "\">").Append("<TD align=\"left\"/>")
                                    .Append("<TD fontIndex=\"2\" align=\"left\">" + t + "</TD>");  //插入副标题
                                for (int i = 0; i < N - 2; i++)
                                {
                                    TableObjects.Append("<TD align=\"left\"/>");
                                }
                                TableObjects.Append("</TR>");
                                MergCells.Append("<Range row1=\"" + (seqIndex - 1) + "\" col1=\"1\" row2=\"" + (seqIndex - 1) + "\" col2=\"" + (N - 2).ToString() + "\"/>");
                            }
                        }
                        TableObjects.Append("<TR height=\"24\" sequence=\"" + seqIndex++ + "\">").Append("<TD fontIndex=\"2\" datatype=\"1\"/>");
                        int tmpsequence = 1;
                        foreach (DataRow dr in gridTable.Rows)
                        {
                            TableObjects.Append("<TD fontIndex=\"2\" bgColor=\"#E0E0E0\" leftBorder=\"1\" topBorder=\"1\" transparent=\"false\" datatype=\"1\">" + dr["text"].ToString() + "</TD>");
                            ColumnsXml.Append("<Column><name>Grids\\" + "gridcls_" + gridCount + "\\jsonobject\\" + dr["dataIndex"] + "</name><text>" + dr["text"].ToString() + "</text><type>string</type><visible>true</visible><sequence>" + tmpsequence++ + "</sequence></Column>");
                        }
                        for (int i = 0; i < N - gridN - 1; i++)
                        {
                            TableObjects.Append("<TD fontIndex=\"2\" " + (i == 0 ? "leftBorder=\"1\"" : "") + "/>");
                        }
                        TableObjects.Append("</TR>");
                        int gridHight_Tr = 24;
                        string picColumnHeight = System.Web.HttpContext.Current.Request.Form["gridcls_" + gridCount + "Height"];
                        if (!string.IsNullOrEmpty(picColumnHeight))
                        {
                            gridHight_Tr = int.Parse(picColumnHeight);
                            if (MasksXml.Length < 1)
                            {
                                MasksXml.Append("<Masks><mask id=\"1\" datatype=\"1\">picture()</mask></Masks>");
                            }
                        }
                        TableObjects.Append("<TR height=\"" + gridHight_Tr + "\" sequence=\"" + seqIndex++ + "\">").Append("<TD fontIndex=\"3\" align=\"left\" datatype=\"1\"/>")
                             .Append("<TD fontIndex=\"3\" textColor=\"#000000\" leftBorder=\"1\" topBorder=\"1\" align=\"left\"" + (gridTable.Rows[0]["picflg"].ToString() == "1" ? " maskid=\"1\"" : "") + "  datatype=\"1\" formula=\"=datarow(&apos;ds1\\Grids\\" + "gridcls_" + gridCount + "\\jsonobject&apos;)\"></TD>");
                        for (int i = 1; i < N - 1; i++)
                        {
                            if (i > gridN - 1)
                            {
                                TableObjects.Append("<TD fontIndex=\"3\" " + (i == gridN ? "leftBorder=\"1\"" : "") + " align=\"left\"/>");
                            }
                            else
                            {
                                TableObjects.Append("<TD fontIndex=\"3\" textColor=\"#000000\" leftBorder=\"1\" topBorder=\"1\" align=\"left\"" + (gridTable.Rows[i]["picflg"].ToString() == "1" ? " maskid=\"1\"" : "") + " isDataSource=\"true\" datatype=\"1\"></TD>");
                            }
                        }
                        TableObjects.Append("</TR>");

                        TableObjects.Append("<TR height=\"20\" sequence=\"" + seqIndex++ + "\">").Append("<TD align=\"left\"/>");  //插入空行
                        for (int i = 0; i < N - 2; i++)
                        {
                            TableObjects.Append("<TD " + (i < gridN ? "topBorder=\"1\"" : "") + " align=\"left\"/>");
                        }
                        TableObjects.Append("<TD align=\"left\"/>").Append("</TR>");
                        NodeXml.Append("<Node><name alias=\"" + alias + "\">Grids\\" + "gridcls_" + gridCount + "\\jsonobject</name></Node>");
                        gridCount++;
                        grid = System.Web.HttpContext.Current.Request.Form["gridcls_" + gridCount];
                    }
                    #endregion
                }
                XmlDocument doc = new XmlDocument();
                #region 构建Xml
                StringBuilder xmlSB = new StringBuilder();
                xmlSB.Append(defaultHeadPage.Replace("HEADTITLEFLG", mTitle));
                for (int i = 0; i < N - 2; i++)
                {
                    xmlSB.Append("<Col width=\"" + (oW - 30) / (N - 2) + "\"/>");
                }
                xmlSB.Append("<Col width=\"15\"/>");
                xmlSB.Append("<TR height=\"65\" sequence=\"0\">")
                     .Append("<TD fontIndex=\"1\">" + mTitle + "</TD>");
                for (int i = 0; i < N - 1; i++)
                {
                    xmlSB.Append("<TD fontIndex=\"1\"/>");
                }
                xmlSB.Append("</TR>");

                if (hasForm == "true")
                {
                    xmlSB.Append("<TR height=\"" + gObjectTrHeight + "\" sequence=\"1\">");  //插入空行,放Form表单
                    for (int i = 0; i < N; i++)
                    {
                        xmlSB.Append("<TD/>");
                    }
                    xmlSB.Append("</TR>");
                }

                if (hasGrid == "true")
                {
                    xmlSB.Append(TableObjects.ToString());  //插入GridTable
                }
                else
                {
                    seqIndex++;
                }

                xmlSB.Append("<TR height=\"20\" sequence=\"" + seqIndex++ + "\">").Append("<TD align=\"left\"/>");  //插入空行
                for (int i = 0; i < N - 2; i++)
                {
                    xmlSB.Append("<TD topBorder=\"0\" align=\"left\"/>");
                }
                xmlSB.Append("<TD align=\"left\"/>").Append("</TR>");

                xmlSB.Append("<TR height=\"26\" sequence=\"" + seqIndex++ + "\">").Append("<TD align=\"left\"/>")
                     .Append("<TD align=\"right\" formula=\"" + currDataFormat + "\"></TD>");  //插入制单日期
                for (int i = 0; i < N - 2; i++)
                {
                    xmlSB.Append("<TD align=\"right\"/>");
                }
                xmlSB.Append("</TR>");

                xmlSB.Append("<TR height=\"13\" sequence=\"" + seqIndex++ + "\">");
                for (int i = 0; i < N; i++)
                {
                    xmlSB.Append("<TD align=\"left\"/>");
                }
                xmlSB.Append("</TR>").Append("</Table>");
                xmlSB.Append("<Merges>")
                     .Append("<Range row1=\"0\" col1=\"0\" row2=\"0\" col2=\"" + (N - 1).ToString() + "\"/>")
                     .Append("<Range row1=\"1\" col1=\"0\" row2=\"1\" col2=\"" + (N - 1).ToString() + "\"/>")
                     .Append("<Range row1=\"" + (seqIndex - 2) + "\" col1=\"1\" row2=\"" + (seqIndex - 2) + "\" col2=\"" + (N - 2).ToString() + "\"/>")
                     .Append(MergCells.ToString())
                     .Append("</Merges>");
                xmlSB.Append(GraphicObjects.ToString());
                xmlSB.Append(GetPrintSetup(System.Web.HttpContext.Current.Request.Form["ctype"]));
                xmlSB.Append("</WorkSheet>");
                xmlSB.Append("<DataSources Version=\"255\" isAutoCalculateWhenOpen=\"false\" isSaveCalculateResult=\"false\"><DataSource type=\"4\">")
                     .Append("<Data><ID>ds1</ID><Version>2</Version><Type>4</Type><TypeMeaning>JSON</TypeMeaning><Source></Source><Memo>主信息</Memo>")
                     .Append("<XML_RecordAble_Nodes>").Append(NodeXml.ToString()).Append("</XML_RecordAble_Nodes>");
                xmlSB.Append("<Columns>").Append(ColumnsXml.ToString());
                xmlSB.Append("</Columns></Data></DataSource></DataSources>").Append(MasksXml.ToString()).Append("</Report>");
                doc.LoadXml(xmlSB.ToString());
                #endregion

                doc.Save(xmlFileName);
                //reJson = "{\"status\":\"ok\",\"xmlName\":\"" + supCanTemplate + "/" + fname + "\"}";
                reJson = "{\"status\":\"ok\",\"xmlName\":\"" + supCanTemplate + "/" + fname + "\",\"dataName\":\"" + supCanTemplate + "/" + dname + "\"}";
            }
            catch (Exception ex)
            {
                reJson = "{\"status\":\"" + ex.Message + "\"}";
            }
            return reJson;
        }

        /// <summary>
        ///   清理7天前数据
        /// </summary>
        /// <returns></returns>
        public void ClearTmp(int cacheDays)
        {              
            string tmpPath = System.Web.HttpContext.Current.Server.MapPath(supCanTemplate);
            try
            {
                DirectoryInfo df = new DirectoryInfo(tmpPath);
                if (df.Exists)
                {
                    FileInfo[] fInfos = df.GetFiles("*.txt");                   
                    foreach (FileInfo f in fInfos)
                    {
                        if (f.CreationTime < DateTime.Now.AddDays(0- cacheDays).Date)//数据缓存7天
                        {
                            f.Delete();
                        }
                    }
                    fInfos = df.GetFiles("*.xml");
                    foreach (FileInfo f in fInfos)
                    {
                        if (f.CreationTime < DateTime.Now.AddDays(0 - cacheDays).Date)//数据缓存7天
                        {
                            f.Delete();
                        }
                    }
                }              
            }
            catch (Exception ex)
            {
               
            }
          
        }


        #region PrintDetail--------单据页面套打
        /// <summary>
        /// PrintDetail
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult PrintDetail()
        {
            ViewBag.PageId = System.Web.HttpContext.Current.Request.Params["pageid"];
            ViewBag.FileName = System.Web.HttpContext.Current.Request.Params["fileName"];
            ViewBag.TypeFile = System.Web.HttpContext.Current.Request.Params["typeFile"];
            ViewBag.MTitle = System.Web.HttpContext.Current.Request.Params["mTitle"];
            string printId = System.Web.HttpContext.Current.Request.Params["printId"];
            ViewBag.PrintId = printId;
            ViewBag.MoudleType = System.Web.HttpContext.Current.Request.Params["moudleType"];
            string datapath = System.Web.HttpContext.Current.Request.Params["datapath"];
            if (!string.IsNullOrEmpty(printId))
            {
                ViewBag.PreViewEditFlg = System.Web.HttpContext.Current.Request.Params["previeweditflg"];
                ViewBag.ShowPreView = System.Web.HttpContext.Current.Request.Params["showpreview"];
            }
            ViewBag.DataPath = datapath;
            ViewBag.TempDir= supCanTemplate;     
            return View("PrintDetail");
        }

        /// <summary>
        /// 生成打印需要的Xml文件(一般为Detail页面，也可以是多个列表页面)
        /// </summary>
        /// <returns></returns>
        public string CreateXmlMouldForDetail()
        {
            //string pageName = System.Web.HttpContext.Current.Request.Form["pageName"];
            string printId = System.Web.HttpContext.Current.Request.Form["printid"];
            string mTitle = System.Web.HttpContext.Current.Request.Form["mtitle"];
            string fname = mTitle + DateTime.Now.ToString("yyyyMMdd") + ".xml";
            string tmpDir = System.Web.HttpContext.Current.Server.MapPath(supCanTemplate);
            string XmlFileName = Path.Combine(tmpDir, fname);
            if (!string.IsNullOrEmpty(printId))
            {
                string printFile = proxy.GetTemplateById(printId, System.Web.HttpContext.Current.Server.MapPath(supCanTemplate));
                return "{\"status\":\"ok\",\"xmlName\":\"" + supCanTemplate + "/" + printFile + "\"}";
            }
            string reJson = "";
            try
            {
                string hasGrid = System.Web.HttpContext.Current.Request.Form["hasgrid"];
                string hasForm = System.Web.HttpContext.Current.Request.Form["hasform"];
                string dTitle = System.Web.HttpContext.Current.Request.Form["dTitle"];
                int oW = int.Parse(System.Web.HttpContext.Current.Request.Form["width"]);
                string buskey = System.Web.HttpContext.Current.Request.Form["buskey"];
                StringBuilder GraphicObjects = new StringBuilder();
                StringBuilder TableObjects = new StringBuilder();
                StringBuilder MergCells = new StringBuilder();
                StringBuilder NodeXml = new StringBuilder();
                StringBuilder ColumnsXml = new StringBuilder();
                StringBuilder MasksXml = new StringBuilder();
                int gObjectTrHeight = 30, gObjectTdWidth = oW, gObjectfontIndex_t = 4, gObjectfontIndex_v = 5, seqIndex = 1;
                int N = 6, gridCount = 0;
                if (hasForm == "true")
                {
                    #region hasForm
                    string mF = System.Web.HttpContext.Current.Request.Form["mform"];
                    int rows = int.Parse(System.Web.HttpContext.Current.Request.Form["rows"]);
                    int cols = int.Parse(System.Web.HttpContext.Current.Request.Form["cols"]);
                    string imgStr = System.Web.HttpContext.Current.Request.Form["hasImg"];
                    bool hasImg = string.IsNullOrEmpty(imgStr) ? false : bool.Parse(imgStr);
                    DataTable mForm = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(mF);
                    gObjectTrHeight *= rows;
                    GraphicObjects.Append("<GraphicObjects>");
                    gObjectTdWidth = hasImg ? (gObjectTdWidth - int.Parse(System.Web.HttpContext.Current.Request.Form["imgWidth"]) - 15) : gObjectTdWidth;
                    int tmpX = gObjectTdWidth;
                    gObjectTdWidth = gObjectTdWidth / cols;
                    int x1 = 0, x2 = 0, y1 = 0, y2 = 0;
                    int r_col = 0, r_index = 0, seq = 1;
                    if (!string.IsNullOrEmpty(buskey))
                    {
                        ColumnsXml.Append("<Column><name>" + buskey + "</name><text>主键</text><type>string</type><visible>true</visible><sequence>" + seq++ + "</sequence></Column>");
                    }
                    for (int i = 0; i < mForm.Rows.Count; i++)
                    {
                        DataRow dr = mForm.Rows[i];
                        int colspan = int.Parse(dr["colspan"].ToString());
                        if (r_col + colspan > cols)
                        {
                            r_col = 0;
                            r_index++;
                        }
                        if (r_col == 0)
                        {
                            x1 = 20;
                        }
                        else
                        {
                            x1 += gObjectTdWidth * int.Parse(mForm.Rows[i - 1]["colspan"].ToString());
                        }
                        r_col += colspan;
                        y1 = 65 + r_index * 30;
                        x2 = x1 + 70;
                        y2 = y1 + 26;
                        seq = seq + i;
                        GraphicObjects.Append("<TextBox transparent=\"true\" fontIndex=\"" + gObjectfontIndex_t + "\" align=\"left\" vAlign=\"middle\" text=\"" + dr["label"] + ":\" datatype=\"1\">")
                            .Append("<Rect x1=\"" + x1 + "\" y1=\"" + y1 + "\" x2=\"" + x2 + "\" y2=\"" + y2 + "\"/></TextBox>");
                        GraphicObjects.Append("<TextBox transparent=\"true\" fontIndex=\"" + gObjectfontIndex_v + "\" align=\"left\" vAlign=\"middle\" formula=\"=data(&apos;ds1&apos;, 1, &apos;" + dr["name"] + "&apos;)\" datatype=\"1\">")
                            .Append("<Rect x1=\"" + (x2 + 5) + "\" y1=\"" + y1 + "\" x2=\"" + (x2 + gObjectTdWidth * colspan - 60) + "\" y2=\"" + y2 + "\"/></TextBox>");
                        ColumnsXml.Append("<Column><name>" + dr["name"] + "</name><text>" + dr["label"].ToString() + "</text><type>string</type><visible>true</visible><sequence>" + seq + "</sequence></Column>");
                    }
                    if (hasImg) //打印表单图片对象，可以是多个
                    {
                        DataTable imgObj = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(System.Web.HttpContext.Current.Request.Form["imgObj"]);
                        foreach (DataRow dr in imgObj.Rows)
                        {
                            int imgw = int.Parse(dr["width"].ToString());
                            GraphicObjects.Append("<TextBox fontIndex=\"" + gObjectfontIndex_v + "\" align=\"center\" vAlign=\"middle\" formula=\"=data(&apos;ds1&apos;, 1, &apos;" + dr["name"] + "&apos;)\" maskid=\"1\" datatype=\"1\">")
                                          .Append("<Rect x1=\"" + tmpX + "\" y1=\"66\" x2=\"" + (tmpX + imgw) + "\" y2=\"" + y2 + "\"/></TextBox>");
                            tmpX += imgw + 2;
                            ColumnsXml.Append("<Column><name>" + dr["name"] + "</name><text>" + dr["title"].ToString() + "</text><type>string</type><visible>true</visible><sequence>" + (++seq) + "</sequence></Column>");
                        }
                        MasksXml.Append("<Masks><mask id=\"1\" datatype=\"1\">picture()</mask></Masks>");
                    }
                    GraphicObjects.Append("</GraphicObjects>");
                    NodeXml.Append("<Node><name alias=\"\"/></Node>");
                    #endregion
                }
                else
                {
                    NodeXml.Append("<Node><name alias=\"\"/></Node>");
                    ColumnsXml.Append("<Column><name>" + buskey + "</name><text>主键</text><type>string</type><visible>true</visible><sequence>1</sequence></Column>");
                }
                if (hasGrid == "true")
                {
                    #region hasGrid
                    string[] dT = null;
                    if (!string.IsNullOrEmpty(dTitle))
                    {
                        dT = dTitle.Split(new string[] { "$jkd$" }, StringSplitOptions.None);
                    }
                    if (hasForm == "true")
                    {
                        seqIndex = 2;
                    }
                    N = int.Parse(System.Web.HttpContext.Current.Request.Form["maxcols"]) + 2;
                    string grid = System.Web.HttpContext.Current.Request.Form["gridcls_0"];
                    while (!string.IsNullOrEmpty(grid))
                    {
                        DataTable gridTable = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(grid);
                        int gridN = gridTable.Rows.Count;
                        if (gridCount == 0)
                        {
                            TableObjects.Append("<TR height=\"20\" sequence=\"" + seqIndex++ + "\">").Append("<TD align=\"left\"/>");  //插入空行
                            for (int i = 0; i < N - 2; i++)
                            {
                                TableObjects.Append("<TD topBorder=\"0\" align=\"left\"/>");
                            }
                            TableObjects.Append("<TD align=\"left\"/>").Append("</TR>");
                        }
                        string alias = "gridcls_" + gridCount;
                        if (null != dT && dT.Length > gridCount + 1)
                        {
                            string t = dT[gridCount + 1];
                            if (t.Length > 0)
                            {
                                alias = t;
                                TableObjects.Append("<TR height=\"23\" sequence=\"" + seqIndex++ + "\">").Append("<TD align=\"left\"/>")
                                    .Append("<TD fontIndex=\"2\" align=\"left\">" + t + "</TD>");  //插入副标题
                                for (int i = 0; i < N - 2; i++)
                                {
                                    TableObjects.Append("<TD align=\"left\"/>");
                                }
                                TableObjects.Append("</TR>");
                                MergCells.Append("<Range row1=\"" + (seqIndex - 1) + "\" col1=\"1\" row2=\"" + (seqIndex - 1) + "\" col2=\"" + (N - 2).ToString() + "\"/>");
                            }
                        }
                        TableObjects.Append("<TR height=\"24\" sequence=\"" + seqIndex++ + "\">").Append("<TD fontIndex=\"2\" datatype=\"1\"/>");
                        int tmpsequence = 1;
                        foreach (DataRow dr in gridTable.Rows)
                        {
                            TableObjects.Append("<TD fontIndex=\"2\" bgColor=\"#E0E0E0\" leftBorder=\"1\" topBorder=\"1\" transparent=\"false\" datatype=\"1\">" + dr["text"].ToString() + "</TD>");
                            ColumnsXml.Append("<Column><name>Grids\\" + "gridcls_" + gridCount + "\\jsonobject\\" + dr["dataIndex"] + "</name><text>" + dr["text"].ToString() + "</text><type>string</type><visible>true</visible><sequence>" + tmpsequence++ + "</sequence></Column>");
                        }
                        for (int i = 0; i < N - gridN - 1; i++)
                        {
                            TableObjects.Append("<TD fontIndex=\"2\" " + (i == 0 ? "leftBorder=\"1\"" : "") + "/>");
                        }
                        TableObjects.Append("</TR>");
                        int gridHight_Tr = 24;
                        string picColumnHeight = System.Web.HttpContext.Current.Request.Form["gridcls_" + gridCount + "Height"];
                        if (!string.IsNullOrEmpty(picColumnHeight))
                        {
                            gridHight_Tr = int.Parse(picColumnHeight);
                            if (MasksXml.Length < 1)
                            {
                                MasksXml.Append("<Masks><mask id=\"1\" datatype=\"1\">picture()</mask></Masks>");
                            }
                        }
                        TableObjects.Append("<TR height=\"" + gridHight_Tr + "\" sequence=\"" + seqIndex++ + "\">").Append("<TD fontIndex=\"3\" align=\"left\" datatype=\"1\"/>")
                             .Append("<TD fontIndex=\"3\" textColor=\"#000000\" leftBorder=\"1\" topBorder=\"1\" align=\"left\"" + (gridTable.Rows[0]["picflg"].ToString() == "1" ? " maskid=\"1\"" : "") + "  datatype=\"1\" formula=\"=datarow(&apos;ds1\\Grids\\" + "gridcls_" + gridCount + "\\jsonobject&apos;)\"></TD>");
                        for (int i = 1; i < N - 1; i++)
                        {
                            if (i > gridN - 1)
                            {
                                TableObjects.Append("<TD fontIndex=\"3\" " + (i == gridN ? "leftBorder=\"1\"" : "") + " align=\"left\"/>");
                            }
                            else
                            {
                                TableObjects.Append("<TD fontIndex=\"3\" textColor=\"#000000\" leftBorder=\"1\" topBorder=\"1\" align=\"left\"" + (gridTable.Rows[i]["picflg"].ToString() == "1" ? " maskid=\"1\"" : "") + " isDataSource=\"true\" datatype=\"1\"></TD>");
                            }
                        }
                        TableObjects.Append("</TR>");

                        TableObjects.Append("<TR height=\"20\" sequence=\"" + seqIndex++ + "\">").Append("<TD align=\"left\"/>");  //插入空行
                        for (int i = 0; i < N - 2; i++)
                        {
                            TableObjects.Append("<TD " + (i < gridN ? "topBorder=\"1\"" : "") + " align=\"left\"/>");
                        }
                        TableObjects.Append("<TD align=\"left\"/>").Append("</TR>");
                        NodeXml.Append("<Node><name alias=\"" + alias + "\">Grids\\" + "gridcls_" + gridCount + "\\jsonobject</name></Node>");
                        gridCount++;
                        grid = System.Web.HttpContext.Current.Request.Form["gridcls_" + gridCount];
                    }
                    #endregion
                }
                XmlDocument doc = new XmlDocument();
                #region 构建Xml
                StringBuilder xmlSB = new StringBuilder();
                xmlSB.Append(defaultHeadPage.Replace("HEADTITLEFLG", mTitle));
                for (int i = 0; i < N - 2; i++)
                {
                    xmlSB.Append("<Col width=\"" + (oW - 30) / (N - 2) + "\"/>");
                }
                xmlSB.Append("<Col width=\"15\"/>");
                xmlSB.Append("<TR height=\"65\" sequence=\"0\">")
                     .Append("<TD fontIndex=\"1\">" + mTitle + "</TD>");
                for (int i = 0; i < N - 1; i++)
                {
                    xmlSB.Append("<TD fontIndex=\"1\"/>");
                }
                xmlSB.Append("</TR>");

                if (hasForm == "true")
                {
                    xmlSB.Append("<TR height=\"" + gObjectTrHeight + "\" sequence=\"1\">");  //插入空行,放Form表单
                    for (int i = 0; i < N; i++)
                    {
                        xmlSB.Append("<TD/>");
                    }
                    xmlSB.Append("</TR>");
                }

                if (hasGrid == "true")
                {
                    xmlSB.Append(TableObjects.ToString());  //插入GridTable
                }
                else
                {
                    seqIndex++;
                }

                xmlSB.Append("<TR height=\"20\" sequence=\"" + seqIndex++ + "\">").Append("<TD align=\"left\"/>");  //插入空行
                for (int i = 0; i < N - 2; i++)
                {
                    xmlSB.Append("<TD topBorder=\"0\" align=\"left\"/>");
                }
                xmlSB.Append("<TD align=\"left\"/>").Append("</TR>");

                xmlSB.Append("<TR height=\"26\" sequence=\"" + seqIndex++ + "\">").Append("<TD align=\"left\"/>")
                     .Append("<TD align=\"right\" formula=\"" + currDataFormat + "\"></TD>");  //插入制单日期
                for (int i = 0; i < N - 2; i++)
                {
                    xmlSB.Append("<TD align=\"right\"/>");
                }
                xmlSB.Append("</TR>");

                xmlSB.Append("<TR height=\"13\" sequence=\"" + seqIndex++ + "\">");
                for (int i = 0; i < N; i++)
                {
                    xmlSB.Append("<TD align=\"left\"/>");
                }
                xmlSB.Append("</TR>").Append("</Table>");
                xmlSB.Append("<Merges>")
                     .Append("<Range row1=\"0\" col1=\"0\" row2=\"0\" col2=\"" + (N - 1).ToString() + "\"/>")
                     .Append("<Range row1=\"1\" col1=\"0\" row2=\"1\" col2=\"" + (N - 1).ToString() + "\"/>")
                     .Append("<Range row1=\"" + (seqIndex - 2) + "\" col1=\"1\" row2=\"" + (seqIndex - 2) + "\" col2=\"" + (N - 2).ToString() + "\"/>")
                     .Append(MergCells.ToString())
                     .Append("</Merges>");
                xmlSB.Append(GraphicObjects.ToString());
                xmlSB.Append(GetPrintSetup(System.Web.HttpContext.Current.Request.Form["ctype"]));
                xmlSB.Append("</WorkSheet>");
                xmlSB.Append("<DataSources Version=\"255\" isAutoCalculateWhenOpen=\"false\" isSaveCalculateResult=\"false\"><DataSource type=\"4\">")
                     .Append("<Data><ID>ds1</ID><Version>2</Version><Type>4</Type><TypeMeaning>JSON</TypeMeaning><Source></Source><Memo>主信息</Memo>")
                     .Append("<XML_RecordAble_Nodes>").Append(NodeXml.ToString()).Append("</XML_RecordAble_Nodes>");
                xmlSB.Append("<Columns>").Append(ColumnsXml.ToString());
                xmlSB.Append("</Columns></Data></DataSource></DataSources>").Append(MasksXml.ToString()).Append("</Report>");
                doc.LoadXml(xmlSB.ToString());
                #endregion
                if (!Directory.Exists(tmpDir))
                {
                    Directory.CreateDirectory(tmpDir);
                }
                doc.Save(XmlFileName);
                reJson = "{\"status\":\"ok\",\"xmlName\":\"" + supCanTemplate + "/" + fname + "\"}";
            }
            catch (Exception ex)
            {
                reJson = "{\"status\":\"" + ex.Message + "\"}";
            }
            return reJson;
        }

        /// <summary>
        /// 保存用户自定义模板文件（xml格式保存）
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        public string SaveXmlMouldForDetail()
        {
            string reJson = "";
            try
            {
                string xmlDoc = System.Web.HttpContext.Current.Request.Form["xmlDoc"];
                string mTitle = System.Web.HttpContext.Current.Request.Form["mTitle"];
                string typeFile = System.Web.HttpContext.Current.Request.Form["typeFile"];
                string printId = System.Web.HttpContext.Current.Request.Form["printId"];
                if (string.IsNullOrEmpty(xmlDoc))
                {
                    reJson = "{\"status\":\"xml文件为空\"}";
                }
                else
                {
                    string printFile = proxy.SaveUserDefTemplate(ref printId, typeFile, mTitle, xmlDoc, System.Web.HttpContext.Current.Server.MapPath(supCanUserTemplate));
                    reJson = "{\"status\":\"ok\",\"printId\":\"" + printId + "\",\"xmlName\":\"" + supCanUserTemplate + "/" + printFile + "\"}";
                }
            }
            catch (Exception ex)
            {
                reJson = "{\"status\":\"" + ex.Message + "\"}";
            }
            return reJson;
        }
        #endregion

        #region LFormList----------套打管理列表
        /// <summary>
        /// LFormList
        /// </summary>
        /// <returns></returns>
        [ActionAuthorize(NG3.Web.Mvc.Level.NonCheck)]
        public ActionResult LFormList()
        {
            SetPublicValue();
            return View("LFormList");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult LoadTplTree()
        {
            IList<TreeJSONBase> list = proxy.LoadTplTree("");
            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 返回套打列表数据
        /// </summary>
        /// <returns></returns>
        public string GetLFormList()
        {
            CreateLfromEnv();

            string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
            string limit = System.Web.HttpContext.Current.Request.Params["limit"];
            string page = System.Web.HttpContext.Current.Request.Params["page"];

            int pageSize = 20, pageIndex = 0, totalRecord = 0;
            int.TryParse(limit, out pageSize);
            int.TryParse(page, out pageIndex);

            DataTable dt = proxy.GetLFormList(clientJsonQuery, pageSize, pageIndex, ref totalRecord);
            string json = DataConverterHelper.ToJson(dt, totalRecord);

            return json;
        }

        /// <summary>
        /// 导入模板(模板格式如下：****(*_***).xml)
        /// </summary>
        /// <returns></returns>
        public string ImportModule()
        {
            string mType = System.Web.HttpContext.Current.Request.Form["type"];
            SetPublicValue();
            string reJson = "{}";
            string errTxt = "";

            string tmpPath = mType == "sys" ? System.Web.HttpContext.Current.Server.MapPath(supCanSysTemplate) : System.Web.HttpContext.Current.Server.MapPath(supCanUserTemplate);
            string alertMsg = mType == "sys" ? " 个系统模板导入完成." : " 个用户模板导入完成.";
            int retInt = proxy.ImportModule(tmpPath, allMoudleType, mType, ref errTxt);
            if (mType == "sys") {
                retInt+= proxy.ImportModule(System.Web.HttpContext.Current.Server.MapPath(supCanPdfTemplate), allMoudleType, "sys_pdf", ref errTxt);
            }
            if (retInt < 0)
            {
                reJson = "{\"status\":\"error\",\"msg\":\"" + errTxt + "\"}";
            }
            else
            {
                reJson = "{\"status\":\"ok\",\"count\":" + retInt.ToString() + ",\"msg\":\"共 " + retInt.ToString() + alertMsg + "\"}";
            }
            return reJson;
        }
        /// <summary>
        /// 导入模板(模板格式如下：****(*_***).xml)
        /// </summary>
        /// <returns></returns>
        public string UpdateModule()
        {
            string mType = System.Web.HttpContext.Current.Request.Form["type"];
            string updateNew = System.Web.HttpContext.Current.Request.Form["updateNew"];
            SetPublicValue();
            string reJson = "{}";
            string errTxt = "";

            string tmpPath = mType == "sys" ? System.Web.HttpContext.Current.Server.MapPath(supCanSysTemplate) : System.Web.HttpContext.Current.Server.MapPath(supCanUserTemplate);
            string alertMsg = mType == "sys" ? " 个系统模板更新完成." : " 个用户模板更新完成.";
            int retInt = proxy.UpdateModule(tmpPath, allMoudleType, mType, ref errTxt);
            if (mType == "sys")
            {
                if (updateNew == "0")
                {
                    retInt += proxy.UpdateModule(System.Web.HttpContext.Current.Server.MapPath(supCanPdfTemplate), allMoudleType, "sys_pdf_0", ref errTxt);
                }
                else
                {
                    retInt += proxy.UpdateModule(System.Web.HttpContext.Current.Server.MapPath(supCanPdfTemplate), allMoudleType, "sys_pdf", ref errTxt);
                }
            }
            if (retInt < 0)
            {
                reJson = "{\"status\":\"error\",\"msg\":\"" + errTxt + "\"}";
            }
            else
            {
                reJson = "{\"status\":\"ok\",\"count\":" + retInt.ToString() + ",\"msg\":\"共 " + retInt.ToString() + alertMsg + "\"}";
            }
            return reJson;
        }
        /// <summary>
        /// 清理临时文件
        /// </summary>
        /// <returns></returns>
        public string ClearTmpModule()
        {
            string reJson = "{}";
            int N = 0;
            string tmpPath = System.Web.HttpContext.Current.Server.MapPath(supCanTemplate);
            try
            {
                DirectoryInfo df = new DirectoryInfo(tmpPath);
                if (df.Exists)
                {
                    FileInfo[] fInfos = df.GetFiles("*.xml");
                    N = fInfos.Length;
                    foreach (FileInfo f in fInfos)
                    {
                        f.Delete();
                    }
                }
                reJson = "{\"status\":\"ok\",\"msg\":\"共 " + N.ToString() + " 个文件清理完成.\"}";
            }
            catch (Exception ex)
            {
                reJson = "{\"status\":\"fail\",\"msg\":\"" + ex.Message.Replace("\r\n", "") + "\"}";
            }
            return reJson;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public string DeleteModule()
        {
            string printId = System.Web.HttpContext.Current.Request.Form["printid"];

            string errTxt = "";
            string reJson = "{}";
            int retInt = proxy.DeleteModule(printId, ref errTxt);
            if (retInt < 0)
            {
                reJson = "{\"status\":\"error\",\"msg\":\"" + errTxt + "\"}";
            }
            else
            {
                reJson = "{\"status\":\"ok\",\"count\":" + retInt.ToString() + ",\"msg\":\"删除成功.\"}";
            }
            return reJson;
        }

        /// <summary>
        /// 创建打印模板目录
        /// </summary>
        private void CreateLfromEnv()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            if (!System.IO.Directory.Exists(supCanSysTemplate))
            {
                System.IO.Directory.CreateDirectory(context.Server.MapPath(supCanSysTemplate));
            }
            if (!System.IO.Directory.Exists(supCanUserTemplate))
            {
                System.IO.Directory.CreateDirectory(context.Server.MapPath(supCanUserTemplate));
            }
            if (!System.IO.Directory.Exists(supCanTemplate))
            {
                System.IO.Directory.CreateDirectory(context.Server.MapPath(supCanTemplate));
            }
        }
        #endregion

        #region LformEdit----------套打管理明细
        /// <summary>
        /// LFormList
        /// </summary>
        /// <returns></returns>
        public ActionResult LformEdit()
        {
            string otype = System.Web.HttpContext.Current.Request.Params["otype"];
            ViewBag.OType = otype;
            ViewBag.PrintID = System.Web.HttpContext.Current.Request.Params["printid"];             
            ViewBag.TypeFile = System.Web.HttpContext.Current.Request.Params["typefile"];
            ViewBag.TemplateType = System.Web.HttpContext.Current.Request.Params["templatetype"];
            string templatename = System.Web.HttpContext.Current.Request.Params["templatename"];
            if (otype == "view")
            {
                ViewBag.Title = "打印模板查看";
            }
            else
            {
                ViewBag.Title = "打印模板编辑";
            }
            //if (!string.IsNullOrEmpty(templatename))
            //{
            //    templatename = System.Web.HttpUtility.UrlEncode(templatename, System.Text.Encoding.GetEncoding("UTF-8"));
            //}
            ViewBag.TemplateName = templatename;
            return View("LformEdit");
        }

        /// <summary>
        /// 获取模板详细数据
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        public string GetModuleByID()
        {
            string printId = System.Web.HttpContext.Current.Request.Form["printid"];
            string xml = System.Web.HttpContext.Current.Request.Form["xml"];

            DataTable tmpDT = proxy.GetModuleById(printId, System.Web.HttpContext.Current.Server.MapPath(supCanTemplate));
            if (tmpDT!=null&&tmpDT.Rows.Count>0)
            {
                JObject jo = tmpDT.Rows[0].ToJObject();
                string json = JsonConvert.SerializeObject(jo);
                string tmpDir = System.Web.HttpContext.Current.Server.MapPath(supCanTemplate);
                string xmlFileName = Path.Combine(tmpDir, tmpDT.Rows[0]["filename"].ToString());
                MergeXML(xml, xmlFileName);
                return "{status : \"ok\",xmlName:\"" + supCanTemplate + "/" + tmpDT.Rows[0]["filename"].ToString() + "\", data:" + json + "}";
            }
            else
            {
                return "{status : \"error\"}";
            }
        }
        /// <summary>
        ///  合并xml
        /// </summary>
        /// <param name="xml">新的</param>
        /// <param name="xmlFileName">旧的</param>
        /// <returns></returns>
        public void MergeXML(string xml, string xmlFileName)
        {          
            if (!string.IsNullOrEmpty(xml))
            {
                XmlDocument newDoc = new XmlDocument();
                newDoc.LoadXml(xml);
                var newNode = newDoc.SelectSingleNode("Report/DataSources/DataSource[@type='4']/Data[ID='ds1']");
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlFileName);                
                var node = doc.SelectSingleNode("Report/DataSources/DataSource[@type='4']/Data[ID='ds1']");
                if (node == null)
                {
                    if (newNode != null)
                    {
                        var sourcesNode = doc.SelectSingleNode("Report/DataSources");
                        var newN = doc.CreateElement("DataSource");
                        newN.SetAttribute("type", "4");
                        newN.InnerXml = newNode.ParentNode.InnerXml;
                        sourcesNode.AppendChild(newN);
                    }
                }
                else if (newNode != null)
                {
                    node.InnerXml = newNode.InnerXml;
                }
                else
                { 
                    node.ParentNode.ParentNode.RemoveAll();
                }
                doc.Save(xmlFileName);
            } 
        }

        /// <summary>
        /// 系统、用户模板更新(新增、修改)
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        public string UpdateLformTemplate()
        {
            string errTxt = "";
            string reJson = "{}";
            string printId = System.Web.HttpContext.Current.Request.Form["printid"];
            string moduleNo = System.Web.HttpContext.Current.Request.Form["moduleno"];
            string typeFile = System.Web.HttpContext.Current.Request.Form["typefile"];
            string billName = System.Web.HttpContext.Current.Request.Form["billname"];
            string fileName = System.Web.HttpContext.Current.Request.Form["filename"];
            string remarks = System.Web.HttpContext.Current.Request.Form["remarks"];
            string xmlDoc = System.Web.HttpContext.Current.Request.Form["xmlDoc"];
            string oType = System.Web.HttpContext.Current.Request.Form["oType"];
            string def_int2 = System.Web.HttpContext.Current.Request.Form["def_int2"];
            string def_int1 = System.Web.HttpContext.Current.Request.Form["def_int1"];
            string ispub = System.Web.HttpContext.Current.Request.Form["ispub"];
            string previeweditflg = System.Web.HttpContext.Current.Request.Form["previeweditflg"];
            
            string tmpPath = System.Web.HttpContext.Current.Server.MapPath(supCanTemplate);
            if (string.IsNullOrEmpty(def_int2))
            {
                def_int2 = "0";
            }
            if (oType == "add" || oType == "edit")  //新增用户模板 ,修改用户、自定义模板
            {
                tmpPath = System.Web.HttpContext.Current.Server.MapPath(supCanUserTemplate);
            }
            string def_str1 = "";
            if (ispub == "0")
            {
                def_str1 = AppInfoBase.UCode;
            }
            printId=proxy.Update(oType, printId, moduleNo, tmpPath, xmlDoc, new Dictionary<string, object>() {
                { "typefile",typeFile},
                { "moduleno",moduleNo},
                { "billname",billName},
                { "filename",fileName},
                { "def_int1",def_int1},
                { "def_int2",def_int2},
                { "def_str1",def_str1},                 
                { "def_str3","supcan"},
                { "ispub",ispub},
                { "previeweditflg",previeweditflg},
                { "iscompress","0"},
                { "remarks",remarks}  
            }, ref errTxt);
           // printId = proxy.UpdateLformTemplate(tmpPath, oType, printId, moduleNo, typeFile, billName, fileName, remarks, xmlDoc, def_int2, ref errTxt);
            if (string.IsNullOrEmpty(printId))
            {
                reJson = "{\"status\":\"error\",\"msg\":\"" + (errTxt.Length > 0 ? errTxt : "保存失败.") + "\"}";
            }
            else
            {
                reJson = "{\"status\":\"ok\",\"printId\":\"" + printId + "\",\"msg\":\"保存成功.\"}";
            }
            return reJson;
        }
        public string UpdateStatus()
        {
            string reJson = "";
            string ret = "";
            string printId = System.Web.HttpContext.Current.Request.Form["printid"];
            string stat = System.Web.HttpContext.Current.Request.Form["stat"];
            if (!string.IsNullOrEmpty(printId))
            {
                ret = proxy.UpdateStatus(printId, "hide", stat);
            }
            if (!string.IsNullOrEmpty(ret))
            {
                reJson = "{\"status\":\"error\",\"msg\":\"" + ret + "\"}";
            }
            else
            {
                reJson = "{\"status\":\"ok\"}";
            }
            return reJson;
        }
        #endregion

        #region LformHelp----------套打选择帮助
        /// <summary>
        /// 打印模板选择
        /// </summary>
        /// <returns></returns>
        public ActionResult PrintTemplateSelect()
        {
            ViewBag.OType = System.Web.HttpContext.Current.Request.Params["otype"];
            ViewBag.TypeFile = System.Web.HttpContext.Current.Request.Params["typefile"];
            string templatename = System.Web.HttpContext.Current.Request.Params["templatename"];
            string datapath = System.Web.HttpContext.Current.Request.Params["datapath"];
            string optmode = System.Web.HttpContext.Current.Request.Params["optmode"];
            if (!string.IsNullOrEmpty(templatename))
            {
                templatename = System.Web.HttpUtility.UrlEncode(templatename, System.Text.Encoding.GetEncoding("UTF-8"));
            }
            if (!string.IsNullOrEmpty(datapath))
            {
                datapath = System.Web.HttpUtility.UrlEncode(datapath, System.Text.Encoding.GetEncoding("UTF-8"));
            }
            ViewBag.TemplateName = templatename;
            ViewBag.DataPath = datapath;
            ViewBag.OptMode = optmode;
            return View("PrintTemplateSelect");
        }
        /// <summary>
        /// 返回套打帮助列表数据
        /// </summary>
        /// <returns></returns>
        public string GetFmtTemplateFromDb()
        {
            string typeFile = System.Web.HttpContext.Current.Request.Params["typefile"]; 
            DataTable dt = proxy.GetFmtTemplateFromDB(typeFile);
            string json = DataConverterHelper.ToJson(dt, dt.Rows.Count);
            return json;
        }
        #endregion

        #region PrintTest --------------打印测试
        /// <summary>
        /// 测试打印
        /// </summary>
        /// <returns></returns>
        public ActionResult Test()
        {
            ViewBag.ID = System.Web.HttpContext.Current.Request.Params["id"];//主键
            return View("PrintTest");
        }
        public ActionResult PrintTest()
        {
            ViewBag.ID = System.Web.HttpContext.Current.Request.Params["id"];//主键
            return View("PrintTest");
        }
        /// <summary>
        /// 
        /// </summary>
        public string GetTestData()
        {
            string id = System.Web.HttpContext.Current.Request.Params["id"];  //主表主键
            string tabtype = System.Web.HttpContext.Current.Request.Params["tabtype"]; //Tab类型
            string json = "";
            switch (tabtype)
            {
                case "mst":
                    DataTable tmpDT = proxy.GetTestMst(id);
                    if (null != tmpDT)
                    {
                        JObject jo = tmpDT.Rows[0].ToJObject();
                        json = JsonConvert.SerializeObject(jo);
                        return "{status : \"ok\", data:" + json + "}";
                    }
                    else
                    {
                        return "{status : \"error\"}";
                    }
                case "ir3basedtl":
                    string clientJsonQuery = System.Web.HttpContext.Current.Request.Params["queryfilter"];//查询条件
                    string limit = System.Web.HttpContext.Current.Request.Params["limit"];
                    string page = System.Web.HttpContext.Current.Request.Params["page"];
                    int pageSize = 20, pageIndex = 0, totalRecord = 0;
                    int.TryParse(limit, out pageSize);
                    int.TryParse(page, out pageIndex);
                    DataTable dt1 = proxy.GetTestDtl(id, pageSize, pageIndex, ref totalRecord);
                    json = DataConverterHelper.ToJson(dt1, totalRecord);
                    break;
                default:
                    return "{status : \"error\"}";
            }
            return json;

        }
        #endregion

        #region   模板设计
        public ActionResult PrintTemplate()
        {
            SetPublicValue();
            ViewBag.ID = System.Web.HttpContext.Current.Request.Params["id"];//主键
            return View("PrintTemplate");
        }
        public string BuildTemplate()
        {
            string mstformData = System.Web.HttpContext.Current.Request.Form["mstformData"];
            string ir3basedtlgridData = System.Web.HttpContext.Current.Request.Form["ir3basedtlgridData"];
            var dt = DataConverterHelper.ToDataTable(mstformData, "select '' moduleno,'' busflag,'' billname,'' def_int2,'' main,'' maincol,'' maincolname FROM PRINTFM ");
            var dtl = DataConverterHelper.ToDataTable(ir3basedtlgridData, "select '' tname,'' fieldname,'' chn FROM PRINTFM ");
            string mTitle = dt.Rows[0]["billname"].ToString();
            string main = dt.Rows[0]["main"].ToString();
            string fname =string.Format("{0}[{1}_{2}].xml", dt.Rows[0]["billname"].ToString(), dt.Rows[0]["moduleno"].ToString(), dt.Rows[0]["busflag"].ToString());
            StringBuilder NodeXml = new StringBuilder();
            StringBuilder ColumnsXml = new StringBuilder();
            string tmpDir = System.Web.HttpContext.Current.Server.MapPath(supCanTemplate);
            if (!Directory.Exists(tmpDir))
            {
                Directory.CreateDirectory(tmpDir);
            }
            string xmlFileName = Path.Combine(tmpDir, fname);
          
            if (dt.Rows[0]["maincol"] != null && !string.IsNullOrEmpty(dt.Rows[0]["maincol"].ToString()))
            {
                string[] cols = dt.Rows[0]["maincol"].ToString().Split(',');
                string[]colnames = (dt.Rows[0]["maincolname"] != null && !string.IsNullOrEmpty(dt.Rows[0]["maincolname"].ToString()))?
                   dt.Rows[0]["maincolname"].ToString().Split(','): dt.Rows[0]["maincol"].ToString().Split(',');
                for (int seq = 0; seq < cols.Length; seq++)
                {
                    ColumnsXml.Append("<Column><name>" + cols[seq] + "</name><text>" + colnames[seq] + "</text><type>string</type><visible>true</visible><sequence>" + (seq+1).ToString() + "</sequence></Column>");  
                }
                NodeXml.Append("<Node><name alias=\"\"/></Node>");
            }
            if (dtl!=null&& dtl.Rows.Count > 0) {
                int gridCount = 0;
                foreach (DataRow dr in dtl.Rows)
                {
                    string alias = "gridcls_" + gridCount;                   
                    if (dr["tname"] != null && !string.IsNullOrEmpty(dr["tname"].ToString()))
                    {
                        alias = dr["tname"].ToString();
                    }
                    string[] cols = dr["fieldname"].ToString().Split(',');
                    string[] colnames = (dr["chn"] != null && !string.IsNullOrEmpty(dr["chn"].ToString())) ?
                       dr["chn"].ToString().Split(',') : dr["fieldname"].ToString().Split(',');
                    for (int seq = 0; seq < cols.Length; seq++)
                    {   
                        ColumnsXml.Append("<Column><name>Grids\\" + "gridcls_" + gridCount + "\\jsonobject\\" + cols[seq] + "</name><text>" + colnames[seq] + "</text><type>string</type><visible>true</visible><sequence>" + (seq + 1).ToString() + "</sequence></Column>");
                    }
                    NodeXml.Append("<Node><name alias=\"" + alias + "\">Grids\\" + "gridcls_" + gridCount + "\\jsonobject</name></Node>");
                  
                    gridCount++;
                } 
            }  
            XmlDocument doc = new XmlDocument();
            #region 构建Xml
            StringBuilder xmlSB = new StringBuilder();
            xmlSB.Append(defaultHeadPage.Replace("HEADTITLEFLG", mTitle));
            int N = 6;
            for (int i = 0; i < N - 2; i++)
            {
                xmlSB.Append("<Col width=\"100\"/>");
            }
            xmlSB.Append("<Col width=\"100\"/>");
            for (int k = 0; k < N - 2; k++)
            {
                xmlSB.Append("<TR height=\"45\" sequence=\"0\">").Append("<TD align=\"left\"/>");  //插入空行
                for (int i = 0; i < N - 2; i++)
                {
                    xmlSB.Append("<TD topBorder=\"0\" align=\"left\"/>");
                }
                xmlSB.Append("<TD align=\"left\"/>").Append("</TR>");
            }


            xmlSB.Append("</Table>");
            xmlSB.Append("<Merges>")                
                 .Append("</Merges>");       
            xmlSB.Append("</WorkSheet>");
            xmlSB.Append("<DataSources Version=\"255\" isAutoCalculateWhenOpen=\"false\" isSaveCalculateResult=\"false\"><DataSource type=\"4\">")
                 .Append("<Data><ID>ds1</ID><Version>2</Version><Type>4</Type><TypeMeaning>JSON</TypeMeaning><Source></Source><Memo>").Append(string.IsNullOrEmpty(main) ?"主信息": main).Append("</Memo>")
                 .Append("<XML_RecordAble_Nodes>").Append(NodeXml.ToString()).Append("</XML_RecordAble_Nodes>");
            xmlSB.Append("<Columns>").Append(ColumnsXml.ToString());
            xmlSB.Append("</Columns></Data></DataSource></DataSources>").Append("</Report>");
            doc.LoadXml(xmlSB.ToString());
            #endregion   
            doc.Save(xmlFileName);
            return "{status : \"ok\", data:\"" + fname + "\"}";
        }
        public ActionResult PrintTemplateDesign()
        {
            string filename = System.Web.HttpContext.Current.Request.Params["fileName"];
            if (!string.IsNullOrEmpty(filename)) {
                filename=System.Web.HttpUtility.UrlDecode(filename, System.Text.Encoding.GetEncoding("UTF-8"));
            }
            ViewBag.FileName = filename;
            ViewBag.TypeFile = System.Web.HttpContext.Current.Request.Params["typeFile"];          
            ViewBag.TempDir = supCanTemplate;
            return View("PrintTemplateDesign");
        }
        #endregion

        #region 打印设置
        /// <summary>
        /// 保存打印设置信息
        /// </summary>
        [ValidateInput(false)]
        public void SetPrintSetup()
        {
            string ctype = System.Web.HttpContext.Current.Request.Form["ctype"];
            if (!string.IsNullOrEmpty(ctype))
            {
                string PrintPage = System.Web.HttpContext.Current.Request.Form["PrintPage"];

                proxy.SetPrintSetup(ctype, PrintPage);
            }
        }

        /// <summary>
        /// 获取打印设置信息
        /// </summary>
        /// <param name="ctype"></param>
        /// <returns></returns>
        private string GetPrintSetup(string ctype)
        {
            if (string.IsNullOrEmpty(ctype))
            {
                return defaultPrintPage;
            }

            string printSetupString = proxy.GetPrintSetup(ctype);
            return string.IsNullOrEmpty(printSetupString) ? defaultPrintPage : printSetupString;
        }
        #endregion
    }
}
