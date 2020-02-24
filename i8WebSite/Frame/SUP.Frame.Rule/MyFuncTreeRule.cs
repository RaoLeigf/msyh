using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SUP.Frame.DataAccess;
using SUP.Common.Base;
using SUP.Common.Rule;
using SUP.Frame.DataEntity;
using SUP.Common.Interface;

namespace SUP.Frame.Rule
{

    public class MyFuncTreeRule : IUserConfig
    {
        private MyFuncTreeDac dac = null;
        private bool show = false;
        SUP.Right.Rules.Services rigthService;


        public MyFuncTreeRule()
        {
            dac = new MyFuncTreeDac();
            rigthService = new Right.Rules.Services();
        }
        public IList<TreeJSONBase> LoadMyFuncTree(long userid,string nodeid)
        {
            string sql = string.Empty;
            string filter = string.Empty;


            if ("root" == nodeid)//首次加载
            {
                DataTable dt = this.GetMainTreeData(userid);
                filter = "(pid='root')";
                return new MyFuncTreeBuilder().GetExtTreeList(dt, "pid", "id", filter, TreeDataLevelType.TopLevel);//加载两层
            }
            else
            {               
                List<TreeJSONBase> l = new List<TreeJSONBase>();
                return l;
            }
        }

      
        public DataTable GetMainTreeData(long userid)
        {
            DataTable menudt = dac.LoadMenuData(userid).Copy();
            return RightkeyController(menudt);
        }

        public int Save(DataTable myFuncTreeTable, long userid)
        {

            List<long> phid = null;
            int count = myFuncTreeTable.Rows.Count;
            if (count > 0)
            {
                phid = SUP.Common.Rule.CommonUtil.GetBillId("fg3_myfunctree", "phid", count);
            }          
            return dac.Save(myFuncTreeTable, userid, phid);
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
        public bool CopyUserConfig(long fromUserId, int fromUserType, long toUserId, int toUserType)
        {
            return dac.UserConfigCopy(fromUserId,fromUserType,toUserId,toUserType);
        }

        public bool DeleteUserConfig(long userid, int usertype)
        {
            return dac.UserConfigDel(userid, usertype);
        }
    }

    class MyFuncTreeJSONBase : TreeJSONBase
    {
        public virtual string url { get; set; }
        public virtual string pid { get; set; }
        public virtual string name { get; set; }
        public virtual string originalcode { get; set; }
        public virtual long phid { get; set; }
        public virtual long userid { get; set; }

        public virtual string rightname { get; set; }
        public virtual string managername { get; set; }
        public virtual string moduleno { get; set; }
        public virtual string suite { get; set; }
        public virtual string rightkey { get; set; }
        public virtual string urlparm { get; set; }
        public virtual int busphid { get; set; }
    }

    public class MyFuncTreeBuilder : ExtJsTreeBuilderBase
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

            MyFuncTreeJSONBase node = new MyFuncTreeJSONBase();

            string functionname = dr["functionname"].ToString();

            node.id = dr["id"].ToString();
            node.text = dr["name"].ToString();
            node.phid = Int64.Parse(dr["phid"].ToString());
            node.pid = dr["pid"].ToString();
            node.name = dr["name"].ToString();
            node.originalcode = dr["originalcode"].ToString();
            node.userid = Int64.Parse(dr["userid"].ToString());
            node.leaf = !String.IsNullOrEmpty(dr["url"].ToString());
            node.url = dr["url"].ToString();
            node.rightname = dr["rightname"].ToString();
            node.managername = dr["managername"].ToString();
            node.moduleno = dr["moduleno"].ToString();
            node.suite = dr["suite"].ToString();
            node.rightkey = dr["rightkey"].ToString();
            node.urlparm = dr["urlparm"].ToString();
            int busphid = 0;
            int.TryParse(dr["busphid"].ToString(), out busphid);
            node.busphid = busphid;
            return node;
        }
    }
}
