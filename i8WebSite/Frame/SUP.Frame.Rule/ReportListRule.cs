using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SUP.Frame.DataAccess;
using SUP.Common.Base;
using SUP.Frame.DataEntity;

namespace SUP.Frame.Rule
{
    public class ReportListRule
    {
        private ReportListDac dac = null;

        public ReportListRule()
        {
            dac = new ReportListDac();
        }

        public IList<TreeJSONBase> LoadReportList(string userid, long orgid,string page="")
        {
            string sql = string.Empty;
            string filter = string.Empty;
            
            DataTable dt = this.GetMainTreeData(userid, orgid, page);

            this.RemoveNode(dt);
            dt.AcceptChanges();

            filter = "(pid=0)";
            return new ReportListBuilder().GetExtTreeList(dt, "pid", "id", filter, TreeDataLevelType.TopLevel);//加载两层

        }
        //清除没有子节点的非叶子节点
        private void RemoveNode(DataTable menudt)
        {
            //Dictionary<string, string> langDic = SUP.Frame.DataAccess.LangInfo.GetLabelLang("SystemMenu");

            //处理掉父节点的seq小于子节点seq的那些行
            for (int i = menudt.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = menudt.Rows[i];
                string id = dr["phid"].ToString();

                if (dr["isleaf"].ToString() == "0")//非功能节点
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

                string id = dr["phid"].ToString();
                if (dr["isleaf"].ToString() == "0")//非功能节点
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
                    string flg = dr["isleaf"].ToString();
                    if (flg == "1")
                    {
                        hit = true;
                        return;
                    }

                    if (!hit)
                    {
                        string tempid = dr["phid"].ToString();
                        IsValidPath(tempid, menudt, ref hit);
                    }
                    else
                    {
                        break;//已经检测到有效，跳出循环
                    }

                }
            }
        }

        public DataTable GetMainTreeData(string userid,long orgid,string page="")
        {
            DataTable menudt = dac.LoadReportList(userid, orgid, page);
            StandardPidAndId(menudt);
            return menudt;
        }

        class ReportListJSONBase : TreeJSONBase
        {
            public virtual string url { get; set; }
            public virtual string pid { get; set; }
            public virtual string name { get; set; }
            public virtual string code { get; set; }
            public virtual string phid { get; set; }
            //public virtual string treeid { get; set; }
            //public virtual string treepid { get; set; }
            //public virtual long userid { get; set; }
            //public virtual string rightname { get; set; }
            //public virtual string managername { get; set; }
            //public virtual string moduleno { get; set; }
            //public virtual string suite { get; set; }
            //public virtual string rightkey { get; set; }
            //public virtual string urlparm { get; set; }
        }

        public class ReportListBuilder : ExtJsTreeBuilderBase
        {
            public override TreeJSONBase BuildTreeNode(DataRow dr)
            {

                ReportListJSONBase node = new ReportListJSONBase();               
                
                node.text = dr["rep_name"].ToString();
                node.phid = dr["phid"].ToString();                
                node.name = dr["rep_name"].ToString();      
                node.leaf = (dr["isleaf"].ToString()=="1");
                node.code = dr["rep_code"].ToString();
                node.id = dr["phid"].ToString();
                node.pid = dr["pid"].ToString();               

                if (dr["isleaf"].ToString() == "1")
                {
                    string phid = node.phid.Substring(0, node.phid.Length - 1);
                    string page = dr["page"]==DBNull.Value?"":dr["page"].ToString();
                    node.url = "RW/DesignFrame/ReportView?rep_src=0&rep_id=" + phid + "&rep_code=" + node.code+"&page="+page;
                }
                
                return node;
            }
        }
        /// <summary>
        /// 手动构建上下级结构,pid和id没有业务关系，舍弃原先的重新构建,顺便给文件节点code赋值
        /// </summary>
        /// <param name="dt"></param>
        public void StandardPidAndId(DataTable dt)
        {
            DataRow[] tempdr = dt.Select("1=1");
            foreach (DataRow dr in tempdr)
            {
                if (dr["isleaf"].ToString() == "0")
                {
                    //node.treeid = dr["phid"].ToString() + "0";
                    dr["phid"] = dr["phid"].ToString() + dr["isleaf"].ToString();
                    dr["rep_code"] = "ReportListRoot";
                }
                else
                {
                    dr["phid"] = dr["phid"].ToString() + dr["isleaf"].ToString();
                }

                dr["pid"] = dr["pid"].ToString() + "0";

            }
        }
    }
}
