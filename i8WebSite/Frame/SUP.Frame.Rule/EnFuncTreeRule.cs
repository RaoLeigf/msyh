using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SUP.Frame.DataAccess;
using SUP.Common.Base;
using SUP.Frame.DataEntity;
using NG.KeepConn;
using Newtonsoft.Json;
using Enterprise3.Rights.AnalyticEngine;

using NG3.Data.Service;
namespace SUP.Frame.Rule
{
    public class EnFuncTreeRule
    {
        private EnFuncTreeDac dac = null;
        private bool show = false;
        SUP.Right.Rules.Services rigthService;
        private RightData rightdata = null;

        public EnFuncTreeRule()
        {
            rightdata = new RightData();
            dac = new EnFuncTreeDac();
            rigthService = new Right.Rules.Services();
        }

        //分割线/////////系统功能树的搜索//////////////////////////////////////////
        public IList<TreeJSONBase> Query(string product, bool isusbuser, string usertype, Int64 orgID, Int64 userID, string condition)
        {
            string sql = string.Empty;
            string filter = string.Empty;
            filter = "(pid is null or pid='')";
            DataTable dt = this.QueryData(product, isusbuser, usertype, orgID, userID, TreeDataLevelType.LazyLevel, condition);
            RemoveNode(dt);

            //取菜单在系统功能树中的套件
            sql = string.Format(@"select * from fg3_menu where menutype='1'");
            DataTable sysTreeMenu = DbHelper.GetDataTable(sql);
            dt.Columns.Add("originalsuite", typeof(string));
            foreach (DataRow dr in dt.Rows)
            {
                DataRow[] sysDr = sysTreeMenu.Select(string.Format(@"code = '{0}'", dr["code"].ToString()));
                if (sysDr != null && sysDr.Length > 0)
                {
                    dr["originalsuite"] = sysDr[0]["suite"];
                }
            }

            return new EnFuncTreeBuilder().GetExtTreeList(dt, "pid", "id", filter, TreeDataLevelType.TopLevel);

        }
        public DataTable QueryData(string product, bool isusbuser, string usertype, Int64 orgID, Int64 userID, TreeDataLevelType level, string condition)
        {
            DataTable querydt = dac.Query(product, condition);
            DataTable alldt = dac.LoadTable(product);
            DataTable finaldt = GetQueryResult(querydt, alldt);
            finaldt.DefaultView.Sort = "seq";
            DataTable dt = RigthFiltrateExt(usertype, finaldt, TreeDataLevelType.LazyLevel);
            this.DealLang(dt);
            return dt;
        }
        public DataTable GetQueryResult(DataTable querydt, DataTable alldt)
        {
            DataTable newTable = querydt.Copy();
            //newTable.Columns.Add(new DataColumn("expanded", typeof(Boolean)));//展开叶子节点

            // int i,l;
            foreach (DataRow dr in querydt.Rows)
            {
                //if (!Isexist(newTable, dr["id"].ToString()))
                //{
                //    DataRow newRow = newTable.NewRow();
                //    CopyRows(newRow, dr);
                //    newTable.Rows.Add(newRow);
                //    //newTable.ImportRow(newRow);

                //}

                string pid = dr["pid"].ToString();
                string suite = dr["suite"].ToString();
                AddFather(newTable, alldt, pid, suite);

                string id = dr["id"].ToString();
                AddChild(newTable, alldt, id, suite);

                //DataRow[] childrows = fg3.Select(string.Format("pid='{0}'", id));   
                //if (childrows.Length != 0)
                //{
                //    i = 0;
                //    l = childrows.Length;
                //    while (l>0)
                //    {
                //        if (!Isexist(newTable, childrows[i]["id"].ToString()))
                //        {
                //            DataRow newRow3 = newTable.NewRow();
                //            CopyRows(newRow3, childrows[i]);
                //            newTable.Rows.Add(newRow3);                           
                //        }
                //        AddChild(newTable, fg3, childrows[i]["id"].ToString());
                //        i++;
                //        l--;
                //    }
                //}

            }
            return newTable;
        }
        private void AddFather(DataTable newTable, DataTable alldt, string pid, string suite)
        {
            while (!string.IsNullOrEmpty(pid))
            {
                DataRow[] fatherRow = alldt.Select(string.Format("id='{0}' and suite='{1}'", pid, suite));
                if (fatherRow.Length == 0)
                    break;
                if (!Isexist(newTable, fatherRow[0]["id"].ToString()))
                {
                    DataRow newRow = newTable.NewRow();
                    CopyRows(newRow, fatherRow[0]);
                    newRow["expanded"] = "1";//父节点展开
                    newTable.Rows.Add(newRow);
                }
                pid = fatherRow[0]["pid"].ToString();

            }

        }
        private void AddChild(DataTable newTable, DataTable alldt, string id, string suite)
        {
            DataRow[] childrows = alldt.Select(string.Format("pid='{0}' and suite='{1}'", id, suite));
            int i = 0;
            int length = childrows.Length;
            while (length > 0)
            {
                if (!Isexist(newTable, childrows[i]["id"].ToString()))
                {
                    DataRow newRow = newTable.NewRow();
                    CopyRows(newRow, childrows[i]);
                    newTable.Rows.Add(newRow);

                    AddChild(newTable, alldt, childrows[i]["id"].ToString(), childrows[i]["suite"].ToString());
                }
                i++;
                length--;
            }
        }

