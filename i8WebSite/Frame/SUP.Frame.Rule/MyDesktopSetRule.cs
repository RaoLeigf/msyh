using GE.BusinessRules.Common;
using GE.DataEntity.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SUP.Frame.DataAccess;
using SUP.Common.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using NG.Cache.Client;
using SUP.Common.Rule;

namespace SUP.Frame.Rule
{
    public class MyDesktopSetRule:IUserConfig
    {
        private MyDesktopSetDac dac = null;

        public MyDesktopSetRule()
        {
            dac = new MyDesktopSetDac();
        }

        public string GetMyDesktopFuncIconData()
        {
            List<MyDesktopInfo> myDesktopInfo = new List<MyDesktopInfo>();
            List<FuncIconEntity> funcIconEntitys = new List<FuncIconEntity>();
            PubCommonRule pubCommonRule = new PubCommonRule();

            object phid = dac.GetMyDesktopIdByUserID(NG3.AppInfoBase.UserID);
            
            if (phid == null)
            {
                return pubCommonRule.GetMyDesktopInitData();
            }
            else
            {
                InitMyDesktopData();
                myDesktopInfo = GetMyDesktopData();
                DataTable dt = pubCommonRule.GetFuncIconDt();
                for (int i = 0; i < myDesktopInfo.Count; i++)
                {
                    List<MyDesktopNode> myDesktopNodes = myDesktopInfo[i].MyDesktopNodes;
                    for (int j = 0; j < myDesktopNodes.Count; j++)
                    {
                        MyDesktopNode desktopNode = myDesktopNodes[j];
                        DataView dv = dt.DefaultView;
                        dv.RowFilter = "busphid = '" + desktopNode.Busphid + "'";
                        DataTable tempdt = dv.ToTable();
                        if (tempdt.Rows.Count > 0)
                        {
                            FuncIconEntity funcIconEntity = new FuncIconEntity();
                            funcIconEntity.busphid = desktopNode.Busphid;
                            funcIconEntity.name = tempdt.Rows[0]["name"].ToString();
                            funcIconEntity.src = tempdt.Rows[0]["src"].ToString();
                            funcIconEntitys.Add(funcIconEntity);
                        }
                    }
                }
                string json = "{\"MyDesktopInfo\": " + JsonConvert.SerializeObject(myDesktopInfo) + ",\"FuncIcon\": " + JsonConvert.SerializeObject(funcIconEntitys) + "}";
                return json;
            }
        }

        public List<MyDesktopInfo> GetMyDesktopData()
        {
            string key = NG3.AppInfoBase.UserID + "-MYDESKTOPDATA";
            List<MyDesktopInfo> myDesktopInfo = CacheClient.Instance.GetData(key) as List<MyDesktopInfo>;
            if (myDesktopInfo != null)
            {
                return myDesktopInfo;
            }
            else
            {
                InitMyDesktopData();
                return GetMyDesktopData();
            }
        }

        public void InitMyDesktopData()
        {
            List<MyDesktopInfo> myDesktopInfo = new List<MyDesktopInfo>();

            object phid = dac.GetMyDesktopIdByUserID(NG3.AppInfoBase.UserID);

            if (phid == null)
            {
                string myDesktopInitData = new PubCommonRule().GetMyDesktopInitData();
                myDesktopInfo = JsonConvert.DeserializeObject<List<MyDesktopInfo>>(JObject.Parse(myDesktopInitData)["MyDesktopInfo"].ToString());
            }
            else
            {
                myDesktopInfo = NG.Runtime.Serialization.SerializerBase.DeSerialize((byte[])dac.GetMyDesktopDataByPhid((long)phid)) as List<MyDesktopInfo>;
            }

            string key = NG3.AppInfoBase.UserID + "-MYDESKTOPDATA";
            CacheClient.Instance.Add(key, myDesktopInfo, 120);
        }

