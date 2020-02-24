using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Mvc;
using NG3.Aop.Transaction;
using SUP.Frame.Facade;
using NG3.Web.Controller;
using NG3;
using SUP.Common.Base;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.IO;
using System.Web;
using System.Drawing;
using NG3.Bill.Base;

namespace SUP.Frame.Controller
{
    public class IndividualSettingController : AFController
    {
        private IIndividualSettingFacade proxy;
        private IMenuConfigFacade MenuConfigProxy;
        public IndividualSettingController()
        {
            proxy = AopObjectProxy.GetObject<IIndividualSettingFacade>(new IndividualSettingFacade());
            MenuConfigProxy = AopObjectProxy.GetObject<IMenuConfigFacade>(new MenuConfigFacade());
        }  

        public string LoadSysSetting()
        {
            long userid = AppInfoBase.UserID; 
            DataTable dt = proxy.LoadSysSetting(userid);
            string json = string.Empty;
            if (dt != null && dt.Rows.Count > 0)
            {
                json = "{\"IndividualSetting\": " + dt.Rows[0]["individualsetting"] + ", \"ApplogoName\": \"" + dt.Rows[0]["applogoname"] + "\"}";
            }
            //string uitheme = MenuConfigProxy.GetUITheme(userid);
            //ViewBag.UITheme = uitheme;
            //最后一个是string拼法
            //if (json != null && json != "")
            //{
            //    int index = json.IndexOf("]");
            //    json = json.Insert(index - 1, "\",\"" + uitheme);
            //} 
            //最后一个是int拼法
            //if (json != null && json != "")
            //{
            //    int index = json.IndexOf("]");
            //    json = json.Insert(index , ",\"" + uitheme + "\"");
            //}
            return json;
        }

        //public string SaveIndividualSetting()
        //{
        //    string data = System.Web.HttpContext.Current.Request.Form["data"];
        //    long userid = AppInfoBase.UserID;
        //    bool iret = proxy.SaveIndividualSetting(userid, data);
        //    if (iret)
        //    {
        //        return "{status : \"ok\"}";
        //    }
        //    else
        //    {
        //        return "{status : \"error\"}";
        //    }
        //}

        public JsonResult LoadDefaultOpenTab()
        {
            string nodeid = System.Web.HttpContext.Current.Request.Params["node"];
            long userid = AppInfoBase.UserID;//作为参数传进来
            //string useridString = System.Web.HttpContext.Current.Request.Params["userid"];
            //int userid = int.Parse(useridString);

            IList<TreeJSONBase> list = proxy.LoadDefaultOpenTab(userid, nodeid);
            if (list == null)
            {
                return null;
            }
            else
            {
                return this.Json(list, JsonRequestBehavior.AllowGet);
            }

        }


        /// <summary>
        /// 注意，默认tab和设置是分别加载，一起保存
        /// </summary>
        /// <returns></returns>
        public string SaveDefaultOpenTab()
        {
            string defaultOpenTab = System.Web.HttpContext.Current.Request.Form["defaultOpenTab"];
            //string userid = System.Web.HttpContext.Current.Request.Form["userid"];
            long userid = AppInfoBase.UserID;
            //保存默认tab页设置
            DataTable defaultOpenTabTable = DataConverterHelper.ToDataTable(defaultOpenTab, "select * from fg3_defaultopen_tab");
            int iret1 = proxy.SaveDefaultOpenTab(userid,defaultOpenTabTable);
            //保存系统设置
            string individualSetting = System.Web.HttpContext.Current.Request.Form["individualSetting"];
            bool iret2 = proxy.SaveSysSetting(userid, individualSetting);

            if (String.Compare(AppInfoBase.UserType, UserType.System, true) == 0)
            {
                //保存管理员权限下的路由设置和显示设置,注意这两个会改了数据库连接串，不再连0002
                string ServerIpAndNetWorkIpConfig = System.Web.HttpContext.Current.Request.Form["ServerIpAndNetWorkIpConfig"];
                string DisplaySetting = System.Web.HttpContext.Current.Request.Form["DisplaySetting"];
                this.SaveServerIpAndNetWorkIpConfig(ServerIpAndNetWorkIpConfig);
                this.SaveDisplaySetting(DisplaySetting);
            }            

            //保存小铃铛设置
            string alertconfig = System.Web.HttpContext.Current.Request.Form["alertconfig"];
            bool iret3 = proxy.SaveAlertItem(alertconfig);
            //bool iret3 = this.SaveAlertItem(alertconfig); 

            //保存换肤设置
            string uitheme = System.Web.HttpContext.Current.Request.Form["uitheme"];
            int tempuitheme = 0;
            Int32.TryParse(uitheme.ToString(), out tempuitheme);
            bool iret4 = MenuConfigProxy.SaveUITheme(tempuitheme, userid);

            ////保存登陆单位信息
            //string SSOOrgValue = System.Web.HttpContext.Current.Request.Form["SSOOrgValue"];
            //proxy.SetSSOOrg(SSOOrgValue);

            //保存APP启动页Logo
            string APPlogo = System.Web.HttpContext.Current.Request.Form["APPlogo"];
            string filePath = Request.PhysicalApplicationPath + "NG3Resource\\APPLogo\\" + APPlogo;
            bool iret5 = true;
            if(!string.IsNullOrEmpty(APPlogo) && System.IO.File.Exists(filePath))
            {               
                byte[] buffer;
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, (int)fs.Length);
                }

