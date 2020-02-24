using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SUP.Frame.DataAccess;
using SUP.Common.Base;
using SUP.Frame.DataEntity;
using NG.KeepConn;
using NG3.Log.Log4Net;
using Enterprise3.Rights.AnalyticEngine;


namespace SUP.Frame.Rule
{
    public class MainTreeRule
    {
        private MainTreeDac dac = null;
        private bool show = false;
        SUP.Right.Rules.Services rigthService;
        private RightData rightdata = null;

        #region 日志相关
        private ILogger _logger = null;
        internal ILogger Logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = Log4NetLoggerFactory.Instance.CreateLogger(typeof(MainTreeRule), LogType.logrules);
                }
                return _logger;
            }
        }
        #endregion


        public MainTreeRule()
        {
            rightdata = new RightData();
            dac = new MainTreeDac();
            rigthService = new Right.Rules.Services();
        }

        //分割线/////////系统功能树的搜索//////////////////////////////////////////
        public IList<TreeJSONBase> Query(string product,  bool isusbuser, string usertype, Int64 orgID, Int64 userID, string condition,bool rightFlag, string treeFilter)
        {
            string sql = string.Empty;
            string filter = string.Empty;
            filter = "(pid is null or pid='')";
            DataTable dt = this.QueryData(product,isusbuser, usertype, orgID, userID, TreeDataLevelType.LazyLevel ,condition, rightFlag, treeFilter);
            RemoveNode(dt);
            return new MenuTreeBuilder().GetExtTreeList(dt, "pid", "id", filter, TreeDataLevelType.TopLevel);
        }
        public DataTable QueryData(string product, bool isusbuser, string usertype, Int64 orgID, Int64 userID, TreeDataLevelType level, string condition, bool rightFlag, string treeFilter)
        {
            DataTable querydt;
            DataTable alldt;
            if (treeFilter != "" && treeFilter != null)//带过滤条件树的搜索
            {
                DataTable onlyLeafDt = dac.FilterTable(treeFilter);//已经被过滤的叶子节点
                DataTable sysAllDt = dac.LoadTable(product, usertype);//系统功能完整树
                DataTable allDt = FindRoot(onlyLeafDt, sysAllDt, treeFilter);//已经被过滤的完整树
                DataRow[] rows = allDt.Select(string.Format("name like'%{0}%'", condition));//从被过滤的完整树中，按搜索条件搜出的节点
                querydt = allDt.Clone();
                foreach(DataRow row in rows)
                {
                    querydt.Rows.Add(row.ItemArray); //已经被过滤的完整树，按搜索条件，取出的节点
                }
                //fg3.Select(string.Format("pid='{0}'", id)); name like '%"+condition+"%' "
                alldt = FindRoot(onlyLeafDt, allDt, treeFilter);//已经被过滤，已执行搜索条件的节点，找父节点构成完整树
            }
            else
            {
                querydt = dac.Query(product, condition, usertype, treeFilter);
                alldt = dac.LoadTable(product, usertype);
            }
            DataTable finaldt = FindRoot(querydt, alldt,treeFilter);//普通搜索结果会展开，这里传treeFilter禁止展开
            finaldt.DefaultView.Sort = "seq";
            finaldt = RightkeyController(finaldt);
            this.DealLang(finaldt);
            if (!rightFlag)
            {
                return finaldt;
            }
            //return finaldt;
            if (String.Compare(usertype, UserType.System, true) == 0)
            {
                return SystemUserRightFilreate(finaldt);
            }
            DataTable dt = RigthFiltrateExt(usertype, finaldt, TreeDataLevelType.TopLevel);
            
            return dt;
        }
        public DataTable FindRoot(DataTable querydt, DataTable alldt, string treeFilter)
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
                AddFather(newTable, alldt, pid,treeFilter, suite);

                string id = dr["id"].ToString();
                AddChild(newTable, alldt, id,suite);

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
        private void AddFather(DataTable newTable, DataTable alldt, string pid, string treeFilter, string suite)
        {
            while (!string.IsNullOrEmpty(pid))
            {
                
                DataRow[] fatherRow = alldt.Select(string.Format("id='{0}' and suite='{1}'", pid, suite));
                if (fatherRow.Length == 0)
                    break;
                    if (!IsExist(newTable, fatherRow[0]["id"].ToString()))
                     {
                        DataRow newRow = newTable.NewRow();
                        CopyRows(newRow, fatherRow[0]);
                        if (treeFilter != null && treeFilter != "")
                        {
                            newRow["expanded"] = "0";
                        }
                        else
                        {
                            newRow["expanded"] = "1";//父节点展开
                        }                        
                        newTable.Rows.Add(newRow);
                     }
                pid = fatherRow[0]["pid"].ToString();
                              
            }
            
        }
        private void AddChild(DataTable newTable, DataTable alldt, string id, string suite)
        {
            DataRow[] childrows = alldt.Select(string.Format("pid='{0}' and suite='{1}'", id,suite));
            int i = 0;
            int length = childrows.Length;
            while (length > 0)
            {
                if (!IsExist(newTable, childrows[i]["id"].ToString()))
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
        private bool IsExist(DataTable querydt, string id)
        {
            DataRow[] dr = querydt.Select(string.Format("id='{0}'", id));
            if(dr.Length == 0)
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

        public IList<TreeJSONBase> LoadMenu(string product, string suite, bool isusbuser, string usertype, Int64 orgID, Int64 userID, string nodeid,bool rightFlag,bool lazyLoadFlag, string treeFilter)
        {
            //新增参数rightFlag，false不加权限过滤，true加权限过滤
            string sql = string.Empty;
            string filter = string.Empty;
            //if(treeFilter != null && treeFilter != "")
            //{
            //    DataTable dt = this.GetMainTreeData(product, suite, isusbuser, usertype, orgID, userID, nodeid, TreeDataLevelType.TopLevel, rightFlag, treeFilter);
            //    filter = "(pid is null or pid='')";
            //    return new MenuTreeBuilder().GetExtTreeList(dt, "pid", "id", filter, TreeDataLevelType.TopLevel);
            //}
                        
            if (lazyLoadFlag)
            {
                if ("root" == nodeid)//首次加载
                {
                    DataTable dt = this.GetMainTreeData(product, suite, isusbuser, usertype, orgID, userID, nodeid, TreeDataLevelType.TopLevel, rightFlag, treeFilter);
                    filter = "(pid is null or pid='')";
                    return new MenuTreeBuilder().GetExtTreeList(dt, "pid", "id", filter, TreeDataLevelType.TopLevel,3);//加载两层
                }
                else
                {
                    DataTable dt = this.GetMainTreeData(product, suite, isusbuser, usertype, orgID, userID, nodeid, TreeDataLevelType.LazyLevel, rightFlag, treeFilter);
                    return new MenuTreeBuilder().GetExtTreeList(dt, "pid", "id", filter, TreeDataLevelType.LazyLevel);
                }
            }
            else
            {
                if ("root" == nodeid)//首次加载
                {
                    DataTable dt = this.GetMainTreeData(product, suite, isusbuser, usertype, orgID, userID, nodeid, TreeDataLevelType.TopLevel, rightFlag, treeFilter);
                    filter = "(pid is null or pid='')";
                    return new MenuTreeBuilder().GetExtTreeList(dt, "pid", "id", filter, TreeDataLevelType.TopLevel);
                }
                else
                {
                    DataTable dt = this.GetMainTreeData(product, suite, isusbuser, usertype, orgID, userID, nodeid, TreeDataLevelType.LazyLevel, rightFlag, treeFilter);
                    return new MenuTreeBuilder().GetExtTreeList(dt, "pid", "id", filter, TreeDataLevelType.LazyLevel);
                }
            }      
            
            //}            
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
        /// <param name="rightFlag">是否控制权限的标记位</param>
        ///  <param name="treeFilter">系统功能树过滤条件</param>
        /// <returns></returns>
        public DataTable GetMainTreeData(string product, string suite, bool isusbuser, string usertype, Int64 orgID, Int64 userID, string nodeid,TreeDataLevelType level, bool rightFlag, string treeFilter)
        {
            try
            {
                string logId = LogHelper<MainTreeRule>.StartMethodLog("LoadMenuData");
                DataTable menudt = new DataTable();
                if (treeFilter != "" && treeFilter != null)
                {                   
                    DataTable onlyLeafDt = dac.FilterTable(product, suite, nodeid,treeFilter);
                    DataTable allDt = dac.LoadTable(product, usertype);
                    menudt = FindRoot(onlyLeafDt, allDt, treeFilter);
                }
                else
                {
                    menudt = dac.LoadMenuData(product, suite, nodeid).Copy();
                }
                LogHelper<MainTreeRule>.EndMethodLog(logId, 0);             
                menudt = RightkeyController(menudt);                
                this.DealLang(menudt);
                if (!rightFlag)
                {
                    return menudt;
                }

                //系统管理员的一些特殊处理
                if (String.Compare(usertype, UserType.System, true) == 0)
                {
                    return SystemUserRightFilreate(menudt);
                }
                //DataTable dt = menudt;
                string logIdRight = LogHelper<MainTreeRule>.StartMethodLog("LoadMenuRightService");
                DataTable dt = RigthFiltrateExt(usertype, menudt, level);
                LogHelper<MainTreeRule>.EndMethodLog(logIdRight, 0);
                return dt;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.StackTrace);
                throw ex;
            }

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
                string apptype = dr["apptype"].ToString().ToLower();
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
                  
                    if (rightkey == -1 || rightkey == 0)
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
                    if (rightkey == -1|| rightkey == 0)
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
            if(level == TreeDataLevelType.TopLevel)
            {
                this.RemoveNode(menudt);
            }
            menudt.AcceptChanges();

            //DataRow[] dr = dt.Select("TabPageID = 'TabPageContractManage'");
            //DataRow[] dddd = menudt.Select("code = 990100120303");
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




        /// <summary>
        /// 系统管理员权限过滤
        /// </summary>
        /// <param name="menudt"></param>
        /// <returns></returns>
        public DataTable SystemUserRightFilreate(DataTable menudt)
        {
            DataRow[] drs = menudt.Select("adminvisiable <> '1'");
            if (drs.Length != 0)
            {
                foreach (DataRow dr in drs)
                {
                    menudt.Rows.Remove(dr);
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
              
                if (dr["langkey"] != null && dr["langkey"] != DBNull.Value )
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
        private void IsValidPath(string id, DataTable menudt,ref bool hit)
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
             
        
        public IList<SuiteInfoEntity> GetSuiteList(bool rightFlag, string treeFilter)//
        {
            try
            {
                string logId = LogHelper<MainTreeRule>.StartMethodLog("getSuiteRigthService");

                List<SuiteInfoEntity> list = new List<SuiteInfoEntity>();               
                //取套件权限
                //DataTable suitrightdt = rigthService.GetSuitRight(NG3.AppInfoBase.OCode, NG3.AppInfoBase.LoginID);
                DataTable dtInCache = rigthService.ReadProductInfo().Tables["SuitInfo"];
                //把缓存里的套件表拷贝出来进行操作，不能直接操作缓存里表的引用，否则会修改了缓存里的表
                DataTable dt = dtInCache.Copy();

                //取套件权限
                //i6.Biz.DMC.CPCM_Interface cpcmInterface = new i6.Biz.DMC.CPCM_Interface(NG3.AppInfoBase.UserConnectString);
                Enterprise3.Rights.AnalyticEngine.FuncRightControl cpcmInterface = new FuncRightControl();
                DataTable suitrightdt = new DataTable();
                for (int i = 0; i < 3; i++)
                {  
                    suitrightdt = cpcmInterface.GetSuitRight(NG3.AppInfoBase.OrgID, NG3.AppInfoBase.UserID);
                    if(suitrightdt==null || suitrightdt.Rows.Count == 0)
                    {
                        System.Threading.Thread.Sleep(500);                       
                    }
                    else
                    {
                        break;
                    }
                }
                LogHelper<MainTreeRule>.EndMethodLog(logId,0);
                if (treeFilter != "" && treeFilter != null)
                {
                    DataTable filterDt = dac.FilterTable(treeFilter);
                    for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow dr = dt.Rows[i];                    
                        DataRow[] drs = filterDt.Select(string.Format("suite='{0}'", dr["Code"]));
                        if (drs.Length == 0)
                        {
                            dr.Delete();
                        }                           
                    }
                }
                dt.AcceptChanges();

                //获取所有套件
                //string currentCultureName = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
                //DataTable dt = MainFrameUIP.GetSuitList(i6Culture.GetProductXmlFilePath(MainCommonUIP.GetPath(), currentCultureName)).Tables[1];

                if (String.Compare(NG3.AppInfoBase.UserType, "SYSTEM", true) == 0 && rightFlag)
                {
                    dt.DefaultView.RowFilter = "Code = 'DMC'";
                    dt = dt.DefaultView.ToTable();
                }

                foreach (DataRow dr in dt.Rows)
                {
                    string module = dr["Code"].ToString();

                    //普通用户和非DMC模块要判断套件权限
                    if ((NG3.AppInfoBase.UserType == SUP.Common.Base.UserType.OrgUser && "PUB" != module.ToUpper()))
                    {
                        DataRow[] drs = suitrightdt.Select(string.Format("suit='{0}' and ishas=true", module));
                        if (drs.Length == 0 && rightFlag)
                        {
                            continue;//无权限
                        }
                    }

                    SuiteInfoEntity entity = new SuiteInfoEntity();
                    entity.Code = dr["Code"].ToString();
                    entity.Name = dr["Name"].ToString();
                    list.Add(entity);

                }

                return list;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.StackTrace);
                return null;
            }
        }
        
        //根据busphid取fg3-menu数据的接口
        public DataTable GetMenuByBusphid(long busphid)
        {
            return dac.GetMenuByBusphid(busphid);
        }
    }


    class MenuTreeJSONBase : TreeJSONBase
    {
        public virtual string url { get; set; }
        public virtual string rightname { get; set; }
        public virtual string functionname { get; set; }
        public virtual string code { get; set; }
        public virtual string rightkey { get; set; }
        public virtual string managername { get; set; }
        public virtual string moduleno { get; set; }
        public virtual string suite { get; set; }
        //public virtual bool allowDrag { get; set; }
        public virtual string busphid { get; set; }
    }

    public class MenuTreeBuilder : ExtJsTreeBuilderBase
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

            MenuTreeJSONBase node = new MenuTreeJSONBase();

            //string functionname = dr["functionname"].ToString();

            node.id = dr["id"].ToString();
            node.text = dr["name"].ToString();            
            node.rightname = dr["rightname"].ToString();
            node.rightkey = dr["rightkey"].ToString();
            node.managername = dr["managername"].ToString();
            node.suite = dr["suite"].ToString();
            node.moduleno = dr["moduleno"].ToString();
            node.functionname = dr["functionname"].ToString();
            node.code = dr["code"].ToString();
            //node.leaf = (dr["functionnode_flag"].ToString() == "1") ? true : false;//有些没有url的虚拟节点，也设置成1,变成叶子节点，下面的节点就无法懒加载出来
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
            //int busphid = 0;
            //int.TryParse(dr["busphid"].ToString(), out busphid);
            //node.busphid = busphid;
            node.busphid = dr["busphid"].ToString();
            return node;
        }
    }
     

}