        public DataTable GetMyDesktopGroup(ref int totalRecord)
        {
            List<MyDesktopInfo> myDesktopInfo = GetMyDesktopData();
            DataTable dt = new DataTable();
            dt.TableName = "MyDesktopGroup";

            dt.Columns.Add(new DataColumn("index", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("name", Type.GetType("System.String")));

            DataRow dr;
            for (int i = 0; i < myDesktopInfo.Count; i++)
            {
                dr = dt.NewRow();
                dr["index"] = i;
                dr["name"] = myDesktopInfo[i].GroupName;
                dt.Rows.Add(dr);
            }

            totalRecord = myDesktopInfo.Count;
            return dt;
        }

        public DataTable GetMyDesktopGroupEx(string index, ref int totalRecord)
        {
            List<MyDesktopInfo> myDesktopInfo = GetMyDesktopData();
            DataTable dt = new DataTable();
            dt.TableName = "MyDesktopGroupEx";

            dt.Columns.Add(new DataColumn("index", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("name", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("color", Type.GetType("System.String")));

            MyDesktopInfo myDesktop = myDesktopInfo[int.Parse(index)];
            DataRow dr = dt.NewRow();
            dr["index"] = index;
            dr["name"] = myDesktop.GroupName;
            dr["color"] = myDesktop.Menubgcolor != string.Empty ? myDesktop.Menubgcolor : "#97c5f0";
            dt.Rows.Add(dr);

            totalRecord = 1;
            return dt;
        }

        public DataTable GetMyDesktopNode(string index, ref int totalRecord)
        {
            List<MyDesktopInfo> myDesktopInfo = GetMyDesktopData();
            DataTable dt = new DataTable();
            dt.TableName = "MyDesktopGroupNode";

            dt.Columns.Add(new DataColumn("index", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("name", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("size", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("color", Type.GetType("System.String")));            

            MyDesktopInfo myDesktop = myDesktopInfo[int.Parse(index)];
            DataRow dr;
            for (int i = 0; i < myDesktop.MyDesktopNodes.Count; i++)
            {
                dr = dt.NewRow();
                dr["index"] = i;
                dr["name"] = myDesktop.MyDesktopNodes[i].MenuNodeName;
                dr["size"] = myDesktop.MyDesktopNodes[i].MenunodeSize;
                dr["color"] = myDesktop.MyDesktopNodes[i].MenuNodeColor != string.Empty? myDesktop.MyDesktopNodes[i].MenuNodeColor : "#97c5f0";
                dt.Rows.Add(dr);
            }

            totalRecord = myDesktop.MyDesktopNodes.Count;
            return dt;
        }

        public string GetColor()
        {
            string json = "[";
            string[] colors = { "#2eb0bd", "#375d81", "#bea881", "#97c5f0",
            "#8bba30","#f2854c","#ebb741","#ef3a5b","#3f48cc","#ffcc00",
            "#66ccff","#c41a87","#b55d37","#ff9bc6","#75e0e6","#bae1ac"};
            for (int i = 0; i < 16; i++)
            {
                json += "{\"color\":\"" + colors[i] + "\"}";
                if(i != 15)
                {
                    json += ",";
                }
            }
            json += "]";
            return json;
        }

        public bool AddMyDesktopGroup()
        {
            List<MyDesktopInfo> myDesktopInfo = GetMyDesktopData();
            MyDesktopInfo mdi = new MyDesktopInfo();
            mdi.GroupName = "";
            mdi.Menubgcolor = "";
            mdi.MyDesktopNodes = new List<MyDesktopNode>();
            myDesktopInfo.Add(mdi);
            string key = NG3.AppInfoBase.UserID + "-MYDESKTOPDATA";
            CacheClient.Instance.Add(key, myDesktopInfo, 120);
            return true;
        }

        public bool DelMyDesktopGroup(string index)
        {
            List<MyDesktopInfo> myDesktopInfo = GetMyDesktopData();
            myDesktopInfo.RemoveAt(int.Parse(index));
            string key = NG3.AppInfoBase.UserID + "-MYDESKTOPDATA";
            CacheClient.Instance.Add(key, myDesktopInfo, 120);
            return true;
        }

        public bool UpMyDesktopGroup(string index)
        {
            List<MyDesktopInfo> myDesktopInfo = GetMyDesktopData();
            MyDesktopInfo myDesk1 = myDesktopInfo[int.Parse(index)];
            MyDesktopInfo myDesk2 = myDesktopInfo[int.Parse(index) - 1];
            myDesktopInfo[int.Parse(index)] = myDesk2;
            myDesktopInfo[int.Parse(index) - 1] = myDesk1;
            string key = NG3.AppInfoBase.UserID + "-MYDESKTOPDATA";
            CacheClient.Instance.Add(key, myDesktopInfo, 120);
            return true;
        }

        public bool DownMyDesktopGroup(string index)
        {
            List<MyDesktopInfo> myDesktopInfo = GetMyDesktopData();
            MyDesktopInfo myDesk1 = myDesktopInfo[int.Parse(index)];
            MyDesktopInfo myDesk2 = myDesktopInfo[int.Parse(index) + 1];
            myDesktopInfo[int.Parse(index)] = myDesk2;
            myDesktopInfo[int.Parse(index) + 1] = myDesk1;
            string key = NG3.AppInfoBase.UserID + "-MYDESKTOPDATA";
            CacheClient.Instance.Add(key, myDesktopInfo, 120);
            return true;
        }

        public bool UpMyDesktopNode(string groupindex, string nodeindex)
        {
            List<MyDesktopInfo> myDesktopInfo = GetMyDesktopData();
            MyDesktopInfo myDesk = myDesktopInfo[int.Parse(groupindex)];
            MyDesktopNode myDeskNode1 = myDesk.MyDesktopNodes[int.Parse(nodeindex)];
            MyDesktopNode myDeskNode2 = myDesk.MyDesktopNodes[int.Parse(nodeindex) - 1];
            myDesk.MyDesktopNodes[int.Parse(nodeindex)] = myDeskNode2;
            myDesk.MyDesktopNodes[int.Parse(nodeindex) - 1] = myDeskNode1;
            string key = NG3.AppInfoBase.UserID + "-MYDESKTOPDATA";
            CacheClient.Instance.Add(key, myDesktopInfo, 120);
            return true;
        }

        public bool DownMyDesktopNode(string groupindex, string nodeindex)
        {
            List<MyDesktopInfo> myDesktopInfo = GetMyDesktopData();
            MyDesktopInfo myDesk = myDesktopInfo[int.Parse(groupindex)];
            MyDesktopNode myDeskNode1 = myDesk.MyDesktopNodes[int.Parse(nodeindex)];
            MyDesktopNode myDeskNode2 = myDesk.MyDesktopNodes[int.Parse(nodeindex) + 1];
            myDesk.MyDesktopNodes[int.Parse(nodeindex)] = myDeskNode2;
            myDesk.MyDesktopNodes[int.Parse(nodeindex) + 1] = myDeskNode1;
            string key = NG3.AppInfoBase.UserID + "-MYDESKTOPDATA";
            CacheClient.Instance.Add(key, myDesktopInfo, 120);
            return true;
        }

        public string AddMyDesktopNode(string json, string groupname, string index)
        {
            JArray jo = JArray.Parse(json);
            List<MyDesktopInfo> myDesktopInfo = GetMyDesktopData();

            if (!string.IsNullOrEmpty(index))
            {
                MyDesktopInfo myDesk = myDesktopInfo[int.Parse(index)];
                myDesk.GroupName = groupname;
            }
            
            for (int i = 0; i < jo.Count; i++)
            {                
                string managername = string.Empty;
                string runType = string.Empty;
                string runaddr = string.Empty;
                string suite = string.Empty;
                string moduleno = string.Empty;
                string menuname = string.Empty;
                string code = string.Empty;
                string rightkey = string.Empty;
                string busphid = string.Empty;

                if (jo[i]["text"] != null)
                {
                    menuname = jo[i]["text"].ToString();
                }
                else if (jo[i]["name"] != null)
                {
                    menuname = jo[i]["name"].ToString();
                }

                if (jo[i]["code"] != null)
                {
                    code = jo[i]["code"].ToString();
                }
                else if (jo[i]["phid"] != null)
                {
                    code = jo[i]["phid"].ToString();
                }

                if (jo[i]["rightkey"] != null)
                {
                    rightkey = jo[i]["rightkey"].ToString();
                }

                if (jo[i]["busphid"] != null)
                {
                    busphid = jo[i]["busphid"].ToString();
                }

                if (jo[i]["managername"] != null)
                {
                    managername = jo[i]["managername"].ToString();
                }
                if (string.IsNullOrEmpty(managername))
                {
                    if (jo[i]["url"].ToString().IndexOf("exe") > -1)
                    {
                        runType = "4";//pb页面
                    }
                    else
                    {
                        runType = "5";//web页面
                    }
                    runaddr = jo[i]["url"].ToString();
                }
                else
                {
                    runType = "3";//winform页面
                    runaddr = jo[i]["managername"] + "@@**" + jo[i]["rightname"];
                }

                if (jo[i]["suite"] != null)
                {
                    suite = jo[i]["suite"].ToString();
                }
                if (jo[i]["moduleno"] != null)
                {
                    moduleno = jo[i]["moduleno"].ToString();
                }

                if (jo[i]["originalcode"] != null && string.IsNullOrEmpty(jo[i]["originalcode"].ToString())  
                    && string.IsNullOrEmpty(suite) && string.IsNullOrEmpty(moduleno) && jo[i]["url"] != null && jo[i]["urlparm"] != null)
                {
                    string url = jo[i]["url"].ToString();
                    string urlparm = jo[i]["urlparm"].ToString();
                    if (url.Substring(0, 7) == "http://")
                    {
                        runaddr = "WebBrowseIndividualManager№,№Caption№=№" + menuname + "№,№Url№=№" + url + urlparm;
                    }
                    else
                    {
                        runaddr = "LocalSoft" + url + "№,№" + urlparm;                        
                    }
                }

                int groupIndex = !string.IsNullOrEmpty(index) ? int.Parse(index) : -1;

                //判断分组是否存在,不存在则添加分组
                if (groupIndex == -1)
                {
                    for (int j = 0; j < myDesktopInfo.Count; j++)
                    {
                        if (myDesktopInfo[j].GroupName.Equals(groupname))
                        {
                            groupIndex = j;
                            break;
                        }
                    }
                }
                else
                {
                    myDesktopInfo[groupIndex].GroupName = groupname;
                }

                if (groupIndex != -1)
                {                    
                    //判断分组节点是否存在,不存在则添加节点
                    List<MyDesktopNode> nodes = myDesktopInfo[groupIndex].MyDesktopNodes;
                    bool existCheck = false;
                    foreach (MyDesktopNode node in nodes)
                    {
                        if (node.Code.Equals(code))
                        {
                            existCheck = true;
                            break;
                        }
                    }
                    if (!existCheck)
                    {
                        //添加树节点
                        MyDesktopNode mdn = AddMyDesktopNode(menuname, runType, runaddr, moduleno, suite, code, rightkey, busphid);
                        myDesktopInfo[groupIndex].MyDesktopNodes.Add(mdn);
                    }
                }
                else
                {
                    if (myDesktopInfo.Count >= 45)
                    {
                        return "{\"success\":\"false\",\"error\":\"我的桌面分组数不能超过45!\"}";
                    }
                    else
                    {
                        //添加树节点
                        MyDesktopNode mdn = AddMyDesktopNode(menuname, runType, runaddr, moduleno, suite, code, rightkey, busphid);
                        //添加组节点
                        MyDesktopInfo mdi = new MyDesktopInfo();
                        mdi.GroupName = groupname;
                        mdi.Menubgcolor = "";
                        mdi.MyDesktopNodes = new List<MyDesktopNode>();
                        mdi.MyDesktopNodes.Add(mdn);
                        myDesktopInfo.Add(mdi);
                    }
                }
            }

            string key = NG3.AppInfoBase.UserID + "-MYDESKTOPDATA";
            CacheClient.Instance.Add(key, myDesktopInfo, 120);

            return "{\"success\": \"true\"}";
        }

        public string AddMyDesktopNodeEx(string json, string groupname)
        {
            JObject jo = JObject.Parse(json);
            List<MyDesktopInfo> myDesktopInfo = GetMyDesktopData();

            string managername = string.Empty;
            string runType = string.Empty;
            string runaddr = string.Empty;
            string suite = string.Empty;
            string moduleno = string.Empty;
            string menuname = string.Empty;
            string code = string.Empty;
            string rightkey = string.Empty;
            string busphid = string.Empty;

            if (jo["text"] != null)
            {
                menuname = jo["text"].ToString();
            }
            else if (jo["name"] != null)
            {
                menuname = jo["name"].ToString();
            }

            if (jo["code"] != null)
            {
                code = jo["code"].ToString();
            }

            if (jo["rightkey"] != null)
            {
                rightkey = jo["rightkey"].ToString();
            }

            if (jo["busphid"] != null)
            {
                busphid = jo["busphid"].ToString();
            }

            if (jo["managername"] != null)
            {
                managername = jo["managername"].ToString();
            }
            if (string.IsNullOrEmpty(managername))
            {
                if (jo["url"].ToString().IndexOf("exe") > -1)
                {
                    runType = "4";//pb页面
                }
                else
                {
                    runType = "5";//web页面
                }
                runaddr = jo["url"].ToString();
            }
            else
            {
                runType = "3";//winform页面
                runaddr = jo["managername"] + "@@**" + jo["rightname"];
            }

            if (jo["suite"] != null)
            {
                suite = jo["suite"].ToString();
            }
            if (jo["moduleno"] != null)
            {
                moduleno = jo["moduleno"].ToString();
            }

            if (jo["originalcode"] != null && string.IsNullOrEmpty(jo["originalcode"].ToString())
                    && string.IsNullOrEmpty(suite) && string.IsNullOrEmpty(moduleno) && jo["url"] != null && jo["urlparm"] != null)
            {
                string url = jo["url"].ToString();
                string urlparm = jo["urlparm"].ToString();
                if (url.Substring(0, 7) == "http://")
                {
                    runaddr = "WebBrowseIndividualManager№,№Caption№=№" + menuname + "№,№Url№=№" + url + urlparm;
                }
                else
                {
                    runaddr = "LocalSoft" + url + "№,№" + urlparm;
                }
            }

            int groupIndex = -1;

            //判断分组是否存在,不存在则添加分组
            for (int j = 0; j < myDesktopInfo.Count; j++)
            {
                if (myDesktopInfo[j].GroupName.Equals(groupname))
                {
                    groupIndex = j;
                    break;
                }
            }

            if (groupIndex != -1)
            {
                //判断分组节点是否存在,不存在则添加节点
                List<MyDesktopNode> nodes = myDesktopInfo[groupIndex].MyDesktopNodes;
                bool existCheck = false;
                foreach (MyDesktopNode node in nodes)
                {
                    if (node.Code.Equals(code))
                    {
                        existCheck = true;
                        break;
                    }
                }
                if (!existCheck)
                {
                    //添加树节点
                    MyDesktopNode mdn = AddMyDesktopNode(menuname, runType, runaddr, moduleno, suite, code, rightkey, busphid);
                    myDesktopInfo[groupIndex].MyDesktopNodes.Add(mdn);
                }
                else
                {
                    return "我的桌面已存在该功能点!";
                }
            }
            else
            {
                //添加树节点
                MyDesktopNode mdn = AddMyDesktopNode(menuname, runType, runaddr, moduleno, suite, code, rightkey, busphid);
                //添加组节点
                MyDesktopInfo mdi = new MyDesktopInfo();
                mdi.GroupName = groupname;
                mdi.Menubgcolor = "";
                mdi.MyDesktopNodes = new List<MyDesktopNode>();
                mdi.MyDesktopNodes.Add(mdn);
                myDesktopInfo.Add(mdi);
            }

            string key = NG3.AppInfoBase.UserID + "-MYDESKTOPDATA";
            CacheClient.Instance.Add(key, myDesktopInfo, 120);

            SaveMyDesktopInfo("");
            
            return "";
        }

        public MyDesktopNode AddMyDesktopNode(string text, string runType, string runaddr, string moduleno, string suite, string code, string rightkey, string busphid)
        {
            MyDesktopNode mdn = new MyDesktopNode();
            mdn.MenuNodeName = text;
            mdn.MenunodeSize = "小";
            mdn.MenuNodeColor = "";
            mdn.Id = Guid.NewGuid().ToString();
            mdn.RunType = runType;
            mdn.Runadr = runaddr;
            mdn.Moduleno = moduleno;
            mdn.Suite = suite;
            mdn.Code = code;
            mdn.Rightkey = rightkey;
            mdn.Busphid = busphid;
            return mdn;
        }

        public bool DelMyDesktopNode(string groupindex, string nodeindex)
        {
            List<MyDesktopInfo> myDesktopInfo = GetMyDesktopData();
            MyDesktopInfo myDesk = myDesktopInfo[int.Parse(groupindex)];
            myDesk.MyDesktopNodes.RemoveAt(int.Parse(nodeindex));
            string key = NG3.AppInfoBase.UserID + "-MYDESKTOPDATA";
            CacheClient.Instance.Add(key, myDesktopInfo, 120);
            return true;
        }

        public bool ChangeMyDesktopInfo(string json)
        {
            if (!String.IsNullOrEmpty(json))
            {
                JObject jo = JObject.Parse(json);
                JObject group = JObject.FromObject(jo["group"]);
                JArray nodes = JArray.FromObject(jo["node"]);

                List<MyDesktopInfo> myDesktopData = GetMyDesktopData();
                int index = 0;
                int.TryParse(group["index"].ToString(), out index);
                MyDesktopInfo myDesktopInfo = myDesktopData[index];
                myDesktopInfo.GroupName = group["name"].ToString();
                myDesktopInfo.Menubgcolor = group["color"].ToString();
                for (int i = 0; i < myDesktopInfo.MyDesktopNodes.Count; i++)
                {
                    myDesktopInfo.MyDesktopNodes[i].MenunodeSize = nodes[i]["size"].ToString();
                    myDesktopInfo.MyDesktopNodes[i].MenuNodeColor = nodes[i]["color"].ToString();
                }

                string key = NG3.AppInfoBase.UserID + "-MYDESKTOPDATA";
                CacheClient.Instance.Add(key, myDesktopData, 120);
            }

            return true;
        }

        public string SaveMyDesktopInfo(string json)
        {
            ChangeMyDesktopInfo(json);
            List<MyDesktopInfo> myDesktopInfoList = GetMyDesktopData();
            foreach (MyDesktopInfo myDesktopInfo in myDesktopInfoList)
            {
                if (string.IsNullOrEmpty(myDesktopInfo.GroupName))
                {
                    return "{\"success\":\"false\",\"error\":\"我的桌面分组名称为空!\"}";
                }
                else if (myDesktopInfo.GroupName.Length > 10)
                {
                    return "{\"success\":\"false\",\"error\":\"我的桌面分组名称长度超过10!\"}";
                }
            }

            Save(myDesktopInfoList);
            return "{\"success\": \"true\"}";
        }

        public void Save(List<MyDesktopInfo> myDesktopInfoList)
        {
            byte[] buffer = NG.Runtime.Serialization.SerializerBase.Serialize(myDesktopInfoList);

            string obj = dac.GetMyDesktopInfoCount(NG3.AppInfoBase.UserID, 0);
            if (obj != "0")
            {
                dac.UpdateMyDesktopInfo(buffer, NG3.AppInfoBase.UserID, 0);
            }
            else
            {
                long phid = CommonUtil.GetPhId("fg3_mydesktop");
                dac.InsertMyDesktopInfo(phid, buffer, NG3.AppInfoBase.UserID, 0);
            }

            string key = NG3.AppInfoBase.UserID + "-MYDESKTOPDATA";
            CacheClient.Instance.Add(key, myDesktopInfoList, 120);
            UserConfigDac.UserConfigSave(NG3.AppInfoBase.UserID, 0, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        public bool ChangeMyDesktopSet(string id, string type, string value)
        {
            List<MyDesktopInfo> myDesktopInfoList = GetMyDesktopData();
            int i = int.Parse(id.Substring(0, id.IndexOf("and")));
            int j = int.Parse(id.Substring(id.IndexOf("and") + 3));
            MyDesktopNode node = myDesktopInfoList[i].MyDesktopNodes[j];
            switch (type)
            {
                case "Size":
                    if (value == "Min")
                    {
                        node.MenunodeSize = "小";
                    }
                    else
                    {
                        node.MenunodeSize = "大";
                    }
                    break;
                case "Group":
                    MyDesktopInfo myDesktopInfo = new MyDesktopInfo();
                    for (int k = 0; k < myDesktopInfoList.Count; k++)
                    {
                        if (myDesktopInfoList[k].GroupName == value)
                        {
                            myDesktopInfo = myDesktopInfoList[k];
                            break;
                        }
                    }
                    myDesktopInfoList[i].MyDesktopNodes.Remove(node);
                    myDesktopInfo.MyDesktopNodes.Add(node);
                    break;
                case "Remove":
                    myDesktopInfoList[i].MyDesktopNodes.Remove(node);
                    if (myDesktopInfoList[i].MyDesktopNodes.Count == 0)
                    {
                        myDesktopInfoList.RemoveAt(i);
                    }
                    break;
                default:
                    break;
            }

            Save(myDesktopInfoList);
            return true;
        }

        /// <summary>
        /// 方案拷贝
        /// </summary>
        /// <param name="fromUserId"></param>
        /// <param name="fromUserType"></param>
        /// <param name="toUserId"></param>
        /// <param name="toUserType"></param>
        /// <returns></returns>
        public bool CopyUserConfig(long fromUserId, int fromUserType, long toUserId, int toUserType)
        {
            object data = dac.GetMyDesktopInfoData(fromUserId, fromUserType);

            if (data != null)
            {
                string obj = dac.GetMyDesktopInfoCount(toUserId, toUserType);
                if (obj != "0")
                {
                    dac.UpdateMyDesktopInfo((byte[])data, toUserId, toUserType);
                }
                else
                {
                    long phid = CommonUtil.GetPhId("fg3_mydesktop");
                    dac.InsertMyDesktopInfo(phid, (byte[])data, toUserId, toUserType);
                }
            }

            return true;
        }

        /// <summary>
        /// 方案删除
        /// </summary>
        /// <param name="userid">目标用户phid</param>
        /// <param name="usertype">目标用户类型</param>
        /// <returns></returns>
        public bool DeleteUserConfig(long userid, int usertype)
        {
            if (usertype == 0)
            {
                string key = userid + "-MYDESKTOPDATA";
                List<MyDesktopInfo> myDesktopInfo = CacheClient.Instance.GetData(key) as List<MyDesktopInfo>;
                if (myDesktopInfo != null)
                {
                    CacheClient.Instance.Remove(key);
                }
            }

            return dac.UserConfigDel(userid, usertype);
        }

    }
}