                string attachid = proxy.GetAPPLogoAttachId();
                try
                {
                    if (string.IsNullOrEmpty(attachid))
                    {
                        string result = NG3UploadFileService.NG3UploadEx("", buffer);
                        JObject jo = (JObject)JsonConvert.DeserializeObject(result);

                        if (jo["success"] != null && jo["success"].ToString() == "true")
                        {
                            attachid =  jo["phid"].ToString();
                        }
                        else
                        {
                            return "{status : \"error\", msg: \"APPLogo上传到附件出错\"}";
                        }
                    }
                    else
                    {
                        NG3UploadFileService.NG3ModifyEx("", long.Parse(attachid), buffer);
                    }
                }
                catch
                {
                    return "{status : \"error\", msg: \"APPLogo上传到附件出错\"}";
                }

                iret5 = proxy.SaveAPPLogo(APPlogo, attachid);
                if (iret5)
                {
                    string dirPath = Request.PhysicalApplicationPath + "NG3Resource\\APPLogo";
                    if (Directory.Exists(dirPath))
                    {
                        Directory.Delete(dirPath, true);
                    }
                }
            }

            if (iret1 >= 0 && iret2 && iret3 && iret4 && iret5)
            {
                return "{status : \"ok\"}";
            }
            else
            {
                return "{status : \"error\"}";
            }
        }

        public string LoadServerIpAndNetWorkIpConfig()
        {
            DataTable dt = proxy.LoadServerSetting();
            DataTable networkdt = proxy.LoadNetWorkIPMappingInfo();
            bool IsFillText = false;

            StringBuilder strbuilder = new StringBuilder();
            strbuilder.Append("{");
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    IsFillText = true;
                    if (dt.Rows.Count >= 1)
                    {
                        strbuilder.Append("name4 :'" + dt.Rows[0]["servername"].ToString() + "',");
                        strbuilder.Append("IP4 :'" + dt.Rows[0]["serverip"].ToString() + "',");
                        strbuilder.Append("port :'" + dt.Rows[0]["serverport"].ToString() + "',");
                    }
                    if (dt.Rows.Count >= 2)
                    {
                        strbuilder.Append("name1 :'" + dt.Rows[1]["servername"].ToString() + "',");
                        strbuilder.Append("IP1 :'" + dt.Rows[1]["serverip"].ToString() + "',");
                    }
                    if (dt.Rows.Count >= 3)
                    {
                        strbuilder.Append("name2 :'" + dt.Rows[2]["servername"].ToString() + "',");
                        strbuilder.Append("IP2 :'" + dt.Rows[2]["serverip"].ToString() + "',");
                    }
                    if (dt.Rows.Count >= 4)
                    {
                        strbuilder.Append("name3 :'" + dt.Rows[3]["servername"].ToString() + "',");
                        strbuilder.Append("IP3 :'" + dt.Rows[3]["serverip"].ToString() + "',");
                    }
                }
            }
            if (!IsFillText)
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    string ipaddress1 = networkdt.Rows[0]["ipaddress1"].ToString();
                    string ipaddress2 = networkdt.Rows[0]["ipaddress2"].ToString();
                    string ipaddress3 = networkdt.Rows[0]["ipaddress3"].ToString();
                    string ipaddress4 = networkdt.Rows[0]["ipaddress4"].ToString();

                    int index1 = ipaddress1.IndexOf(":");
                    int index2 = ipaddress2.IndexOf(":");
                    int index3 = ipaddress3.IndexOf(":");
                    int index4 = ipaddress4.IndexOf(":");

                    if (index1 >= 0)
                    {
                        strbuilder.Append("name1 :'" + ipaddress1.Substring(index1 + 1, ipaddress1.Length - index1 - 1) + "',");
                        strbuilder.Append("IP1 :'" + ipaddress1.Substring(0, index1) + "',");
                    }
                    else
                    {
                        strbuilder.Append("name1 :' ',");
                        strbuilder.Append("IP1 :'" + ipaddress1 + "',");
                    }

                    if (index2 >= 0)
                    {
                        strbuilder.Append("name2 :'" + ipaddress2.Substring(index2 + 1, ipaddress2.Length - index2 - 1) + "',");
                        strbuilder.Append("IP2 :'" + ipaddress2.Substring(0, index2) + "',");
                    }
                    else
                    {
                        strbuilder.Append("name2 :' ',");
                        strbuilder.Append("IP2 :'" + ipaddress2 + "',");
                    }

                    if (index3 >= 0)
                    {
                        strbuilder.Append("name3 :'" + ipaddress3.Substring(index3 + 1, ipaddress3.Length - index3 - 1) + "',");
                        strbuilder.Append("IP3 :'" + ipaddress3.Substring(0, index3) + "',");
                    }
                    else
                    {
                        strbuilder.Append("name3 :' ',");
                        strbuilder.Append("IP3 :'" + ipaddress3 + "',");
                    }

                    if (index4 >= 0)
                    {
                        strbuilder.Append("name4 :'" + ipaddress4.Substring(index4 + 1, ipaddress4.Length - index4 - 1) + "',");
                        strbuilder.Append("IP4 :'" + ipaddress4.Substring(0, index4) + "',");
                    }
                    else
                    {
                        strbuilder.Append("name4 :' ',");
                        strbuilder.Append("IP4 :'" + ipaddress4 + "',");
                    }
                }
            }

            if (networkdt != null && networkdt.Rows.Count > 0)
            {
                string connectType = networkdt.Rows[0]["connectType"].ToString();
                strbuilder.Append("connectType :'" + networkdt.Rows[0]["connectType"].ToString() + "'");
            }
            else
            {
                strbuilder.Append("connectType :'" + 0 + "'");
            }
            strbuilder.Append("}");
            return strbuilder.ToString();
        }

        public void SaveServerIpAndNetWorkIpConfig(string formData)
        {
            //string formData = System.Web.HttpContext.Current.Request.Params["formData"];
            JObject jo = JObject.Parse(formData);
            DataTable networkdt = new DataTable();
            networkdt.Columns.Add("ipaddress1", typeof(string));
            networkdt.Columns.Add("ipaddress2", typeof(string));
            networkdt.Columns.Add("ipaddress3", typeof(string));
            networkdt.Columns.Add("ipaddress4", typeof(string));
            networkdt.Columns.Add("ipaddress5", typeof(string));
            networkdt.Columns.Add("ipaddress6", typeof(string));
            networkdt.Columns.Add("connectType", typeof(string));
            
            DataTable dt = new DataTable();
            dt.Columns.Add("servername", typeof(string));
            dt.Columns.Add("serverip", typeof(string));
            dt.Columns.Add("serverport", typeof(string));

            DataRow dr1 = dt.NewRow();
            dr1["servername"] = jo["name4"];
            dr1["serverip"] = jo["IP4"];
            dr1["serverport"] = jo["port"];
            dt.Rows.Add(dr1);

            DataRow dr2 = dt.NewRow();
            dr2["servername"] = jo["name1"];
            dr2["serverip"] = jo["IP1"];
            dr2["serverport"] = jo["port"];
            dt.Rows.Add(dr2);

            DataRow dr3 = dt.NewRow();
            dr3["servername"] = jo["name2"];
            dr3["serverip"] = jo["IP2"];
            dr3["serverport"] = jo["port"];
            dt.Rows.Add(dr3);

            DataRow dr4 = dt.NewRow();
            dr4["servername"] = jo["name3"];
            dr4["serverip"] = jo["IP3"];
            dr4["serverport"] = jo["port"];
            dt.Rows.Add(dr4);

            dt.AcceptChanges();

            if (networkdt.Rows.Count > 0)
            {
                networkdt.Rows[0]["ipaddress1"] = jo["IP4"];
                networkdt.Rows[0]["ipaddress2"] = jo["IP1"];
                networkdt.Rows[0]["ipaddress3"] = jo["IP2"];
                networkdt.Rows[0]["ipaddress4"] = jo["IP3"];
                networkdt.Rows[0]["ipaddress5"] = "";
                networkdt.Rows[0]["ipaddress6"] = "";
                networkdt.Rows[0]["connectType"] = jo["connectType"];
            }
            else
            {
                DataRow dr = networkdt.NewRow();

                dr["ipaddress1"] = jo["IP4"];
                dr["ipaddress2"] = jo["IP1"];
                dr["ipaddress3"] = jo["IP2"];
                dr["ipaddress4"] = jo["IP3"];
                dr["ipaddress5"] = "";
                dr["ipaddress6"] = "";
                dr["connectType"] = jo["connectType"];
                networkdt.Rows.Add(dr);
            }
            bool status =proxy.SaveServerIpAndNetWorkIpConfig(dt, networkdt);
        }

        public string LoadDisplaySetting()
        {
            string ucode = AppInfoBase.UCode;
            string []s = proxy.LoadDisplaySetting(ucode);            
            string xmlFileFunctionTreeShow = AppDomain.CurrentDomain.BaseDirectory + @"/CONFIG\FunctionTreeShow.xml";
            DataSet dsFunctionTreeShow = MenuConfigProxy.ConvertXMLToDataSet(xmlFileFunctionTreeShow); //路径怎么破
            DataTable dtFunctionTreeShow = dsFunctionTreeShow.Tables[0];
            string stringCheckedPhysicalMemory = dtFunctionTreeShow.Rows[0]["CheckedPhysicalMemory"].ToString();

            string xmlFileCheckCodeConfig = AppDomain.CurrentDomain.BaseDirectory + @"/CONFIG\CheckCodeConfig.xml";
            DataSet dsIfUserCheckCode = MenuConfigProxy.ConvertXMLToDataSet(xmlFileCheckCodeConfig); //路径怎么破
            DataTable dtIfUserCheckCode = dsIfUserCheckCode.Tables[0];
            string stringIfUserCheckCode = dtIfUserCheckCode.Rows[0]["IfUserCheckCode"].ToString();

            if (s[0] == "" || s[0] == null)
            {
                s[0] = "";
            }
            if (s[1] == "" || s[1] == null)
            {
                s[1] = "1";
            }
            if (stringCheckedPhysicalMemory == "" || stringCheckedPhysicalMemory == null)
            {
                stringCheckedPhysicalMemory = "1";
            }
            if (stringIfUserCheckCode == "" || stringIfUserCheckCode == null)
            {
                stringIfUserCheckCode = "1";
            }
            string param = "{title : '"+ s[0] + "',codeShowId : '" + s[1] + "',CheckedPhysicalMemory : '" + stringCheckedPhysicalMemory + "',IfUserCheckCode : '" + stringIfUserCheckCode + "'}";
            return param;
        }
        public bool SaveDisplaySetting(string formData)
        {
            //string formData = System.Web.HttpContext.Current.Request.Params["formData"];
            string ucode = AppInfoBase.UCode;
            JObject jo = JObject.Parse(formData);
            string[] s = { jo["title"].ToString(), jo["codeShowId"].ToString() };
            bool sta = proxy.SaveDisplaySetting(ucode, s);
            XmlDocument doc = new XmlDocument();
            string configPath = AppDomain.CurrentDomain.BaseDirectory;
            //doc.Load(@"/CONFIG\FunctionTreeShow.xml");
            doc.Load(configPath + "\\CONFIG\\FunctionTreeShow.xml");
            XmlElement root = doc.DocumentElement;
            XmlNodeList nodes = root.GetElementsByTagName("showflag");
            XmlElement ele = (XmlElement)nodes[0];
            XmlElement CheckedPhysicalMemory = (XmlElement)ele.GetElementsByTagName("CheckedPhysicalMemory")[0];
            CheckedPhysicalMemory.InnerText = jo["CheckedPhysicalMemory"].ToString();
            doc.Save(configPath + "\\CONFIG\\FunctionTreeShow.xml");

            //doc.Load(@"/CONFIG\CheckCodeConfig.xml");
            doc.Load(configPath + "\\CONFIG\\CheckCodeConfig.xml");
            root = doc.DocumentElement;
            nodes = root.GetElementsByTagName("IfUserCheckCode");
            ele = (XmlElement)nodes[0];
            XmlElement IfUserCheckCode = (XmlElement)ele.GetElementsByTagName("IfUserCheckCode")[0];
            IfUserCheckCode.InnerText = jo["IfUserCheckCode"].ToString();
            doc.Save(configPath + "\\CONFIG\\CheckCodeConfig.xml");
            //if (sta)
            //{
            //    return "{status : \"ok\"}";
            //}
            //else
            //{
            //    return "{status : \"error\"}";
            //}
            return sta;
        }
        //string ucode = AppInfoBase.UCode;
        //F:\i6sMidServer\NGWebSite\CONFIG\FunctionTreeShow.xml
        //select file_value from fg_systemconfigfile where file_key='CODESHOWID' //合法用户显示账套
        //F:\i6sMidServer\NGWebSite\CONFIG\CheckCodeConfig.xml 

        //public string GetSSOOrg()
        //{
        //    return proxy.GetSSOOrg();
        //}

        public bool SaveAlertItem(string formData)
        {
            JArray jo = JArray.Parse(formData);
            //string[] s = { jo["title"].ToString(), jo["codeShowId"].ToString() };
            //bool sta = proxy.SaveDisplaySetting(ucode, s);
            string product = AppInfoBase.UP.Product;
            string series = AppInfoBase.UP.Series;
            long userid = AppInfoBase.UserID;

            XmlDocument doc = new XmlDocument();
            string xmlFile = AppDomain.CurrentDomain.BaseDirectory + @"/" + product + series + "Config/" + product + series + "AlertConfig.xml";
            int flag = 0;
       
            doc.Load(xmlFile);
            XmlElement root = doc.DocumentElement;
            XmlNodeList nodes = root.GetElementsByTagName("TaskContent");
            for(int i = 0; i < jo.Count; i++)
            {
                int.TryParse(jo[i]["id"].ToString(), out flag);
                XmlElement ele = (XmlElement)nodes[flag];
                XmlElement Alert = (XmlElement)ele.GetElementsByTagName("Alert")[0];
                Alert.InnerText = jo[i]["value"].ToString()=="True"? "false" : "true";
            }
            doc.Save(xmlFile);
            return true;
        }

        public string AddPicUpload()
        {
            try
            {
                string dirPath = Request.PhysicalApplicationPath + "NG3Resource\\APPLogo";
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                HttpPostedFileBase imgFile = Request.Files["addPic"];
                if (imgFile.FileName.Substring(imgFile.FileName.LastIndexOf(".")) != ".png")
                {
                    return DataConverterHelper.SerializeObject(new
                    {
                        success = false,
                        message = "请上传300*300的png图片"
                    });
                }

                string filePath = dirPath + "\\" + imgFile.FileName;
                string width;
                string height;
                Image image;
                using (Stream fs = imgFile.InputStream)
                {
                    image = Image.FromStream(fs);
                    width = image.Width.ToString();
                    height = image.Height.ToString();
                }

                if (width != "300" && height != "300")
                {
                    return DataConverterHelper.SerializeObject(new
                    {
                        success = false,
                        message = "请上传300*300的png图片"
                    });
                }

                image.Save(filePath);
                return DataConverterHelper.SerializeObject(new
                {
                    success = true,
                    filename = imgFile.FileName
                });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string LoadDefaultOpenTabForMainFrame()
        {
            DataTable dt = proxy.LoadDefaultOpenTabForMainFrame();
            int totalRecord = 0;
            string json = DataConverterHelper.ToJson(dt, totalRecord);
            return json;
        }

    }


}