        private bool Isexist(DataTable querydt, string id)
        {
            DataRow[] dr = querydt.Select(string.Format("id='{0}'", id));
            if (dr.Length == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void CopyRows(DataRow newRow, DataRow dataRow)
        {
            newRow.ItemArray = dataRow.ItemArray;
            return;
        }

        //分割线/////////////////////////////////////////////////////

        public IList<TreeJSONBase> LoadMenu(string product, string suite, bool isusbuser, string usertype, Int64 orgID, Int64 userID, string nodeid)
        {
            string sql = string.Empty;
            string filter = string.Empty;           
            
            if ("root" == nodeid)//首次加载
            {
                DataTable dt = this.GetMainTreeData(product, suite, isusbuser, usertype, orgID, userID, nodeid, TreeDataLevelType.TopLevel);
                filter = "(pid is null or pid='')";
                return new EnFuncTreeBuilder().GetExtTreeList(dt, "pid", "id", filter, TreeDataLevelType.TopLevel, 3);//加载两层
            }
            else
            {
                DataTable dt = this.GetMainTreeData(product, suite, isusbuser, usertype, orgID, userID, nodeid, TreeDataLevelType.LazyLevel);
                return new EnFuncTreeBuilder().GetExtTreeList(dt, "pid", "id", filter, TreeDataLevelType.LazyLevel);
            }
            
        }

        /// <summary>
        /// 取得功能树数据
        /// </summary>
        /// <param name="product">产品名称</param>
        /// <param name="suite">套件</param>
        /// <param name="isusbuser">是否是usb用户</param>
        /// <param name="usertype">用户类型</param>
        /// <param name="ocode">组织号</param>
        /// <param name="logid">用户id</param>
        /// <returns></returns>
        public DataTable GetMainTreeData(string product, string suite, bool isusbuser, string usertype, Int64 orgID, Int64 userID, string nodeid, TreeDataLevelType level)
        {
            DataTable menudt = dac.LoadMenuData(product, suite, nodeid).Copy();
            string sql = string.Format(@"select * from fg3_menu where menutype='1'");
            DataTable sysTreeMenu = DbHelper.GetDataTable(sql);
            menudt.Columns.Add("originalsuite", typeof(string));
            foreach(DataRow dr in menudt.Rows)
            {
                DataRow[] sysDr = sysTreeMenu.Select(string.Format(@"code = '{0}'", dr["code"].ToString()));
                if(sysDr!=null && sysDr.Length > 0)
                {
                    dr["originalsuite"] = sysDr[0]["suite"];
                }
            }
            menudt = RightkeyController(menudt);            
            //DataTable dt = menudt;
            DataTable dt = RigthFiltrateExt(usertype, menudt, level);
            this.DealLang(dt);
            return dt;

            //return menudt;
            //return RigthFiltrateExt(usertype, menudt);
        }
        
        public DataTable RigthFiltrateExt(string usertype, DataTable menudt, TreeDataLevelType level)
        {
            rightdata.LoginConnectionStr = NG3.AppInfoBase.UserConnectString;
            //用户的菜单权限
            DataTable dtUserPageRights = rightdata.UserPageRightsDtExt; //cpcmInterface.GetAllPageRights(ocode, logid);            
            //所有菜单权限
            DataTable dtAllPageRights = rightdata.AllPageRightsDt;//cpcmInterface.GetAllPageRights();


            for (int i = menudt.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = menudt.Rows[i];

                string norightcontrol = dr["norightcontrol"].ToString();
                if (norightcontrol == "1")
                {
                    if (show)
                    {
                        dr["name"] += "(不控制权限)";
                    }
                    continue;//不需要权限控制标记为:"1"，跳过
                }

                //功能节点
                string functionnodeflag = dr["functionnode_flag"].ToString();
                string url = dr["url"].ToString();
                string nodeid = dr["id"].ToString();
                string apptype = dr["apptype"].ToString();
                string rightname = dr["rightname"].ToString();//权限名、权限号

                string moduleno = dr["moduleno"].ToString();
                //string rightid = dr["rightid"].ToString();
                string funcname = (dr["functionname"] == null || dr["functionname"] == DBNull.Value) ? string.Empty : dr["functionname"].ToString();

                Int64 rightkey = 0;
                Int64.TryParse(dr["rightkey"].ToString(), out rightkey);

                if ("ProjectOpportunityListManager" == rightname)
                {

                }

                //winform
                if (apptype == AppType.WinForm)
                {

                    if (rightkey == -1)
                    {
                        if (show)
                        {
                            dr["name"] += "(win权限error)";
                        }
                        continue;
                    }
                    else
                    {
                        DataRow[] drs = dtUserPageRights.Select(string.Format("rightkey={0}", rightkey));
                        if (drs.Length == 0)
                        {
                            if (show)
                            {
                                dr["name"] += "(win无权限)";
                            }
                            else
                            {
                                dr.Delete();
                            }
                        }
                    }
                }
                else if (apptype == AppType.PB)//pb
                {

                    #region intfi网银控制,为提高性能,暂时先放在pb权限控制这一段

                    if (dr["ebankflg"] != null && dr["ebankflg"] != DBNull.Value && !string.IsNullOrEmpty(dr["ebankflg"].ToString()))
                    {
                        string ebankflg = dr["ebankflg"].ToString();

                        if (ebankflg.Equals("1"))
                        {
                            NGCOM ngcom = new NGCOM();
                            string flag = ngcom.GetEbank();
                            if (flag.Equals("0"))
                            {
                                if (show)
                                {
                                    dr[""] += "(无网银)";
                                    //ShowTitle(doc, node, "(无网银)");//测试用
                                }
                                else
                                {
                                    dr.Delete();
                                }
                                continue;
                            }
                        }
                    }

                    #endregion


                    if (rightkey == 0)
                    {
                        if (show)
                        {
                            dr["name"] += "(pb权限error)";
                        }
                        continue;
                    }
                    else if (rightkey == -1)
                    {
                        if (show)
                        {
                            dr["name"] += "(pb权限error)";
                        }
                        continue;
                    }
                    else
                    {

                        DataRow[] drs = dtUserPageRights.Select(string.Format("rightkey={0}", rightkey));
                        if (drs.Length == 0)
                        {
                            if (show)
                            {
                                dr["name"] += "(pb无权限)";
                            }
                            else
                            {
                                dr.Delete();
                            }
                        }
                    }

                }
                else if (apptype == AppType.WebForm || apptype == AppType.WebMvc)//web
                {
                    if (rightkey == -1)
                    {
                        if (show)
                        {
                            dr["name"] += "(web权限error)";
                        }
                        continue;
                    }
                    else
                    {
                        DataRow[] drs = dtUserPageRights.Select(string.Format("rightkey={0}", rightkey));
                        if (drs.Length == 0)
                        {
                            if (show)
                            {
                                dr["name"] += "(web无权限)";
                            }
                            else
                            {
                                dr.Delete();
                            }
                        }
                    }
                }
            }

            menudt.AcceptChanges();
            if (level == TreeDataLevelType.TopLevel)
            {
                this.RemoveNode(menudt);
            }
            menudt.AcceptChanges();

            //this.DealLang(menudt);

            return menudt;
        }
        //如果不控制权限，把right设置为0
        public DataTable RightkeyController(DataTable menudt)
        {
            for (int i = menudt.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = menudt.Rows[i];
                string norightcontrol = dr["norightcontrol"].ToString();
                if (norightcontrol == "1")
                {
                    dr["rightkey"] = "0";
                }
            }
            return menudt;
        }

        //多语言处理
        private void DealLang(DataTable menudt)
        {
            Dictionary<string, string> langDic = SUP.Common.DataAccess.LangInfo.GetLabelLang("bustree");

            //处理掉父节点的seq小于子节点seq的那些行
            for (int i = menudt.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = menudt.Rows[i];

                if (dr["langkey"] != null && dr["langkey"] != DBNull.Value)
                {
                    string langKey = dr["langkey"].ToString();//多语言用code当语言编码
                    if (langDic.ContainsKey(langKey) && !string.IsNullOrWhiteSpace(langDic[langKey]))
                    {
                        dr["name"] = langDic[langKey];
                    }
                }
            }
        }

        private void RemoveNode(DataTable menudt)
        {
            Dictionary<string, string> langDic = SUP.Common.DataAccess.LangInfo.GetLabelLang("SystemMenu");

            //处理掉父节点的seq小于子节点seq的那些行
            for (int i = menudt.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = menudt.Rows[i];
                string id = dr["id"].ToString();

                if (dr["functionnode_flag"].ToString() != "1" )//非功能节点
                {
                    DataRow[] tempdr = menudt.Select("pid='" + id + "'");
                    if (tempdr.Length == 0)
                    {
                        dr.Delete();
                    }
                }
            }

            //清除剩余的节点，父节点seq大于子节点seq
            for (int i = menudt.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = menudt.Rows[i];
                if (dr.RowState == DataRowState.Deleted) continue;

                string id = dr["id"].ToString();
                if (dr["functionnode_flag"].ToString() == "0")//非功能节点
                {
                    bool hit = false;
                    IsValidPath(id, menudt, ref hit);
                    if (!hit)
                    {
                        dr.Delete();
                    }
                }
            }
        }

        //检测是否是有效路径，从当前节点出发能到达叶子节点
        //递归处理，容易引起性能问题，最好是toolkit导出的时候菜单的顺序排好，
        //保证父节点的seq小于子节点的seq，就不需要递归检测
        private void IsValidPath(string id, DataTable menudt, ref bool hit)
        {
            if (hit)
            {
                return;
            }
            DataRow[] tempdr = menudt.Select("pid='" + id + "'");
            if (tempdr.Length > 0)
            {
                foreach (DataRow dr in tempdr)
                {
                    string flg = dr["functionnode_flag"].ToString();
                    if (flg == "1")
                    {
                        hit = true;
                        return;
                    }

                    if (!hit)
                    {
                        string tempid = dr["id"].ToString();
                        IsValidPath(tempid, menudt, ref hit);
                    }
                    else
                    {
                        break;//已经检测到有效，跳出循环
                    }

                }
            }
        }
        
        //public IList<IndivadualSuiteInfoEntity> GetSuiteList()
        //{
        //    DataTable dt = dac.GetSuiteList();
        //    string str = JsonConvert.SerializeObject(dt);
        //    IList<IndivadualSuiteInfoEntity> suites = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<IndivadualSuiteInfoEntity>>(str);
        //    return suites;
        //}

        public IList<SuiteInfoEntity> GetSuiteList()
        {

            List<SuiteInfoEntity> list = new List<SuiteInfoEntity>();
            //取套件权限
            //DataTable suitrightdt = rigthService.GetSuitRight(NG3.AppInfoBase.OCode, NG3.AppInfoBase.LoginID);
            //DataTable dt = rigthService.ReadProductInfo().Tables["SuitInfo"];
            DataTable dt = dac.GetSuiteList();

            //取套件权限
            Enterprise3.Rights.AnalyticEngine.FuncRightControl cpcmInterface = new FuncRightControl();
            DataTable suitrightdt = new DataTable();
            for (int i = 0; i < 3; i++)
            {
                suitrightdt = cpcmInterface.GetSuitRight(NG3.AppInfoBase.OrgID, NG3.AppInfoBase.UserID);
                if (suitrightdt == null || suitrightdt.Rows.Count == 0)
                {
                    System.Threading.Thread.Sleep(500);
                }
                else
                {
                    break;
                }
            }
            //获取所有套件
            //string currentCultureName = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
            //DataTable dt = MainFrameUIP.GetSuitList(i6Culture.GetProductXmlFilePath(MainCommonUIP.GetPath(), currentCultureName)).Tables[1];


            foreach (DataRow dr in dt.Rows)
            {
                string module = dr["suite"].ToString();

                //普通用户和非DMC模块要判断套件权限
                if ((NG3.AppInfoBase.UserType == SUP.Common.Base.UserType.OrgUser && "PUB" != module.ToUpper()))
                {
                    DataRow[] drs = suitrightdt.Select(string.Format("suit='{0}' and ishas=true", module));
                    if (drs.Length == 0)
                    {
                        continue;//无权限
                    }

                }

                SuiteInfoEntity entity = new SuiteInfoEntity();
                entity.Code = dr["suite"].ToString();
                entity.Name = dr["Name"].ToString();
                list.Add(entity);

            }

            return list;
        }

    }


    class EnFuncTreeJSONBase : TreeJSONBase
    {
        public virtual string url { get; set; }
        public virtual string rightname { get; set; }
        public virtual string functionname { get; set; }
        public virtual string code { get; set; }
        public virtual string rightkey { get; set; }
        public virtual string managername { get; set; }
        public virtual string moduleno { get; set; }
        public virtual string suite { get; set; }
        public virtual int busphid { get; set; }
        public virtual string originalsuite { get; set; }
    }

    public class EnFuncTreeBuilder : ExtJsTreeBuilderBase
    {
        public override TreeJSONBase BuildTreeNode(DataRow dr)
        {
            //string root = System.Web.HttpContext.Current.Request.ApplicationPath; //+ "/";

            //if (root != "/")//
            //{
            //    if (dr["url"].ToString().StartsWith("/") == false)//有些url以"/"起头，不需要加了
            //    {
            //        root += "/";
            //    }
            //}

            EnFuncTreeJSONBase node = new EnFuncTreeJSONBase();

            //string functionname = dr["functionname"].ToString();

            node.id = dr["id"].ToString();
            node.text = dr["name"].ToString();
            node.rightname = dr["rightname"].ToString();
            node.rightkey = dr["rightkey"].ToString();
            node.managername = dr["managername"].ToString();
            node.suite = dr["suite"].ToString();
            node.moduleno = dr["moduleno"].ToString();
            //node.functionname = dr["functionname"].ToString();
            node.code = dr["code"].ToString();
            //node.leaf = (dr["functionnode_flag"].ToString() == "1") ? true : false;
            node.leaf = string.IsNullOrWhiteSpace(dr["url"].ToString()) ? false : true;
            //if (dr.Table.Columns.Contains("expanded"))//搜索的时候才有这列
            //{
            //    if (dr["expanded"] != null && dr["expanded"] != DBNull.Value)
            //    {
            //        node.expanded = (bool)dr["expanded"];
            //    }
            //    else {
            //        node.expanded = false;
            //    }
            //}
            node.expanded = (dr["expanded"].ToString() == "1") ? true : false;
            node.url = dr["url"].ToString();
            //if(string.IsNullOrEmpty(d))
            //if (string.IsNullOrWhiteSpace(functionname))
            //{
            //    node.url = root + dr["url"].ToString();
            //}
            //else
            //{
            //    string url = dr["url"].ToString();
            //    if (url.IndexOf("?") > 0)
            //    {
            //        node.url = root + url + "&FuncName=" + functionname;
            //    }
            //    else
            //    {
            //        node.url = root + url + "?FuncName=" + functionname;
            //    }
            //}
            node.allowDrag = string.IsNullOrWhiteSpace(dr["url"].ToString()) ? false : true;
            int busphid = 0;
            int.TryParse(dr["busphid"].ToString(), out busphid);
            node.busphid = busphid;
            node.originalsuite = dr["originalsuite"].ToString();
            return node;
        }
    }
}
